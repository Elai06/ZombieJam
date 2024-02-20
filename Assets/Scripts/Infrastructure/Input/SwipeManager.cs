using System;
using Gameplay.Enums;
using Gameplay.Tutorial;
using Gameplay.Units;
using Gameplay.Windows.Gameplay;
using UnityEngine;
using Zenject;

namespace Infrastructure.Input
{
    public class SwipeManager : MonoBehaviour
    {
        public event Action<TutorialSwipeInfo> OnSwipe;

        private IGameplayModel _gameplayModel;

        private Vector3 _startPosition;
        private RaycastDetector _raycastDetector;

        private UnitSwipe _swipeObject;
        private TutorialSwipeInfo _tutorialSwipeInfo;

        private bool _isWasSwipe;

        public bool IsSwipeTutorialCompleted { get; set; }

        [Inject]
        public void Construct(IGameplayModel gameplayModel)
        {
            _gameplayModel = gameplayModel;
        }

        public void Initialize()
        {
            _raycastDetector = new RaycastDetector();
            IsSwipeTutorialCompleted = _gameplayModel.TutorialState != ETutorialState.Swipe;
        }

        private void Update()
        {
            if (!_gameplayModel.IsStartWave) return;

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                _startPosition = UnityEngine.Input.mousePosition;
                SetSwipedObject();
            }

            if (UnityEngine.Input.GetMouseButton(0))
            {
                DetectSwipe();
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                _swipeObject = null;
                _isWasSwipe = false;
                _tutorialSwipeInfo.Reset();
            }
        }

        private void DetectSwipe()
        {
            if (_swipeObject != null && !_isWasSwipe)
            {
                var swipe = DefineSwipe();

                if (swipe != ESwipeSide.None)
                {
                    _isWasSwipe = true;

                    if (_gameplayModel.TutorialState == ETutorialState.Swipe && !IsSwipeTutorialCompleted)
                    {
                        _tutorialSwipeInfo.SwipeSide = swipe;
                        OnSwipe?.Invoke(_tutorialSwipeInfo);
                        return;
                    }

                    _swipeObject.Swipe(swipe);
                    OnSwipe?.Invoke(_tutorialSwipeInfo);
                }
            }
        }

        private void SetSwipedObject()
        {
            var contactInfo = _raycastDetector.RayCast(10);
            if (contactInfo.Collider == null) return;

            _swipeObject = contactInfo.Collider.GetComponent<UnitSwipe>();
            _tutorialSwipeInfo.SwipeGameObject = contactInfo.Collider.gameObject;
            _tutorialSwipeInfo.SwipeDirection = _swipeObject.SwipeDirection;
            _tutorialSwipeInfo.UnitSwipe = _swipeObject;
        }

        private ESwipeSide DefineSwipe()
        {
            var currentPosition = UnityEngine.Input.mousePosition;
            if (_startPosition != currentPosition)
            {
                var deltaPosition = _startPosition - currentPosition;
                var deltaHorizontalSwipe = Math.Abs(_startPosition.x - currentPosition.x);
                var deltaVerticalSwipe = Math.Abs(_startPosition.y - currentPosition.y);

                if (deltaPosition.x != 0 && deltaHorizontalSwipe > deltaVerticalSwipe)
                {
                    var swipeSide = deltaPosition.x > 0 ? ESwipeSide.Left : ESwipeSide.Right;
                    return swipeSide;
                }

                if (deltaPosition.y != 0)
                {
                    var swipeSide = deltaPosition.y > 0 ? ESwipeSide.Back : ESwipeSide.Forward;
                    return swipeSide;
                }
            }


            return ESwipeSide.None;
        }
    }
}
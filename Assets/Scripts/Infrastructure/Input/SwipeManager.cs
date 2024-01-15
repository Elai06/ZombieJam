using System;
using Gameplay.Enums;
using Gameplay.Tutorial;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Infrastructure.Input
{
    public class SwipeManager : MonoBehaviour
    {
        public event Action<TutorialSwipeInfo> OnSwipe;

        private ITutorialService _tutorialService;

        private Vector3 _startPosition;
        private RaycastDetector _raycastDetector;

        private ISwipeObject _swipeObject;
        private TutorialSwipeInfo _tutorialSwipeInfo;

        private bool _isWasSwipe;

        [Inject]
        public void Construct(ITutorialService tutorialService)
        {
            _tutorialService = tutorialService;
        }

        public void Initialize()
        {
            _raycastDetector = new RaycastDetector();
        }

        private void Update()
        {
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

                    if (_tutorialService.CurrentState == ETutorialState.Swipe)
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
            var contactInfo = _raycastDetector.RayCast(3);
            if (contactInfo.Collider == null) return;

            _swipeObject = contactInfo.Collider.GetComponent<ISwipeObject>();
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
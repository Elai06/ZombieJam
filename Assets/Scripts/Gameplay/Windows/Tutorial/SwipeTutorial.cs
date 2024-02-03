using DG.Tweening;
using Gameplay.Enums;
using Gameplay.Tutorial;
using Gameplay.Tutorial.States.SwipeState;
using Gameplay.Windows.Gameplay;
using Infrastructure.Input;
using Infrastructure.Windows;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Tutorial
{
    // ПРОСТИТЕ МЕНЯ, ЕСЛИ КТО УВИДИТ ЭТО
    public class SwipeTutorial : MonoBehaviour
    {
        [SerializeField] private ArrowTutorial _firstArrow;
        [SerializeField] private ArrowTutorial _secondArrow;

        [Inject] private SwipeManager _swipeManager;

        private readonly Vector3 _firstUnitPosition = new(-1.5f, 0.5f, 0);
        private readonly Vector3 _secondUnitPosition = new(1f, 0.5f, 0.5f);

        private Tween _tween;

        private void Start()
        {
            InjectService.Instance.Inject(this);
            _firstArrow.gameObject.SetActive(true);

            StartAnimation();
            _swipeManager.OnSwipe += OnSwipe;
            _secondArrow.gameObject.SetActive(false);
        }

        private void StartAnimation()
        {
            _tween = _firstArrow.transform.DOLocalMoveZ(_firstArrow.transform.position.z + 1.75f, 1.75f)
                .SetLoops(-1, LoopType.Restart);
        }

        private void OnSwipe(TutorialSwipeInfo swipeObject)
        {
            if (swipeObject.SwipeDirection == ESwipeDirection.Vertical)
            {
                VerticalSwiped(swipeObject);
            }

            if (swipeObject.SwipeDirection == ESwipeDirection.Horizontal)
            {
                HorizontalSwipe(swipeObject);
            }
        }

        private void VerticalSwiped(TutorialSwipeInfo swipeObject)
        {
            if (swipeObject.SwipeDirection == ESwipeDirection.Vertical)
            {
                if (swipeObject.SwipeSide == ESwipeSide.Forward || swipeObject.SwipeSide == ESwipeSide.Back)
                {
                    if (swipeObject.SwipeGameObject.transform.position == _firstUnitPosition)
                    {
                        _tween?.Kill();
                        swipeObject.UnitSwipe.Swipe(swipeObject.SwipeSide);

                        if (_firstArrow == null)
                        {
                            _firstArrow = FindFirstObjectByType<ArrowTutorial>();
                            Destroy(_firstArrow.gameObject);
                        }

                        if (_secondArrow == null)
                        {
                            _secondArrow = FindFirstObjectByType<ArrowTutorial>();
                        }

                        _secondArrow.gameObject.SetActive(true);
                        _tween = _secondArrow.transform.DOLocalMoveX(_secondArrow.transform.position.x - 1.75f, 1.75f)
                            .SetLoops(-1, LoopType.Restart);
                    }
                }
            }
        }

        private void HorizontalSwipe(TutorialSwipeInfo swipeObject)
        {
            if (swipeObject.SwipeDirection == ESwipeDirection.Horizontal)
            {
                if (swipeObject.SwipeSide == ESwipeSide.Left || swipeObject.SwipeSide == ESwipeSide.Right)
                {
                    if (swipeObject.SwipeGameObject.transform.position == _secondUnitPosition)
                    {
                        _tween?.Kill();

                        if (_secondArrow == null)
                        {
                            _secondArrow = FindFirstObjectByType<ArrowTutorial>();
                        }

                        swipeObject.UnitSwipe.Swipe(swipeObject.SwipeSide);
                        Destroy(_secondArrow.gameObject);
                        _swipeManager.IsSwipeTutorialCompleted = true;
                    }
                }
            }
        }
    }
}
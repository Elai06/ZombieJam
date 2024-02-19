using System.Threading.Tasks;
using DG.Tweening;
using Gameplay.Enums;
using Gameplay.Parking;
using Gameplay.Tutorial.States.SwipeState;
using Infrastructure.Input;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Tutorial
{
    // ПРОСТИТЕ МЕНЯ, ЕСЛИ КТО УВИДИТ ЭТО
    public class SwipeTutorial : MonoBehaviour
    {
        [SerializeField] private ArrowTutorial _verticalArrow;
        [SerializeField] private ArrowTutorial _horizontalArrow;

        [SerializeField] private GameObject _horizontalFieldIllumination;
        [SerializeField] private GameObject _verticalFieldIllumination;

        [SerializeField] private GameObject _tutorialContent;

        [SerializeField] private ZombieSpawner _zombieSpawner;

        [Inject] private SwipeManager _swipeManager;

        private readonly Vector3 _firstUnitPosition = new(1.5f, 0.5f, 0.5f);
        private readonly Vector3 _secondUnitPosition = new(-1.5f, 0.5f, -.5f);

        private bool _isFirstSwipeCompleted;

        private Tween _tween;

        private int _unitIndex;

        private void Start()
        {
            InjectService.Instance.Inject(this);
            StartAnimation();

            _swipeManager.OnSwipe += OnSwipe;
            
            foreach (var zombie in _zombieSpawner.Zombies)
            {
                zombie.OnInitializePath += OnInitializePathfinder;
            }
        }

        private void OnEnable()
        {
            _horizontalArrow.gameObject.SetActive(false);
            _horizontalFieldIllumination.gameObject.SetActive(false);

            _verticalArrow.gameObject.SetActive(false);
            _verticalFieldIllumination.gameObject.SetActive(false);
            _tutorialContent.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            foreach (var zombie in _zombieSpawner.Zombies)
            {
                zombie.OnInitializePath -= OnInitializePathfinder;
            }
        }

        private void OnInitializePathfinder()
        {
            _tween?.Kill();
            _unitIndex++;

            if (_unitIndex == 1)
            {
                _verticalArrow.gameObject.SetActive(true);
                _verticalFieldIllumination.gameObject.SetActive(true);
                _tween = _verticalArrow.transform
                    .DOLocalMoveZ(_verticalArrow.transform.position.z + 1.75f, 1.75f)
                    .SetLoops(-1, LoopType.Restart);

                _isFirstSwipeCompleted = true;
            }
            else if (_unitIndex == 2)
            {
                _tutorialContent.SetActive(false);
                _swipeManager.IsSwipeTutorialCompleted = true;
            }
        }

        private async void StartAnimation()
        {
            await Task.Delay(1500);

            _tutorialContent.gameObject.SetActive(true);
            _horizontalArrow.gameObject.SetActive(true);
            _horizontalFieldIllumination.gameObject.SetActive(true);

            _tween = _horizontalArrow.transform.DOLocalMoveX(_horizontalArrow.transform.position.x - 1.75f, 1.75f)
                .SetLoops(-1, LoopType.Restart);
        }

        private void OnUnitRoadCompleted(int unitIndex)
        {
            _tween?.Kill();

            if (unitIndex == 1)
            {
                _verticalArrow.gameObject.SetActive(true);
                _verticalFieldIllumination.gameObject.SetActive(true);
                _tween = _verticalArrow.transform
                    .DOLocalMoveZ(_verticalArrow.transform.position.z + 1.75f, 1.75f)
                    .SetLoops(-1, LoopType.Restart);

                _isFirstSwipeCompleted = true;
            }
            else if (unitIndex == 2)
            {
                _tutorialContent.SetActive(false);
                _swipeManager.IsSwipeTutorialCompleted = true;
            }
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
            if (!_isFirstSwipeCompleted) return;
            if (swipeObject.SwipeDirection == ESwipeDirection.Vertical)
            {
                if (swipeObject.SwipeSide == ESwipeSide.Forward || swipeObject.SwipeSide == ESwipeSide.Back)
                {
                    if (swipeObject.SwipeGameObject.transform.position == _secondUnitPosition)
                    {
                        swipeObject.UnitSwipe.Swipe(swipeObject.SwipeSide);

                        _verticalArrow.gameObject.SetActive(false);
                        _verticalFieldIllumination.SetActive(false);
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
                    if (swipeObject.SwipeGameObject.transform.position == _firstUnitPosition)
                    {
                        swipeObject.UnitSwipe.Swipe(swipeObject.SwipeSide);

                        _horizontalArrow.gameObject.SetActive(false);
                        _horizontalFieldIllumination.SetActive(false);
                    }
                }
            }
        }
    }
}
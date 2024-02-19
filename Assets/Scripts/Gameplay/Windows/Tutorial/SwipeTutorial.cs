using System.Threading.Tasks;
using DG.Tweening;
using Gameplay.Enums;
using Gameplay.Parking;
using Gameplay.Tutorial.States.SwipeState;
using Infrastructure.Input;
using UnityEngine;
using UnityEngine.Serialization;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Tutorial
{
    // ПРОСТИТЕ МЕНЯ, ЕСЛИ КТО УВИДИТ ЭТО
    public class SwipeTutorial : MonoBehaviour
    {
        [SerializeField] private ArrowTutorial _secondArrow;
        [SerializeField] private ArrowTutorial _firstArrow;
        [SerializeField] private ArrowTutorial _thirdArrow;
        [SerializeField] private ArrowTutorial _fourtArrow;

        [SerializeField] private GameObject _firstFieldIllumination;
        [SerializeField] private GameObject _secondFieldIllumination;
        [SerializeField] private GameObject _tutorialBG;

        [SerializeField] private ZombieSpawner _zombieSpawner;

        [Inject] private SwipeManager _swipeManager;

        private readonly Vector3 _firstUnitPosition = new(1.5f, 0.5f, 0.5f);
        private readonly Vector3 _secondUnitPosition = new(-1.5f, 0.5f, -.5f);
        private readonly Vector3 _thirdUnitPosition = new(0.5f, 0.5f, -1.5f);
        private readonly Vector3 _fourthUnitPosition = new(2.5f, 0.5f, -1.5f);

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
            _firstFieldIllumination.gameObject.SetActive(false);
            _secondFieldIllumination.gameObject.SetActive(false);

            _firstArrow.gameObject.SetActive(false);
            _secondArrow.gameObject.SetActive(false);
            _thirdArrow.gameObject.SetActive(false);
            _fourtArrow.gameObject.SetActive(false);

            _tutorialBG.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            foreach (var zombie in _zombieSpawner.Zombies)
            {
                zombie.OnInitializePath -= OnInitializePathfinder;
            }
        }

        private async void StartAnimation()
        {
            await Task.Delay(2000);

            _tutorialBG.gameObject.SetActive(true);
            _firstArrow.gameObject.SetActive(true);
            _firstFieldIllumination.gameObject.SetActive(true);
            _tween = _firstArrow.transform.DOLocalMoveX(_firstArrow.transform.position.x - 1.75f, 1.75f)
                .SetLoops(-1, LoopType.Restart);
        }

        private void OnInitializePathfinder()
        {
            _tween?.Kill();
            _unitIndex++;

            if (_unitIndex == 1)
            {
                _firstFieldIllumination.gameObject.SetActive(false);
                _secondArrow.gameObject.SetActive(true);
                _secondFieldIllumination.gameObject.SetActive(true);
                _tween = _secondArrow.transform
                    .DOLocalMoveZ(_secondArrow.transform.position.z + 1.75f, 1.75f)
                    .SetLoops(-1, LoopType.Restart);
            }
            else if (_unitIndex == 2)
            {
                _secondFieldIllumination.gameObject.SetActive(false);
                _thirdArrow.gameObject.SetActive(true);
                _tutorialBG.gameObject.SetActive(false);
                _tween = _thirdArrow.transform.DOLocalMoveZ(_thirdArrow.transform.position.z + 1.75f, 1.75f)
                    .SetLoops(-1, LoopType.Restart);
            }
            else if (_unitIndex == 3)
            {
                _fourtArrow.gameObject.SetActive(true);
                _tween = _fourtArrow.transform
                    .DOLocalMoveX(_fourtArrow.transform.position.x - 1.75f, 1.75f)
                    .SetLoops(-1, LoopType.Restart);
            }
            else if (_unitIndex == 4)
            {
                _swipeManager.IsSwipeTutorialCompleted = true;
            }
        }

        private void OnSwipe(TutorialSwipeInfo swipeObject)
        {
            var unitPosition = GetUnitPosition(_unitIndex);

            if (swipeObject.SwipeDirection == ESwipeDirection.Vertical)
            {
                VerticalSwiped(swipeObject, unitPosition);
            }
            else if (swipeObject.SwipeDirection == ESwipeDirection.Horizontal)
            {
                HorizontalSwipe(swipeObject, unitPosition);
            }
        }

        private void VerticalSwiped(TutorialSwipeInfo swipeObject, Vector3 unitPosition)
        {
            if (swipeObject.SwipeDirection == ESwipeDirection.Vertical)
            {
                if (swipeObject.SwipeSide == ESwipeSide.Forward || swipeObject.SwipeSide == ESwipeSide.Back)
                {
                    if (swipeObject.SwipeGameObject.transform.position == unitPosition)
                    {
                        swipeObject.UnitSwipe.Swipe(swipeObject.SwipeSide);

                        HideArrow();
                    }
                }
            }
        }

        private void HorizontalSwipe(TutorialSwipeInfo swipeObject, Vector3 unitPosition)
        {
            if (swipeObject.SwipeDirection == ESwipeDirection.Horizontal)
            {
                if (swipeObject.SwipeSide == ESwipeSide.Left || swipeObject.SwipeSide == ESwipeSide.Right)
                {
                    if (swipeObject.SwipeGameObject.transform.position == unitPosition)
                    {
                        swipeObject.UnitSwipe.Swipe(swipeObject.SwipeSide);

                        HideArrow();
                    }
                }
            }
        }

        private void HideArrow()
        {
            if (_unitIndex == 0)
            {
                _firstArrow.gameObject.SetActive(false);
            }
            else if (_unitIndex == 1)
            {
                _secondArrow.gameObject.SetActive(false);
            }
            else if (_unitIndex == 2)
            {
                _thirdArrow.gameObject.SetActive(false);
            }
            else if (_unitIndex == 3)
            {
                _fourtArrow.gameObject.SetActive(false);
            }
        }

        private Vector3 GetUnitPosition(int unitIndex)
        {
            var position = unitIndex switch
            {
                0 => _firstUnitPosition,
                1 => _secondUnitPosition,
                2 => _thirdUnitPosition,
                3 => _fourthUnitPosition,
                _ => Vector3.zero
            };

            return position;
        }
    }
}
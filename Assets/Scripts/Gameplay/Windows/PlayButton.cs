using System.Threading.Tasks;
using Gameplay.Ad;
using Gameplay.CinemachineCamera;
using Gameplay.Tutorial;
using Gameplay.Windows.Gameplay;
using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private Image _tutorialFinger;

        private Button _button;

        private CameraSelector _cameraSelector;
        private IWindowService _windowService;
        private IAdsService _adsService;
        private IGameplayModel _gameplayModel;
        private ITutorialService _tutorialService;

        [Inject]
        private void Construct(IWindowService windowService, IAdsService adsService, IGameplayModel gameplayModel,
            ITutorialService tutorialService)
        {
            _windowService = windowService;
            _adsService = adsService;
            _gameplayModel = gameplayModel;
            _tutorialService = tutorialService;
        }

        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();
        }

        private async void Start()
        {
            InjectService.Instance.Inject(this);

            if (_tutorialService.CurrentState == ETutorialState.Swipe)
            {
                await Task.Delay(10);
                _button.interactable = false;
                StartPlay();
            }
            
            OnChangedState(_tutorialService.CurrentState);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Play);
            _button.interactable = true;
            _tutorialService.СhangedState += OnChangedState;

            if (_tutorialService.CurrentState == ETutorialState.StartCard)
            {
                var waveIndex = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion().CurrentWaweIndex;
                _button.interactable = waveIndex < 2;
            }
        }

        private void OnDisable()
        {
            _tutorialService.СhangedState -= OnChangedState;

            _button.onClick.RemoveListener(Play);
        }

        private void Play()
        {
            if (_tutorialService.CurrentState != ETutorialState.PlayButton) return;

            if (_gameplayModel.GetCurrentRegionProgress().RegionIndex > 0)
            {
                if (_adsService.ShowAds(EAdsType.Interstitial))
                {
                    _adsService.Showed += OnShowedAds;
                    _adsService.OnSkipAds += OnShowedAds;
                    return;
                }
            }

            StartPlay();
        }

        private void OnShowedAds()
        {
            _adsService.Showed -= OnShowedAds;
            _adsService.OnSkipAds -= OnShowedAds;

            StartPlay();
        }

        private void StartPlay()
        {
            _cameraSelector = FindObjectOfType<CameraSelector>();
            _cameraSelector.ChangeCamera(ECameraType.Park);
            _windowService.Close(WindowType.Footer);
            _windowService.Open(WindowType.Gameplay);
            _gameplayModel.StartWave();

            _tutorialFinger.gameObject.SetActive(false);
        }

        private void OnChangedState(ETutorialState state)
        {
            if (state != ETutorialState.Completed && state != ETutorialState.PlayButton)
            {
                _button.interactable = false;
                return;
            }

            if (state == ETutorialState.PlayButton)
            {
                _tutorialFinger.gameObject.SetActive(true);
            }

            _button.interactable = true;
        }
    }
}
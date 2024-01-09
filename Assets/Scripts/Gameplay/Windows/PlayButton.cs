using Gameplay.Ad;
using Gameplay.CinemachineCamera;
using Gameplay.Configs.Region;
using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows
{
    public class PlayButton : MonoBehaviour
    {
        private Button _button;

        private CameraSelector _cameraSelector;
        private IWindowService _windowService;
        private IAdsService _adsService;
        private IRegionManager _regionManager;

        [Inject]
        private void Construct(IWindowService windowService, IAdsService adsService, IRegionManager regionManager)
        {
            _windowService = windowService;
            _adsService = adsService;
            _regionManager = regionManager;
        }

        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();
        }

        private void Start()
        {
            InjectService.Instance.Inject(this);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Play);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Play);
        }

        private void Play()
        {
            if (_regionManager.Progress.RegionIndex > 0)
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
            _windowService.Open(WindowType.Gameplay);
            _windowService.Close(WindowType.Footer);
        }
    }
}
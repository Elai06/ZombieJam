using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Ad
{
    public class AdsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private Button _closeButton;

        [Inject] private IAdsService _adsService;

        private void Start()
        {
            InjectService.Instance.Inject(this);
            Initialize();
        }

        private void OnEnable()
        {
            Initialize();

            _closeButton.gameObject.SetActive(false);
            _closeButton.onClick.AddListener(SkipAds);
        }

        private void OnDisable()
        {
            _adsService.Tick -= OnTick;
            _closeButton.onClick.RemoveListener(SkipAds);
        }

        private void SkipAds()
        {
            _adsService.SkipAds();
        }

        private void Initialize()
        {
            if (_adsService == null) return;

            _description.text = $"{_adsService.AdsType}";
            _timer.text = $"{_adsService.Timer.TimeProgress.Time}";
            _adsService.Tick += OnTick;

            DOVirtual.DelayedCall(_adsService.AdsConfig.HowLongToShowCloseButton,
                () => { _closeButton.gameObject.SetActive(true); });
        }

        private void OnTick(int time)
        {
            _timer.text = $"{time}";
        }
    }
}
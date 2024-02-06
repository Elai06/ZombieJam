using Gameplay.Enums;
using Gameplay.Level;
using Gameplay.Shop;
using Gameplay.Windows.Gameplay;
using Infrastructure.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows
{
    public class DiedView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _waveText;
        [SerializeField] private TextMeshProUGUI _regionName;
        [SerializeField] private TextMeshProUGUI _experienceText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _currentExperienceSlider;
        [SerializeField] private TextMeshProUGUI _reviveTextButton;
        [SerializeField] private Slider _levelSlider;
        [SerializeField] private Button _reviveButton;

        [SerializeField] private Transform _inAppContent;
        [SerializeField] private Transform _reviveContent;

        [Inject] private IGameplayModel _gameplayModel;
        [Inject] private ILevelModel _levelModel;
        [Inject] private IShopModel _shopModel;
        [Inject] private IWindowService _windowService;

        public void Start()
        {
            InjectService.Instance.Inject(this);
        }

        public void OnEnable()
        {
            _reviveContent.gameObject.SetActive(false);
            _inAppContent.gameObject.SetActive(false);

            var progress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var waveIndex = progress.CurrentWaweIndex == 0 ? 0 : progress.CurrentWaweIndex - 1;
            SetWave(progress.ERegionType, waveIndex);
            _gameplayModel.LooseWave();
            SetLevel();

            if (_gameplayModel.WaveType == EWaveType.Logic)
            {
                SetInAppContent();
                _reviveButton.onClick.AddListener(ClaimSimpleBox);
            }
            else
            {
                if (_gameplayModel.IsAvailableRessuraction)
                {
                    SetReviveContent();
                    _reviveButton.onClick.AddListener(Revive);
                }
                else
                {
                    SetInAppContent();
                    _reviveButton.onClick.AddListener(ClaimSimpleBox);
                }
            }
        }

        private void OnDisable()
        {
            _reviveButton.onClick.RemoveAllListeners();
        }

        private void Revive()
        {
            _gameplayModel.RessurectionUnits();
        }

        private void SetWave(ERegionType regionType, int index)
        {
            _waveText.text = $"Wave {index + 1} loose";
            _regionName.text = regionType.ToString();
        }

        private void SetLevel()
        {
            var currentLevel = _levelModel.CurrentLevel;
            var reqiredExperience = _levelModel.ReqiredExperienceForUp();
            var currentExperience = _levelModel.CurrentExperience;
            _experienceText.text = $" +{_gameplayModel.GetExperience(true)} experience";
            _levelText.text = $"{currentLevel + 1}";
            _levelSlider.value = (float)currentExperience / reqiredExperience;
            _currentExperienceSlider.text = $"{currentExperience}/{reqiredExperience}";
        }

        private void SetReviveContent()
        {
            _reviveTextButton.text = "Revive";
            _reviveContent.gameObject.SetActive(true);
        }

        private void SetInAppContent()
        {
            _inAppContent.gameObject.SetActive(true);
            _reviveTextButton.text = "Claim";
        }

        private void ClaimSimpleBox()
        {
            _shopModel.BuyProduct(EShopProductType.SimpleBox);

            Restart();
        }

        private void Restart()
        {
            SceneManager.LoadScene($"Gameplay");

            _windowService.Close(WindowType.Died);
            _windowService.Open(WindowType.MainMenu);
            _windowService.Open(WindowType.Footer);
            _gameplayModel.StopWave();
        }
    }
}
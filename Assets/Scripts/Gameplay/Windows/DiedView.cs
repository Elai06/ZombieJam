using System.Threading.Tasks;
using Gameplay.CinemachineCamera;
using Gameplay.Enums;
using Gameplay.Level;
using Gameplay.Shop;
using Gameplay.Windows.Gameplay;
using Infrastructure.StaticData;
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
        [SerializeField] private Image _adImage;
        [SerializeField] private Slider _levelSlider;
        [SerializeField] private Button _reviveButton;

        [SerializeField] private Transform _inAppContent;
        [SerializeField] private Transform _reviveContent;
        [SerializeField] private Transform _patrolContent;
        [SerializeField] private Transform _tankHintContent;

        [SerializeField] private Image _unitIcon;

        [Inject] private IGameplayModel _gameplayModel;
        [Inject] private ILevelModel _levelModel;
        [Inject] private IShopModel _shopModel;
        [Inject] private IWindowService _windowService;
        [Inject] private GameStaticData _gameStaticData;

        public void Start()
        {
            InjectService.Instance.Inject(this);
        }

        public bool DiedFromPatrol { get; set; }

        public void OnEnable()
        {
            _reviveContent.gameObject.SetActive(false);
            _inAppContent.gameObject.SetActive(false);
            _patrolContent.gameObject.SetActive(false);
            _tankHintContent.gameObject.SetActive(false);
            _adImage.gameObject.SetActive(true);

            var progress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var waveIndex = progress.CurrentWaweIndex;
            SetWave(progress.ERegionType, waveIndex);
            _gameplayModel.LooseWave();
            SetLevel();

            if (_gameplayModel.WaveType == EWaveType.Logic)
            {
                _reviveButton.onClick.AddListener(Restart);

                if (DiedFromPatrol)
                {
                    SetPatrolContent();
                }
                else
                {
                    SetInAppHintContent();
                }
            }
            else
            {
                var zombieTank = _gameplayModel.IsHaveTank();
                if (zombieTank != null)
                {
                    var sprite = _gameStaticData.SpritesConfig.GetZombieIcon(zombieTank.Config.Name).HalfHeighSprite;
                    SetTankHintContent(sprite);
                    _reviveButton.onClick.AddListener(Revive);
                }
                else if (_gameplayModel.IsAvailableRevive)
                {
                    SetReviveHintContent();
                    _reviveButton.onClick.AddListener(Revive);
                }
                else
                {
                    SetInAppHintContent();
                    _reviveButton.onClick.AddListener(ClaimSimpleBox);
                }
            }
        }

        private void OnDisable()
        {
            _reviveButton.onClick.RemoveAllListeners();
            DiedFromPatrol = false;
        }

        private void Revive()
        {
            _gameplayModel.StartReviveForAds();
        }

        private void SetWave(ERegionType regionType, int index)
        {
            _waveText.text = $"Wave {index + 1} loose";
            _regionName.text = regionType.ToString();
        }

        private void SetLevel()
        {
            var currentLevel = _levelModel.CurrentLevel;
            var reqiredExperience = _levelModel.RequiredExperienceForUp();
            var currentExperience = _levelModel.CurrentExperience;
            _experienceText.text = $" +{_gameplayModel.GetExperience(true)} experience";
            _levelText.text = $"{currentLevel + 1}";
            _levelSlider.value = (float)currentExperience / reqiredExperience;
            _currentExperienceSlider.text = $"{currentExperience}/{reqiredExperience}";
        }

        private void SetReviveHintContent()
        {
            _reviveContent.gameObject.SetActive(true);
            _reviveTextButton.text = "5 Revive";
        }

        private void SetInAppHintContent()
        {
            _inAppContent.gameObject.SetActive(true);
            _adImage.gameObject.SetActive(false);
            _reviveTextButton.text = "Restart";
        }

        private void SetPatrolContent()
        {
            _patrolContent.gameObject.SetActive(true);
            _adImage.gameObject.SetActive(false);
            _reviveTextButton.text = "Restart";
        }

        private void ClaimSimpleBox()
        {
            _shopModel.BuyProduct(EShopProductType.SimpleBox);

            Restart();
        }

        private void SetTankHintContent(Sprite sprite)
        {
            _reviveTextButton.text = "5 Revive";
            _tankHintContent.gameObject.SetActive(true);
            _unitIcon.sprite = sprite;
        }

        private async void Restart()
        {
            _gameplayModel.StopWave();

            SceneManager.LoadScene($"Gameplay");
            await Task.Delay(50);

            if (_windowService.IsOpen(WindowType.Died))
            {
                _windowService.Close(WindowType.Died);
            }

            var cameraSelector = FindObjectOfType<CameraSelector>();
            cameraSelector.ChangeCamera(ECameraType.Park);

            _gameplayModel.StartWave();
        }
    }
}
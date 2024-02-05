using Gameplay.Enums;
using Gameplay.Level;
using Gameplay.Windows.Gameplay;
using TMPro;
using UnityEngine;
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
        [SerializeField] private Slider _levelSlider;
        [SerializeField] private Button _ressurectionButton;

        [Inject] private IGameplayModel _gameplayModel;
        [Inject] private ILevelModel _levelModel;

        public void Start()
        {
            InjectService.Instance.Inject(this);
        }

        public void OnEnable()
        {
            var progress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var waveIndex = progress.CurrentWaweIndex == 0 ? 0 : progress.CurrentWaweIndex - 1;
            SetWave(progress.ERegionType, waveIndex);
            _gameplayModel.LooseWave();
            SetLevel();

            if (_gameplayModel.WaveType == EWaveType.Logic)
            {
                _ressurectionButton.gameObject.SetActive(false);
                return;
            }
            
            _ressurectionButton.onClick.AddListener(Revive);
            _ressurectionButton.gameObject.SetActive(_gameplayModel.IsAvailableRessuraction);
        }

        private void Revive()
        {
            if (_gameplayModel.IsAvailableRessuraction)
            {
                _gameplayModel.RessurectionUnits();
            }
        }

        private void OnDisable()
        {
            _ressurectionButton.onClick.AddListener(Revive);
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
    }
}
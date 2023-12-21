using Gameplay.Enums;
using Gameplay.Windows.Gameplay;
using TMPro;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows
{
    public class VictoryView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _waveText;
        [SerializeField] private TextMeshProUGUI _regionName;

        [Inject] private IGameplayModel _gameplayModel;

        public void Start()
        {
            InjectService.Instance.Inject(this);
        }

        public void OnEnable()
        {
            var progress = _gameplayModel.GetCurrentRegionProgress();
            var waveIndex = progress.CurrentWaweIndex == 0 ? 0 : progress.CurrentWaweIndex - 1;
            SetWave(progress.CurrentRegionType, waveIndex);
        }

        private void SetWave(ERegionType regionType, int index)
        {
            _waveText.text = $"Wave {index}";
            _regionName.text = regionType.ToString();
        }
    }
}
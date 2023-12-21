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

        [Inject] private IGameplayModel _gameplayWindow;

        public void Start()
        {
            InjectService.Instance.Inject(this);
        }

        public void OnEnable()
        {
            var waveIndex = _gameplayWindow.GetCurrentWaveIndex() == 0 ? 0 : _gameplayWindow.GetCurrentWaveIndex() - 1;
            SetWave(waveIndex);
        }

        private void SetWave(int index)
        {
            _waveText.text = $"Волна {index}";
        }
    }
}
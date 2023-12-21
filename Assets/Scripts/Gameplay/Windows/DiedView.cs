using System;
using Gameplay.Windows.Gameplay;
using TMPro;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows
{
    public class DiedView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _waveText;

        [Inject] private IGameplayModel _gameplayWindow;

        public void Start()
        {
            InjectService.Instance.Inject(this);
        }

        public void OnEnable()
        {
            SetWave(_gameplayWindow.GetCurrentWaveIndex());
        }

        private void SetWave(int index)
        {
            _waveText.text = $"Волна {index}";
        }
    }
}
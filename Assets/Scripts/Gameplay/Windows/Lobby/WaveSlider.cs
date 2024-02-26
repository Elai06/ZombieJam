using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Lobby
{
    public class WaveSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Sprite _completedWave;
        [SerializeField] private Sprite _notCompletedWave;
        [SerializeField] private Sprite _currentWave;

        [SerializeField] private List<Image> _waveSprites;


        public void UpdateWave(float waveCount, int waveIndex)
        {
            _slider.value = waveIndex / (waveCount - 1);
            UpdateSliderWavesImage(waveIndex);
        }

        private void UpdateSliderWavesImage(int waveIndex)
        {
            for (var index = 0; index < _waveSprites.Count; index++)
            {
                var waveSprite = _waveSprites[index];
                if (waveIndex > index)
                {
                    waveSprite.sprite = _completedWave;
                }
                else if (waveIndex == index)
                {
                    waveSprite.sprite = _currentWave;
                }
                else
                {
                    waveSprite.sprite = _notCompletedWave;
                }
            }
        }
    }
}
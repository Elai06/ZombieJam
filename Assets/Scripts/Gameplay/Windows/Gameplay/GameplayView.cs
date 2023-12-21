using Gameplay.Enums;
using TMPro;
using UnityEngine;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _waveText;

        public void SetWave(ERegionType regionType, int index)
        {
            _waveText.text = $"{regionType.ToString()}: Wave {index}";
        }
    }
}
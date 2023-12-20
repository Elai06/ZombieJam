using TMPro;
using UnityEngine;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _waveIndex;

        public void SetWave(int index)
        {
            _waveIndex.text = $"Wave {index}";
        }
    }
}
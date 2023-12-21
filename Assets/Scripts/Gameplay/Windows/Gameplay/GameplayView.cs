using TMPro;
using UnityEngine;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _waveText;

        public void SetWave(int index)
        {
            _waveText.text = $"Кладбище: Волна {index}";
        }
    }
}
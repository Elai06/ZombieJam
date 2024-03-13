using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views
{
    public class VignetteView : MonoBehaviour
    {
        [SerializeField] private Image _vignette;
        [SerializeField] private float _duration = 1;
        [SerializeField, Range(0, 1)] private float _alpha = 0.5f;

        private void OnEnable()
        {
            _vignette.color = Color.clear;

            DOVirtual.Float(0, _alpha, _duration, value => { _vignette.color = new Color(0, 0, 0, value); });
        }

        private void OnDisable()
        {
            _vignette.color = Color.clear;
        }
    }
}
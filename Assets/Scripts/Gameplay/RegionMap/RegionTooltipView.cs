using System;
using DG.Tweening;
using Gameplay.Configs.Region;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.RegionMap
{
    public class RegionTooltipView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _regionName;
        [SerializeField] private Image _availabilityText;
        [SerializeField] private Image _inProgressIcon;
        [SerializeField] private TextMeshProUGUI _waveText;
        [SerializeField] private TextMeshProUGUI _completeText;
        [SerializeField] private Image _completeIcon;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _closeButton;

        private Tween _tween;

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(Close);
        }

        private void Start()
        {
            _canvas.enabled = false;
        }

        public void Initialize(RegionProgressData data)
        {
            _canvas.enabled = true;
            _regionName.text = data.ERegionType.ToString();
            _waveText.text = $"Wave {data.CurrentWaweIndex} / 4";
            _completeText.text = data.IsCompleted ? "Complete" : "Capture the previous region";
            _completeText.color = data.IsCompleted ? Color.green : Color.red;

            _inProgressIcon.gameObject.SetActive(data.IsOpen && !data.IsCompleted);

            _completeText.text = data.IsOpen && !data.IsCompleted ? "In progress" : _completeText.text;
            _completeText.color = data.IsOpen && !data.IsCompleted ? Color.white : _completeText.color;

            _availabilityText.gameObject.SetActive(!data.IsOpen && !data.IsCompleted);

            _completeIcon.gameObject.SetActive(data.IsCompleted);

            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(3, () => { _canvas.enabled = false; });
        }

        private void Close()
        {
            _canvas.enabled = false;
        }
    }
}
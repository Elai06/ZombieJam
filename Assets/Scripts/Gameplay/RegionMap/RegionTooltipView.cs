using System;
using DG.Tweening;
using Gameplay.Configs.Region;
using TMPro;
using UnityEngine;

namespace Gameplay.RegionMap
{
    public class RegionTooltipView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _regionName;
        [SerializeField] private TextMeshProUGUI _availabilityText;
        [SerializeField] private TextMeshProUGUI _waveText;
        [SerializeField] private TextMeshProUGUI _completeText;
        [SerializeField] private Canvas _canvas;

        private Tween _tween;

        private void Start()
        {
            _canvas.enabled = false;
        }

        public void Initialize(RegionProgressData data)
        {
            _canvas.enabled = true;
            _regionName.text = data.ERegionType.ToString();
            _availabilityText.text = data.IsOpen ? "Availability: IsOpen" : "Availability: IsClosed";
            _waveText.text = $"Wave {data.CurrentWaweIndex} / 4";
            _completeText.text = data.IsCompleted ? "Complete" : "Not complete";
            _completeText.color = data.IsCompleted ? Color.green : Color.red;

            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(3, () => { _canvas.enabled = false; });
        }
    }
}
﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Header
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Slider _experienceSlider;

        public void Initialize(int level, int exeperience, int reqiredExperience)
        {
            _levelText.text = $"{level + 1}";
            _experienceSlider.value = (float)exeperience / reqiredExperience;
        }
    }
}
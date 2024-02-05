using System;
using Gameplay.Level;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Windows.LevelUp
{
    public class LevelUpWindow : Window
    {
        [SerializeField] private LevelUpViewInitializer _levelUpViewInitializer;

        [Inject] private ILevelModel _levelModel;

        private void OnEnable()
        {
            _levelUpViewInitializer.Initialize(_levelModel);
        }
    }
}
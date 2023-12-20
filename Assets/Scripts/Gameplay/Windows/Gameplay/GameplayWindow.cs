using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayWindow : Window
    {
        [SerializeField] private GameplayViewInitializer _gameplayViewInitializer;

        [Inject] private IGameplayModel _gameplayModel;

        private void OnEnable()
        {
            _gameplayViewInitializer.Initialize(_gameplayModel);
        }
    }
}
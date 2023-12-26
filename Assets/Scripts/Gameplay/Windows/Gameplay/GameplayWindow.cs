using Gameplay.Boosters;
using Gameplay.Windows.Boosters;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayWindow : Window
    {
        [SerializeField] private GameplayViewInitializer _gameplayViewInitializer;
        [SerializeField] private BoosterViewInitialier _boosterViewInitialier;

        [Inject] private IGameplayModel _gameplayModel;
        [Inject] private IBoostersManager _boostersManager;

        private void OnEnable()
        {
            _gameplayViewInitializer.Initialize(_gameplayModel);
            _boosterViewInitialier.Initialize(_boostersManager);
        }
    }
}
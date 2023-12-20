using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.UnityBehaviours;
using Infrastructure.Windows;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class GameState : IState
    {
        private readonly IWindowService _windowService;
        private IStateMachine _stateMachine;

        public GameState(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _windowService.Open(WindowType.MainMenu);
            _windowService.Open(WindowType.Gameplay);
        }

        public void Exit()
        {
        }
    }
}
using _Project.Scripts.Infrastructure.StateMachine.States;
using Infrastructure.Session;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Infrastructure.Bootstrap
{
    public class GameBootstrapper : MonoBehaviour
    {
        private IStateMachine _stateMachine;
        private ISessionManager _sessionManager;

        [Inject]
        private void Construct(IStateMachine stateMachine, ISessionManager sessionManager)
        {
            _stateMachine = stateMachine;
            _sessionManager = sessionManager;
        }

        private void Awake() => DontDestroyOnLoad(gameObject);

        private void Start()
        {
            _stateMachine?.Enter<BootstrapState>();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _stateMachine.Enter<ExitState>();
            }
        }

        private void OnApplicationQuit()
        {
            _sessionManager.SendSessionEvent();
            _stateMachine.Enter<ExitState>();
        }
    }
}
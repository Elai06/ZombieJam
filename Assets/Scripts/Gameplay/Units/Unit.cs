using System;
using Gameplay.Battle;
using Gameplay.Enemies;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units.Mover;
using Gameplay.Units.States;
using Infrastructure.Input;
using Infrastructure.StateMachine;
using Infrastructure.UnityBehaviours;
using UnityEngine;
using Utils.CurveBezier;

namespace Gameplay.Units
{
    public abstract class Unit : MonoBehaviour, ISwiped
    {
        public event Action<ESwipeSide> OnSwipe;
        public event Action<GameObject> OnCollision;

        [SerializeField] private ArrowDirection _arrowDirection;
        [SerializeField] private RotateObject _rotateObject;
        [SerializeField] private HealthBar _healthBar;

        private readonly StateMachine _stateMachine = new();

        private ICoroutineService _coroutineService;
        private ParametersConfig _parametersConfig;
        private ESwipeDirection _eSwipeDirection;
        private BattleManager _battleManager;

        public EUnitState CurrentState { get; set; }
        public BezierCurve Curve { get; private set; }

        public ParametersConfig Parameters => _parametersConfig;

        public float Health { get; private set; }


        public void Initialize(ParametersConfig parametersConfig, ICoroutineService coroutineService,
            BattleManager battleManager)
        {
            _parametersConfig = parametersConfig;
            _coroutineService = coroutineService;
            _battleManager = battleManager;
            
            Health = _parametersConfig.GetDictionary()[EParameter.Health];

            _healthBar.Initialize(Health);
            InitializeStates();
        }

        private void InitializeStates()
        {
            var parkingState = new UnitParkingState(this, _eSwipeDirection, _parametersConfig, _coroutineService);
            var roadState = new UnitRoadState(this, _coroutineService, _rotateObject);
            var battleState = new UnitBattleState(this, _battleManager, _coroutineService);

            _stateMachine.AddState(parkingState);
            _stateMachine.AddState(roadState);
            _stateMachine.AddState(battleState);
            _stateMachine.Enter<UnitParkingState>();
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            OnCollision?.Invoke(collision.gameObject);
        }

        public void Swipe(ESwipeSide swipe)
        {
            OnSwipe?.Invoke(swipe);
        }

        public void SetSwipeDirection(ESwipeDirection swipeDirection)
        {
            _eSwipeDirection = swipeDirection;
            _arrowDirection.SetArrowDirection(swipeDirection);
        }

        public void InitializePath(BezierCurve bezierCurve)
        {
            if (CurrentState == EUnitState.Road) return;

            Curve = bezierCurve;
            _stateMachine.Enter<UnitRoadState>();
        }

        public void DamageToTarget(Enemy enemy)
        {
            var attack = _parametersConfig.GetDictionary()[EParameter.Attack];
            enemy.GetDamage(attack);
        }

        public void GetDamage(float damage)
        {
            Health -= damage;
            _healthBar.ChangeHealth(Health);
        }
    }
}
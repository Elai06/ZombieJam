using System;
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Cards;
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
        public event Action ResetMoving;
        public event Action OnDied;
        public event Action<ESwipeSide> OnSwipe;
        public event Action<GameObject> OnCollision;
        public event Action OnInitializePath;

        [SerializeField] private ArrowDirection _arrowDirection;
        [SerializeField] private RotateObject _rotateObject;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _prefab;
        [SerializeField] private ESwipeDirection _eSwipeDirection;

        private CardModel _cardModel;

        private readonly StateMachine _stateMachine = new();

        private ICoroutineService _coroutineService;
        private ITargetManager _targetManager;

        public EUnitState CurrentState { get; set; }
        public EZombieType ZombieType { get; set; }
        public BezierCurve Curve { get; private set; }
        public Enemy Target { get; set; }

        public float Health { get; private set; }
        public bool IsDied { get; private set; }

        public StateMachine StateMachine => _stateMachine;

        public Transform Prefab => _prefab;

        public ESwipeDirection SwipeDirection => _eSwipeDirection;

        public void Initialize(CardModel cardModel, ICoroutineService coroutineService,
            ITargetManager targetManager, EZombieType type)
        {
            _cardModel = cardModel;
            _coroutineService = coroutineService;
            _targetManager = targetManager;
            ZombieType = type;

            Health = Parameters[EParameter.Health];

            _healthBar.Initialize(Health);
            InitializeStates();
        }

        public Dictionary<EParameter, float> Parameters => _cardModel.Parameters;

        private void InitializeStates()
        {
            var parkingState = new UnitParkingState(this, Parameters, _coroutineService);
            var roadState = new UnitRoadState(this, _coroutineService, _rotateObject);
            var battleState = new UnitBattleState(this, _targetManager, _coroutineService, _rotateObject);
            var diedState = new UnitDiedState(this);

            _stateMachine.AddState(parkingState);
            _stateMachine.AddState(roadState);
            _stateMachine.AddState(battleState);
            _stateMachine.AddState(diedState);
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
            if (CurrentState == EUnitState.Road || IsDied) return;

            Curve = bezierCurve;

            OnInitializePath?.Invoke();
        }

        public void DamageToTarget(Enemy enemy)
        {
            if (enemy == null) return;

            var attack = Parameters[EParameter.Attack];
            enemy.GetDamage(attack);
        }

        public void GetDamage(float damage)
        {
            if (Health <= 0) return;

            Health -= damage;
            _healthBar.ChangeHealth(Health, (int)damage);

            if (Health <= 0)
            {
                Died();
            }
        }

        public void PlayAttackAnimation()
        {
            _animator.SetTrigger("Attack");
        }

        public void ResetSwipeDirection()
        {
            _eSwipeDirection = ESwipeDirection.None;
        }

        public void ResetMovingAfterBooster()
        {
            ResetMoving?.Invoke();
        }

        public void Resurection()
        {
            IsDied = false;
            Health = Parameters[EParameter.Health] / 2;
            gameObject.SetActive(true);
            _healthBar.ChangeHealth(Health, 0);
            _stateMachine.Enter<UnitBattleState>();
        }

        public void Died()
        {
            Health = 0;
            IsDied = true;
            _stateMachine.Enter<UnitDiedState>();
            OnDied?.Invoke();
        }
    }
}
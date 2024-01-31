using System;
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Cards;
using Gameplay.Configs.Zombies;
using Gameplay.Enemies;
using Gameplay.Enums;
using Gameplay.Units.Mover;
using Gameplay.Units.States;
using Infrastructure.Input;
using Infrastructure.UnityBehaviours;
using UnityEngine;
using Utils.CurveBezier;
using StateMachine = Infrastructure.StateMachine.StateMachine;

namespace Gameplay.Units
{
    public abstract class Unit : MonoBehaviour, ISwipeObject
    {
        public event Action ResetMoving;
        public event Action<Unit> OnDied;
        public event Action<Unit> Kicked;
        public event Action<ESwipeSide> OnSwipe;
        public event Action<GameObject> OnCollision;
        public event Action OnInitializePath;

        [SerializeField] protected ArrowDirection _arrowDirection;
        [SerializeField] protected RotateObject _rotateObject;
        [SerializeField] protected HealthBar _healthBar;
        [SerializeField] protected Animator _animator;
        [SerializeField] protected Transform _prefab;
        [SerializeField] protected ESwipeDirection _eSwipeDirection;

        protected CardModel _cardModel;

        protected readonly StateMachine _stateMachine = new();

        protected ICoroutineService _coroutineService;
        protected ITargetManager _targetManager;

        public EUnitState CurrentState { get; set; }
        public EUnitClass UnitClass { get; set; }
        public BezierCurve Curve { get; private set; }
        public IEnemy Target { get; set; }

        public float Health { get; private set; }
        public bool IsDied { get; private set; }

        public ZombieData Config;
        public StateMachine StateMachine => _stateMachine;

        public Transform Prefab => _prefab;

        public ESwipeDirection SwipeDirection => _eSwipeDirection;

        public Dictionary<EParameter, float> Parameters { get; private set; } = new();

        public Animator Animator => _animator;

        protected List<IEnemy> _attackedEnemies = new();

        public void Initialize(CardModel cardModel, ICoroutineService coroutineService,
            ITargetManager targetManager, EUnitClass type, ZombieData zombieData)
        {
            _cardModel = cardModel;
            _coroutineService = coroutineService;
            _targetManager = targetManager;
            UnitClass = type;
            Config = zombieData;

            InitializeParameters();
            InitializeStates();
        }

        private void InitializeParameters()
        {
            Parameters.Clear();
            foreach (var parameter in _cardModel.Parameters)
            {
                Parameters.Add(parameter.Key, parameter.Value);
            }

            Health = Parameters[EParameter.Health];
            _healthBar.Initialize(Health);
        }

        public virtual void InitializeStates()
        {
            var kickState = new UnitKickState(this);
            _stateMachine.AddState(kickState);
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

        public void DamageToTarget(IEnemy enemy)
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
            if (_animator == null) return;

            _animator.SetTrigger("Attack");
            _animator.GetComponentInChildren<AnimatorDuplicator>().Attack();
        }

        public void ResetSwipeDirection()
        {
            _eSwipeDirection = ESwipeDirection.None;
        }

        public void ResetMovingAfterBooster()
        {
            ResetMoving?.Invoke();
        }

        public virtual void Resurection()
        {
            if (CurrentState != EUnitState.Died) return;

            IsDied = false;
            Health = Parameters[EParameter.Health] / 2;
            gameObject.SetActive(true);
            _healthBar.ChangeHealth(Health, 0);
        }

        public void Died()
        {
            Health = 0;
            IsDied = true;
            _stateMachine.Enter<UnitDiedState>();
            _animator.SetTrigger("Died");
            OnDied?.Invoke(this);
        }

        public Vector3 GetPosition(IEnemy enemy, float radiusAttack)
        {
            _attackedEnemies.Add(enemy);
            var angle = _attackedEnemies.Count - 12 * Mathf.PI * 2 / 12;
            return new Vector3(Mathf.Cos(angle) * radiusAttack, 0, Mathf.Sin(angle) * radiusAttack) +
                   gameObject.transform.position;
        }

        public void Kick()
        {
            _stateMachine.Enter<UnitKickState>();
            Kicked?.Invoke(this);
        }
    }
}
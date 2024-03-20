using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Battle;
using Gameplay.Cards;
using Gameplay.Configs.Zombies;
using Gameplay.Enemies;
using Gameplay.Enums;
using Gameplay.Units.Mover;
using Gameplay.Units.States;
using Infrastructure.UnityBehaviours;
using UnityEngine;
using Utils.CurveBezier;
using StateMachine = Infrastructure.StateMachine.StateMachine;

namespace Gameplay.Units
{
    public abstract class Unit : MonoBehaviour
    {
        public event Action ResetMoving;
        public event Action<Unit> OnDied;
        public event Action<Unit> Kicked;
        public event Action<ESwipeSide> OnSwipe;
        public event Action<GameObject> OnCollision;
        public event Action OnInitializePath;
        public event Action TakeDamage;
        public event Action DoDamage;

        [SerializeField] protected RotateObject _rotateObject;
        [SerializeField] protected ArrowDirection _arrowDirection;
        [SerializeField] protected HealthBar _healthBar;
        [SerializeField] protected Animator _animator;
        [SerializeField] protected ESwipeDirection _eSwipeDirection;
        [SerializeField] private UnitSwipe _unitSwipe;

        [Header("Effects")] 
        [SerializeField] private ParticleSystem _stunnedEffect;
        [SerializeField] private ParticleSystem _getDamageEffect;
        
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
        public ZombieData Config { get; set; }
        public StateMachine StateMachine => _stateMachine;
        public Transform Prefab => transform;
        public ESwipeDirection SwipeDirection => _eSwipeDirection;
        public Dictionary<EParameter, float> Parameters { get; } = new();
        public Animator Animator => _animator;
        public CapsuleCollider Collider { get; private set; }

        public void Initialize(CardModel cardModel, ICoroutineService coroutineService,
            ITargetManager targetManager, ZombieData zombieData)
        {
            _cardModel = cardModel;
            _coroutineService = coroutineService;
            _targetManager = targetManager;
            Config = zombieData;
            UnitClass = zombieData.Type;
            
            InitializeParameters();
            InitializeStates();

            Collider = gameObject.GetComponent<CapsuleCollider>();
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

        protected virtual void InitializeStates()
        {
            var kickState = new UnitKickState(this);
            var victoryState = new UnitVictoryState(this);
            _stateMachine.AddState(kickState);
            _stateMachine.AddState(victoryState);
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            OnCollision?.Invoke(collision.gameObject);
        }

        public void Swipe(ESwipeSide swipe)
        {
            OnSwipe?.Invoke(swipe);
        }

        public void SetSwipeDirection(ESwipeDirection swipeDirection, ESwipeSide swipeSide = ESwipeSide.None)
        {
            _eSwipeDirection = swipeDirection;
            _unitSwipe.SwipeDirection = _eSwipeDirection;
            _arrowDirection.SetArrowDirection(SwipeDirection, swipeSide);
        }

        public void InitializePath(BezierCurve bezierCurve)
        {
            if (CurrentState == EUnitState.Road || IsDied) return;

            Curve = bezierCurve;

            OnInitializePath?.Invoke();
        }

        public void DamageToTarget(IEnemy enemy, bool isNeedBlood = true)
        {
            if (enemy == null || this == null) return;

            var attack = Parameters[EParameter.Damage];
            enemy.GetDamage(attack, isNeedBlood);
            DoDamage?.Invoke();
        }

        public void GetDamage(float damage, bool isNeedBlood = true)
        {
            if (Health <= 0) return;

            Health -= damage;
            _healthBar.ChangeHealth(Health, (int)damage);

            if (isNeedBlood)
            {
                _getDamageEffect.Play();
            }

            if (Health <= 0)
            {
                Died();
            }

            TakeDamage?.Invoke();
        }

        public void PlayAttackAnimation()
        {
            if (_animator == null) return;

            _animator.SetTrigger("Attack");

            var animatorDuplicator = _animator.GetComponentInChildren<AnimatorDuplicator>();
            if (animatorDuplicator != null)
            {
                animatorDuplicator.Attack();
            }
        }

        public void ResetSwipeDirection()
        {
            _eSwipeDirection = ESwipeDirection.None;
        }

        public void ResetMovingAfterBooster()
        {
            ResetMoving?.Invoke();
        }

        public virtual void Revive()
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

        public void Kick()
        {
            _stateMachine.Enter<UnitKickState>();
            Kicked?.Invoke(this);
        }

        public async void Bash(int duration)
        {
            _stunnedEffect.Play();

            await Task.Delay(duration);

            _stunnedEffect.Stop();
        }

        public void EnterVictoryState()
        {
           _stateMachine.Enter<UnitVictoryState>();
        }
    }
}
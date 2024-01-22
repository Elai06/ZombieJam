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
using Infrastructure.UnityBehaviours;
using Unity.VisualScripting;
using UnityEngine;
using Utils.CurveBezier;
using StateMachine = Infrastructure.StateMachine.StateMachine;

namespace Gameplay.Units
{
    public abstract class Unit : MonoBehaviour, ISwipeObject

    {
    public event Action ResetMoving;
    public event Action OnDied;
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

    public StateMachine StateMachine => _stateMachine;

    public Transform Prefab => _prefab;

    public ESwipeDirection SwipeDirection => _eSwipeDirection;

    protected List<IEnemy> _attackedEnemies = new();

    public void Initialize(CardModel cardModel, ICoroutineService coroutineService,
        ITargetManager targetManager, EUnitClass type)
    {
        _cardModel = cardModel;
        _coroutineService = coroutineService;
        _targetManager = targetManager;
        UnitClass = type;

        Health = Parameters[EParameter.Health];

        _healthBar.Initialize(Health);
        InitializeStates();
    }

    public Dictionary<EParameter, float> Parameters => _cardModel.Parameters;

    public virtual void InitializeStates()
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

    public Vector3 GetPosition(IEnemy enemy, float radiusAttack)
    {
        _attackedEnemies.Add(enemy);
        var angle = _attackedEnemies.Count - 12 * Mathf.PI * 2 / 12;
        return new Vector3(Mathf.Cos(angle) * radiusAttack, 0, Mathf.Sin(angle) * radiusAttack) +
               gameObject.transform.position;
    }
    }
}
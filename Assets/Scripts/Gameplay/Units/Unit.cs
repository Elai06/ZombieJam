using System;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units.Mover;
using Gameplay.Units.States;
using Infrastructure.Input;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
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

        private readonly StateMachine _stateMachine = new();

        public BezierCurve Curve { get; private set; }
        private ESwipeDirection _eSwipeDirection;
        private ICoroutineService _coroutineService;
        private ParametersConfig _parametersConfig;
        public EUnitState CurrentState { get; set; }

        public void Initialize(ParametersConfig parametersConfig, ICoroutineService coroutineService)
        {
            _parametersConfig = parametersConfig;
            _coroutineService = coroutineService;

            InitializeStates();
        }

        private void InitializeStates()
        {
            var parkingState = new UnitParkingState(this, _eSwipeDirection, _parametersConfig, _coroutineService);
            var roadState = new UnitRoadState(this, _coroutineService, _rotateObject);
            _stateMachine.AddState(parkingState);
            _stateMachine.AddState(roadState);
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
            if(CurrentState == EUnitState.Road) return;
            
            Curve = bezierCurve;
            _stateMachine.Enter<UnitRoadState>();
        }
    }
}
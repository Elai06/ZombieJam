using System.Collections;
using System.Threading.Tasks;
using Gameplay.Enums;
using Gameplay.Parameters;
using Infrastructure.StateMachine.States;
using Infrastructure.UnityBehaviours;
using SirGames.Scripts.Infrastructure.StateMachine;
using UnityEngine;

namespace Gameplay.Units.States
{
    public class UnitParkingState : IState
    {
        private Unit _unit;
        private IStateMachine _stateMachine;
        private ICoroutineService _coroutineService;

        private ESwipeDirection _eSwipeDirection;
        private ESwipeSide _eSwipeSide = ESwipeSide.None;

        private float _bashBackWard = 0.25f;
        private float _speed;

        private bool _isMove;

        public UnitParkingState(Unit unit, ESwipeDirection swipeDirection, ParametersConfig parametersConfig,
            ICoroutineService coroutineService)
        {
            _unit = unit;
            _eSwipeDirection = swipeDirection;
            _speed = parametersConfig.GetDictionary()[EParameter.TravelSpeed];
            _coroutineService = coroutineService;
        }

        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _unit.OnSwipe += Swipe;
            _unit.OnCollision += OnCollisionEnter;
            _unit.CurrentState = EUnitState.Parking;
        }

        public void Exit()
        {
            _unit.OnSwipe -= Swipe;
            _unit.OnCollision -= OnCollisionEnter;
        }

        private void Swipe(ESwipeSide swipe)
        {
            if (_isMove || swipe == ESwipeSide.None || !IsAvailableSwipe(swipe)) return;

            _eSwipeSide = swipe;
            _isMove = true;

            _coroutineService.StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            while (_isMove)
            {
                yield return new WaitForFixedUpdate();
                SelectDirection(_eSwipeSide);
            }
        }

        private void OnCollisionEnter(GameObject other)
        {
            if (!_isMove) return;
            _isMove = false;


            if (other.layer == 3)
            {
                var collision = other.GetComponent<Unit>();
                if (collision.CurrentState == EUnitState.Road)
                {
                    MoveAfterBash();
                }
            }

            if (other.layer == 3 || other.layer == 6)
            {
                Bash(_eSwipeSide, _bashBackWard);
            }
        }

        private void SelectDirection(ESwipeSide eSwipeSide)
        {
            if (eSwipeSide == ESwipeSide.None)
            {
                _isMove = false;
                return;
            }

            if (_eSwipeDirection == ESwipeDirection.Vertical)
            {
                switch (eSwipeSide)
                {
                    case ESwipeSide.Back:
                        Move(Vector3.back);
                        return;
                    case ESwipeSide.Forward:
                        Move(Vector3.forward);
                        return;
                }
            }

            if (_eSwipeDirection == ESwipeDirection.Horizontal)
            {
                switch (eSwipeSide)
                {
                    case ESwipeSide.Left:
                        Move(Vector3.left);
                        return;
                    case ESwipeSide.Right:
                        Move(Vector3.right);
                        return;
                }
            }
        }

        private void Bash(ESwipeSide eSwipeSide, float bashBackWard)
        {
            switch (eSwipeSide)
            {
                case ESwipeSide.Back:
                    _unit.transform.position -= Vector3.back * bashBackWard;
                    break;
                case ESwipeSide.Forward:
                    _unit.transform.position -= Vector3.forward * bashBackWard;
                    break;
                case ESwipeSide.Left:
                    _unit.transform.position -= Vector3.left * bashBackWard;
                    break;
                case ESwipeSide.Right:
                    _unit.transform.position -= Vector3.right * bashBackWard;
                    break;
            }
        }

        private bool IsAvailableSwipe(ESwipeSide eSwipeSide)
        {
            switch (_eSwipeDirection)
            {
                case ESwipeDirection.Horizontal:
                    return eSwipeSide == ESwipeSide.Left || eSwipeSide == ESwipeSide.Right;
                case ESwipeDirection.Vertical:
                    return eSwipeSide == ESwipeSide.Forward || eSwipeSide == ESwipeSide.Back;
                default:
                    return false;
            }
        }

        private void Move(Vector3 targetPosition)
        {
            _unit.gameObject.transform.position += targetPosition * Time.fixedDeltaTime * _speed;
        }

        private async void MoveAfterBash()
        {
            await Task.Delay(500);
            _isMove = true;
            _coroutineService.StartCoroutine(Move());
        }
    }
}
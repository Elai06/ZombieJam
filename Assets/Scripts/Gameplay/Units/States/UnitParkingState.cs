using System.Collections;
using Gameplay.Enums;
using Gameplay.Parameters;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Units.States
{
    public class UnitParkingState : UnitState
    {
        private ICoroutineService _coroutineService;

        private ESwipeSide _eSwipeSide = ESwipeSide.None;

        private float _bashBackWard = 0.25f;
        private float _speed;

        private bool _isMove;

        private Coroutine _coroutine;

        public UnitParkingState(Unit unit, ParametersConfig parametersConfig,
            ICoroutineService coroutineService) : base(EUnitState.Parking, unit)
        {
            _unit = unit;
            _speed = parametersConfig.GetDictionary()[EParameter.SpeedOnPark];
            _coroutineService = coroutineService;
        }

        public override void Enter()
        {
            base.Enter();

            _unit.OnSwipe += Swipe;
            _unit.OnCollision += OnCollisionEnter;
            _unit.OnInitializePath += InitializePath;
            _unit.ResetMoving += OnResetMoving;
        }

        public override void Exit()
        {
            base.Exit();

            _unit.OnSwipe -= Swipe;
            _unit.OnCollision -= OnCollisionEnter;
            _unit.OnInitializePath -= InitializePath;
            _unit.ResetMoving -= OnResetMoving;

            if (_coroutine != null)
            {
                _coroutineService.StopCoroutine(_coroutine);
            }

            _isMove = false;
            _unit.ResetSwipeDirection();
        }

        private void InitializePath()
        {
            _stateMachine.Enter<UnitRoadState>();
        }

        private IEnumerator StartMove()
        {
            while (_isMove)
            {
                yield return new WaitForFixedUpdate();
                SelectDirection(_eSwipeSide);
            }
        }

        private void Swipe(ESwipeSide swipe)
        {
            if (_isMove || swipe == ESwipeSide.None || !IsAvailableSwipe(swipe)) return;

            _eSwipeSide = swipe;
            _isMove = true;

            _coroutine = _coroutineService.StartCoroutine(StartMove());
        }

        private void OnCollisionEnter(GameObject other)
        {
            if (!_isMove) return;
            _isMove = false;

            var collision = other.GetComponent<Unit>();

            if (collision != null && collision.CurrentState == EUnitState.Road)
            {
                _coroutine = _coroutineService.StartCoroutine(MoveAfterBash(collision.transform));
                return;
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

            if (_unit.SwipeDirection == ESwipeDirection.Vertical)
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

            if (_unit.SwipeDirection == ESwipeDirection.Horizontal)
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
            switch (_unit.SwipeDirection)
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

        private IEnumerator MoveAfterBash(Transform collision)
        {
            _isMove = false;

            var distanceToCollision = Vector3.Distance(_unit.transform.position, collision.position);
            while (distanceToCollision < 1f + _unit.Prefab.localScale.z)
            {
                distanceToCollision = Vector3.Distance(_unit.transform.position, collision.position);
                yield return new WaitForFixedUpdate();
            }

            _isMove = true;
            _coroutineService.StartCoroutine(StartMove());
        }
        
        private void OnResetMoving()
        {
            _isMove = false;
            _eSwipeSide = ESwipeSide.None;
        }
    }
}
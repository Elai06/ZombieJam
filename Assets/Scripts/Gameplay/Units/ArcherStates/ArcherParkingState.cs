using System.Collections;
using System.Collections.Generic;
using Gameplay.Enums;
using Gameplay.Units.States;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Units.ArcherStates
{
    public class ArcherParkingState : ArcherState
    {
        private ICoroutineService _coroutineService;

        private ESwipeSide _eSwipeSide = ESwipeSide.None;

        private float _bashBackWard = 0.25f;
        private float _speed;

        private bool _isMove;

        private Coroutine _coroutine;

        public ArcherParkingState(ArcherUnit unit, Dictionary<EParameter, float> parametersConfig,
            ICoroutineService coroutineService) : base(EUnitState.Parking, unit)
        {
            _unit = unit;
            _speed = parametersConfig[EParameter.SpeedOnPark];
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

            StopMove();

            _unit.ResetSwipeDirection();
        }

        private void StopMove()
        {
            _isMove = false;
            if (_coroutine != null)
            {
                _coroutineService.StopCoroutine(_coroutine);
            }
        }

        private void InitializePath()
        {
            _stateMachine.Enter<ArcherRoadState>();
        }

        private IEnumerator StartMove()
        {
            _isMove = true;
            _unit.Animator.SetTrigger("Move");
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

            _coroutine = _coroutineService.StartCoroutine(StartMove());
        }

        private void OnCollisionEnter(GameObject other)
        {
            if (!_isMove) return;
            StopMove();

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
                StopMove();
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
            StopMove();

            var distanceToCollision = Vector3.Distance(_unit.transform.position, collision.position);
            while (distanceToCollision < 1f + _unit.Prefab.localScale.z)
            {
                distanceToCollision = Vector3.Distance(_unit.transform.position, collision.position);
                yield return new WaitForFixedUpdate();
            }

            _coroutineService.StartCoroutine(StartMove());
        }

        private void OnResetMoving()
        {
            StopMove();
            _eSwipeSide = ESwipeSide.None;
        }
    }
}
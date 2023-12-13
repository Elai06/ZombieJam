using System;
using Gameplay.Enums;
using Infrastructure.Input;
using UnityEngine;

namespace Gameplay.Units.Mover
{
    public class UnitParkingMover : MonoBehaviour, ISwiped
    {

        [SerializeField] private ArrowDirection _arrowDirection;

        [SerializeField] private float _speed = 1.75f;
        [SerializeField] private float _bashBackWard = 0.25f;

        private ESwipeDirection _eSwipeDirection;
        private bool _isMove;
        private bool _inParking = true;

        private ESwipeSide _eSwipeSide = ESwipeSide.None;

        private void FixedUpdate()
        {
            if (_isMove && _inParking)
            {
                SelectDirection(_eSwipeSide);
            }
        }

        public void Swipe(ESwipeSide swipe)
        {
            if (_isMove || swipe == ESwipeSide.None || !IsAvailableSwipe(swipe)) return;

            _eSwipeSide = swipe;
            _isMove = true;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!_isMove) return;
            _isMove = false;

            if (other.gameObject.layer == 3 || other.gameObject.layer == 6)
            {
                Bash(_eSwipeSide, _bashBackWard);
            }

            if (other.gameObject.layer == 7)
            {
                _inParking = false;
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
                    transform.position -= Vector3.back * bashBackWard;
                    break;
                case ESwipeSide.Forward:
                    transform.position -= Vector3.forward * bashBackWard;
                    break;
                case ESwipeSide.Left:
                    transform.position -= Vector3.left * bashBackWard;
                    break;
                case ESwipeSide.Right:
                    transform.position -= Vector3.right * bashBackWard;
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
            transform.position += targetPosition * Time.fixedDeltaTime * _speed;
        }

        public void SetSwipeDirection(ESwipeDirection swipeDirection)
        {
            _eSwipeDirection = swipeDirection;
            _arrowDirection.SetArrowDirection(swipeDirection);
        }

        public void MoveAfterBash()
        {
            if (_inParking)
            {
                _isMove = true;
            }
        }
    }
}
using Gameplay.Enums;
using Infrastructure.Input;
using UnityEngine;

namespace Gameplay.Units.Mover
{
    public class UnitParkingMover : MonoBehaviour, ISwiped
    {
        [SerializeField] private float _speed = 0.5f;
        [SerializeField] private float _bashBackWard = 0.25f;
        [SerializeField] private ESwipeDirection _eSwipeDirection;

        private bool _isMove;
        private bool _inParking = true;

        private ESwipeSide _eSwipeSide = ESwipeSide.None;

        private void FixedUpdate()
        {
            if (_isMove)
            {
                Move(_eSwipeSide, _speed);
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

            if (other.gameObject.layer == 3 || other.gameObject.layer == 6)
            {
                _isMove = false;
                Bash(_eSwipeSide, _bashBackWard);
            }
        }

        private void Move(ESwipeSide eSwipeSide, float speed)
        {
            if (_eSwipeDirection == ESwipeDirection.Vertical)
            {
                switch (eSwipeSide)
                {
                    case ESwipeSide.Back:
                        transform.position += Vector3.back * speed;
                        return;
                    case ESwipeSide.Forward:
                        transform.position += Vector3.forward * speed;
                        return;
                }
            }

            if (_eSwipeDirection == ESwipeDirection.Horizontal)
            {
                switch (eSwipeSide)
                {
                    case ESwipeSide.Left:
                        transform.position += Vector3.left * speed;
                        return;
                    case ESwipeSide.Right:
                        transform.position += Vector3.right * speed;
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

        public void SetSwipeDirection(ESwipeDirection swipeDirection)
        {
            _eSwipeDirection = swipeDirection;
        }
    }
}
using System;
using System.Threading.Tasks;
using Gameplay.Enums;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Portals
{
    public class Portal : MonoBehaviour
    {
        private const int UNIT_LAYER = 3;

        [SerializeField] private Portal _portal;
        [SerializeField] private ESwipeDirection _swipeDirection;
        [SerializeField] private ESwipeSide _swipeSide;

        private bool _isActive = true;

        public ESwipeDirection SwipeDirection => _swipeDirection;

        public ESwipeSide SwipeSide => _swipeSide;

        private void OnCollisionEnter(Collision other)
        {
            if (!_isActive) return;

            if (other.gameObject.layer == UNIT_LAYER)
            {
                var unit = other.gameObject.GetComponent<Unit>();
                Teleport(unit);
            }
        }

        private void Teleport(Unit unit)
        {
            unit.transform.position = _portal.transform.position;

            unit.SetSwipeDirection(_portal.SwipeDirection);
            unit.Swipe(_portal.SwipeSide);

            if (_portal._swipeSide == ESwipeSide.Back)
            {
                unit.transform.position += Vector3.back;
            }

            if (_portal._swipeSide == ESwipeSide.Forward)
            {
                unit.transform.position += Vector3.forward;
            }

            if (_portal._swipeSide == ESwipeSide.Left)
            {
                unit.transform.position += Vector3.left;
            }

            if (_portal._swipeSide == ESwipeSide.Right)
            {
                unit.transform.position += Vector3.right;
            }
        }
    }
}
using System;
using Gameplay.Enums;
using Infrastructure.Input;
using UnityEngine;

namespace Gameplay.Units
{
    public class UserInput : MonoBehaviour
    {
        public event Action<ESwipeSide> Swipe;

        private Vector3 _startPosition;
        private RaycastDetector _raycastDetector = new();

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                var contactInfo = _raycastDetector.RayCast(3);
                if (contactInfo.Transform != null)
                {
                    var swipe = DefineSwipe();
                    contactInfo.Transform.GetComponent<ISwiped>().Swipe(swipe);
                }
            }
        }

        private ESwipeSide DefineSwipe()
        {
            var currentPosition = Input.mousePosition;
            if (_startPosition != currentPosition)
            {
                var deltaPosition = _startPosition - currentPosition;

                if (deltaPosition.x != 0)
                {
                    var swipeSide = deltaPosition.x > 0 ? ESwipeSide.Left : ESwipeSide.Right;
                    Swipe?.Invoke(swipeSide);
                    return swipeSide;
                }

                if (deltaPosition.y != 0)
                {
                    var swipeSide = deltaPosition.y > 0 ? ESwipeSide.Back : ESwipeSide.Forward;
                    Swipe?.Invoke(swipeSide);
                    return swipeSide;
                }
            }

            return ESwipeSide.None;
        }
    }
}
using System;
using Gameplay.Enums;
using Gameplay.Units;
using UnityEngine;

namespace Infrastructure.Input
{
    public class InputManager : MonoBehaviour
    {
        private Vector3 _startPosition;
        private readonly RaycastDetector _raycastDetector = new();

        private ISwiped _swipedObject;

        private bool _isWasSwipe;

        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                _startPosition = UnityEngine.Input.mousePosition;
                SetSwipedObject();
            }

            if (UnityEngine.Input.GetMouseButton(0))
            {
                DetectSwipe();
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                _swipedObject = null;
                _isWasSwipe = false;
            }
        }

        private void DetectSwipe()
        {
            if (_swipedObject != null && !_isWasSwipe)
            {
                var swipe = DefineSwipe();

                if (swipe != ESwipeSide.None)
                {
                    _isWasSwipe = true;
                    _swipedObject.Swipe(swipe);
                }
            }
        }

        private void SetSwipedObject()
        {
            var contactInfo = _raycastDetector.RayCast(3);
            if (contactInfo.Collider == null) return;

            _swipedObject = contactInfo.Collider.GetComponent<ISwiped>();
        }

        private ESwipeSide DefineSwipe()
        {
            var currentPosition = UnityEngine.Input.mousePosition;
            if (_startPosition != currentPosition)
            {
                var deltaPosition = _startPosition - currentPosition;
                var deltaHorizontalSwipe = Math.Abs(_startPosition.x - currentPosition.x);
                var deltaVerticalSwipe = Math.Abs(_startPosition.y - currentPosition.y);

                if (deltaPosition.x != 0 && deltaHorizontalSwipe > deltaVerticalSwipe)
                {
                    var swipeSide = deltaPosition.x > 0 ? ESwipeSide.Left : ESwipeSide.Right;
                    return swipeSide;
                }

                if (deltaPosition.y != 0)
                {
                    var swipeSide = deltaPosition.y > 0 ? ESwipeSide.Back : ESwipeSide.Forward;
                    return swipeSide;
                }
            }


            return ESwipeSide.None;
        }
    }
}
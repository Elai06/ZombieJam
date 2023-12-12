using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Gameplay.Enums;
using Infrastructure.Input;
using UnityEngine;

namespace Gameplay.Units
{
    public class UserInput : MonoBehaviour
    {
        private Vector3 _startPosition;
        private readonly RaycastDetector _raycastDetector = new();

        private ISwiped _swipedObject;

        private bool _isWasSwipe;

        private int _trashHold;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPosition = Input.mousePosition;
                SetSwipedObject();
            }

            if (Input.GetMouseButton(0))
            {
                DetectSwipe();
            }

            if (Input.GetMouseButtonUp(0))
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
            var currentPosition = Input.mousePosition;
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
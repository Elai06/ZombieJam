using System;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay
{
    public class ArrowDirection : MonoBehaviour
    {
        public void SetArrowDirection(ESwipeDirection swipeSide)
        {
            switch (swipeSide)
            {
                case ESwipeDirection.Horizontal:
                    transform.eulerAngles = Vector3.down * 90;
                    break;
                case ESwipeDirection.None:
                    break;
                case ESwipeDirection.Vertical:
                    transform.eulerAngles = Vector3.zero;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeSide), swipeSide, null);
            }
        }
    }
}
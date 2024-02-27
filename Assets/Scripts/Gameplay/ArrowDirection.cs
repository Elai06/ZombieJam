using System;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay
{
    public class ArrowDirection : MonoBehaviour
    {
        public void SetArrowDirection(ESwipeDirection swipeSide, ESwipeSide eSwipeSide)
        {
            switch (swipeSide)
            {
                case ESwipeDirection.Horizontal:
                    transform.eulerAngles = eSwipeSide == ESwipeSide.Right ? Vector3.up * 90 : Vector3.down * 90;
                    break;
                case ESwipeDirection.None:
                    break;
                case ESwipeDirection.Vertical:
                    transform.eulerAngles = eSwipeSide == ESwipeSide.Forward ? Vector3.zero : Vector3.down * 180;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeSide), swipeSide, null);
            }
        }

        public void SetSwipeRotate(ESwipeSide swipeSide)
        {
            if (swipeSide == ESwipeSide.Back)
            {
                transform.eulerAngles = Vector3.down * 180;
            }

            if (swipeSide == ESwipeSide.Forward)
            {
                transform.eulerAngles = Vector3.zero;
            }

            if (swipeSide == ESwipeSide.Left)
            {
                transform.eulerAngles = Vector3.down * 90;
            }

            if (swipeSide == ESwipeSide.Right)
            {
                transform.eulerAngles = Vector3.up * 90;
            }
        }
    }
}
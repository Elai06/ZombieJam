using Gameplay.Enums;
using UnityEngine;

namespace Infrastructure.Input
{
    public struct TutorialSwipeInfo
    {
        public ESwipeDirection SwipeDirection;
        public ESwipeSide SwipeSide;
        public GameObject SwipeGameObject;
        public ISwipeObject UnitSwipe;

        public void Reset()
        {
            SwipeDirection = ESwipeDirection.None;
            SwipeDirection = ESwipeDirection.None;
            SwipeGameObject = null;
            UnitSwipe = null;
        }
    }
}
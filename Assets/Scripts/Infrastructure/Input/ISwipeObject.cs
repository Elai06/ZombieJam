using Gameplay.Enums;

namespace Infrastructure.Input
{
    public interface ISwipeObject
    {
        void Swipe(ESwipeSide swipe);
        ESwipeDirection SwipeDirection { get;}
        void SetSwipeObject();
        void ResetSwipeObject();
    }
}
using Gameplay.Enums;
using Infrastructure.Input;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitSwipe : MonoBehaviour, ISwipeObject
    {
        [SerializeField] private Unit _unit;

        public ESwipeDirection SwipeDirection { get; set; }

        public void Swipe(ESwipeSide swipe)
        {
            _unit.Swipe(swipe);
        }
    }
}
using UnityEngine;

namespace Gameplay.Units
{
    public struct ContactInfo
    {
        public bool IsContacted;
        public Vector3 Point;
        public Collider Collider;
        public Transform Transform;
    }
}
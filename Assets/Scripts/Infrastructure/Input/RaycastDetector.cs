using UnityEngine;

namespace Gameplay.Units
{
    public class RaycastDetector 
    {
        public ContactInfo RayCast(int layer)
        {
            if (Camera.main == null) return new ContactInfo();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(ray, out RaycastHit hitInfo, float.PositiveInfinity, 1 << layer);

            return new ContactInfo
            {
                Collider = hitInfo.collider,
            };
        }
    }
}
using DG.Tweening;
using UnityEngine;

namespace Gameplay.RegionMap
{
    public class RegionCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public void MoveCamera(Transform currentPosition, Transform regionTransform)
        {
            var position = regionTransform.position;
            position.y = transform.position.y;
            position.z -= 9f;
            position.x -= 3.5f;

            if (currentPosition.position != Vector3.zero)
            {
                transform.position = currentPosition.position;
                transform.position = Vector3.up * position.y;
            }

            var distance = Vector3.Distance(transform.position, regionTransform.position);
            _camera.transform.DOMove(position, distance / 20);
        }
    }
}
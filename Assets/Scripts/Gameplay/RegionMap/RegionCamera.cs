using DG.Tweening;
using UnityEngine;

namespace Gameplay.RegionMap
{
    public class RegionCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public void MoveCamera(Transform regionTransform)
        {
            var position = regionTransform.position;
            position.y = transform.position.y;
            position.z -= 5;

            var distance = Vector3.Distance(transform.position, regionTransform.position);
            _camera.transform.DOMove(position, distance / 20);
        }
    }
}
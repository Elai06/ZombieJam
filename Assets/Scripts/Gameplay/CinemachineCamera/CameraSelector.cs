using Cinemachine;
using UnityEngine;

namespace Gameplay.CinemachineCamera
{
    public class CameraSelector : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void ChangeCamera(ECameraType cameraType)
        {
            if (_animator == null)
            {
                _animator = gameObject.GetComponent<Animator>();
            }

            _animator.SetTrigger(cameraType.ToString());
        }
    }
}
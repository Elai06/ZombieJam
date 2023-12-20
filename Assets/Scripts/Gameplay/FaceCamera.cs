using UnityEngine;

namespace Gameplay
{
    public class FaceCamera : MonoBehaviour
    {
        private void Update()
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, -Camera.main.transform.up);
            transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x, 0, 0);
        }
    }
}
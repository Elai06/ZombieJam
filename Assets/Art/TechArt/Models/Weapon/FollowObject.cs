using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform target; 
    [SerializeField] private float smoothSpeed = 0.125f; 

    void FixedUpdate()
    {
        
        Vector3 desiredPosition = target.position;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothedPosition;

        
        Quaternion desiredRotation = target.rotation;
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, desiredRotation, smoothSpeed * Time.fixedDeltaTime);
        transform.rotation = smoothedRotation;
    }
}

using UnityEngine;

public class BoneRotationLock : MonoBehaviour
{
    public Transform bone; 
    public bool lockX, lockY, lockZ; 
    public Vector3 fixedRotation; 

    void LateUpdate()
    {
        if (bone == null) return;

        Vector3 currentRotation = bone.eulerAngles; 
        Vector3 newRotation = new Vector3(
            lockX ? fixedRotation.x : currentRotation.x,
            lockY ? fixedRotation.y : currentRotation.y,
            lockZ ? fixedRotation.z : currentRotation.z);

        bone.eulerAngles = newRotation;
    }
}

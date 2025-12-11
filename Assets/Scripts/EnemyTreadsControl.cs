using UnityEngine;

public class FollowTransformWithAxisLock : MonoBehaviour
{
    [Header("Follow Target")]
    public Transform target;

    [Header("Follow Options")]
    [Tooltip("If true the script will copy the target's position axes (you can toggle which axes below)")]
    public bool followPosition = false;
    [Tooltip("If true the script will copy the target's rotation axes (you can toggle which axes below)")]
    public bool followRotation = true;

    [Header("Position Axes")]
    public bool followPosX = false;
    public bool followPosY = false;
    public bool followPosZ = false;

    [Header("Rotation Axes")]
    public bool followRotX = true;
    public bool followRotY = false;
    public bool followRotZ = false;

    void Update()
    {
        if (target == null)
            return;

        // --- POSITION ---
        if (followPosition)
        {
            Vector3 targetPos = target.position;
            Vector3 currentPos = transform.position;

            float x = followPosX ? targetPos.x : currentPos.x;
            float y = followPosY ? targetPos.y : currentPos.y;
            float z = followPosZ ? targetPos.z : currentPos.z;

            transform.position = new Vector3(x, y, z);
        }

        // --- ROTATION ---
        if (followRotation)
        {
            Vector3 targetRot = target.rotation.eulerAngles;
            Vector3 currentRot = transform.rotation.eulerAngles;

            float x = followRotX ? targetRot.x : currentRot.x;
            float y = followRotY ? targetRot.y : currentRot.y;
            float z = followRotZ ? targetRot.z : currentRot.z;

            transform.rotation = Quaternion.Euler(x, y, z);
        }
    }
}
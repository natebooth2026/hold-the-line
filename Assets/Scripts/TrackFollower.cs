using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple follower for rectangular track meshes.
/// Attach to the track cube and set the tank (root) Transform as the target.
/// The script captures the initial offset on Start (or uses an explicit offset)
/// and then updates the track's world position every frame to maintain that
/// offset relative to the target. You can optionally lock rotation and/or the
/// Y axis so the cube doesn't tilt or move vertically.
/// </summary>
public class TrackFollower : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // assign the tank root here

    [Header("Offset")]
    public bool useManualOffset = false;
    public Vector3 manualOffset = Vector3.zero;

    [Header("Behavior")]
    public bool lockRotation = true; // keep track cube orientation unchanged
    [Tooltip("Lock movement on the X axis (maintain current world X)")]
    public bool lockX = false;
    [Tooltip("Lock movement on the Y axis (maintain current world Y)")]
    public bool lockY = true;       // prevent vertical movement
    [Tooltip("Lock movement on the Z axis (maintain current world Z)")]
    public bool lockZ = false;

    [Header("Rotation Controls")]
    [Tooltip("If true the cube will copy the target's rotation on the given axis; if false that axis stays at the cube's initial rotation")]
    public bool rotateWithTargetX = false;
    public bool rotateWithTargetY = false;
    public bool rotateWithTargetZ = false;

    Vector3 initialOffset;
    Quaternion initialRotation;

    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("TrackFollower: no target assigned.");
            enabled = false;
            return;
        }

        // compute offset in world space
        if (useManualOffset)
        {
            initialOffset = manualOffset;
        }
        else
        {
            initialOffset = transform.position - target.position;
        }

        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // new desired position is target + offset
        Vector3 desired = target.position + initialOffset;

        // Apply per-axis locks so the track cube can be fixed on any axis during testing
        if (lockX) desired.x = transform.position.x;
        if (lockY) desired.y = transform.position.y;
        if (lockZ) desired.z = transform.position.z;

        transform.position = desired;

        if (lockRotation)
        {
            transform.rotation = initialRotation;
        }
        else
        {
            // Compose a rotation that copies target rotation per-axis when requested,
            // otherwise preserves the initial rotation for that axis.
            Vector3 targetEuler = target.eulerAngles;
            Vector3 initEuler = initialRotation.eulerAngles;

            Vector3 newEuler = transform.eulerAngles;

            newEuler.x = rotateWithTargetX ? targetEuler.x : initEuler.x;
            newEuler.y = rotateWithTargetY ? targetEuler.y : initEuler.y;
            newEuler.z = rotateWithTargetZ ? targetEuler.z : initEuler.z;

            transform.eulerAngles = newEuler;
        }
    }
}

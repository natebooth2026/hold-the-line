using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Visual track animator for a tank-like object. Attach to the tank root and
/// assign the left/right track transforms (the visual meshes that should rotate).
///
/// The script computes the linear distance the tank moved along its forward axis
/// and the yaw change (degrees) since the last frame. Those are converted to
/// travel distances for each track (differential drive) and converted into
/// rotation degrees based on the track radius (circumference).
///
/// This keeps the visual tracks animating independently of the body rotation.
/// </summary>
public class TankTracks : MonoBehaviour
{
    [Header("Track Transforms (visuals)")]
    public Transform leftTrack;
    public Transform rightTrack;

    [Header("Track Geometry")]
    [Tooltip("Radius (meters) of the track wheel / drum used to compute rotation from travel distance")]
    public float trackRadius = 0.25f;
    [Tooltip("Distance (meters) between left and right track centers")]
    public float trackBaseWidth = 1.0f;

    [Header("Rotation Axis (local) and tuning")]
    [Tooltip("Local axis of the track transform to rotate (default X axis = (1,0,0)). Change to match your model orientation.")]
    public Vector3 rotationAxis = Vector3.right;

    // optional multiplier fallback if radius is zero
    public float fallbackMultiplier = 360f;

    // internal state
    Vector3 prevPosition;
    float prevYaw;

    void Start()
    {
        prevPosition = transform.position;
        prevYaw = transform.eulerAngles.y;

        // normalize axis to local space usage
        rotationAxis = rotationAxis.normalized;
    }

    void Update()
    {
        // Compute linear movement in world space
        Vector3 deltaPos = transform.position - prevPosition;

        // Project onto local forward to get forward distance (meters)
        float forwardDistance = Vector3.Dot(transform.forward, deltaPos);

        // Compute yaw change (degrees)
        float currYaw = transform.eulerAngles.y;
        float deltaYaw = Mathf.DeltaAngle(prevYaw, currYaw); // degrees

        // Convert deltaYaw (degrees) into an approximate arc distance per track.
        // arcLength = angleRadians * radius_of_turn. For a differential track, radius_of_turn ~ half the base width.
        float angleRad = deltaYaw * Mathf.Deg2Rad;
        float arcPerTrack = angleRad * (trackBaseWidth * 0.5f); // meters

        // Effective travel distances for left and right tracks this frame
        float leftTravel = forwardDistance + arcPerTrack;
        float rightTravel = forwardDistance - arcPerTrack;

        // Convert travel (meters) to rotation degrees: degrees = travel / circumference * 360
        // circumference = 2 * PI * trackRadius
        float leftDegrees = 0f;
        float rightDegrees = 0f;

        if (trackRadius > 0.0001f)
        {
            float degPerMeter = 360f / (2f * Mathf.PI * trackRadius);
            leftDegrees = leftTravel * degPerMeter;
            rightDegrees = rightTravel * degPerMeter;
        }
        else
        {
            // fallback: use a simple multiplier (visual-only)
            leftDegrees = leftTravel * fallbackMultiplier;
            rightDegrees = rightTravel * fallbackMultiplier;
        }

        // Apply rotations to the track visuals. Rotate in local space around rotationAxis.
        if (leftTrack != null)
        {
            leftTrack.localRotation = leftTrack.localRotation * Quaternion.AngleAxis(leftDegrees, rotationAxis);
        }

        if (rightTrack != null)
        {
            rightTrack.localRotation = rightTrack.localRotation * Quaternion.AngleAxis(rightDegrees, rotationAxis);
        }

        // Update previous state
        prevPosition = transform.position;
        prevYaw = currYaw;
    }
}

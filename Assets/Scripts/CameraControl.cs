using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float sensitivity = 200f;
    [SerializeField] float minVerticalAngle = -60f; // Minimum angle, editable in Inspector
    [SerializeField] float maxVerticalAngle = 60f;  // Maximum angle, editable in Inspector
    float xRotation = 0f, mouseX, mouseY;
    [SerializeField] Transform Player;

    private void Update()
    {
        Application.targetFrameRate = 65;

        mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle); // Use both min and max angles

        transform.localRotation = UnityEngine.Quaternion.Euler(xRotation, 0f, 0f);
        Player.Rotate(UnityEngine.Vector3.up * mouseX * 4);
    }
}

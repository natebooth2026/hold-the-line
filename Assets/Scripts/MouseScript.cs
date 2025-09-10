using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    [SerializeField] float sensitivity = 200f;
    [SerializeField] float minVerticalAngle = -60f; // Minimum angle, editable in Inspector
    [SerializeField] float maxVerticalAngle = 60f;  // Maximum angle, editable in Inspector
    float xRotation = 0f, mouseX, mouseY;
    [SerializeField] Transform Player;

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = 65;

        mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle); // Use both min and max angles
        
    transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    Player.Rotate(Vector3.up * mouseX);
    }
}//EndScript
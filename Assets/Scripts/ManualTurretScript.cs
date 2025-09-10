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

    public float projectileSpeed = 5f;

    public List<GameObject> projectiles = new List<GameObject>();

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

        StartCoroutine(projectileLaunch());
        MoveProjectiles(projectiles);
    }

    IEnumerator projectileLaunch() {
        if(Input.GetMouseButtonDown(0)){
            GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            temp.name = "PROJECTILE";
            temp.transform.position = new Vector3(mouseX, mouseY, Player.transform.position.z);
            temp.SetActive(true);
            projectiles.Add(temp);
            yield return new WaitForSeconds(5f);
            projectiles.Remove(temp);
            Destroy(temp);
        }
    }

    void MoveProjectiles(List<GameObject> pList)
    {
        foreach(GameObject p in pList)
        {
            p.transform.Translate(Vector3.forward * projectileSpeed *  Time.deltaTime);
        }

    }
}//EndScript
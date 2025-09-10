using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public bool isTrigger;

    [Range(1, 10)]
    public float speed = 5;

    [Range(10, 50)]
    public float turnSpeed = 20;

    public GameObject light;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) 
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * -turnSpeed *  Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.F) && isTrigger)
        {
            light.SetActive(!light.activeInHierarchy);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger: " + other);
        if(other.tag == "Dead")
        {
            isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Dead")
        {
            isTrigger = false;
        }
    }
}

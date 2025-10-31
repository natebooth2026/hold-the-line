using UnityEngine;

public class AITurretController : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public bool instantAim = false;

    private Transform currentTarget;

    void Update()
    {
        if (currentTarget != null)
        {
            Vector3 direction = currentTarget.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            if (instantAim)
            {
                transform.rotation = targetRotation;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy entered turret range: " + other.name);
            if (currentTarget == null)
                currentTarget = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && other.transform == currentTarget)
        {
            Debug.Log("Enemy left turret range: " + other.name);
            currentTarget = null;
        }
    }
}
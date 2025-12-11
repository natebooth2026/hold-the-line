using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurretController : MonoBehaviour
{
    [Header("Aiming")]
    public float rotationSpeed = 5f;
    public bool instantAim = false;

    [Header("Shooting")]
    public float cooldown = 2f;
    public float projectileSpeed = 10f;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform gun;
    [SerializeField] private Transform tempObjHolder;

    private Transform currentTarget = null;

    private bool shooting = false;
    private const float PROJECTILE_DESTROY_TIME = 5f;

    void Start()
    {
        // Auto-find or auto-create ProjectileHolder
        if (tempObjHolder == null)
        {
            GameObject holder = GameObject.FindWithTag("ProjectileHolder");

            if (holder == null)
            {
                holder = new GameObject("ProjectileHolder");
                holder.tag = "ProjectileHolder";
            }

            tempObjHolder = holder.transform;
        }

        // Debug validations
        if (gun == null) Debug.LogError("[AITurret] Gun reference is NULL!");
        if (projectilePrefab == null) Debug.LogError("[AITurret] Projectile Prefab is NULL!");
    }


    void Update()
    {
        if (currentTarget != null)
        {
            Vector3 direction = currentTarget.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            if (instantAim)
                transform.rotation = targetRotation;
            else
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Trigger detection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && currentTarget == null)
        {
            currentTarget = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (currentTarget == null)
            {
                currentTarget = other.transform; // find target again if lost
            }

            if (!shooting && currentTarget != null)
            {
                StartCoroutine(RepeatShoot());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && other.transform == currentTarget)
        {
            currentTarget = null;
        }
    }

    // Firing loop
    private IEnumerator RepeatShoot()
    {
        shooting = true;

        while (currentTarget != null)
        {
            yield return Shoot();
            yield return new WaitForSeconds(cooldown);
        }

        shooting = false;
    }

    // SHOOT ONCE
    private IEnumerator Shoot()
    {
        if (currentTarget == null)
            yield break;

        if (gun == null)
        {
            Debug.LogError("[AITurret] Gun is NULL, cannot shoot.");
            yield break;
        }

        Vector3 dir = (currentTarget.position - gun.position).normalized;

        GameObject temp = Instantiate(projectilePrefab, gun.position, Quaternion.identity, tempObjHolder);

        Rigidbody tempBody = temp.GetComponent<Rigidbody>();
        if (tempBody != null)
        {
            tempBody.useGravity = false;
            tempBody.drag = 0f;
            tempBody.velocity = dir * projectileSpeed;
        }

        Collider tempCollide = temp.GetComponent<Collider>();
        if (tempCollide != null)
            tempCollide.enabled = true;

        StartCoroutine(DestroyProjectileAfterTime(temp, PROJECTILE_DESTROY_TIME));
    }

    private IEnumerator DestroyProjectileAfterTime(GameObject proj, float time)
    {
        yield return new WaitForSeconds(time);
        if (proj != null) Destroy(proj);
    }
}
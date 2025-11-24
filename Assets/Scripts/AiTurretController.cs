using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Scripting;

public class AITurretController : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public bool instantAim = false;

    private Transform currentTarget;

    private bool shooting = false;
    public float cooldown = 2f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] public Transform tempObjHolder;
    [SerializeField] Transform gun;
    public float projectileSpeed = 10f;
    private const float PROJECTILE_DESTROY_TIME = 5f;

    void Start()
    {
        if(tempObjHolder == null) tempObjHolder = GameObject.FindWithTag("ProjectileHolder").transform;
    }

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
        if (other.CompareTag("Enemy") && currentTarget == null)
        {
            Debug.Log("Enemy entered turret range: " + other.name);
            currentTarget = other.transform;
        }
    }

    private void OnTriggerStay(Collider other){
        if(other.CompareTag("Enemy") && currentTarget != null)
        {
            if(!shooting) StartCoroutine(repeatShoot(cooldown));
        } else if (currentTarget == null)
        {
            OnTriggerEnter(other);
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

     private IEnumerator delayDestroyProjectile(GameObject x, float time)
    {
        yield return new WaitForSeconds(time);
        if (x != null) Destroy(x);
    }

    private IEnumerator Shoot()
    {
        Vector3 dir;

        if(currentTarget != null){
            dir = (currentTarget.position - gun.position).normalized;

            Vector3 tempPos = gun.position;
            GameObject temp = Instantiate(projectilePrefab, tempPos, UnityEngine.Quaternion.identity, tempObjHolder);

            Rigidbody tempBody = temp.GetComponent<Rigidbody>();
            if (tempBody != null)
            {
                tempBody.useGravity = false;
                tempBody.drag = 0f;
                tempBody.velocity = dir * projectileSpeed;
            }

            Collider tempCollide = temp.GetComponent<Collider>();
            tempCollide.enabled = true;

            StartCoroutine(delayDestroyProjectile(temp, PROJECTILE_DESTROY_TIME));
            shooting = false;
            
        }

        yield return null;
    }

    private IEnumerator repeatShoot(float c)
    {
        shooting = true;
        yield return new WaitForSeconds(c);
        StartCoroutine(Shoot());
    }
}
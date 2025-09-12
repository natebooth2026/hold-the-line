using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using UnityEditor.Search;
using UnityEngine;

public class ManualTurret : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform tempObjHolder;
    [SerializeField] bool shoot;
    [SerializeField] GameObject otherGun;
    private ManualTurret otherGunScript;

    public float projectileSpeed = 10f;
    private float maxRayDistance = 100f;

    private bool isShooting = false;

    private void Start()
    {
        otherGunScript = otherGun.GetComponent<ManualTurret>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && shoot == true && !isShooting)
        {
            shoot = false;
            otherGunScript.shoot = true;
            StartCoroutine(ProjectileLaunch());
        }   
    }

    private IEnumerator ProjectileLaunch() {
        isShooting = true;

        bool raycastSuccess = false;
        UnityEngine.Vector3 target = new UnityEngine.Vector3();

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        raycastSuccess = Physics.Raycast(r, maxRayDistance);

        if (raycastSuccess)
        {
            target = r.origin + r.direction * maxRayDistance;

            UnityEngine.Vector3 tempPos = transform.position;

            GameObject temp = Instantiate(projectilePrefab, tempPos, UnityEngine.Quaternion.identity, tempObjHolder);

            UnityEngine.Vector3 dir = (target - temp.transform.position).normalized;

            Rigidbody tempBody = temp.GetComponent<Rigidbody>();
            if (tempBody != null)
            {
                tempBody.useGravity = false;
                tempBody.drag = 0f;
                tempBody.velocity = dir * projectileSpeed;
            }

            //Collider tempCollide = temp.GetComponent<Collider>();
            //tempCollide.enabled = true;
            //if (tempCollide != null)
            //{
            //    if (tempCollide.isTrigger){
            //        Destroy(temp);
            //    }

            //}

            StartCoroutine(delayDestroyProjectile(temp, 5f));

            yield return null;
            isShooting = false;
        }
    }

    private IEnumerator delayDestroyProjectile(GameObject x, float time)
    {
        yield return new WaitForSeconds(time);
        if (x != null) Destroy(x);
    }
    
}//EndScript


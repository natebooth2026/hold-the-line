using System.Collections;
using UnityEngine;

public class ManualTurret : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform tempObjHolder;

    bool shoot;
    private string initShootName = "LeftGun";
    [SerializeField] GameObject otherGun;
    private ManualTurret otherGunScript;

    public float projectileSpeed = 10f;
    private float maxRayDistance = 100f;

    private bool isShooting = false;
    private float shootSwitchBuffer = 1f; //ALLOWS FOR UPGRADES :D
    private const float PROJECTILE_DESTROY_TIME = 5f;

    private void Start()
    {
        otherGunScript = otherGun.GetComponent<ManualTurret>();
        if(this.name == initShootName)
        {
            shoot = true;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && shoot == true && !isShooting && !otherGunScript.isShooting)
        {
            shoot = false;
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

            Collider tempCollide = temp.GetComponent<Collider>();
            tempCollide.enabled = true;

            StartCoroutine(delayDestroyProjectile(temp, PROJECTILE_DESTROY_TIME));

            yield return new WaitForSeconds(shootSwitchBuffer);
            otherGunScript.shoot = true;
            isShooting = false;
        }
    }

    private IEnumerator delayDestroyProjectile(GameObject x, float time)
    {
        yield return new WaitForSeconds(time);
        otherGunScript.shoot = true;
        if (x != null) Destroy(x);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision == null) return;
        else
        {
            Destroy(collision.gameObject);
        }
    }

}//EndScript


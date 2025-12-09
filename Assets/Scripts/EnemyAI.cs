using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Scripting;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] Transform gun, bulletSpawnPoint;
    [SerializeField] GameObject enemyContainer;

    [SerializeField] Transform turret;
    public Transform player;
    private HealthBarManager playerControl;

    public LayerMask whatIsGround, whatIsPlayer;
    
    private float fixedYPosition; // Store the initial Y position

    //Attacking
    public float timeBetweenAttacks;
    private float maxRayDistance = 100f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform tempObjHolder;
    public float projectileSpeed = 10f;
    private const float PROJECTILE_DESTROY_TIME = 5f;
    public float cooldown = 2f;

    //States
    public float attackRange;
    public bool playerInAttackRange;
    private bool shooting = false;
    [SerializeField] public int health;

    //Upgrade Menu Toggle
    [SerializeField] GameObject eventSystem;
    private UpgradeToggle upgradeToggle;
    [Header("Barrel Aim")]
    [Tooltip("Pitch offset (degrees) applied to the gun when aiming. Positive = rotate up.")]
    public float barrelPitchOffset = 0f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    private void Awake()
    {
        // Auto-assign components
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("gunner_attracter").transform;
        eventSystem = GameObject.Find("EventSystem");
        if(tempObjHolder == null)
            tempObjHolder = GameObject.Find("PROJECTILES").transform;

        // Only check fields that MUST be manually assigned on the prefab
        if (gun == null || enemyContainer == null ||
            projectilePrefab == null || tempObjHolder == null)
        {
            Debug.LogError("EnemyAI -- MISSING OBJECT ERROR");
        }

        fixedYPosition = transform.position.y;
        playerControl = player.GetComponentInChildren<HealthBarManager>();
        if (health <= 0) health = 1;

        upgradeToggle = eventSystem.GetComponent<UpgradeToggle>();
        if (turret == null)
        {
            if (transform.childCount > 1)
            {
                turret = transform.GetChild(1);
            } else
            {
                Debug.LogWarning($"{transform.name} has small number of children!");
            }
        }

        if (bulletSpawnPoint == null)
        {
            Debug.LogWarning("Missing bullet spawn point transform");
            bulletSpawnPoint = gun; // fallback to gun position
        }
    }


    private void Update()
    {
        if(health <= 0) //kills the enemy
        {
            Destroy(enemyContainer);
        } else if (!upgradeToggle.activeUpgradeMenu)
        {
            //Check for sight and attack range
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            if (!playerInAttackRange){
                MoveToPlayer();
            } else{
                AttackPlayer();
            }
            
            // Lock Y position to prevent vertical movement
            Vector3 pos = transform.position;
            pos.y = fixedYPosition;
            transform.position = pos;
        } else if (upgradeToggle.activeUpgradeMenu) //freezes the enemy
        {
            agent.SetDestination(transform.position);
        }
    }

    private void MoveToPlayer()
    {
        agent.SetDestination(player.position);
    }

    private IEnumerator delayDestroyProjectile(GameObject x, float time)
    {
        yield return new WaitForSeconds(time);
        if (x != null) Destroy(x);
    }

    private IEnumerator Shoot()
    {
        bool raycastSuccess = false;
        UnityEngine.Vector3 target = new UnityEngine.Vector3();

        Vector3 forward = new Vector3(bulletSpawnPoint.forward.x, bulletSpawnPoint.forward.y, bulletSpawnPoint.forward.z);
        Ray r = new Ray(bulletSpawnPoint.position, forward);
        raycastSuccess = Physics.Raycast(r, maxRayDistance);

        if (raycastSuccess)
        {
            target = r.origin + r.direction * maxRayDistance;

            UnityEngine.Vector3 tempPos = bulletSpawnPoint.position;

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
            shooting = false;
            yield return null;
        }
    }

    private IEnumerator repeatShoot(float c)
    {
        shooting = true;
        yield return new WaitForSeconds(c);
        StartCoroutine(Shoot());
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move by making its speed 0
        agent.speed = 0;
        

        //Attack code here
        if (health > 0 && !shooting)
        {
            StartCoroutine(repeatShoot(cooldown));
        }

        if (player != null)
        {
            // Rotate the turret to face the player
            // turret.LookAt(player);

            // Apply a pitch offset to the gun/barrel so you can tune its elevation angle
            if (gun != null)
            {
                // Direction from gun to player
                Vector3 dir = (player.position - gun.position).normalized;

                // Rotate the direction around the turret's local right axis by the pitch offset
                Vector3 rotatedDir = Quaternion.AngleAxis(barrelPitchOffset, turret.right) * dir;

                // Point the gun along the rotated direction, using turret.up as the world-up reference
                gun.rotation = Quaternion.LookRotation(rotatedDir, turret.up);
            }
        }
    }
}

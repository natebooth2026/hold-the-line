using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] Transform gun;
    [SerializeField] GameObject enemyContainer;

    private Transform turret;
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    private void Awake()
    {
        player = GameObject.Find("TurretSeat").transform;
        agent = GetComponent<NavMeshAgent>();
        fixedYPosition = transform.position.y; // Store the starting Y position
        playerControl = player.GetComponentInChildren<HealthBarManager>();
        if (health <= 0) health = 1;
        upgradeToggle = eventSystem.GetComponent<UpgradeToggle>();

        turret = transform.GetChild(1);
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
            if (!playerInAttackRange) MoveToPlayer();

            if (playerInAttackRange)
            {
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

        Vector3 forward = new Vector3(gun.forward.x, gun.forward.y, gun.forward.z);
        Ray r = new Ray(gun.position, forward);
        raycastSuccess = Physics.Raycast(r, maxRayDistance);

        if (raycastSuccess)
        {
            Debug.Log("MADE IT HERE"); //DEBUG
            target = r.origin + r.direction * maxRayDistance;

            UnityEngine.Vector3 tempPos = gun.position;

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
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        turret.LookAt(player);

        //Attack code here
        if (health > 0 && !shooting)
        {
            StartCoroutine(repeatShoot(cooldown));
        }
    }
}

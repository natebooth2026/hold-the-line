using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;
    
    private float fixedYPosition; // Store the initial Y position

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float attackRange;
    public bool playerInAttackRange;

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
    }

    private void Update()
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
    }

    private void MoveToPlayer()
    {
        agent.SetDestination(player.position);
    }
    

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        //Attack code here
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform enemyToFollow; // Drag the specific enemy object here
    
    private Vector3 offset; // The offset from the enemy
    private bool hasCalculatedOffset = false;
    
    void Start()
    {
        // Only proceed if enemy is manually assigned
        if (enemyToFollow == null)
        {
            Debug.LogWarning("FollowEnemy: No enemy assigned to follow!");
        }
    }

    void Update()
    {
        if (enemyToFollow != null)
        {
            // Calculate offset on first frame
            if (!hasCalculatedOffset)
            {
                offset = transform.position - enemyToFollow.position;
                hasCalculatedOffset = true;
            }
            
            // Follow the enemy maintaining the offset
            transform.position = enemyToFollow.position + offset;
        }
    }
}

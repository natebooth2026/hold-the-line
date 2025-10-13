using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public int playerLayer = 8;
    public int enemyLayer = 9;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision == null) return;
        else
        {
            Destroy(this.gameObject);
            if(collision.gameObject.layer == playerLayer)
            {
                collision.gameObject.GetComponentInChildren<HealthBarManager>().currentHealth -= 1;
            } else if (collision.gameObject.layer == enemyLayer)
            {
                collision.gameObject.GetComponentInChildren<EnemyAI>().health -= 1;
            }
        }
    }
}

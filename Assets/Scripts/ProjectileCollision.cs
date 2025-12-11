using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public KillsTextManager kills;
    private CurrencyHandler currencyHandler;
    private int damageModifier;
    private const int EARNED_CURRENCY = 5;
    private GameObject sfx;
    private AudioSource[] sfxCollection;
    private const int ENEMY_HIT = 0;
    void Awake()
    {
        currencyHandler = GameObject.Find("EventSystem").GetComponent<CurrencyHandler>();
        sfx = GameObject.Find("SFX_SOURCE");
        sfxCollection = sfx.GetComponents<AudioSource>();
    }

    void Update()
    {
        damageModifier = currencyHandler.damageModifier;
    }

    bool IsInLayerMask(Collision obj, LayerMask mask)
    {
        if (obj == null) return false;
        return (mask.value & (1 << obj.gameObject.layer)) != 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectile collided with: " + collision.gameObject.name);
        Debug.Log("(LAYER) Projectile collided with: " + collision.gameObject.layer);

        if (IsInLayerMask(collision, playerLayer))
        {
            HealthBarManager healthBar = FindObjectOfType<HealthBarManager>();
            if (healthBar != null)
            {
                healthBar.currentHealth -= 1;
                healthBar.UpdateHealth(healthBar.currentHealth);
                Debug.Log("Player hit! Current Health: " + healthBar.currentHealth);
            }
            else
            {
                Debug.LogWarning("ProjectileCollision: No HealthBarManager found in the scene!");
            }
        }
        else
        {
            Debug.Log("Not Player Layer!");
        }

        if (IsInLayerMask(collision, enemyLayer))
        {
            EnemyAI enemyAI = collision.gameObject.GetComponentInParent<EnemyAI>();
            if (enemyAI != null)
            {
                if(enemyAI.health - damageModifier <= 0) 
                {
                    if(kills != null) ++kills.kills;
                    currencyHandler.currency += EARNED_CURRENCY;
                }
                enemyAI.health -= damageModifier;
                sfxCollection[ENEMY_HIT].Play();
            }
        }
        else
        {
            Debug.Log("Not Enemy Layer!");
        }

        Destroy(gameObject);
    }
}
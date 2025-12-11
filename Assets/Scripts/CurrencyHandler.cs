using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class CurrencyHandler : MonoBehaviour
{
    public int currency = 100; //100 for testing
    public const int AI_TURRET_PRICE = 30;
    [SerializeField] GameObject AITurretPrefab;
    [SerializeField] Transform AITurretContainer;
    public const int INCREASE_DAMAGE_PRICE = 50;
    public int damageModifier;
    public const int INCREASE_HEALTH_PRICE = 70;
    private const int INCREASE_HEALTH_BY = 35;
    [SerializeField] HealthBarManager healthScript;
    [SerializeField] TextMeshProUGUI currencyCount; 
    [SerializeField] GameObject eventSys;
    [SerializeField] Transform hexMap;   // parent of tiles
    //[SerializeField] Transform turretHolder; // optional: parent for spawned turrets

    int nextSpawnIndex = 1;// start at 1 to skip the first tile which is under the player base

    void Awake()
    {
        damageModifier = 1;
    }
    // Update is called once per frame
    void Update()
    {
        currencyCount.text = currency.ToString();
    }


    public void CreateAITurret()
    {
        if (currency - AI_TURRET_PRICE >= 0)
        {
            currency -= AI_TURRET_PRICE;

            // make sure we don't go past the number of children
            if (nextSpawnIndex >= hexMap.childCount)
            {
                Debug.Log("No more spawn points available!");
                return;
            }

            // get the child transform where the turret should spawn
            Transform spawnPoint = hexMap.GetChild(nextSpawnIndex);

            // spawn the turret at that child's position + rotation
            GameObject newTurret = Instantiate(
                AITurretPrefab,
                spawnPoint.position,
                quaternion.identity
            );

            nextSpawnIndex++; // move to the next tile
        }
    }

    public void IncreaseDamage()
    {
        if(currency - INCREASE_DAMAGE_PRICE >= 0)
        {
            currency -= INCREASE_DAMAGE_PRICE;
            ++damageModifier;
        }
    }

    public void IncreaseHealth()
    {
        if(currency - INCREASE_HEALTH_PRICE >= 0)
        {
            currency -= INCREASE_HEALTH_PRICE;
            healthScript.currentHealth += INCREASE_HEALTH_BY;
        }
    }

    
    // public void CreateAITurret()
    // {
    //     if(currency - AI_TURRET_PRICE >= 0)
    //     {
    //         currency -= AI_TURRET_PRICE;
    //         GameObject newTurret = Instantiate(AITurretPrefab, new Vector3(UnityEngine.Random.Range(-3, 3), 0.0f, UnityEngine.Random.Range(-3, 3)), Quaternion.identity, AITurretContainer);
    //     }
    // }
}

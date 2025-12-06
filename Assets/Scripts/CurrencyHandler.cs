using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyHandler : MonoBehaviour
{
    public int currency = 100; //100 for testing
    public const int AI_TURRET_PRICE = 30;
    [SerializeField] TextMeshProUGUI currencyCount; 
    [SerializeField] GameObject AITurretPrefab;
    [SerializeField] Transform AITurretContainer;
    [SerializeField] GameObject eventSys;
    [SerializeField] Transform bulletHolder;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        currencyCount.text = currency.ToString();
    }

    public void CreateAITurret()
    {
        if(currency - AI_TURRET_PRICE >= 0)
        {
            currency -= AI_TURRET_PRICE;
            GameObject newTurret = Instantiate(AITurretPrefab, new Vector3(UnityEngine.Random.Range(-3, 3), 0.0f, UnityEngine.Random.Range(-3, 3)), Quaternion.identity, AITurretContainer);
        }
    }
}

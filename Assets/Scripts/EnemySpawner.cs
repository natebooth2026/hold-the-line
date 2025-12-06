using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    private List<Transform> spawnPoints = new List<Transform>();

    [Header("Wave Settings")]
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float timeBetweenSpawns = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;

    [SerializeField] private TextMeshProUGUI waveText;
    private int currentWave = 1;
    private bool tick = false;
    private int currentWaveTransitionTimer;
    public bool betweenWave = false;
    [SerializeField] UpgradeToggle upgradeTrackerScript;

    private void Awake()
    {
        // Collect all children as spawn points
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }

        currentWaveTransitionTimer = (int)timeBetweenWaves;
    }

    private void Start()
    {
        UpdateWaveUI();
        StartCoroutine(WaveLoop());
    }

    private void tickTimer()
    {
        --currentWaveTransitionTimer;
        if(currentWaveTransitionTimer == 0) 
        {
            betweenWave = false;
            currentWaveTransitionTimer = (int)timeBetweenWaves;
        }
    }

    private IEnumerator repeatTimer()
    {
        waveText.text = currentWaveTransitionTimer.ToString();
        tick = true;
        yield return new WaitForSeconds(1f);
        tickTimer();
        waveText.text = currentWaveTransitionTimer.ToString();
        tick = false;
    }

    void Update()
    {
        if(betweenWave && !tick 
        && !upgradeTrackerScript.activeUpgradeMenu) 
            StartCoroutine(repeatTimer());
    }

    private void UpdateWaveUI()
    {
        if (waveText != null && !betweenWave)
        {
            waveText.text = "Wave: " + currentWave;
        }
    }


    private IEnumerator WaveLoop()
    {
        while (true)
        {
            UpdateWaveUI();

            if (!betweenWave)
            {
                Debug.Log("Wave " + currentWave + " started!");

                // Spawn this wave's enemies
                for (int i = 0; i < enemiesPerWave; i++)
                {
                    SpawnEnemyAtRandomPoint();
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }

                // Wait until all enemies are dead
                yield return new WaitUntil(() =>
                    GameObject.FindGameObjectsWithTag("Enemy").Length == 0
                );

                betweenWave = true;

                // Increase difficulty
                enemiesPerWave += 2;
                currentWave++;
            } else
            {
                yield return new WaitUntil(() =>
                    betweenWave == false
                );
            }
            
        }
    }

    private void SpawnEnemyAtRandomPoint()
    {
        int index = Random.Range(0, spawnPoints.Count);
        Transform point = spawnPoints[index];

        Instantiate(enemyPrefab, point.position, point.rotation);
    }
}
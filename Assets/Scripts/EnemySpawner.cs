using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnEntry
    {
        public GameObject enemyPrefab;
        [Range(0, 100)] public float spawnWeight = 50f; // Percentage weight for Inspector
    }

    [Header("Enemy Types")]
    [SerializeField] private List<EnemySpawnEntry> enemyTypes = new List<EnemySpawnEntry>();

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
        if (currentWaveTransitionTimer == 0)
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
        if (betweenWave && !tick
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

    // --- NEW: Weighted selection function ---
    private GameObject GetWeightedRandomEnemy()
    {
        float totalWeight = 0f;

        foreach (var entry in enemyTypes)
            totalWeight += entry.spawnWeight;

        float roll = Random.Range(0, totalWeight);
        float cumulative = 0f;

        foreach (var entry in enemyTypes)
        {
            cumulative += entry.spawnWeight;
            if (roll <= cumulative)
                return entry.enemyPrefab;
        }

        return enemyTypes[0].enemyPrefab; // Fallback
    }

    private IEnumerator WaveLoop()
    {
        while (true)
        {
            UpdateWaveUI();

            if (!betweenWave)
            {
                Debug.Log("Wave " + currentWave + " started!");

                // Spawn enemies in this wave
                for (int i = 0; i < enemiesPerWave; i++)
                {
                    SpawnEnemyAtRandomPoint();
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }

                // Wait until all enemies are dead
                yield return new WaitUntil(() =>
                    GameObject.FindGameObjectsWithTag("Enemy").Length == 0 &&
                    GameObject.FindGameObjectsWithTag("InvisibleEnemy").Length == 0
                );

                betweenWave = true;

                // Increase difficulty
                enemiesPerWave += 2;
                currentWave++;
            }
            else
            {
                yield return new WaitUntil(() =>
                    betweenWave == false
                );
            }
        }
    }

    private void SpawnEnemyAtRandomPoint()
    {
        if (enemyTypes.Count == 0)
        {
            Debug.LogError("EnemySpawner has no enemy types assigned!");
            return;
        }

        int index = Random.Range(0, spawnPoints.Count);
        Transform point = spawnPoints[index];

        GameObject selectedEnemy = GetWeightedRandomEnemy();
        Instantiate(selectedEnemy, point.position, point.rotation);
    }
}
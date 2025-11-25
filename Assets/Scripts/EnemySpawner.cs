using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void Awake()
    {
        // Collect all children as spawn points
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }
    }

    private void Start()
    {
        UpdateWaveUI();
        StartCoroutine(WaveLoop());
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
        }
    }


    private IEnumerator WaveLoop()
    {
        while (true)
        {
            UpdateWaveUI();

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

            // Increase difficulty
            enemiesPerWave += 2;
            currentWave++;
        }
    }

    private void SpawnEnemyAtRandomPoint()
    {
        int index = Random.Range(0, spawnPoints.Count);
        Transform point = spawnPoints[index];

        Instantiate(enemyPrefab, point.position, point.rotation);
    }
}
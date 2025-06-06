using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int numberOfZombiesToSpawn = 5;
    private GameObject[] spawnPoints;
    private List<GameObject> activeZombies = new List<GameObject>();

    private int waveNumber = 1;
    private bool waveInProgress = false;
    private float waveDelay = 5f;
    private float waveTimer = 0f;

    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("ZombieSpawn");

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No ZombieSpawn points found!");
            return;
        }

        SpawnZombies();
    }

    void Update()
    {
        // Clean up destroyed zombies
        activeZombies.RemoveAll(z => z == null);

        // If all zombies are dead and we're not already starting a wave
        if (!waveInProgress && activeZombies.Count == 0)
        {
            waveInProgress = true;
            waveTimer = waveDelay;
        }

        if (waveInProgress)
        {
            waveTimer -= Time.deltaTime;
            if (waveTimer <= 0f)
            {
                waveNumber++;
                numberOfZombiesToSpawn += 2; // Increase difficulty per wave
                SpawnZombies();
                waveInProgress = false;
            }
        }
    }

    void SpawnZombies()
    {
        Debug.Log($"Spawning wave {waveNumber} with {numberOfZombiesToSpawn} zombies");

        for (int i = 0; i < numberOfZombiesToSpawn; i++)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex].transform;

            GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
            activeZombies.Add(zombie);
        }
    }
}

using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;      // Assign your zombie prefab in inspector
    public int numberOfZombiesToSpawn = 5;
    private GameObject[] spawnPoints;

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

    void SpawnZombies()
    {
        for (int i = 0; i < numberOfZombiesToSpawn; i++)
        {
            // Pick a random spawn point for each zombie
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex].transform;

            Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log($"Spawned zombie {i+1} at {spawnPoint.position}");
        }
    }
}
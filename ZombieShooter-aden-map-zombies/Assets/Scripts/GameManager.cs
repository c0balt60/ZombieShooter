using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        playerPrefab = GameObject.Find("Player");
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawn");

        if (playerPrefab != null && spawnPoints.Length > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex].transform;

            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Not assigned or couldnt find: " + "This" + " State of prefab " + (playerPrefab != null));
        }
    }
}

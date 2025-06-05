using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawn");
        if (player != null && spawnPoints.Length > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex].transform;

            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Reset motion
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // Teleport using Rigidbody
                rb.position = spawnPoint.position;
                rb.rotation = spawnPoint.rotation;

                Debug.Log("✅ Player teleported to spawn point at: " + spawnPoint.position);
            }
            else
            {
                // Fallback if Rigidbody isn't found
                player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
                Debug.Log("✅ Player moved via Transform to spawn point at: " + spawnPoint.position);
            }
        }
        else
        {
            Debug.LogError("❌ Player or spawn points missing!");
        }
    }
}
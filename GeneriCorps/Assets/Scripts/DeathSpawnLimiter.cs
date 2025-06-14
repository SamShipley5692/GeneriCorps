using UnityEngine;

public class DeathSpawnLimiter : MonoBehaviour
{
    [SerializeField] int maxSpawns = 3;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform[] spawnPoints;

    int playerDeathCount = 0;

    public void OnPlayerDeath()
    {
        playerDeathCount++;

        int spawnCount = Mathf.Clamp(maxSpawns - playerDeathCount, 0, maxSpawns);

        for (int i = 0; i < spawnCount && i < spawnPoints.Length; i++)
        {
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoints[i].position, Quaternion.identity);
        }
    }
}

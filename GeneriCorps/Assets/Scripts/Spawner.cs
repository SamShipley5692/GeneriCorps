using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnedObject;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnRate;
    [SerializeField] Transform[] spawnPOS;

    int spawnCount;

    float spawnTimer;

    bool spawning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager.instance.updateGameGoal(numToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawning)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnRate && spawnCount < numToSpawn)
            {
                spawn();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawning = true;
        }
    }

    void spawn()
    {
        int arrayPOS = Random.Range(0, spawnPOS.Length);
        Instantiate(spawnedObject, spawnPOS[arrayPOS].position, spawnPOS[arrayPOS].rotation);
        spawnCount++;
        spawnTimer = 0;
    }
}

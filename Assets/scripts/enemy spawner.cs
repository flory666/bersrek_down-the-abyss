using UnityEngine;

public class enemyspawner : MonoBehaviour
{
    public Transform player;
    public Transform[] spawnPoints;
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    private bool isPlayerDead = false;
    [SerializeField]
    private int maxEnemies = 15;
    private int maxAliveEnemies = 5;
    private int currentEnemies = 0;
    private int spawnedEnemies = 0;
    public float spawnCooldown = 3f;
    private float spawnTimer = 3f;
    private int[][] enemyAlive = new int[1][];

    private void FixedUpdate()
    {
        if (isPlayerDead)
            return;
        spawnTimer += Time.fixedDeltaTime;

        if (spawnTimer >= spawnCooldown && currentEnemies < maxAliveEnemies && spawnedEnemies < maxEnemies)
        {
            SpawnEnemy();
            spawnTimer = 0f;

            Debug.Log("Spawned Enemy. Current Enemies: "
                + currentEnemies + ", Spawned Enemies: " + spawnedEnemies);
        }
    }
    private void SpawnEnemy()
    {
        int enemyRnd = Random.Range(1, 4);
        if (enemyRnd == 1)
        {
            Instantiate(enemyPrefab2, GetFarthestSpawnPoint().position, Quaternion.identity);
        }
        else
        {
            Instantiate(enemyPrefab1, GetFarthestSpawnPoint().position, Quaternion.identity);
        }
        currentEnemies++;
        spawnedEnemies++;
    }
    Transform GetFarthestSpawnPoint()
    {
        Transform farthest = null;
        float maxDistance = 0f;

        foreach (Transform spawn in spawnPoints)
        {
            float dist = Vector3.Distance(player.position, spawn.position);

            if (dist > maxDistance)
            {
                maxDistance = dist;
                farthest = spawn;
            }
        }

        return farthest;
    }
    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += OnEnemyKilled;
        GameEvents.OnPlayerDied += OnPlayerDied;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= OnEnemyKilled;
        GameEvents.OnPlayerDied -= OnPlayerDied;
    }
    private void OnEnemyKilled()
    {
        Debug.Log("An enemy was killed. Decreasing current enemy count.");
        currentEnemies--;
    }
    private void OnPlayerDied()
    {
        isPlayerDead = true;
    }
}

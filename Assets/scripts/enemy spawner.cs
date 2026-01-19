using System.Net.NetworkInformation;
using UnityEngine;

public class enemyspawner : MonoBehaviour
{
    public Transform player;
    public GameObject[] spawnPoints;
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject CheckSpawn;
    private bool isPlayerDead = false;
    [SerializeField]
    private int maxEnemies = 15;
    private int maxAliveEnemies = 5;
    private int currentEnemies = 0;
    private int spawnedEnemies = 0;
    public float spawnCooldown = 3f;
    private float spawnTimer = 3f;
    private Transform farthest = null;
    private bool check = false;
    private int[][] enemyAlive = new int[1][];

    private void FixedUpdate()
    {
        if (isPlayerDead)
            return;
        spawnTimer += Time.fixedDeltaTime;

        if (spawnTimer >= spawnCooldown && currentEnemies < maxAliveEnemies && spawnedEnemies < maxEnemies)
        {
            SpawnEnemy();
        }
    }
    private void SetTimer(float time)
    {
        spawnTimer = time;
    }
    private void SpawnEnemy()
    {
        int enemyRnd = Random.Range(1, 4);
        GetFarthestSpawnPoint();
        if (check)
        {
            if (enemyRnd == 1)
            {
                Instantiate(enemyPrefab2, farthest.position, Quaternion.identity);
            }
            else
            {
                Instantiate(enemyPrefab1, farthest.position, Quaternion.identity);
            }
            currentEnemies++;
            spawnedEnemies++;
            SetTimer(0f);
        }
    }
    private void GetFarthestSpawnPoint()
    {
        float maxDistance = 0f;

        foreach (GameObject spawn in spawnPoints)
        {
            float dist = Vector3.Distance(player.position, spawn.transform.position);
            if (dist > maxDistance)
            {
                //CheckSpawn spawnCheck = spawn.GetComponent<CheckSpawn>();
                //check = spawn.checkSpawn.verifySpawn();
                maxDistance = dist;
                farthest = spawn.transform;
            }
        }
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

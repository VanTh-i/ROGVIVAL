using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyType> enemyTypes;
        public int waveQuantity; // so luong ke dich co trong luot
        public int spawnCount; // so luong ke dich da spawn
        public int spawnInterval; // gian cach spawn

    }

    [System.Serializable]
    public class EnemyType
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }

    public List<Wave> waves;
    public int currWaveCount;
    protected Transform player;

    [Header("Wave Interval")]
    public float waveInterval; // gian cach moi wave
    private bool isBeginNextWave = false;
    private float spawnTimer;
    public int maxEnemiesAllowed; // so quai duoc phep ton tai cung luc
    [HideInInspector] public int enemiesAlive; // quai co tren san
    [HideInInspector] public bool maxEnemiesReached = false;

    [Header("Spawn Position")]
    public List<Transform> spawnPosition;


    private void Awake()
    {
        player = FindPlayer.GetPlayer();
        if (player == null)
        {
            Debug.LogError("Can not find player");
        }
    }

    private void Start()
    {
        CalculateWaveQuantity();
    }

    private void Update()
    {
        if (currWaveCount < waves.Count && waves[currWaveCount].spawnCount == 0 && !isBeginNextWave)
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= waves[currWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    private void CalculateWaveQuantity()
    {
        int currWaveQuantity = 0;
        foreach (var enemyType in waves[currWaveCount].enemyTypes)
        {
            currWaveQuantity += enemyType.enemyCount;
        }

        waves[currWaveCount].waveQuantity = currWaveQuantity;
    }

    private void SpawnEnemies()
    {
        if (waves[currWaveCount].spawnCount < waves[currWaveCount].waveQuantity && !maxEnemiesReached)
        {
            foreach (var enemyType in waves[currWaveCount].enemyTypes)
            {
                if (enemyType.spawnCount < enemyType.enemyCount)
                {
                    int enemyGroupSize = Random.Range(1, currWaveCount + 3);
                    Vector3 spawnPos = player.position + spawnPosition[Random.Range(0, spawnPosition.Count)].position;
                    for (int i = 0; i < enemyGroupSize; i++)
                    {
                        float offsetMinDist = 1;
                        Vector3 offset = new Vector3(Random.Range(-offsetMinDist, offsetMinDist), Random.Range(-offsetMinDist, offsetMinDist), 0f);
                        spawnPos += offset;

                        Quaternion rot = Quaternion.identity;
                        Transform enemy = EnemySpawn.Instance.Spawn(enemyType.enemyPrefab.transform, spawnPos, rot);
                        enemy.gameObject.SetActive(true);

                        enemyType.spawnCount++;
                        waves[currWaveCount].spawnCount++;
                        enemiesAlive++;

                        if (enemyType.spawnCount >= enemyType.enemyCount)
                        {
                            break; // Đã spawn đủ số lượng kẻ địch của loại này
                        }
                    }

                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                }
            }
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

    private IEnumerator BeginNextWave()
    {
        isBeginNextWave = true;

        yield return new WaitForSeconds(waveInterval);
        if (currWaveCount < waves.Count - 1)
        {
            isBeginNextWave = false;
            currWaveCount++;
            CalculateWaveQuantity();
        }

    }
}

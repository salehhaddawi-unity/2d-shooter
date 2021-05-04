using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    // 0 is ignored
    // X => spawn count
    // Y => spawn delay
    public Vector2[] wavesSpawn;

    public GameObject enemyPrefab = null;

    public float timeBetweenWaves = 5;

    [SerializeField] private SpawnPoint[] spawnPoints;

    // The most recent spawn time
    private float lastSpawnTime = Mathf.Infinity;

    // The number of enemies that have been spawned
    [SerializeField] private int currentlySpawned = 0;

    [SerializeField] private int rememiningSpawned = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = FindObjectsOfType<SpawnPoint>();

        StartCoroutine(nameof(StartNewWave));
    }

    // Update is called once per frame
    void Update()
    {
        CheckSpawnTimer();
    }

    public IEnumerator StartNewWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        lastSpawnTime = Mathf.NegativeInfinity;
        currentlySpawned = 0;
    }

    public void OnEnemyDestroyed()
    {
        rememiningSpawned--;

        if (rememiningSpawned == 0)
        {
            GameManager.instance.NextWave();

            StartCoroutine(nameof(StartNewWave));
        }
    }

    private float GetSpawnDelayForCurrentWave()
    {
        int wave = GameManager.instance.GetWave();

        float delay = wavesSpawn[wave].y;

        return delay;
    }

    private float GetMaxSpawnCountForCurrentWave()
    {
        int wave = GameManager.instance.GetWave();

        float maxCount = wavesSpawn[wave].x;

        return maxCount;
    }

    private void CheckSpawnTimer()
    {
        // If it is time for an enemy to be spawned
        if (Time.timeSinceLevelLoad > lastSpawnTime + GetSpawnDelayForCurrentWave() && (currentlySpawned < GetMaxSpawnCountForCurrentWave()))
        {
            // Determine spawn location
            Vector3 spawnLocation = GetSpawnLocation();

            // Spawn an enemy
            Spawn(spawnLocation);
        }
    }

    private Vector3 GetSpawnLocation()
    {
        var rand = Random.Range(0, spawnPoints.Length);
        return spawnPoints[rand].transform.position;
    }

    private void Spawn(Vector3 spawnLocation)
    {
        // Make sure the prefab is valid
        if (enemyPrefab != null)
        {
            // Create the enemy gameobject
            GameObject enemyGameObject = Instantiate(enemyPrefab, spawnLocation, enemyPrefab.transform.rotation, null);
            /*
            Enemy enemy = enemyGameObject.GetComponent<Enemy>();
            ShootingController[] shootingControllers = enemyGameObject.GetComponentsInChildren<ShootingController>();

            // Setup the enemy if necessary
            if (enemy != null)
            {
                enemy.followTarget = Find;
            }
            foreach (ShootingController gun in shootingControllers)
            {
                gun.projectileHolder = projectileHolder;
            }*/

            // Incremment the spawn count
            currentlySpawned++;
            rememiningSpawned++;
            lastSpawnTime = Time.timeSinceLevelLoad;
        }
    }
}

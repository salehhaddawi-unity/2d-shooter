using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public GameObject enemyPrefab = null;

    [Header("Enemy Count Options")]
    public int mainEnemiesCount = 30;

    public int startEnemiesCount = 6;

    public int wave1To4Increase = 2;

    public int wave5To9Increase = 2;

    public float wave10PlusIncreaseMultiplier = 0.15f;

    [Header("Enemy Health Options")]
    public float startHealth = 3;

    public float healthIncrease = 1;

    [Header("Enemy Speed Options")]
    public float startSpeed = 2;

    public float maxSpeed = 6;

    [Header("Spawn Wave Options")]
    public float timeBetweenWaves = 5;

    [SerializeField] private SpawnPoint[] spawnPoints;

    // The most recent spawn time
    private float lastSpawnTime = Mathf.Infinity;

    // The number of enemies that have been spawned
    [SerializeField] private int spawned = 0;

    [SerializeField] private int rememining = 0;

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
        spawned = 0;
        rememining = GetMaxSpawnCountForCurrentWave();
    }

    public void OnEnemyDestroyed()
    {
        rememining--;

        if (rememining == 0)
        {
            GameManager.instance.NextWave();

            StartCoroutine(nameof(StartNewWave));
        }
    }

    private float GetSpawnDelayForCurrentWave()
    {
        return 1;
    }

    private int GetMaxSpawnCountForCurrentWave()
    {
        int wave = GameManager.instance.GetWave();

        // wave 1 - 4
        if (wave >= 1 || wave <= 4)
        {
            return startEnemiesCount + (wave * wave1To4Increase);
        }

        // wave 5 - 9
        else if (wave >= 5 || wave <= 9)
        {
            return mainEnemiesCount + (wave * wave5To9Increase);
        }

        // wave 10+
        return (int)(wave * wave10PlusIncreaseMultiplier) * mainEnemiesCount;
    }

    private void CheckSpawnTimer()
    {
        // If it is time for an enemy to be spawned
        if (Time.timeSinceLevelLoad > lastSpawnTime + GetSpawnDelayForCurrentWave() && (spawned < GetMaxSpawnCountForCurrentWave()))
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

    private int GetSpawnedEnemyHealth()
    {
        int wave = GameManager.instance.GetWave();

        return (int) (startHealth + (wave * healthIncrease));
    }

    private void Spawn(Vector3 spawnLocation)
    {
        // Make sure the prefab is valid
        if (enemyPrefab != null)
        {
            // Create the enemy gameobject
            GameObject enemyGameObject = Instantiate(enemyPrefab, spawnLocation, enemyPrefab.transform.rotation, null);
            
            Enemy enemy = enemyGameObject.GetComponent<Enemy>();

            Health enemyHealth = enemyGameObject.GetComponent<Health>();
            enemyHealth.maximumHealth = GetSpawnedEnemyHealth();
            enemyHealth.currentHealth = GetSpawnedEnemyHealth();

            // Incremment the spawn count
            spawned++;
            lastSpawnTime = Time.timeSinceLevelLoad;
        }
    }
}

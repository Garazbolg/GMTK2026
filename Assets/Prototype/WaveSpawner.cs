using System;
using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Serializable]
    public struct Wave
    {
        public MobRule[] enemies;
        public int  enemiesCount;
    }
    
    [Serializable]
    public struct MobRule
    {
        public GameObject mobPrefab;
        public float probability;
    }
    
    public Wave[] waves;
    
    public float timeBetweenWaves;
    public float spawnRadius;
    
    private int currentWaveIndex = 0;

    public float timeBeforeFirstWave;
    public float timeBetweenEnemies;

    private float lastWaveTime;
    
    private void Start()
    {
        lastWaveTime = Time.time + timeBeforeFirstWave - timeBetweenWaves;
        currentWaveIndex = 0;
    }

    private void Update()
    {
        if (Time.time - lastWaveTime > timeBetweenWaves)
        {
            lastWaveTime = Time.time;
            StartCoroutine(SpawnNextWave());
        }
    }

    private IEnumerator SpawnNextWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            Wave wave = waves[currentWaveIndex];
            currentWaveIndex++;
            var probabilitySum = 0f;
            foreach (var mobRule in wave.enemies)
            {
                probabilitySum += mobRule.probability;
            }

            for (int i = 0; i < wave.enemiesCount; i++)
            {
                var mobSelection = UnityEngine.Random.value * probabilitySum;
                var currentProbabilitySum = 0f;
                foreach (var mobRule in wave.enemies)
                {
                    currentProbabilitySum += mobRule.probability;
                    if (mobSelection <= currentProbabilitySum)
                    {
                        Vector2 spawnPosition = (Vector2)transform.position + UnityEngine.Random.onUnitCircle * spawnRadius;
                        Instantiate(mobRule.mobPrefab, spawnPosition, Quaternion.identity);
                        break;
                    }
                }
                yield return new WaitForSeconds(timeBetweenEnemies);
            }
        }
    }
}
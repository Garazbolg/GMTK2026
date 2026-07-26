using System;
using System.Collections;
using DevCore.ScriptableVariables;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Serializable]
    public struct Wave
    {
        public MobRule[] enemies;
        public int  enemiesCount;
        public float waveDuration;
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
    public ScriptableInt currentWaveNumber;
    public ScriptableInt mawWaveNumber;

    private float timeStartedLastWave;
    public ScriptableFloat timeBeforeWaveEnd;

    public float timeBeforeFirstWave;
    private Wave currentWave;

    public bool canSpawnInside;
    
    private void Start()
    {
        currentWaveIndex = 0;
        StartCoroutine(SpawnNextWave());
        
        if(currentWaveNumber != null)
            currentWaveNumber.value = currentWaveIndex + 1;
        if(mawWaveNumber != null)
            mawWaveNumber.value = waves.Length;
    }

    private void Update()
    {
        if (currentWaveIndex < waves.Length && timeBeforeWaveEnd != null)
        {
            timeBeforeWaveEnd.value = currentWave.waveDuration - (Time.time - timeStartedLastWave);
        }
    }


    private IEnumerator SpawnNextWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            timeStartedLastWave = Time.time;
            currentWave = waves[currentWaveIndex];
            if(currentWaveNumber != null)
                currentWaveNumber.value = currentWaveIndex;
            var probabilitySum = 0f;
            foreach (var mobRule in currentWave.enemies)
            {
                probabilitySum += mobRule.probability;
            }

            for (int i = 0; i < currentWave.enemiesCount; i++)
            {
                var mobSelection = UnityEngine.Random.value * probabilitySum;
                var currentProbabilitySum = 0f;
                foreach (var mobRule in currentWave.enemies)
                {
                    currentProbabilitySum += mobRule.probability;
                    if (mobSelection <= currentProbabilitySum)
                    {
                        Vector2 offset;
                        if (canSpawnInside)
                        {
                            offset = UnityEngine.Random.insideUnitCircle;
                        }
                        else
                        {
                            offset = UnityEngine.Random.onUnitCircle;
                        }
                        
                        Vector2 spawnPosition = (Vector2)transform.position + offset * spawnRadius;
                        Instantiate(mobRule.mobPrefab, spawnPosition, Quaternion.identity);
                        break;
                    }
                }
                yield return new WaitForSeconds(currentWave.waveDuration / currentWave.enemiesCount);
            }
            ClearAllEnemies();
            yield return new WaitForSeconds(timeBetweenWaves);
            currentWaveIndex++;
            yield return StartCoroutine(SpawnNextWave());
        }
    }
    
    private void ClearAllEnemies()
    {
        var enemies = FindObjectsByType<EnemyController>();
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            enemies[i].GetComponent<Health>().Despawn();
        }
    }
}
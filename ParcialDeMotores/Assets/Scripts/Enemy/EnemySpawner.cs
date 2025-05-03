using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private float spawnInterval = 5f;
        [SerializeField] private EnemyPool enemyPool;
        [SerializeField] private List<EnemySpawnConfig> enemyTypes;

        private void Start()
        {
            StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);

                Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject selectedPrefab = GetRandomEnemyPrefab();
                
                if (selectedPrefab != null)
                {
                    var enemy = enemyPool.GetEnemy(selectedPrefab, randomPoint.position);
                    if (enemy == null)
                    {
                        Debug.LogWarning("[Spawner] No se pudo spawnear enemigo.");
                    }
                }
            }
        }

        private GameObject GetRandomEnemyPrefab()
        {
            float totalWeight = 0f;
            foreach (var config in enemyTypes)
                totalWeight += config.spawnProbability;

            float randomValue = Random.Range(0, totalWeight);
            float current = 0f;

            foreach (var config in enemyTypes)
            {
                current += config.spawnProbability;
                if (randomValue <= current)
                    return config.prefab;
            }

            return enemyTypes.Count > 0 ? enemyTypes[0].prefab : null;
        }
    }
}
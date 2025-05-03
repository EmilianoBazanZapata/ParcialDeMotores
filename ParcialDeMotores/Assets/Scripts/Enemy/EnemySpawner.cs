using System.Collections;
using System.Collections.Generic;
using Enums;
using Managers;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _spawnInterval = 5f;
        [SerializeField] private EnemyPool _enemyPool;
        [SerializeField] private List<EnemySpawnConfig> _enemyTypes;

        private void Start()
        {
            StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);
                
                if (GameManager.Instance.CurrentState != GameState.InGame) continue;

                var randomPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                var selectedPrefab = GetRandomEnemyPrefab();

                if (selectedPrefab == null) continue;
                var enemy = _enemyPool.GetEnemy(selectedPrefab, randomPoint.position);
                if (enemy == null)
                {
                    Debug.LogWarning("[Spawner] No se pudo spawnear enemigo.");
                }
            }
        }

        private GameObject GetRandomEnemyPrefab()
        {
            var totalWeight = 0f;
            foreach (var config in _enemyTypes)
                totalWeight += config.spawnProbability;

            var randomValue = Random.Range(0, totalWeight);
            var current = 0f;

            foreach (var config in _enemyTypes)
            {
                current += config.spawnProbability;
                if (randomValue <= current)
                    return config.prefab;
            }

            return _enemyTypes.Count > 0 ? _enemyTypes[0].prefab : null;
        }
    }
}
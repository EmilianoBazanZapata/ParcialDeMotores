using System.Collections;
using System.Collections.Generic;
using Game.Enemy.States;
using Game.Managers;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.Spawners
{
    /// <summary>
    /// Se encarga de spawnear enemigos en puntos aleatorios a intervalos definidos.
    /// Usa un sistema de probabilidad para seleccionar prefabs desde una pool.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Configuración de Spawneo")]
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _spawnInterval = 5f;
        [SerializeField] private EnemyPool _enemyPool;
        [SerializeField] private List<EnemySpawnConfig> _enemyTypes;

        private void Start()
        {
            StartCoroutine(SpawnLoop());
        }

        /// <summary>
        /// Corrutina que genera enemigos continuamente según el intervalo.
        /// </summary>
        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);

                if (GameManager.Instance.CurrentState != GameState.InGame)
                    continue;

                var spawnPoint = GetRandomSpawnPoint();
                var selectedPrefab = GetRandomEnemyPrefab();

                if (selectedPrefab == null || spawnPoint == null)
                    continue;

                var enemy = _enemyPool.GetEnemy(selectedPrefab, spawnPoint.position);

                if (enemy == null)
                    Debug.LogWarning("[EnemySpawner] No se pudo spawnear enemigo desde la pool.");
            }
        }

        /// <summary>
        /// Devuelve un punto de spawn aleatorio.
        /// </summary>
        private Transform GetRandomSpawnPoint()
        {
            if (_spawnPoints.Length == 0) return null;
            return _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        }

        /// <summary>
        /// Selecciona un prefab de enemigo según su probabilidad.
        /// </summary>
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

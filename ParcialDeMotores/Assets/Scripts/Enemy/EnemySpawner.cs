using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private float spawnInterval = 5f;
        [SerializeField] private EnemyPool enemyPool;

        private void Start()
        {
            Debug.Log("[EnemySpawner] Start ejecutado.");
            StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                Debug.Log("[EnemySpawner] Dentro del SpawnRoutine");
                yield return new WaitForSeconds(spawnInterval);

                Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                var enemy = enemyPool.GetEnemy(randomPoint.position);

                if (enemy == null)
                {
                    Debug.LogWarning("[Spawner] No se pudo spawnear enemigo (pool vacío).");
                }
            }
        }
    }
}
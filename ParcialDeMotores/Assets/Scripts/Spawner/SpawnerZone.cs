using System.Collections;
using UnityEngine;

namespace Spawner
{
    public class SpawnerZone : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject pickupPrefab;
        [SerializeField] private float spawnInterval = 10f;
        [SerializeField] private float playerCheckRadius = 10f;

        private GameObject currentPickup;
        private Transform player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogError("Jugador no encontrado. Asegúrate de que tenga la tag 'Player'.");
                return;
            }

            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);

                if (currentPickup == null)
                {
                    Transform spawnPoint = GetRandomAvailableSpawnPoint();
                    if (spawnPoint != null)
                    {
                        currentPickup = Instantiate(pickupPrefab, spawnPoint.position, Quaternion.identity);
                    }
                }
            }
        }

        private Transform GetRandomAvailableSpawnPoint()
        {
            // Intentamos varias veces buscar un punto alejado del jugador
            for (int i = 0; i < 10; i++)
            {
                Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                if (Vector3.Distance(player.position, randomPoint.position) > playerCheckRadius)
                {
                    return randomPoint;
                }
            }

            // No se encontró un punto suficientemente alejado
            return null;
        }
    }
}
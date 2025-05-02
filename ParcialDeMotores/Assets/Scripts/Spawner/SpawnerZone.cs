using System.Collections;
using PickUps;
using UnityEngine;

namespace Spawner
{
    public class SpawnerZone : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject pickupPrefab;
        [SerializeField] private float spawnInterval = 10f;
        [SerializeField] private float playerCheckRadius = 10f;
        [SerializeField] private PickupPool pickupPool;

        private GameObject currentPickup;
        private Transform player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);

                if (currentPickup != null && currentPickup.activeInHierarchy) continue;
                var spawnPoint = GetRandomAvailableSpawnPoint();

                if (spawnPoint == null) continue;
                
                currentPickup = pickupPool.GetPickup();
                currentPickup.transform.position = spawnPoint.position;
                currentPickup.transform.rotation = Quaternion.identity;

                // Solo si el pool no activa el objeto
                if (!currentPickup.activeInHierarchy)
                    currentPickup.SetActive(true);
            }
        }


        private Transform GetRandomAvailableSpawnPoint()
        {
            // Intentamos varias veces buscar un punto alejado del jugador
            for (int i = 0; i < 10; i++)
            {
                var randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                
                if (Vector3.Distance(player.position, randomPoint.position) > playerCheckRadius)
                    return randomPoint;
            }

            // No se encontró un punto suficientemente alejado
            return null;
        }
    }
}
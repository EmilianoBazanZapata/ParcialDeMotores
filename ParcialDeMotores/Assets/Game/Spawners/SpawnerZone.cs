using System.Collections;
using Game.PickUps;
using UnityEngine;

namespace Game.Spawners
{
    public class SpawnerZone : MonoBehaviour
    {
        [Header("Configuración del Spawner")]
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private GameObject _pickupPrefab;
        [SerializeField] private float _spawnInterval = 10f;
        [SerializeField] private float _playerCheckRadius = 10f;
        [SerializeField] private PickupPool _pickupPool;

        private GameObject _currentPickup;
        private Transform _player;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player")?.transform;
            StartCoroutine(SpawnRoutine());
        }

        /// <summary>
        /// Corrutina que intenta spawnear pickups a intervalos regulares.
        /// </summary>
        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);

                if (_currentPickup != null && _currentPickup.activeInHierarchy)
                    continue;

                var spawnPoint = GetRandomAvailableSpawnPoint();

                if (spawnPoint == null)
                    continue;

                _currentPickup = _pickupPool.GetPickup();
                if (_currentPickup == null)
                    continue;

                _currentPickup.transform.SetPositionAndRotation(spawnPoint.position, Quaternion.identity);

                // Activamos el pickup si el pool no lo hace por defecto
                if (!_currentPickup.activeInHierarchy)
                    _currentPickup.SetActive(true);
            }
        }

        /// <summary>
        /// Obtiene un punto aleatorio que esté suficientemente alejado del jugador.
        /// </summary>
        private Transform GetRandomAvailableSpawnPoint()
        {
            for (int i = 0; i < 10; i++)
            {
                var randomPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

                if (Vector3.Distance(_player.position, randomPoint.position) > _playerCheckRadius)
                    return randomPoint;
            }

            return null;
        }
    }
}

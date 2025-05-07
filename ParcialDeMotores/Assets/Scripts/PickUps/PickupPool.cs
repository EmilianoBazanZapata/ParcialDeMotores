using System.Collections.Generic;
using UnityEngine;

namespace PickUps
{
    public class PickupPool : MonoBehaviour
    {
        [Header("Configuración de la Pool")]
        [SerializeField] private GameObject pickupPrefab;
        [SerializeField] private int poolSize = 2;

        private readonly Queue<GameObject> _pool = new();

        private void Awake() => InitializePool();
        
        /// <summary>
        /// Inicializa la pool creando objetos inactivos desde el prefab.
        /// </summary>
        private void InitializePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                var pickup = CreatePickup();
                _pool.Enqueue(pickup);
            }
        }

        /// <summary>
        /// Instancia un nuevo pickup y lo desactiva.
        /// </summary>
        private GameObject CreatePickup()
        {
            var pickup = Instantiate(pickupPrefab, transform);
            pickup.SetActive(false);
            return pickup;
        }

        /// <summary>
        /// Obtiene un pickup de la pool. Si está vacía, retorna null.
        /// </summary>
        public GameObject GetPickup()
        {
            if (_pool.Count == 0)
            {
                Debug.LogWarning("⚠️ No hay pickups disponibles en la pool.");
                return null;
            }

            var pickup = _pool.Dequeue();
            pickup.SetActive(true);
            return pickup;
        }

        /// <summary>
        /// Devuelve un pickup a la pool y lo desactiva.
        /// </summary>
        public void ReturnPickup(GameObject pickup)
        {
            pickup.SetActive(false);
            _pool.Enqueue(pickup);
        }
    }
}
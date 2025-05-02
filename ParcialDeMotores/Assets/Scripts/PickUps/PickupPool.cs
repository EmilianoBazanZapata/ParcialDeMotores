using System.Collections.Generic;
using UnityEngine;

namespace PickUps
{
    public class PickupPool: MonoBehaviour
    {
        [SerializeField] private GameObject pickupPrefab;
        [SerializeField] private int poolSize = 2;

        private Queue<GameObject> _pool = new Queue<GameObject>();

        private void Awake()
        {
            for (int i = 0; i < poolSize; i++)
            {
                var pickup = Instantiate(pickupPrefab, transform);
                pickup.SetActive(false);
                _pool.Enqueue(pickup);
            }
        }

        public GameObject GetPickup()
        {
            if (_pool.Count <= 0) return null;
            
            var pickup = _pool.Dequeue();
            pickup.SetActive(true);
            
            return pickup;

        }


        public void ReturnPickup(GameObject pickup)
        {
            pickup.SetActive(false);
            _pool.Enqueue(pickup);
        }
    }
}
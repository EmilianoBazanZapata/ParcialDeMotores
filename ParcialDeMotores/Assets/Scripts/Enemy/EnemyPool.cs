using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private int _maxPoolSize = 20;
        private readonly List<Enemy> _pool = new();
        private int _currentCount = 0;

        public Enemy GetEnemy(GameObject prefab, Vector3 spawnPosition)
        {
            foreach (var enemy in _pool)
            {
                if (enemy.gameObject.activeInHierarchy) continue;
                enemy.transform.position = spawnPosition;
                enemy.gameObject.SetActive(true);
                enemy.ResetEnemy();
                return enemy;
            }

            if (_currentCount >= _maxPoolSize)
                return null;
            
            var newObj = Instantiate(prefab, spawnPosition, Quaternion.identity);
            newObj.transform.SetParent(transform); // mantiene jerarquía limpia

            var newEnemy = newObj.GetComponent<Enemy>();
            newEnemy.Initialize(this);
            newEnemy.ResetEnemy();
            _pool.Add(newEnemy);
            _currentCount++;

            return newEnemy;
        }

        public void ReturnEnemy(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}
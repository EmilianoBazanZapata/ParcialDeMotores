using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private int poolSize = 10;

        private Queue<Enemy> pool = new Queue<Enemy>();

        private void Awake()
        {
            for (int i = 0; i < poolSize; i++)
            {
                Enemy enemy = Instantiate(enemyPrefab, transform);
                enemy.Initialize(this);
                enemy.gameObject.SetActive(false);
                pool.Enqueue(enemy);
            }
        }

        public Enemy GetEnemy(Vector3 position)
        {
            if (pool.Count <= 0) return null;
            var enemy = pool.Dequeue();
            enemy.transform.position = position;
            enemy.gameObject.SetActive(true);
            return enemy;
        }

        public void ReturnEnemy(Enemy enemy)
        {
            enemy.ResetEnemy();
            enemy.gameObject.SetActive(false);
            pool.Enqueue(enemy);
        }
    }

}
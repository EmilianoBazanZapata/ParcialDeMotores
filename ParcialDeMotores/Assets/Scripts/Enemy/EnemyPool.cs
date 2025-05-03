using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private int maxPoolSize = 20;
        [SerializeField] private GameObject defaultEnemyPrefab;

        private readonly List<Enemy> pool = new();
        private int currentCount = 0;

        private void Start()
        {
            // Pre-instancia enemigos si querés que el pool arranque con objetos disponibles
            for (int i = 0; i < maxPoolSize; i++)
            {
                GameObject obj = Instantiate(defaultEnemyPrefab, transform);
                obj.SetActive(false);

                Enemy enemy = obj.GetComponent<Enemy>();
                enemy.Initialize(this);
                pool.Add(enemy);
                currentCount++;
            }
        }

        public Enemy GetEnemy(GameObject prefab, Vector3 spawnPosition)
        {
            foreach (var enemy in pool)
            {
                if (!enemy.gameObject.activeInHierarchy)
                {
                    enemy.transform.position = spawnPosition;
                    enemy.gameObject.SetActive(true);
                    enemy.ResetEnemy();
                    return enemy;
                }
            }

            if (currentCount >= maxPoolSize)
            {
                Debug.LogWarning("[EnemyPool] Límite máximo alcanzado, no se crearán más enemigos.");
                return null;
            }

            GameObject newObj = Instantiate(prefab, spawnPosition, Quaternion.identity);
            newObj.transform.SetParent(transform); // mantiene jerarquía limpia

            Enemy newEnemy = newObj.GetComponent<Enemy>();
            newEnemy.Initialize(this);
            newEnemy.ResetEnemy();
            pool.Add(newEnemy);
            currentCount++;

            return newEnemy;
        }

        public void ReturnEnemy(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}
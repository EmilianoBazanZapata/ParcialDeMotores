using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Sistema de pool para enemigos.
    /// Reutiliza instancias para evitar overhead de instanciación/destrucción.
    /// </summary>
    public class EnemyPool : MonoBehaviour
    {
        [Header("Configuración del Pool")]
        [SerializeField] private int _maxPoolSize = 20;

        private readonly List<Enemy> _pool = new();
        private int _currentCount = 0;

        /// <summary>
        /// Devuelve un enemigo disponible del pool, o crea uno nuevo si hay capacidad.
        /// </summary>
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
            newObj.transform.SetParent(transform); // para organización en jerarquía

            var newEnemy = newObj.GetComponent<Enemy>();
            newEnemy.Initialize(this);
            newEnemy.ResetEnemy();

            _pool.Add(newEnemy);
            _currentCount++;

            return newEnemy;
        }

        /// <summary>
        /// Devuelve un enemigo al pool (lo desactiva).
        /// </summary>
        public void ReturnEnemy(Enemy enemy) => enemy.gameObject.SetActive(false);
    }
}
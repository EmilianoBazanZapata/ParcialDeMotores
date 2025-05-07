using System.Collections.Generic;
using UnityEngine;

namespace Game.Bullet
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField] private Game.Bullet.Bullet bulletPrefab;
        [SerializeField] private int initialSize = 10;

        private Queue<Game.Bullet.Bullet> _pool = new();

        private void Awake()
        {
            for (int i = 0; i < initialSize; i++)
            {
                var bullet = Instantiate(bulletPrefab, transform);
                bullet.gameObject.SetActive(false);
                _pool.Enqueue(bullet);
            }
        }

        public Game.Bullet.Bullet GetBullet()
        {
            Game.Bullet.Bullet bullet = _pool.Count > 0 ? _pool.Dequeue() : Instantiate(bulletPrefab, transform);
            return bullet;
        }

        public void ReturnBullet(Game.Bullet.Bullet bullet)
        {
            _pool.Enqueue(bullet);
        }
    }
}
using UnityEngine;

namespace Bullet
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 20f;
        public float lifetime = 2f;

        private Vector3 _direction;
        private float _timer;
        private System.Action<Bullet> _onReturnToPool;

        public void Initialize(Vector3 direction, System.Action<Bullet> returnToPool)
        {
            _direction = direction.normalized;
            _onReturnToPool = returnToPool;
            _timer = 0f;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            transform.position += _direction * speed * Time.deltaTime;
            _timer += Time.deltaTime;

            if (_timer >= lifetime)
                ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("Enemy hit: " + other.name);
                // Podés aplicar daño aquí
            }

            ReturnToPool();
        }

        private void ReturnToPool()
        {
            gameObject.SetActive(false);
            _onReturnToPool?.Invoke(this);
        }
    }
}
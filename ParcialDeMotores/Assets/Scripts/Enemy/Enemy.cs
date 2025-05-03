using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class Enemy: MonoBehaviour
    {
        public int maxHealth = 50;
        private int currentHealth;

        public float stunDuration = 0.3f;
        private bool isStunned = false;
        [SerializeField]private NavMeshAgent agent;
        [SerializeField] private float damageInterval = 2.5f;
        private float lastDamageTime = -Mathf.Infinity;
        
        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (isStunned) return;

            currentHealth -= damage;
            Debug.Log($"Enemigo recibe {damage} de daño. Vida restante: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
                return;
            }

            StartCoroutine(StunCoroutine());
        }
        
        private IEnumerator StunCoroutine()
        {
            isStunned = true;
            agent.isStopped = true;
            yield return new WaitForSeconds(stunDuration);
            agent.isStopped = false;
            isStunned = false;
        }

        private void Die()
        {
            Debug.Log("[Enemy] ¡Muerto!");
            Destroy(gameObject); // o return al pool si usás pooling
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (Time.time >= lastDamageTime + damageInterval)
                {
                    var player = other.GetComponent<Player.Player>();
                    if (player != null)
                    {
                        player.TakeDamage(25);
                        lastDamageTime = Time.time;
                        Debug.Log("El jugador fue dañado por contacto con el enemigo");
                    }
                }
            }
        }
    }
}
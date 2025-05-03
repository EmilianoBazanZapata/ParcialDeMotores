using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class EnemyAI: MonoBehaviour
    {
        [SerializeField] private float detectionRadius = 10f;
        [SerializeField] private LayerMask playerLayer;

        private NavMeshAgent agent;
        private Transform player;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        private void Update()
        {
            if (player == null) return;

            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= detectionRadius)
            {
                agent.SetDestination(player.position);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}
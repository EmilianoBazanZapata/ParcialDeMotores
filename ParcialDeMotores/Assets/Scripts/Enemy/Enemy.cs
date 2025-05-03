using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        [Header("Stats")] public int maxHealth = 50;
        public float damageInterval = 2.5f;
        public float stunDuration = 0.3f;
        public float attackRange = 2f;
        public float detectionRadius = 10f;

        [Header("Referencias")] public Transform Player;
        public NavMeshAgent Agent;
        [SerializeField] private LayerMask playerLayer;

        private int _currentHealth;
        private float _lastDamageTime;
        public bool IsStunned;
        public EnemyPool Pool;

        public Animator Animator { get; private set; }

        #region States

        public EnemyStateMachine StateMachine { get; private set; }
        public EnemyIdleState IdleState { get; private set; }
        public ChaseState ChaseState { get; private set; }
        public EnemyAttackState AttackState { get; private set; }
        public EnemyDeadState DeadState { get; private set; }

        #endregion

        public void Initialize(EnemyPool enemyPool)
        {
            Pool = enemyPool;
        }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            Player = GameObject.FindGameObjectWithTag("Player")?.transform;
            StateMachine = new EnemyStateMachine();
            IdleState = new EnemyIdleState(this, StateMachine, "Idle");
            ChaseState = new ChaseState(this, StateMachine, "Chase");
            DeadState = new EnemyDeadState(this, StateMachine, "Die");
            AttackState = new EnemyAttackState(this, StateMachine, "Attack");
        }

        private void Start()
        {
            _currentHealth = maxHealth;
            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            StateMachine.CurrentState?.Update();
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                StateMachine.ChangeState(DeadState);
                return;
            }

            StartCoroutine(StunCoroutine());
        }

        private IEnumerator StunCoroutine()
        {
            IsStunned = true;
            Agent.isStopped = true;
            yield return new WaitForSeconds(stunDuration);
            Agent.isStopped = false;
            IsStunned = false;
        }

        public bool IsPlayerInAttackRange()
        {
            return Player != null && Vector3.Distance(transform.position, Player.position) <= attackRange;
        }

        public bool IsPlayerDetected()
        {
            if (Player == null) return false;
            var distance = Vector3.Distance(transform.position, Player.position);
            return distance <= detectionRadius && Physics.CheckSphere(transform.position, detectionRadius, playerLayer);
        }

        public void MoveToPlayer()
        {
            if (Player != null && Agent.enabled)
                Agent.SetDestination(Player.position);
        }

        public void ResetEnemy()
        {
            _currentHealth = maxHealth;
            IsStunned = false;
            Agent.isStopped = false;
            Animator.Rebind();
            StateMachine.ChangeState(IdleState);
        }
    }
}
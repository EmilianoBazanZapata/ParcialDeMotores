using System.Collections;
using Game.Enemy.States;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        [Header("Estadísticas")] [SerializeField]
        private int _maxHealth = 50;

        [SerializeField] private float _damageInterval = 2.5f;
        [SerializeField] private float _stunDuration = 0.3f;
        [SerializeField] private float _attackRange = 2f;
        [SerializeField] private float _detectionRadius = 10f;
        [SerializeField] private int _damage = 10;
        
        public int Damage => _damage;
        public float DamageInterval => _damageInterval;

        [Header("Referencias")] [SerializeField]
        private LayerMask _playerLayer;

        public Transform Player { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public EnemyPool Pool { get; private set; }
        public Animator Animator { get; private set; }

        private int _currentHealth;
        private float _lastDamageTime;
        public bool IsStunned { get; private set; }

        #region Máquina de estados

        public EnemyStateMachine StateMachine { get; private set; }
        public EnemyIdleState IdleState { get; private set; }
        public EnemyChaseState EnemyChaseState { get; private set; }
        public EnemyAttackState AttackState { get; private set; }
        public EnemyDeadState DeadState { get; private set; }

        #endregion

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            Player = GameObject.FindGameObjectWithTag("Player")?.transform;

            StateMachine = new EnemyStateMachine();
            IdleState = new EnemyIdleState(this, StateMachine, "Idle");
            EnemyChaseState = new EnemyChaseState(this, StateMachine, "Chase");
            AttackState = new EnemyAttackState(this, StateMachine, "Attack");
            DeadState = new EnemyDeadState(this, StateMachine, "Die");
        }

        private void Start()
        {
            _currentHealth = _maxHealth;
            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            StateMachine.CurrentState?.Update();
        }

        public void Initialize(EnemyPool enemyPool) => Pool = enemyPool;

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
            yield return new WaitForSeconds(_stunDuration);
            Agent.isStopped = false;
            IsStunned = false;
        }

        public bool IsPlayerInAttackRange() =>
            Player != null && Vector3.Distance(transform.position, Player.position) <= _attackRange;

        public bool IsPlayerDetected()
        {
            if (Player == null) return false;

            var distance = Vector3.Distance(transform.position, Player.position);
            return distance <= _detectionRadius &&
                   Physics.CheckSphere(transform.position, _detectionRadius, _playerLayer);
        }

        public void MoveToPlayer()
        {
            if (Player != null && Agent.enabled)
                Agent.SetDestination(Player.position);
        }

        public void ResetEnemy()
        {
            _currentHealth = _maxHealth;
            IsStunned = false;
            Agent.isStopped = false;
            Animator.Rebind();
            StateMachine.ChangeState(IdleState);
        }
    }
}
using Enums;
using Managers;
using UnityEngine;

namespace Enemy
{
    public class EnemyAttackState: EnemyState
    {
        private Enemy _enemy;
        private float _lastAttackTime;

        public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
            : base(enemy, stateMachine, animBoolName)
        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();
            _enemy.Agent.isStopped = true;
        }

        public override void Exit()
        {
            base.Exit();
            _enemy.Agent.isStopped = false;
        }

        public override void Update()
        {
            base.Update();

            if (!_enemy.IsPlayerInAttackRange())
            {
                StateMachine.ChangeState(_enemy.ChaseState);
                return;
            }

            if (!(Time.time - _lastAttackTime > _enemy.DamageInterval)) return;
            _lastAttackTime = Time.time;
            _enemy.Animator.SetTrigger("Attack");
            
            if (!_enemy.Player.TryGetComponent<Player.Player>(out var player)) return;
            player.TakeDamage(_enemy.Damage);
            SoundManager.Instance.PlaySound(SoundType.ZombieAttack);
        }
    }
}
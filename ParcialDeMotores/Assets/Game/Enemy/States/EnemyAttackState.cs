using Enemy;
using Game.Managers;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.Enemy.States
{
    public class EnemyAttackState: EnemyState
    {
        private global::Enemy.Enemy _enemy;
        private float _lastAttackTime;

        public EnemyAttackState(global::Enemy.Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
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
                StateMachine.ChangeState(_enemy.EnemyChaseState);
                return;
            }

            if (!(Time.time - _lastAttackTime > _enemy.DamageInterval)) return;
            _lastAttackTime = Time.time;
            _enemy.Animator.SetTrigger("Attack");
            
            if (!_enemy.Player.TryGetComponent<Game.Player.Player>(out var player)) return;
            player.TakeDamage(_enemy.Damage);
            SoundManager.Instance.PlaySound(SoundType.ZombieAttack);
        }
    }
}
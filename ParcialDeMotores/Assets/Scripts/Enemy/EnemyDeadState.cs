using UnityEngine;

namespace Enemy
{
    public class EnemyDeadState : EnemyState
    {
        private readonly Enemy _enemy;

        public EnemyDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
            : base(enemy, stateMachine, animBoolName)
        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            _enemy.Agent.isStopped = true;
            _enemy.Animator.SetTrigger("Die");
            _enemy.StartCoroutine(WaitAndDestroy());
        }

        private System.Collections.IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(1.3f); // Tiempo de animación
            _enemy.Pool.ReturnEnemy(_enemy);
        }
    }
}
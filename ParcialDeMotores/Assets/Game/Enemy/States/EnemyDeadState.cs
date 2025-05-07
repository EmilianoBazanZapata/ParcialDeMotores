using Enemy;
using Game.Managers;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.Enemy.States
{
    /// <summary>
    /// Estado que se activa cuando el enemigo muere.
    /// Reproduce animación y sonido, y luego devuelve el enemigo a la pool.
    /// </summary>
    public class EnemyDeadState : EnemyState
    {
        private readonly global::Enemy.Enemy _enemy;

        public EnemyDeadState(global::Enemy.Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
            : base(enemy, stateMachine, animBoolName)
        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            SoundManager.Instance.PlaySound(SoundType.ZombieDeath);

            _enemy.Agent.isStopped = true;
            _enemy.Animator.SetTrigger("Die");

            _enemy.StartCoroutine(WaitAndReturnToPool());
        }

        /// <summary>
        /// Espera a que finalice la animación y devuelve el enemigo al pool.
        /// </summary>
        private System.Collections.IEnumerator WaitAndReturnToPool()
        {
            yield return new WaitForSeconds(1.3f); // Tiempo estimado de la animación
            _enemy.Pool.ReturnEnemy(_enemy);
        }
    }
}
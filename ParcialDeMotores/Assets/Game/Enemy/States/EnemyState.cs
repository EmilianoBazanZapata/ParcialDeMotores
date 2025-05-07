using Enemy;
using Game.Shared.Interfaces;

namespace Game.Enemy.States
{
    /// <summary>
    /// Clase base para todos los estados del enemigo.
    /// Define el ciclo de vida del estado y su animación asociada.
    /// </summary>
    public class EnemyState : IState
    {
        protected readonly EnemyStateMachine StateMachine;
        protected readonly global::Enemy.Enemy Enemy;
        protected readonly string AnimBoolName;

        public EnemyState(global::Enemy.Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
        {
            Enemy = enemy;
            StateMachine = stateMachine;
            AnimBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            Enemy.Animator.SetBool(AnimBoolName, true);
        }

        public virtual void Update() { }

        public virtual void Exit()
        {
            Enemy.Animator.SetBool(AnimBoolName, false);
        }
    }
}
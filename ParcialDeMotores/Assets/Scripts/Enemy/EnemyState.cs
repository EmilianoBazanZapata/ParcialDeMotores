using Player;

namespace Enemy
{
    public class EnemyState : IState
    {
        protected readonly EnemyStateMachine StateMachine;
        protected readonly Enemy Enemy;
        protected readonly string AnimBoolName;

        public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
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
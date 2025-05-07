using Enemy;

namespace Game.Enemy.States
{
    public class EnemyIdleState : EnemyState
    {
        private readonly global::Enemy.Enemy _enemy;

        public EnemyIdleState(global::Enemy.Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
            : base(enemy, stateMachine, animBoolName)
        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();
            _enemy.Agent.isStopped = true;
        }

        public override void Update()
        {
            base.Update();

            if (_enemy.IsPlayerDetected())
                StateMachine.ChangeState(_enemy.EnemyChaseState);
        }

        public override void Exit()
        {
            base.Exit();
            _enemy.Agent.isStopped = false;
        }
    }
}
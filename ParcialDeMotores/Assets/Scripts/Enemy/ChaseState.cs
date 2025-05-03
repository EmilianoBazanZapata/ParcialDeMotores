namespace Enemy
{
    public class ChaseState : EnemyState
    {
        private Enemy _enemy;

        public ChaseState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
            : base(enemy, stateMachine, animBoolName)
        {
            _enemy = enemy;
        }

        public override void Update()
        {
            base.Update();

            if (_enemy.IsStunned) return;

            _enemy.MoveToPlayer();

            if (_enemy.IsPlayerInAttackRange())
            {
                StateMachine.ChangeState(_enemy.AttackState);
                return;
            }

            if (!_enemy.IsPlayerDetected())
            {
                StateMachine.ChangeState(_enemy.IdleState);
            }
        }
    }
}
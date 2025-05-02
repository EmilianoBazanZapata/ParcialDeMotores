namespace Player
{
    public class PlayerMoveLeftState : PlayerState
    {
        private readonly Player _player;

        public PlayerMoveLeftState(Player player, PlayerStateMachine stateMachine, string animBoolName)
            : base(player, stateMachine, animBoolName)
        {
            _player = player;
        }

        public override void Update()
        {
            base.Enter();
            
            base.Update();

            var input = _player.GetInputDirection();

            SetIdleState(input);
            
            HandleDirectionalStateChange();
            
            _player.Move(input);

            _player.RotateTowardsMouse();
        }
    }
}
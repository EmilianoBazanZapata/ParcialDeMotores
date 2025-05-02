namespace Player
{
    public class PlayerMoveBackwardsState : PlayerState
    {
        private readonly Player _player;

        public PlayerMoveBackwardsState(Player player, PlayerStateMachine stateMachine, string animBoolName)
            : base(player, stateMachine, animBoolName)
        {
            _player = player;
        }
        
        public override void Update()
        {
            base.Update();

            var input = _player.GetInputDirection();
            
            SetIdleState(input);
            
            HandleDirectionalStateChange();

            _player.Move(input);

            _player.RotateTowardsMouse();
        }
    }
}

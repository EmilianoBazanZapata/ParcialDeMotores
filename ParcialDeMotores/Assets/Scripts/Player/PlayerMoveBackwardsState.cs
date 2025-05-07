namespace Player
{
    /// <summary>
    /// Estado de movimiento hacia atrás del jugador.
    /// Permite transición a idle o a otros estados direccionales.
    /// </summary>
    public class PlayerMoveBackwardsState : PlayerState
    {
        private readonly Player _player;

        public PlayerMoveBackwardsState(Player player, 
                                        PlayerStateMachine stateMachine, 
                                        string animBoolName) : base(player, stateMachine, animBoolName)
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
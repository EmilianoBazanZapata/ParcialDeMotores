namespace Player
{
    /// <summary>
    /// Estado de movimiento lateral hacia la derecha del jugador.
    /// Transiciona según la dirección del input.
    /// </summary>
    public class PlayerMoveRightState : PlayerState
    {
        private readonly Player _player;

        public PlayerMoveRightState(Player player, 
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
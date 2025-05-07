namespace Game.Player.States
{
    /// <summary>
    /// Estado de movimiento lateral hacia la izquierda del jugador.
    /// Transiciona según la dirección del input.
    /// </summary>
    public class PlayerMoveLeftState : PlayerState
    {
        private readonly Game.Player.Player _player;

        public PlayerMoveLeftState(Game.Player.Player player, 
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
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Estado de movimiento hacia adelante del jugador.
    /// Controla la transición a idle u otros estados direccionales.
    /// </summary>
    public class PlayerMoveState : PlayerState
    {
        private readonly Player _player;

        public PlayerMoveState(Player player, 
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
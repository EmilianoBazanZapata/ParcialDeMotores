using UnityEngine;

namespace Game.Player.States
{
    /// <summary>
    /// Estado del jugador cuando está en reposo (idle).
    /// Cambia al estado de movimiento si se detecta input.
    /// </summary>
    public class PlayerIdleState : PlayerState
    {
        private readonly Game.Player.Player _player;

        public PlayerIdleState(Game.Player.Player player, 
                               PlayerStateMachine stateMachine, 
                               string animBoolName) : base(player, stateMachine, animBoolName)
        {
            _player = player;
        }

        public override void Update()
        {
            base.Update();

            var input = _player.GetInputDirection();

            if (input != Vector3.zero)
                _player.StateMachine.ChangeState(_player.MoveState);
        }
    }
}
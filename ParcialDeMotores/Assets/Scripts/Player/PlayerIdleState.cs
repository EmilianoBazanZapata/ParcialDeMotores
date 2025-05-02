using UnityEngine;

namespace Player
{
    public class PlayerIdleState : PlayerState
    {
        private readonly Player _player;

        public PlayerIdleState(Player player, 
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
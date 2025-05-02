using UnityEngine;

namespace Player
{
    public class PlayerDeadState: PlayerState
    {
        private readonly Player _player;

        public PlayerDeadState(Player player, PlayerStateMachine stateMachine, string animBoolName)
            : base(player, stateMachine, animBoolName)
        {
            _player = player;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("💀 Entrando en estado de muerte");
        }

        public override void Update()
        {
            // Nada. Está muerto.
        }

        public override void Exit()
        {
            base.Exit(); // por si querés reiniciar luego
        }
    }
}
using Game.Shared.Interfaces;
using UnityEngine;

namespace Game.Player.States
{
    /// <summary>
    /// Clase base para todos los estados del jugador.
    /// Controla las transiciones, animaciones y lógica compartida.
    /// </summary>
    public class PlayerState : IState
    {
        protected readonly PlayerStateMachine StateMachine;
        protected readonly Game.Player.Player Player;
        protected readonly string AnimBoolName;

        public PlayerState(Game.Player.Player player, PlayerStateMachine stateMachine, string animBoolName)
        {
            Player = player;
            StateMachine = stateMachine;
            AnimBoolName = animBoolName;
        }

        public virtual void Enter() => Player.Animator.SetBool(AnimBoolName, true);

        public virtual void Update()
        {
        }

        public virtual void Exit() => Player.Animator.SetBool(AnimBoolName, false);

        /// <summary>
        /// Cambia el estado según la dirección de movimiento detectada.
        /// </summary>
        protected void HandleDirectionalStateChange()
        {
            if (Input.GetKey(KeyCode.W) && StateMachine.CurrentState != Player.MoveState)
                StateMachine.ChangeState(Player.MoveState);
            else if (Input.GetKey(KeyCode.S) && StateMachine.CurrentState != Player.MoveBackwardsState)
                StateMachine.ChangeState(Player.MoveBackwardsState);
            else if (Input.GetKey(KeyCode.A) && StateMachine.CurrentState != Player.MoveLeftState)
                StateMachine.ChangeState(Player.MoveLeftState);
            else if (Input.GetKey(KeyCode.D) && StateMachine.CurrentState != Player.MoveRightState)
                StateMachine.ChangeState(Player.MoveRightState);
        }

        /// <summary>
        /// Cambia a estado Idle si no hay input de movimiento.
        /// </summary>
        protected void SetIdleState(Vector3 input)
        {
            if (input == Vector3.zero)
                StateMachine.ChangeState(Player.IdleState);
        }
    }
}
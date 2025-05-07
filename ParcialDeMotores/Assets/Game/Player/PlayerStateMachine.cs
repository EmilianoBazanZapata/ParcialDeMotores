using Game.Player.States;
using Game.Shared.Interfaces;

namespace Game.Player
{
    /// <summary>
    /// Máquina de estados del jugador.
    /// Maneja las transiciones entre estados lógicos y de animación.
    /// </summary>
    public class PlayerStateMachine : IStateMachine<PlayerState>
    {
        public PlayerState CurrentState { get; private set; }

        public void Initialize(PlayerState startState)
        {
            CurrentState = startState;
            CurrentState.Enter();
        }

        public void ChangeState(PlayerState newState)
        {
            if (newState == CurrentState)
                return;

            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
using Game.Enemy.States;
using Game.Shared.Interfaces;

namespace Enemy
{
    /// <summary>
    /// Máquina de estados para enemigos.
    /// Maneja la transición entre estados como Idle, Persecución, Ataque y Muerte.
    /// </summary>
    public class EnemyStateMachine : IStateMachine<EnemyState>
    {
        public EnemyState CurrentState { get; private set; }

        public void Initialize(EnemyState startState)
        {
            CurrentState = startState;
            CurrentState.Enter();
        }

        public void ChangeState(EnemyState newState)
        {
            if (newState == CurrentState)
                return;

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
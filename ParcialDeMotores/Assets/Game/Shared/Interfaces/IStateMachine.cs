namespace Game.Shared.Interfaces
{
    public interface IStateMachine<TState> where TState : IState
    {
        TState CurrentState { get; }
        void Initialize(TState startState);
        void ChangeState(TState newState);
    }
}
namespace Game.Shared.Interfaces
{
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }
}
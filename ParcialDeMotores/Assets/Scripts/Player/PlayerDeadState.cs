using Managers;

namespace Player
{
    /// <summary>
    /// Estado que se activa cuando el jugador muere.
    /// Cambia el estado global del juego a Game Over.
    /// </summary>
    public class PlayerDeadState : PlayerState
    {
        public PlayerDeadState(Player player, 
                               PlayerStateMachine stateMachine, 
                               string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            GameManager.Instance.LoseGame();
        }
    }
}
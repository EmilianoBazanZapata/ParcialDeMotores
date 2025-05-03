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
        public override void Update()
        {
            // Nada. Está muerto.
        }
    }
}
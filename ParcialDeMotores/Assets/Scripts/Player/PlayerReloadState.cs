namespace Player
{
    /// <summary>
    /// Estado de recarga del jugador. Inicia la corrutina de recarga si hay munición disponible.
    /// </summary>
    public class PlayerReloadState : PlayerState
    {
        private readonly Player _player;

        public PlayerReloadState(Player player, 
                                 PlayerStateMachine stateMachine, 
                                 string animBoolName) : base(player, stateMachine, animBoolName)
        {
            _player = player;
        }

        public override void Enter()
        {
            if (_player.TotalAmmo == 0)
                return;

            base.Enter();

            _player.StartCoroutine(_player.ReloadCoroutine());
        }
    }
}
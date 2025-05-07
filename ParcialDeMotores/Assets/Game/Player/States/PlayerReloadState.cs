namespace Game.Player.States
{
    /// <summary>
    /// Estado de recarga del jugador. Inicia la corrutina de recarga si hay munición disponible.
    /// </summary>
    public class PlayerReloadState : PlayerState
    {
        private readonly Game.Player.Player _player;

        public PlayerReloadState(Game.Player.Player player, 
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
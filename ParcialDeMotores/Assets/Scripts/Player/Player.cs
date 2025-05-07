using System;
using System.Collections;
using Bullet;
using Enums;
using Managers;
using UI;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator), typeof(Rigidbody))]
    public class Player : Entity
    {
        [Header("Ángulos para transición de animaciones")]
        [SerializeField] private float _forwardMaxAngle = 60f;

        [Header("Cámara del jugador")]
        [SerializeField] private Camera _playerCamera;

        [Header("Disparo")]
        [SerializeField] private int _maxAmmo = 10;
        [SerializeField] private float _reloadTime = 2f;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private BulletPool _bulletPool;
        public float _shootCooldown = 0.3f;
        private float _lastShootTime = -Mathf.Infinity;

        [Header("Vida")]
        [SerializeField] private int _maxHealth = 75;
        [SerializeField] private UIHealthBar _healthBarUI;
        
        public int MaxHealth => _maxHealth;

        public int CurrentAmmo { get; private set; }
        public int TotalAmmo { get; set; } = 100;
        public int CurrentHealth { get; private set; }
        public event Action<int, int> OnAmmoChanged;

        private bool _isAlive = true;

        #region Máquina de estados

        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerMoveBackwardsState MoveBackwardsState { get; private set; }
        public PlayerMoveLeftState MoveLeftState { get; private set; }
        public PlayerMoveRightState MoveRightState { get; private set; }
        public PlayerReloadState ReloadState { get; private set; }
        public PlayerDeadState DeadState { get; private set; }

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            InitializeStates();
        }

        protected override void Start()
        {
            base.Start();

            StateMachine.Initialize(IdleState);
            CurrentHealth = _maxHealth;
            CurrentAmmo = _maxAmmo;
            _healthBarUI.SetHealth((float)CurrentHealth / _maxHealth);
            NotifyAmmoChange();
        }

        protected override void Update()
        {
            base.Update();

            if (GameManager.Instance.CurrentState != GameState.InGame || !_isAlive) return;

            StateMachine.CurrentState?.Update();
            RotateTowardsMouse();

            if (Input.GetMouseButton(0) && 
                CurrentAmmo > 0 && 
                CanShoot() &&
                StateMachine.CurrentState != MoveBackwardsState)
            {
                ShootBullet();
            }

            if (Input.GetKeyDown(KeyCode.R) && TotalAmmo > 0 && CurrentAmmo < _maxAmmo)
                StateMachine.ChangeState(ReloadState);
        }

        #endregion

        #region Movimiento

        public void Move(Vector3 input)
        {
            if (input == Vector3.zero) return;

            var moveDir = transform.forward * input.z + transform.right * input.x;
            var newPosition = transform.position + moveDir.normalized * _moveSpeed * Time.deltaTime;
            Rb.MovePosition(newPosition);
        }

        public Vector3 GetInputDirection()
        {
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");
            return new Vector3(h, 0, v).normalized;
        }

        public void RotateTowardsMouse()
        {
            var ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;

            var lookDir = hit.point - transform.position;
            lookDir.y = 0;

            if (lookDir.magnitude <= 0.2f) return;

            var targetRotation = Quaternion.LookRotation(lookDir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        #endregion

        #region Combate

        public bool CanShoot() => Time.time >= _lastShootTime + _shootCooldown && CurrentAmmo > 0;

        public void ShootBullet()
        {
            SoundManager.Instance.PlaySound(SoundType.Shot);
            _lastShootTime = Time.time;

            var ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var hitPoint = hit.point;
                hitPoint.y = _shootPoint.position.y;
                var direction = (hitPoint - _shootPoint.position).normalized;

                var bullet = _bulletPool.GetBullet();
                bullet.transform.position = _shootPoint.position;
                bullet.Initialize(direction, _bulletPool.ReturnBullet);
            }

            CurrentAmmo--;
            NotifyAmmoChange();

            if (CurrentAmmo <= 0)
                StateMachine.ChangeState(ReloadState);
        }

        public IEnumerator ReloadCoroutine()
        {
            SoundManager.Instance.PlaySound(SoundType.Reload);
            yield return new WaitForSeconds(_reloadTime);

            var bulletsNeeded = _maxAmmo - CurrentAmmo;
            var bulletsToReload = Mathf.Min(bulletsNeeded, TotalAmmo);

            if (bulletsToReload <= 0)
            {
                StateMachine.ChangeState(IdleState);
                yield break;
            }

            CurrentAmmo += bulletsToReload;
            TotalAmmo -= bulletsToReload;

            NotifyAmmoChange();
            StateMachine.ChangeState(IdleState);
        }

        public void NotifyAmmoChange()
        {
            OnAmmoChanged?.Invoke(CurrentAmmo, TotalAmmo);
        }

        #endregion

        #region Salud

        public void TakeDamage(int amount)
        {
            if (!_isAlive) return;

            CurrentHealth -= amount;
            CurrentHealth = Mathf.Max(0, CurrentHealth);
            _healthBarUI.SetHealth((float)CurrentHealth / _maxHealth);

            if (CurrentHealth <= 0)
                Die();
        }

        public void Heal(int amount)
        {
            if (CurrentHealth >= _maxHealth) return;

            CurrentHealth = Mathf.Min(CurrentHealth + amount, _maxHealth);
            _healthBarUI.SetHealth((float)CurrentHealth / _maxHealth);
        }

        private void Die()
        {
            _isAlive = false;
            SoundManager.Instance.PlaySound(SoundType.PlayerDeath);
            StateMachine.ChangeState(DeadState);
        }

        #endregion

        #region Inicialización

        private void InitializeStates()
        {
            StateMachine = new PlayerStateMachine();
            IdleState = new PlayerIdleState(this, StateMachine, "Idle");
            MoveState = new PlayerMoveState(this, StateMachine, "Move");
            MoveBackwardsState = new PlayerMoveBackwardsState(this, StateMachine, "MoveBackwards");
            MoveLeftState = new PlayerMoveLeftState(this, StateMachine, "MoveLeft");
            MoveRightState = new PlayerMoveRightState(this, StateMachine, "MoveRight");
            ReloadState = new PlayerReloadState(this, StateMachine, "Reload");
            DeadState = new PlayerDeadState(this, StateMachine, "Die");
        }

        #endregion
    }
}

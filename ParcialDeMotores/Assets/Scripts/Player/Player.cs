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
        [Header("Ángulos para transición de animaciones")] [SerializeField]
        private float forwardMaxAngle = 60f;
        
        [Header("Camara")] public Camera playerCamera;

        [Header("Disparo")] public int maxAmmo = 10;
        public float reloadTime = 2f;
        public int currentAmmo;
        public int totalAmmo = 100;
        public event Action<int, int> OnAmmoChanged;

        [Header("Cooldown de disparo")] public float shootCooldown = 0.3f;
        private float _lastShootTime = -Mathf.Infinity;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private BulletPool bulletPool;

        [Header("Vida")] public int maxHealth = 75;
        public int currentHealth;
        private bool _live = true;
        
        [SerializeField] private UIHealthBar healthBarUI;

        #region States

        public PlayerStateMachine StateMachine { get; private set; }

        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerMoveBackwardsState MoveBackwardsState { get; private set; }
        public PlayerMoveLeftState MoveLeftState { get; private set; }
        public PlayerMoveRightState MoveRightState { get; private set; }
        public PlayerReloadState ReloadState { get; private set; }
        public PlayerDeadState DeadState { get; private set; }

        #endregion


        protected override void Awake()
        {
            base.Awake();
            StateMachine = new PlayerStateMachine();
            IdleState = new PlayerIdleState(this, StateMachine, "Idle");
            MoveState = new PlayerMoveState(this, StateMachine, "Move");
            MoveBackwardsState = new PlayerMoveBackwardsState(this, StateMachine, "MoveBackwards");
            MoveLeftState = new PlayerMoveLeftState(this, StateMachine, "MoveLeft");
            MoveRightState = new PlayerMoveRightState(this, StateMachine, "MoveRight");
            ReloadState = new PlayerReloadState(this, StateMachine, "Reload");
            DeadState = new PlayerDeadState(this, StateMachine, "Die");
        }


        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(IdleState);

            currentHealth = maxHealth;
            healthBarUI.SetHealth((float)currentHealth / maxHealth);
        }

        protected override void Update()
        {
            base.Update();
            
            if (GameManager.Instance.CurrentState != GameState.InGame) return;
            
            StateMachine.CurrentState?.Update();

            if (!_live)
                return;

            RotateTowardsMouse();

            if (Input.GetMouseButton(0) && currentAmmo > 0 && CanShoot() &&
                StateMachine.CurrentState != MoveBackwardsState)
                ShootBullet();
            
            if(Input.GetKeyDown(KeyCode.R) && totalAmmo >0 && currentAmmo < maxAmmo)
                StateMachine.ChangeState(ReloadState);
        }

        public void Move(Vector3 input)
        {
            if (input == Vector3.zero) return;
            
            var moveDir = transform.forward * input.z + transform.right * input.x;
            var newPosition = transform.position + moveDir.normalized * moveSpeed * Time.deltaTime;

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
            var ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;
            var lookDir = hit.point - transform.position;
            lookDir.y = 0;

            if (!(lookDir.magnitude > 0.2f)) return;
            var targetRotation = Quaternion.LookRotation(lookDir.normalized);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        public IEnumerator ReloadCoroutine()
        {
            SoundManager.Instance.PlaySound(SoundType.Reload);
            yield return new WaitForSeconds(reloadTime);

            var bulletsNeeded = maxAmmo - currentAmmo;

            if (totalAmmo <= 0)
            {
                StateMachine.ChangeState(IdleState);
                yield break;
            }

            var bulletsToReload = Mathf.Min(bulletsNeeded, totalAmmo);

            currentAmmo += bulletsToReload;
            totalAmmo -= bulletsToReload;
            
            NotifyAmmoChange();

            StateMachine.ChangeState(IdleState);
        }


        public void ShootBullet()
        {
            SoundManager.Instance.PlaySound(SoundType.Shot);
            
            _lastShootTime = Time.time;

            var ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var hitPoint = hit.point;
                hitPoint.y = shootPoint.position.y;
                var direction = (hitPoint - shootPoint.position).normalized;

                var bullet = bulletPool.GetBullet();
                bullet.transform.position = shootPoint.position;
                bullet.Initialize(direction, bulletPool.ReturnBullet);
            }

            currentAmmo--;
            NotifyAmmoChange();

            if (currentAmmo <= 0)
                StateMachine.ChangeState(ReloadState);
        }

        public void TakeDamage(int amount)
        {
            if(!_live)
                return;
            
            currentHealth -= amount;
            currentHealth = Mathf.Max(0, currentHealth);
            healthBarUI.SetHealth((float)currentHealth / maxHealth);

            if (currentHealth <= 0)
                Die();
        }

        private void Die()
        {
            _live = false;
            SoundManager.Instance.PlaySound(SoundType.PlayerDeath);
            StateMachine.ChangeState(DeadState);
        }

        public void Heal(int amount)
        {
            if (currentHealth >= maxHealth)
                return;

            currentHealth += amount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            healthBarUI.SetHealth((float)currentHealth / maxHealth);
        }
        
        private void NotifyAmmoChange()
        {
            OnAmmoChanged?.Invoke(currentAmmo, totalAmmo);
        }

        public bool CanShoot()
        {
            return Time.time >= _lastShootTime + shootCooldown && currentAmmo > 0;
        }
    }
}
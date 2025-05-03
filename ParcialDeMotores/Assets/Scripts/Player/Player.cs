using System.Collections;
using Bullet;
using UI;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator), typeof(Rigidbody))]
    public class Player : Entity
    {
        [Header("Ángulos para transición de animaciones")] [SerializeField]
        private float forwardMaxAngle = 60f;

        [SerializeField] private float sideMinAngle = 60f;
        [SerializeField] private float sideMaxAngle = 120f;
        [SerializeField] private float backwardMinAngle = 120f;

        [Header("Camara")] public Camera playerCamera;

        [Header("Disparo")] public int maxAmmo = 10;
        public float reloadTime = 2f;
        public int currentAmmo;
        public int totalAmmo = 100;

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
            StateMachine.CurrentState?.Update();

            if (!_live)
                return;

            RotateTowardsMouse();

            if (Input.GetMouseButton(0) && currentAmmo > 0 && CanShoot() &&
                StateMachine.CurrentState != MoveBackwardsState)
                ShootBullet();
        }

        public void Move(Vector3 input)
        {
            if (input == Vector3.zero) return;

            // Convertí el input a espacio local (independiente de rotación del mouse)
            Vector3 moveDir = transform.forward * input.z + transform.right * input.x;
            Vector3 newPosition = transform.position + moveDir.normalized * moveSpeed * Time.deltaTime;

            Rb.MovePosition(newPosition);
        }

        public Vector3 GetInputDirection()
        {
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");
            return new Vector3(h, 0, v).normalized;
        }

        public MovementDirection GetMovementDirectionFromInput(Vector3 input)
        {
            if (input == Vector3.zero)
                return MovementDirection.None;

            // Convertimos input global a local respecto al jugador
            Vector3 localInput = transform.InverseTransformDirection(input);

            float angle = Mathf.Atan2(localInput.x, localInput.z) * Mathf.Rad2Deg;

            // Rango ajustable si querés más precisión
            if (angle >= -45 && angle <= 45)
                return MovementDirection.Forward;

            if (angle > 45 && angle < 135)
                return MovementDirection.Right;

            if (angle < -45 && angle > -135)
                return MovementDirection.Left;

            return MovementDirection.Backward;
        }

        public void RotateTowardsMouse()
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 lookDir = hit.point - transform.position;
                lookDir.y = 0;

                if (lookDir.magnitude > 0.2f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDir.normalized);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRotation,
                        rotationSpeed * Time.deltaTime
                    );
                }
            }
        }

        public IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(reloadTime);

            int bulletsNeeded = maxAmmo - currentAmmo;

            if (totalAmmo <= 0)
            {
                Debug.Log("⚠ Sin munición en reserva.");
                StateMachine.ChangeState(IdleState);
                yield break;
            }

            int bulletsToReload = Mathf.Min(bulletsNeeded, totalAmmo);

            currentAmmo += bulletsToReload;
            totalAmmo -= bulletsToReload;

            Debug.Log($"🔄 Recargado: +{bulletsToReload} balas | Munición restante: {totalAmmo}");

            StateMachine.ChangeState(IdleState);
        }


        public void ShootBullet()
        {
            _lastShootTime = Time.time;

            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 hitPoint = hit.point;
                hitPoint.y = shootPoint.position.y;
                Vector3 direction = (hitPoint - shootPoint.position).normalized;

                var bullet = bulletPool.GetBullet();
                bullet.transform.position = shootPoint.position;
                bullet.Initialize(direction, bulletPool.ReturnBullet);
            }

            currentAmmo--;

            if (currentAmmo <= 0)
                StateMachine.ChangeState(ReloadState);
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Max(0, currentHealth);

            Debug.Log($"🩸 Daño recibido: -{amount} | Vida restante: {currentHealth}");
            healthBarUI.SetHealth((float)currentHealth / maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("💀 Jugador muerto");
            _live = false;
            StateMachine.ChangeState(DeadState);
        }

        public void Heal(int amount)
        {
            if (currentHealth >= maxHealth)
                return;

            currentHealth += amount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            Debug.Log($"❤️ Vida curada: +{amount} | Vida actual: {currentHealth}");
            healthBarUI.SetHealth((float)currentHealth / maxHealth);
        }

        public bool CanShoot()
        {
            return Time.time >= _lastShootTime + shootCooldown && currentAmmo > 0;
        }

        public void AnimationTrigger()
        {
            StateMachine.CurrentState?.AnimationFinishTrigger();
        }
    }
}
using UnityEngine;
using UnityEngine.Serialization;

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
        public PlayerStateMachine StateMachine { get; private set; }
        
        
        [Header("Dirección - Amortiguación de cambio (°)")]
        [SerializeField] private float directionHysteresis = 10f;
        public MovementDirection LastDirection = MovementDirection.None;


        #region States

        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerMoveBackwardsState MoveBackwardsState { get; private set; }
        public PlayerMoveLeftState MoveLeftState { get; private set; }
        public PlayerMoveRightState MoveRightState { get; private set; }

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
        }


        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(IdleState);
        }

        protected override void Update()
        {
            base.Update();
            StateMachine.CurrentState?.Update();
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


        public void AnimationTrigger()
        {
            StateMachine.CurrentState?.AnimationFinishTrigger();
        }
    }
}
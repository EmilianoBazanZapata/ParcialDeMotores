using UnityEngine;

namespace Player
{
    public class Entity : MonoBehaviour
    {
        [Header("Movimiento")]
        [SerializeField] protected float _moveSpeed = 5f;
        [SerializeField] protected float _rotationSpeed = 10f;

        public Animator Animator { get; private set; }
        public Rigidbody Rb { get; private set; }

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody>();
        }

        protected virtual void Start() { }

        protected virtual void Update() { }
    }
}
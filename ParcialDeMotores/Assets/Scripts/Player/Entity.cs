using UnityEngine;

namespace Player
{
    public class Entity : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float rotationSpeed = 10f;
        public Animator Animator { get; private set; }
        public Rigidbody Rb { get; private set; }
        
        protected virtual void Awake()
        {
            // Se puede sobrescribir en clases hijas
        }

        protected virtual void Start()
        {
            Animator = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody>();
        }

        protected virtual void Update()
        {
            // Se puede sobrescribir en clases hijas
        }
    }
}
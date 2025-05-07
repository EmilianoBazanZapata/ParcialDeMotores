using UnityEngine;

namespace CameraPlayer
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Objetivo a seguir")]
        [SerializeField] private Transform _target;

        [Header("Offset de la cámara")]
        [SerializeField] private Vector3 _offset = new(0f, 5f, -6f);

        [Header("Velocidad de seguimiento")]
        [SerializeField] private float _smoothSpeed = 10f;

        [Header("Altura fija al mirar al jugador")]
        [SerializeField] private float _lookHeightOffset = 1.5f;

        private void LateUpdate()
        {
            if (_target == null) return;

            FollowTarget();
            LookAtTarget();
        }

        /// <summary>
        /// Interpola suavemente la posición de la cámara hacia el objetivo.
        /// </summary>
        private void FollowTarget()
        {
            var desiredPosition = _target.position + _offset;
            var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }

        /// <summary>
        /// Hace que la cámara mire hacia el jugador, ajustando ligeramente la altura.
        /// </summary>
        private void LookAtTarget()
        {
            var lookPoint = _target.position + Vector3.up * _lookHeightOffset;
            transform.LookAt(lookPoint);
        }
    }
}
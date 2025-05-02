using UnityEngine;

namespace CameraPlayer
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Objetivo a seguir")]
        public Transform target;

        [Header("Offset de la cámara")]
        public Vector3 offset = new Vector3(0f, 5f, -6f);

        [Header("Velocidad de seguimiento")]
        public float smoothSpeed = 10f;

        [Header("Altura fija al mirar al jugador")]
        public float lookHeightOffset = 1.5f;

        void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            // Mirar hacia el jugador con un pequeño desplazamiento en altura
            Vector3 lookPoint = target.position + Vector3.up * lookHeightOffset;
            transform.LookAt(lookPoint);
        }
    }
}
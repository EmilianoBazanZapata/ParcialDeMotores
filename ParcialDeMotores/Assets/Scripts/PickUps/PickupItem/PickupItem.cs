using Enums;
using UnityEngine;

namespace PickUps.PickupItem
{
    public class PickupItem : MonoBehaviour
    {
        [Header("Configuración del ítem")]
        public PickupType type;
        public int amount = 10;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player player))
                return;

            if (type == PickupType.Health && player.currentHealth == player.maxHealth)
                return;

            ApplyPickupTo(player);
            ReturnToPool();
        }

        /// <summary>
        /// Aplica el efecto del pickup al jugador según su tipo.
        /// </summary>
        private void ApplyPickupTo(Player.Player player)
        {
            switch (type)
            {
                case PickupType.Health:
                    player.Heal(amount);
                    break;

                case PickupType.Ammo:
                    player.totalAmmo += amount;
                    player.NotifyAmmoChange();
                    Debug.Log($"🔫 Munición obtenida: +{amount} | Total: player.totalAmmo");
                    break;

                default:
                    Debug.LogWarning($"PickupType no manejado: {type}");
                    break;
            }
        }

        /// <summary>
        /// Devuelve el pickup a la pool.
        /// </summary>
        private void ReturnToPool()
        {
            var pool = FindObjectOfType<PickupPool>();
            if (pool != null)
                pool.ReturnPickup(gameObject);
            else
                Destroy(gameObject); // fallback por si no hay pool
        }
    }
}
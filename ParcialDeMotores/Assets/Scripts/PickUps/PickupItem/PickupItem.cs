using Enums;
using UnityEngine;

namespace PickUps.PickupItem
{
    public class PickupItem: MonoBehaviour
    {
        public PickupType type;
        public int amount = 10;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                if (type == PickupType.Health && player.currentHealth == player.maxHealth)
                    return;
                
                switch (type)
                {
                    case PickupType.Health:
                        player.Heal(amount);
                        break;
                    case PickupType.Ammo:
                        player.totalAmmo += amount;
                        player.NotifyAmmoChange();
                        Debug.Log($"🔫 Munición obtenida: +{amount} | Total: {player.totalAmmo}");
                        break;
                }

                var pool = FindObjectOfType<PickupPool>();
                pool.ReturnPickup(gameObject);
            }
        }
    }
}
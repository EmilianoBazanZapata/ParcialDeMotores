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
                switch (type)
                {
                    case PickupType.Health:
                        player.Heal(amount);
                        break;
                    case PickupType.Ammo:
                        player.totalAmmo += amount;
                        Debug.Log($"🔫 Munición obtenida: +{amount} | Total: {player.totalAmmo}");
                        break;
                }

                Destroy(gameObject);
            }
        }
    }
}
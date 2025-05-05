using TMPro;
using UnityEngine;

namespace UI
{
    public class UIAmmoDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ammoText;
        private Player.Player player;

        private void Start()
        {
            player = FindObjectOfType<Player.Player>();

            if (player != null)
            {
                player.OnAmmoChanged += UpdateAmmoUI;
                
                UpdateAmmoUI(player.currentAmmo, player.totalAmmo);
            }
            else
            {
                Debug.LogWarning("No se encontró el jugador en la escena.");
            }
        }

        private void OnDestroy()
        {
            if (player != null)
                player.OnAmmoChanged -= UpdateAmmoUI;
        }

        private void UpdateAmmoUI(int current, int max)
        {
            ammoText.text = $"Ammo: {current} / {max}";
        }
    }
}
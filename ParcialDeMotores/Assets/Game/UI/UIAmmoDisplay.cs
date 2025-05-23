﻿using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class UIAmmoDisplay : MonoBehaviour
    {
        [Header("Referencia UI")]
        [SerializeField] private TextMeshProUGUI _ammoText;

        private Game.Player.Player _player;

        private void Start()
        {
            _player = FindObjectOfType<Game.Player.Player>();

            if (_player != null)
            {
                _player.OnAmmoChanged += UpdateAmmoUI;
                UpdateAmmoUI(_player.CurrentAmmo, _player.TotalAmmo);
            }
            else
            {
                Debug.LogWarning("No se encontró el jugador en la escena.");
            }
        }

        private void OnDestroy()
        {
            if (_player != null)
                _player.OnAmmoChanged -= UpdateAmmoUI;
        }

        /// <summary>
        /// Actualiza el texto de la interfaz con la munición actual y total.
        /// </summary>
        private void UpdateAmmoUI(int current, int max)
        {
            _ammoText.text = $"Ammo: {current} / {max}";
        }
    }
}
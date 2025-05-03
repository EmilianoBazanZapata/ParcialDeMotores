using UnityEngine;
using TMPro;

namespace Managers
{
    public class SurvivalTimerManager : MonoBehaviour
    {
        [SerializeField] private float survivalTime = 120f; // tiempo en segundos
        [SerializeField] private TextMeshProUGUI timerText;
    
        private float currentTime;
        private bool gameEnded = false;

        private void Start()
        {
            currentTime = survivalTime;
        }

        private void Update()
        {
            if (gameEnded) return;

            currentTime -= Time.deltaTime;
            UpdateTimerUI();

            if (!(currentTime <= 0f)) return;
            
            currentTime = 0f;
            gameEnded = true;
            HandleVictory();
        }

        private void UpdateTimerUI()
        {
            var minutes = Mathf.FloorToInt(currentTime / 60f);
            var seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = $"Time Left: {minutes:00}:{seconds:00}";
        }

        private void HandleVictory()
        {
            Debug.Log("[Timer] ¡Tiempo cumplido! El jugador sobrevivió.");
        }
    }
}
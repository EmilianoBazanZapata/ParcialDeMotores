using Enums;
using UnityEngine;
using TMPro;

namespace Managers
{
    public class SurvivalTimerManager : MonoBehaviour
    {
        [SerializeField] private float survivalTime = 120f; // tiempo en segundos
        [SerializeField] private TextMeshProUGUI timerText;
    
        private float _currentTime;
        private bool _gameEnded = false;

        private void Start()
        {
            _currentTime = survivalTime;
        }

        private void Update()
        {
            if (_gameEnded) return;
            
            if (GameManager.Instance.CurrentState != GameState.InGame) return;

            _currentTime -= Time.deltaTime;
            UpdateTimerUI();

            if (!(_currentTime <= 0f)) return;
            
            _currentTime = 0f;
            _gameEnded = true;
            GameManager.Instance.SetGameState(GameState.Victory);
        }

        private void UpdateTimerUI()
        {
            var minutes = Mathf.FloorToInt(_currentTime / 60f);
            var seconds = Mathf.FloorToInt(_currentTime % 60f);
            timerText.text = $"Time Left: {minutes:00}:{seconds:00}";
        }
    }
}
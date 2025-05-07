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
            if (ShouldSkipUpdate()) return;

            DecreaseTimer();
            UpdateTimerUI();

            if (IsTimeOver())
            {
                HandleTimeOver();
            }
        }

        private bool ShouldSkipUpdate()
        {
            return _gameEnded || GameManager.Instance.CurrentState != GameState.InGame;
        }

        private void DecreaseTimer()
        {
            _currentTime -= Time.deltaTime;
        }

        private bool IsTimeOver()
        {
            return _currentTime <= 0f;
        }

        private void HandleTimeOver()
        {
            _currentTime = 0f;
            _gameEnded = true;
            GameManager.Instance.WinGame();
        }

        private void UpdateTimerUI()
        {
            var minutes = Mathf.FloorToInt(_currentTime / 60f);
            var seconds = Mathf.FloorToInt(_currentTime % 60f);
            timerText.text = $"Time Left: {minutes:00}:{seconds:00}";
        }
    }
}
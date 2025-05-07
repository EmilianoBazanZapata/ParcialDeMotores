using Game.Shared.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statusMessageText;
        [SerializeField] private Button retryButton;

        private void Start()
        {
            SubscribeToEvents();
            HideEndMessage();
            ShowRetryButton(true);
        }

        private void OnDestroy() => UnsubscribeFromEvents();

        private void SubscribeToEvents()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        private void UnsubscribeFromEvents()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }

        private void HandleGameStateChanged(GameState newState)
        {
            if (newState == GameState.Victory || newState == GameState.GameOver)
                ShowEndGameMessage(newState == GameState.Victory);
        }

        public void PlayGame() =>SceneManager.LoadScene("Loading");
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void HideEndMessage()
        {
            if (statusMessageText != null)
                statusMessageText.gameObject.SetActive(false);
        }

        private void ShowRetryButton(bool show)
        {
            if (retryButton != null)
                retryButton.gameObject.SetActive(show);
        }

        private void ShowEndGameMessage(bool hasWon)
        {
            if (statusMessageText != null)
            {
                statusMessageText.text = hasWon ? "VICTORY!" : "GAME OVER";
                statusMessageText.color = hasWon ? Color.green : Color.red;
                statusMessageText.gameObject.SetActive(true);
            }

            ShowRetryButton(false);
        }
    }
}

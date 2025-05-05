using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statusMessageText;
        [SerializeField] private Button retryButton;

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            }
            
            if (statusMessageText != null)
            {
                statusMessageText.gameObject.SetActive(false);
            }

            if (retryButton != null)
            {
                retryButton.gameObject.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
            }
        }

        private void HandleGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.Victory:
                    ShowEndGameMessage(true);
                    break;
                case GameState.GameOver:
                    ShowEndGameMessage(false);
                    break;
            }
        }

        public void PlayGame()
        {
            SceneManager.LoadScene("Loading");
        }

        public void ShowEndGameMessage(bool hasWon)
        {
            if (statusMessageText != null)
            {
                statusMessageText.text = hasWon ? "VICTORY!" : "GAME OVER";
                statusMessageText.color = hasWon ? Color.green : Color.red;
                statusMessageText.gameObject.SetActive(true);
            }

            if (retryButton != null)
            {
                retryButton.gameObject.SetActive(false);
            }
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
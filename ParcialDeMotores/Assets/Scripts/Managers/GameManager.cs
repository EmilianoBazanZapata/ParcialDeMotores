using System;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Referencias")]
        [SerializeField] private Camera gameCamera;
        [SerializeField] private Camera menuCamera; 

        private bool isPaused;

        public static GameManager Instance { get; private set; }
        public GameState CurrentState { get; private set; }
        public event Action<GameState> OnGameStateChanged;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (gameCamera == null)
                gameCamera = Camera.main;

            if (menuCamera != null)
                menuCamera.enabled = false;

            SetGameState(GameState.InGame);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P) && CurrentState == GameState.InGame)
            {
                TogglePause();
            }
        }
        
        public void ResumeFromMenu()
        {
            isPaused = false;
            Time.timeScale = 1;

            if (gameCamera != null)
                gameCamera.enabled = true;

            if (menuCamera != null)
                menuCamera.enabled = false;
            
            SetGameState(GameState.InGame);
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private void TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;

            // Activar/Desactivar cámaras
            if (gameCamera != null)
                gameCamera.enabled = !isPaused;

            if (menuCamera != null)
                menuCamera.enabled = isPaused;
            
            SetGameState(isPaused ? GameState.Menu : GameState.InGame);
        }

        public void SetGameState(GameState newState)
        {
            if (newState == CurrentState) return;
            CurrentState = newState;
            OnGameStateChanged?.Invoke(CurrentState);
        }

        #region Estados Públicos

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void StartGame() => SetGameState(GameState.StartGame);
        public void WinGame() => SetGameState(GameState.Victory);
        public void LoseGame() => SetGameState(GameState.GameOver);
        public void GoToMenu() => SetGameState(GameState.MainMenu);
        public void GoToControl() => SetGameState(GameState.InControls);
        public void GoToOptions() => SetGameState(GameState.InOptions);
        public void GoToCharacter() => SetGameState(GameState.InCharacter);
        public void GoToCraft() => SetGameState(GameState.InCraft);
        public void ResumeGame() => SetGameState(GameState.InGame);
        public void InGame() => SetGameState(GameState.InGame);
        public void InMenu() => SetGameState(GameState.Menu);

        #endregion
    }
}

using System;
using Game.Core;
using Game.Shared.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Referencias")]
        [SerializeField] private Camera _gameCamera;
        [SerializeField] private Camera _menuCamera;

        private bool isPaused;

        public GameState CurrentState { get; private set; }
        public event Action<GameState> OnGameStateChanged;

        private void Start()
        {
            // Inicializa cámaras
            if (_gameCamera == null)
                _gameCamera = Camera.main;

            if (_menuCamera != null)
                _menuCamera.enabled = false;

            SetGameState(GameState.InGame);
        }

        private void Update()
        {
            // Alterna pausa con la tecla P
            if (Input.GetKeyDown(KeyCode.P) && CurrentState == GameState.InGame)
                TogglePause();
        }

        /// <summary>
        /// Cambia entre pausa y juego.
        /// </summary>
        private void TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            ToggleCameras(isPaused);
            SetGameState(isPaused ? GameState.Menu : GameState.InGame);
        }

        /// <summary>
        /// Activa o desactiva las cámaras del juego y del menú según el estado de pausa.
        /// </summary>
        private void ToggleCameras(bool pause)
        {
            if (_gameCamera != null)
                _gameCamera.enabled = !pause;

            if (_menuCamera != null)
                _menuCamera.enabled = pause;
        }

        /// <summary>
        /// Cambia el estado del juego y notifica a los suscriptores.
        /// </summary>
        public void SetGameState(GameState newState)
        {
            if (newState == CurrentState) return;
            CurrentState = newState;
            OnGameStateChanged?.Invoke(CurrentState);
        }

        /// <summary>
        /// Reanuda el juego desde el menú de pausa.
        /// </summary>
        public void ResumeFromMenu()
        {
            isPaused = false;
            Time.timeScale = 1;
            ToggleCameras(false);
            ResumeGame();
        }

        /// <summary>
        /// Vuelve al menú principal si el juego no está en curso.
        /// </summary>
        public void ReturnToMenu()
        {
            if (CurrentState == GameState.InGame) return;
            SceneManager.LoadScene("MainMenu");
        }

        /// <summary>
        /// Recarga la escena actual (útil tras derrota o victoria).
        /// </summary>
        public void ReloadScene()
        {
            if (CurrentState == GameState.InGame) return;
            Time.timeScale = 1f;
            SceneManager.LoadScene("Loading");
        }

        /// <summary>
        /// Cierra el juego (o detiene el editor si está en desarrollo).
        /// </summary>
        public void QuitGame()
        {
            if (CurrentState == GameState.InGame) return;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #region Métodos Públicos de Estados

        public void StartGame() => SetGameState(GameState.StartGame);
        public void GoToMenu() => SetGameState(GameState.MainMenu);
        public void GoToControl() => SetGameState(GameState.InControls);
        public void GoToOptions() => SetGameState(GameState.InOptions);
        public void GoToCharacter() => SetGameState(GameState.InCharacter);
        public void GoToCraft() => SetGameState(GameState.InCraft);
        public void ResumeGame() => SetGameState(GameState.InGame);
        public void InGame() => SetGameState(GameState.InGame);
        public void InMenu() => SetGameState(GameState.Menu);

        public void WinGame()
        {
            SetGameState(GameState.Victory);
            TogglePause();
        }

        public void LoseGame()
        {
            SetGameState(GameState.GameOver);
            TogglePause();
        }

        #endregion
    }
}

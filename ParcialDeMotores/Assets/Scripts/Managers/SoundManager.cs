using Enums;
using UnityEngine;

namespace Managers
{
public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [Header("Audio Sources")] [SerializeField]
        private AudioSource musicSource;

        [SerializeField] private AudioSource sfxSource;

        [Header("Audio Clips")] public AudioClip backgroundMusic;
        public AudioClip shotClip;
        public AudioClip reloadClip;
        public AudioClip zombieWalkClip;
        public AudioClip zombiedeadClip;
        public AudioClip zombieAttackClip;
        public AudioClip playerWalkClip;
        public AudioClip playerdeadClip;
        public AudioClip victoryClip;
        public AudioClip defeatClip;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }
        
        private void Start()
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            HandleGameStateChanged(GameManager.Instance.CurrentState);
        }

        private void PlaySFX(AudioClip clip)
        {
            if (clip != null)
                sfxSource.PlayOneShot(clip);
        }

        private void PlayMusic(AudioClip clip)
        {
            if (clip != null)
            {
                musicSource.clip = clip;
                musicSource.loop = true;
                musicSource.Play();
            }
        }

        public void PlaySound(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.BackgroundMusic:
                    PlayMusic(backgroundMusic);
                    break;
                case SoundType.Shot:
                    PlaySFX(shotClip);
                    break;
                case SoundType.Reload:
                    PlaySFX(reloadClip);
                    break;
                case SoundType.ZombieWalk:
                    PlaySFX(zombieWalkClip);
                    break;
                case SoundType.ZombieDeath:
                    PlaySFX(zombiedeadClip);
                    break;
                case SoundType.ZombieAttack:
                    PlaySFX(zombieAttackClip);
                    break;
                case SoundType.PlayerWalk:
                    PlaySFX(playerWalkClip);
                    break;
                case SoundType.PlayerDeath:
                    PlaySFX(playerdeadClip);
                    break;
            }
        }
        
        private void HandleGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.GameOver:
                    PlayMusic(defeatClip);
                    break;
                case GameState.Victory:
                    PlayMusic(victoryClip);
                    break;
                case GameState.InGame:
                    PlayMusic(backgroundMusic);
                    break;
            }
        }

        public void PauseMusic()
        {
            if (musicSource.isPlaying)
                musicSource.Pause();
        }

        public void ResumeMusic()
        {
            if (!musicSource.isPlaying)
                musicSource.UnPause();
        }

        public void SetSFXVolume(float volume) => sfxSource.volume = volume;
        public void SetMusicVolume(float volume) => musicSource.volume = volume;
        public void StartBackgroundMusic() => PlayMusic(backgroundMusic);
    }
}
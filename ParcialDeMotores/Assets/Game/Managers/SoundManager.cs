using Game.Core;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.Managers
{
    public class SoundManager : Singleton<SoundManager>
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Audio Clips")]
        public AudioClip backgroundMusic;
        public AudioClip shotClip;
        public AudioClip reloadClip;
        public AudioClip zombieWalkClip;
        public AudioClip zombieDeathClip;
        public AudioClip zombieAttackClip;
        public AudioClip playerWalkClip;
        public AudioClip playerDeathClip;
        public AudioClip victoryClip;
        public AudioClip defeatClip;
        
        private void Start()
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            HandleGameStateChanged(GameManager.Instance.CurrentState);
        }

        /// <summary>
        /// Reproduce un sonido de efectos (SFX).
        /// </summary>
        private void PlaySFX(AudioClip clip)
        {
            if (clip != null)
                sfxSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Reproduce música de fondo.
        /// </summary>
        private void PlayMusic(AudioClip clip)
        {
            if (clip == null) return;
            
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }

        /// <summary>
        /// Reproduce un sonido según el tipo especificado.
        /// </summary>
        public void PlaySound(SoundType soundType)
        {
            AudioClip clip = GetClipBySoundType(soundType);
            if (IsMusic(soundType))
                PlayMusic(clip);
            else
                PlaySFX(clip);
        }

        /// <summary>
        /// Devuelve el clip correspondiente al tipo de sonido.
        /// </summary>
        private AudioClip GetClipBySoundType(SoundType type) => type switch
        {
            SoundType.BackgroundMusic => backgroundMusic,
            SoundType.Shot => shotClip,
            SoundType.Reload => reloadClip,
            SoundType.ZombieWalk => zombieWalkClip,
            SoundType.ZombieDeath => zombieDeathClip,
            SoundType.ZombieAttack => zombieAttackClip,
            SoundType.PlayerWalk => playerWalkClip,
            SoundType.PlayerDeath => playerDeathClip,
            SoundType.Victory => victoryClip,
            SoundType.Defeat => defeatClip,
            _ => null
        };

        /// <summary>
        /// Determina si el tipo de sonido corresponde a música.
        /// </summary>
        private bool IsMusic(SoundType type) =>
            type == SoundType.BackgroundMusic || type == SoundType.Victory || type == SoundType.Defeat;

        /// <summary>
        /// Maneja la reproducción de música según el estado del juego.
        /// </summary>
        private void HandleGameStateChanged(GameState state)
        {
            var music = state switch
            {
                GameState.InGame => backgroundMusic,
                GameState.Victory => victoryClip,
                GameState.GameOver => defeatClip,
                _ => null
            };

            PlayMusic(music);
        }
    }
}

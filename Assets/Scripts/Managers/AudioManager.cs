using UnityEngine;

namespace KyleStankovich.Utils {
    [AddComponentMenu("Battle Dash/Managers/Audio Manager")]
    public class AudioManager : Singleton<AudioManager> {
        [Header("Audio Sources")]
        [SerializeField]
        AudioSource musicAudio = null;
        [SerializeField]
        AudioSource effectAudio = null;

        [Header("Audio Clips")]
        [SerializeField]
        AudioClip startMusic = null;
        [SerializeField]
        AudioClip runMusic = null;
        [SerializeField]
        AudioClip battleMusic = null;

        #region Properties
        /// <summary>
        /// The music audio source.
        /// </summary>
        public AudioSource MusicAudio {
            get { return musicAudio; }
        }

        /// <summary>
        /// The sound effet audio source.
        /// </summary>
        /// <returns></returns>
        public AudioSource EffectAudio {
            get { return effectAudio; }
        }
        #endregion

        #region Methods

        #region Unity methods
        new void Awake() {
            base.Awake();

            PlayStartMusic();
        }

        void Start() {
            //Load audio levels
            AudioListener.volume = PlayerPrefs.GetFloat(Constants.PREF_MASTERAUDIOLEVEL, 1.0f);
            musicAudio.volume = PlayerPrefs.GetFloat(Constants.PREF_MUSICAUDIOLEVEL, 1.0f);
            effectAudio.volume = PlayerPrefs.GetFloat(Constants.PREF_EFFECTAUDIOLEVEL, 1.0f);
        }

        void OnEnable() {
            //Subscribe to events
            EventManager.StartListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, PlayBattleMusic);
            EventManager.StartListening(Constants.EVENT_BOSSBATTLEENDCOMBAT, PlayRunMusic);
            EventManager.StartListening(Constants.EVENT_GAMESTART, PlayRunMusic);
        }

        void OnDisable() {
            //Unsubscribe from events
            EventManager.StopListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, PlayBattleMusic);
            EventManager.StopListening(Constants.EVENT_BOSSBATTLEENDCOMBAT, PlayRunMusic);
            EventManager.StopListening(Constants.EVENT_GAMESTART, PlayRunMusic);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Plays a music clip from the music AudioSouce.
        /// </summary>
        /// <param name="clip">Clip to play.</param>
        public void PlayMusic(AudioClip clip) {
            Play(musicAudio, clip);
        }

        /// <summary>
        /// Plays a sound effect clip from the sound effect AudioSouce.
        /// </summary>
        /// <param name="clip">Clip to play.</param>
        public void PlayEffect(AudioClip clip) {
            Play(effectAudio, clip);
        }

        /// <summary>
        /// Plays the game's start music.
        /// </summary>
        public void PlayStartMusic() {
            Play(musicAudio, startMusic);
        }

        /// <summary>
        /// Plays the game's run music.
        /// </summary>
        public void PlayRunMusic() {
            Play(musicAudio, runMusic);
        }

        /// <summary>
        /// Play's the game's battle music.
        /// </summary>
        public void PlayBattleMusic() {
            Play(musicAudio, battleMusic);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Plays an AudioClip from a AudioSource.
        /// </summary>
        /// <param name="source">AudioSource to play from.</param>
        /// <param name="clip">AudioClip to play.</param>
        void Play(AudioSource source, AudioClip clip) {
            if(source == null || clip == null)
                return;

            source.clip = clip;
            source.Play();
        }

        /// <summary>
        /// Plays an AudioClip from a AudioSource at a certain point.
        /// </summary>
        /// <param name="source">AudioSource to play from.</param>
        /// <param name="clip">AudioClip to play.</param>
        /// <param name="time">Time of AudioClip to start playing from.</param>
        void Play(AudioSource source, AudioClip clip, float time = 0.0f) {
            if(source == null || clip == null)
                return;

            Play(source, clip);
            source.time = time;
        }
        #endregion

        #endregion
    }
}

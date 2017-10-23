using UnityEngine;
using KyleStankovich.Utils;

public class SoundPlayer : MonoBehaviour {
	#region Public methods
	/// <summary>
	/// Plays some music through the music AudioSource.
	/// </summary>
	/// <param name="clip">Music to play.</param>
	public void PlayMusic(AudioClip clip) {
		AudioManager.Instance.PlayMusic(clip);
	}

	/// <summary>
	/// Plays a sound effect through the sound effect AudioSource.
	/// </summary>
	/// <param name="clip">Sound effect to play.</param>
	public void PlaySoundEffect(AudioClip clip) {
		AudioManager.Instance.PlayEffect(clip);
	}
	#endregion
}

using UnityEngine;

namespace MysticalForestAdventure
{
    [RequireComponent(typeof(AudioSource))]
    public class GameAudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
		[SerializeField] private AudioSource _bgmAudioSource;

		[SerializeField] private AudioClip _generalButtonClicAudioClip;

		private void Awake()
		{
			_audioSource = GetComponent<AudioSource>();
		}

		private void Start()
		{
			_bgmAudioSource.loop = true;
			_bgmAudioSource.playOnAwake = false;
		}

		public void PlayBGM()
		{
			_bgmAudioSource.Play();
		}

		public void StopBGM()
		{
			_bgmAudioSource.Stop();
		}

		public void PlayGeneralButtonClickSfx()
		{
			_audioSource.PlayOneShot(_generalButtonClicAudioClip);
		}


	}
}

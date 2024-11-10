using UnityEngine;
using DG.Tweening;

namespace MysticalForestAdventure
{
    [RequireComponent(typeof(AudioSource))]
    public class ReelAudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private AudioClip _spinStartAudioClip;
        [SerializeField] private AudioClip _spinStopAudioClip;

		private void Awake()
		{
			_audioSource = GetComponent<AudioSource>();
		}

		private void Start()
		{
			_audioSource.playOnAwake = false;
		}

        public void PlaySpinStartSfx()
        {
            _audioSource.clip = _spinStartAudioClip;
            _audioSource.loop = true;

			_audioSource.Play();
        }

        public void StopSpinSfx()
        {
			float volume = _audioSource.volume;
			_audioSource.DOFade(0f, 0.3f).OnComplete(() =>
			{
				_audioSource.Stop();
				_audioSource.clip = null;
				_audioSource.loop = false;
				_audioSource.volume = volume;
			});
		}

        public void PlaySpinStopSfx()
        {
            _audioSource.PlayOneShot(_spinStopAudioClip);
        }
    }
}

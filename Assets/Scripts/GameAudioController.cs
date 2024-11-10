using UnityEngine;

namespace MysticalForestAdventure
{
    [RequireComponent(typeof(AudioSource))]
    public class GameAudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
		[SerializeField] private AudioSource _bgmAudioSource;
		[SerializeField] private AudioClip _generalButtonClicAudioClip;

		[SerializeField] private AudioClip _noWinAudioClip;
		[SerializeField] private AudioClip _lowPayWinAudioClip;
		[SerializeField] private AudioClip _highPay1WinAudioClip;
		[SerializeField] private AudioClip _highPay2WinAudioClip;
		[SerializeField] private AudioClip _highPay3WinAudioClip;
		[SerializeField] private AudioClip _highPay4WinAudioClip;

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

		public void PlayWinSfx(WinResult winResult)
		{
			if(winResult == null)
			{
				Debug.Log($"No win result");
				return;
			}

			if(winResult.MaxLength <= 2)
			{
				_audioSource.PlayOneShot(_noWinAudioClip);
				return;
			}

			switch(winResult.Symbol)
			{
				case Symbol.LOW1:
				case Symbol.LOW2:
				case Symbol.LOW3:
				case Symbol.LOW4:
				case Symbol.LOW5:
					_audioSource.PlayOneShot(_lowPayWinAudioClip);
					break;

				case Symbol.HIGH1:
					_audioSource.PlayOneShot(_highPay1WinAudioClip);
					break;
				case Symbol.HIGH2:
					_audioSource.PlayOneShot(_highPay2WinAudioClip);
					break;
				case Symbol.HIGH3:
					_audioSource.PlayOneShot(_highPay3WinAudioClip);
					break;
				case Symbol.HIGH4:
					_audioSource.PlayOneShot(_highPay4WinAudioClip);
					break;
			}
		}
	}
}

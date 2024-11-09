using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace MysticalForestAdventure
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Image _loadingBarFillerImage;

		private void Start()
		{
			LoadGameScene();
		}

		private async void LoadGameScene()
		{
			AsyncOperation loadingAsyncOperation = SceneManager.LoadSceneAsync(Scenes.k_gameScene);

			loadingAsyncOperation.allowSceneActivation = false;
			SetLoadingFillBar(0f);

			while (loadingAsyncOperation.progress < 0.9f)
			{
				SetLoadingFillBar(loadingAsyncOperation.progress);
				await Task.Yield();
			}

			SetLoadingFillBar(1f);

			await Task.Delay(1000);

			loadingAsyncOperation.allowSceneActivation = true;
		}

		private void SetLoadingFillBar(float percent)
		{
			_loadingBarFillerImage.DOFillAmount(percent, 0.1f);
		}
	}
}

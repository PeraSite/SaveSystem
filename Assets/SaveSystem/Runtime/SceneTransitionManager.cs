using System;
using Cysharp.Threading.Tasks;
using Zenject;

namespace SaveSystem.Runtime {
	public class SceneTransitionManager {
		[Inject] private readonly ISceneTransition _sceneTransition;
		[Inject] private readonly ZenjectSceneLoader _sceneLoader;

		public void ChangeScene(string sceneName,
			Action beforeSceneChange = null,
			Action afterSceneChange = null) {
			ChangeSceneAsync(sceneName, beforeSceneChange, afterSceneChange).Forget();
		}

		private async UniTask ChangeSceneAsync(string sceneName,
			Action beforeSceneChange = null,
			Action afterSceneChange = null
		) {
			await _sceneTransition.StartTransition();
			beforeSceneChange?.Invoke();
			await _sceneLoader.LoadSceneAsync(sceneName);
			afterSceneChange?.Invoke();
			await _sceneTransition.EndTransition();
		}
	}
}

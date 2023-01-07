using System.Collections;
using SaveSystem.Runtime;
using UnityEngine;
using Zenject;

namespace SaveSystem.Samples {
	public class SceneChanger : MonoBehaviour {
		private ZenjectSceneLoader _sceneLoader;
		private SaveManager _saveManager;

		[Inject]
		public void Construct(ZenjectSceneLoader sceneLoader, SaveManager saveManager) {
			_sceneLoader = sceneLoader;
			_saveManager = saveManager;
		}

		public void ChangeScene(string sceneName) {
			StartCoroutine(ChangeSceneRoutine(sceneName));
		}

		private IEnumerator ChangeSceneRoutine(string sceneName) {
			_saveManager.Save();
			var task = _sceneLoader.LoadSceneAsync(sceneName);
			yield return new WaitUntil(() => task.isDone);
			_saveManager.Load();
		}
	}
}

using SaveSystem.Runtime;
using UnityEngine;
using Zenject;

namespace SaveSystem.Samples {
	public class SceneChanger : MonoBehaviour {
		private SaveManager _saveManager;

		[Inject]
		public void Construct(ZenjectSceneLoader sceneLoader, SaveManager saveManager) {
			_saveManager = saveManager;
		}

		public void ChangeScene(string sceneName) {
			_saveManager.ChangeScene(sceneName);
		}
	}
}

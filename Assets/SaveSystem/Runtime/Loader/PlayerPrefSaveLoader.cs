using UnityEngine;
using Zenject;

namespace SaveSystem.Runtime {
	public class PlayerPrefSaveLoader : ScriptableObjectInstaller<PlayerPrefSaveLoader>, ISaveLoader {
		public void Save(string saveName, string saveData) {
			PlayerPrefs.SetString(saveName, saveData);
		}

		public string Load(string saveName) {
			return PlayerPrefs.GetString(saveName);
		}

		public void Delete(string saveName) {
			PlayerPrefs.DeleteKey(saveName);
		}

		public override void InstallBindings() {
			Container.BindInterfacesAndSelfTo<PlayerPrefSaveLoader>().FromInstance(this).AsSingle();
		}
	}
}

using UnityEngine;
using Zenject;

namespace SaveSystem.Runtime {
	public class PlayerPrefSaveLoader : ScriptableObjectInstaller<PlayerPrefSaveLoader>, ISaveLoader {
		private IDataSerializer _dataSerializer;

		public void Save(string saveName, SaveData saveData) {
			var serializedData = _dataSerializer.Serialize(saveData);
			PlayerPrefs.SetString(saveName, serializedData);
			Debug.Log($"[PlayerPrefSaveLoader] Saved {serializedData}");
		}

		public SaveData Load(string saveName) {
			var serializedData = PlayerPrefs.GetString(saveName);
			Debug.Log($"[PlayerPrefSaveLoader] Loaded {serializedData}");
			return _dataSerializer.Deserialize<SaveData>(serializedData);
		}

		public void Delete(string saveName) {
			PlayerPrefs.DeleteKey(saveName);
		}

		public bool Has(string saveName) {
			return PlayerPrefs.HasKey(saveName);
		}

		public override void InstallBindings() {
			Container.BindInterfacesAndSelfTo<PlayerPrefSaveLoader>().FromInstance(this).AsSingle();
		}

		[Inject]
		public void Construct(IDataSerializer dataSerializer) {
			_dataSerializer = dataSerializer;
		}
	}
}

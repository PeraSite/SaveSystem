using UnityEngine;
using Zenject;

namespace SaveSystem.Runtime {
	public class PlayerPrefDataStorage : IDataStorage {
		[Inject]
		private IDataSerializer _dataSerializer;

		public void Save(string saveName, SaveData saveData) {
			var serializedData = _dataSerializer.Serialize(saveData);
			PlayerPrefs.SetString(saveName, serializedData);
			Debug.Log($"[PlayerPrefDataStorage] Saved {serializedData}");
		}

		public SaveData Load(string saveName) {
			var serializedData = PlayerPrefs.GetString(saveName);
			Debug.Log($"[PlayerPrefDataStorage] Loaded {serializedData}");
			return _dataSerializer.Deserialize<SaveData>(serializedData);
		}

		public void Delete(string saveName) {
			PlayerPrefs.DeleteKey(saveName);
		}

		public bool Has(string saveName) {
			return PlayerPrefs.HasKey(saveName);
		}
	}
}

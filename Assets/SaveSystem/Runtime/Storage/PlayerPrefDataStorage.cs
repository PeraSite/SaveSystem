using UnityEngine;

namespace SaveSystem.Runtime {
	public class PlayerPrefDataStorage : IDataStorage {
		public void Save(string key, string value) {
			PlayerPrefs.SetString(key, value);
		}

		public string Load(string key) {
			return PlayerPrefs.GetString(key);
		}

		public void Delete(string key) {
			PlayerPrefs.DeleteKey(key);
		}

		public bool Has(string key) {
			return PlayerPrefs.HasKey(key);
		}
	}
}

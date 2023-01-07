using UnityEngine;

namespace SaveSystem.Runtime {
	public class UnityDataSerializer : ScriptableObject, IDataSerializer {
		public string Serialize<T>(T value) {
			return JsonUtility.ToJson(value);
		}

		public T Deserialize<T>(string text) {
			return JsonUtility.FromJson<T>(text);
		}
	}
}

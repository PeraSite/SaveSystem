using SaveSystem.Runtime;
using UnityEngine.SceneManagement;

namespace SaveSystem.Samples {
	public class SaveMetadataSaver : ISaver<SaveMetadata> {
		public string Key => "Metadata";

		public void ResetData() { }

		public void ApplyData(SaveMetadata data) { }

		public SaveMetadata SaveData() {
			return new SaveMetadata {
				SceneName = SceneManager.GetActiveScene().name
			};
		}
	}

	public struct SaveMetadata {
		public string SceneName;
	}
}

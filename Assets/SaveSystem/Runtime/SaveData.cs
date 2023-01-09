using System.Collections.Generic;

namespace SaveSystem.Runtime {
	public class SaveData {
		public string SceneName;

		public readonly Dictionary<string, Dictionary<string, object>> Data;

		public SaveData() {
			Data = new Dictionary<string, Dictionary<string, object>>();
		}
	}
}

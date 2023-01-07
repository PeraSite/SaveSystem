using System;
using System.Collections.Generic;

namespace SaveSystem.Runtime {
	public class SaveData {
		public string SceneName;

		public Dictionary<string, string> Data;

		public SaveData() {
			Data = new Dictionary<string, string>();
		}
	}
}

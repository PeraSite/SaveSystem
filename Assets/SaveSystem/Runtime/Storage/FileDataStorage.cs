using System.IO;
using System.Text;
using UnityEngine;

namespace SaveSystem.Runtime {
	public class FileDataStorage : IDataStorage {
		public void Save(string key, string value) {
			var path = Path.Combine(Application.persistentDataPath, key);
			File.WriteAllText(path, value, Encoding.UTF8);
		}

		public string Load(string key) {
			var path = Path.Combine(Application.persistentDataPath, key);
			return File.ReadAllText(path, Encoding.UTF8);
		}

		public void Delete(string key) {
			var path = Path.Combine(Application.persistentDataPath, key);
			File.Delete(path);
		}

		public bool Has(string key) {
			var path = Path.Combine(Application.persistentDataPath, key);
			return File.Exists(path);
		}
	}
}

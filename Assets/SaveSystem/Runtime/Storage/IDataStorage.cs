namespace SaveSystem.Runtime {
	public interface IDataStorage {
		public void Save(string key, string value);
		public string Load(string key);
		public void Delete(string key);
		public bool Has(string key);
	}
}

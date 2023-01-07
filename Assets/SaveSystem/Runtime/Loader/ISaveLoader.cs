namespace SaveSystem.Runtime {
	public interface ISaveLoader {
		public void Save(string saveName, SaveData saveData);
		public SaveData Load(string saveName);
		public void Delete(string saveName);
		public bool Has(string saveName);
	}
}

namespace SaveSystem.Runtime {
	public interface ISaveLoader {
		public void Save(string saveName, string saveData);
		public string Load(string saveName);
		public void Delete(string saveName);
	}
}

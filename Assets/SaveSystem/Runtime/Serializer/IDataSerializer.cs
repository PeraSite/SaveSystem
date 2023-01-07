namespace SaveSystem.Runtime {
	public interface IDataSerializer {
		public string Serialize<T>(T value);
		public T Deserialize<T>(string text);
	}
}

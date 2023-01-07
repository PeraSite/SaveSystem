namespace SaveSystem.Runtime {
	public interface ISaver {
		public string Key { get; }

		public void ApplyDataWeak(object data);

		public object SaveDataWeak();

		public void ResetData();
	}

	public interface ISaver<T> : ISaver {
		public void ApplyData(T data);

		public T SaveData();

		void ISaver.ApplyDataWeak(object data) {
			ApplyData((T) data);
		}

		object ISaver.SaveDataWeak() {
			return SaveData();
		}
	}
}

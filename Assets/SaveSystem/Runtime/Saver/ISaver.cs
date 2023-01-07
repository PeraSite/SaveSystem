using System;

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
			// 데이터 직렬화 과정에서 int를 long으로 변환하는 문제 대응
			if (data is long && typeof(T) == typeof(int)) {
				ApplyData((T) (object) Convert.ToInt32(data));
				return;
			}

			ApplyData((T) data);
		}

		object ISaver.SaveDataWeak() {
			return SaveData();
		}
	}
}

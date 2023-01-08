using System;

namespace SaveSystem.Runtime {
	public interface ISaver {
		public string Key { get; }

		public void ApplyDataWeak(object data);

		public object SaveDataWeak();

		public void ResetData();
	}

	public interface ISaver<TValue> : ISaver {
		public void ApplyData(TValue data);

		public TValue SaveData();

		void ISaver.ApplyDataWeak(object data) {
			// 데이터 직렬화 과정에서 int를 long으로 변환하는 문제 대응
			if (data is long && typeof(TValue) == typeof(int)) {
				ApplyData((TValue) (object) Convert.ToInt32(data));
				return;
			}

			ApplyData((TValue) data);
		}

		object ISaver.SaveDataWeak() {
			return SaveData();
		}
	}
}

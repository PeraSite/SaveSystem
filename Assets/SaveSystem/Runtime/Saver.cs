using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

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

	public abstract class Saver<T> : MonoBehaviour, ISaver<T> {
		[FoldoutGroup("Saver"), SerializeField, ReadOnly]
		private string _key;
		public string Key => _key;

		[FoldoutGroup("Saver"), SerializeField, ReadOnly]
		private int _cachedInstanceId = -1;

		[FoldoutGroup("Saver"), ShowInInspector, ReadOnly]
		private SaveManager _saveManager;

#if UNITY_EDITOR
		private void OnValidate() {
			// Key가 할당되지 않았거나, 인스턴스 아이디가 변경되었을 때(복제되었을 때)
			if (_key == null || _cachedInstanceId != GetInstanceID()) {
				_key = Guid.NewGuid().ToString();
				_cachedInstanceId = GetInstanceID();
			}
		}
#endif

		[Inject]
		public void Construct(SaveManager manager) {
			_saveManager = manager;

			_saveManager.RegisterSaver(this);
		}

		private void OnDestroy() {
			_saveManager.UnregisterSaver(this);
		}

		public abstract void ApplyData(T data);

		public abstract T SaveData();

		public virtual void ResetData() { }
	}
}

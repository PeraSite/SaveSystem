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
		[property: FoldoutGroup("Saver"), ShowInInspector, PropertyOrder(-999)]
		public string Key { get; } = Guid.NewGuid().ToString();

		[ShowInInspector] protected SaveManager _saveManager;

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

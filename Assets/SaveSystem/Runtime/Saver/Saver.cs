using System;
using UnityEngine;
using Zenject;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace SaveSystem.Runtime {
	public abstract class Saver<T> : MonoBehaviour, ISaver<T> {
		[SerializeField]
#if ODIN_INSPECTOR
		[FoldoutGroup("Saver"), ReadOnly]
#endif

		private string _key;
		public string Key => _key;

		[SerializeField]
#if ODIN_INSPECTOR
		[FoldoutGroup("Saver"), ReadOnly]
#endif
		private int _cachedInstanceId = -1;

#if ODIN_INSPECTOR
		[FoldoutGroup("Saver"), ShowInInspector, ReadOnly]
#endif
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

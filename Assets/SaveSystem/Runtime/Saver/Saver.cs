using System;
using UnityEngine;
using Zenject;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace SaveSystem.Runtime {
	public abstract class Saver<T> : MonoBehaviour, ISaver<T> {
#if ODIN_INSPECTOR
		[FoldoutGroup("Saver"), SerializeField, OnValueChanged("GenerateKey")]
		private KeyType _keyType;

		[FoldoutGroup("Saver"), SerializeField]
		private string _key;
		public string Key => _key;

		[FoldoutGroup("Saver"), SerializeField, ReadOnly]
		private int _cachedInstanceId = -1;
#else
		[SerializeField]
		private KeyType _keyType;

		[SerializeField]
		private string _key;

		public string Key => _key;

		[SerializeField]
		private int _cachedInstanceId = -1;

#endif

		private SaveManager _saveManager;

		private void OnValidate() {
			// Key가 할당되지 않았거나, 인스턴스 아이디가 변경되었을 때(복제되었을 때)
			if (_key == null || _cachedInstanceId != GetInstanceID()) {
				GenerateKey();
				_cachedInstanceId = GetInstanceID();
			}
		}

		private void GenerateKey() {
			_key = _keyType switch {
				KeyType.Guid => Guid.NewGuid().ToString(),
				KeyType.Type_Name => $"{GetType().Name}_{name}",
				KeyType.Name => name,
				_ => throw new ArgumentOutOfRangeException()
			};
		}

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

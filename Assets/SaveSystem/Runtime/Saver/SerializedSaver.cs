#if ODIN_INSPECTOR
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace SaveSystem.Runtime {
	public abstract class SerializedSaver<T> : SerializedMonoBehaviour, ISaver<T> {
		[FoldoutGroup("Saver"), SerializeField, OnValueChanged("GenerateKey")]
		private KeyType _keyType;

		[FoldoutGroup("Saver"), SerializeField]
		private string _key;
		public string Key => _key;

		private SaveManager _saveManager;

#if UNITY_EDITOR
		private void OnValidate() {
			// Key가 할당되지 않았으면 새로 할당
			_key ??= _keyType switch {
				KeyType.Guid => Guid.NewGuid().ToString(),
				KeyType.Type_Name => $"{GetType().Name}_{name}",
				KeyType.Name => name,
				_ => throw new ArgumentOutOfRangeException()
			};
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
#endif

#if ODIN_INSPECTOR
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace SaveSystem.Runtime {
	public abstract class SerializedSaver<TValue, TScope> : SerializedMonoBehaviour, ISaver<TValue>
		where TScope : IScope {
		[FoldoutGroup("Saver"), SerializeField]
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
		private TScope _scope;

		[Inject]
		private void Construct(TScope scope) {
			_scope = scope;
			_scope.RegisterSaver(this);
		}

		private void OnDestroy() {
			_scope.UnregisterSaver(this);
		}

		public abstract void ApplyData(TValue data);

		public abstract TValue SaveData();

		public virtual void ResetData() { }
	}
}
#endif

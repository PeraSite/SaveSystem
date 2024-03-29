﻿using System;
using UnityEngine;
using Zenject;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace SaveSystem.Runtime {
	public abstract class Saver<TValue, TScope> : MonoBehaviour, ISaver<TValue> where TScope : IScope {
#if ODIN_INSPECTOR
		[FoldoutGroup("Saver"), OnValueChanged("OnValueChanged"), SerializeField]
		private KeyType _keyType;

		[FoldoutGroup("Saver"), EnableIf("IsKeyEditable"), SerializeField]
		private string _key;
		public string Key => _key;
#else
		[SerializeField]
		private KeyType _keyType;

		[SerializeField]
		private string _key;

		public string Key => _key;
#endif

#if UNITY_EDITOR
		private void OnValidate() {
			// Key가 할당되지 않았으면 새로 할당
			_key ??= _keyType switch {
				KeyType.Guid => Guid.NewGuid().ToString(),
				KeyType.Type_Name => $"{GetType().Name}_{name}",
				KeyType.Name or KeyType.Custom => name,
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

		protected virtual void Awake() {
			if (_scope == null) {
				Debug.LogError($"Saver object {name} is not injected {typeof(TScope).Name}> scope!");
				gameObject.SetActive(false);
			}
		}

		protected virtual void OnDestroy() {
			_scope?.UnregisterSaver(this);
		}

		public abstract void ApplyData(TValue data);

		public abstract TValue SaveData();

		public virtual void ResetData() { }

#if ODIN_INSPECTOR
		private void OnValueChanged() {
			_key = _keyType switch {
				KeyType.Guid => Guid.NewGuid().ToString(),
				KeyType.Type_Name => $"{GetType().Name}_{name}",
				KeyType.Name => name,
				_ => ""
			};
		}

		private bool IsKeyEditable() => _keyType == KeyType.Custom;
#endif
	}
}

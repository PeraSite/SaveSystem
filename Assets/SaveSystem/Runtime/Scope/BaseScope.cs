using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SaveSystem.Runtime {
	public abstract class BaseScope : IScope<Dictionary<string, object>> {
		public abstract string Key { get; }

		public Dictionary<string, ISaver> Savers { get; } = new();
		public Dictionary<string, object> Snapshot { get; set; } = new();

		public virtual void ApplyData(Dictionary<string, object> data) {
			Debug.Log($"[{Key} Scope] ApplyData {new OdinDataSerializer().Serialize(data)}");

			foreach (var (key, value) in data) {
				if (!Savers.TryGetValue(key, out var saver)) {
					Debug.Log($"[{Key} Scope] No saver found for key {key}");
					continue;
				}
				saver.ApplyDataWeak(value);
			}
			Snapshot = data;
		}

		public virtual Dictionary<string, object> SaveData() {
			var result = new Dictionary<string, object>(Snapshot);
			var capturedState = Savers.ToDictionary(pair => pair.Key, pair => pair.Value.SaveDataWeak());

			foreach (var (key, value) in capturedState) {
				Debug.Log($"[{Key} Scope] {key} -> {new OdinDataSerializer().Serialize(value)}");
				result[key] = value;
			}

			Snapshot = result;
			return result;
		}

		public virtual void ResetData() {
			foreach (var (_, saver) in Savers) {
				saver.ResetData();
			}
		}

		public void RegisterSaver(ISaver saver) {
			if (Savers.ContainsKey(saver.Key))
				throw new Exception($"Can't register same identifier saver: {saver.Key}");
			Savers[saver.Key] = saver;
			Debug.Log($"[{Key} Scope] Registered {saver.Key}");
		}

		public void UnregisterSaver(ISaver saver) {
			if (!Savers.ContainsKey(saver.Key))
				throw new Exception($"Can't unregister saver that not registered: {saver.Key}");
			Savers.Remove(saver.Key);
			Debug.Log($"[{Key} Scope] Unregistered {saver.Key}");
		}

		public void UnregisterAllSaver() {
			Savers.Clear();
			Debug.Log($"[{Key} Scope] Unregistered all");
		}

		public T GetSaver<T>() where T : ISaver {
			return (T) Savers.Values.FirstOrDefault(saver => saver.GetType() == typeof(T));
		}

		public TValue GetData<TSaver, TValue>() where TSaver : ISaver<TValue> {
			var saver = GetSaver<TSaver>();
			if (saver == null) return default;
			if (!Snapshot.TryGetValue(saver.Key, out var data)) return default;
			return (TValue) data;
		}
	}

	public abstract class BaseScope<TValue> : IScope<TValue> {
		public abstract string Key { get; }
		public abstract TValue Snapshot { get; set; }

		public Dictionary<string, ISaver> Savers { get; } = new();

		public virtual void ResetData() {
			foreach (var (_, saver) in Savers) {
				saver.ResetData();
			}
		}

		public abstract void ApplyData(TValue data);
		public abstract TValue SaveData();

		public void RegisterSaver(ISaver saver) {
			if (Savers.ContainsKey(saver.Key))
				throw new Exception($"Can't register same identifier saver: {saver.Key}");
			Savers[saver.Key] = saver;
			Debug.Log($"[{Key} Scope] Registered {saver.Key}");
		}

		public void UnregisterSaver(ISaver saver) {
			if (!Savers.ContainsKey(saver.Key))
				throw new Exception($"Can't unregister saver that not registered: {saver.Key}");
			Savers.Remove(saver.Key);
			Debug.Log($"[{Key} Scope] Unregistered {saver.Key}");
		}

		public void UnregisterAllSaver() {
			Savers.Clear();
			Debug.Log($"[{Key} Scope] Unregistered all");
		}

		public T GetSaver<T>() where T : ISaver {
			return (T) Savers.Values.FirstOrDefault(saver => saver.GetType() == typeof(T));
		}
	}
}

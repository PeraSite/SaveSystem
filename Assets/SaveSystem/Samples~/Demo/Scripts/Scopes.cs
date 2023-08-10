using System;
using System.Collections.Generic;
using System.Linq;
using SaveSystem.Runtime;
using UnityEngine;
using Zenject;

namespace SaveSystem.Samples {
	public class RootScope : BaseScope, IInitializable, IDisposable {
		public override string Key => "Root";

		private GlobalScope _globalScope;
		private SlotScope _slotScope;

		public void Initialize() {
			RegisterSaver(_globalScope);
			RegisterSaver(_slotScope);
		}

		public void Dispose() {
			UnregisterAllSaver();
		}

		[Inject]
		private void Construct(GlobalScope globalScope, SlotScope slotScope) {
			_globalScope = globalScope;
			_slotScope = slotScope;
		}
	}

	public class GlobalScope : BaseScope {
		public override string Key => "Global";
	}

	public class SlotScope : BaseScope<Dictionary<int, Dictionary<string, object>>>, IInitializable {
		// Constants
		public const int NOT_SELECTED_SLOT = -1;
		public const int EDITOR_SLOT = 999;

		// Identifier
		public override string Key => "Slot";
		public override Dictionary<int, Dictionary<string, object>> Snapshot { get; set; } = new();

		// States
		public int CurrentSlot = EDITOR_SLOT; // 빌드 시 메인화면에서 -1로 설정함

		[Inject] private IDataSerializer _serializer;

		public override void ApplyData(Dictionary<int, Dictionary<string, object>> data) {
			Debug.Log($"[{Key} Scope] ApplyData {_serializer.Serialize(data)}");

			Snapshot = data;

			// 현재 슬롯이 지정되지 않으면 리턴
			if (CurrentSlot == NOT_SELECTED_SLOT) return;

			// 지정된 현재 슬롯이 세이브에 없으면 리턴
			if (!data.TryGetValue(CurrentSlot, out var saverDataMap)) return;

			// 데이터 적용
			foreach (var (key, value) in saverDataMap) {
				if (!Savers.TryGetValue(key, out var saver)) continue;
				saver.ApplyDataWeak(value);
			}

		}

		public override Dictionary<int, Dictionary<string, object>> SaveData() {
			var result = new Dictionary<int, Dictionary<string, object>>(Snapshot);

			if (CurrentSlot != NOT_SELECTED_SLOT) {
				if (!result.ContainsKey(CurrentSlot)) {
					result[CurrentSlot] = new Dictionary<string, object>();
				}

				var capturedState = CaptureSaver();
				foreach (var (key, value) in capturedState) {
					result[CurrentSlot][key] = value;
					Debug.Log($"[{Key} Scope] Slot {CurrentSlot} / {key} -> {_serializer.Serialize(value)}");
				}
			}

			Snapshot = result;
			return result;
		}

		private Dictionary<string, object> CaptureSaver() {
			return Savers.ToDictionary(pair => pair.Key, pair => pair.Value.SaveDataWeak());
		}

		public void Initialize() {
			RegisterSaver(new SaveMetadataSaver());
		}

		public TValue GetData<TSaver, TValue>(int slot) where TSaver : ISaver<TValue> {
			var saver = GetSaver<TSaver>();
			if (saver == null) return default;
			if (!Snapshot.TryGetValue(slot, out var data)) return default;
			if (!data.TryGetValue(saver.Key, out var value)) return default;
			return (TValue) value;
		}

		public SaveMetadata GetMetadata(int slot) {
			return GetData<SaveMetadataSaver, SaveMetadata>(slot);
		}
	}
}

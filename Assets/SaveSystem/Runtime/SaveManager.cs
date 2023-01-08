using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ModestTree;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SaveSystem.Runtime {
	public class SaveManager : IInitializable, IDisposable {
		// Dependencies
		[Inject] private IDataStorage _dataStorage;
		[Inject] private IDataSerializer _dataSerializer;
		[Inject] private ISceneTransition _sceneTransition;
		[Inject] private ZenjectSceneLoader _sceneLoader;

		// State
		[InjectOptional] public string CurrentSaveSlot = "Slot";
		public SaveData CurrentSaveData { get; private set; } = new();
		[InjectOptional] private readonly List<ISaver> _savers = new();

		public void MakeSnapshot() {
			// 씬 이름 저장
			CurrentSaveData.SceneName = SceneManager.GetActiveScene().name;

			// 데이터 저장
			foreach (var saver in _savers) {
				CurrentSaveData.Data[saver.Key] = _dataSerializer.Serialize(saver.SaveDataWeak());
			}

			Debug.Log("[SaveManager] Snapshot created");
		}

		public void ApplySnapshot() {
			// 데이터 로드
			foreach (var saver in _savers) {
				if (CurrentSaveData.Data.TryGetValue(saver.Key, out var data)) {
					var value = _dataSerializer.Deserialize(data);
					if (value == null) continue;
					saver.ApplyDataWeak(value);
				}
			}
			Debug.Log("[SaveManager] Snapshot applied");
		}

		public void Save() {
			MakeSnapshot();
			_dataStorage.Save(CurrentSaveSlot, CurrentSaveData);
			Debug.Log($"[SaveSystem] Saved to {CurrentSaveSlot}");
		}

		public void Load() {
			CurrentSaveData = _dataStorage.Load(CurrentSaveSlot);

			if (CurrentSaveData == null) {
				throw new Exception($"[SaveSystem] Failed to load {CurrentSaveSlot}");
			}

			ApplySnapshot();
			Debug.Log($"[SaveSystem] Loaded from {CurrentSaveSlot}");
		}

		public void Delete() {
			_dataStorage.Delete(CurrentSaveSlot);
			Debug.Log($"[SaveSystem] Deleted {CurrentSaveSlot}");
		}

		public void ResetData() {
			// 모든 Saver의 ResetData 호출
			foreach (var saver in _savers) {
				saver.ResetData();
			}

			Debug.Log($"[SaveSystem] Data reset");
		}

		public void RegisterSaver(ISaver saver) {
			Debug.Log($"[SaveSystem] Register: {saver.GetType().Name}");
			_savers.Add(saver);
		}

		public void UnregisterSaver(ISaver saver) {
			Debug.Log($"[SaveSystem] Unregister: {saver.GetType().Name}");
			_savers.Remove(saver);
		}

		public void Initialize() {
			// 저장된 데이터가 없으면 데이터 초기화
			if (_dataStorage.Has(CurrentSaveSlot)) {
				Load();
			} else {
				ResetData();
			}
		}

		public void Dispose() {
			Save();
		}

		public void ChangeScene(string sceneName) {
			ChangeSceneAsync(sceneName).Forget();
		}

		private async UniTask ChangeSceneAsync(string sceneName) {
			await _sceneTransition.StartTransition();
			MakeSnapshot();
			await _sceneLoader.LoadSceneAsync(sceneName);
			ApplySnapshot();
			await _sceneTransition.EndTransition();
		}
	}
}

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SaveSystem.Runtime {
	public class SaveManager : IInitializable, IDisposable {
		// Dependencies
		private IDataStorage _dataStorage;
		private IDataSerializer _dataSerializer;
		private ISceneTransition _sceneTransition;
		private ZenjectSceneLoader _sceneLoader;

		// State
		private List<ISaver> _savers = new();
		private SaveData _currentSaveData;
		private string _currentSaveSlot;

		public SaveData CurrentSaveData => _currentSaveData;

		public void MakeSnapshot() {
			// 씬 이름 저장
			_currentSaveData.SceneName = SceneManager.GetActiveScene().name;
			
			// 데이터 저장
			foreach (var saver in _savers) {
				_currentSaveData.Data[saver.Key] = _dataSerializer.Serialize(saver.SaveDataWeak());
			}
			
			Debug.Log("[SaveManager] Snapshot created");
		}

		public void ApplySnapshot() {
			// TODO: 씬 불러오기

			// 데이터 로드
			foreach (var saver in _savers) {
				if (_currentSaveData.Data.TryGetValue(saver.Key, out var data)) {
					var value = _dataSerializer.Deserialize(data);
					if (value == null) continue;
					saver.ApplyDataWeak(value);
				}
			}
			Debug.Log("[SaveManager] Snapshot applied");
		}

		public void Save() {
			MakeSnapshot();
			_dataStorage.Save(_currentSaveSlot, _currentSaveData);
			Debug.Log($"[SaveSystem] Saved to {_currentSaveSlot}");
		}

		public void Load() {
			_currentSaveData = _dataStorage.Load(_currentSaveSlot);

			if (_currentSaveData == null) {
				throw new Exception($"[SaveSystem] Failed to load {_currentSaveSlot}");
			}

			ApplySnapshot();
			Debug.Log($"[SaveSystem] Loaded from {_currentSaveSlot}");
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
			if (_dataStorage.Has(_currentSaveSlot)) {
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

		[Inject]
		public void Construct(IDataStorage dataStorage, IDataSerializer dataSerializer,
			ISceneTransition sceneTransition,
			ZenjectSceneLoader sceneLoader, string saveSlot = "Slot") {
			_dataStorage = dataStorage;
			_dataSerializer = dataSerializer;
			_sceneTransition = sceneTransition;
			_sceneLoader = sceneLoader;

			_currentSaveSlot = saveSlot;
			_currentSaveData = new SaveData();
		}
	}
}

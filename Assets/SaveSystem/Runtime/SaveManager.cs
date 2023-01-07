using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SaveSystem.Runtime {
	public class SaveManager : IInitializable, IDisposable {
		// Dependencies
		private ISaveLoader _saveLoader;
		private IDataSerializer _dataSerializer;
		private ISceneTransition _sceneTransition;
		private ZenjectSceneLoader _sceneLoader;

		// State
		private List<ISaver> _savers = new();
		private SaveData _currentSaveData;
		private string _currentSaveSlot;

		public void Save() {
			// TODO: 데이터 메모리에 캐싱 후 캐싱된 것 저장 
			// 새 데이터 만들기
			var saveData = new SaveData();

			// 모든 Saver 데이터 가져오기
			foreach (var saver in _savers) {
				saveData.SceneName = SceneManager.GetActiveScene().name;
				var data = saver.SaveDataWeak();
				saveData.Data[saver.Key] = _dataSerializer.Serialize(data);
			}

			// 가져온 데이터 저장하기
			_saveLoader.Save(_currentSaveSlot, saveData);

			Debug.Log($"[SaveSystem] Saved to {_currentSaveSlot}");
		}

		public void Load() {
			// 데이터 불러오기
			var saveData = _saveLoader.Load(_currentSaveSlot);

			if (saveData == null) {
				throw new Exception($"[SaveSystem] Failed to load {_currentSaveSlot}");
			}

			// TODO: 저장된 씬으로 이동

			// 모든 Saver 데이터 적용
			foreach (var saver in _savers) {
				if (saveData.Data.TryGetValue(saver.Key, out var data)) {
					saver.ApplyDataWeak(_dataSerializer.Deserialize(data));
				}
			}

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
			Debug.Log("[SaveSystem] Initialized");

			// 저장된 데이터가 없으면 데이터 초기화
			if (_saveLoader.Has(_currentSaveSlot)) {
				Load();
			} else {
				ResetData();
			}
		}

		public void Dispose() {
			Debug.Log("[SaveSystem] Disposed");
			Save();
		}

		public void ChangeScene(string sceneName) {
			ChangeSceneAsync(sceneName).Forget();
		}

		private async UniTask ChangeSceneAsync(string sceneName) {
			await _sceneTransition.StartTransition();
			Save();
			await _sceneLoader.LoadSceneAsync(sceneName);
			Load();
			await _sceneTransition.EndTransition();
		}

		[Inject]
		public void Construct(ISaveLoader saveLoader, IDataSerializer dataSerializer, ISceneTransition sceneTransition,
			ZenjectSceneLoader sceneLoader, string saveSlot = "Slot") {
			_saveLoader = saveLoader;
			_dataSerializer = dataSerializer;
			_sceneTransition = sceneTransition;
			_sceneLoader = sceneLoader;

			_currentSaveSlot = saveSlot;
			_currentSaveData = new SaveData();
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SaveSystem.Runtime {
	public class SaveManager : IInitializable, IDisposable {
		public const string SAVE_FILE_KEY = "save";

		// Dependencies
		[Inject] private readonly IDataStorage _dataStorage;
		[Inject] private readonly IDataSerializer _dataSerializer;
		[Inject] private readonly SceneTransitionManager _sceneTransitionManager;
		[InjectOptional] private readonly List<ISaver> _savers = new();

		// State
		public SaveData CurrentSnapshot { get; private set; }
		[InjectOptional] public string SlotName = "Default Slot";

		public void MakeSnapshot() {
			// 씬 이름 저장
			CurrentSnapshot.SceneName = SceneManager.GetActiveScene().name;

			// 슬롯에 데이터 없으면 새로 생성
			if (!CurrentSnapshot.Data.ContainsKey(SlotName)) {
				CurrentSnapshot.Data.Add(SlotName, new Dictionary<string, object>());
			}

			// 데이터 저장
			foreach (var saver in _savers) {
				CurrentSnapshot.Data[SlotName][saver.Key] = saver.SaveDataWeak();
			}

			Debug.Log("[SaveManager] Snapshot created");
		}

		public void ApplySnapshot() {
			if (!CurrentSnapshot.Data.TryGetValue(SlotName, out var dataMap)) {
				ResetSavers();
				return;
			}

			// 데이터 로드
			foreach (var saver in _savers) {
				if (dataMap.TryGetValue(saver.Key, out var value)) {
					saver.ApplyDataWeak(value);
				}
			}
			Debug.Log("[SaveManager] Snapshot applied");
		}

		public void ResetSavers() {
			// 모든 Saver의 ResetData 호출
			foreach (var saver in _savers) {
				saver.ResetData();
			}
			Debug.Log("[SaveManager] All savers reset");
		}

		public void Save() {
			var serializedSaveData = _dataSerializer.Serialize(CurrentSnapshot);
			_dataStorage.Save(SAVE_FILE_KEY, serializedSaveData);
			Debug.Log($"[SaveSystem] Data Saved : {serializedSaveData}");
		}

		public void Load() {
			var saveDataString = _dataStorage.Load(SAVE_FILE_KEY);
			CurrentSnapshot = _dataSerializer.Deserialize<SaveData>(saveDataString);
			Debug.Log($"[SaveSystem] Data Loaded : {saveDataString}");
		}

		public void Delete() {
			_dataStorage.Delete(SAVE_FILE_KEY);
			Debug.Log($"[SaveSystem] Data Deleted");
		}

		public void ResetData() {
			// CurrentSnapshot 초기화
			CurrentSnapshot = new SaveData();
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
			if (_dataStorage.Has(SAVE_FILE_KEY)) {
				Load();
				ApplySnapshot();
			} else {
				ResetData();
				ResetSavers();
			}
		}

		public void Dispose() {
			MakeSnapshot();
			Save();
		}

#region Scene Management
		public void StartGame(string startSceneName) {
			_sceneTransitionManager.ChangeScene(startSceneName, afterSceneChange: ApplySnapshot);
		}

		public void NewGame(string startSceneName) {
			_sceneTransitionManager.ChangeScene(startSceneName, afterSceneChange: ResetSavers);
		}

		public void ChangeScene(string sceneName) {
			_sceneTransitionManager.ChangeScene(sceneName, MakeSnapshot, ApplySnapshot);
		}
#endregion
	}
}

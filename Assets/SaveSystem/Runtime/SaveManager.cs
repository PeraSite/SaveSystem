using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SaveSystem.Runtime {
	public class SaveManager : IInitializable, IDisposable {
		// Constants
		private const string SAVE_KEY = "DummySave";

		// Dependencies
		private IDataStorage _dataStorage;
		private IDataSerializer _dataSerializer;
		private IScope<Dictionary<string, object>> _rootScope;
		private SceneTransitionManager _transitionManager;

		// States
		// BUG: SaveManager#Snapshot이 _rootScope#Snapshot과 동기화되지 않음
		public Dictionary<string, object> Snapshot = new();

		public void ApplySnapshot() {
			_rootScope.ApplyData(Snapshot);
			Debug.Log("[SaveManager] ApplySnapshot");
		}

		public void CaptureSnapshot() {
			Snapshot = _rootScope.SaveData();
			Debug.Log($"[SaveManager] CaptureSnapshot: {_dataSerializer.Serialize(Snapshot)}");
		}

		public void Load() {
			// Storage에서 불러와 캐시에 저장
			var loaded = _dataStorage.Load(SAVE_KEY);
			var serialized = _dataSerializer.Deserialize<Dictionary<string, object>>(loaded);
			Snapshot = serialized ?? throw new Exception($"Can't serialized: {loaded}");
			Debug.Log($"[SaveManager] Load: {loaded}");

			// 저장된 캐시 적용
			ApplySnapshot();
		}

		public void Save() {
			// 캐시 업데이트
			CaptureSnapshot();

			// 캐시를 Storage에 저장
			var saved = _dataSerializer.Serialize(Snapshot);
			_dataStorage.Save(SAVE_KEY, saved);
			Debug.Log($"[SaveManager] Save: {saved}");
		}

		public void Reset() {
			_rootScope.ResetData();
			Debug.Log("[SaveManager] Reset");
		}

		public void Initialize() {
			if (_dataStorage.Has(SAVE_KEY)) {
				Load();
			} else {
				Reset();
			}
		}

		public void Dispose() {
			Save();
		}

		[Inject]
		public void Construct(IDataStorage dataStorage,
			IDataSerializer dataSerializer,
			SceneTransitionManager transitionManager,
			IScope<Dictionary<string, object>> rootScope) {
			_dataStorage = dataStorage;
			_dataSerializer = dataSerializer;
			_rootScope = rootScope;
			_transitionManager = transitionManager;
		}

#region Scene Management
		public void StartGame(string startSceneName) {
			_transitionManager.ChangeScene(startSceneName, CaptureSnapshot, afterSceneChange: ApplySnapshot);
		}

		public void NewGame(string startSceneName) {
			_transitionManager.ChangeScene(startSceneName, CaptureSnapshot, afterSceneChange: Reset);
		}

		public void ChangeScene(string sceneName) {
			_transitionManager.ChangeScene(sceneName, CaptureSnapshot, ApplySnapshot);
		}
#endregion
	}
}

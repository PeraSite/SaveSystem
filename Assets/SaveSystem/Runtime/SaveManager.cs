using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace SaveSystem.Runtime {
	public class SaveManager : IInitializable, IDisposable {
		// Constants
		private const string SAVE_KEY = "DummySave";

		// Dependencies
		private IDataStorage _dataStorage;
		private IDataSerializer _dataSerializer;
		private SceneTransitionManager _transitionManager;
		public IScope<Dictionary<string, object>> RootScope;

		public void ApplySnapshot() {
			RootScope.ApplyData(RootScope.Snapshot);
			Debug.Log("[SaveManager] ApplySnapshot");
		}

		public void CaptureSnapshot() {
			RootScope.SaveData();
			Debug.Log($"[SaveManager] CaptureSnapshot: {_dataSerializer.Serialize(RootScope.Snapshot)}");
		}

		public void Load() {
			// Storage에서 불러와 캐시에 저장
			var stopwatch = System.Diagnostics.Stopwatch.StartNew();
			var loaded = _dataStorage.Load(SAVE_KEY);
			var serialized = _dataSerializer.Deserialize<Dictionary<string, object>>(loaded);
			var snapshot = serialized ?? throw new Exception($"Can't serialized: {loaded}");
			Debug.Log($"[SaveManager] Load: {loaded}");


			// 로드된 Snapshot 적용
			RootScope.ApplyData(snapshot);
			Debug.Log($"[SaveManager] Load takes {stopwatch.ElapsedMilliseconds}ms");
		}

		public void Save() {
			var stopwatch = System.Diagnostics.Stopwatch.StartNew();
			// 캐시 업데이트
			CaptureSnapshot();

			// 캐시를 Storage에 저장
			var saved = _dataSerializer.Serialize(RootScope.Snapshot);
			_dataStorage.Save(SAVE_KEY, saved);
			Debug.Log($"[SaveManager] Save: {saved}");
			Debug.Log($"[SaveManager] Save takes {stopwatch.ElapsedMilliseconds}ms");
		}

		public void Reset() {
			RootScope.ResetData();
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
			RootScope = rootScope;
			_transitionManager = transitionManager;
		}

#region Scene Management
		public void StartGame(string startSceneName) {
			_transitionManager.ChangeScene(startSceneName, CaptureSnapshot, ApplySnapshot);
		}

		public void NewGame(string startSceneName) {
			_transitionManager.ChangeScene(startSceneName, CaptureSnapshot, Reset);
		}

		public void ChangeScene(string sceneName) {
			_transitionManager.ChangeScene(sceneName, CaptureSnapshot, ApplySnapshot);
		}
#endregion
	}
}

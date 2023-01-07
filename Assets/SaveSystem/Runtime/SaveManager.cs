using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SaveSystem.Runtime {
	public class SaveManager : ScriptableObjectInstaller<SaveManager>, IInitializable, IDisposable {
		private ISaveLoader _saveLoader;
		private IDataSerializer _dataSerializer;
		private List<ISaver> _savers = new();

		//TODO: Custom slot name
		private const string currentSaveName = "Slot 1";

		[Button]
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
			_saveLoader.Save(currentSaveName, saveData);
		}

		[Button]
		public void Load() {
			// 저장된 데이터가 없으면 데이터 초기화
			if (!_saveLoader.Has(currentSaveName)) {
				ResetData();
				return;
			}

			// 데이터 불러오기
			var saveData = _saveLoader.Load(currentSaveName);

			// TODO: 씬 변경
			// if (saveData.SceneName != SceneManager.GetActiveScene().name) {
			// 	SceneManager.LoadScene(saveData.SceneName);
			// }

			// 모든 Saver 데이터 적용
			foreach (var saver in _savers) {
				if (saveData.Data.TryGetValue(saver.Key, out var data)) {
					saver.ApplyDataWeak(_dataSerializer.Deserialize(data));
				}
			}
		}

		[Button]
		public void ResetData() {
			foreach (var saver in _savers) {
				saver.ResetData();
			}
		}

		public void RegisterSaver(ISaver saver) {
			Debug.Log($"[SaveSystem] Registering saver {saver.GetType().GetNiceName()}");
			_savers.Add(saver);
		}

		public void UnregisterSaver(ISaver saver) {
			Debug.Log($"[SaveSystem] Unregistering saver {saver.GetType().GetNiceName()}");
			_savers.Remove(saver);
		}

		public void Initialize() {
			Debug.Log("[SaveSystem] Initialized");
			Load();
		}

		public void Dispose() {
			Debug.Log("[SaveSystem] Disposed");
			Save();
		}

#region DI
		public override void InstallBindings() {
			Container.BindInterfacesAndSelfTo<SaveManager>().FromInstance(this).AsSingle();
		}

		[Inject]
		public void Construct(ISaveLoader saveLoader, IDataSerializer dataSerializer) {
			_saveLoader = saveLoader;
			_dataSerializer = dataSerializer;
		}
#endregion
	}
}

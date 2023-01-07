using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace SaveSystem.Runtime {
	public class SaveManager : ScriptableObjectInstaller<SaveManager>, IInitializable, IDisposable {
		public SaveData CurrentSave;

		private ISaveLoader _saveLoader;
		private IDataSerializer _dataSerializer;
		private List<ISaver> _savers = new();

		[Button]
		public void Save() { }

		[Button]
		public void Load() { }

		[Button]
		public void Restart() { }

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
		}

		public void Dispose() {
			_savers.Clear();
			Debug.Log("[SaveSystem] Disposed");
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

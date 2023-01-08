#if ODIN_INSPECTOR
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SaveSystem.Runtime;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using Zenject;
using SerializationUtility = Sirenix.Serialization.SerializationUtility;

namespace SaveSystem.Editor {
	public class SaveEditor : OdinEditorWindow {
		[ShowInInspector, ReadOnly]
		private SaveData SaveData => SaveManager?.CurrentSnapshot;

		[ButtonGroup("Storage", VisibleIf = "@SaveManager != null")]
		private void Save() {
			SaveManager?.Save();
		}

		[ButtonGroup("Storage")]
		private void Load() {
			SaveManager?.Load();
		}

		[ButtonGroup("Storage")]
		private void Delete() {
			SaveManager?.Delete();
		}

		[ButtonGroup("Snapshot", VisibleIf = "@SaveManager != null")]
		private void MakeSnapshot() {
			SaveManager?.MakeSnapshot();
		}

		[ButtonGroup("Snapshot")]
		private void ApplySnapshot() {
			SaveManager?.ApplySnapshot();
		}

		[Button]
		private SerializedSaveData GetSaveData() {
			var dataStorage = new PlayerPrefDataStorage();
			var dataSerializer = new OdinDataSerializer();
			var saveData = dataSerializer.Deserialize<SaveData>(dataStorage.Load(SaveManager.SAVE_FILE_KEY));
			return new SerializedSaveData(saveData);
		}

		[HideReferenceObjectPicker]
		private struct SerializedSaveData {
			public string SceneName;

			[HideReferenceObjectPicker]
			public readonly Dictionary<string, Dictionary<string, ValueWrapper>> Data;

			public SerializedSaveData(SaveData original) {
				SceneName = "";
				Data = new Dictionary<string, Dictionary<string, ValueWrapper>>();

				foreach (var (identifier, valueMap) in original.Data) {
					Data[identifier] = valueMap.ToDictionary(pair => pair.Key,
						pair => new ValueWrapper {
							value = SerializationUtility.DeserializeValueWeak(Encoding.UTF8.GetBytes(pair.Value),
								DataFormat.JSON)
						});
				}
			}

			[InlineProperty]
			public struct ValueWrapper {
				[HideReferenceObjectPicker, HideLabel]
				public object value;
			}
		}

#region Boilerplate
		[MenuItem("Tools/Save System/Save Editor")]
		public static void Open() {
			var window = GetWindow<SaveEditor>();
			window.titleContent = new GUIContent("Save Editor");
			window.Show();
		}

		[ShowInInspector, EnableIf("HasProjectContext"),
		 InfoBox("No project context found.", "@!HasProjectContext()", Icon = SdfIconType.ExclamationCircle)]
		private SaveManager _saveManager;

		private SaveManager SaveManager {
			get {
				if (_saveManager != null) return _saveManager;
				_saveManager = ProjectContext.HasInstance
					? ProjectContext.Instance.Container.TryResolve<SaveManager>()
					: null;
				return _saveManager;
			}
		}

		private bool HasProjectContext() {
			return ProjectContext.HasInstance;
		}
#endregion
	}
}

#endif

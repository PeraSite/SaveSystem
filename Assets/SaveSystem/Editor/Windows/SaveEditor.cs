#if ODIN_INSPECTOR
using SaveSystem.Runtime;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace SaveSystem.Editor {
	public class SaveEditor : OdinEditorWindow {
		[ShowInInspector, ReadOnly]
		private string SaveSlot => SaveManager?.CurrentSaveSlot;

		[ShowInInspector, ReadOnly]
		private SaveData SaveData => SaveManager?.CurrentSaveData;

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

#if ODIN_INSPECTOR
using System.Collections.Generic;
using SaveSystem.Runtime;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace SaveSystem.Editor {
	public class SaveEditor : OdinEditorWindow {
		[ShowInInspector, EnableIf("@SaveManager != null"), OnValueChanged("OnSaveDataChanged", true)]
		private Dictionary<string, object> SaveData {
			get => SaveManager?.RootScope.Snapshot;
			set {
				if (SaveManager != null)
					SaveManager.RootScope.Snapshot = value;
			}
		}

		[ButtonGroup("Storage", VisibleIf = "@SaveManager != null")]
		private void Save() {
			SaveManager?.Save();
		}

		[ButtonGroup("Storage")]
		private void Load() {
			SaveManager?.Load();
		}

		[ButtonGroup("Snapshot", VisibleIf = "@SaveManager != null")]
		private void MakeSnapshot() {
			SaveManager?.CaptureSnapshot();
		}

		[ButtonGroup("Snapshot")]
		private void ApplySnapshot() {
			SaveManager?.ApplySnapshot();
		}

		private void OnSaveDataChanged() {
			ApplySnapshot();
		}

#region Boilerplate
		[MenuItem("Tools/Save System/Save Editor")]
		public static void Open() {
			var window = GetWindow<SaveEditor>();
			window.titleContent = new GUIContent("Save Editor");
			window.Show();
		}

		protected override void OnEnable() {
			base.OnEnable();
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		protected override void OnDisable() {
			base.OnDisable();
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		private void OnPlayModeStateChanged(PlayModeStateChange state) {
			if (state == PlayModeStateChange.EnteredEditMode) {
				_saveManager = null;
			}
		}

		[ShowInInspector, EnableIf("HasProjectContext"),
		 InfoBox("No project context found.", "@!HasProjectContext()", Icon = SdfIconType.ExclamationCircle)]
		private SaveManager _saveManager;

		private SaveManager SaveManager {
			get {
				if (_saveManager != null) return _saveManager;
				_saveManager = HasProjectContext()
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

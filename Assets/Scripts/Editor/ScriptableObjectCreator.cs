#if ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;


namespace PeraCore.Editor {
	public class ScriptableObjectCreator : OdinMenuEditorWindow {
		private static readonly HashSet<Type> scriptableObjectTypes = new HashSet<Type>(AssemblyUtilities
			.GetTypes(AssemblyTypeFlags.CustomTypes)
			.Where(t =>
				t.IsClass &&
				typeof(ScriptableObject).IsAssignableFrom(t) &&
				!typeof(EditorWindow).IsAssignableFrom(t) &&
				!typeof(UnityEditor.Editor).IsAssignableFrom(t)));

		[MenuItem("Assets/Create Scriptable Object", priority = -1000)]
		private static void ShowDialog() {
			var path = "Assets";
			var obj = Selection.activeObject;
			if (obj && AssetDatabase.Contains(obj)) {
				path = AssetDatabase.GetAssetPath(obj);
				if (!Directory.Exists(path)) {
					path = Path.GetDirectoryName(path);
				}
			}

			var window = CreateInstance<ScriptableObjectCreator>();
			window.ShowUtility();
			window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
			window.titleContent = new GUIContent(path);
			window.targetFolder = path.Trim('/');
		}

		private ScriptableObject previewObject;
		private string targetFolder;
		private Vector2 scroll;

		private Type SelectedType {
			get {
				var m = MenuTree.Selection.LastOrDefault();
				return m?.Value as Type;
			}
		}

		protected override OdinMenuTree BuildMenuTree() {
			MenuWidth = 270;
			WindowPadding = Vector4.zero;

			var tree = new OdinMenuTree(false);
			tree.Config.DrawSearchToolbar = true;
			tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;
			tree.AddRange(scriptableObjectTypes.Where(x => !x.IsAbstract), GetMenuPathForType).AddThumbnailIcons();
			tree.SortMenuItemsByName();
			tree.Selection.SelectionConfirmed += x => CreateAsset();
			tree.Selection.SelectionChanged += e => {
				if (previewObject && !AssetDatabase.Contains(previewObject)) {
					DestroyImmediate(previewObject);
				}

				if (e != SelectionChangedType.ItemAdded) {
					return;
				}

				var t = SelectedType;
				if (t != null && !t.IsAbstract) {
					previewObject = CreateInstance(t);
				}
			};

			return tree;
		}

		private string GetMenuPathForType(Type t) {
			if (t != null && scriptableObjectTypes.Contains(t)) {
				var name = t.Name.Split('`').First().SplitPascalCase();
				return GetMenuPathForType(t.BaseType) + "/" + name;
			}

			return "";
		}

		protected override IEnumerable<object> GetTargets() {
			yield return previewObject;
		}

		protected override void DrawEditor(int index) {
			scroll = GUILayout.BeginScrollView(scroll);
			{
				base.DrawEditor(index);
			}
			GUILayout.EndScrollView();

			if (previewObject) {
				GUILayout.FlexibleSpace();
				SirenixEditorGUI.HorizontalLineSeparator();
				if (GUILayout.Button("Create Asset", GUILayoutOptions.Height(30))) {
					CreateAsset();
				}
			}
		}

		private void CreateAsset() {
			if (previewObject) {
				var dest = targetFolder + "/" + MenuTree.Selection.First().Name + ".asset";
				dest = AssetDatabase.GenerateUniqueAssetPath(dest);
				AssetDatabase.CreateAsset(previewObject, dest);
				AssetDatabase.Refresh();
				Selection.activeObject = previewObject;
				EditorApplication.delayCall += Close;
			}
		}
	}
}

  #endif

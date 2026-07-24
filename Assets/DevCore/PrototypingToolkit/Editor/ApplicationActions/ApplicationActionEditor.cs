using DevCore.Core;
using DevCore.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace DevCore.PrototypingToolkit.Editor {
	[CustomEditor(typeof(ApplicationAction))]
	public class ApplicationActionEditor : CompositeAssetEditor {
		protected override void OnCompositeAssetFooterInpectorGUI() {
			if (AppCore.IsRunning()) {
				DrawExecuteGUI();
			}
		}

		private void DrawExecuteGUI() {
			DevCoreEditorGUILayout.SeparatorBar();
			if (GUILayout.Button("Execute")) {
				(target as ApplicationAction).Execute();
			}
		}
	}
}
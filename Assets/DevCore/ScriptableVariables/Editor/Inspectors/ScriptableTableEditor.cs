using UnityEditor;
using UnityEngine;

namespace DevCore.ScriptableVariables.Editor {
	// [CustomEditor(typeof(ScriptableTableBase<,>), true)]
	public class ScriptableTableEditor : UnityEditor.Editor {
		// private static bool m_IsDrawingTab = false;
		// public static bool isDrawingTab => m_IsDrawingTab;
		
		
		public override void OnInspectorGUI() {
			// m_IsDrawingTab = true;
			base.OnInspectorGUI();
			// m_IsDrawingTab = false;			
		}
	}
}
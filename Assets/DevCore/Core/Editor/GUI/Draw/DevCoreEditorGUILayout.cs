using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevCore.Core.Editor {
	public static class DevCoreEditorGUILayout {
		private static GUIContent m_EmptyContent = new GUIContent();
		
		public static void SeparatorBar() {
			GUILayout.Space(6f);
			GUILayout.Box(m_EmptyContent, GUILayout.Height(3f), GUILayout.Width(EditorGUIUtility.currentViewWidth - 25f));
			GUILayout.Space(6f);
		}	
	}
}
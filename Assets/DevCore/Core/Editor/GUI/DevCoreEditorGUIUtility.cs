using UnityEditor;
using UnityEngine;

namespace DevCore.Core.Editor {
	public static class DevCoreEditorGUIUtility {
		#region Styles
		public static class Styles {
			public static readonly GUIStyle italicLabel = new GUIStyle(EditorStyles.label)
				{fontStyle = FontStyle.Italic};
		}
		#endregion


		#region Currents
		private static Object m_LastSelectedObject = null;
		#endregion


		#region Callbacks
		static DevCoreEditorGUIUtility() {
			Selection.selectionChanged -= OnSelectionChanged;
			Selection.selectionChanged += OnSelectionChanged;
		}

		private static void OnSelectionChanged() {
			m_LastSelectedObject = null;
		}
		#endregion


		#region Project View
		public static void SelectObject(Object target, bool ping = true) {
			Selection.objects = new[] {target};
			if (ping) {
				EditorGUIUtility.PingObject(target);
			}

			m_LastSelectedObject = target;
		}

		/// <summary>
		/// Ping an object when this method is called once and select it if it's called twice on the same object
		/// </summary>
		/// <param name="target"></param>
		public static void SoftSelectObject(Object target) {
			if (m_LastSelectedObject != target) {
				m_LastSelectedObject = target;
				EditorGUIUtility.PingObject(target);
			} else {
				if (Selection.activeObject != target) {
					SelectObject(target);
				}
			}
		}
		#endregion
	}
}
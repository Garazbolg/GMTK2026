using System.Collections;
using DevCore.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace DevCore.ScriptableVariables.Editor {
	
	[CustomPropertyDrawer(typeof(ScriptableVariableBase), true)]
	public class ScriptableVariableBaseInspector : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			ScriptableVariableGUI.DrawGUI(position, property, label);
		}
	}
}
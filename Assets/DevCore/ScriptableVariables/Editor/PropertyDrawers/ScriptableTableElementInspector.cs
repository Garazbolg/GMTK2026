using UnityEditor;
using UnityEngine;

namespace DevCore.ScriptableVariables.Editor {
	[CustomPropertyDrawer(typeof(ScriptableTableElement<>))]
	public class ScriptableTableElementInspector : PropertyDrawer {
		private const float k_Spacing = 3f;
		private GUIContent m_VariableContent = new GUIContent();

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return base.GetPropertyHeight(property, label) + k_Spacing;
		}

		public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
			rect.height -= k_Spacing;

			//Draw name
			property.Next(true);
			SerializedProperty name = property.Copy();
			float labelWidth = EditorGUIUtility.labelWidth;
			Rect nameRect = new Rect(rect.position, new Vector2(labelWidth - 5f, rect.size.y));
			EditorGUI.PropertyField(nameRect, name, GUIContent.none);

			//Draw variable field
			property.Next(false);
			SerializedProperty variable = property.Copy();
			Rect fieldRect =
				new Rect(new Vector2(rect.position.x + labelWidth, rect.position.y),
					new Vector2(rect.width - labelWidth, rect.size.y));

			var propertyRef = property.objectReferenceValue;
			float variableLabelWidth;
			if (propertyRef == null) {
				m_VariableContent.text = "";
				m_VariableContent.tooltip = string.Empty;
				variableLabelWidth = 0f;
			} else {
				m_VariableContent.text = "...";
				m_VariableContent.tooltip = propertyRef.name;
				variableLabelWidth = 15f;
			}

			ScriptableVariableDrawSetting drawSetting = ScriptableVariableDrawSetting.Default;
			drawSetting.labelWidth = variableLabelWidth;
			drawSetting.appendNameToLabel = false;
			
			ScriptableVariableGUI.DrawGUI(fieldRect, variable, m_VariableContent, drawSetting);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DevCore.Core.Editor {
	[CustomPropertyDrawer(typeof(RequireTypeAttribute))]
	public class RequireTypeAttributeDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			var previousValue = property.objectReferenceValue;
			EditorGUI.PropertyField(position, property, label);
			var targetType = (attribute as RequireTypeAttribute).type;
			if (property.objectReferenceValue != null && !property.objectReferenceValue.GetType().IsAssignableFrom(targetType)) {
				property.objectReferenceValue = previousValue;
				Debug.LogError($"Cannot reference a value that is not of type {nameof(targetType)}");
			}
		}
	}
}
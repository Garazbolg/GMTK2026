using System.Collections;
using System.Collections.Generic;
using DevCore.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace DevCore.ScriptableVariables.Editor
{
    [CustomPropertyDrawer(typeof(BindableValue<>), false)]
    public class BindableValueInspector : PropertyDrawer {
        private bool m_Initialized = false;
        private bool m_IsBindingValue = false;
        private SerializedProperty m_ValueProperty;
        private SerializedProperty m_VariableProperty;

        internal static class Contents {
            internal static bool initialized = false;

            internal static GUIContent linkButtonContent;
            internal static GUIStyle linkButtonStyle;
            
            
            
            internal static void Initialize() {
                linkButtonContent = EditorGUIUtility.IconContent("d_Linked");
                linkButtonContent.tooltip = "Link variable";
                linkButtonStyle = new GUIStyle("Button");
                linkButtonStyle.padding = new RectOffset(1, 1, 0, 0);
                initialized = true;
            }
        }
        
        private void Initialize(SerializedProperty wrapperProperty) {
            m_ValueProperty = wrapperProperty.FindPropertyRelative("m_Value");
            m_VariableProperty = wrapperProperty.FindPropertyRelative("m_Variable");
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (!m_Initialized) {
                Initialize(property);
            }

            if (!Contents.initialized) {
                Contents.Initialize();
            }

            if (m_VariableProperty.objectReferenceValue == null) {
                if (m_IsBindingValue) {
                    DrawLinkToggle(position, out Rect propertyRect);
                    DrawVariableField(propertyRect, label);
                } else {
                    DrawLinkToggle(position, out Rect propertyRect);
                    EditorGUI.PropertyField(propertyRect, m_ValueProperty, label);
                }
            } else {
                if (m_IsBindingValue) {
                    m_IsBindingValue = false;
                }
                DrawVariableField(position, label);
            }
        }

        private void DrawLinkToggle(Rect position, out Rect propertyRect) {
            var scope = new GUIHorizontalScope(position);
            float buttonWidth = 25f;
            float fieldWidth = scope.GetRemainingSpace(buttonWidth, 1, false);
            propertyRect = scope.GetInsertedRect(fieldWidth, true);
            m_IsBindingValue = GUI.Toggle(scope.GetInsertedRect(buttonWidth), m_IsBindingValue,
                Contents.linkButtonContent, Contents.linkButtonStyle);
        }

        private void DrawVariableField(Rect position, GUIContent label) {
            ScriptableVariableGUI.DrawGUI(position, m_VariableProperty, label);
        }
    }
}

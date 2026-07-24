using System;
using System.Collections;
using System.Collections.Generic;
using DevCore.Core.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace DevCore.ScriptableVariables.Editor {
	public struct ScriptableVariableDrawSetting {
		public bool appendNameToLabel;
		public float labelWidth;
		
		public static ScriptableVariableDrawSetting Default => new ScriptableVariableDrawSetting()
		{
			appendNameToLabel = true,
			labelWidth = float.PositiveInfinity 
		};
	}
	
	
	public static class ScriptableVariableGUI {
		private static GUIContent fieldLabel = new GUIContent();

		private struct DrawContext {
			public Rect position;
			public SerializedProperty property;
			public GUIContent label;
			public ScriptableVariableDrawSetting drawSetting;

			public DrawContext(Rect position, SerializedProperty property, GUIContent label, ScriptableVariableDrawSetting drawSetting) {
				this.position = position;
				this.property = property;
				this.label = label;
				this.drawSetting = drawSetting;
			}
		}
		
		internal static class Contents {
			internal static bool initialized = false;
			internal static GUIContent selectButtonContent;
			internal static GUIStyle selectButtonStyle;
			internal static GUIContent unlinkButtonContent;
			internal static GUIStyle unlinkButtonStyle;

			internal static void Initialize() {
				selectButtonStyle = new GUIStyle("Button");
				selectButtonStyle.padding = new RectOffset(1, 1, 2, 0);
				selectButtonContent = EditorGUIUtility.IconContent("d_Selectable Icon");
				selectButtonContent.tooltip = "Select linked variable";
				unlinkButtonContent = EditorGUIUtility.IconContent("d_Unlinked");
				unlinkButtonStyle = new GUIStyle(selectButtonStyle);
				unlinkButtonStyle.padding = new RectOffset(1, 1, 0, 0);
				unlinkButtonContent.tooltip = "Unlink variable";
				initialized = true;
			}
		}


		public static void DrawGUI(Rect position, SerializedProperty property, GUIContent label) {
			DrawGUI(position, property, label, ScriptableVariableDrawSetting.Default);
		}
		
		public static void DrawGUI(Rect position, SerializedProperty property, GUIContent label, 
			ScriptableVariableDrawSetting drawSetting) {
			
			if (!Contents.initialized) {
				Contents.Initialize();
			}

			var context = new DrawContext(position, property, label, drawSetting);
			bool hasValue = property.objectReferenceValue != null;

			if (property.objectReferenceValue != null) {
				var obj = property.objectReferenceValue;
				SerializedObject so = new SerializedObject(obj);
				var valueProp = so.FindProperty("m_Value");

				if (valueProp != null) {
					DrawValueField(context, valueProp);
				} else {
					DrawFallbackField(context);
				}
			} else {
				DrawEmptyField(position, property, label, context);
			}
		}


		#region Draw Modes
		private static void DrawFallbackField(DrawContext context) {
			UpdateLabel(true, context.label.text, string.Empty, context.label.tooltip, context);
			
			context.property.serializedObject.Update();
			DrawProperty(context.position, context.property, context);
			context.property.serializedObject.ApplyModifiedProperties();
		}
		
		private static void DrawValueField(DrawContext context, SerializedProperty value) {
			//Draw the sub value field
			string ownerName = context.property.IsArrayElement()
				? string.Empty
				: context.label.text;
			UpdateLabel(true, ownerName, context.property.objectReferenceValue.name, 
				context.label.tooltip, context);

			//Draw field
			var scope = new GUIHorizontalScope(context.position);
			float buttonWidth = 24f;
			float fieldSpace = scope.GetRemainingSpace(buttonWidth * 2, 1, false);
			Rect propertyRect = scope.GetInsertedRect(fieldSpace, true);

			value.serializedObject.Update();
			DrawProperty(propertyRect, value, context);

			//Select and unlink buttons
			if (GUI.Button(scope.GetInsertedRect(buttonWidth), Contents.selectButtonContent,
				    Contents.selectButtonStyle)) {
				DevCoreEditorGUIUtility.SoftSelectObject(context.property.objectReferenceValue);
			}

			if (GUI.Button(scope.GetInsertedRect(buttonWidth), Contents.unlinkButtonContent,
				    Contents.unlinkButtonStyle)) {
				context.property.objectReferenceValue = null;
			}
			value.serializedObject.ApplyModifiedProperties();
		}

		private static void DrawEmptyField(Rect position, SerializedProperty property, GUIContent label, 
			DrawContext context) {
			UpdateLabel(false, label.text, string.Empty, label.tooltip, context);
			EditorGUI.PropertyField(position, property, fieldLabel);
		}

		private static void DrawProperty(Rect position, SerializedProperty value, DrawContext context) {
			if (context.drawSetting.labelWidth < EditorGUIUtility.labelWidth) {
				using (new GUILabelOverrideWidthScope(context.drawSetting.labelWidth)) {
					EditorGUI.PropertyField(position, value, fieldLabel);	
				}
			} else {
				EditorGUI.PropertyField(position, value, fieldLabel);
			}
		}
		#endregion


		#region Label
		private static void UpdateLabel(bool hasValue, string wrapperName, string variableName, string tooltip, 
			DrawContext context) {
			if (hasValue) {
				if (context.drawSetting.appendNameToLabel) {
					if (wrapperName.Length > 0) {
						fieldLabel.text = wrapperName + $" ({variableName})";
					} else {
						fieldLabel.text = variableName;
					}
				} else {
					fieldLabel.text = wrapperName;
				}
			} else {
				fieldLabel.text = wrapperName;
			}
		}
		#endregion
	}
}
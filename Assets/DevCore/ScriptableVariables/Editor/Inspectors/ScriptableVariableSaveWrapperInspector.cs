using System.IO;
using DevCore.Core;
using DevCore.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace DevCore.ScriptableVariables.Editor {
	[CustomEditor(typeof(ScriptableVariableSaveWrapper))]
	public class ScriptableVariableSaveWrapperInspector : UnityEditor.Editor {
		#region Currents
		private ScriptableVariableSaveWrapper m_Target = null;
		#endregion


		#region Serialized Properties
		private SerializedProperty m_VariableProp = null;
		private SerializedProperty m_FileNameProp = null;
		private SerializedProperty m_ExtensionProp = null;
		private SerializedProperty m_SubDirectoryProp = null;
		private SerializedProperty m_FileFormatProp = null;
		#endregion


		#region Callbacks
		private void OnEnable() {
			if (target == null) {
				return; 
			}
			
			m_Target = target as ScriptableVariableSaveWrapper;
			InitializeProperties();
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();
			DrawGUI();
			serializedObject.ApplyModifiedProperties();
		}
		#endregion

		#region GUI
		private void DrawGUI() {
			EditorGUILayout.PropertyField(m_VariableProp);
			EditorGUILayout.PropertyField(m_FileFormatProp);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Path", EditorStyles.boldLabel);
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(m_FileNameProp);
			EditorGUILayout.LabelField(".", GUILayout.Width(4f));
			EditorGUILayout.PropertyField(m_ExtensionProp, GUIContent.none, GUILayout.Width(60f));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.PropertyField(m_SubDirectoryProp);
			
			//Draw full path
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Target path: ", GUILayout.Width(EditorGUIUtility.labelWidth));
			EditorGUILayout.LabelField(m_Target.GetSavePath(), DevCoreEditorGUIUtility.Styles.italicLabel);
			
			if (GUILayout.Button("../", GUILayout.Width(40f))) {
				EditorUtility.RevealInFinder(Application.persistentDataPath);
			}
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(12f);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Save")) {
				m_Target.Save();
			}

			if (GUILayout.Button("Load")) {
				m_Target.Load();
			}

			EditorGUILayout.EndHorizontal();

		}	
		#endregion


		#region Data
		private void InitializeProperties() {
			m_VariableProp = serializedObject.FindProperty("m_Variables");
			m_FileNameProp = serializedObject.FindProperty("m_FileName");
			m_ExtensionProp = serializedObject.FindProperty("m_Extension");
			m_SubDirectoryProp = serializedObject.FindProperty("m_SubDirectory");
			m_FileFormatProp = serializedObject.FindProperty("m_FileFormat");
		}	
		#endregion
	}
}
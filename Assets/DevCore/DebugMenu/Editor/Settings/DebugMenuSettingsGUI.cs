using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevCore.DebugMenu.Editor {
	public static class DebugMenuSettingsGUI {
		#region Currents
		private static SerializedObject m_SettingAsset = null;
		private static SerializedProperty m_EnableDebugMenuProp = null;
		private static SerializedProperty m_InfoRefreshSkipFrameProp = null;
		private static SerializedProperty m_RefreshedInfosPerFramesProp = null;
		#endregion


		#region Construction
		[SettingsProvider]
		public static SettingsProvider CreateSettingsProvider() {
			if (m_SettingAsset == null) {
				m_SettingAsset = new SerializedObject(DebugMenuSettings.GetAsset());
				m_EnableDebugMenuProp = m_SettingAsset.FindProperty("m_EnableDebugMenu");
				m_InfoRefreshSkipFrameProp = m_SettingAsset.FindProperty("m_InfoRefreshSkipFrame");
				m_RefreshedInfosPerFramesProp = m_SettingAsset.FindProperty("m_RefreshedInfosPerFrames");
			}
			
			return new SettingsProvider("Project/Dev Core/Debug Menu", SettingsScope.Project)
			{
				label = "Debug Menu",
				guiHandler = OnSettingGUI
			};
		}
		#endregion


		#region GUI
		private static void OnSettingGUI(string searchContext) {
			GUILayout.Space(12f);
			EditorGUI.indentLevel ++;
			m_SettingAsset.Update();
			EditorGUIUtility.labelWidth += 40f;
			EditorGUILayout.PropertyField(m_EnableDebugMenuProp);
			GUILayout.Space(6f);

			EditorGUILayout.PropertyField(m_InfoRefreshSkipFrameProp);
			EditorGUILayout.PropertyField(m_RefreshedInfosPerFramesProp);
			EditorGUIUtility.labelWidth -= 40f;
			m_SettingAsset.ApplyModifiedProperties();
			
			GUILayout.Space(20f);
			using (new EditorGUILayout.VerticalScope("HelpBox")) {
				DebugMenuEditorUtility.DrawDocumentation();
			}
			
			EditorGUI.indentLevel --;

			GUILayout.Space(15f);
			if (GUILayout.Button("Show Setup Wizard", GUILayout.Width(140f))) {
				DebugMenuSetupWizard.ShowSetupWizard();
			}
			
		}
		#endregion
	}
}
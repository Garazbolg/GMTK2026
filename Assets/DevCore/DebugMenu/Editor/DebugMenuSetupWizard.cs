using System;
using System.Collections.Generic;
using DevCore.Core.Editor;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace DevCore.DebugMenu.Editor {
	public class DebugMenuSetupWizard : EditorWindow {
		#region Currents
		private static DebugMenuSetupWizard m_CurrentSetupWizard = null;
		private static GUIStyle m_HeaderStyle;
		private static GUIStyle m_BoldLabelStyle;
		private static GUIStyle m_WrapLabelStyle;
		private static bool m_IsGuiInit = false;
		#endregion
		
		#region Window
		[MenuItem("DevCore/Debug Menu/Setup")]
		internal static void ShowSetupWizard() {
			m_CurrentSetupWizard = EditorWindow.GetWindow<DebugMenuSetupWizard>(true, "Debug Menu Setup", true);
			var rect = m_CurrentSetupWizard.position;
			rect.height = 230f;
			m_CurrentSetupWizard.position = rect;
				
			CompilationPipeline.compilationStarted -= OnCompilationStarts;
			CompilationPipeline.compilationStarted += OnCompilationStarts;
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanaged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanaged;
		}
		#endregion

		#region Callbacks
		private void OnGUI() {
			if (!m_IsGuiInit) {
				InitGUI();
				m_IsGuiInit = true;
			}

			GUILayout.Space(10f);
			using (new EditorColorUtils.GUIColorScope(GUI.skin.customStyles[0].normal.textColor)) {
				EditorGUILayout.LabelField("Debug Menu Setup", m_HeaderStyle);
			}
			
			GUILayout.Space(4f);
			EditorGUILayout.LabelField("Go to <b>ProjectSettings/DevCore/Dev Menu</b> to Enable/Disable the debug menu.", m_WrapLabelStyle);
			
			GUILayout.Space(15f);

			DebugMenuEditorUtility.DrawDocumentation();

			GUILayout.Space(30f);
			var buttonRect = position;
			buttonRect.x = 2f;
			buttonRect.width -= 4f;
			buttonRect.y = position.height - 32f;
			buttonRect.height = 30f;
			if (GUI.Button(buttonRect, "Complete Setup")) {
				SetupInputsIfNeeded();
				Close();
				DebugMenuSettings.GetAsset().hasBeenSetup = true;
			}
		}

		private static void OnPlayModeStateChanaged(PlayModeStateChange state) {
			if (state == PlayModeStateChange.EnteredPlayMode || state == PlayModeStateChange.ExitingEditMode) {
				if (m_CurrentSetupWizard != null) {
					m_CurrentSetupWizard.Close();
				}
			}
		}

		private void InitGUI() {
			m_BoldLabelStyle = EditorStyles.boldLabel;
			m_HeaderStyle = new GUIStyle(GUI.skin.label)
			{
				alignment = TextAnchor.MiddleCenter,
				fontStyle = FontStyle.Bold,
				fontSize = 13
			};

			m_WrapLabelStyle = new GUIStyle(GUI.skin.label)
			{
				wordWrap = true,
				richText = true
			};
		}

		private static void OnCompilationStarts(object obj) {
			if (m_CurrentSetupWizard != null) {
				m_CurrentSetupWizard.Close();
			}
		}
		#endregion


		#region GUI
		private void SetupInputsIfNeeded() {
			var inputEntries = new List<InputManagerEntry>()
			{
				new InputManagerEntry
				{
					name = "DebugMenu/Draw", kind = InputManagerEntry.Kind.KeyOrButton, btnPositive = "p",
					altBtnPositive = "joystick button 8"
				},
				new InputManagerEntry
				{
					name = "DebugMenu/Hold", kind = InputManagerEntry.Kind.KeyOrButton, btnPositive = "p",
					altBtnPositive = "joystick button 4"
				},
				new InputManagerEntry
				{
					name = "DebugMenu/Previous", kind = InputManagerEntry.Kind.KeyOrButton, btnPositive = "mouse 1",
					altBtnPositive = "joystick button 1"
				},
			};

			InputRegistering.RegisterInputs(inputEntries);
			DebugMenuSettings.GetAsset().enableDebugMenu = true;
		}
		#endregion
	}
}
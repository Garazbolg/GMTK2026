using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DevCore.DebugMenu.Editor {
    public static class DebugMenuEditorUtility {
	    private static GUIStyle m_BoldLabelStyle;
	    private static GUIStyle m_WrapLabelStyle;

	    private static bool m_IsGUIInit = false;
	    
        public static void DrawDocumentation() {
		    CheckGUIInit();
		    
		    string ColorBold(string color, string text) => $"<color={color}><b>{text}</b></color>";

            string GrB(string str) => ColorBold("#84de83", str);
            string CyB(string str) => ColorBold("#89dbe0", str);
            string ViB(string str) => ColorBold("#d89ce6", str);
            string OrB(string str) => ColorBold("#d4a053", str);
            string BlB(string str) => ColorBold("#3d76cc", str);

            EditorGUILayout.LabelField($"Use {GrB("P")} to toggle the menu with {GrB("Keyboard")} or " +
                                       $"{CyB("L + Dpad Up")} with {CyB("Gamepad")}", m_WrapLabelStyle);

            GUILayout.Space(10f);
			
            string strField = BlB("string");

            EditorGUILayout.LabelField("Register Method : ", m_BoldLabelStyle);
            EditorGUILayout.LabelField($"{OrB("DebugActionHandle")} {OrB("DebugMenu")}.{ViB("RegisterAction")}" +
                                       $"({strField} {GrB("path")}, {OrB("DebugAction")} {GrB("action")});",
                m_WrapLabelStyle);


            GUILayout.Space(5);
            EditorGUILayout.LabelField("Register Info : ", m_BoldLabelStyle);
            EditorGUILayout.LabelField($"{OrB("DebugInfoHandle")} {OrB("DebugMenu")}.{ViB("RegisterInfo")}" +
                                       $"({strField} {GrB("path")}, {OrB("DebugInfo")} {GrB("action")});", m_WrapLabelStyle);
        }

	    private static void CheckGUIInit() {
		    if (m_IsGUIInit) return;

		    m_BoldLabelStyle = EditorStyles.boldLabel;
		    
		    m_WrapLabelStyle = new GUIStyle(GUI.skin.label)
		    {
			    wordWrap = true,
			    richText = true
		    };
		    
		    m_IsGUIInit = true;
	    }
    }
}
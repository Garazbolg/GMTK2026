using UnityEditor;
using UnityEngine;

namespace DevCore.Core.Editor {
    public static class EditorLoadingIcon {
        private const float FRAME_DURATION = 0.1f;

        private static GUIContent[] m_SpinIcons = new GUIContent[] {
            EditorGUIUtility.IconContent("d_WaitSpin00"),
            EditorGUIUtility.IconContent("d_WaitSpin01"),
            EditorGUIUtility.IconContent("d_WaitSpin02"),
            EditorGUIUtility.IconContent("d_WaitSpin03"),
            EditorGUIUtility.IconContent("d_WaitSpin04"),
            EditorGUIUtility.IconContent("d_WaitSpin05"),
            EditorGUIUtility.IconContent("d_WaitSpin06"),
            EditorGUIUtility.IconContent("d_WaitSpin07"),
            EditorGUIUtility.IconContent("d_WaitSpin08"),
            EditorGUIUtility.IconContent("d_WaitSpin09"),
            EditorGUIUtility.IconContent("d_WaitSpin10"),
            EditorGUIUtility.IconContent("d_WaitSpin11")
        };


        public static void Draw(string tooltip, float speed = 1f, params GUILayoutOption[] guiLayoutOptions) {
            int frameId = Mathf.FloorToInt((Time.realtimeSinceStartup * speed) / FRAME_DURATION);
            frameId %= 12;

            var content = m_SpinIcons[frameId];
            content.tooltip = tooltip;
            EditorGUILayout.LabelField(content, guiLayoutOptions);
        }
    }
}
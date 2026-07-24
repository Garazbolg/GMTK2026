using System;
using UnityEngine;

namespace DevCore.Core.Editor {
    public static class EditorColorUtils {
        #region Currents
        private static Color m_LastGUIColor = Color.clear;
        #endregion


        #region Override Color
        public static void StartGUIColor(Color color) {
            m_LastGUIColor = GUI.color;
            GUI.color = color;
        }

        public static void EndGUIColor() {
            GUI.color = m_LastGUIColor;
        }
        
        public class GUIColorScope : IDisposable {
            public GUIColorScope(Color color) {
                EditorColorUtils.StartGUIColor(color);
            }

            public void Dispose() {
                EditorColorUtils.EndGUIColor();
            }
        }
        #endregion
    }

    
}
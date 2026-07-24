using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevCore.Core.Editor
{
    /// <summary>
    /// Overides editor label dimensions in the scope of the object
    /// </summary>
    public class GUILabelOverrideWidthScope : IDisposable {
        private float m_OldWidth;
        
        public GUILabelOverrideWidthScope(float widthOverride) {
            m_OldWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = widthOverride;
        }

        public void Dispose() {
            EditorGUIUtility.labelWidth = m_OldWidth;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DevCore.Core.Editor
{
    /// <summary>
    /// Contains debug test system to execute a specified method on an object referenced in the field drawn with the <see cref="DrawGUI"/> method 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EditorObjectTester<T> where T : Object {
        private string m_Name = string.Empty;
        private bool m_RuntimeOnly = false;
        private T m_LastTested = null;
        private Func<T, bool> m_IsObjectValidFunction;
        private Action<T> m_OnTestObject;

        public EditorObjectTester(string name, bool runtimeOnly, Func<T, bool> isObjectValidFunction, Action<T> onTestObject) {
            m_Name = name;
            m_RuntimeOnly = runtimeOnly;
            m_IsObjectValidFunction = isObjectValidFunction;
            m_OnTestObject = onTestObject;
        }

        public EditorObjectTester(string name, bool runtimeOnly, Action<T> onTestObject) {
            m_Name = name;
            m_RuntimeOnly = runtimeOnly;
            m_OnTestObject = onTestObject;
        }

        public void DrawGUI() {
            if (m_RuntimeOnly && !AppCore.IsRunning()) {
                return;
            }
            
            DevCoreEditorGUILayout.SeparatorBar();
            T testObject = null;
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                EditorGUILayout.LabelField(m_Name, EditorStyles.boldLabel);
                EditorGUILayout.Space(2f);
                using (new EditorGUILayout.HorizontalScope()) {
                    testObject = EditorGUILayout.ObjectField(m_LastTested, typeof(T), true) as T;
                    if (testObject != null && testObject != m_LastTested && IsObjectValid(testObject)) {
                        m_OnTestObject?.Invoke(testObject);
                        m_LastTested = testObject;
                    }

                    if (m_LastTested) {
                        if (GUILayout.Button("Repeat", GUILayout.Width(60f))) {
                            m_OnTestObject?.Invoke(m_LastTested);
                        }
                    }
                }
                EditorGUILayout.Space(1f);
            }
        }

        
        
        private bool IsObjectValid(T testObject) {
            if (m_IsObjectValidFunction == null) {
                return true;
            } else {
                return m_IsObjectValidFunction.Invoke(testObject);
            }
        }
    }
}

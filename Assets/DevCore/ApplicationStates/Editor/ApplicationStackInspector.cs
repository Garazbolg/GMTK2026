using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DevCore.ApplicationStates;
using DevCore.Core.Editor;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;

namespace DevCore.ApplicationStates.Editor {
    [CustomEditor(typeof(ApplicationStackManager))]
    public class ApplicationStackInspector : UnityEditor.Editor {
        #region Properties
        private static List<ApplicationState> m_CurrentStates = new List<ApplicationState>(50);

        private bool m_Initialized = false;
        private static GUIContent m_PlayingContent;
        private static GUIContent m_PauseContent;
        
        private static GUIContent[] m_InitializingContent = new GUIContent[12];

        private static GUIStyle m_PriorStyle;
        private static GUIStyle m_NormalStyle;
        #endregion


        #region Callbacks
        private void OnEnable() {
            if (!m_Initialized) {
                m_PlayingContent = new GUIContent(EditorGUIUtility.IconContent("d_PlayButton")) {
                    tooltip = "Updating"
                };
                
                m_PauseContent = new GUIContent(EditorGUIUtility.IconContent("d_PauseButton")) {
                    tooltip = "Not updating"
                };
                
                m_PriorStyle = EditorStyles.boldLabel;
                m_NormalStyle = EditorStyles.label;
                
                m_Initialized = true;
            }

            EditorApplication.update += Repaint;
        }
        

        private void OnDisable() {
            EditorApplication.update -= Repaint;
        }
        
        public override void OnInspectorGUI() {
            ApplicationStack.GetStatesNonAlloc(m_CurrentStates);

            using (new EditorGUILayout.VerticalScope("HelpBox")) {
                if (m_CurrentStates.Count > 0) {
                    DrawStateField(m_CurrentStates[0], 0, m_PriorStyle);

                    for (int i = 1; i < m_CurrentStates.Count; i++) {
                        DrawStateField(m_CurrentStates[i], i, m_NormalStyle);
                    }
                } else {
                    EditorGUILayout.LabelField("No State Applied", EditorStyles.boldLabel);
                }
            }
        }
        #endregion


        #region State Display
        private void DrawStateField(ApplicationState state, int stateId, GUIStyle style) {
            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.LabelField(state.priority.ToString(), style, GUILayout.Width(40f));
                EditorGUILayout.LabelField(state.name, style, GUILayout.MinWidth(60f));

                EditorGUILayout.LabelField(state.category.ToString(), style, GUILayout.MaxWidth(80f), GUILayout.MinWidth(20f));
                
                if (!state.initialized) {
                    using (new EditorColorUtils.GUIColorScope(EditorColors.Light.Color())) {
                        EditorLoadingIcon.Draw("Initializing",2f, GUILayout.Width(20f));
                    }
                } 
                
                EditorColors color;
                GUIContent content;
                
                if (state.isUpdating) {
                    color = state.paused ? EditorColors.Yellow: EditorColors.Green;
                    content = m_PlayingContent;
                } else {
                    color = EditorColors.Orange;
                    content = m_PauseContent;
                }
                
                using (new EditorColorUtils.GUIColorScope(color.Color())) {
                    EditorGUILayout.LabelField(content, 
                        GUILayout.Width(20f));
                }
                
                GUILayout.Space(4f);
                
                using (new EditorColorUtils.GUIColorScope(EditorColors.Red.Color())) {
                    if (GUILayout.Button("Kill", GUILayout.Width(40f))) {
                        state.EndState();
                    }
                }
            }
        }
        #endregion
    }
}
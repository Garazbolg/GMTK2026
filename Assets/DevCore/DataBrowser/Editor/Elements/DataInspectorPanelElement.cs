using System;
using System.Collections;
using System.Collections.Generic;
using DevCore.Core.Editor;
using DevCore.DataBrowser.Editor;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

internal class DataInspectorPanelElement : ScrollView, IDataBrowserElement{
    public string serializationKey => "data-browser.data-inspector-panel";
    
    private Object m_InspectedObject;
    private Editor m_CurrentEditor; 
    private IMGUIContainer m_IMGUIContainer;

    
    public DataInspectorPanelElement() : base(ScrollViewMode.Vertical){
        style.minWidth = 300f;
        
        m_IMGUIContainer = new IMGUIContainer(OnInspectorGUI);
        Add(m_IMGUIContainer);
    }

    public void SetInspectedObject(string guid, bool pingObject) {
        var obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(guid));

        if (obj == null) {
            return;
        }
        
        DevCoreEditor.CreateCachedEditor(obj, ref m_CurrentEditor);
        m_InspectedObject = obj;
        MarkDirtyRepaint();
        if (pingObject) {
            EditorGUIUtility.PingObject(obj);
        }
    }
    
    
    private void OnInspectorGUI() {
        if (m_InspectedObject != null) {
            if (m_CurrentEditor != null) {
                DrawInspector();
            } else {
                DrawEmptyArea();
            }
        } else {
            if (m_CurrentEditor != null) {
                m_CurrentEditor = null;
            }
            
            DrawEmptyArea();
        }
    }

    private void DrawEmptyArea() {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(" Select a data");
    }

    private void DrawInspector() {
        EditorGUILayout.Space();
        using (new GUILabelOverrideWidthScope(200f)) {
            m_CurrentEditor.DrawHeader();
            m_CurrentEditor.OnInspectorGUI();
        }
    }

    public string GetSerializedData() {
        return JsonUtility.ToJson(new SerializableData<float>(scrollOffset.y));
    }

    public void ApplySerializedData(string textData) {
        var data = JsonUtility.FromJson<SerializableData<float>>(textData);
        if (data != null) {
            verticalScroller.value = data.value;
        }
    }
}

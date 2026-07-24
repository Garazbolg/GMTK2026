using System;
using System.Collections.Generic;
using DevCore.Core.Editor;
using DevCore.DataBrowser.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DataBrowserWindow : EditorWindow {
	private static readonly GUIContent m_WindowTabContent = new GUIContent("Data Browser");
	private DataBrowserContext m_Context;
	private List<IDataBrowserElement> m_Elements = new List<IDataBrowserElement>();

	private class DataBrowserContext {
		private DataFiltersPanelElement m_FilterPanel;
		private DataListElement m_DataList;
		private DataInspectorPanelElement m_DataInspector;
		private DataBrowserToolbar m_Toolbar;
		
		public DataBrowserContext(DataFiltersPanelElement filterPanel, 
			DataListElement dataList, DataInspectorPanelElement inspector, DataBrowserToolbar toolbar) {
			m_FilterPanel = filterPanel;
			m_DataList = dataList;
			m_DataInspector = inspector;
			m_Toolbar = toolbar;

			m_FilterPanel.onFilterSelected += OnFilterSelected;
			m_DataList.onSelectData += OnSelectData;
		}


		~DataBrowserContext() {
			m_FilterPanel.onFilterSelected -= OnFilterSelected;
			m_DataList.onSelectData -= OnSelectData;
		}
		
		private void OnFilterSelected(DataFilterSetting filter, bool resetSelection) {
			m_DataList.SetList(filter.GetCache(), resetSelection);
		}

		private void OnSelectData(string guid, bool pingObject) {
			m_DataInspector.SetInspectedObject(guid, pingObject && m_Toolbar.pingSelection);
		}
	}
	
	[MenuItem(EditorConstants.k_MenuPath + "Data Browser")]
	public static void ShowWindow() {
		var window = GetWindow<DataBrowserWindow>();
		window.titleContent = m_WindowTabContent;
	}
	
	private void OnEnable() {
		AssemblyReloadEvents.beforeAssemblyReload += SaveSerializedData;
		EditorApplication.quitting += SaveSerializedData;
	}

	private void OnDisable() {
		AssemblyReloadEvents.beforeAssemblyReload -= SaveSerializedData;
		EditorApplication.quitting -= SaveSerializedData;
	}


	private void CreateGUI() {
		var toolBar = new DataBrowserToolbar();
		rootVisualElement.Add(toolBar);
		
		//Separator
		rootVisualElement.Add(new Box());
		
		var filtersPanel = new DataFiltersPanelElement();
		var dataList = new DataListElement(); 
		var dataEditor = new DataInspectorPanelElement(); 
		
		m_Elements.Add(filtersPanel);
		m_Elements.Add(dataList);
		m_Elements.Add(dataEditor);
		m_Elements.Add(toolBar);

		
		var dataContainer = new VisualElement();
		dataContainer.style.minHeight = 400f;
		DevCoreUIElements.AttachSplitViews(rootVisualElement, 150f, TwoPaneSplitViewOrientation.Horizontal, 
			filtersPanel, dataContainer);

		DevCoreUIElements.AttachSplitViews(dataContainer, 180f, TwoPaneSplitViewOrientation.Horizontal, 
			dataList, dataEditor);

		m_Context = new DataBrowserContext(filtersPanel, dataList, dataEditor, toolBar);
		LoadSerializedData();
	}

	private void LoadSerializedData() {
		foreach (var element in m_Elements) {
			string serializedData = EditorPrefs.GetString(element.serializationKey, "");
			if (serializedData == null) {
				serializedData = String.Empty;
			}
			element.ApplySerializedData(serializedData);
		}
	}

	private void SaveSerializedData() {
		foreach (var element in m_Elements) {
			var data = element.GetSerializedData();
			EditorPrefs.SetString(element.serializationKey, data);
		}
	}
}

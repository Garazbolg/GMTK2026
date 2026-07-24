using System;
using System.Collections.Generic;
using DevCore.DataBrowser.Editor;
using UnityEngine;
using UnityEngine.UIElements;


internal class DataFiltersPanelElement : ListView, IDataBrowserElement {
	public string serializationKey => "data-browser.data-filter-panel";
	private bool m_AutomaticSelectionFlag = false;
	
	public delegate void FilterSelectAction(DataFilterSetting filter, bool resetSelection);
	public event FilterSelectAction onFilterSelected;
	
	public DataFiltersPanelElement() {
		style.paddingTop = 3f;
		style.paddingBottom = 3f;
		style.paddingLeft = 2f;
		style.paddingRight = 2f;

		style.minWidth = 80f;
		
		DataFiltersCache.BuildFilterCache();
		DataFiltersCache.onCacheUpdated += OnAnyCacheUpdated;
		
		makeItem = MakeListItem;
		bindItem = BindListItem;
		itemsSource = DataFiltersCache.GetAbstractFiltersList();
		selectionChanged += OnSelectionChanged;
	}

	private void OnSelectionChanged(IEnumerable<object> obj) {
		OnSelectIndex();
	}

	private void OnSelectIndex() {
		if (selectedIndex >= 0f && itemsSource != null) {
			onFilterSelected?.Invoke(itemsSource[selectedIndex] as DataFilterSetting, !m_AutomaticSelectionFlag);
			ConsumeAutomaticSelection();
		}
	}

	~DataFiltersPanelElement() {
		DataFiltersCache.onCacheUpdated -= OnAnyCacheUpdated;
		selectionChanged -= OnSelectionChanged;
	}
	
	private void BindListItem(VisualElement element, int index) {
		(element as Label).text = DataFiltersCache.GetFiltersList()[index].displayName;
	}

	private VisualElement MakeListItem() {
		var label = new Label();
		label.style.marginLeft = 2f;
		label.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft);
		return label;
	}
	
	
	private void OnAnyCacheUpdated() {
		RefreshItems();
		MarkDirtyRepaint();
	}

	public string GetSerializedData() {
		
		return JsonUtility.ToJson(new SerializableData<int>(selectedIndex));
	}

	private void ConsumeAutomaticSelection() {
		m_AutomaticSelectionFlag = false;
	}
	
	public void ApplySerializedData(string textData) {
		var data = JsonUtility.FromJson<SerializableData<int>>(textData);
		if (data != null) {
			m_AutomaticSelectionFlag = true;
			selectedIndex = data.value;
		}
	}
}

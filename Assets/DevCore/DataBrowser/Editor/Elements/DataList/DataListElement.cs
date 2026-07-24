using System.Collections;
using System.Collections.Generic;
using DevCore.Core.Editor;
using DevCore.DataBrowser.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public delegate void SelectDataAction(string guid, bool pingObject);

internal class DataListElement : ListView, IDataBrowserElement {
    public string serializationKey => "data-browser.data-list-element";

    private bool m_AutomaticSelectionFlag = false;

    public event SelectDataAction onSelectData; 
    
    private class DataListItem : VisualElement {
        private Label m_Label;
        private Box m_Background;

        public string text {
            get {
                return m_Label.text;
            }
            set {
                m_Label.text = value;
            }
        }

        public void SetupItem(int index, string objectGuid) {
            text = DevCoreAssetUtility.GetAssetNameFromGUID(objectGuid);;
        }

        public DataListItem() {
            m_Background = new Box();
            m_Background.style.color = new StyleColor(new Color(0f, 0f, 0f));
            Add(m_Background);
            
            m_Label = new Label();
            m_Label.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft);
            m_Label.style.marginLeft = 5f;
            Add(m_Label);
        }
    }
    
    public DataListElement() {
        makeItem = MakeListItem;
        bindItem = BindListItem;
        itemsSource = default;
        selectionType = SelectionType.Single;

        style.minWidth = 120f;
        
        selectionChanged += OnSelectionChanged;
        FilteredAssetsCache.onAnyCacheUpdated += RefreshItems;
        FilteredAssetsCache.onAnyCachedAssetMoveOrRename += RefreshItems;
    }

    public void SetList(IList list, bool resetSelection) {
        if (resetSelection) {
            selectedIndex = -1;
        }
        
        itemsSource = list;
        RefreshItems();
    }

    ~DataListElement() {
        selectionChanged -= OnSelectionChanged;
        FilteredAssetsCache.onAnyCacheUpdated -= RefreshItems;
        FilteredAssetsCache.onAnyCachedAssetMoveOrRename -= RefreshItems;
    }
    
    private void OnSelectionChanged(IEnumerable<object> item) {
        if (selectedIndex >= 0f && itemsSource != null) {
            onSelectData?.Invoke(itemsSource[selectedIndex] as string, !m_AutomaticSelectionFlag);
            ConsumeAutomaticSelection();
        }
    }

    private VisualElement MakeListItem() {
        return new DataListItem();
    }

    private void BindListItem(VisualElement element, int index) {
        var item  = (element as DataListItem);
        item.SetupItem(index, itemsSource[index].ToString());
    }


    private void ConsumeAutomaticSelection() {
        m_AutomaticSelectionFlag = false;
    }
    
    public string GetSerializedData() {
        return JsonUtility.ToJson(new SerializableData<int>(selectedIndex));
    }

    public void ApplySerializedData(string textData) {
        var data = JsonUtility.FromJson<SerializableData<int>>(textData);
        if (data != null) {
            m_AutomaticSelectionFlag = true;
            selectedIndex = data.value;
        }
    }
}

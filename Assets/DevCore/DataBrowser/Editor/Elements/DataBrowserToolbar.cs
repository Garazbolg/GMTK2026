using DevCore.DataBrowser.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DataBrowserToolbar : VisualElement, IDataBrowserElement {
	private ToolbarToggle m_PingObjectOnSelectToggle = new ToolbarToggle();

	public string serializationKey => "data-browser.toolbar";
	public bool pingSelection => m_PingObjectOnSelectToggle.value;
	
	public DataBrowserToolbar() {
		style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
		
		var saveButton = new ToolbarButton(OnSaveClicked)
		{
			text = "Save Assets"
		};
		saveButton.style.alignSelf = new StyleEnum<Align>(Align.FlexStart);
		Add(saveButton);

		Add(new ToolbarSpacer());
		
		m_PingObjectOnSelectToggle = new ToolbarToggle()
		{
			text = "Ping Selection"
		};
		m_PingObjectOnSelectToggle.style.alignSelf = new StyleEnum<Align>(Align.FlexStart);
		Add(m_PingObjectOnSelectToggle);
	}

	private void OnSaveClicked() {
		AssetDatabase.SaveAssets();
	}

	public string GetSerializedData() {
		return JsonUtility.ToJson(new SerializableData<bool>(m_PingObjectOnSelectToggle.value));
	}

	public void ApplySerializedData(string textData) {
		var data = JsonUtility.FromJson<SerializableData<bool>>(textData);
		if (data != null) {
			m_PingObjectOnSelectToggle.value = data.value;
		}
	}
}

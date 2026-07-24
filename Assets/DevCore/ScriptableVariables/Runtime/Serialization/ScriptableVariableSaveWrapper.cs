using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using DevCore.Core;
using DevCore.Core.Serialization;
using UnityEngine;

namespace DevCore.ScriptableVariables {
	[CreateAssetMenu(fileName = "SAVE_", menuName = SVConsts.SV_UTILS_PATH + "Save Wrapper",
		order = SVConsts.ASSET_ORDER_UTILITIES)]
	public class ScriptableVariableSaveWrapper : ScriptableObject {
		#region Constants
		private const int k_DefaultCacheCapacity = 40;
		#endregion


		#region Settings
		[SerializeField] private ScriptableVariableBase[] m_Variables = {}; 
		[SerializeField] private string m_FileName = "Save";
		[SerializeField] private string m_Extension = "sav";
		[SerializeField] private string m_SubDirectory = string.Empty;
		[SerializeField] private FileFormat m_FileFormat = FileFormat.JSON;
		#endregion


		#region Nested Types
		[System.Serializable]
		private class KeyValueDatasWrapper {
			public List<KeyValueData> datas = new List<KeyValueData>();
		}

		[System.Serializable]
		private struct KeyValueData {
			public string key;
			public string value;

			public KeyValueData(string key, string value) {
				this.key = key;
				this.value = value;
			}
		}
		#endregion


		#region Currents
		private static Dictionary<string, string> m_CachedSerializableObjects =
			new Dictionary<string, string>(k_DefaultCacheCapacity);
		#endregion


		#region Callbacks
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void OnSubsystemRegistration() {
			m_CachedSerializableObjects = new Dictionary<string, string>(k_DefaultCacheCapacity);
		}
		#endregion


		#region Data Management
		/// <summary>
		/// Resets all the referenced variables
		/// </summary>
		/// <param name="save">Force a save after the reset</param>
		public void ResetFile(bool save) {
			for (int i = 0; i < m_Variables.Length; i++) {
				var variable = m_Variables[i];
				if (variable != null) {
					m_Variables[i].ResetValue();
				}
			}

			if (save) {
				Save();
			}
		}
		
		/// <summary>
		/// Loads data at wrapper's target path
		/// </summary>
		/// <returns></returns>
		public void Load() {
			FileInfo fileInfo = GetFileInfo();
			KeyValueDatasWrapper wrapper;
			switch (m_FileFormat) {
				case FileFormat.JSON:
					string json = SaveUtility.ReadText(fileInfo);
					wrapper = JsonUtility.FromJson<KeyValueDatasWrapper>(json);
					break;
				default: //Read binary by default
					byte[] bytes = SaveUtility.ReadBytes(fileInfo);
					using (MemoryStream ms = new MemoryStream(bytes)) {
						var formatter = new BinaryFormatter();
						wrapper = (KeyValueDatasWrapper) formatter.Deserialize(ms);
					}
					break;
			}

			for (int i = 0; i < wrapper.datas.Count; i++) {
				var item = wrapper.datas[i];
				if (m_CachedSerializableObjects.ContainsKey(item.key)) {
					Debug.LogError($"[{name}] Serialized multiple objects with the same key, data cannot be deserialized");
					m_CachedSerializableObjects.Clear();
					return;
				} else {
					m_CachedSerializableObjects.Add(item.key, item.value);
				}
			}

			for (int i = 0; i < m_Variables.Length; i++) {
				var obj = m_Variables[i]; 
				if (obj != null && obj is ITextSerializable serializable) {
					if (m_CachedSerializableObjects.TryGetValue(obj.name, out string value)) {
						serializable.FromTextData(value);
					}
				}
			}
			
			m_CachedSerializableObjects.Clear();
		}

		/// <summary>
		/// Save data at wrapper's target path
		/// </summary>
		public void Save() {
			var wrapper = new KeyValueDatasWrapper();
			for (int i = 0; i < m_Variables.Length; i++) {
				var obj = m_Variables[i];
				if (obj != null && obj is ITextSerializable serializable) {
					wrapper.datas.Add(new KeyValueData(obj.name, serializable.GetTextData()));
				}
			}

			FileInfo fileInfo = GetFileInfo();
			
			switch (m_FileFormat) {
				case FileFormat.JSON:
					string jsonData = JsonUtility.ToJson(wrapper, true);
					SaveUtility.WriteText(fileInfo, jsonData);
					break;
				default: //Save to binary by default
					var formatter = new BinaryFormatter();
					using (MemoryStream ms = new MemoryStream()) {
						formatter.Serialize(ms, wrapper);
						SaveUtility.WriteBytes(fileInfo, ms.ToArray());
					}
					break;
			}
		}

		//TODO : Implement async save and load
		#endregion


		#region Utility
		public string GetSavePath() {
			string dir = GetDirectoryPath();
			return SaveUtility.ConcatFilePath(GetDirectoryPath(), m_FileName, m_Extension);
		}

		public FileInfo GetFileInfo() {
			return new FileInfo(GetDirectoryPath(), m_FileName, m_Extension);
		}

		public string GetDirectoryPath() {
			string subDir = string.Empty;
			if (m_SubDirectory.Length > 0) {
				subDir = SaveUtility.fileSeparator + m_SubDirectory;
			}

			return SaveUtility.defaultSavePath + subDir;
		}
		#endregion


		#region Editor
		private void OnValidate() {
			for (int i = 0; i < m_Variables.Length; i++) {
				var variable = m_Variables[i];
				if (variable != null && variable is not ITextSerializable) {
					m_Variables[i] = null;
					Debug.LogError("[Scriptable Variable Save Wrapper] " +
					               "Cannot register this variable, the save wrapper can contains Text Serializable variables only");
				}
			}
		}
		#endregion
	}
}
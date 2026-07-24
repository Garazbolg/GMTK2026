using System;
using System.Collections;
using System.Collections.Generic;
using DevCore.Core;
using DevCore.Core.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DevCore.DataBrowser.Editor {
	[CreateAssetMenu(menuName = EditorConstants.k_EditorAssetPath + "Data Filter", fileName = "Filter_")]
	public class DataFilterSetting : ScriptableObject {
		[SerializeField] private string m_DisplayName = string.Empty;
		[SerializeField] private MonoScript m_Class;
		[SerializeField] private bool m_FilterChildTypes = true;

		public string displayName {
			get {
				if(string.IsNullOrEmpty(m_DisplayName)) {
					return name;
				}

				return m_DisplayName;
			}
		}

		public bool isValid => m_Class != null;

		public IList GetCache() {
			return FilteredAssetsCache.GetObjectCacheReadOnly(this);
		}

		/// <summary>
		/// Return a hash set containing filtered objects guids 
		/// Return null if the filter is not valid
		/// </summary>
		/// <returns></returns>
		public HashSet<string> GetFilteredAssets() {
			if (!isValid) {
				return null;
			}

			Type scriptType = m_Class.GetClass();
			var guids = DevCoreAssetUtility.FindAssetsWithTypeFilter(scriptType);
			
			return GetFiltertedAssetsInternal(guids, scriptType);
		}

		/// <summary>
		/// Return a hash set containing filtered objects guids from input paths
		/// Return null if the filter is not valid
		/// </summary>
		/// <returns></returns>
		public HashSet<string> GetFiltertedAssetsFromPaths(string[] paths) {
			if (!isValid) {
				return null;
			}
			
			DevCoreAssetUtility.AssetPathsToGUIDs(paths);
			return GetFiltertedAssetsInternal(paths, m_Class.GetClass());
		}
		
		/// <summary>
		/// Return a hash set containing filtered objects guids from input paths
		/// Return null if the filter is not valid
		/// </summary>
		/// <returns></returns>
		public HashSet<string> GetFiltertedAssetsFromGUIDs(string[] guids) {
			if (!isValid) {
				return null;
			}
			
			return GetFiltertedAssetsInternal(guids, m_Class.GetClass());
		}
		
		public HashSet<string> GetFiltertedAssetsInternal(string[] guids, Type scriptType) {
			var filteredObjects = new HashSet<string>();
			
			if (m_FilterChildTypes) {
				foreach (var guid in guids) {
					Type assetype = AssetDatabase.GetMainAssetTypeFromGUID(new GUID(guid));
					if (assetype.IsTypeOrInherithFrom(scriptType)) {
						filteredObjects.Add(guid);
					}
				}
			} else {
				foreach (var guid in guids) {
					Type assetype = AssetDatabase.GetMainAssetTypeFromGUID(new GUID(guid));
					if (assetype == scriptType) {
						filteredObjects.Add(guid);
					}
				}
			}

			return filteredObjects;
		} 
		
		public string GetDisplayName() {
			return displayName;
		}

		public override string ToString() {
			return GetDisplayName();
		}
	}
}

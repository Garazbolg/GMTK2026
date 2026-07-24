using System.Collections;
using System.Collections.Generic;
using DevCore.Core.Editor;
using UnityEngine;

namespace DevCore.DataBrowser.Editor {
	internal class AssetReferencesCache {
		#region Currents
		private DataFilterSetting m_Filter;
		private HashSet<string> m_GuidsCache;
		private List<string> m_ReadOnlyList;
		private static List<string> m_PendingGuidsToUnregister = new List<string>();
		#endregion


		#region Constructor
		public AssetReferencesCache(DataFilterSetting filter) {
			m_GuidsCache = filter.GetFilteredAssets();
			m_ReadOnlyList = new List<string>();

			if (m_GuidsCache != null) {
				RefillReadOnlyList();
			}

			m_Filter = filter;
		}
		#endregion


		#region Manage Read Only List
		internal IList GetCacheReadOnlyList(bool sort = false) {
			if (sort) {
				SortCacheList();	
			}
			return m_ReadOnlyList;
		}

		private void SortCacheList() {
			if (m_ReadOnlyList.Count > 0) {
				m_ReadOnlyList.Sort(AssetGUIDAlphabeticalComparer.AssetComparerCurrentCulture);
			}
		}
		
		private void RefillReadOnlyList() {
			m_ReadOnlyList.Clear();
			foreach (var guid in m_GuidsCache) {
				m_ReadOnlyList.Add(guid);
			}
		}
		#endregion


		#region Assets Registration
		public bool TryRegisterAssetsFromGuidsIfValid(string[] guids) {
			if (m_GuidsCache == null) {
				return false;
			}

			bool hasRegisteredAnyAsset = false;
			var filteredAssets = m_Filter.GetFiltertedAssetsFromGUIDs(guids);

			if (filteredAssets.Count > 0) {
				foreach (var assetGuid in filteredAssets) {
					if (!m_GuidsCache.Contains(assetGuid)) {
						m_GuidsCache.Add(assetGuid);
						if (!hasRegisteredAnyAsset) {
							hasRegisteredAnyAsset = true;
						}
					}
				}
			}

			if (hasRegisteredAnyAsset) {
				RefillReadOnlyList();
			}

			return hasRegisteredAnyAsset;
		}


		public bool TryUnregisterAssetsFromGuidsIfValid(string[] guids) {
			if (m_GuidsCache == null) {
				return false;
			}

			bool hasUnregisteredAnyAsset = false;

			//Check guids to remove
			foreach (var guid in guids) {
				if (m_GuidsCache.Contains(guid)) {
					m_PendingGuidsToUnregister.Add(guid);
				}
			}

			if (m_PendingGuidsToUnregister.Count > 0) {
				hasUnregisteredAnyAsset = true;
			}
			
			//Remove pending items
			for (var i = 0; i < m_PendingGuidsToUnregister.Count; i++) {
				m_GuidsCache.Remove(m_PendingGuidsToUnregister[i]);
			}
			
			m_PendingGuidsToUnregister.Clear();

			if (hasUnregisteredAnyAsset) {
				RefillReadOnlyList();
			}

			return hasUnregisteredAnyAsset;
		}
		#endregion


		#region Utils
		internal bool ContainsAny(string[] guids) {
			for (int i = 0; i < guids.Length; i++) {
				if (m_GuidsCache.Contains(guids[i])) {
					SortCacheList();
					return true;
				}
			}

			return false;
		}
		#endregion
	}
}
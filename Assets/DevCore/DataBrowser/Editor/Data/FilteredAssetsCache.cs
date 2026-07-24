using System;
using System.Collections;
using System.Collections.Generic;
using DevCore.Core.Editor;
using UnityEngine;

namespace DevCore.DataBrowser.Editor {
	public static class FilteredAssetsCache {
		private static Dictionary<DataFilterSetting, AssetReferencesCache> m_Cache =
			new Dictionary<DataFilterSetting, AssetReferencesCache>();

		public static event Action onAnyCacheUpdated;
		public static event Action onAnyCachedAssetMoveOrRename;

		private static void CreateCache(DataFilterSetting filter) {
			m_Cache.Add(filter, new AssetReferencesCache(filter));
		}

		internal static AssetReferencesCache GetOrCreateCache(DataFilterSetting filter) {
			if (!m_Cache.ContainsKey(filter)) {
				CreateCache(filter);
			}

			return m_Cache[filter];
		}

		public static IList GetObjectCacheReadOnly(DataFilterSetting filter) {
			return GetOrCreateCache(filter).GetCacheReadOnlyList(true);
		}

		public static bool TryRegisterAssetsFromPathsIfValid(string[] paths) {
			bool hasRegisteredAnyAsset = false;

			if (paths.Length <= 0f) {
				return false;
			}

			DevCoreAssetUtility.AssetPathsToGUIDs(paths);

			foreach (var pair in m_Cache) {
				if (pair.Value.TryRegisterAssetsFromGuidsIfValid(paths) && !hasRegisteredAnyAsset) {
					hasRegisteredAnyAsset = true;
				}
			}

			if (hasRegisteredAnyAsset) {
				onAnyCacheUpdated?.Invoke();
			}

			return hasRegisteredAnyAsset;
		}

		public static bool TryUnregisterAssetsFromPathsIfValid(string[] paths) {
			bool hasUnregisteredAnyAsset = false;

			if (paths.Length <= 0f) {
				return false;
			}

			DevCoreAssetUtility.AssetPathsToGUIDs(paths);

			foreach (var pair in m_Cache) {
				if (pair.Value.TryUnregisterAssetsFromGuidsIfValid(paths) && !hasUnregisteredAnyAsset) {
					hasUnregisteredAnyAsset = true;
				}
			}

			if (hasUnregisteredAnyAsset) {
				onAnyCacheUpdated?.Invoke();
			}

			return hasUnregisteredAnyAsset;
		}

		internal static void CheckMovedAssets(string[] movedAssetsPaths) {
			DevCoreAssetUtility.AssetPathsToGUIDs(movedAssetsPaths);
			
			bool hasAnyMovedAssetRegistered = false;
			foreach (var pair in m_Cache) {
				if (pair.Value.ContainsAny(movedAssetsPaths)) {
					if (!hasAnyMovedAssetRegistered) {
						hasAnyMovedAssetRegistered = true;
					}
				}
			}

			if (hasAnyMovedAssetRegistered) {
				onAnyCachedAssetMoveOrRename?.Invoke();
			}
		}
	}
}
using DevCore.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace DevCore.DataBrowser.Editor {
	public class DataFilterAssetPostProcessor : AssetPostprocessor {
		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
			string[] movedAssets,
			string[] movedFromAssetPaths) {
			if (DataFiltersCache.isCacheBuilt) {
				RegisterFilterSettingAssets(importedAssets);
				UnregisterFilterSettingAssets(deletedAssets);
			}
		}

		private static void RegisterFilterSettingAssets(string[] importedAssets) {
			foreach (var path in importedAssets) {
				if (DevCoreAssetUtility.IsMainAssetOfType(path, typeof(DataFilterSetting))) {
					DataFiltersCache.TryRegisterFilterFromPath(path);
				}
			}
		}

		private static void UnregisterFilterSettingAssets(string[] deletedAssets) {
			foreach (var path in deletedAssets) {
				DataFiltersCache.TryUnregisterFilterFromPath(path);
			}
		}
	}
}
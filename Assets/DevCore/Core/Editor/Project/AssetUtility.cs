using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DevCore.Core.Editor {
	/// <summary>
	/// Utility class to work with the asset database
	/// </summary>
	public static class AssetUtility {
		
		/// <summary>
		/// Returns all assets of type T
		/// </summary>
		/// <param name="searchInFolders">Specified project directory where types must be found</param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T[] FindAllAssetsOfType<T>(string[] searchInFolders = null) where T : Object{
			string[] assetsGuids;
			if (searchInFolders != null) {
				assetsGuids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", searchInFolders);
			} else {
				assetsGuids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
			}

			var foundAssets = new T[assetsGuids.Length]; 
			for (int i = 0; i < assetsGuids.Length; i++) {
				foundAssets[i] = LoadAssetFromGUID<T>(assetsGuids[i]);
			}

			return foundAssets;
		}

		/// <summary>
		/// Load asset of type from its GUID
		/// </summary>
		/// <param name="guid"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T LoadAssetFromGUID<T>(string guid) where T : Object{
			return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
		}
	}
}
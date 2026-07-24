using UnityEditor;
using UnityEngine;

namespace DevCore.Core.Editor {
	public static class AssetExtensions {
		/// <summary>
		/// Returns the asset path if this object belongs to the asset database
		/// </summary>
		/// <param name="asset"></param>
		/// <returns></returns>
		public static string GetAssetPath(this Object asset) {
			if (AssetDatabase.Contains(asset)) {
				return AssetDatabase.GetAssetPath(asset);
			}

			return null;
		}
	}
}
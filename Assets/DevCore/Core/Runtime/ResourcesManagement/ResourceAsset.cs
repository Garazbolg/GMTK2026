using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DevCore.Core {
	public abstract class ResourceAsset<T> : ScriptableObject where T : ResourceAsset<T> {
		#region Currents
		private static T m_CurrentAsset = null;
		#endregion
		
		#region Resource Access
		public static T GetAsset() {
			if (m_CurrentAsset) {
				return m_CurrentAsset;
			}
			
			string fileName = typeof(T).Name;
			T asset = Resources.Load<T>(fileName);
			if (asset == null) {
#if UNITY_EDITOR
				asset = CreateResourceAsset();
#else
				throw new Exception($"[Resource Asset] The asset of type {typeof(T).Name} cannot be accessed, you must create one at edit time");
#endif
			}

			return asset;
		}
		#endregion


		#region Editor
#if UNITY_EDITOR
		private static T CreateResourceAsset() {
			T asset = ScriptableObject.CreateInstance<T>();
			Type type = typeof(T);
			string typeName = type.Name;
			string path = $"Assets/Resources/{typeName}.asset";

			if (!AssetDatabase.IsValidFolder("Assets/Resources")) {
				AssetDatabase.CreateFolder("Assets", "Resources");
			}
			
			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			return asset;
		}
#endif

		protected void Save() {
#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssetIfDirty(this);
#endif
		}
		#endregion
	}
}
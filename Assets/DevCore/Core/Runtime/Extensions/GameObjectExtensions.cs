using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.Core {
	/// <summary>
	/// Extension library for Game Objects
	/// </summary>
	public static class GameObjectExtensions {
		/// <summary>
		/// Returns the first component found on the target Game Object. Creates it if this one doesn't exists
		/// </summary>
		/// <param name="go"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetOrAddComponent<T>(this GameObject go) where T : Component {
			if (go.TryGetComponent(out T comp)) {
				return comp;
			} else {
				return go.AddComponent<T>();
			}
		}

		/// <summary>
		/// Return true if this object 
		/// </summary>
		/// <param name="go"></param>
		/// <returns></returns>
		public static bool IsSceneObject(this GameObject go) {
			return !string.IsNullOrEmpty(go.scene.path);
		}

		/// <summary>
		/// Return true if the object is in the DontDestroyOnLoad scene
		/// </summary>
		/// <returns></returns>
		public static bool IsPersistent(this GameObject go) {
			return go.scene.buildIndex < 0; //Persistent scene build index is -1
		}
	}
}
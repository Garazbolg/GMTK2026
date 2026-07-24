using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.Core {
	public static class TypesUtility {
		/// <summary>
		/// Returns true if sourceType is equal, child of or implements interface of T
		/// </summary>
		/// <param name="targetType"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static bool IsTypeOrInherithFrom<T>(Type sourceType) {
			return IsTypeOrInherithFrom(sourceType, typeof(T));
		}

		/// <summary>
		/// Returns true if sourceType is equal, child of or implements interface of targetType
		/// </summary>
		/// <param name="targetType"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static bool IsTypeOrInherithFrom(Type sourceType, Type targetType) {
			return sourceType == targetType || sourceType.IsSubclassOf(targetType) ||
			       sourceType.IsAssignableFrom(targetType);
		}
	}
}
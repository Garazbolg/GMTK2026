using System;
using UnityEngine;

namespace DevCore.Core {
	public static class TypesExtensions {
		/// <summary>
		/// Returns true if equal, child of or implements interface of T
		/// </summary>
		/// <param name="targetType"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static bool IsTypeOrInherithFrom<T>(this Type sourceType) {
			return TypesUtility.IsTypeOrInherithFrom<T>(sourceType);
		}
		
		/// <summary>
		/// Returns true if equal, child of or implements interface of T
		/// </summary>
		/// <param name="targetType"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static bool IsTypeOrInherithFrom<T>(this object sourceObject) {
			return TypesUtility.IsTypeOrInherithFrom<T>(sourceObject.GetType());
		}
		
		/// <summary>
		/// Returns true if equal, child of or implements interface of targetType
		/// </summary>
		/// <param name="targetType"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static bool IsTypeOrInherithFrom(this Type sourceType, Type targetType) {
			return TypesUtility.IsTypeOrInherithFrom(sourceType, targetType);
		}
		
		/// <summary>
		/// Returns true if equal, child of or implements interface of targetType
		/// </summary>
		/// <param name="targetType"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static bool IsTypeOrInherithFrom(this object sourceObject, Type targetType) {
			return TypesUtility.IsTypeOrInherithFrom(sourceObject.GetType(), targetType);
		}
	}
}
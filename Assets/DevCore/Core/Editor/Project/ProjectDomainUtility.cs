using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DevCore.Core.Editor {
	/// <summary>
	/// Utility class for scripts contained in the project domain
	/// </summary>
	public static class ProjectDomainUtility {
		/// <summary>
		/// Returns a list of all types equal, childs or that implements an interface of T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		private static List<Type> GetChildTypesListOf<T>() {
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			var foundTypes = new List<Type>();
			for (int i = 0; i < assemblies.Length; i++) {
				var asm = assemblies[i];
				var asmTypes = asm.GetTypes();
				for (int j = 0; j < asmTypes.Length; j++) {
					var type = asmTypes[i];
					if (type.IsTypeOrInherithFrom<T>()) {
						foundTypes.Add(type);
					}
				}
			}

			return foundTypes;
		}
		
		
		/// <summary>
		/// Returns all types equal, childs or that implements an interface of T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static Type[] FindAllChildTypesOf<T>() {
			return GetChildTypesListOf<T>().ToArray();
		}

		/// <summary>
		/// Returns all MonoScript Types equal, childs or that implements an interface of T 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static Type[] FindAllUnityObjectsTypesOf<T>() where T : Object {
			return FindAllUnityObjectsTypesOf(typeof(T));
		}
		
		/// <summary>
		/// Returns all MonoScript Types equal, childs or that implements an interface of targetType
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static Type[] FindAllUnityObjectsTypesOf(Type targetType) {
			if (!targetType.IsTypeOrInherithFrom<Object>()) {
				throw new Exception($"The type {targetType.Name} is not child of Unity Object native type");
			}
			
			//TODO : Bypass genertic types
			
			var monoScripts = AssetUtility.FindAllAssetsOfType<MonoScript>();
			var filteredTypes = new List<Type>();
			for (int i = 0; i < monoScripts.Length; i++) {
				var type = monoScripts[i].GetClass(); 
				if (type != null && type.IsTypeOrInherithFrom(targetType)) {
					filteredTypes.Add(type);
				}
			}

			return filteredTypes.ToArray();
		}
	}
}
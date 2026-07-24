using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevCore.Core.Editor
{
    public class DevCoreAssetUtility : MonoBehaviour
    {
        /// <summary>
        /// Return all the asset guid using input type as filter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] FindFilteredAssetsOfType<T>() {
            return FindAssetsWithTypeFilter(typeof(T));
        }
        
        /// <summary>
        /// Return all the asset guid using input type as filter
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string[] FindAssetsWithTypeFilter(Type t) {
            return AssetDatabase.FindAssets($"t:{t.Name}");
        }

        /// <summary>
        /// Return asset name from input guid
        /// </summary>
        /// <param name="objectGuid"></param>
        /// <returns></returns>
        public static string GetAssetNameFromGUID(string objectGuid) {
            return GetAssetNameFromPath(AssetDatabase.GUIDToAssetPath(objectGuid));
        }
        
        /// <summary>
        /// Return asset name from input path 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetAssetNameFromPath(string path) {
            return FileUtility.FileNameFromPath(path, '/');
        }

        /// <summary>
        /// Return true if the asset <see cref="Type"/> corresponds to the input <see cref="Type"/>
        /// </summary>
        /// <param name="assetGuid"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsMainAssetOfType(GUID assetGuid, Type t, bool allowChilds = true) {
            var type =AssetDatabase.GetMainAssetTypeFromGUID(assetGuid);
            if (allowChilds) {
                return type == t;
            } else {
                return type.IsTypeOrInherithFrom(t);
            }
        }
        
        /// <summary>
        /// Return true if the asset <see cref="Type"/> corresponds to the input <see cref="Type"/>
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsMainAssetOfType(string assetPath, Type t, bool allowChilds = true) {
            var type = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
            if (allowChilds) {
                return type == t;
            } else {
                return type.IsTypeOrInherithFrom(t);
            }
        }

        /// <summary>
        /// Convert all path array items to a guid
        /// </summary>
        /// <param name="assetPaths"></param>
        public static void AssetPathsToGUIDs(string[] assetPaths) {
            for (int i = 0; i < assetPaths.Length; i++) {
                assetPaths[i] = AssetDatabase.AssetPathToGUID(assetPaths[i]);
            }
        }
        
        /// <summary>
        /// Convert all guid array items to a path
        /// </summary>
        /// <param name="assetPaths"></param>
        public static void GUIDsToAssetPaths(string[] guids) {
            for (int i = 0; i < guids.Length; i++) {
                guids[i] = AssetDatabase.GUIDFromAssetPath(guids[i]).ToString();
            }
        }
    }
}

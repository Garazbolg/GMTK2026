using System;
using System.Collections;
using System.Collections.Generic;
using DevCore.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace DevCore.DataBrowser.Editor
{
    public static class DataFiltersCache
    {
        #region Cache
        private static bool m_IsCacheBuilt = false;
        private static Dictionary<string, DataFilterSetting> m_DataFilters = new Dictionary<string, DataFilterSetting>();
        private static List<DataFilterSetting> m_ReadOnlyListCache = new List<DataFilterSetting>();
        #endregion


        #region Properties
        public static bool isCacheBuilt => m_IsCacheBuilt;
        #endregion


        #region Events
        public static event Action onCacheUpdated;
        #endregion
        
        #region Build Caches
        public static void BuildFilterCache() {
            m_DataFilters.Clear();
            
            string[] filtersGuids = DevCoreAssetUtility.FindFilteredAssetsOfType<DataFilterSetting>();
            foreach (var guid in filtersGuids) {
                if(DevCoreAssetUtility.IsMainAssetOfType(new GUID(guid), typeof(DataFilterSetting))) {
                    TryRegisterFilterInternal(guid);
                }
            }

            m_IsCacheBuilt = true;
            RefillListCache();
        }
        
        private static bool TryRegisterFilterInternal(string guid) {
            if (!m_DataFilters.ContainsKey(guid)) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var filterSetting = AssetDatabase.LoadAssetAtPath<DataFilterSetting>(path);
                m_DataFilters.Add(guid, filterSetting);
                
                //Create a corresponding data cache
                FilteredAssetsCache.GetOrCreateCache(filterSetting);
                return true;
            }

            return false;
        }

        public static void TryRegisterFilterFromPath(string path) {
            if (TryRegisterFilterInternal(AssetDatabase.GUIDFromAssetPath(path).ToString())) {
                RefillListCache();
                onCacheUpdated?.Invoke();
            }
        }

        private static bool TryUnregisterFilterInternal(string guid) {
            if (m_DataFilters.ContainsKey(guid)) {
                m_DataFilters.Remove(guid);
                return true;
            }

            return false;
        }
        
        internal static void TryUnregisterFilterFromPath(string path) {
            string guid = AssetDatabase.GUIDFromAssetPath(path).ToString();
            if (TryUnregisterFilterInternal(guid)) {
                RefillListCache();
                onCacheUpdated?.Invoke();
            }
        }

        private static void RefillListCache() {
            m_ReadOnlyListCache.Clear();
            foreach (var pair in m_DataFilters) {
                m_ReadOnlyListCache.Add(pair.Value);
            }
        }
        #endregion


        #region Get Data
        public static IList GetAbstractFiltersList() {
            if (!m_IsCacheBuilt) {
                BuildFilterCache();
            }

            return m_ReadOnlyListCache;
        }
        
        public static IList<DataFilterSetting> GetFiltersList() {
            if (!m_IsCacheBuilt) {
                BuildFilterCache();
            }

            return m_ReadOnlyListCache;
        }
        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DevCore.Core.Editor
{
    public static class DevCoreEditor
    {
        /// <summary>
        /// Pass a reference of a rebuilt editor corresponding to the input object
        /// Supports : Importers
        /// Doesn't support : Materials, GameObjects, Prefabs 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cachedEditor"></param>
        public static void CreateCachedEditor(Object obj, ref UnityEditor.Editor cachedEditor) {
            if (obj is GameObject) {
                return;
            }
            
            Type editorType = null;
            
            if (editorType == null && AssetDatabase.Contains(obj) || obj is GameObject) {
                var importer = AssetImporter.GetAtPath(obj.GetAssetPath());
                //Bypass default importer inspector
                if (importer != null && importer.GetType() != typeof(AssetImporter)) {
                    obj = importer;
                }
            } 
        
            
            UnityEditor.Editor.CreateCachedEditor(obj, editorType, ref cachedEditor);
        }
    }
}

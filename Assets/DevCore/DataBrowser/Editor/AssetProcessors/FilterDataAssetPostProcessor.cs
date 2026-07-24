using System;
using System.Collections;
using System.Collections.Generic;
using DevCore.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace DevCore.DataBrowser.Editor
{
    public class FilterDataAssetPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths) {
            FilteredAssetsCache.TryRegisterAssetsFromPathsIfValid(importedAssets);
            FilteredAssetsCache.TryUnregisterAssetsFromPathsIfValid(deletedAssets);
            FilteredAssetsCache.CheckMovedAssets(movedAssets);
        }
    }
}

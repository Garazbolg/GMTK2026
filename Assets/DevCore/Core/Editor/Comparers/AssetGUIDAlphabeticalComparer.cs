using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevCore.Core.Editor
{
    public class AssetGUIDAlphabeticalComparer : StringComparer {
        public static readonly AssetGUIDAlphabeticalComparer AssetComparerCurrentCulture =
            new AssetGUIDAlphabeticalComparer(); 
        
        public override int Compare(string guidA, string guidB) {
            string pathA = AssetDatabase.GUIDToAssetPath(guidA);
            string pathB = AssetDatabase.GUIDToAssetPath(guidB);
            return CurrentCulture.Compare(pathA, pathB);
        }

        public override bool Equals(string guidA, string guidB) {
            return new Guid(guidA) == new Guid(guidA);
        }

        public override int GetHashCode(string str) {
            return str.GetHashCode();
        }
    }
}

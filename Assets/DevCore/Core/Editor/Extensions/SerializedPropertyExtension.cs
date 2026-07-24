using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevCore.Core.Editor
{
    public static class SerializedPropertyExtension
    {
        /// <summary>
        /// Is this property drawn as an array element
        /// </summary>
        /// <param name="property"></param>
        public static bool IsArrayElement(this SerializedProperty property) {
            string path = property.propertyPath;

            if (path == null && string.IsNullOrEmpty(path)) {
                return false;
            }
            
            //Closing bracked can only be used to close an array path 
            if (property.propertyPath[^1] == ']') {
                return true;
            }

            return false;
        }
    }
}

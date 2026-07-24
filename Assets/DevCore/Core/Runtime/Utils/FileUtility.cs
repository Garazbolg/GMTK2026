using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DevCore.Core
{
    public static class FileUtility
    {
        /// <summary>
        /// Return the name of a file from its path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string FileNameFromPath(string path, char separator) {
            var lastSeparatorIndex = path.LastIndexOf(separator) + 1;
            var extensionDotIndex = path.LastIndexOf('.');
            int nameLength = extensionDotIndex - lastSeparatorIndex;
            string name = path.Substring(lastSeparatorIndex, nameLength);
            return name;
        }
        
        
        /// <summary>
        /// Return the name for a file from its path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FileNameFromPath(string path) {
            return FileNameFromPath(path, Path.DirectorySeparatorChar);
        }
    }
}

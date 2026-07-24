using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.Core {
	/// <summary>
	/// Utility class to improve or automate operations on strings
	/// </summary>
	public static class StringUtility {
		/// <summary>
		/// Splits string elements into unique string into the input elements list, outputs the exact elements count.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="separator"></param>
		/// <param name="elements"></param>
		/// <param name="elementsCount"></param>
		public static void SplitNonAlloc(string str, char separator, List<string> elements) {
			elements.Clear();
			
			int nextIndex = -1; 
			int lastIndex = str.IndexOf(separator, 0);

			if (lastIndex < 0) {
				elements.Add(str);
				return;
			}
			
			while (lastIndex >= 0) {
				string separatedString = str.Substring(nextIndex + 1, lastIndex - nextIndex - 1);
				elements.Add(separatedString);
				nextIndex = lastIndex;
				lastIndex = str.IndexOf(separator, nextIndex + 1);
			}

			if (nextIndex != str.Length - 1 && nextIndex > 0) {
				string separatedString = str.Substring(nextIndex + 1, str.Length - nextIndex - 1); 
				elements.Add(separatedString);
			}
		}
	}
}
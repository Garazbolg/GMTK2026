using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace DevCore.Core {
	public static class StringExtensions {
		/// <summary>
		/// Return the string value of the input float with an invariant culture
		/// </summary>
		public static string ToICString(this float value) {
			return value.ToString(CultureInfo.InvariantCulture);
		}
		
		/// <summary>
		/// Return the string value of the input integer with an invariant culture
		/// </summary>
		public static string ToICString(this int value) {
			return value.ToString(CultureInfo.InvariantCulture);
		}
		
		/// <summary>
		/// Return the string value of the input boolean with an invariant culture
		/// </summary>
		public static string ToICString(this bool value) {
			return value.ToString(CultureInfo.InvariantCulture);
		}


		/// <summary>
		/// Splits string elements into unique string into the input elements list, outputs the exact elements count.
		/// The list is cleared before this operation
		/// </summary>
		/// <param name="str"></param>
		/// <param name="separator"></param>
		/// <param name="elements"></param>
		/// <param name="elementsCount"></param>
		public static void SplitNonAlloc(this string str,  char separator, List<string> outputList) {
			StringUtility.SplitNonAlloc(str, separator, outputList);
		}
	}
}
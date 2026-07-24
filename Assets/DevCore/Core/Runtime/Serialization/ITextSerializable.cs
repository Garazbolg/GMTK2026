using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.Core.Serialization {
	public interface ITextSerializable {
		/// <summary>
		/// Returns a converted value to a string of a data
		/// </summary>
		/// <returns></returns>
		string GetTextData();
		
		/// <summary>
		/// Parse a serialized string data to a target type value
		/// </summary>
		/// <param name="value"></param>
		void FromTextData(string data);
	}
}
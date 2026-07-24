using UnityEngine;

namespace DevCore.Core {
	public static class ObjectExtensions{
		public static bool IsNull(this Object obj) {
			return ReferenceEquals(obj, null);
		}
	}
}
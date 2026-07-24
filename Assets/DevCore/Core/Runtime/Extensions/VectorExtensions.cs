using UnityEngine;

namespace DevCore.Core {
	public static class VectorExtensions{
		#region 2D
		/// <summary>
		/// Converts a Vector3 to a Vector2
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static Vector2 To2D(this Vector3 vec) {
			return new Vector2(vec.x, vec.y);
		}
		
		/// <summary>
		/// Divide with input value
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2 Divide(this Vector2 a, Vector2 b) {
			return VectorUtility.Divide(a, b);
		}
		#endregion

		#region 3D
		/// <summary>
		/// Converts a Vector2 to a Vector3
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static Vector3 To3D(this Vector2 vec, float z = 0f) {
			return new Vector3(vec.x, vec.y, z);
		}
		
		/// <summary>
		/// Divide with input value
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3 Divide(this Vector3 a, Vector3 b) {
			return VectorUtility.Divide(a, b);
		}
		#endregion
	}
}
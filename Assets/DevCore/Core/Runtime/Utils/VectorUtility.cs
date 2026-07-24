using UnityEngine;

namespace DevCore.Core {
	public class VectorUtility {

		/// <summary>
		/// Returns a random normalized direction in 2D
		/// </summary>
		/// <returns></returns>
		public static Vector2 RandomDirection2D() {
			return new Vector2(
				Random.Range(-1f, 1f), 
				Random.Range(-1f, 1f)).normalized;
		}
		
		/// <summary>
		/// Divide a vector with b 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector2 Divide(Vector2 a, Vector2 b) {
			return new Vector2(a.x / b.x, a.y / b.y);
		}
		
		/// <summary>
		/// Returns a random normalized direction in 3D
		/// </summary>
		/// <returns></returns>
		public static Vector3 RandomDirection3D() {
			return new Vector3(
				Random.Range(-1f, 1f), 
				Random.Range(-1f, 1f),
				Random.Range(-1f, 1f)).normalized;
		}

		/// <summary>
		/// Divide a vector with b 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector3 Divide(Vector3 a, Vector3 b) {
			return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
		}
	}
}
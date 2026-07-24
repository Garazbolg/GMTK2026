using UnityEngine;

namespace DevCore.InputManagement {
	public class Axis2DInputAction : ButtonInputAction {
		public Vector2 GetAxis2D() {
			return Vector2.zero;
		}

		public float GetAxisAngle() {
			Vector2 axis = GetAxis2D();
			float rad = ((Mathf.Atan2(axis.x, axis.y) / (Mathf.PI)) + 1f) * 0.5f;
			return rad;
		}
	}
}
using UnityEngine;

namespace DevCore.PrototypingToolkit {
	[AddComponentMenu("Prototyping Toolkit/Movement/Rotation")]
	public class RotationMovement : MonoBehaviour {
		public Vector3 eulerRotation = Vector3.up;
		public bool local = false;

		public Transform targetTransform = null;

		Quaternion currentRot = Quaternion.identity;

		void Update() {
			currentRot *= Quaternion.Euler(eulerRotation * Time.deltaTime);
			if (local) {
				targetTransform.rotation = currentRot;
			} else {
				targetTransform.localRotation = currentRot;
			}
		}
	}
}
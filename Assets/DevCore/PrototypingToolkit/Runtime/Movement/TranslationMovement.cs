using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.PrototypingToolkit {
	[AddComponentMenu("Prototyping Toolkit/Movement/Translation")]
	public class TranslationMovement : MonoBehaviour {
		public Vector3 direction = Vector3.right;
		public bool local = false;

		public Transform targetTransform = null;
		
		void Update() {
			Vector3 movement = direction * Time.deltaTime;
			
			if (local) {
				movement = targetTransform.rotation * movement;
			}

			targetTransform.position += movement;
		}
	}
}
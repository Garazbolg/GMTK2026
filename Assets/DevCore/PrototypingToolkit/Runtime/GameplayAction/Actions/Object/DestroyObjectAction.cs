using System.Collections;
using System.Collections.Generic;
using DevCore.Core;
using UnityEngine;

namespace DevCore.PrototypingToolkit {
	/// <summary>
	/// Destroys input entity
	/// </summary>
	[AddComponentMenu("Object/Destroy")]
	public class DestroyObjectAction : GameplayActionComponent {
		protected override void Execute(GameObject gameObject) {
			if (gameObject.IsPersistent()) {
				Debug.LogError($"[{name}] Cannot destroy {gameObject.name}. This one is persistent.", gameObject);
				return;
			}
			Destroy(gameObject);
		}
	}
}
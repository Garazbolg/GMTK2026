using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.PrototypingToolkit {
	[AddComponentMenu("Health/Heal")]
	public class HealAction : GameplayActionComponent {
		[SerializeField, Min(0)] private int m_HealAmount = 10;
		
		protected override void Execute(GameObject gameObject) {
			if (gameObject.TryGetComponent(out HealthBar hb)) {
				hb.Heal(m_HealAmount);
			} else {
				Debug.LogError($"[{name}] No Health bar found on Game Object {gameObject}", gameObject);
			}
		}
	}
}
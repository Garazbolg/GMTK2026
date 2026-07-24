using UnityEngine;

namespace DevCore.PrototypingToolkit {
	[AddComponentMenu("Health/Take Damages")]
	public class TakeDamage : GameplayActionComponent {
		[SerializeField, Min(0)] private int m_DamagesAmount;

		protected override void Execute(GameObject gameObject) {
			if (gameObject.TryGetComponent(out HealthBar hb)) {
				hb.TakeDamages(m_DamagesAmount);
			} else {
				Debug.LogError($"[{name}] No Health bar found on Game Object {gameObject}", gameObject);
			}
		}
	}
}
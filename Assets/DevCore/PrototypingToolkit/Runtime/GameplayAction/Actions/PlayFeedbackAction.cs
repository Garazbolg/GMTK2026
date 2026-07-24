using System.Collections;
using System.Collections.Generic;
using DevCore.Core;
using DevCore.FeedbackEngine;
using UnityEngine;

namespace DevCore.PrototypingToolkit {
	[AddComponentMenu("Feedback/Play Feedback")]
	public class PlayFeedbackAction : GameplayActionDelayedComponent {
		[SerializeField] private FeedbackAsset m_Feedback;

		protected override void OnDelayEllapsed(GameObject gameObject, CooldownResult cooldownResult) {
			m_Feedback.Play(gameObject);
		}
	}
}
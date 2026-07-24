using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.FeedbackEngine {
	public abstract class ShakeFeedback : TransformTweenFeedback {
		#region Settings
		[SerializeField, Min(0f)] protected Vector3 m_Strength = Vector3.one;
		[SerializeField, Min(0)] protected int m_Vibrato = 10;
		[SerializeField] protected bool m_FadeOut = true;
		[SerializeField, Min(0f)] protected float m_Randomness = 90f;
		#endregion
	}
}
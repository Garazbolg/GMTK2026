using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.FeedbackEngine {
	/// <summary>
	/// Set a float parameter value on the owner animator when played
	/// </summary>
	[AddComponentMenu("Animation/Set Animator Float Parameter")]
	public class AnimatorSetFloatFeedback : AnimatorSetParameterFeedback {
		[SerializeField] private float m_Value = 0;

		protected override void PlayAnimatorFeedback(Animator animator, int propertyHash) {
			animator.SetFloat(propertyHash, m_Value);
		}
	}
}
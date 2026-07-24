using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.FeedbackEngine {
	/// <summary>
	/// Set a bool parameter value on the owner animator when played
	/// </summary>
	[AddComponentMenu("Animation/Set Animator Bool Parameter")]
	public class AnimatorSetBoolFeedback : AnimatorSetParameterFeedback {
		[SerializeField] private bool m_Value = false;
		
		protected override void PlayAnimatorFeedback(Animator animator, int propertyHash) {
			animator.SetBool(propertyHash, m_Value);
		}
	}
}
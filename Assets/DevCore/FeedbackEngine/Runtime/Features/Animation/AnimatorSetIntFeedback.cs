using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.FeedbackEngine {
	/// <summary>
	/// Set a int parameter value on the owner animator when played
	/// </summary>
	[AddComponentMenu("Animation/Set Animator Int Parameter")]
	public class AnimatorSetIntFeedback : AnimatorSetParameterFeedback {
		[SerializeField] private int m_Value = 0;

		protected override void PlayAnimatorFeedback(Animator animator, int propertyHash) {
			animator.SetInteger(propertyHash, m_Value);
		}
	}
}
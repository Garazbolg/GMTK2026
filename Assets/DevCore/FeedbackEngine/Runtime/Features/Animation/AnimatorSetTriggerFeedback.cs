using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.FeedbackEngine {
	/// <summary>
	/// Set a trigger parameter value on the owner animator when played
	/// </summary>
	[AddComponentMenu("Animation/Set Animator Trigger Parameter")]
	public class AnimatorSetTriggerFeedback : AnimatorSetParameterFeedback {
		protected override void PlayAnimatorFeedback(Animator animator, int propertyHash) {
			animator.SetTrigger(propertyHash);
		}
	}
}
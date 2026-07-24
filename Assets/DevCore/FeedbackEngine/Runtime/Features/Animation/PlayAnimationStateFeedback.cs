using UnityEngine;

namespace DevCore.FeedbackEngine {
	/// <summary>
	/// Force an animation state when played on the owner
	/// </summary>
	[AddComponentMenu("Animation/Play Animation State")]
	public class PlayAnimationStateFeedback : AnimatorFeedback {
		[SerializeField] private string m_StateName = string.Empty;


		protected override string animatorPropertyId => m_StateName;
		
		protected override void PlayAnimatorFeedback(Animator animator, int propertyHash) {
			animator.Play(propertyHash);
		}
	}
}
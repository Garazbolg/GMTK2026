using UnityEngine;

namespace DevCore.FeedbackEngine {
	public abstract class AnimatorFeedback : FeedbackComponent {
		private int m_PropertyHash = 0;
		
		protected abstract string animatorPropertyId { get; }

		private void OnEnable() {
			m_PropertyHash = Animator.StringToHash(animatorPropertyId);
		}

		protected sealed override void PlayFeedbackComponent(GameObject owner) {
			if (owner.TryGetComponent(out Animator animator)) {
				PlayAnimatorFeedback(animator, m_PropertyHash);
			}
		}

		protected abstract void PlayAnimatorFeedback(Animator animator, int propertyHash);
	}
}
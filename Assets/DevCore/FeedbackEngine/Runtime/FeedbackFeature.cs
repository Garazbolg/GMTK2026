using DevCore.Core;
using UnityEngine;

namespace DevCore.FeedbackEngine {
	/// <summary>
	/// Base class for feedback components
	/// </summary>
	public abstract class FeedbackComponent : AssetComponent {
		protected abstract void PlayFeedbackComponent(GameObject owner);

		internal void ExecuteFeedbackComponent_Internal(GameObject owner) {
			if (active) {
				PlayFeedbackComponent(owner);
			}
		}
	}
}

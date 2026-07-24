using System;
using DevCore.Core;
using UnityEngine;

namespace DevCore.FeedbackEngine {
	[CreateAssetMenu(fileName = "FB_", menuName = Constants.ASSET_PATH + "Feedback", order = 300)]
	public sealed class FeedbackAsset : CompositeAsset<FeedbackComponent> {
		#region Feedback Behaviour
		/// <summary>
		/// Play all the feedback components stack
		/// </summary>
		/// <param name="owner">Specified owner Game Object, could be required for some features</param>
		public void Play(GameObject owner) {
			if (!AppCore.IsRunning()) {
				Debug.LogError("Cannot play a feedback in edit mode");
				return;
			}

#if UNITY_EDITOR
			try {
#endif
				int compCount = componentsCount;
				for (int i = 0; i < compCount; i++) {
					GetComponentAtIndex(i).ExecuteFeedbackComponent_Internal(owner);
				}
#if UNITY_EDITOR
			}
			catch (Exception e) {
				Debug.LogException(e, this);
				throw;
			}
#endif
		}

		/// <summary>
		/// Play all the feedback components stack
		/// </summary>
		public void Play() {
			Play(null);
		}
		#endregion
	}
}

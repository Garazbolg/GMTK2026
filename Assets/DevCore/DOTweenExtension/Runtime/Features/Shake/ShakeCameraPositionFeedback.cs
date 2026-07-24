using DevCore.Core;
using UnityEngine;
using DG.Tweening;

namespace DevCore.FeedbackEngine {
	/// <summary>
	/// Shake the camera position when played
	/// </summary>
	[AddComponentMenu("Camera/Shake Camera")]
	public class ShakeCameraPositionFeedback : ShakeFeedback {
		#region Properties
		protected override TransformCoordinate m_TargetCoordinate => TransformCoordinate.Position;
		#endregion


		#region Execution
		protected override Tween GetTween(Transform targetTransform) {
			return targetTransform.DOShakePosition(m_Duration, m_Strength, m_Vibrato, m_Randomness, false, m_FadeOut);
		}

		protected override Transform GetTargetTransform(GameObject owner) {
			return Camera.main.transform;
		}
		#endregion
	}
}
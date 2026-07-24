using System;
using DevCore.Core;
using UnityEngine;
using DG.Tweening;

namespace DevCore.FeedbackEngine {
	/// <summary>
	/// Shakes input Game Object position when played
	/// </summary>
	[AddComponentMenu("Tweening/Shake")]
	public class ShakeTransformFeedback : ShakeFeedback {
		#region Settings
		[Space]
		[SerializeField] private TransformCoordinate m_Coordinate = TransformCoordinate.Position;
		#endregion

		#region Properties
		protected override TransformCoordinate m_TargetCoordinate => m_Coordinate;
		#endregion

		#region Execution
		protected override Tween GetTween(Transform targetTransform) {
			switch (m_Coordinate) {
				case TransformCoordinate.Position:
					return targetTransform.DOShakePosition(m_Duration, m_Strength, m_Vibrato, m_Randomness, false, m_FadeOut);
				case TransformCoordinate.Rotation:
					return targetTransform.DOShakeRotation(m_Duration, m_Strength, m_Vibrato, m_Randomness, m_FadeOut);
				case TransformCoordinate.Scale:
					return targetTransform.DOShakeScale(m_Duration, m_Strength, m_Vibrato, m_Randomness, m_FadeOut);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		#endregion
	}
}
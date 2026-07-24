using System;
using DevCore.Core;
using UnityEngine;
using DG.Tweening;

namespace DevCore.FeedbackEngine {
	public abstract class TransformTweenFeedback : FeedbackComponent {
		#pragma warning disable CS0414
		[SerializeField] private int priority = 0; 
		
		[Header("Tween Settings")]
		[SerializeField, Min(0f)] protected float m_Duration = 1f;

		protected abstract TransformCoordinate m_TargetCoordinate { get; }

		protected override void PlayFeedbackComponent(GameObject owner) {
			var targetTransform = GetTargetTransform(owner); 
			var handler = targetTransform.gameObject.GetOrAddComponent<TransformTweenHandler>();
			var tween = GetTween(targetTransform).Pause();
			handler.RegisterTween(tween, priority, m_TargetCoordinate);
		}

		protected abstract Tween GetTween(Transform targetTransform);

		protected virtual Transform GetTargetTransform(GameObject owner) {
			return owner.transform;
		}
	}
}
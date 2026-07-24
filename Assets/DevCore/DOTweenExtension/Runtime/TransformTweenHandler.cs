using System.Collections;
using System.Collections.Generic;
using DevCore.Core;
using UnityEngine;

#if USE_DOTWEEN
using DG.Tweening;
#endif

namespace DevCore.FeedbackEngine {
	/// <summary>
	/// Handles tween on a Transform to ensure we're not offseting transform coordinates
	/// </summary>
	public class TransformTweenHandler : MonoBehaviour {
		#region Currents
		private TweenHandler m_PositionHandler = new TweenHandler();
		private TweenHandler m_RotationHandler = new TweenHandler();
		private TweenHandler m_ScaleHandler = new TweenHandler(); 
		#endregion


		#region SubClasses
		private class TweenHandler {
#if USE_DOTWEEN
			private Tween m_Tween;
			private int m_Priority = int.MinValue;

			public void TryPlayTween(Tween tween, int priority) {
				if (m_Tween == null) {
					PlayTween(tween, priority);
				} else {
					if (m_Tween.IsPlaying()) {
						if (priority >= m_Priority) {
							m_Tween.Complete();
							PlayTween(tween, priority);
						}
					} else {
						PlayTween(tween, priority);
					}
				}
			}

			private void PlayTween(Tween tween, int priority) {
				m_Priority = priority;
				m_Tween = tween;
				tween.OnComplete(OnComplete);
				tween.Play();
			}

			private void OnComplete() {
				m_Priority = int.MinValue;
				m_Tween = null;
			}
#endif
		}
		#endregion


#if USE_DOTWEEN
		internal void RegisterTween(Tween tween, int priority, TransformCoordinate coordinate) {
			switch (coordinate) {
				case TransformCoordinate.Position:
					m_PositionHandler.TryPlayTween(tween, priority);
					break;
				case TransformCoordinate.Rotation:
					m_RotationHandler.TryPlayTween(tween, priority);
					break;
				case TransformCoordinate.Scale:
					m_ScaleHandler.TryPlayTween(tween, priority);
					break;
			}
		}
#endif
	}
}
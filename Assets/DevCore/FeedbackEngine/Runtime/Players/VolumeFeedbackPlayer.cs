using UnityEngine;
using UnityEngine.Rendering;

namespace DevCore.FeedbackEngine {
	public class VolumeFeedbackPlayer : MonoBehaviour {
		#region Settings
		[SerializeField] private Volume m_Volume = null;
		#endregion


		#region Curents
		private AnimationCurve m_Curve = null;
		private float m_TimeIncrement = 0f;
		private float m_NormalizedTime = 0f;
		#endregion


		#region Callbacks
		private void LateUpdate() {
			m_NormalizedTime += m_TimeIncrement * Time.deltaTime;
			m_Volume.weight = m_Curve.Evaluate(m_NormalizedTime);
			if (m_NormalizedTime >= 1f) {
				Stop();
			}
		}
		#endregion
		

		#region Behaviour
		internal void Play(VolumeEffectFeedback feedback) {
			m_Volume.sharedProfile = feedback.profile;
			m_Volume.weight = 0f;
			m_Volume.priority = feedback.priority;
			
			m_Curve = feedback.weightOverLifetime;

			m_NormalizedTime = 0f;
			m_TimeIncrement = 1f / feedback.duration;
			
			enabled = true;
			m_Volume.enabled = true;
		}

		private void Stop() {
			enabled = false;
			m_Volume.enabled = false;
			VolumeEffectFeedback.m_FeedbackPlayerPool.Release(this);
			Dispose();
		}

		internal void Dispose() {
			m_Volume.sharedProfile = null;
			m_Curve = null;
		}
		#endregion
	}
}
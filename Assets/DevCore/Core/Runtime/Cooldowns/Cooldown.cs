using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DevCore.Core {
	public enum CooldownEndType {
		Finished,
		Skipped
	}

	public delegate void CooldownCallback(CooldownResult result);

	/// <summary>
	/// Result of an ellapsed timer 
	/// </summary>
	public struct CooldownResult {
		public readonly float finalRemainingTime;
		public readonly float duration;
		public readonly CooldownEndType endType;

		public float totalEllapsedTime => duration - finalRemainingTime;
		public float durationExceed => -finalRemainingTime;

		public CooldownResult(float finalRemainingTime, float duration, CooldownEndType endType) {
			this.finalRemainingTime = finalRemainingTime;
			this.duration = duration;
			this.endType = endType;
		}

		public static CooldownResult skipped => new CooldownResult(0f, 0f, CooldownEndType.Finished);
	}

	/// <summary>
	/// A timer that trigger a callback when elapsed
	/// Can be paused
	/// Update by the Timer Handle
	/// </summary>
	public class Cooldown {
		#region Properties
		public float duration {
			get { return m_Duration; }
		}

		public bool isPlaying {
			get {
				return m_IsPlaying;
			}
			internal set {
				m_IsPlaying = isPlaying;
			}
		}
		#endregion


		#region Current
		private bool m_IsPlaying = false;
		private float m_RemainingTime;
		private float m_Duration;
		private CooldownCallback m_Callback;
		#endregion


		#region Constructors
		public Cooldown(float duration) {
			m_Duration = Mathf.Max(duration, 0f);
			m_RemainingTime = duration;
		}

		public Cooldown(float duration, CooldownCallback completeCallback) : this(duration) {
			m_Callback = completeCallback;
		}
		#endregion


		#region Behaviour
		/// <summary>
		/// Lauch a new timer with a specified duration
		/// </summary>
		/// <param name="duration"></param>
		/// <param name="callback"></param>
		/// <returns></returns>
		public static Cooldown Launch(float duration) {
			CooldownCallback _ = null;
			return Launch(duration, _);
		}

		/// <summary>
		/// Lauch a new timer with a specified duration and stop event
		/// </summary>
		/// <param name="duration"></param>
		/// <param name="callback"></param>
		/// <returns></returns>
		public static Cooldown Launch(float duration, CooldownCallback callback) {
			if (duration <= 0f) {
				callback?.Invoke(CooldownResult.skipped);
			}
			
			var cooldown = new Cooldown(duration, callback);
			cooldown.Launch();
			return cooldown;
		}

		/// <summary>
		/// Launch the timer execution
		/// </summary>
		public void Launch() {
			Activate();
		}

		public void LaunchFromStart() {
			Reset();
			Launch();
		}

		/// <summary>
		/// Pauses the timer execution
		/// </summary>
		public void Pause() {
			Deactivate();
		}

		/// <summary>
		/// Resets timer ellapsed time to 0
		/// </summary>
		public void Reset() {
			m_RemainingTime = m_Duration;
		}

		/// <summary>
		/// Stops the timer and send the send the complete feedback
		/// </summary>
		public void Skip() {
			Deactivate();
			m_Callback?.Invoke(new CooldownResult(m_RemainingTime, m_Duration, CooldownEndType.Skipped));
		}

		public void SetDuration(float duration, bool remapCurrentTime = false) {
			if (duration < 0f) {
				Debug.LogError("[Cooldown] Cannot set a cooldown duration to less than 0.");
			}

			if (m_IsPlaying && remapCurrentTime) {
				m_RemainingTime += duration - m_Duration;
				if (m_RemainingTime <= 0f) {
					Finish();
				}
			}

			m_Duration = duration;
		}

		internal void Update() {
			m_RemainingTime -= Time.deltaTime;

			if (m_RemainingTime <= 0f) {
				Finish();
			}
		}

		private void Finish() {
			Deactivate();
			m_Callback?.Invoke(new CooldownResult(m_RemainingTime, m_Duration, CooldownEndType.Finished));
		}


		private void Activate() {
			if (m_IsPlaying) return;
			m_IsPlaying = true;
			CooldownManager.RegisterCooldown(this);
		}

		private void Deactivate() {
			if (!m_IsPlaying) return;
			m_IsPlaying = false;
			CooldownManager.UnregisterCooldown(this);
		}
		#endregion
	}
}
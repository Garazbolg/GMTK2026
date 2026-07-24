using System;
using UnityEngine;
using UnityEngine.Events;
using CoreDev.Utility;
using DevCore.Core;

namespace DevCore.PrototypingToolkit {
	/// <summary>
	/// Base health system for game objects
	/// </summary>
	[AddComponentMenu("Prototyping Toolkit/Health Bar")]
	public class HealthBar : MonoBehaviour {
		#region Settings
		[SerializeField, Min(0)] private int m_MaxHealth = 100;
		[SerializeField, Min(0f)] private float m_RecoveryTime = 0f;
		public bool invicible = false;

		[Space]
		[SerializeField] private bool m_DefaultDestroyOnDie = false;
		
		[Header("Actions")]
		[SerializeField] private GameplayAction m_DeathAction = null;
		[SerializeField] private GameplayAction m_HealAction = null;
		[SerializeField] private GameplayAction m_TakeDamagesAction = null;
		#endregion


		#region Events
		public event Action<int> onTakeDamages;
		public event Action<int> onHeal;
		public event Action<int> onMaxHealthChanges;
		#endregion


		#region Currents
		private int m_CurrentHealth = 100;
		private Cooldown m_RecoveryCooldown = new Cooldown(0f);
		#endregion


		#region Properties
		public int maxHealth => m_MaxHealth;
		public int currentHealth => m_CurrentHealth;
		public float currentHealthRatio => (float) m_CurrentHealth / (float) m_MaxHealth;
		public bool isRecovering => m_RecoveryCooldown.isPlaying;
		#endregion


		#region Callbacks
		private void Start() {
			m_CurrentHealth = m_MaxHealth;
		}
		#endregion


		#region Update Health
		public void Heal(int amount) {
			if (amount < 0) {
				return;
			}

			m_CurrentHealth += amount;
			if (m_CurrentHealth > m_MaxHealth) {
				m_CurrentHealth = m_MaxHealth;
			}

			m_HealAction?.Execute(gameObject);
			onHeal?.Invoke(amount);
		}

		public void TakeDamages(int amount) {
			if (amount < 0 || invicible || isRecovering) {
				return;
			}

			m_CurrentHealth -= amount;
			if (m_CurrentHealth <= 0) {
				m_CurrentHealth = 0;
				Die();
			}

			if (m_RecoveryTime > 0f) {
				m_RecoveryCooldown.SetDuration(m_RecoveryTime);
				m_RecoveryCooldown.LaunchFromStart();
			}

			m_TakeDamagesAction?.Execute(gameObject);
			onTakeDamages?.Invoke(amount);
		}

		public void AddMaxHealth(int amount) {
			m_MaxHealth += amount;
			if (maxHealth < 1) {
				m_MaxHealth = 1;
			}

			onMaxHealthChanges?.Invoke(m_MaxHealth);
		}
		#endregion


		#region Die
		public void Die() {
			if (m_DeathAction != null) {
				m_DeathAction?.Execute(gameObject);
			} else {
				if (m_DefaultDestroyOnDie) {
					Destroy(gameObject);
				}
			}
		}
		#endregion
	}
}
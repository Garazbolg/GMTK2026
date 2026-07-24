namespace DevCore.Core {
	using UnityEngine;
	using System.Collections.Generic;

	/// <summary>
	/// The cooldown updater system
	/// Must not be destroyed
	/// </summary>
	[DefaultExecutionOrder(-500)]
	public class CooldownManager : MonoBehaviour {
		#region Currents
		internal static CooldownManager instance = null;
		#endregion


		#region Properties
		private static List<Cooldown> m_Cooldowns = new List<Cooldown>(100);
		#endregion


		#region Init
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Init() {
			//Create timer handle on run application
			GameObject go = new GameObject("Cooldown Manager");
			instance = go.AddComponent<CooldownManager>();
			DontDestroyOnLoad(go);
			
			ResetCache();
		}
		#endregion


		#region Callbacks
		private void Update() {
			UpdateTimers();
		}
		#endregion


		#region Timers
		
		internal static void RegisterCooldown(Cooldown cooldown) {
			m_Cooldowns.Add(cooldown);
		}

		internal static void UnregisterCooldown(Cooldown cooldown) {
			m_Cooldowns.Remove(cooldown);
		}

		private void UpdateTimers() {
			for (int i = m_Cooldowns.Count - 1; i >= 0; i--) {
				m_Cooldowns[i].Update();
			}
		}
		#endregion


		#region Cache
		private static void ResetCache() {
			m_Cooldowns = new List<Cooldown>(100);
		}
		#endregion
	}
}
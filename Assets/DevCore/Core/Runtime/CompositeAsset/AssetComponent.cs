using UnityEngine;

namespace DevCore.Core {
	public abstract class AssetComponent : ScriptableObject {
		#region Attributes
		[SerializeField, HideInInspector] private bool m_Active = true;
		#endregion


		#region Properties
		public bool active {
			get => m_Active;
			set {
				if (value == true && m_Active) {
					m_Active = true;
					OnComponentEnabled();
				} else if (value == false && !m_Active) {
					m_Active = false;
					OnComponentDisabled();
				}
			}
		} 
		#endregion


		#region Callbacks
		protected virtual void OnComponentEnabled() { }

		protected virtual void OnComponentDisabled() {}
		#endregion
	}
}
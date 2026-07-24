using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.Core {
	/// <summary>
	/// An asset with child component of the specified input type
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class CompositeAsset<T> : ScriptableObject where T : AssetComponent {
		#region Datas
		[SerializeField, HideInInspector] private List<T> m_Components = new List<T>(15);
		#endregion

		#region Properties
		protected int componentsCount => m_Components.Count;
		#endregion

		#region Manage Components
		protected T GetComponentAtIndex(int index) {
			return m_Components[index];
		}
		
		public TC GetComponent<TC>() where TC : T{
			for (int i = 0; i < m_Components.Count; i++) {
				var comp = m_Components[i]; 
				if (comp is TC targetComponent) {
					return targetComponent; 
				}
			}
				
			return null;
		}
		
		public bool TryGetComponent<TC>(out TC component) where TC : T{
			for (int i = 0; i < m_Components.Count; i++) {
				var comp = m_Components[i]; 
				if (comp is TC targetComponent) {
					component = targetComponent;
					return true; 
				}
			}
				
			component = null;
			return true;
		}
		
		public bool HasComponent<TC>() {
			for (int i = 0; i < m_Components.Count; i++) {
				if (m_Components[i].GetType() == typeof(TC)) {
					return true; 
				}
			}
		
			return false;
		}
		
		//TODO : Move this in the composite asset editor
		//Instances shall not create component at runtime
		internal T AddComponent(Type componentType) {
			var component = (T)CreateInstance(componentType);
			m_Components.Add(component);
			
			#if UNITY_EDITOR
			component.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            component.name = componentType.Name;
			#endif
			
			return component;
		}
		#endregion
	}
}

using System;
using UnityEngine;

namespace DevCore.ScriptableVariables {
	public class ScriptableVariable<T> : ScriptableVariableBase {
		#region Data
		[SerializeField] private T m_Value;

		[Header("Editor Behaviour")] 
		[SerializeField] private bool m_EditorValueChangeCallback = true; 
		#endregion


		#region Properties
		public T value {
			get => m_Value;
			set {
				m_Value = value;
				OnValueChanged();
			}
		}
		
		public override object wrappedValue {
			get => m_Value;
			set {
				if (m_Value is T validValue) {
					value = validValue;
					OnValueChanged();
				}
				else {
					throw new InvalidWrapperCastException();
				}
			} 
		}
		#endregion
		

		#region Wrapper Generics

		public override void ResetValue() {
			value = default;
		}
		#endregion
		
        
		#region Editor
#if UNITY_EDITOR
		private void OnValidate() {
			if (m_EditorValueChangeCallback && Application.isPlaying) {
				OnValueChanged();
			}			
		}
#endif
		#endregion
	}
}
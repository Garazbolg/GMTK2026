using System;
using UnityEngine;

namespace DevCore.ScriptableVariables {
	public abstract class ScriptableVariableBase : ScriptableObject, IValueWrapper {
		#region Events
		public event Action onValueChanged; 
		#endregion
		
		public abstract object wrappedValue { get; set; }
		
		public string wrapperName => name;

		public override string ToString() {
			return wrappedValue.ToString();
		}

		protected void OnValueChanged() {
			onValueChanged?.Invoke();	
		}
		public abstract void ResetValue();

		protected class InvalidWrapperCastException : System.InvalidCastException {
			public override string Message => "Invalid value wrapper type cast";
		}
	}
}
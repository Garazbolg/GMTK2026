using UnityEngine;

namespace DevCore.ScriptableVariables {
	[System.Serializable]
	public class ScriptableTableElement<T> : IValueWrapper where T : ScriptableVariableBase {
		#region Datas
		[SerializeField] private string m_Name = string.Empty;
		public T value;
		#endregion


		#region Properties
		public string name => m_Name;
		public string wrapperName => value.wrapperName;

		public object wrappedValue {
			get => value.wrappedValue;
			set => this.value.wrappedValue = value;
		}
		#endregion
	}
}
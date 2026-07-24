using System;

namespace DevCore.Core.Editor {
	public class TreeElement {
		#region Attributes
		private string m_Name = string.Empty;
		private object m_AttachedObject = null;
		private Action<object> m_ElementSelectionCallback = null;
		#endregion


		#region Properties
		public string name => m_Name;
		#endregion


		#region Construction
		public TreeElement(string name, object attachedObject,Action<object> selectionCallback) {
			m_Name = name;
			m_AttachedObject = attachedObject;
			m_ElementSelectionCallback = selectionCallback;
		}
		#endregion


		#region Methods
		public void Select() {
			m_ElementSelectionCallback.Invoke(m_AttachedObject);
		}
		#endregion
	}
}
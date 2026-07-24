using System;
using System.Text;
using DevCore.Core;
using DevCore.ScriptableVariables;
using TMPro;
using UnityEngine;

namespace DevCore.PrototypingToolkit {
	[RequireComponent(typeof(TextMeshProUGUI))]
	[AddComponentMenu("Prototyping Toolkit/UI/Variable UI Reader")]
	public class VariableUIReader : MonoBehaviour {
		[SerializeField, TextArea] private string m_Text = null;
		[SerializeField] private ScriptableVariableBase[] m_Variables = {};

		[Space]
		[SerializeField] private TextMeshProUGUI m_TextField = null;

		private void OnEnable() {
			for (int i = 0; i < m_Variables.Length; i++) {
				m_Variables[i].onValueChanged += OnAnyValueChanged;
			}			
		}


		private void OnDisable() {
			for (int i = 0; i < m_Variables.Length; i++) {
				m_Variables[i].onValueChanged -= OnAnyValueChanged;
			}
		}
		
		
		private void OnAnyValueChanged() {
			string text = string.Format(m_Text, m_Variables);
			if (m_TextField != null) {
				m_TextField.text = text;
			}
		}

		private void OnValidate() {
			try {
				OnAnyValueChanged();
			}
			catch (Exception e) {
				//Supports format exception in edit time
				if (AppCore.IsRunning() && e is not FormatException) {
					throw e;
				}
			}
			
			if (m_TextField == null) {
				m_TextField = GetComponent<TextMeshProUGUI>();
			}
		}
	}
}
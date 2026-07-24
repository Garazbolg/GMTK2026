using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevCore.DebugMenu {
	public class DebugInfoUI : MonoBehaviour {
		#region Settings
		[SerializeField] private Text m_Text = null;
		#endregion


		#region Currents
		protected DebugInfoHandle m_TargetHandle = null;
		protected string m_Label = string.Empty;
		#endregion


		#region Properties
		internal DebugInfoHandle targetHandle => m_TargetHandle;	
		#endregion

		
		#region Behaviour
		internal void Dispose() {
			m_TargetHandle = null;
		}

		internal void ResetLabel() {
			m_Label = $"<color=#ffd85c>{m_TargetHandle.m_Name}:</color>\n";
		}
		
		internal void Setup(DebugInfoHandle info) {
			m_TargetHandle = info;
			ResetLabel();
		}

		internal void Refresh() {
			m_Text.text = $"{m_Label}{m_TargetHandle.GetInfo()}";
		}
		#endregion
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DevCore.DebugMenu {
	public class DebugActionButton : MonoBehaviour{
		#region Settings
		[SerializeField] private DebugButton m_Button = null;
		[SerializeField] private Sprite m_DirectoryIcon = null;
		[SerializeField] private Sprite m_ActionIcon = null;

		[Space]
		[SerializeField] private Image m_IconRenderer = null;
		[SerializeField] private Text m_Text = null;
		#endregion


		#region Currents
		private DebugActionHandle m_Handle = null;
		#endregion


		#region Callbacks
		private void Awake() {
			m_Button.SetAction(CallAction);
		}
		#endregion
		
		#region Execution
		internal void SetAction(DebugActionHandle handle, bool isDirectory) {
			string name = handle.m_Name;
			if (isDirectory) {
				name = "    " + name;
			}

			SetAction(name, handle, isDirectory);
		}

		internal void SetAction(string name, DebugActionHandle handle, bool isDirectory) {
			m_Handle = handle;

			m_Text.text = name;
			m_Text.alignment = isDirectory ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter;

			m_IconRenderer.sprite = isDirectory ? m_DirectoryIcon : m_ActionIcon;
		}
		#endregion

		public void OnPointerClick(PointerEventData eventData) {
			CallAction();
		}

		public void CallAction() {
			m_Handle.TriggerAction();
		}
	}
}
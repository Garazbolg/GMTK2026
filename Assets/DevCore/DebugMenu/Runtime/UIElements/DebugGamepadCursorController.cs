using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DevCore.DebugMenu {
	public class DebugGamepadCursorController : MonoBehaviour {
		[SerializeField] private float m_EdgePadding = 2f;
		[SerializeField] private RectTransform m_Cursor = null;
		// [SerializeField] private DebugMenuInputModuleBase m_InputModule = null;

		[SerializeField] private DebugButton m_CurrentButton = null;

		private bool m_IsActivated = false;

		internal void SetButton(DebugButton button) {
			m_CurrentButton = button;
		}
		
		private void Update() {
			if (m_CurrentButton == null && m_IsActivated) {
				m_Cursor.gameObject.SetActive(false);
				m_IsActivated = false;
			} else if (m_CurrentButton != null && !m_IsActivated) {
				m_Cursor.gameObject.SetActive(true);
				m_IsActivated = true;
			}

			if (m_IsActivated && m_CurrentButton.TryGetComponent(out RectTransform rectTrs)) {
				Vector2 padding = new Vector2(m_EdgePadding, m_EdgePadding);
				m_Cursor.position = Vector3.Lerp(m_Cursor.position, rectTrs.position, Time.deltaTime / 0.04f);
				m_Cursor.sizeDelta = Vector2.Lerp(m_Cursor.sizeDelta,rectTrs.sizeDelta + padding, Time.deltaTime / 0.04f) ;
			}
		}
	}
}
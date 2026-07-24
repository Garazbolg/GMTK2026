using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DevCore.DebugMenu {
	public class DebugButton : Selectable, IPointerClickHandler {
		private Action m_Action;

		internal DebugButton m_LeftButton, m_RightButton, m_UpButton, m_DownButton = null;
		
		internal void SetAction(Action action) {
			m_Action = action;
		}
		
		public void OnPointerClick(PointerEventData eventData) {
			if (eventData.button == PointerEventData.InputButton.Left) {
				m_Action ?.Invoke();
			}
		}

		public override void OnPointerEnter(PointerEventData eventData) {
			base.OnPointerEnter(eventData);
			// DebugMenuController.m_Instance.m_CursorController.SetButton(this);			
		}
	}
}
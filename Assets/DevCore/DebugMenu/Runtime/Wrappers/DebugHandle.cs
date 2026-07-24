using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.DebugMenu {
	public abstract class DebugHandle {
		internal string m_Name;
		internal DebugCategory m_OwnerCategory = null;

		internal DebugHandle(string name, DebugCategory ownerCategory) {
			m_Name = name;
			m_OwnerCategory = ownerCategory;
		}

		public abstract void Unregister();

		/// <summary>
		/// Display the owner category of the debug handle
		/// </summary>
		public void ShowInMenu() {
			if (m_OwnerCategory != null) {
				if (!DebugMenu.isActive) {
					DebugMenu.Open();
				}

				DebugMenuController.m_Instance.DrawCategory(m_OwnerCategory);
			} else {
				Debug.LogError($"[Debug Menu] Owner categoy has not been found for {m_Name} debug handle");
			}
		}
	}
}
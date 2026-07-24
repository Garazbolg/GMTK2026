using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.DebugMenu {
	public class DebugCategory {
		#region Currents
		internal string m_Name = string.Empty;

		internal DebugCategory m_ParentCategory = null;

		internal List<DebugActionHandle> m_Actions = new List<DebugActionHandle>();
		internal List<DebugInfoHandle> m_Infos = new List<DebugInfoHandle>();

		internal List<DebugCategory> m_SubCategories = new List<DebugCategory>();
		internal DebugActionHandle m_DrawSelfHandle = null;
		internal int m_CategoryDepth = 0;
		#endregion


		#region Construction
		internal DebugCategory(string name, DebugCategory parentCategory, int depth) {
			m_ParentCategory = parentCategory;
			m_Name = name;
			m_DrawSelfHandle = new DebugActionHandle(m_Name, Draw, this);
			m_CategoryDepth = depth;
		}
		#endregion
		

		#region Actions
		public void Draw() {
			DebugMenuController.m_Instance.DrawCategory(this);
		}
		
		private void Refresh() {
			var controller = DebugMenuController.m_Instance;
			if (controller.m_CurrentDrawnCategory == this) {
				controller.DrawCategory(this, true);
			}		
		}
		#endregion

		#region Content
		/// <summary>
		/// Return an existing category or create it if doesn't exist
		/// </summary>
		internal DebugCategory GetOrAddSubCategory(string name, int depth) {
			for (int i = 0; i < m_SubCategories.Count; i++) {
				var subC = m_SubCategories[i];
				if (subC.m_Name == name) {
					return subC;
				}
			}

			return CreateSubCategory(name, depth);
		}

		internal DebugCategory CreateSubCategory(string name, int depth) {
			var cat = new DebugCategory(name, this, depth);
			for (int i = 0; i < m_SubCategories.Count; i++) {
				if (name.CompareTo(m_SubCategories[i].m_Name) <= 0) {
					m_SubCategories.Insert(i, cat);
					return cat;
				}
			}
			m_SubCategories.Add(cat);
			return cat;
		}

		internal DebugActionHandle RegisterAction(string name, DebugAction actionCallback,
			bool replaceIfExisting = false) {
			var handle = new DebugActionHandle(name, actionCallback, this);

			for (int i = 0; i < m_Actions.Count; i++) {
				var acName = m_Actions[i].m_Name;
				int strComp = name.CompareTo(acName);
				switch (strComp) {
					case -1:
						m_Actions.Insert(i, handle);
						return handle;
					case 0:
						if (name == acName) {
							Debug.LogError(
								$"[Debug Menu] : Cannot register the action of name [{name}], one already exists");
							return null;
						}
						break;
					default: break;
				}
			}

			m_Actions.Add(handle);
			return handle;
		}

		internal DebugInfoHandle RegisterInfo(string name, DebugInfoAction infoActionCallback,
			bool replaceIfExisting = false) {
			var handle = new DebugInfoHandle(name, infoActionCallback, this);

			for (int i = 0; i < m_Actions.Count; i++) {
				if (m_Actions[i].m_Name == name) {
					if (replaceIfExisting) {
						m_Infos[i] = handle;
						return handle;
					} else {
						Debug.LogError($"[Debug Menu] : Cannot register the info of name [{name}], one already exists");
						return null;
					}
				}
			}

			m_Infos.Add(handle);
			return handle;
		}

		/// <summary>
		/// Check if the category has content and remove this one if its empty
		/// </summary>
		private void CheckCategoryContent() {
			if (m_SubCategories.Count == 0 && m_Actions.Count == 0 && m_Infos.Count == 0) {
				m_ParentCategory.RemoveCategory(this);
				
				//Fallback to the root category
				if (DebugMenuController.m_Instance.m_CurrentDrawnCategory == this && m_CategoryDepth > 0) {
					DebugMenu.m_RootCategory.Draw();
				}
			}
		}
		
		internal void UnregisterAction(DebugActionHandle handle) {
			m_Actions.Remove(handle);
			CheckCategoryContent();
			Refresh();
		}

		internal void UnregisterInfo(DebugInfoHandle handle) {
			m_Infos.Remove(handle);
			CheckCategoryContent();
			Refresh();
		}

		private void RemoveCategory(DebugCategory category) {
			m_SubCategories.Remove(category);
			if (m_CategoryDepth > 0) {
				CheckCategoryContent();
			}
			Refresh();
		}
		#endregion
	}
}
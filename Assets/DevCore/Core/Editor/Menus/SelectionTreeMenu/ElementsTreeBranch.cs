using System.Collections.Generic;
using UnityEngine;

namespace DevCore.Core.Editor {
	public class ElementsTreeBranch {
		#region Attributes
		private string m_Name = string.Empty;
		private List<ElementsTreeBranch> m_ChildBranches = new List<ElementsTreeBranch>();
		private List<TreeElement> m_TreeElements = new List<TreeElement>();
		#endregion


		#region Properties
		public string name => m_Name;
		public int childBranchesCount => m_ChildBranches.Count;
		public int elementsCount => m_TreeElements.Count;
		#endregion


		#region Construction
		internal ElementsTreeBranch(string name) {
			m_Name = name;
		}
		#endregion


		#region Branches
		internal ElementsTreeBranch GetOrAddChildBranch(string name) {
			for (int i = 0; i < m_ChildBranches.Count; i++) {
				var branch = m_ChildBranches[i];
				if (branch.m_Name == name) {
					return branch;
				}
			}

			var newBranch = new ElementsTreeBranch(name);
			m_ChildBranches.Add(newBranch);
			return newBranch;
		}
		
		public ElementsTreeBranch GetChildBranchAt(int index) {
			return m_ChildBranches[index];
		}
		
		internal void AddChildBranch(ElementsTreeBranch branch) {
			if (!m_ChildBranches.Contains(branch)) {
				m_ChildBranches.Add(branch);
			} else {
				Debug.LogError($"The branch {branch.m_Name} is already child of {m_Name}");
			}
		}

		internal ElementsTreeBranch GetChildBranch(string name) {
			for (int i = 0; i < m_ChildBranches.Count; i++) {
				var branch = m_ChildBranches[i]; 
				if (branch.name == name) {
					return branch;
				}
			}

			return null;
		}
		
		internal void RemoveChildBranch(ElementsTreeBranch branch) {
			if (!m_ChildBranches.Contains(branch)) {
				m_ChildBranches.Remove(branch);
			} else {
				Debug.LogError($"The branch {branch.m_Name} is not child of {m_Name}");
			}
		}
		#endregion


		#region Elements
		public TreeElement GetTreeElementAt(int index) {
			return m_TreeElements[index];
		}
		
		public void AddElement(TreeElement element) {
			if (!m_TreeElements.Contains(element)) {
				m_TreeElements.Add(element);
			} else {
				Debug.LogError($"The element {element.name} is already contained into {m_Name}");
			}
		}

		public void RemoveElement(TreeElement element) {
			if (!m_TreeElements.Contains(element)) {
				m_TreeElements.Remove(element);
			} else {
				Debug.LogError($"The element {element.name} is not contained into {m_Name}");
			}
		}
		#endregion
	}
}
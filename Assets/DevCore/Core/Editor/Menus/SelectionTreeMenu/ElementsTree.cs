using System.Collections.Generic;
using UnityEngine;

namespace DevCore.Core.Editor {
	/// <summary>
	/// Multi elements hierarchy composed with branches
	/// Allow quick elements filtering
	/// </summary>
	public class ElementsTree {
		private ElementsTreeBranch m_RootBranch = null;
		private static List<string> m_cachedCategories = new List<string>();

		public ElementsTree(string rootBranchName) {
			m_RootBranch = new ElementsTreeBranch(rootBranchName);
		}

		public void AddElement(TreeElement element, string path) {
			if (path != null && path.Length > 0) {
				path.SplitNonAlloc('/', m_cachedCategories);
				ElementsTreeBranch branch = m_RootBranch;
				for (int i = 0; i < m_cachedCategories.Count; i++) {
					branch = branch.GetOrAddChildBranch(m_cachedCategories[i]);
				}
				branch.AddElement(element);
			} else {
				m_RootBranch.AddElement(element);
			}
		}

		public void AddElement(TreeElement element) {
			AddElement(element, null);
		}

		public ElementsTreeBranch GetRootBranch() {
			return m_RootBranch;
		}
	}
}
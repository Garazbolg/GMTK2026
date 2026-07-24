using UnityEditor;
using UnityEngine;

namespace DevCore.Core.Editor {
	public class ElementsTreeMenu {
		public static void DisplayElementSelectionTree(ElementsTree tree) {
			var genericMenu = new GenericMenu();
			var rootBranch = tree.GetRootBranch();
			for (int i = 0; i < rootBranch.childBranchesCount; i++) {
				var childBranch = rootBranch.GetChildBranchAt(i);
				string branchParentDirectory = childBranch.name + '/';
				AddBranchSubcategory(childBranch, genericMenu, branchParentDirectory);
			}
			
			for (int i = 0; i < rootBranch.elementsCount; i++) {
				var element = rootBranch.GetTreeElementAt(i);
				genericMenu.AddItem(new GUIContent(element.name), false, (userData) => {
					(userData as TreeElement).Select();
				}, element);
			}
			
			genericMenu.ShowAsContext();
		}

		private static void AddBranchSubcategory(ElementsTreeBranch branch, GenericMenu menu, string parentDirectory) {
			for (int i = 0; i < branch.childBranchesCount; i++) {
				var childBranch = branch.GetChildBranchAt(i);
				string branchParentDirectory = parentDirectory + childBranch.name + '/';
				AddBranchSubcategory(childBranch, menu, branchParentDirectory);
			}
			
			for (int i = 0; i < branch.elementsCount; i++) {
				var element = branch.GetTreeElementAt(i);
				menu.AddItem(new GUIContent(parentDirectory + element.name), false, (userData) => {
					(userData as TreeElement).Select();
				}, element);
			}
		}
	}
}
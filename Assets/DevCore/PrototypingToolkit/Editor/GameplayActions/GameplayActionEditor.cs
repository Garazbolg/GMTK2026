using DevCore.Core;
using DevCore.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace DevCore.PrototypingToolkit.Editor {
	[CustomEditor(typeof(GameplayAction))]
	public class GameplayActionEditor : CompositeAssetEditor {
		private EditorObjectTester<GameObject> m_Tester;

		protected override void OnEditorEnable() {
			m_Tester = new EditorObjectTester<GameObject>("Execute on:", true, IsGameObjectValid, TestGameObject);
		}

		private void TestGameObject(GameObject gameObject) {
			(target as GameplayAction).Execute(gameObject);
		}

		private bool IsGameObjectValid(GameObject gameObject) {
			return gameObject.IsSceneObject();
		}

		protected override void OnCompositeAssetFooterInpectorGUI() {
			m_Tester.DrawGUI();
		}
	}
}
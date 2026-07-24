using DevCore.Core;
using DevCore.Core.Editor;
using DevCore.FeedbackEngine;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FeedbackAsset))]
public class FeedbackAssetEditor : CompositeAssetEditor {
	private EditorObjectTester<GameObject> m_Tester = null;

	protected override void OnEditorEnable() {
		m_Tester = new EditorObjectTester<GameObject>("Play on:", true, IsGameObjectValid, TestGameObject);
	}

	private void TestGameObject(GameObject gameObject) {
		(target as FeedbackAsset).Play(gameObject);
	}

	private bool IsGameObjectValid(GameObject gameObject) {
		return gameObject.IsSceneObject();
	}

	protected override void OnCompositeAssetFooterInpectorGUI() {
		m_Tester.DrawGUI();
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.Rendering;
using UnityEngine;
using Assembly = System.Reflection.Assembly;
using Object = UnityEngine.Object;

namespace DevCore.Core.Editor {
	[CustomEditor(typeof(CompositeAsset<>), true)]
	public class CompositeAssetEditor : UnityEditor.Editor {
		#region Classes
		private class ComponentDrawer {
			public UnityEditor.Editor editor = null;
			public Texture icon = null;

			public ComponentDrawer(UnityEditor.Editor editor, Texture icon) {
				this.editor = editor;
				this.icon = icon;
			}

			public void Dispose() {
				DestroyImmediate(editor);
			}
		}
		#endregion
		
		#region Currents
		private SerializedProperty m_Components = null;
		
		private static ElementsTree m_CachedComponentsTree = null;
		private MethodInfo m_AddComponentMethod = null;

		private List<ComponentDrawer> m_ComponentDrawers = new List<ComponentDrawer>();
		#endregion
		
		#region Callbacks
		private void OnEnable() {
			InitSerializedObjectDatas();
			OnEditorEnable();
		}

		protected virtual void OnEditorEnable() { }

		private void InitSerializedObjectDatas() {
			if (m_AddComponentMethod == null) {
				m_AddComponentMethod = target.GetType().GetMethod(
					"AddComponent",
					(BindingFlags.Instance | BindingFlags.NonPublic),
					Type.DefaultBinder,
					new Type[] {typeof(Type)}, null);
			}
			
			if (target != null) {
				m_Components = serializedObject.FindProperty("m_Components");
				UpdateEditorList();
			}
		}

		public sealed override void OnInspectorGUI() {
			base.OnInspectorGUI();

			OnCompositeAssetInpectorGUI();
			
			if (m_Components == null) {
				InitSerializedObjectDatas();
			}
			else if (m_Components.arraySize != m_ComponentDrawers.Count) {
				UpdateEditorList();
			}

			serializedObject.Update();
			
			EditorGUILayout.Space();

			if (m_Components.arraySize == 0)
			{
				EditorGUILayout.HelpBox("No Components Added", MessageType.Info);
			}
			else
			{
				//Draw List
				CoreEditorUtils.DrawSplitter();
				for (int i = 0; i < m_Components.arraySize; i++)
				{
					SerializedProperty componentProperty = m_Components.GetArrayElementAtIndex(i);
					DrawComponent(i, ref componentProperty);
					CoreEditorUtils.DrawSplitter();
				}
			}
			
			EditorGUILayout.Space();
			if (GUILayout.Button("Add Component")) {
				DisplayAddComponentMenu();
			}
			
			EditorGUILayout.Space(5f);
			OnCompositeAssetFooterInpectorGUI();
		}

		protected virtual void OnCompositeAssetInpectorGUI() { }
		protected virtual void OnCompositeAssetFooterInpectorGUI() { }
		#endregion

		#region Draw
		private void DrawComponent(int index, ref SerializedProperty componentProperty)
        {
            Object componentObjRef = componentProperty.objectReferenceValue;
            if (componentObjRef != null)
            {
                bool hasChangedProperties = false;
                string title = ObjectNames.GetInspectorTitle(componentObjRef);

                // Get the serialized object for the editor script & update it
                ComponentDrawer componentDrawer = m_ComponentDrawers[index];
                UnityEditor.Editor componentEditor = componentDrawer.editor;
                SerializedObject serializedComponentEditor = componentEditor.serializedObject;
                serializedComponentEditor.Update();

                // Foldout header
                EditorGUI.BeginChangeCheck();
                SerializedProperty activeProperty = serializedComponentEditor.FindProperty("m_Active");
                GUIContent headerContent = EditorGUIUtility.TrTextContent(title, string.Empty);
                
                if (componentDrawer.icon) {
	                headerContent.image = componentDrawer.icon;
                }
                
                bool displayContent = CoreEditorUtils.DrawHeaderToggle(headerContent, 
	                componentProperty, activeProperty, pos => OnContextClick(pos, index));
                hasChangedProperties |= EditorGUI.EndChangeCheck();

                // ObjectEditor
                if (displayContent)
                {
                    EditorGUI.BeginChangeCheck();
                    componentEditor.OnInspectorGUI();
                    hasChangedProperties |= EditorGUI.EndChangeCheck();

                    EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
                }

                // Apply changes and save if the user has modified any settings
                if (hasChangedProperties)
                {
                    serializedComponentEditor.ApplyModifiedProperties();
                    serializedObject.ApplyModifiedProperties();
                    ForceSave();
                }
            }
        }
		
		private void OnContextClick(Vector2 position, int id)
		{
			var menu = new GenericMenu();

			if (id == 0)
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Move Up"));
			else
				menu.AddItem(EditorGUIUtility.TrTextContent("Move Up"), false, () => MoveComponent(id, -1));

			if (id == m_Components.arraySize - 1)
				menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Move Down"));
			else
				menu.AddItem(EditorGUIUtility.TrTextContent("Move Down"), false, () => MoveComponent(id, 1));

			menu.AddSeparator(string.Empty);
			menu.AddItem(EditorGUIUtility.TrTextContent("Remove"), false, () => RemoveComponent(id));

			menu.DropDown(new Rect(position, Vector2.zero));
		}
		
		private void DisplayAddComponentMenu() {
			if (m_CachedComponentsTree == null) {
				BuildComponentsTreeCache();
			}

			ElementsTreeMenu.DisplayElementSelectionTree(m_CachedComponentsTree);
			m_CachedComponentsTree = null;
		}
		#endregion

		#region Manage Editors
		private void UpdateEditorList()
		{
			ClearEditorsList();
			for (int i = 0; i < m_Components.arraySize; i++) {
				SerializedProperty component = m_Components.GetArrayElementAtIndex(i);
				MonoScript monoScript = MonoScript.FromScriptableObject(component.objectReferenceValue as ScriptableObject);
				MonoImporter componentImporter = MonoImporter.GetAtPath(AssetDatabase.GetAssetPath(monoScript)) as MonoImporter;
				Texture2D icon = null;
				if (componentImporter) {
					icon = componentImporter.GetIcon();
				}

				UnityEditor.Editor editor = CreateEditor(component.objectReferenceValue);
				ComponentDrawer drawer = new ComponentDrawer(editor, icon);
				m_ComponentDrawers.Add(drawer);
			}
		}
		
		private void ClearEditorsList()
		{
			for (int i = m_ComponentDrawers.Count - 1; i >= 0; --i)
			{
				m_ComponentDrawers[i].Dispose();
			}
			m_ComponentDrawers.Clear();
		}
		#endregion

		#region Manage Component Datas
		private void CreateComponent(Type componentType) {
			Undo.RecordObject(target, "Add Component");
			var component = (Object)m_AddComponentMethod.Invoke(target, new object[]{componentType});
			
			Undo.RegisterCreatedObjectUndo(component, "Add Asset Component");
			
			if (EditorUtility.IsPersistent(target)) {
				AssetDatabase.AddObjectToAsset(component, target);
				AssetDatabase.Refresh();
				
				ForceSave();
			}


			UpdateEditorList();
		}
		
		private void RemoveComponent(int id) {
			SerializedProperty property = m_Components.GetArrayElementAtIndex(id);
			Object objectRef = property.objectReferenceValue;
			property.objectReferenceValue = null;

			Undo.RecordObject(target, objectRef == null ? "Remove Component" : $"Remove {objectRef.name}");

			// remove the array index itself from the list
			m_Components.DeleteArrayElementAtIndex(id);
			UpdateEditorList();
			serializedObject.ApplyModifiedProperties();
			
			// Destroy the setting object after ApplyModifiedProperties(). If we do it before, redo
			// actions will be in the wrong order and the reference to the setting object in the
			// list will be lost.
			if (objectRef != null)
			{
				AssetComponent component = objectRef as AssetComponent;
				if (component != null) {
					component.active = false;
				}
				
				Undo.DestroyObjectImmediate(objectRef);
			}

			
			// Force save / refresh
			ForceSave();
		}

		private void MoveComponent(int id, int offset) {
			Undo.SetCurrentGroupName("Move Render Feature");
			serializedObject.Update();
			m_Components.MoveArrayElement(id, id + offset);
			UpdateEditorList();
			serializedObject.ApplyModifiedProperties();

			// Force save / refresh
			ForceSave();
		}

		private void BuildComponentsTreeCache() {
			Type parentGenericType = null;
			var targetGenericParentTypeDef = typeof(CompositeAsset<AssetComponent>).GetGenericTypeDefinition();
			Type currentCheckedParent = target.GetType().BaseType;

			while (parentGenericType == null) {
				if (currentCheckedParent != null) {
					if (currentCheckedParent.IsGenericType &&
					    currentCheckedParent.GetGenericTypeDefinition() == targetGenericParentTypeDef) {
						parentGenericType = currentCheckedParent;
					} else {
						currentCheckedParent = currentCheckedParent.BaseType;
					}
				} else {
					throw new Exception($"Didn't found parent generic type for asset of type {target.name}");
				}
			}

			var parentTypeInfo = parentGenericType.GetTypeInfo();
			var childsAssetBaseType = parentTypeInfo.GenericTypeArguments[0];
			var childAssetsTypes = ProjectDomainUtility.FindAllUnityObjectsTypesOf(childsAssetBaseType);

			var availableComponentsTree = new ElementsTree("Add Component");

			for (int i = 0; i < childAssetsTypes.Length; i++) {
				var type = childAssetsTypes[i];
				if (type.IsAbstract) {
					continue;
				}

				var addComponentMenuAttribute = type.GetCustomAttribute<AddComponentMenu>();
				if (addComponentMenuAttribute != null) {
					string path = addComponentMenuAttribute.componentMenu;

					if (path != null && !string.IsNullOrEmpty(path) && !string.IsNullOrWhiteSpace(path)) {
						string name;
						string parentPath = null;
						if (path.Contains('/')) {
							int lastSeparatorIndex = path.LastIndexOf('/');
							name = path.Substring(lastSeparatorIndex + 1, path.Length - lastSeparatorIndex - 1);
							parentPath = path.Substring(0, lastSeparatorIndex);
						} else {
							name = path;
						}

						availableComponentsTree.AddElement(
							new TreeElement(name, type, (obj) => { CreateComponent(obj as Type); }), parentPath);	
					} else {
						availableComponentsTree.AddElement(new TreeElement(ObjectNames.NicifyVariableName(type.Name), type,
							(obj) => { CreateComponent(obj as Type); }));	
					}
				} else {
					availableComponentsTree.AddElement(new TreeElement(ObjectNames.NicifyVariableName(type.Name), type,
						(obj) => { CreateComponent(obj as Type); }));
				}

				m_CachedComponentsTree = availableComponentsTree;
			}
		}
		#endregion

		#region Utils
		private void ForceSave() {
			EditorUtility.SetDirty(target);
		}
		#endregion
	}
}
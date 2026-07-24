//Created by Julien Delaunay, see more on https://github.com/Sorangon/Enhanced-Scene-Manager

using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DevCore.SceneManagement.Legacy.Internal;
using Object = UnityEngine.Object;

namespace DevCore.SceneManagement.Legacy {
	public enum LoadingState {
		Sleeping,
		Loading,
		Unloading
	}

	public enum LoadingStepResult {
		Success,
		Canceled
	}

	public delegate void LoadingCallback(LoadingStepResult result);
	
	/// <summary>
	///Manage the scene groups, refers the current used scene list 
	/// </summary>
	public static class EnhancedSceneManager {
		#region Current
		private static SceneBundleList m_CurrentSceneList = null;
		private static SceneBundle m_LastLoadedBundle = null;
		private static Scene m_SetActiveScene;
		private static EnhancedSceneOrchestrator m_SceneOrchestratorInstance = null;
		private static LoadingState m_LoadingState = LoadingState.Sleeping;
		#endregion


		#region Properties
		/// <summary>Returns the last loaded scene bundle from the <see cref="EnhancedSceneManager"/></summary>
		public static SceneBundle lastLoadedBundle => m_LastLoadedBundle;

		/// <summary>Returns the current loading state of the <see cref="EnhancedSceneManager"/></summary>
		public static LoadingState loadingState => m_LoadingState;
		#endregion


		#region Events and Delegates
		public static event Action onStartLoading;
		public static event Action onSceneAllUnloaded, onSceneAllLoaded;
		#endregion


		#region Manage Scene List
		/// <summary>
		/// Returns the current used scene list
		/// </summary>
		/// <returns></returns>
		public static SceneBundleList GetCurrentSceneList() {
			if (m_CurrentSceneList == null) {
				m_CurrentSceneList = StartupSceneManagerSetting.GetCurrentSceneList();
			}

			return m_CurrentSceneList;
		}

		/// <summary>
		/// Sets the current scene list
		/// </summary>
		internal static void SetCurrentSceneList(SceneBundleList sceneList) {
			m_CurrentSceneList = sceneList;

#if UNITY_EDITOR
			//Update startup manager setting
			StartupSceneManagerSetting.Instance.CurrentSceneList = m_CurrentSceneList;
#endif
		}
		#endregion


		#region Initialize
		/// <summary>
		/// Clear the cache calues
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialize() {
			m_LoadingState = LoadingState.Sleeping;
		}
		#endregion


		#region Scenes Loading
		/// <summary>
		/// Load a new scene bundle after unloading the current one
		/// </summary>
		/// <param name="groupName">The target bundle</param>
		public static void LoadSceneBundle(SceneBundle bundle) {
			LoadSceneInternal(bundle, null, false);
		}

		/// <summary>
		/// Asynchronously load a new scene bundle after unloading the current one
		/// </summary>
		/// <param name="groupName">The target bundle</param>
		public static void LoadSceneBundleAsync(SceneBundle bundle, LoadingCallback onLoadingComplete = null) {
			LoadSceneInternal(bundle, onLoadingComplete, true);
		}

		private static void LoadSceneInternal(SceneBundle bundle, LoadingCallback onLoadingComplete, bool async) {
			if (!CanLoad(bundle)) {
				onLoadingComplete?.Invoke(LoadingStepResult.Canceled);
			}
			
			onStartLoading?.Invoke();
			CheckSceneOrchestratorInstance();
			
			m_SceneOrchestratorInstance.StartLoadingCoroutine(LoadingProcessCoroutine(bundle,
				async, onLoadingComplete)); //Call a start coroutine on the scene oorchestrator
		}

		/// <summary>
		/// Load scenes in bundle
		/// </summary>
		private static void LoadScenes(SceneBundle bundle, bool hasToFullyReload, int persistantScenesCount) {
			//Get scenes in the bundle
			var scenes = bundle.GetScenes();

			SceneManager.LoadScene(scenes[0], hasToFullyReload ? LoadSceneMode.Single : LoadSceneMode.Additive);
			m_SetActiveScene =
				SceneManager.GetSceneAt(SceneManager.sceneCount - 1); //Get the loaded scene to set it as active later

			for (int i = 1; i < scenes.Length; i++) {
				SceneManager.LoadScene(scenes[i], LoadSceneMode.Additive);
			}

			if (persistantScenesCount >= 0) {
				//Check if persistant scenes are correctly loaded
				if (persistantScenesCount != m_CurrentSceneList.PersistantScenesBundle.ScenesCount) {
					//Re-open persistant scenes
					for (int i = 0; i < m_CurrentSceneList.PersistantScenesBundle.ScenesCount; i++) {
						SceneManager.LoadScene(m_CurrentSceneList.PersistantScenesBundle.GetSceneAtID(i),
							LoadSceneMode.Additive);
					}
				}
			}
		}

		/// <summary>
		/// Check if the bundle can be loaded
		/// </summary>
		/// <returns></returns>
		public static bool CanLoad(SceneBundle targetBundle) {
			//Return false if trying to load a bundle that doesn't belong to the current scene list
			if (!m_CurrentSceneList.HasBundleInList(targetBundle)) {
				Debug.LogError(
					"[Enhanced Scene Manager] This scene bundle doesn't belong to the scene list! It cannot be loaded");
				return false;
			}

			//Return false if trying to load persistant scene bundle
			if (targetBundle == m_CurrentSceneList.PersistantScenesBundle) {
				Debug.LogError(
					"[Enhanced Scene Manager] Persistant scene bundle is automaticaly and permanently loaded, you cannot load it manually");
				return false;
			}

			switch (m_LoadingState) {
				case LoadingState.Loading:
					Debug.LogError(
						"[Enhanced Scene Manager] Cannot load another bundle while the Enhanced Scene Manager is loading scenes");
					return false;

				case LoadingState.Unloading:
					Debug.LogError(
						"[Enhanced Scene Manager] Cannot load another bundle while the Enhanced Scene Manager is unloading scenes");
					return false;

				default:
					break;
			}

			return true;
		}

		/// <summary>
		/// Returns all the scene that shoud be unloaded
		/// </summary>
		/// <param name="hasToFullyReload"></param>
		/// <param name="persistantScenesCount"></param>
		/// <returns></returns>
		private static List<Scene> GetScenesToUnload(out bool hasToFullyReload, out int persistantScenesCount) {
			hasToFullyReload = false;
			//This value will be incremented each persisant scene loaded, will be checked later to get missing persistant scenes
			persistantScenesCount =
				m_CurrentSceneList.PersistantScenesBundle != null
					? 0
					: -1; //Pesistant are equal to -1 if any persistant level has been referenced

			var dirtyScenes = new List<Scene>();

			if (persistantScenesCount >= 0) {
				//Filter persistant scenes if there are referenced
				for (int i = 0; i < SceneManager.sceneCount; i++) {
					//Filter persistant scenes
					Scene scene = SceneManager.GetSceneAt(i);

					//Check if iterated scene is persisant
					var persisantFlag = false;
					for (int j = 0; j < m_CurrentSceneList.PersistantScenesBundle.ScenesCount; j++) {
						if (scene.name == m_CurrentSceneList.PersistantScenesBundle.GetSceneAtID(j)) {
							persisantFlag = true;
						}
					}

					if (persisantFlag) {
						//Doesn't add persistant scenes to dirty list
						persistantScenesCount++; //Count persistant scenes to check if each one is correctly loaded 
					} else {
						dirtyScenes.Add(scene);
					}
				}
			} else {
				//Simply add all scenes if there isn't persistant bundle
				for (int i = 0; i < SceneManager.sceneCount; i++) {
					//Filter persistant scenes
					Scene scene = SceneManager.GetSceneAt(i);
					dirtyScenes.Add(scene);
				}
			}

			//Check if all the scenes will be unload
			hasToFullyReload = SceneManager.sceneCount <= dirtyScenes.Count;
			return dirtyScenes;
		}
		#endregion


		#region Coroutines
		/// <summary>
		/// The loading process coroutine
		/// </summary>
		/// <param name="async"></param>
		/// <returns></returns>
		private static IEnumerator LoadingProcessCoroutine(SceneBundle bundle, bool async, LoadingCallback completeCallback = null) {
			List<Scene> scenesToUnload = GetScenesToUnload(out bool hasToFullyReload, out int persistantScenesCount);
			yield return UnloadingAsyncCoroutine(scenesToUnload, hasToFullyReload);
			
			//Load scenes
			m_LoadingState = LoadingState.Loading;
			if (async) {
				yield return LoadingAsyncCoroutine(bundle, hasToFullyReload, persistantScenesCount);
			} else {
				LoadScenes(bundle, hasToFullyReload, persistantScenesCount);
			}

			//Has to wait for a complete frame
			yield return null;

			m_LoadingState = LoadingState.Sleeping;
			SceneManager.SetActiveScene(m_SetActiveScene);
			m_LastLoadedBundle = bundle;
			
			
			onSceneAllLoaded?.Invoke();

			if (async) {
				m_SceneOrchestratorInstance.StopLoadingCoroutine(); //Stops the coroutine handler
			}
			
			//Setup environment
			DynamicGI.UpdateEnvironment();
			LightProbes.Tetrahedralize();
			
			completeCallback?.Invoke(LoadingStepResult.Success);
		}

		/// <summary>
		/// Async loading coroutine
		/// </summary>
		/// <param name="bundle"></param>
		/// <returns></returns>
		private static IEnumerator LoadingAsyncCoroutine(SceneBundle bundle, bool hasToFullyReload,
			int persistantScenesCount) {
			//Get scenes in the bundle
			var scenes = bundle.GetScenes();

			AsyncOperation loadingOp;
			loadingOp = SceneManager.LoadSceneAsync(scenes[0],
				hasToFullyReload ? LoadSceneMode.Single : LoadSceneMode.Additive);
			yield return loadingOp;

			m_SetActiveScene =
				SceneManager.GetSceneAt(SceneManager.sceneCount - 1); //Get the loaded scene to set it as active later

			for (int i = 1; i < scenes.Length; i++) {
				loadingOp = SceneManager.LoadSceneAsync(scenes[i], LoadSceneMode.Additive);
				yield return loadingOp;
			}

			if (persistantScenesCount >= 0) {
				//Check if persistant scenes are correctly loaded
				if (persistantScenesCount != m_CurrentSceneList.PersistantScenesBundle.ScenesCount) {
					//Re-open persistant scenes
					for (int i = 0; i < m_CurrentSceneList.PersistantScenesBundle.ScenesCount; i++) {
						loadingOp = SceneManager.LoadSceneAsync(
							m_CurrentSceneList.PersistantScenesBundle.GetSceneAtID(i), LoadSceneMode.Additive);
						yield return loadingOp;
					}
				}
			}
		}

		/// <summary>
		/// Async unloading coroutine
		/// </summary>
		/// <param name="bundle"></param>
		/// <returns></returns>
		private static IEnumerator UnloadingAsyncCoroutine(List<Scene> scenesToUnload, bool hasToFullyReload) {
			m_LoadingState = LoadingState.Unloading;

			//Unload current scene except persistant ones, skip one scene if all scenes have to be reloaded
			for (int i = hasToFullyReload ? 1 : 0; i < scenesToUnload.Count; i++) {
				AsyncOperation unload = SceneManager.UnloadSceneAsync(scenesToUnload[i]);
				yield return unload;
			}

			m_LoadingState = LoadingState.Sleeping;
			onSceneAllUnloaded?.Invoke();
		}
		#endregion


		#region Scene Orchestrator
		/// <summary>
		/// Instantaite abd initialize a scene orchestrator
		/// </summary>
		private static void CheckSceneOrchestratorInstance() {
			if (m_SceneOrchestratorInstance != null) {
				return;
			}
			EnhancedSceneOrchestrator prefab = Resources.Load<EnhancedSceneOrchestrator>("SceneOrchestrator");
			m_SceneOrchestratorInstance = Object.Instantiate(prefab);
			Object.DontDestroyOnLoad(m_SceneOrchestratorInstance);
		}
		#endregion
	}
}
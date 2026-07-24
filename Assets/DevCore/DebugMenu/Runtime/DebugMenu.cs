using System;
using UnityEngine;

namespace DevCore.DebugMenu {
	#region Delegates
	public delegate void DebugAction();

	public delegate string DebugInfoAction();
	#endregion


	/// <summary>
	/// Managed multiple debug options and values
	/// </summary>
	public static class DebugMenu {
		#region Constants
		public const int MAX_SUBMENU_COUNT = 20;
		#endregion


		#region Properties
		public static bool isActive {
			get {
				if (IsEditor()) return false;

				if (CheckDebugMenuInstance()) {
					return DebugMenuController.m_Instance.isMenuActive;
				} else {
					return false;
				}
			}
		}
		#endregion


		#region Currents
		internal static DebugCategory m_RootCategory = null;
		private static string[] m_PathsSearchBuffer = new string[MAX_SUBMENU_COUNT];
		private static int m_PathSearchDepth = 0;
		private static bool m_IsActive = false;
		private static DebugMenuSettings m_Settings = DebugMenuSettings.GetAsset();
		#endregion


		#region Callbacks
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void OnInitialize() {
			if (!m_IsActive) {
				Initialize();
			}
		}


		#endregion


		#region Initialization
		private static void Initialize() {
			if (!m_Settings.enableDebugMenu) {
				return;
			}

			if (DebugMenuController.m_Instance == null) {
				//TODO : Instantiate a different prefab if input system is used
				var controller = GameObject.Instantiate(Resources.Load<GameObject>("DebugMenu"));
				controller.name = "DebugMenuController";
			}
			
			m_RootCategory = new DebugCategory("Menu", null, 0);
			for (int i = 0; i < MAX_SUBMENU_COUNT; i++) {
				m_PathsSearchBuffer[i] = string.Empty;
			}

			m_PathSearchDepth = 0;
			m_IsActive = true;
		}
		#endregion
		

		#region Display
		/// <summary>
		/// Draw the debug menu
		/// </summary>
		public static void Open() {
#if UNITY_EDITOR
			if (IsEditor()) return;
#endif

			if (CheckDebugMenuInstance()) {
				DebugMenuController.m_Instance.OpenDebugMenu();
			}
		}

		/// <summary>
		/// Hide the debug menu
		/// </summary>
		public static void Close() {
#if UNITY_EDITOR
			if (IsEditor()) return;
#endif

			if (CheckDebugMenuInstance()) {
				DebugMenuController.m_Instance.CloseDebugMenu();
			}
		}

		/// <summary>
		/// Display or Hide the debug menu depending the current display state
		/// </summary>
		public static void ToggleMenu() {
#if UNITY_EDITOR
			if (IsEditor()) return;
#endif

			if (CheckDebugMenuInstance()) {
				if (DebugMenuController.m_Instance.isMenuActive) {
					DebugMenuController.m_Instance.CloseDebugMenu();
				} else {
					DebugMenuController.m_Instance.OpenDebugMenu();
				}
			}
		}

		/// <summary>
		/// Clear all pinned informations on pinboard
		/// </summary>
		public static void ClearPinboard() {
#if UNITY_EDITOR
			if (IsEditor()) return;
#endif
			if (CheckDebugMenuInstance()) {
				DebugMenuController.m_Instance.ClearPinboard();
			}
		}
		
		private static bool CheckDebugMenuInstance() {
			if (DebugMenuController.m_HasInstance) {
				return true;
			} else {
				Debug.LogError("[Debug Menu Controller] : No Debug menu object exists in the current context, " +
				               "ensure you try to access it after Awake");
			}

			return false;
		}
		#endregion


		#region Register
		public static DebugActionHandle RegisterAction(string path, DebugAction action, bool replaceIfExisting = false) {
#if UNITY_EDITOR
			if (IsEditor()) return null;
#endif
			if (PrepareRegistration(path, out DebugCategory category, out string name)) {
				return category.RegisterAction(name, action, replaceIfExisting);
			} else {
				return null;
			}
		}

		public static DebugInfoHandle RegisterInfo(string path, DebugInfoAction infoActionCallback,
			bool replaceIfExisting = false) {
#if UNITY_EDITOR
			if (IsEditor()) return null;
#endif
			
			if (PrepareRegistration(path, out DebugCategory category, out string name)) {
				return category.RegisterInfo(name, infoActionCallback, replaceIfExisting);
			} else {
				return null;
			}
		}

		/// <summary>
		/// Return true if registration is valid and return target category and action name
		/// </summary>
		/// <param name="category"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private static bool PrepareRegistration(string path, out DebugCategory category, out string name) {
			if (!m_IsActive) {
				if (!m_Settings.enableDebugMenu) {
					category = null;
					name = string.Empty;
					return false;
				}
				
				Initialize();
			}
			
			if (!RefreshSearchPath(path)) {
				category = null;
				name = null;
				return false;
			}

			category = GetDeepestCategoryForCurrentPath();
			name = m_PathsSearchBuffer[m_PathSearchDepth - 1];
			return true;
		}

		private static DebugCategory GetDeepestCategoryForCurrentPath() {
			DebugCategory cat = m_RootCategory;
			if (m_PathSearchDepth > 1) {
				for (int i = 0; i < m_PathSearchDepth - 1; i++) {
					cat = cat.GetOrAddSubCategory(m_PathsSearchBuffer[i], i + 1);
				}
			}

			return cat;
		}

		private static bool RefreshSearchPath(string path) {
			if (string.IsNullOrEmpty(path)) {
				Debug.LogError($"[Debug Menu] A Debug Path cannot be null or empty");
				return false;
			}

			if (path[0] == '/') {
				Debug.LogError($"[Debug Menu] A Debug Path cannot start with a separator");
				return false;
			}

			int separatorIndex = 0;
			int lastSeparatorIndex = 0;
			m_PathSearchDepth = 0;
			while (separatorIndex > -1) {
				separatorIndex = path.IndexOf('/', lastSeparatorIndex);

				string category;
				if (separatorIndex > -1) {
					category = path.Substring(lastSeparatorIndex, separatorIndex - lastSeparatorIndex);
					if (category.Length == 0) {
						Debug.LogError($"[Debug Menu] A Debug Path cannot has an empty category");
						return false;
					}

					separatorIndex++;
				} else {
					category = path.Substring(lastSeparatorIndex);
					if (category.Length == 0) {
						Debug.LogError($"[Debug Menu] A Debug Path cannot contains an empty function");
						return false;
					}
				}

				m_PathsSearchBuffer[m_PathSearchDepth] = category;
				m_PathSearchDepth++;


				lastSeparatorIndex = separatorIndex;

				if (m_PathSearchDepth > MAX_SUBMENU_COUNT) {
					Debug.LogError(
						$"[Debug Menu] Maximum sub menu depth has been reached, ensure your path depth doesn't exceed {MAX_SUBMENU_COUNT}");
					return false;
				}
			}

			return true;
		}
		#endregion


		#region Utils
		private static bool IsEditor() {
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                Debug.LogError("[Debug Menu] : Cannot access the debug menu in edit mode");
                return true;
            }
#endif
			return false;
		}
		#endregion
	}
}
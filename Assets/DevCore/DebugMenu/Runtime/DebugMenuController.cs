using System;
using System.Collections.Generic;
using DevCore.ApplicationStates;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

namespace DevCore.DebugMenu {
	[DefaultExecutionOrder(-5000), RequireComponent(typeof(Canvas))]
	public sealed class DebugMenuController : MonoBehaviour {
		#region Settings
		[SerializeField] private GameObject m_MenuGroup = null;
		[SerializeField] private GameObject m_PinboardGroup = null;
		[SerializeField] private DebugButton m_QuitButton = null;

		[Header("Actions Panel")]
		[SerializeField] private DebugActionButton m_BrowseToParentAction = null;
		[SerializeField] private DebugActionButton m_ActionButtonPrefab = null;
		[SerializeField] private Transform m_ActionButtonsContainer = null;
		[SerializeField] private RectTransform m_ActionsSeparator = null;

		[Header("Infos Panel")]
		[SerializeField] private DebugInfoButton m_InfoButtonPrefab = null;
		[SerializeField] private RectTransform m_DebugInfosContainer = null;

		[Header("Pin Board")]
		[SerializeField] private DebugInfoUI m_InfoUIPrefab = null;
		[SerializeField] private RectTransform m_PinnednInfoContainer = null;
		[SerializeField] private DebugButton m_TogglePinboardVisibilityButton = null;
		[SerializeField] private Image m_VisbilityButtonImage = null;
		[SerializeField] private DebugButton m_ClearPinBoardButton = null;

		[Header("Paths")]
		[SerializeField] private DebugCategoryPathUI m_PathButton = null;
		[SerializeField] private RectTransform m_PathButtonsContainer = null;

		[Header("Icons")]
		[SerializeField] private Sprite m_VisibleIcon = null;
		[SerializeField] private Sprite m_HiddenIcon = null;

		[Header("Sub Systems")]
		[SerializeField] internal DebugGamepadCursorController m_CursorController = null;
		#endregion


		#region Currents
		[Header("Pools")]
		[SerializeField] private List<DebugActionButton> m_ActionButtonsPool = new List<DebugActionButton>();
		[SerializeField] private List<DebugInfoButton> m_InfoButtonsPool = new List<DebugInfoButton>();
		[SerializeField] private List<DebugInfoUI> m_PinboardInfosPool = new List<DebugInfoUI>();
		[SerializeField] private List<DebugCategoryPathUI> m_PathButtonsPool = new List<DebugCategoryPathUI>();

		internal static DebugMenuController m_Instance = null;
		internal static bool m_HasInstance = false;

		private DebugMenuSettings m_DebugSettings;

		private DebugMenuState m_CurrentMenuState = null;
		private DebugMenuStateSetting m_Setting = null;
		private bool m_DrawnOnce = false;

		private Canvas m_Canvas = null;

		internal DebugCategory m_CurrentDrawnCategory = null;
		private float m_LastDisplayWidth = 0f;

		//Info texts
		private int m_DrawnInformationsCount = 0;

		//Pin board
		private List<DebugInfoUI> m_PinnedInfos = new List<DebugInfoUI>(20);
		private bool m_IsPinboardActive = false;
		private bool m_ArePinboardUtilsActive = false;

		//Paths
		private int m_PrevDrawnCategories = 0;

		//Infos Refresh
		private int m_RefreshFramesToWait = 0;
		private int m_CurrentRefreshedInfo = 0;
		private int m_CurrentRefreshedPinnedInfos = 0;
		#endregion


		#region Properties
		public bool isMenuActive => m_CurrentMenuState != null;
		#endregion


		#region Callbacks
		private void Awake() {
			m_Canvas = GetComponent<Canvas>();

			if (m_Instance != null) {
				Debug.LogError("[Debug Menu Controller] : Only one instance can be created");
				Destroy(gameObject);
			} else {
				m_Instance = this;
				m_HasInstance = true;
				DontDestroyOnLoad(gameObject);

				if (m_MenuGroup.gameObject.activeSelf) {
					m_MenuGroup.gameObject.SetActive(false);
				}

				m_Setting = new DebugMenuStateSetting(this);
			}

			m_LastDisplayWidth = GetDisplayWidth();

			m_QuitButton.SetAction(CloseDebugMenu);
			m_ClearPinBoardButton.SetAction(ClearPinboard);
			m_TogglePinboardVisibilityButton.SetAction(TogglePinboardVisbility);

			m_DebugSettings = DebugMenuSettings.GetAsset();
		}

		private void LateUpdate() {
			bool isMenuDisplayed = m_CurrentMenuState != null;
			bool isInfoRefreshFrame = false;

			if (isMenuDisplayed || m_IsPinboardActive) {
				m_RefreshFramesToWait--;
				if (m_RefreshFramesToWait < 0) {
					isInfoRefreshFrame = true;
					m_RefreshFramesToWait = m_DebugSettings.InfoRefreshSkipFrame;
				}
			}

			int infosToRefresh = m_DebugSettings.RefreshedInfosPerFrames;

			if (isMenuDisplayed) {
				if (isInfoRefreshFrame) {
					if (infosToRefresh > 0) {
						int menuInfosToRefresh = infosToRefresh;
						int outOfBoundsInfosCount =
							m_CurrentRefreshedInfo + menuInfosToRefresh - m_DrawnInformationsCount;
						bool infosOutOfBounds = outOfBoundsInfosCount >= 0;
						if (infosOutOfBounds) {
							menuInfosToRefresh = menuInfosToRefresh - outOfBoundsInfosCount;
						}

						for (int i = 0; i < menuInfosToRefresh; i++) {
							m_InfoButtonsPool[i + m_CurrentRefreshedInfo].Refresh();
						}

						if (infosOutOfBounds) {
							m_CurrentRefreshedInfo = 0;
						} else {
							m_CurrentRefreshedInfo += menuInfosToRefresh;
						}
					} else {
						for (int i = 0; i < m_DrawnInformationsCount; i++) {
							m_InfoButtonsPool[i].Refresh();
						}
					}
				}

				float currentDisplayWith = GetDisplayWidth();
				if (m_LastDisplayWidth != currentDisplayWith) {
					RefreshPathDisplay();
					m_LastDisplayWidth = currentDisplayWith;
				}
			}

			if (m_IsPinboardActive) {
				if (isInfoRefreshFrame) {
					if (infosToRefresh > 0) {
						int outOfBoundsInfosCount =
							m_CurrentRefreshedPinnedInfos + infosToRefresh - m_PinnedInfos.Count;
						bool infosOutOfBounds = outOfBoundsInfosCount >= 0;
						if (infosOutOfBounds) {
							infosToRefresh = infosToRefresh - outOfBoundsInfosCount;
						}

						for (int i = 0; i < infosToRefresh; i++) {
							m_PinnedInfos[i + m_CurrentRefreshedPinnedInfos].Refresh();
						}

						if (infosOutOfBounds) {
							m_CurrentRefreshedPinnedInfos = 0;
						} else {
							m_CurrentRefreshedPinnedInfos += infosToRefresh;
						}
					} else {
						for (int i = 0; i < m_PinnedInfos.Count; i++) {
							m_PinnedInfos[i].Refresh();
						}
					}
				}
			}
		}

		private void OnDestroy() {
			if (m_Instance == this) {
				m_Instance = null;
				m_HasInstance = false;
			}
		}
		#endregion


		#region Debug Menu Draw
		internal void OpenDebugMenu() {
			if (!isMenuActive) {
				m_CurrentMenuState =
					ApplicationStack.AddStateWithSetting<DebugMenuState, DebugMenuStateSetting>(m_Setting);
				if (!m_DrawnOnce) {
					DrawCategory(DebugMenu.m_RootCategory);
					m_DrawnOnce = true;
				}
			} else {
				Debug.LogError("[Debug Menu] Debug menu is already displayed");
			}
		}

		internal void CloseDebugMenu() {
			if (isMenuActive) {
				m_CurrentMenuState.EndState();
			} else {
				Debug.LogError("[Debug Menu] Debug menu is already hidden");
			}
		}

		internal void DrawCategory(DebugCategory category, bool forceRefresh = false) {
			if (!forceRefresh && m_CurrentDrawnCategory == category) {
				return;
			}

			m_CurrentRefreshedInfo = 0;

			Profiler.BeginSample("Debug Menu Draw Category");


			#region Actions And Submenus
			//----------------------------------------------------------------------------
			//						   Draw Actions and Submenus
			//----------------------------------------------------------------------------

			bool hasParent = category.m_ParentCategory != null;
			//Draw parent browser button
			if (hasParent) {
				if (!m_BrowseToParentAction.gameObject.activeSelf) {
					m_BrowseToParentAction.gameObject.SetActive(true);
				}

				m_BrowseToParentAction.SetAction("../", category.m_ParentCategory.m_DrawSelfHandle, true);
			} else {
				if (m_BrowseToParentAction.gameObject.activeSelf) {
					m_BrowseToParentAction.gameObject.SetActive(false);
				}
			}

			int currentButtonId = 0;

			DebugActionButton GetButton(int i) {
				if (i >= m_ActionButtonsPool.Count) {
					m_ActionButtonsPool.Add(Instantiate(m_ActionButtonPrefab, m_ActionButtonsContainer));
				} else {
					m_ActionButtonsPool[i].gameObject.SetActive(true);
				}

				return m_ActionButtonsPool[i];
			}

			//-------Draw subcategories buttons-----
			//
			var subCategories = category.m_SubCategories;
			for (int i = 0; i < subCategories.Count; i++) {
				var btn = GetButton(currentButtonId);
				btn.SetAction(subCategories[i].m_DrawSelfHandle, true);
				currentButtonId++;
			}

			//----------Manage separator-----------
			//
			m_ActionsSeparator.SetSiblingIndex(currentButtonId + 1);

			//----------Draw action buttons----------
			//
			var actions = category.m_Actions;
			for (int i = 0; i < actions.Count; i++) {
				var btn = GetButton(currentButtonId);
				btn.SetAction(actions[i], false);
				currentButtonId++;
			}

			//Hide remaining buttons
			for (int i = currentButtonId; i < m_ActionButtonsPool.Count; i++) {
				m_ActionButtonsPool[i].gameObject.SetActive(false);
			}
			#endregion


			#region Infos
			//----------------------------------------------------------------------------
			//								  Draw infos
			//----------------------------------------------------------------------------

			int currentInfoId = 0;
			// category

			DebugInfoButton GetInfoButton(int index) {
				if (index >= m_InfoButtonsPool.Count) {
					m_InfoButtonsPool.Add(Instantiate(m_InfoButtonPrefab, m_DebugInfosContainer));
				} else {
					m_InfoButtonsPool[index].gameObject.SetActive(true);
				}

				return m_InfoButtonsPool[index];
			}

			for (int i = 0; i < category.m_Infos.Count; i++) {
				var info = GetInfoButton(i);
				info.Setup(category.m_Infos[i]);
				info.Refresh();
				currentInfoId++;
			}

			for (int i = currentInfoId; i < m_InfoButtonsPool.Count; i++) {
				m_InfoButtonsPool[i].gameObject.SetActive(false);
			}

			m_DrawnInformationsCount = currentInfoId;
			#endregion


			m_CurrentDrawnCategory = category;
			RefreshPathDisplay();
			Profiler.EndSample();
		}

		private void RefreshPathDisplay() {
			int targetDepth = m_CurrentDrawnCategory.m_CategoryDepth + 1;
			for (int i = m_PrevDrawnCategories; i < targetDepth; i++) {
				if (i >= m_PathButtonsPool.Count) {
					m_PathButtonsPool.Add(Instantiate(m_PathButton, m_PathButtonsContainer));
				} else {
					m_PathButtonsPool[i].gameObject.SetActive(true);
				}
			}

			for (int i = targetDepth; i < m_PathButtonsPool.Count; i++) {
				m_PathButtonsPool[i].gameObject.SetActive(false);
			}

			var currentInitCategory = m_CurrentDrawnCategory;

			float maximumWidth = m_PathButtonsContainer.rect.width;
			float currentWidth = 0f;

			void SetupCategoryButton(bool last) {
				var button = m_PathButtonsPool[currentInitCategory.m_CategoryDepth];
				button.SetCategory(currentInitCategory);
				button.DrawLabel(true, last);
				currentWidth += button.m_Text.preferredWidth;
				currentInitCategory = currentInitCategory.m_ParentCategory;
			}

			SetupCategoryButton(true);

			while (currentInitCategory != null) {
				SetupCategoryButton(false);
			}

			if (currentWidth > maximumWidth) {
				int currentText = 0;
				bool overflowing = true;

				while (currentText < targetDepth - 1 && overflowing) {
					var button = m_PathButtonsPool[currentText];
					float initialWidth = button.m_Text.preferredWidth;
					button.DrawLabel(false, false);
					initialWidth -= button.m_Text.preferredWidth;
					currentWidth -= initialWidth;
					currentText++;
					if (currentWidth <= maximumWidth) {
						overflowing = false;
					}
				}

				if (overflowing) {
					m_PathButtonsPool[currentText].DrawLabel(false, true);
				}

				m_PrevDrawnCategories = 0; //Will must refresh the whole path
			} else {
				//Allow refreshing only required paths instead of each one
				m_PrevDrawnCategories = targetDepth;
			}
		}

		internal void ShowUI() {
			m_MenuGroup.SetActive(true);
		}

		internal void HideUI() {
			m_MenuGroup.SetActive(false);
		}

		internal void BrowseToParent() {
			if (m_CurrentDrawnCategory.m_CategoryDepth > 0) {
				m_CurrentDrawnCategory.m_ParentCategory.Draw();
			} else {
				CloseDebugMenu();
			}
		}
		#endregion


		#region State Management
		internal void DisposeState() {
			m_CurrentMenuState = null;
		}
		#endregion


		#region Pinboard
		internal void PinInfo(DebugInfoHandle debugInfoHandle) {
			m_CurrentRefreshedPinnedInfos = 0;

			DebugInfoUI infoUI = null;

			if (m_PinnedInfos.Count == 0) {
				SetPinboardVisibility(true, true);
			}

			if (m_PinnedInfos.Count + 1 >= m_PinboardInfosPool.Count) {
				infoUI = Instantiate(m_InfoUIPrefab, m_PinnednInfoContainer);
				m_PinboardInfosPool.Add(infoUI);
			} else {
				infoUI = m_PinboardInfosPool[m_PinnedInfos.Count];
				infoUI.gameObject.SetActive(true);
			}

			infoUI.Setup(debugInfoHandle);
			infoUI.Refresh();
			m_PinnedInfos.Add(infoUI);
			debugInfoHandle.m_IsPinned = true;
		}


		internal void UnpinInfo(DebugInfoHandle debugInfoHandle) {
			m_CurrentRefreshedPinnedInfos = 0;

			for (int i = 0; i < m_PinnedInfos.Count; i++) {
				var info = m_PinnedInfos[i];
				if (info.targetHandle == debugInfoHandle) {
					debugInfoHandle.m_IsPinned = false;
					int lastId = m_PinnedInfos.Count - 1;

					for (int j = i; j < lastId; j++) {
						m_PinnedInfos[j].Setup(m_PinnedInfos[j + 1].targetHandle);
					}

					var last = m_PinnedInfos[lastId];
					last.gameObject.SetActive(false);
					last.Dispose();

					m_PinnedInfos.RemoveAt(lastId);

					if (m_PinnedInfos.Count == 0) {
						SetPinboardVisibility(false, false);
					}

					return;
				}
			}
		}

		private void SetPinboardVisibility(bool visible, bool hasContent) {
			if ((!m_IsPinboardActive && visible) || (m_IsPinboardActive && !visible)) {
				m_PinboardGroup.SetActive(visible);
				m_IsPinboardActive = visible;
				m_VisbilityButtonImage.sprite = visible ? m_VisibleIcon : m_HiddenIcon;
				if (visible) {
					for (int i = 0; i < m_PinnedInfos.Count; i++) {
						m_PinnedInfos[i].Refresh();
					}
				}
			}

			if ((!m_ArePinboardUtilsActive && hasContent) || (m_ArePinboardUtilsActive && !hasContent)) {
				m_ClearPinBoardButton.gameObject.SetActive(hasContent);
				m_TogglePinboardVisibilityButton.gameObject.SetActive(hasContent);
				m_ArePinboardUtilsActive = hasContent;
			}
		}

		internal void TogglePinboardVisbility() {
			SetPinboardVisibility(!m_IsPinboardActive, true);
		}

		internal void ClearPinboard() {
			m_CurrentRefreshedPinnedInfos = 0;

			for (int i = 0; i < m_DrawnInformationsCount; i++) {
				var btn = m_InfoButtonsPool[i];
				if (btn.targetHandle.m_IsPinned) {
					btn.ResetLabel();
				}
			}

			for (int i = 0; i < m_PinnedInfos.Count; i++) {
				var info = m_PinnedInfos[i];
				info.targetHandle.m_IsPinned = false;
				info.Dispose();
				info.gameObject.SetActive(false);
			}

			m_PinnedInfos.Clear();

			for (int i = 0; i < m_DrawnInformationsCount; i++) {
				m_InfoButtonsPool[i].Refresh();
			}

			SetPinboardVisibility(false, false);
		}
		#endregion


		#region Navigation
		private void BuildNavigation() {
			//Utility buttons

			//Paths

			//Actions and infos
		}
		#endregion


		#region Utils
		private float GetDisplayWidth() {
			return Display.displays[m_Canvas.targetDisplay].systemWidth;
		}
		#endregion
	}
}
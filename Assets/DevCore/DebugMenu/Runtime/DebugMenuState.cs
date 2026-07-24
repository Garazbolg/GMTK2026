using DevCore.ApplicationStates;

namespace DevCore.DebugMenu {
    public sealed class DebugMenuState : ApplicationStateWithSetting<DebugMenuStateSetting> {
        #region Properties
        public override StateCategory category => StateCategory.Debug;
        #endregion


        #region Callbacks
        protected override void OnApplyOverrides(ref ApplicationStateOverrides overrides) {
            overrides.canPause = false;
            overrides.timeScale = 0f;
            overrides.isCursorVisible = true;
            overrides.displayInGameUI = false;
            overrides.playerHasControl = false;
        }

        protected override void OnStartOrResume() {
            settings.m_DebugMenuController.ShowUI();
        }

        protected override void OnStatePaused() {
            settings.m_DebugMenuController.HideUI();
        }

        protected override void OnStateEnded() {
            var controller = settings.m_DebugMenuController;
            controller.HideUI();
            controller.DisposeState();
        }
        #endregion
    }

    [System.Serializable]
    public class DebugMenuStateSetting : ApplicationStateSetting {
        internal DebugMenuController m_DebugMenuController = null;

        public DebugMenuStateSetting() { }

        internal DebugMenuStateSetting(DebugMenuController controller) {
            m_DebugMenuController = controller;
        }
    }
}
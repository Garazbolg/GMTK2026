using UnityEngine;

namespace DevCore.DebugMenu {
    public sealed class DebugMenuInputModule : DebugMenuInputModuleBase {
        #region Settings
        [SerializeField] private string m_ToggleDebugMenuInput = string.Empty;
        [SerializeField] private string m_ToggleDebugMenuAdditionalInput = string.Empty;
        [SerializeField] private string m_GoToPreviousTabInput = string.Empty;
        #endregion


        #region Listen Inputs
        protected override bool IsTogglingDebugMenu() {
            bool isToggling = Input.GetButtonDown(m_ToggleDebugMenuInput);

            if (isToggling && !string.IsNullOrEmpty(m_ToggleDebugMenuAdditionalInput)) {
                return isToggling && Input.GetButton(m_ToggleDebugMenuAdditionalInput);
            }

            return isToggling;
        }

        protected override bool IsRequestingPreviousTab() {
            return Input.GetButtonDown(m_GoToPreviousTabInput);
        }
        #endregion
    }
}
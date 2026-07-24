using System;
using UnityEngine;
using UnityEngine.UI;

namespace DevCore.DebugMenu {
    public class DebugInfoButton : DebugInfoUI {
        [SerializeField] private DebugButton m_Button = null;
        
        private void Awake() {
            m_Button.SetAction(TogglePinInfo);   
        }

        public void TogglePinInfo() {
            m_TargetHandle.ToggleInfoPin();
            ResetLabel();
            if (m_TargetHandle.isPinned) {
                m_Label = $"<color=#a5ffae>[Pinned]</color> {m_Label}";
            }
            Refresh();
        }
    }
}
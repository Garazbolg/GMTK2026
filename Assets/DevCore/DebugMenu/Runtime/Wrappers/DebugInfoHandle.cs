using System;
using UnityEngine;

namespace DevCore.DebugMenu
{
    public class DebugInfoHandle : DebugHandle {
        #region Currents
        private DebugInfoAction m_Action;
        internal bool m_IsPinned = false;
        #endregion
        

        #region Properties
        /// <summary> Is the information displayed on the Pin Board </summary>
        public bool isPinned => m_IsPinned;
        #endregion

        
        #region Construction
        internal DebugInfoHandle(string name, DebugInfoAction action, DebugCategory owner) : base(name, owner) {
            m_Action = action;
        }
        #endregion


        #region Info
        public string GetInfo() {
            try {
                return m_Action?.Invoke();
            }
            catch  {
                Debug.LogWarning($"[Debug Menu] Infoo with name {m_Name} cannot be found, this one is removed from the menu");
                Unregister();
                return "";
            }
        }        
        #endregion
        
        #region Pin
        public void PinInfo() {
            if (!m_IsPinned) {
                DebugMenuController.m_Instance.PinInfo(this);
            } else {
                Debug.LogError($"[Debug Menu] Info {m_Name} is already on the Pin Board");
            }
        }

        public void ToggleInfoPin() {
            if (!m_IsPinned) {
                DebugMenuController.m_Instance.PinInfo(this);
            } else {
                DebugMenuController.m_Instance.UnpinInfo(this);
            }
        }
        
        public void UnpinInfo() {
            if (m_IsPinned) {
                DebugMenuController.m_Instance.UnpinInfo(this);
            } else {
                Debug.LogError($"[Debug Menu] Info {m_Name} is currently not on the pinbooard");
            }
        }
        #endregion

        
        #region Registration
        public override void Unregister() {
            if (m_IsPinned) {
                UnpinInfo();
            }
            m_OwnerCategory.UnregisterInfo(this);
        }
        #endregion
    }
}

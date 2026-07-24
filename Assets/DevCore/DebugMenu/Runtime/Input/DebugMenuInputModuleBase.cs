using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DevCore.DebugMenu {
    [RequireComponent(typeof(DebugMenuController))]
    public abstract class DebugMenuInputModuleBase : MonoBehaviour {
        #region Settings
        [Header("Input Behaviour")]
        
        #pragma warning disable CS0414
        [SerializeField, Min(0f)] private float m_selectionScrollDelay = 0.4f; 
        [SerializeField, Min(0f)] private float m_selectionScrollFrequency = 0.05f;
        #pragma warning restore CS0414
        
        #endregion
        
        #region Currents
        private DebugMenuController m_Controller = null;
        #endregion
        
        #region Callbacks
        private void Awake() {
            m_Controller = GetComponent<DebugMenuController>();
        }

        private void Update() {
            if (IsTogglingDebugMenu()) {
                DebugMenu.ToggleMenu();
            }

            if (IsRequestingPreviousTab()) {
                m_Controller.BrowseToParent();
            }
            
            //TODO : Implement gamepad navigation
            // var evt = EventSystem.current; 
            // if (evt != null) {
            //     var inputModule = evt.currentInputModule;
            //     
            //     BaseInput input;
            //     if (inputModule.inputOverride != null) {
            //         input = inputModule.inputOverride;
            //     } else {
            //         input = inputModule.input;
            //     }
            //
            //     Vector2 direction = new Vector2(
            //         input.GetAxisRaw("Horizontal"),
            //         input.GetAxisRaw("Vertical"));
            //
            //     if (!Mathf.Approximately(direction.x, 0f) || !Mathf.Approximately(direction.y, 0f)) {
            //         
            //     }
            // }
        }
        #endregion
        
        #region Listen Inputs
        /// <summary>
        /// Return true if the Debug Menu toggle input is pressed
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsTogglingDebugMenu();

        /// <summary>
        /// Return true if the Debug Menu previous tab is pressed
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsRequestingPreviousTab();
        #endregion
    }
}
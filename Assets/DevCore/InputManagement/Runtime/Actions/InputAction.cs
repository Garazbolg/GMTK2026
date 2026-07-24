using System;
using System.Collections;
using System.Collections.Generic;
using DevCore.Core;
using DevCore.Localization;
using UnityEngine;

namespace DevCore.InputManagement
{
    public abstract class InputAction : ScriptableObject {
        #region Data
        [SerializeField] private LocalizableText m_ActionDisplayName = new LocalizableText();
        [SerializeField] private string m_ActionPath = string.Empty;

        [Space]
        [SerializeField] internal bool m_IsRebindable = true;
        #endregion


        #region Properties
        
        #endregion

        
        #region Input State
        public abstract bool IsStarted();
        public abstract bool IsPerformed();
        public abstract bool IsReleased();
        public abstract bool IsHolding();

        public abstract void RegisterStartAction(Action callback);
        public abstract void RegisterPerformAction(Action callback);
        public abstract void RegisterReleaseAction(Action callback);
        public abstract void RegisterHoldAction(Action callback);
        
        public abstract void UnregisterStartAction(Action callback);
        public abstract void UnregisterPerformAction(Action callback);
        public abstract void UnregisterReleaseAction(Action callback);
        public abstract void UnregisterHoldAction(Action callback);
        #endregion
    }
}

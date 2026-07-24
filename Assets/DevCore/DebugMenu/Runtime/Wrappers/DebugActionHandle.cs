using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.DebugMenu {
    public class DebugActionHandle : DebugHandle{
        private DebugAction m_Action;

        internal DebugActionHandle(string name, DebugAction mAction, DebugCategory owner) : base(name, owner) {
            this.m_Action = mAction;
        }

        public void TriggerAction() {
            try {
                m_Action.Invoke();
            }
            catch {
                Debug.LogWarning($"[Debug Menu] Action with name {m_Name} cannot be executed, this one is removed from the menu");
                Unregister();
            }
        }
        
        public override void Unregister() {
            m_OwnerCategory.UnregisterAction(this);
        }
    }
}
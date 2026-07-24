using System.Collections;
using System.Collections.Generic;
using DevCore.Core;
using UnityEngine;

namespace DevCore.PrototypingToolkit
{
    public abstract class GameplayActionDelayedComponent : GameplayActionComponent {
        [SerializeField, Min(0f)] private float m_Delay = 0f;
        
        protected sealed override void Execute(GameObject gameObject) {
            OnBeforeDelayStart(gameObject);

            if (m_Delay <= 0f)  {
                OnDelayEllapsed(gameObject, CooldownResult.skipped);
            } else {
                Cooldown.Launch(m_Delay, (result) => OnDelayEllapsed(gameObject, result));
            }
        }
        
        protected virtual void OnBeforeDelayStart(GameObject gameObject){}
        protected abstract void OnDelayEllapsed(GameObject gameObject, CooldownResult cooldownResult);
    }
}

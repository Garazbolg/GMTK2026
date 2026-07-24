using System.Runtime.Serialization;
using DevCore.Core;
using UnityEngine;

namespace DevCore.PrototypingToolkit
{
    [AddComponentMenu("Sub Action")]
    public class SubAction : GameplayActionDelayedComponent {
        [SerializeField] private GameplayAction m_SubAction = null;

        protected override void OnDelayEllapsed(GameObject gameObject, CooldownResult cooldownResult) {
            m_SubAction.Execute(gameObject);
        }
    }
}

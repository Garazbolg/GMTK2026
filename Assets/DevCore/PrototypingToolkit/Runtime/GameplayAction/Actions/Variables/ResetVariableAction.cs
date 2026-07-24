using DevCore.ScriptableVariables;
using UnityEngine;

namespace DevCore.PrototypingToolkit
{
    [AddComponentMenu("Variable/Reset Variable")]
    public class ResetVariableAction : GameplayActionComponent {
        [SerializeField] private ScriptableVariableBase m_Variable = null;
        protected override void Execute(GameObject gameObject) {
            m_Variable.ResetValue();
        }
    }
}

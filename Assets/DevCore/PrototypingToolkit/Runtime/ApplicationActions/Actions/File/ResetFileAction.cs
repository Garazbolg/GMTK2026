using DevCore.ScriptableVariables;
using UnityEngine;

namespace DevCore.PrototypingToolkit
{
    [AddComponentMenu("File/Reset")]
    public class ResetFileAction : ApplicationActionComponent {
        [SerializeField] private ScriptableVariableSaveWrapper m_SaveWrapper = null;
        [SerializeField] private bool m_SaveOnReset = true;
        
        protected override void Execute() {
            m_SaveWrapper.ResetFile(m_SaveOnReset);
        }
    }
}

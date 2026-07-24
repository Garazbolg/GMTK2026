using DevCore.ScriptableVariables;
using UnityEngine;

namespace DevCore.PrototypingToolkit
{
    [AddComponentMenu("File/Load")]
    public class LoadFileAction : ApplicationActionComponent {
        [SerializeField] private ScriptableVariableSaveWrapper m_SaveWrapper = null;
        protected override void Execute() {
            m_SaveWrapper.Load();
        }
    }
}

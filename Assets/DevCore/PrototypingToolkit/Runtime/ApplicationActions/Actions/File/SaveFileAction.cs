using System.Collections;
using System.Collections.Generic;
using DevCore.ScriptableVariables;
using UnityEngine;

namespace DevCore.PrototypingToolkit
{
    [AddComponentMenu("File/Save")]
    public class SaveFileAction : ApplicationActionComponent {
        [SerializeField] private ScriptableVariableSaveWrapper m_SaveWrapper = null;
        protected override void Execute() {
            m_SaveWrapper.Save();
        }
    }
}

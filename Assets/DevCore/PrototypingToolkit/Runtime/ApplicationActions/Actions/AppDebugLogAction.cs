using UnityEngine;

namespace DevCore.PrototypingToolkit
{
    [AddComponentMenu("Debug/Log")]
    public class AppDebugLogAction : ApplicationActionComponent {
        private enum LogType {
            Message,
            Warning,
            Error
        }

        [SerializeField] private LogType m_LogType = LogType.Message; 
        [SerializeField, TextArea] private string m_LogText = string.Empty; 

        protected override void Execute() {
            switch (m_LogType) {
                case LogType.Error:
                    Debug.LogError(m_LogText, this);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(m_LogText, this);
                    break;
                default:
                    Debug.Log(m_LogText, this);
                    break;
            }
        }
    }
}

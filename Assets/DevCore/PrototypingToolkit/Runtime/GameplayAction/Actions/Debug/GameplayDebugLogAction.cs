using UnityEngine;

namespace DevCore.PrototypingToolkit
{
    [AddComponentMenu("Debug/Log Message")]
    public class GameplayDebugLogAction : GameplayActionComponent
    {
        private enum ContextObject {
            TargetGameObject,
            Action
        }
        
        [SerializeField, TextArea] private string m_LogText = string.Empty;
        
        [Space]
        [SerializeField] private ContextObject m_Context = ContextObject.Action;
        [SerializeField] private bool m_AppendContextObjectName = true;
        
        protected override void Execute(GameObject gameObject) {
            Object context = m_Context == ContextObject.Action ? this : gameObject;
            string message = m_LogText;
            
            if (m_AppendContextObjectName) {
                message = $"[{context.name}] {message}";
            }
            
            Debug.Log(message, context);
        }
    }
}

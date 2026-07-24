using DevCore.ScriptableVariables;
using UnityEngine;
using UnityEngine.Serialization;

namespace DevCore.PrototypingToolkit
{
    internal static class NOAPath {
        internal const string k_Path = "Variable/Operation/";
    }
    
    /// <summary>
    /// Make a simple operation between a <see cref="ScriptableNumeric{T}"/> and a <see cref="BindableValue{T}"/>  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NumericOperationAction<T> : GameplayActionComponent {
        [SerializeField] private ScriptableNumeric<T> m_Variable = null;
        [SerializeField] private OperatorType m_Operator = OperatorType.Add;
        [SerializeField] private BindableValue<T> m_Operand = default; 
        
        public enum OperatorType {
            Set,
            Add,
            Subtract,
            Multiply,
            Divide,
        }
        
        protected override void Execute(GameObject gameObject) {
            switch (m_Operator) {
                case OperatorType.Add:
                    m_Variable.Add(m_Operand.value);
                    break;
                case OperatorType.Subtract:
                    m_Variable.Subtract(m_Operand.value);
                    break;
                case OperatorType.Multiply:
                    m_Variable.Multiply(m_Operand.value);
                    break;
                case OperatorType.Divide:
                    m_Variable.Divide(m_Operand.value);
                    break;
                case OperatorType.Set:
                    m_Variable.value = m_Operand.value;
                    break;
            }
        }
    }
}

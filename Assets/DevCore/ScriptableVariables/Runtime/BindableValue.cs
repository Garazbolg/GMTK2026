using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.ScriptableVariables
{
    /// <summary>
    /// Can either be a local value or point to a variable value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public struct BindableValue<T> {
        [SerializeField] private T m_Value;
        [SerializeField] private ScriptableVariable<T> m_Variable;
        
        public T value {
            get {
                if (ReferenceEquals(m_Variable, null)) {
                    return m_Value;
                } else {
                    return m_Variable.value;
                }
            }

            set {
                if (ReferenceEquals(m_Variable, null)) {
                    m_Value = value;
                } else {
                    m_Variable.value = value;
                }
            }
        }
    }
}

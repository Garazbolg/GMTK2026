using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.ScriptableVariables
{
    /// <summary>
    /// Define numeric type and implements base math operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ScriptableNumeric<T> : ScriptableVariable<T> {
        public abstract void Add(T value);
        public abstract void Subtract(T value);
        public abstract void Multiply(T value);
        public abstract void Divide(T value);
        
        public void Add(ScriptableNumeric<T> variable) {
            Add(variable.value);
        }
        
        public void Subtract(ScriptableNumeric<T> variable) {
            Subtract(variable.value);
        }
        
        public void Multiply(ScriptableNumeric<T> variable) {
            Multiply(variable.value);
        }
        
        public void Divide(ScriptableNumeric<T> variable) {
            Divide(variable.value);
        }
    }
}

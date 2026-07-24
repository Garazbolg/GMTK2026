using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.ScriptableVariables
{
    public interface IValueWrapper {
        string wrapperName { get; }
        object wrappedValue { get; set; }
    }
}

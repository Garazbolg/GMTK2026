using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.PrototypingToolkit {
    public abstract class ConditionHandler : MonoBehaviour {
        public abstract bool IsValid();
    }
}
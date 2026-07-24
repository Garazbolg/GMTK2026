using System.Collections;
using System.Collections.Generic;
using DevCore.Core;
using UnityEngine;

namespace DevCore.PrototypingToolkit
{
    public abstract class ApplicationActionComponent : AssetComponent
    {
        internal void ExecuteInternal() {
            if (active) {
                Execute();
            }
        }

        protected abstract void Execute();
    }
}

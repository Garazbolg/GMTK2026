using UnityEngine;

namespace DevCore.PrototypingToolkit {
    public class OrCondition : ConditionHandler {
        public ConditionHandler[] _conditions = { };

        public override bool IsValid() {
            bool valid = false;

            for (int i = 0; i < _conditions.Length; i++) {
                valid |= _conditions[i].IsValid();
            }

            return valid;
        }
    }
}
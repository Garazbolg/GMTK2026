using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.FeedbackEngine
{
    public abstract class AnimatorSetParameterFeedback : AnimatorFeedback {
        [SerializeField] protected string m_ParameterName = string.Empty;

        protected override string animatorPropertyId => m_ParameterName;
    }
}

using System;
using DevCore.Core;
using UnityEngine;
using DG.Tweening;

namespace DevCore.FeedbackEngine
{
    /// <summary>
    /// Abstract class for punch feedback on transform
    /// </summary>
    [AddComponentMenu("Tweening/Punch")]
    public class TransformPunchTween : TransformTweenFeedback
    {
        #region Settings
        #pragma warning disable CS0414
        [SerializeField] private Vector3 m_Punch = Vector3.one;
        [SerializeField] private int m_Vibrato = 10;
        [SerializeField] private float m_Elasticity = 1f;
        
        [Space]
        [SerializeField] private TransformCoordinate m_Coordinate = TransformCoordinate.Position;
        #endregion


        #region Properties
        protected override TransformCoordinate m_TargetCoordinate => m_Coordinate;
        #endregion

        
        protected override Tween GetTween(Transform targetTransform) {
            switch (m_Coordinate) {
                case TransformCoordinate.Position:
                    return targetTransform.DOPunchPosition(m_Punch, m_Duration, m_Vibrato, m_Elasticity);
                case TransformCoordinate.Rotation:
                    return targetTransform.DOPunchRotation(m_Punch, m_Duration, m_Vibrato, m_Elasticity);
                case TransformCoordinate.Scale:
                    return targetTransform.DOPunchScale(m_Punch, m_Duration, m_Vibrato, m_Elasticity);
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}

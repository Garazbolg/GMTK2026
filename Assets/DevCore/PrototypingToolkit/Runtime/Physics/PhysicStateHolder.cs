using System;
using System.Collections.Generic;
using DevCore.Core;
using DevCore.FeedbackEngine;
using UnityEngine;

namespace DevCore.PrototypingToolkit {
    [SelectionBase]
    [AddComponentMenu("Prototyping Toolkit/Physics/Physics State Holder")]
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PhysicStateHolder : MonoBehaviour {
        #region Settings
        [SerializeField] private PhysicState m_DefaultState = new PhysicState();
        
        [Header("References")]
        internal Rigidbody m_TargetRigidbody;
        internal Transform m_RootTransform;
        internal Collider[] m_Colliders;
        #endregion


        #region Currents
        private List<PhysicState> m_StatesStack = new List<PhysicState>();
        private PhysicState m_ActiveState = null;
        #endregion

        
        #region Callbacks
        private void Awake() {
            AddStateOnStack(m_DefaultState);
        }

        private void FixedUpdate() {
            m_ActiveState.ResolvePhysics();
        }
        #endregion


        #region Stack
        public void AddStateOnStack(PhysicState behaviourState) {
            if (m_StatesStack.Contains(behaviourState)) {
                return;
            }

            behaviourState.m_Wrapper = this;

            if (m_StatesStack.Count <= 0) {
                m_StatesStack.Add(behaviourState);
                ActivateState(behaviourState);
                return;
            }

            for (int i = 0; i < m_StatesStack.Count; i++) {
                var state = m_StatesStack[i];
                if (behaviourState.priority >= state.priority) {
                    if (i == 0) {
                        ActivateState(behaviourState);
                        state.DeactivateState();
                    }

                    m_StatesStack.Insert(i, behaviourState);
                    return;
                }
            }

            m_StatesStack.Add(behaviourState);
        }

        public void RemoveStateOnStack(PhysicState state) {
            if (m_StatesStack.Contains(state)) {
                m_StatesStack.Remove(state);
                state.DeactivateState();
                state.m_Wrapper = null;
            }

            if (m_StatesStack.Count > 0) {
                if (!m_StatesStack[0].isActive) {
                    ActivateState(m_StatesStack[0]);
                }
            }
        }

        private void ActivateState(PhysicState state) {
            state.ActivateState();
            m_ActiveState = state;
        }
        #endregion
        
        
        #region Physic Callbacks
        private void OnCollisionEnter(Collision other) {
            for (int i = 0; i < m_StatesStack.Count; i++) {
                m_StatesStack[i].CollisionEnter(other);
            }
        }

        private void OnCollisionExit(Collision other) {
            for (int i = 0; i < m_StatesStack.Count; i++) {
                m_StatesStack[i].CollisionExit(other);
            }
        }

        private void OnCollisionStay(Collision other) {
            for (int i = 0; i < m_StatesStack.Count; i++) {
                m_StatesStack[i].CollisionStay(other);
            }
        }

        private void OnTriggerEnter(Collider other) {
            for (int i = 0; i < m_StatesStack.Count; i++) {
                m_StatesStack[i].TriggerEnter(other);
            }
        }

        private void OnTriggerExit(Collider other) {
            for (int i = 0; i < m_StatesStack.Count; i++) {
                m_StatesStack[i].TriggerExit(other);
            }
        }

        private void OnTriggerStay(Collider other) {
            for (int i = 0; i < m_StatesStack.Count; i++) {
                m_StatesStack[i].TriggerStay(other);
            }
        }
        #endregion


        #region Components
        private void CacheComponents() {
            m_TargetRigidbody = GetComponent<Rigidbody>();
            m_RootTransform = m_TargetRigidbody.transform;
            m_Colliders = GetComponentsInChildren<Collider>();
        }
        
        private void SetRigidbodySettings(PhysicState state) {
            m_TargetRigidbody.mass = state.mass;
            m_TargetRigidbody.linearDamping = state.drag;
            m_TargetRigidbody.angularDamping = state.angularDrag;
            m_TargetRigidbody.useGravity = state.useGravity;
            m_TargetRigidbody.constraints = state.constraints;
            m_TargetRigidbody.isKinematic = state.isKinematic;
        }
        
        private void SetPhysicMaterial(PhysicsMaterial physicMaterial) {
            for (int i = 0; i < m_Colliders.Length; i++) {
                m_Colliders[i].sharedMaterial = physicMaterial;
            }
        }
        
        internal void SetupStateSettings(PhysicState state) {
            SetRigidbodySettings(state);
            SetPhysicMaterial(state.physicMaterial);
        }
        #endregion

        
        #region Editor
        private void OnValidate() {
            CacheComponents();
            SetupStateSettings(m_DefaultState);
        }
        #endregion
    }
}
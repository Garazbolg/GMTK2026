using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.PrototypingToolkit {
	/// <summary>
	/// Defines a rigidbody physic state
	/// </summary>
	[Serializable]
	public sealed class PhysicState {
		#region Settings
		public uint priority = 0;

		[Header("Rigidbody Setting")]
		public float mass = 1f;
		public float drag = 1f;
		public float angularDrag = 1f;
		public bool useGravity = false;
		public bool isKinematic = false;
		public RigidbodyConstraints constraints;

		[Space]
		public Vector3 velocityMultiplier = Vector3.one;
		public Vector3 angularVelocityMultiplier = Vector3.one;

		[Header("Collision Setting")]
		public PhysicsMaterial physicMaterial;

		[Header("Actions")]
		public GameplayAction stateStartAction = null;
		public GameplayAction stateEndAction = null;
		#endregion


		#region Currents
		private Vector3 m_Movement = Vector3.zero;
		private bool m_IsActive = false;
		internal PhysicStateHolder m_Wrapper = null;
		internal IPhysicStateBehaviour m_AttachedBehaviour;
		#endregion


		#region Properties
		public bool isActive => m_IsActive;
		#endregion


		#region Events
		public delegate void TriggerAction(Collider collider, bool isActive);

		public delegate void CollisionAction(Collision collision, bool isActive);

		public event Action<bool> onSetActiveState;
		#endregion


		#region Movements
		public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Force) {
			if (!m_IsActive) return;
			m_Wrapper.m_TargetRigidbody.AddForce(force, forceMode);
		}

		public void SetAngularVelocity(Vector3 angularVelocity) {
			if (!m_IsActive) return;
			m_Wrapper.m_TargetRigidbody.angularVelocity = angularVelocity;
		}

		public void SetMovement(Vector3 movement) {
			if (!m_IsActive) return;
			m_Movement = movement;
		}

		public void AddMovement(Vector3 movement) {
			if (!m_IsActive) return;
			m_Movement += movement;
		}

		public void SetPosition(Vector3 position) {
			if (!m_IsActive) return;
			m_Wrapper.m_TargetRigidbody.MovePosition(position);
		}

		public void SetVelocity(Vector3 velocity) {
			if (!m_IsActive) return;
			m_Wrapper.m_TargetRigidbody.linearVelocity = velocity;
		}

		internal void ResolvePhysics() {
			if (!m_IsActive) {
				return;
			}
			
			m_AttachedBehaviour?.OnBeforePhysicsResolved();
			
			var rb = m_Wrapper.m_TargetRigidbody;

			if (!isKinematic) {
				var velocity = rb.linearVelocity;
				velocity.Scale(velocityMultiplier);
				rb.linearVelocity = velocity;

				var angularVelocity = rb.angularVelocity;
				rb.angularVelocity.Scale(angularVelocityMultiplier);
				rb.angularVelocity = angularVelocity;
			}

			if (m_Movement != Vector3.zero) {
				var position = m_Wrapper.m_RootTransform.position;
				SetPosition(position + m_Movement);
				m_Movement = Vector3.zero;
			}
				
			m_AttachedBehaviour?.OnAfterPhysicsResolved();
		}

		internal void ActivateState() {
			m_IsActive = true;
			m_Wrapper.SetupStateSettings(this);
			stateStartAction?.Execute(m_Wrapper.m_RootTransform.gameObject);
			onSetActiveState?.Invoke(true);
		}

		internal void DeactivateState() {
			m_Movement = Vector3.zero;
			m_IsActive = false;
			stateEndAction?.Execute(m_Wrapper.m_RootTransform.gameObject);
			onSetActiveState?.Invoke(false);
		}

		public void AttachBehaviour(IPhysicStateBehaviour behaviour) {
			m_AttachedBehaviour = behaviour;
		}
		#endregion


		#region Physics Callbacks
		internal void CollisionEnter(Collision other) {
			m_AttachedBehaviour?.OnStateCollisionEnter(m_IsActive, other);
		}

		internal void CollisionExit(Collision other) {
			m_AttachedBehaviour?.OnStateCollisionExit(m_IsActive, other);
		}
		
		internal void CollisionStay(Collision other) {
			m_AttachedBehaviour?.OnStateCollisionStay(m_IsActive, other);
		}

		internal void TriggerEnter(Collider other) {
			m_AttachedBehaviour?.OnStateTriggerEnter(m_IsActive, other);
		}

		internal void TriggerExit(Collider other) {
			m_AttachedBehaviour?.OnStateTriggerExit(m_IsActive, other);
		}
		
		internal void TriggerStay(Collider other) {
			m_AttachedBehaviour?.OnStateTriggerStay(m_IsActive, other);
		}
		#endregion
	}
}
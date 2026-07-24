using DevCore.PrototypingToolkit;
using UnityEngine;

public abstract class PhysicsStateBehaviour : MonoBehaviour, IPhysicStateBehaviour {
	[Header("State Setting")]
	[SerializeField] private PhysicState m_PhysicState = new PhysicState();
	[SerializeField] private PhysicStateHolder m_Holder = null;
	[SerializeField] private bool activateOnEnable = false;

	private void Awake() {
		m_PhysicState.AttachBehaviour(this);
		OnBehaviourAwake();
	}

	private void OnEnable() {
		if (activateOnEnable) {
			ActivateState();
		}

		OnBehaviourEnable();
	}

	private void OnDisable() {
		if (m_PhysicState.isActive) {
			DeactivateState();
		}
		OnBehaviourDisable();
	}

	public void ActivateState() {
		m_Holder.AddStateOnStack(m_PhysicState);
	}

	public void DeactivateState() {
		m_Holder.RemoveStateOnStack(m_PhysicState);
	}


	#region Callbacks
	protected virtual void OnBehaviourAwake() { }
	protected virtual void OnBehaviourEnable() { }
	protected virtual void OnBehaviourDisable() { }

	public virtual void OnBeforePhysicsResolved() { }
	public virtual void OnAfterPhysicsResolved() { }

	public virtual void OnStateCollisionEnter(bool active, Collision other) { }
	public virtual void OnStateCollisionExit(bool active, Collision other) { }
	public virtual void OnStateCollisionStay(bool active, Collision other) { }

	public virtual void OnStateTriggerEnter(bool active, Collider other) { }
	public virtual void OnStateTriggerExit(bool active, Collider other) { }
	public virtual void OnStateTriggerStay(bool active, Collider other) { }
	#endregion


	#region Editor
	private void OnValidate() {
		if (m_Holder == null) {
			m_Holder = GetComponentInParent<PhysicStateHolder>();
		}
		OnBehaviourValidate();
	}
	
	protected virtual void OnBehaviourValidate(){}
	#endregion
}
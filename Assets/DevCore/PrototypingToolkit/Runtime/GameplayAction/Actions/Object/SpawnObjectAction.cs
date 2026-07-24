using DevCore.PrototypingToolkit;
using UnityEngine;

namespace DevCore.PrototypingToolkit {
	/// <summary>
	/// Loot objects at position on death
	/// </summary>
	[AddComponentMenu("Spawn/Spawn Object")]
	public class SpawnObjectAction : GameplayActionComponent {
		#region Settings
		[SerializeField] private GameObject m_Object = null;
		
		[Header("Transformation")]
		[SerializeField] private Vector3 m_PositionOffset = Vector3.zero;
		[SerializeField] private Vector3 m_RotationOffset = Vector3.zero;

		[Header("Parent")]
		[SerializeField] private bool m_AttachToParent = false;
		[SerializeField] private bool m_InitWithParentRotation = false;
		#endregion
		
		#region Behaviour
		protected override void Execute(GameObject gameObject) {
			Transform rootTransform = gameObject.transform;
			Vector3 position = rootTransform.position + m_PositionOffset;
			Quaternion rotation = Quaternion.Euler(m_RotationOffset);

			if (m_InitWithParentRotation) {
				rotation *= rootTransform.rotation;
			}

			if (m_AttachToParent) {
				Instantiate(m_Object, position, rotation, rootTransform);
			} else {
				Instantiate(m_Object, position, rotation);
			}
		}
		#endregion
	}
}
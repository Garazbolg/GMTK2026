using UnityEngine;

namespace DevCore.FeedbackEngine {
	[AddComponentMenu("VFX/Particle Effect")]
	public class ParticleEffectFeedback : FeedbackComponent {
		#region Settings
		[SerializeField] private ParticleSystem m_ParticleEffect = null;

		[Header("Transform")]
		[SerializeField] private Vector3 m_PositionOffset = Vector3.zero;
		[SerializeField] private Vector3 m_RotationOffset = Vector3.zero;
		[SerializeField] private bool m_SpawnWithParentRotation = false;
		#endregion
		

		protected override void PlayFeedbackComponent(GameObject owner) {
			//TODO manage pooling
			Vector3 position; 
			Quaternion rotation;
			if (owner) {
				var ownerTransform = owner.transform;
				position = ownerTransform.position + m_PositionOffset;
				rotation = Quaternion.Euler(m_RotationOffset);
				
				if (m_SpawnWithParentRotation) {
					rotation *= ownerTransform.rotation;
				}
			} else {
				position = m_PositionOffset;
				rotation = Quaternion.Euler(m_RotationOffset);
			}
			
			var instance = Instantiate(m_ParticleEffect, position, rotation);
			
			var mainModule = instance.main;
			mainModule.playOnAwake = false;
			mainModule.stopAction = ParticleSystemStopAction.Destroy;
			
			instance.Play();
		}
	}
}
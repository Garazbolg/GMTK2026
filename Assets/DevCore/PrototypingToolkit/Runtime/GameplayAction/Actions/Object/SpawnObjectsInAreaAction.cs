using DevCore.Core;
using UnityEngine;

namespace DevCore.PrototypingToolkit {
	/// <summary>
	/// Loot objects at position on death
	/// </summary>
	[AddComponentMenu("Spawn/Spawn Objects In Area")]
	public class SpawnObjectsInAreaAction : GameplayActionComponent {
		#region Settings
		[SerializeField] private float m_SpawnAreaRadius = 1f;
		[SerializeField] private Vector3 m_AreaScale = Vector3.one;
		[SerializeField] private SpawnableObject[] m_Objects = { };
		#endregion

		#region Sub classes
		[System.Serializable]
		private class SpawnableObject {
			public GameObject m_Object = null;
			[Min(0)] public Vector2Int m_CountRange = new Vector2Int(1, 3);
			[Range(0f, 1f)] public float m_SpawnProbability = 1f;
		}
		#endregion

		#region Behaviour
		protected override void Execute(GameObject gameObject) {
			foreach (var spawnable in m_Objects) {
				if (Random.value <= spawnable.m_SpawnProbability) {
					int instancesCount = Random.Range(spawnable.m_CountRange.x, spawnable.m_CountRange.y);
					for (int i = 0; i < instancesCount; i++) {
						Vector3 position = VectorUtility.RandomDirection3D();
						position *= Random.value * m_SpawnAreaRadius;
						position.Scale(m_AreaScale);
						Instantiate(spawnable.m_Object, position + gameObject.transform.position, Quaternion.identity);
					}
				}
			}
		}
		#endregion
	}

}

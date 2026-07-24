using UnityEngine;
using UnityEngine.Rendering;

namespace DevCore.FeedbackEngine {
	[AddComponentMenu("Camera/Volume Effect")]
	public class VolumeEffectFeedback : FeedbackComponent {
		#region Settings
		[SerializeField] private VolumeProfile m_Profile = null;
		[SerializeField, Min(0f)] private int m_Priority = 0;
		[SerializeField, Min(0f)] private float m_Duration = 1f;
		[SerializeField] private AnimationCurve m_WeightOverLifetime = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});
		#endregion


		#region Properties
		public VolumeProfile profile => m_Profile;
		public int priority => m_Priority;
		public float duration => m_Duration;
		public AnimationCurve weightOverLifetime => m_WeightOverLifetime;
		#endregion


		#region Current
		private static VolumeFeedbackPlayer m_VolumeEffectPlayerPrefab = null;
		internal static UnityEngine.Pool.ObjectPool<VolumeFeedbackPlayer> m_FeedbackPlayerPool = null;
		private static int m_InstantiatedPlayersCount = 0;
		#endregion


		#region Init
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void OnInit() {
			m_VolumeEffectPlayerPrefab = (Resources.Load("FeedbackPlayers/VolumeFeedbackPlayer") as GameObject).GetComponent<VolumeFeedbackPlayer>();
			m_FeedbackPlayerPool = new UnityEngine.Pool.ObjectPool<VolumeFeedbackPlayer>(InstantiatePlayer);
			m_InstantiatedPlayersCount = 0;
		}
		#endregion

		#region Play
		protected override void PlayFeedbackComponent(GameObject owner) {
			var player = m_FeedbackPlayerPool.Get();
			player.Play(this);
		}
		#endregion


		#region Pool
		private static VolumeFeedbackPlayer InstantiatePlayer() {
			var instance = Instantiate(m_VolumeEffectPlayerPrefab);
			DontDestroyOnLoad(instance);
			instance.name = $"VolumeFeedbackPlayer_{m_InstantiatedPlayersCount}";
			m_InstantiatedPlayersCount++;
			return instance;
		}
		#endregion
	}
}
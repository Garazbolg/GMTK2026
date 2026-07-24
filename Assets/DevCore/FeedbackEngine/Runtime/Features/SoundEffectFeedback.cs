using DevCore.Core;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace DevCore.FeedbackEngine {
	[AddComponentMenu("Audio/Sound Effect")] 
	public class SoundEffectFeedback : FeedbackComponent {
		#region Settings
		[SerializeField] private AudioClip[] m_AudioClips = null;
		[SerializeField] private AudioMixerGroup m_MixerGroup = null;

		[Space]
		[SerializeField, Min(0.001f)] private Vector2 m_VolumeMinMax = new Vector2(0.9f, 1.1f);
		[SerializeField, Min(0f)] private Vector2 m_PitchMinMax = new Vector2(1f, 1f);
		[SerializeField, Range(0f, 1f)] private float m_StereoPan = 0f;

		[Space]
		[SerializeField, Min(0f)] private float m_Delay = 0f;
		#endregion

		#region Currents
		private static AudioSource m_AudioPlayerPrefab = null;
		private static ObjectPool<AudioSource> m_AudioPlayersPool = null;
		private static int m_AudioPlayerCount = 0;
		#endregion
		
		#region Init
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void OnInit() {
			m_AudioPlayerPrefab = (Resources.Load("FeedbackPlayers/AudioFeedbackPlayer") as GameObject).GetComponent<AudioSource>();
			m_AudioPlayersPool = new ObjectPool<AudioSource>(InstantiatePLayer, null, DisposePlayer);
			m_AudioPlayerCount = 0;
		}

		
		#endregion
		
		protected override void PlayFeedbackComponent(GameObject owner) {
			int audioClipsCount = m_AudioClips.Length;
			AudioClip clip;
			
			if (audioClipsCount > 1) {
				clip = m_AudioClips.GetRandom();
			} else if (audioClipsCount == 1) {
				clip = m_AudioClips[0];
			} else {
				return;
			}
			
			var instance = m_AudioPlayersPool.Get();
			instance.enabled = true;
			instance.clip = clip;
			instance.outputAudioMixerGroup = m_MixerGroup;
			instance.volume = m_VolumeMinMax.RandomRange();
			instance.panStereo = m_StereoPan;
			
			float pitch = m_PitchMinMax.RandomRange();
			instance.pitch = pitch;

			instance.transform.position = owner.transform.position; 

			if (m_Delay <= 0f) {
				instance.Play();
			} else {
				instance.PlayDelayed(m_Delay);
			}

			Cooldown.Launch((clip.length / pitch) + m_Delay, (result) => Release(instance));
		}
		
		private void Release(AudioSource player) {
			m_AudioPlayersPool.Release(player);
		}

		#region Pooling
		private static AudioSource InstantiatePLayer() {
			var instance = Instantiate(m_AudioPlayerPrefab);
			DontDestroyOnLoad(instance);
			instance.name = $"AudioFeedbackPlayer{m_AudioPlayerCount}";
			m_AudioPlayerCount++;
			return instance;
		}
		
		private static void DisposePlayer(AudioSource player) {
			player.enabled = false;
			player.clip = null;
			player.outputAudioMixerGroup = null;
		}
		#endregion
	}
}
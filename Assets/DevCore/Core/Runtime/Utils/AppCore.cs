using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DevCore.Core {
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public static class AppCore {
		#region Currents
		private static bool m_IsPlaying = false;
		#endregion


		#region Construction
		static AppCore() {
#if UNITY_EDITOR
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
			Application.quitting -= OnApplicationQuit;
			Application.quitting += OnApplicationQuit;
#endif
		}
		#endregion


#if UNITY_EDITOR
		private static void OnApplicationQuit(){
			m_IsPlaying = false;
		}

		private static void OnPlayModeStateChanged(PlayModeStateChange state) {
			switch (state) {
				case PlayModeStateChange.EnteredPlayMode:
					m_IsPlaying = true;
					break;
				
				case PlayModeStateChange.ExitingPlayMode:
					m_IsPlaying = false;
					break;
				
				default:
					break;
			}
		}
#endif

		public static  void Quit() {
#if UNITY_EDITOR
			if (Application.isEditor) {
				EditorApplication.isPlaying = false;
			}
#endif
			
			Application.Quit();
		}
		
		public static bool IsRunning() {
#if UNITY_EDITOR
			return Application.isPlaying && EditorApplication.isPlaying && m_IsPlaying;   
#else
			return true;
#endif
		}
	}
}
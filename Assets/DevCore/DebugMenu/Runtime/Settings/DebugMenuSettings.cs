using System.Collections;
using System.Collections.Generic;
using DevCore.Core;
using UnityEngine;

namespace DevCore.DebugMenu {
	public class DebugMenuSettings : ResourceAsset<DebugMenuSettings> {
		#region Settings
		[SerializeField, HideInInspector] private bool m_EnableDebugMenu = false;
		[SerializeField, HideInInspector] private bool m_HasBeenSetup = false;
		[SerializeField, HideInInspector,Min(0),
		 Tooltip("The frames count infos has to wait for a refresh, the higher is this value, the less the infos draw will be expensive. " +
		                                         "Refreshing a lot of informations per frames could case stuttering. It is also recommended to increase Refreshed Infos Per Frames")] 
		private int m_InfoRefreshSkipFrame = 0;
		[SerializeField, HideInInspector,Min(0),
		 Tooltip("The infos refreshed per frames. 0 refresh every displayed info")] 
		private int m_RefreshedInfosPerFrames = 0;
		#endregion


		#region Properties
		public bool enableDebugMenu {
			get => m_EnableDebugMenu;
			set {
				m_EnableDebugMenu = value;
				Save();
			}
		}
		
		public bool hasBeenSetup {
			get => m_HasBeenSetup;
			set {
				m_HasBeenSetup = value;
				Save();
			}
		}
		
		public int InfoRefreshSkipFrame {
			get => m_InfoRefreshSkipFrame;
			set {
				if (value < 0) {
					value = 0;
				}
				m_InfoRefreshSkipFrame = value;
				Save();
			}
		}
		
		public int RefreshedInfosPerFrames {
			get => m_RefreshedInfosPerFrames;
			set {
				if (value < 0) {
					value = 0;
				}
				m_RefreshedInfosPerFrames = value;
				Save();
			}
		}
		#endregion
	}
}
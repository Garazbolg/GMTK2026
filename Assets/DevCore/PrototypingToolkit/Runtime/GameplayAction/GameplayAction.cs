using System;
using DevCore.Core;
using UnityEngine;

namespace DevCore.PrototypingToolkit {
	[CreateAssetMenu(fileName = "GA_", menuName = Constants.ASSET_PATH + "Toolbox/Gameplay Action")]
	public sealed class GameplayAction : CompositeAsset<GameplayActionComponent> {
		public void Execute(GameObject gameObject) {
#if UNITY_EDITOR
			if (!AppCore.IsRunning()) {
				return;
			}
#endif

#if UNITY_EDITOR
			try {
#endif
				int compCount = componentsCount;
				for (int i = 0; i < compCount; i++) {
					GetComponentAtIndex(i).ExecuteInternal(gameObject);
				}
#if UNITY_EDITOR
			}
			catch (Exception e) {
				Debug.LogException(e, this);
				throw;
			}
#endif
		}

		public void Stop() {
#if UNITY_EDITOR
			if (!AppCore.IsRunning()) {
				return;
			}
#endif

#if UNITY_EDITOR
			try {
#endif
				int compCount = componentsCount;
				for (int i = 0; i < compCount; i++) {
					GetComponentAtIndex(i).StopInternal();
				}
#if UNITY_EDITOR
			}
			catch (Exception e) {
				Debug.LogException(e, this);
				throw;
			}
#endif
		}
	}
}
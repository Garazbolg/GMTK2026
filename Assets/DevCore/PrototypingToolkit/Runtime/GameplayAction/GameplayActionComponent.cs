using DevCore.Core;
using UnityEngine;

namespace DevCore.PrototypingToolkit {
	public abstract class GameplayActionComponent : AssetComponent {
		protected abstract void Execute(GameObject gameObject);
		protected virtual void Stop(){}
        
		internal void ExecuteInternal(GameObject gameObject) {
			if (active) {
				Execute(gameObject);
			}
		}

		internal void StopInternal() {
			if (active) {
				Stop();
			}
		}
	}
}
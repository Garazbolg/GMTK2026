using System;
using DevCore.Core;
using UnityEngine;

namespace DevCore.PrototypingToolkit
{
    [CreateAssetMenu(fileName = "APP_", menuName = Constants.ASSET_PATH + "Toolbox/Application Action")]
    public sealed class ApplicationAction : CompositeAsset<ApplicationActionComponent>
    {
        public void Execute() {
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
                GetComponentAtIndex(i).ExecuteInternal();
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

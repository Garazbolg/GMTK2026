using DevCore.SceneManagement.Legacy;
using UnityEngine;

namespace DevCore.PrototypingToolkit
{
    [AddComponentMenu("Scenes/Load Scenes")]
    public class LoadScenesAction : ApplicationActionComponent {
        [SerializeField] private SceneBundle m_Scenes;
        [SerializeField] private bool m_Async;
        [Space]
        [SerializeField] private ApplicationAction m_LoadingCanceledAction;
        [SerializeField] private ApplicationAction m_LoadingCompleteAction;
        
        protected override void Execute() {
            if (!EnhancedSceneManager.CanLoad(m_Scenes)) {
                m_LoadingCanceledAction?.Execute();
                return;
            }

            if (m_Async) {
                m_Scenes.LoadAsync(OnLoadingAsyncComplete);
            } else {
                m_Scenes.Load();
                m_LoadingCompleteAction?.Execute();
            }
        }

        private void OnLoadingAsyncComplete(LoadingStepResult result) {
            m_LoadingCompleteAction?.Execute();
        }
    }
}

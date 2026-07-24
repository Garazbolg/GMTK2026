using UnityEngine;

namespace DevCore.ApplicationStates {
    [DefaultExecutionOrder(-20000)]
    public sealed class ApplicationStackManager : MonoBehaviour {
        #region Currents
        internal static ApplicationStackManager m_Instance = null;
        #endregion


        #region Callbacks
        private void Awake() {
            if (m_Instance != null) {
                Debug.LogError("[Application Stack Manager] : Only one instance can be created");
                Destroy(gameObject);
            } else {
                m_Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        
        private void Update() {
            ApplicationStack.Update();
        }

        private void OnDestroy() {
            if (m_Instance == this) {
                m_Instance = null;
                ApplicationStack.m_initialized = false;
            }
        }
        #endregion
    }
}
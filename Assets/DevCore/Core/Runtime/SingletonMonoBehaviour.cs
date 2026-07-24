using System;

namespace CoreDev.Utility {
    using UnityEngine;

    /// <summary>
    /// Base singleton class
    /// </summary>
    [DefaultExecutionOrder(-500)]
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
        #region Currents
        private static T m_Instance = null;
        private static bool m_HasInstance = false;
        #endregion

        #region Propeties
        public static T instance {
            get {
                if (!m_HasInstance) {
                    m_Instance = FindAnyObjectByType<T>();
                }

                return m_Instance;
            }
            private set {
                m_HasInstance = value != null;
                m_Instance = value;
            }
        }
        #endregion

        #region Callbacks
        protected virtual void Awake() {
            if(m_Instance == null) {
                instance = this as T;
            } else {
                if(m_Instance != this) {
                    Destroy(gameObject);
                }
            }
        }

        protected void OnDestroy() {
            if (m_Instance == this) {
                instance = null;
            }
        }

        private static void OnInit() {
            m_Instance = null;
            m_HasInstance = false;
        }
        #endregion
    }
}

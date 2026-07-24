using UnityEngine;

namespace DevCore.Core {

    /// <summary>
    /// Stores the main camera in a static reference value
    /// </summary>
    public class MainCameraBuffer : MonoBehaviour {
        [SerializeField] private Camera m_Camera = null;

        private static Camera m_MainCamera = null;

        public static Camera Get() {
            return m_MainCamera;
        }

        private void OnEnable() {
            m_MainCamera = m_Camera;
        }

        private void OnDisable() {
            if (m_MainCamera == this) {
                m_MainCamera = null;
            }
        }
    }

}
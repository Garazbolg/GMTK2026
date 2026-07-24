//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace DevCore.ArtToolset {

    public abstract class GeneratedTexture : ScriptableObject {
        #region Data
        [SerializeField] protected Texture2D m_Texture = null;
        #endregion
        

        #region Texture
        /// <summary>
        /// Returns the generated texture
        /// </summary>
        /// <param name="recompute">Should the texture should be recomputed, in case a parameter had been changed</param>
        /// <returns></returns>
        public Texture2D GetTexture(bool recompute = false) {
            bool generateTexture = m_Texture == null;
            if(generateTexture) {
                RegenerateTexture();
            }

            if(recompute || generateTexture) {
                ComputeTexture();
            }

            return m_Texture;
        }

        /// <summary>
        /// Compute each pixel of the texture and return the result
        /// </summary>
        /// <returns></returns>
        protected abstract void ComputeTexture();

        /// <summary>
        /// Generate a the texture with target dimensions 
        /// </summary>
        /// <returns></returns>
        protected abstract Texture2D CreateTexture();
        #endregion

        private void RegenerateTexture() {
            m_Texture = CreateTexture();
            
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}

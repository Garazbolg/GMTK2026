namespace DevCore.ArtToolset {
    using UnityEngine;

    [CreateAssetMenu(fileName = "newAtlasTexture", menuName = ArtToolsetUtility.kArtToolsetAssetPath + "Generated Texture/Gradient Atlas")]
    public class GradientAtlas : GeneratedTexture {
        private const int k_TextureWidth = 128;
        
        #region Datas
        [SerializeField, GradientUsage(true)] private Gradient[] m_Gradients = { };
        [SerializeField] private int m_Resolution = 128;
        [SerializeField] private bool m_StackFromTop = false;
        #endregion

        protected override void ComputeTexture() {
            int rows = Mathf.Min(m_Gradients.Length, m_Resolution);

            m_Texture.Reinitialize(k_TextureWidth, m_Resolution);
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                m_Texture.SetPixels(new Color[k_TextureWidth * m_Resolution]);
            }
#endif
            if (m_StackFromTop) {
                for (int i = 0; i < rows; i++) {
                    WriteAtlasRow(i, i);   
                }
            } else {
                for (int i = m_Resolution - 1, gradId = 0; i >= 0 && gradId < rows; i--, gradId++) {
                    WriteAtlasRow(i, gradId);
                }
            }

            m_Texture.Apply();
        }

        private void WriteAtlasRow(int row, int gradientId) {
            for (int x = 0; x < k_TextureWidth; x++) {
                float t = (float)x / (float)k_TextureWidth;
                t += (1f / (float)k_TextureWidth) * t;
                m_Texture.SetPixel(x, m_Resolution - row - 1, m_Gradients[gradientId].Evaluate(t));
            }
        }

        protected override Texture2D CreateTexture() {
            Texture2D tex = new Texture2D(m_Resolution, m_Resolution, TextureFormat.RGBA32, false);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Bilinear;
            tex.anisoLevel = 0;
            return tex;
        }
    }
}
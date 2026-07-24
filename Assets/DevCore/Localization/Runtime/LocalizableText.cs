using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.Localization {
    [System.Serializable]
    public class LocalizableText {
        #region Datas
        private string m_FallbackText = null;
        #endregion


        #region Properties
        public string Text {
            get {
                //For nwo returns fallback text but should return the localized one later
                return m_FallbackText;    
            }
        }
        #endregion


        #region Overloads
        public static implicit operator string(LocalizableText text) => text.Text;
        
        public override string ToString() {
            return Text;
        }
        #endregion
    }
}

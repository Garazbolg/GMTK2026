using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevCore.DebugMenu
{
    public class DebugCategoryPathUI : MonoBehaviour {
        [SerializeField] private DebugButton m_Button = null;
        [SerializeField] internal Text m_Text = null;
        
        private DebugCategory m_Category = null;

        private void Awake() {
            m_Button.SetAction(DrawCategory);
        }

        internal void SetCategory(DebugCategory category) {
            m_Category = category;
        }

        internal void DrawLabel(bool wide, bool last) {
            string text;
            if (wide) {
                text = m_Category.m_Name;
            } else {
                text = m_Category.m_Name[0].ToString();
            }

            if (!last) {
                text += " / ";
            }

            m_Text.text = text;
        }

        internal void Dispose() {
            m_Category = null;
        }

        public void DrawCategory() {
            m_Category.Draw();
        }
    }
}

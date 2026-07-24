using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevCore.DebugMenu {
	public class GraphicGroup : Graphic {
		#region Settings
		[SerializeField] private Graphic[] m_ChildGraphics = { };
		#endregion
		
		#region Properties
		public override Color color {
			get => base.color;
			set { base.color = value; }
		}
		#endregion


		#region Color
		private void SetChildsColor(Color col) {
			for (int i = 0; i < m_ChildGraphics.Length; i++) {
				m_ChildGraphics[i].color = col;
			}
		}
		
		public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha) {
			base.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
			SetChildsColor(targetColor);
		}

		public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha,
			bool useRGB) {
			base.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha, useRGB);
			SetChildsColor(targetColor);
		}
		#endregion
	}
}
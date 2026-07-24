using UnityEditor;
using UnityEngine;

namespace DevCore.Core.Editor {
	public class GUIHorizontalScope {
		#region Constants
		private const float SPACING = 2f;
		#endregion


		#region Currents
		private Rect m_RemainingRect = Rect.zero;
		#endregion


		#region Construction
		public GUIHorizontalScope(Rect rect) {
			m_RemainingRect = rect;
		}
		#endregion


		public Rect GetInsertedRect() {
			return m_RemainingRect;
		}

		public float GetRemainingSpace(float space, int elements, bool ignoreLabelSpace = true) {
			float remainingSpacing = m_RemainingRect.width - space - (SPACING * elements);
			if (!ignoreLabelSpace) {
				remainingSpacing -= EditorGUIUtility.labelWidth;
			}
			if (remainingSpacing < 0f) {
				return 0f;
			}

			return remainingSpacing;
		}

		public Rect GetInsertedRect(float width, bool fillLabelSpace = false) {
			Rect rect = m_RemainingRect;
			float filledSpace = width;
			if (fillLabelSpace) {
				filledSpace += EditorGUIUtility.labelWidth;
			}

			if (filledSpace <= m_RemainingRect.width) {
				rect.width = filledSpace;
				float offset = Mathf.Min(filledSpace + SPACING, m_RemainingRect.width);
				m_RemainingRect.width -= offset;
				m_RemainingRect.x += offset;
			}
			else {
				rect.width = m_RemainingRect.width;
				m_RemainingRect.width = 0f;
			}

			return rect;
		}
	}
}
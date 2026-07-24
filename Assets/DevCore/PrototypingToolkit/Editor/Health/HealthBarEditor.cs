using System;
using DevCore.Core;
using UnityEditor;
using UnityEngine;

namespace DevCore.PrototypingToolkit.Editor {
	[CustomEditor(typeof(HealthBar))]
	public class HealthBarEditor : UnityEditor.Editor {
		private HealthBar m_Target = null;
		
		private void OnEnable() {
			m_Target = (target as HealthBar);
			m_Target.onHeal += OnHeal;
			m_Target.onTakeDamages += OnTakeDamages;
			m_Target.onMaxHealthChanges += OnMaxHealthChanges;
		}
		

		private void OnDisable() {
			m_Target.onHeal -= OnHeal;
			m_Target.onTakeDamages -= OnTakeDamages;
			m_Target.onMaxHealthChanges -= OnMaxHealthChanges;
		}

		public override void OnInspectorGUI() {
			if (AppCore.IsRunning()) {
				EditorGUILayout.Space();
				;

				int healthPoints = m_Target.currentHealth;
				int maxHealthPoints = m_Target.maxHealth;
				float percentage = (float) healthPoints / (float) maxHealthPoints;
			
				var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
				EditorGUI.ProgressBar(rect, percentage, $"{healthPoints}/{maxHealthPoints}");
				EditorGUILayout.Space();
			}
			
			base.OnInspectorGUI();
		}
		
		private void OnMaxHealthChanges(int maxHealth) {
			Repaint();	
		}

		private void OnTakeDamages(int damages) {
			Repaint();
		}

		private void OnHeal(int heal) {
			Repaint();
		}
	}
}
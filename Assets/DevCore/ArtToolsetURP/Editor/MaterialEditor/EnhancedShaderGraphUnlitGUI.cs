using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevCore.URP.Editor {
	public class EnhancedShaderGraphUnlitGUI : FilteredShaderGraphUnlitGUI {
		public override void OnGUI(MaterialEditor materialEditorIn, MaterialProperty[] properties) {
			base.OnGUI(materialEditorIn, properties);
		}

		public override void FindProperties(MaterialProperty[] properties) {
			base.FindProperties(properties);
		}
	}
}

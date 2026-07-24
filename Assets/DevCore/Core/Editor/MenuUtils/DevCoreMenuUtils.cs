using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace DevCore.Core.Editor {
	public static class DevCoreMenuUtils {
		public const string DEV_CORE_MENU = "DevCore/Utility/";


		#region Utils
		[MenuItem(DEV_CORE_MENU + "Reload Domain")]
		public static void ReloadDomain() {
			CompilationPipeline.RequestScriptCompilation();
		}
		#endregion
	}
}
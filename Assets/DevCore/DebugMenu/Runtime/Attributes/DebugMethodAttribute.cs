using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.DebugMenu {
	[AttributeUsage(AttributeTargets.Method)]
	public class DebugMethodAttribute : Attribute {
		internal string m_Path;
		
		public DebugMethodAttribute(string path) {
			m_Path = null;
		}
	}
}
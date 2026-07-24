using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.Core {
	[AttributeUsage(AttributeTargets.Field)]
	public class RequireTypeAttribute : PropertyAttribute {
		public Type type;

		public RequireTypeAttribute(Type type) {
			type = this.type;
		}
	}
}
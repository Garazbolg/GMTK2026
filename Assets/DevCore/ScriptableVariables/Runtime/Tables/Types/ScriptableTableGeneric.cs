using UnityEngine;

namespace DevCore.ScriptableVariables {
	[CreateAssetMenu(fileName = "TAB_", menuName = SVConsts.ST_PATH + "Generic", order = SVConsts.ASSET_ORDER_GENERIC)]
	public class ScriptableTableGeneric : ScriptableTableBase<object, ScriptableVariableBase> {
		public override object GetValue(int index) {
			return variables[index].value.wrappedValue;
		}
	}
}

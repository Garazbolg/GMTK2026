using UnityEngine;

namespace DevCore.ScriptableVariables {
	[CreateAssetMenu(fileName = "TAB_", menuName = SVConsts.ST_PATH + "String", order = SVConsts.ASSET_ORDER)]
	public class ScriptableTableString : ScriptableVariableTable<string> { }
}
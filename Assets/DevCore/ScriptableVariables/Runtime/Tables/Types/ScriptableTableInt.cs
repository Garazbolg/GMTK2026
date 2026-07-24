using UnityEngine;

namespace DevCore.ScriptableVariables {
	[CreateAssetMenu(fileName = "TAB_", menuName = SVConsts.ST_PATH + "Int", order = SVConsts.ASSET_ORDER)]
	public class ScriptableTableInt : ScriptableVariableTable<int> { }
}
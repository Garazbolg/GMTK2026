using UnityEngine;

namespace DevCore.ScriptableVariables {
	[CreateAssetMenu(fileName = "TAB_", menuName = SVConsts.ST_PATH + "Float", order = SVConsts.ASSET_ORDER)]
	public class ScriptableTableFloat : ScriptableVariableTable<float> { }
}
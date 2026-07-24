using UnityEngine;

namespace DevCore.ScriptableVariables {
	[CreateAssetMenu(fileName = "LIST_", menuName = SVConsts.SLIST_PATH + "Float", order = SVConsts.ASSET_ORDER)]
	public class ScriptableListFloat : ScriptableVariablesList<float> { }
}
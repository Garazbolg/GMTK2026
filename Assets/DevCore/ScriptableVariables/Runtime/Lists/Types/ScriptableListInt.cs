using UnityEngine;

namespace DevCore.ScriptableVariables {
	[CreateAssetMenu(fileName = "LIST_", menuName = SVConsts.SLIST_PATH + "Int", order = SVConsts.ASSET_ORDER)]
	public class ScriptableListInt : ScriptableVariablesList<int> { }
}
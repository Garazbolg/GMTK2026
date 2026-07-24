using UnityEngine;

namespace DevCore.ScriptableVariables {
	[CreateAssetMenu(fileName = "LIST_", menuName = SVConsts.SLIST_PATH + "String", order = SVConsts.ASSET_ORDER)]
	public class ScriptableListString : ScriptableVariablesList<string> { }
}
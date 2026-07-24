using UnityEngine;

namespace DevCore.ScriptableVariables {
	[CreateAssetMenu(fileName = "LIST_", menuName = SVConsts.SLIST_PATH + "Generic", order = SVConsts.ASSET_ORDER_GENERIC)]
	public class ScriptableListGeneric : ScriptableListBase<ScriptableVariableBase> { }
}
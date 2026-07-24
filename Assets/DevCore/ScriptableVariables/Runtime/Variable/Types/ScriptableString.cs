using DevCore.Core.Serialization;
using UnityEngine;

namespace DevCore.ScriptableVariables {
	[CreateAssetMenu(fileName = "STR_", menuName = SVConsts.SMV_PATH + "String", order = SVConsts.ASSET_ORDER)]
	public class ScriptableString : ScriptableVariable<string>, ITextSerializable {
		public string GetTextData() {
			return value;
		}

		public void FromTextData(string data) {
			value = data;
		}
	}
}
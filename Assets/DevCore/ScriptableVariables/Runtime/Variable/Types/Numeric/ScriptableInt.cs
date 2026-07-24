using System.Globalization;
using DevCore.Core;
using DevCore.Core.Serialization;
using UnityEngine;

namespace DevCore.ScriptableVariables {
	[CreateAssetMenu(fileName = "INT_", menuName = SVConsts.SMV_PATH + "Int", order = SVConsts.ASSET_ORDER)]
	public class ScriptableInt : ScriptableNumeric<int>, ITextSerializable {
		public string GetTextData() {
			return value.ToICString();
		}

		public void FromTextData(string data) {
			if (int.TryParse(data, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intResult)) {
				value = intResult;
			} else {
				Debug.LogError($"[Scriptable Int] Failed to parse data {data} on variable {name}");
			}
		}

		public override void Add(int value) {
			this.value += value;
		}

		public override void Subtract(int value) {
			this.value -= value;
		}

		public override void Multiply(int value) {
			this.value *= value;
		}

		public override void Divide(int value) {
			this.value /= value;
		}
	}
}
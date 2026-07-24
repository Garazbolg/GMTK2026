using System.Globalization;
using DevCore.Core;
using DevCore.Core.Serialization;
using UnityEngine;

namespace DevCore.ScriptableVariables {
	[CreateAssetMenu(fileName = "FLT_", menuName = SVConsts.SMV_PATH + "Float", order = SVConsts.ASSET_ORDER)]

	public class ScriptableFloat : ScriptableNumeric<float>, ITextSerializable {
		public string GetTextData() {
			return value.ToICString();
		}

		public void FromTextData(string data) {
			if (float.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatResult)) {
				value = floatResult;
			} else {
				Debug.LogError($"[Scriptable Int] Failed to parse data {data} on variable {name}");
			}
		}

		public override void Add(float value) {
			this.value += value;
		}

		public override void Subtract(float value) {
			this.value -= value;
		}

		public override void Multiply(float value) {
			this.value *= value;
		}

		public override void Divide(float value) {
			this.value /= value;
		}
	}
}
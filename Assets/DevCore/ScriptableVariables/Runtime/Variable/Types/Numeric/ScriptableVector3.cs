using System.Globalization;
using DevCore.Core;
using DevCore.Core.Serialization;
using UnityEngine;

namespace DevCore.ScriptableVariables {
	[CreateAssetMenu(fileName = "V3_", menuName = SVConsts.SMV_PATH + "Vector 3", order = SVConsts.ASSET_ORDER)]
	public class ScriptableVector3 : ScriptableNumeric<Vector3>, ITextSerializable {
		public string GetTextData() {
			return $"{value.x.ToICString()},{value.y.ToICString()},{value.z.ToICString()}";
		}

		public void FromTextData(string data) {
			string[] vectorComponents = data.Split(',');
			Vector3 vector = new Vector3();
			for (int i = 0; i < 3; i++) {
				if (float.TryParse(vectorComponents[i], NumberStyles.Float, CultureInfo.InvariantCulture,
					    out float floatResult)) {
					vector[i] = floatResult;
				} else {
					vector = Vector3.zero;
					Debug.LogError($"[Scriptable Vector3] Failed to parse data {data} on variable {name}");
					break;
				}
			}

			value = vector;
		}


		#region Numeric Override
		public override void Add(Vector3 value) {
			this.value += value;
		}

		public override void Subtract(Vector3 value) {
			this.value -= value;
		}

		public override void Multiply(Vector3 value) {
			this.value.Scale(value);
		}

		public override void Divide(Vector3 value) {
			this.value.Divide(value);
		}
		#endregion
	}
}
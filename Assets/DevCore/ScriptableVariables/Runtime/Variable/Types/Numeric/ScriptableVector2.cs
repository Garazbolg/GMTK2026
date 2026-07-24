using System.Globalization;
using DevCore.Core;
using DevCore.Core.Serialization;
using UnityEngine;

namespace DevCore.ScriptableVariables
{
    [CreateAssetMenu(fileName = "V2_", menuName = SVConsts.SMV_PATH + "Vector 2", order = SVConsts.ASSET_ORDER)]
    public class ScriptableVector2 : ScriptableNumeric<Vector2>, ITextSerializable
    {
        public string GetTextData() {
            return $"{value.x.ToICString()},{value.y.ToICString()}";
        }

        public void FromTextData(string data) {
            string[] vectorComponents = data.Split(',');
            Vector2 vector = new Vector2();
            for (int i = 0; i < 2; i++) {
                if (float.TryParse(vectorComponents[i], NumberStyles.Float, CultureInfo.InvariantCulture,
                        out float floatResult)) {
                    vector[i] = floatResult;
                } else {
                    vector = Vector2.zero;
                    Debug.LogError($"[Scriptable Vector2] Failed to parse data {data} on variable {name}");
                    break;
                }
            }

            value = vector;
        }


        #region Numeric Override
        public override void Add(Vector2 value) {
            this.value += value;
        }

        public override void Subtract(Vector2 value) {
            this.value -= value;
        }

        public override void Multiply(Vector2 value) {
            this.value.Scale(value);
        }

        public override void Divide(Vector2 value) {
            this.value.Divide(value);
        }
        #endregion
    }
}

using System.Collections;
using UnityEngine;

namespace DevCore.ScriptableVariables {
	public abstract class ScriptableArrayBase<T> : ScriptableEnumerable<T> where T : IValueWrapper {
		#region Datas
		[SerializeField] protected T[] variables = {};
		#endregion

		#region Properties
		public int length => variables.Length;
		
		public override object wrappedValue {
			get => variables;
			set {
				if (value is T[] arr) {
					variables = arr;
				}
				else {
					throw new InvalidWrapperCastException();
				}
			}
		}
		#endregion


		#region Getters
		public override int GetCount() {
			return length;
		}

		public override T Get(int index) {
			return variables[index];
		}

		public override IEnumerator GetEnumerator() {
			return variables.GetEnumerator();
		}
		#endregion
	}
}
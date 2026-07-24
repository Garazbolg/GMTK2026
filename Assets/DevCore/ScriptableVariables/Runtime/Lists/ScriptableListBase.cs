using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevCore.ScriptableVariables {
	public abstract class ScriptableListBase<T> : ScriptableEnumerable<T> where T : ScriptableVariableBase {
		#region Enums
		public enum ResetBehaviour {
			ResetElementsValues,
			Clear
		}
		#endregion


		#region Datas
		[SerializeField] protected List<T> variables = new List<T>();
		#endregion


		#region Properties
		public virtual T this[int index] {
			get => variables[index];
		}

		public int Count => variables.Count;

		public override object wrappedValue {
			get => variables;
			set {
				if (value is List<T> arr) {
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
			return Count;
		}

		public override T Get(int index) {
			return variables[index];
		}

		public override IEnumerator GetEnumerator() {
			return variables.GetEnumerator();
		}
		#endregion


		#region Manage List
		#region Datas
		public ResetBehaviour resetBehaviour = ResetBehaviour.ResetElementsValues;
		#endregion


		public void Clear() {
			variables.Clear();
		}

		public override sealed void ResetValue() {
			switch (resetBehaviour) {
				case ResetBehaviour.ResetElementsValues:
					for (int i = 0; i < variables.Count; i++) {
						variables[i].ResetValue();
					}

					break;
				case ResetBehaviour.Clear:
					Clear();
					break;
				default:
					break;
			}
		}

		public void Add(T element) {
			variables.Add(element);
		}

		public void Remove(T element) {
			variables.Remove(element);
		}

		public void RemoveAt(int index) {
			variables.RemoveAt(index);
		}
		#endregion
	}
}
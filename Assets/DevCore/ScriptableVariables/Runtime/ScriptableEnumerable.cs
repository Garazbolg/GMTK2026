using System.Collections;

namespace DevCore.ScriptableVariables {
	public abstract class ScriptableEnumerable<T> : ScriptableVariableBase, IValueTableConvertible, IEnumerable
		where T : IValueWrapper {
		public abstract int GetCount();
		public abstract T Get(int index);

		public object[] ToObjectArray() {
			int count = GetCount();
			var array = new object[count];
			for (int i = 0; i < count; i++) {
				array[i] = Get(i).wrappedValue;
			}

			return array;
		}

		public virtual KeyedValue[] GetValueTable() {
			int count = GetCount();
			var values = new KeyedValue[count];
			for (int i = 0; i < count; i++) {
				KeyedValue keyedVal;
				var val = Get(i);
				if (val is IValueTableConvertible convertibleVal) {
					keyedVal = new KeyedValue(val.wrapperName, convertibleVal.GetValueTable());
				} else {
					keyedVal = new KeyedValue(val);
				}

				values[i] = keyedVal;
			}

			return values;
		}

		public abstract IEnumerator GetEnumerator();
	}
}
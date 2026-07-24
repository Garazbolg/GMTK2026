using System;

namespace DevCore.ScriptableVariables {
	public abstract class ScriptableTableBase<ValueT, VariableT> : ScriptableArrayBase<ScriptableTableElement<VariableT>>
		where VariableT : ScriptableVariableBase {

		#region Properties
		public VariableT this[int index] {
			get => variables[index].value;
		}
		#endregion

		#region Getters
		public abstract ValueT GetValue(int index);

		public VariableT GetElementVariableByName(string name) {
			return GetElementByName(name).value;
		}
		
		public ScriptableTableElement<VariableT> GetElementByName(string name) {
			for (int i = 0; i < variables.Length; i++) {
				if (variables[i].name == name) {
					return variables[i];
				}
			}

			throw new ArgumentOutOfRangeException($"There's no element with name {name} in this table");	
		}

		public override KeyedValue[] GetValueTable() {
			var values = new KeyedValue[variables.Length];

			for (int i = 0; i < variables.Length; i++) {
				KeyedValue keyedVal;
				var var = variables[i]; 
				if (var.value is IValueTableConvertible convertible) {
					keyedVal = new KeyedValue(var.wrapperName, convertible.GetValueTable());
				}
				else {
					keyedVal = new KeyedValue(var.wrapperName, var.value.wrappedValue);
				}

				values[i] = keyedVal;
			}

			return values;
		}
		#endregion
		
		#region Table Management
		public override void ResetValue() {
			for (int i = 0; i < variables.Length; i++) {
				variables[i].value.ResetValue();
			} 
		}
		#endregion
	}
}
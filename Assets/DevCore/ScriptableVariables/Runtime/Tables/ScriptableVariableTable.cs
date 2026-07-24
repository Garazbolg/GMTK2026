namespace DevCore.ScriptableVariables {
	public class ScriptableVariableTable<T> : ScriptableTableBase<T, ScriptableVariable<T>> {
		public override T GetValue(int index) {
			return variables[index].value.value;
		}
	}
}
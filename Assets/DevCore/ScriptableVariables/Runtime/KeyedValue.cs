namespace DevCore.ScriptableVariables {
	public struct KeyedValue {
		public string name;
		public object value;

		public KeyedValue(string name, object value) {
			this.name = name;
			this.value = value;
		}

		public KeyedValue(IValueWrapper wrapper) {
			name = wrapper.wrapperName;
			value = wrapper.wrappedValue;
		}

		public override string ToString() {
			return name + " : " + value;
		}
	}
}
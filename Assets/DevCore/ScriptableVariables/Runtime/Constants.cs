using DevCore.Core;

namespace DevCore.ScriptableVariables {
	public static class SVConsts {
		/// <summary> Common menu path for every Scriptable Variable element</summary>
		private const string SV_PATH = Constants.ASSET_PATH + "Scriptable Variables/";
		
		/// <summary> Scriptable variable menu path</summary>
		public const string SMV_PATH = SV_PATH + "Variables/";
		
		/// <summary> Scriptable variable table menu path</summary>
		public const string ST_PATH = SV_PATH + "Table/";
		
		/// <summary> Scriptable variable array menu path</summary>
		public const string SARR_PATH = SV_PATH + "Array/";
		
		/// <summary> Scriptable variables list menu path </summary>
		public const string SLIST_PATH = SV_PATH + "List/";

		/// <summary> Scriptable variables utility objects menu path </summary>
		public const string SV_UTILS_PATH = SV_PATH + "Utility/";
		
		/// <summary>Default asset order for Scriptable Variable assets</summary>
		public const int ASSET_ORDER = 200;
		
		/// <summary> Asset order for generic Scriptable Variable assets (generic Table, Array, List...) </summary>
		public const int ASSET_ORDER_GENERIC = ASSET_ORDER - 100;
		
		/// <summary> Asset order for utility scriptable objects for scriptable variables </summary>
		public const int ASSET_ORDER_UTILITIES = ASSET_ORDER + 100;
	}
}
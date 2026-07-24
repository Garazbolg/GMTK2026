using UnityEngine;

namespace DevCore.Core {
	/// <summary>
	/// Describes a scripting symbol and its display name
	/// </summary>
	[CreateAssetMenu(menuName = Constants.ASSET_PATH + "Utility/Keyword Descriptor")]
	public class KeywordDescriptor : ScriptableObject {
		[SerializeField] private string m_DisplayName = string.Empty;
		[SerializeField] private string m_ScryptingSymbol = string.Empty;

		public string displayName => m_DisplayName;
		public string scryptingSymbol => m_ScryptingSymbol;
	}
}
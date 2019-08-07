using System.Collections.Generic;

namespace TranslateBot.Translation.Model
{
	/// <summary>
	/// Array of translated results from Translator API v3.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Избегайте внутренних классов, не имеющих экземпляры", Justification = "<Ожидание>")]
	internal class TranslatorResponse
	{
		public IEnumerable<TranslatorResult> Translations { get; set; }
	}
}

using System.Collections.Generic;

namespace TranslateBot.Translation.Model
{
	/// <summary>
	/// Array of translated results from Translator API v3.
	/// </summary>
	internal class TranslatorResponse
	{
		public IEnumerable<TranslatorResult> Translations { get; set; }
	}
}

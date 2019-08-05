using System.ComponentModel.DataAnnotations;

namespace TranslateBot.DAL.Models
{
	internal class Translation
	{
		public int Id { get; set; }
		[Required]
		public string RussianPhrase { get; set; }
		[Required]
		public string EnglishPhrase { get; set; }
	}
}

using System.ComponentModel.DataAnnotations;

namespace TranslateBot.DAL.Models
{
	public class Translation
	{
		public int Id { get; set; }
		[Required]
		public string RussianPhrase { get; set; }
		[Required]
		public string EnglishPhrase { get; set; }
	}
}

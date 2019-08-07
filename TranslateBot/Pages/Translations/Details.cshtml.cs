using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using TranslateBot.DAL;

namespace TranslateBot.Pages.Translations
{
	public class DetailsModel : PageModel
	{
		private readonly ApplicationContext _context;

		public DAL.Models.Translation Translation { get; set; }

		public DetailsModel(ApplicationContext context) => _context = context;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Translation = await _context.Translations
				.FirstOrDefaultAsync(item => item.Id == id)
				.ConfigureAwait(false);

			if (Translation == null)
			{
				return NotFound();
			}
			return Page();
		}
	}
}

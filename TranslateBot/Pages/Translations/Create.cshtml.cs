using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using TranslateBot.DAL;
using TranslateBot.DAL.Models;

namespace TranslateBot.Pages.Translations
{
	public class CreateModel : PageModel
	{
		private readonly ApplicationContext _context;

		[BindProperty]
		public Translation Translation { get; set; }

		public CreateModel(ApplicationContext context) => _context = context;

		public IActionResult OnGet() => Page();

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			await _context.Translations
				.AddAsync(Translation)
				.ConfigureAwait(false);
			await _context
				.SaveChangesAsync()
				.ConfigureAwait(false);

			return RedirectToPage("./Index");
		}
	}
}
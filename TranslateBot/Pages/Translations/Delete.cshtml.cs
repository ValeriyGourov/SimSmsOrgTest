using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using TranslateBot.DAL;

namespace TranslateBot.Pages.Translations
{
	public class DeleteModel : PageModel
	{
		private readonly ApplicationContext _context;

		[BindProperty]
		public DAL.Models.Translation Translation { get; set; }

		public DeleteModel(ApplicationContext context) => _context = context;

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

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Translation = await _context.Translations
				.FindAsync(id)
				.ConfigureAwait(false);

			if (Translation != null)
			{
				_context.Translations.Remove(Translation);
				await _context
					.SaveChangesAsync()
					.ConfigureAwait(false);
			}

			return RedirectToPage("./Index");
		}
	}
}

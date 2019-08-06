using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using TranslateBot.DAL;
using TranslateBot.DAL.Models;

namespace TranslateBot.Pages.Translations
{
	public class EditModel : PageModel
	{
		private readonly ApplicationContext _context;

		[BindProperty]
		public Translation Translation { get; set; }

		public EditModel(ApplicationContext context) => _context = context;

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

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			_context.Attach(Translation).State = EntityState.Modified;

			try
			{
				await _context
					.SaveChangesAsync()
					.ConfigureAwait(false);
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TranslationExists(Translation.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return RedirectToPage("./Index");
		}

		private bool TranslationExists(int id) => _context.Translations.Any(item => item.Id == id);
	}
}

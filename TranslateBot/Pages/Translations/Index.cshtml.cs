using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using TranslateBot.DAL;

namespace TranslateBot.Pages.Translations
{
	public class IndexModel : PageModel
	{
		private readonly ApplicationContext _context;

		public IList<DAL.Models.Translation> Translation { get; private set; }

		public IndexModel(ApplicationContext context) => _context = context;

		public async Task OnGetAsync()
		{
			Translation = await _context.Translations
				.ToListAsync()
				.ConfigureAwait(false);
		}
	}
}

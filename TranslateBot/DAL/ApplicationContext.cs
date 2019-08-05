using Microsoft.EntityFrameworkCore;

using TranslateBot.DAL.Models;

namespace TranslateBot.DAL
{
	internal class ApplicationContext : DbContext
	{
		public DbSet<Translation> Translations { get; set; }

		public ApplicationContext()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=wwwroot/TranslateBot.db");
			//optionsBuilder.UseSqlite("Data Source=TranslateBot.db");
			//optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
		}
	}
}

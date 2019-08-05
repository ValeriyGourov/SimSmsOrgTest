using Microsoft.EntityFrameworkCore;

using TranslateBot.DAL.Models;

namespace TranslateBot.DAL
{
	internal class ApplicationContext : DbContext
	{
		//private readonly string _connectionString;

		public DbSet<Translation> Translations { get; set; }

		public ApplicationContext(DbContextOptions<ApplicationContext> dbContextOptions)
			: base(dbContextOptions)
		{
			Database.EnsureCreated();
		}
		//public ApplicationContext(string connectionString)
		//{
		//	_connectionString = connectionString;

		//	Database.EnsureCreated();
		//}

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	optionsBuilder.UseSqlite(_connectionString);
		//	//optionsBuilder.UseSqlite("Data Source=wwwroot/TranslateBot.db");
		//	//optionsBuilder.UseSqlite("Data Source=TranslateBot.db");
		//	//optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
		//}
	}
}

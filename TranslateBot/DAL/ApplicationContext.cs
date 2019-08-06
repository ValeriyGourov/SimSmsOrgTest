using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using TranslateBot.DAL.Models;

namespace TranslateBot.DAL
{
	public class ApplicationContext : DbContext
	{
		//private readonly string _connectionString;

		private readonly ILogger<ApplicationContext> _logger;
		public DbSet<Translation> Translations { get; set; }

		public ApplicationContext(
			DbContextOptions<ApplicationContext> options,
			ILogger<ApplicationContext> logger)
			: base(options)
		{
			_logger = logger;

			try
			{
				Database.EnsureCreated();
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Ошибка при создании базы данных.");
				throw;
			}
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

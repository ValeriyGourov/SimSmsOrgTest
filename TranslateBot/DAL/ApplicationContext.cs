using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TranslateBot.DAL
{
	public class ApplicationContext : DbContext
	{
		private readonly ILogger<ApplicationContext> _logger;

		public DbSet<Models.Translation> Translations { get; set; }

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
	}
}

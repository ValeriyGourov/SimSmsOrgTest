// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.5.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using TranslateBot.DAL;
using TranslateBot.Translation;

namespace TranslateBot.Bots
{
	internal class TranslationBot : ActivityHandler
	{
		private readonly ApplicationContext _context;
		private readonly ILogger<TranslationBot> _logger;
		private readonly MicrosoftTranslator _translator;

		public TranslationBot(
			ApplicationContext context,
			ILogger<TranslationBot> logger,
			MicrosoftTranslator translator)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_translator = translator ?? throw new ArgumentNullException(nameof(translator));
		}

		protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
		{
			_logger.LogTrace(nameof(OnMessageActivityAsync));

			if (turnContext == null)
			{
				throw new ArgumentNullException(nameof(turnContext));
			}

			const string infoText = "Translating...";    // TODO: ѕри отправки кириллицы в канал текст отображаетс€ некорректно. ≈сть мнение, что это из-за кодировки проекта (?) или кодировки хостинга, на котором опубликовано приложение бота.
			await turnContext.SendActivityAsync(MessageFactory.Text(infoText), cancellationToken)
				.ConfigureAwait(false);

			string translation = await GetTranslationAsync(turnContext.Activity.Text, cancellationToken)
				.ConfigureAwait(false);
			await turnContext.SendActivityAsync(MessageFactory.Text(translation), cancellationToken)
				.ConfigureAwait(false);
		}

		protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
		{
			_logger.LogTrace(nameof(OnMembersAddedAsync));

			if (membersAdded == null)
			{
				throw new ArgumentNullException(nameof(membersAdded));
			}

			if (turnContext == null)
			{
				throw new ArgumentNullException(nameof(turnContext));
			}

			foreach (ChannelAccount member in membersAdded)
			{
				if (member.Id != turnContext.Activity.Recipient.Id)
				{
					await turnContext
						.SendActivityAsync(MessageFactory.Text($"Hello and welcome, {member.Name}!"), cancellationToken)
						.ConfigureAwait(false);
				}
			}
		}

		private async Task<string> GetTranslationAsync(string originText, CancellationToken cancellationToken)
		{
			_logger.LogTrace(nameof(GetTranslationAsync));

			string[] splittedOriginText = Regex.Split(originText, @"(?<=[.!?;])");
			IEnumerable<string> phrases = splittedOriginText
				.Where(phrase => !string.IsNullOrEmpty(phrase))
				.Select(phrase => phrase.Trim());

			Dictionary<string, string> translatedPhrases = await GetTranslatedPhrasesAsync(phrases, cancellationToken)
				.ConfigureAwait(false);
			List<string> newPhraseList = phrases
				.Except(translatedPhrases.Select(item => item.Key))
				.ToList();

			if (newPhraseList.Count > 0)
			{
				Dictionary<string, string> newPhrases = await GetNewPhraseTranslationsAsync(newPhraseList, cancellationToken)
					.ConfigureAwait(false);
				translatedPhrases = translatedPhrases
					.Concat(newPhrases)
					.ToDictionary(key => key.Key, element => element.Value);
			}

			StringBuilder phraseBuilder = new StringBuilder();
			foreach (string textPart in splittedOriginText)
			{
				string trimmedTextPart = textPart.Trim();

				string newTextPart;
				if (translatedPhrases.TryGetValue(trimmedTextPart, out string translation))
				{
					newTextPart = textPart.Replace(trimmedTextPart, translation, StringComparison.OrdinalIgnoreCase);
				}
				else
				{
					newTextPart = textPart;
				}
				phraseBuilder.Append(newTextPart);
			}

			return phraseBuilder.ToString();
		}

		private Task<Dictionary<string, string>> GetTranslatedPhrasesAsync(IEnumerable<string> phrases, CancellationToken cancellationToken)
		{
			_logger.LogTrace(nameof(GetTranslatedPhrasesAsync));

			return _context.Translations
				.Where(item => phrases.Contains(item.RussianPhrase))
				.ToDictionaryAsync(key => key.RussianPhrase, element => element.EnglishPhrase, cancellationToken);
		}

		private async Task<Dictionary<string, string>> GetNewPhraseTranslationsAsync(List<string> phrases, CancellationToken cancellationToken)
		{
			_logger.LogTrace(nameof(GetNewPhraseTranslationsAsync));

			IDictionary<string, string> phraseTranslations = await _translator
				.TranslateAsync(phrases, cancellationToken)
				.ConfigureAwait(false);

			Dictionary<string, string> newPhrases = new Dictionary<string, string>();
			List<DAL.Models.Translation> translations = new List<DAL.Models.Translation>(phrases.Count);
			foreach (string phrase in phrases)
			{
				string newTranslation = phraseTranslations[phrase];

				newPhrases.Add(phrase, newTranslation);

				translations.Add(new DAL.Models.Translation
				{
					RussianPhrase = phrase,
					EnglishPhrase = newTranslation
				});
			}

			try
			{
				await _context.Translations
					.AddRangeAsync(translations, cancellationToken)
					.ConfigureAwait(false);
				await _context.SaveChangesAsync(cancellationToken)
					.ConfigureAwait(false);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Ќе выполнена запись новых переводов в базу данных.");
				throw;
			}

			return newPhrases;
		}
	}
}

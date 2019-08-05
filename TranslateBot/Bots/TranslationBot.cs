// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.5.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;

using TranslateBot.DAL;
using TranslateBot.DAL.Models;

namespace TranslateBot.Bots
{
	internal class TranslationBot : ActivityHandler
	{
		private readonly char[] _phraseSeparators = { '.', '!', '?', ';' };
		private readonly ApplicationContext _db;

		public TranslationBot(ApplicationContext db)
		{
			_db = db;
		}

		protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
		{
			if (turnContext == null)
			{
				throw new ArgumentNullException(nameof(turnContext));
			}

			string translation = await GetTranslationAsync(turnContext.Activity.Text, cancellationToken)
				.ConfigureAwait(false);
			await turnContext.SendActivityAsync(MessageFactory.Text(translation), cancellationToken)
				.ConfigureAwait(false);
			//return turnContext.SendActivityAsync(MessageFactory.Text($"Phrases:{Environment.NewLine}{phraseCollectionBuilder.ToString()}"), cancellationToken);
			//return turnContext.SendActivityAsync(MessageFactory.Text($"Echo: {turnContext.Activity.Text}"), cancellationToken);
		}

		protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
		{
			if (membersAdded == null)
			{
				throw new ArgumentNullException(nameof(membersAdded));
			}

			if (turnContext == null)
			{
				throw new ArgumentNullException(nameof(turnContext));
			}

			foreach (var member in membersAdded)
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
			string[] phrases = originText.Split(_phraseSeparators, StringSplitOptions.RemoveEmptyEntries);

			Dictionary<string, string> translatedPhrases = await GetTranslatedPhrasesAsync(phrases, cancellationToken)
				.ConfigureAwait(false);
			List<string> newPhraseList = phrases
				.Except(translatedPhrases.Select(item => item.Key))
				.ToList();
			//Dictionary<string, string> newPhrases = phrases
			//	.Except(translatedPhrases.Select(item => item.Key))
			//	.ToDictionary(item => item, element => (string)null);

			StringBuilder phraseCollectionBuilder = new StringBuilder();

			if (translatedPhrases.Count > 0)
			{
				phraseCollectionBuilder.AppendLine("Translated phrases:");

				foreach (KeyValuePair<string, string> phrase in translatedPhrases)
				{
					phraseCollectionBuilder.AppendLine($"{phrase.Key} -> {phrase.Value}");
				}
			}

			if (newPhraseList.Count > 0)
			{
				Dictionary<string, string> newPhrases = await GetNewPhraseTranslationsAsync(newPhraseList, cancellationToken)
					.ConfigureAwait(false);

				phraseCollectionBuilder.AppendLine("New phrases:");

				foreach (KeyValuePair<string, string> phrase in newPhrases)
				{
					phraseCollectionBuilder.AppendLine($"{phrase.Key} -> {phrase.Value}");
				}
			}

			return phraseCollectionBuilder.ToString();
		}

		private Task<Dictionary<string, string>> GetTranslatedPhrasesAsync(string[] phrases, CancellationToken cancellationToken)
		{
			return _db.Translations
				.Where(item => phrases.Contains(item.RussianPhrase))
				.ToDictionaryAsync(key => key.RussianPhrase, element => element.EnglishPhrase, cancellationToken);
		}
		//private async Task<Dictionary<string, string>> GetTranslatedPhrasesAsync(string[] phrases)
		//{
		//	using (ApplicationContext db = new ApplicationContext())
		//	{
		//		return await db.Translations
		//			.Where(item => phrases.Contains(item.RussianPhrase))
		//			.ToDictionaryAsync(key => key.RussianPhrase, element => element.EnglishPhrase)
		//			.ConfigureAwait(false);
		//	}
		//}

		private async Task<Dictionary<string, string>> GetNewPhraseTranslationsAsync(List<string> phrases, CancellationToken cancellationToken)
		{
			Dictionary<string, string> newPhrases = new Dictionary<string, string>();
			List<Translation> translations = new List<Translation>(phrases.Count);
			foreach (string phrase in phrases)
			{
				string newTranslation = $"Translation {phrase}"; // TODO: Заглушка.

				newPhrases.Add(phrase, newTranslation);

				translations.Add(new Translation
				{
					RussianPhrase = phrase,
					EnglishPhrase = newTranslation
				});
			}

			await _db.Translations
				.AddRangeAsync(translations, cancellationToken)
				.ConfigureAwait(false);
			await _db.SaveChangesAsync(cancellationToken)
				.ConfigureAwait(false);
			//using (ApplicationContext db = new ApplicationContext())
			//{
			//	await db.Translations
			//		.AddRangeAsync(translations)
			//		.ConfigureAwait(false);
			//	await db.SaveChangesAsync()
			//		.ConfigureAwait(false);
			//}

			return newPhrases;
		}
		//private static async Task SetNewPhraseTranslationsAsync(Dictionary<string, string> phrases)
		//{
		//	List<Translation> translations = new List<Translation>(phrases.Count);
		//	foreach (KeyValuePair<string, string> phrase in phrases)
		//	{
		//		string newTranslation = $"Translation {phrase.Key}"; // TODO: Заглушка.
		//		phrases[phrase.Key] = newTranslation;

		//		translations.Add(new Translation
		//		{
		//			RussianPhrase = phrase.Key,
		//			EnglishPhrase = newTranslation
		//		});
		//	}

		//	using (ApplicationContext db = new ApplicationContext())
		//	{
		//		await db.Translations
		//			.AddRangeAsync(translations)
		//			.ConfigureAwait(false);
		//		await db.SaveChangesAsync()
		//			.ConfigureAwait(false);
		//	}
		//}
	}
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.5.0

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace TranslateBot.Bots
{
	public class TranslationBot : ActivityHandler
	{
		private char[] _phraseSeparators = { '.', '!', '?', ';' };

		protected override Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
		{
			if (turnContext == null)
			{
				throw new ArgumentNullException(nameof(turnContext));
			}

			string[] phrases = turnContext.Activity.Text.Split(_phraseSeparators, StringSplitOptions.RemoveEmptyEntries);
			StringBuilder phraseCollectionBuilder = new StringBuilder();
			foreach (string phrase in phrases)
			{
				phraseCollectionBuilder.AppendLine(phrase.Trim());
			}

			return turnContext.SendActivityAsync(MessageFactory.Text($"Фразы:{Environment.NewLine}{phraseCollectionBuilder.ToString()}"), cancellationToken);
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
	}
}

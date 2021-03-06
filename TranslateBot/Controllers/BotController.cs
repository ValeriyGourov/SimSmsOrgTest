// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.5.0

using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Logging;

namespace TranslateBot.Controllers
{
	// This ASP Controller is created to handle a request. Dependency Injection will provide the Adapter and IBot
	// implementation at runtime. Multiple different IBot implementations running at different endpoints can be
	// achieved by specifying a more specific type for the bot constructor argument.
	[Route("api/messages")]
	[ApiController]
	public class BotController : ControllerBase
	{
		private readonly IBotFrameworkHttpAdapter _adapter;
		private readonly IBot _bot;
		private readonly ILogger<BotController> _logger;

		public BotController(
			IBotFrameworkHttpAdapter adapter,
			IBot bot,
			ILogger<BotController> logger)
		{
			_adapter = adapter;
			_bot = bot;
			_logger = logger;
		}

		[HttpPost]
		public Task PostAsync()
		{
			_logger.LogTrace(nameof(PostAsync));

			// Delegate the processing of the HTTP POST to the adapter.
			// The adapter will invoke the bot.
			return _adapter.ProcessAsync(Request, Response, _bot);
		}
	}
}

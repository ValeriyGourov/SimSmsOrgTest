using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using TranslateBot.Translation.Model;

namespace TranslateBot.Translation
{
	internal class MicrosoftTranslator
	{
		private static readonly Uri _requestUri = new Uri("https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&from=ru&to=en");
		private static readonly HttpClient _httpClient = new HttpClient();
		private readonly string _key;
		private readonly ILogger<MicrosoftTranslator> _logger;

		public MicrosoftTranslator(
			IConfiguration configuration,
			ILogger<MicrosoftTranslator> logger)
		{
			// TODO: Для тестового задание хранение ключа в настройках сойдёт, для реального проекта нужно его хранить в более надёжном хранилище.
			_key = configuration["TranslatorKey"] ?? throw new ApplicationException("В настройках приложения не указан ключ службы перевода текстов.");
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<IDictionary<string, string>> TranslateAsync(IEnumerable<string> texts, CancellationToken cancellationToken = default)
		{
			// From Cognitive Services translation documentation:
			// https://docs.microsoft.com/en-us/azure/cognitive-services/translator/quickstart-csharp-translate

			_logger.LogTrace(nameof(TranslateAsync));

			// TODO: Реализовать следующие ограничения:
			// - Массив может содержать не более 100 элементов.
			// - Общий объем текста запроса не должен превышать 5000 символов, в том числе пробелы.

			int textsCount = texts.Count();

			List<object> body = new List<object>(textsCount);
			foreach (string text in texts)
			{
				body.Add(new { Text = text });
			}
			string requestBody = JsonConvert.SerializeObject(body);

			using (HttpRequestMessage request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = _requestUri,
				Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
			})
			{
				request.Headers.Add("Ocp-Apim-Subscription-Key", _key);

				string responseBody = null;
				using (HttpResponseMessage response = await _httpClient
					.SendAsync(request, cancellationToken)
					.ConfigureAwait(false))
				{
					responseBody = await response.Content
					   .ReadAsStringAsync()
					   .ConfigureAwait(false);
					_logger.LogInformation(responseBody);

					response.EnsureSuccessStatusCode();
				}

				TranslatorResponse[] translatorResponses = JsonConvert.DeserializeObject<TranslatorResponse[]>(responseBody);

				IDictionary<string, string> translations = new Dictionary<string, string>(textsCount);
				for (int i = 0; i < textsCount; i++)
				{
					string originText = texts.ElementAt(i);
					string translatedText = translatorResponses[i].Translations?.FirstOrDefault()?.Text;
					translations.Add(originText, translatedText);
				}

				return translations;
			}
		}
	}
}

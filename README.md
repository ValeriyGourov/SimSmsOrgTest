# SimSMS.ORG
Тестовое задание для "SimSMS.ORG"

## Описание
Простой телеграм бот, который любую фразу(слово) будет переводить с русского на английский язык. Принимает сообщение, проверяет есть ли в базе уже готовый перевод, если нет, то обращается к стороннему ресурсу для перевода, после чего сохраняет в базу(SQLite) и отвечает пользователю. Плюс небольшая таблица, на стороне ресурса, для просмотра и правки значений. Логирование ошибок через NLog.
1. Для хранения информации подключаем базу SQLite, используем EF.
2. Работа с телеграм ботом через любую dll, или своими запросами.
3. Для просмотра таблиц, используем веб-интерфейс.
4. Работа с любым кол-вом пользователей.
5. Каждый текст должен разбиваться на фразу, по знакам препинания('.', '!', '?', ';')

## Реализация
Бот реализован на базе ASP.NET Core 2 с помощью [Microsoft Bot Framework](https://dev.botframework.com/) и [Служба Bot (Azure Bot Service)](https://azure.microsoft.com/ru-ru/services/bot-service/). Функции перевода текстов реализованы через [Перевод текстов (Microsoft Translator)](https://azure.microsoft.com/ru-ru/services/cognitive-services/translator-text-api/). Для просмотра и изменения сохранённых переводов используется [Razor Pages](https://docs.microsoft.com/ru-ru/aspnet/core/razor-pages).

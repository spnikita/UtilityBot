using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    /// <summary>
    /// Контроллер нажатия кнопок
    /// </summary>
    internal sealed class InlineKeyboardController
    {
        /// <summary>
        /// Клиент к Telegram Bot API
        /// </summary>
        private readonly ITelegramBotClient _botClient;

        /// <summary>
        /// Хранилище данных сессии
        /// </summary>
        private readonly IStorage _memoryStorage;

        /// <summary>
        /// Параметризированный конструктор
        /// </summary>
        /// <param name="botClient"><inheritdoc cref="_botClient" path="/summary"/></param>
        /// <param name="memoryStorage"><inheritdoc cref="_memoryStorage" path="/summary"/></param>
        public InlineKeyboardController(ITelegramBotClient botClient, IStorage memoryStorage)
        {
            _botClient = botClient;
            _memoryStorage = memoryStorage;
        }

        /// <summary>
        /// Обработать нажатие на кнопку
        /// </summary>
        /// <param name="callbackQuery">Callback query при нажатии на кнопку</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns></returns>
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).TextMessageHandlerType = callbackQuery.Data;

            // Генерим информационное сообщение
            string languageText = callbackQuery.Data switch
            {
                "message_length" => "Выбран подсчет количества символов в сообщении",
                "numbers_sum" => "Выбрано суммирование чисел, перечисленных через пробел",
                _ => string.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await _botClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>{languageText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UtilityBot.Services;
using UtilityBot.Utilities;

namespace UtilityBot.Controllers
{
    /// <summary>
    /// Контроллер текстовых сообщений
    /// </summary>
    internal sealed class TextMessageController : MessageController
    {
        /// <summary>
        /// Хранилище данных сессии
        /// </summary>
        private readonly IStorage _memoryStorage;

        /// <summary>
        /// <inheritdoc cref="MessageController(ITelegramBotClient)" path="/summary"/>
        /// </summary>
        /// <param name="botClient"><inheritdoc cref="MessageController(ITelegramBotClient)" path="/param[@name='botClient']"/></param>
        /// <param name="memoryStorage"><inheritdoc cref="_memoryStorage" path="/summary"/></param>
        public TextMessageController(ITelegramBotClient botClient, IStorage memoryStorage) : base (botClient)
        {
            _memoryStorage = memoryStorage;
        }

        /// <inheritdoc />
        public override async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Посчитать количество символов в сообщении" , $"message_length"),
                        InlineKeyboardButton.WithCallbackData($" Вычислить сумму чисел, введенных через пробел" , $"numbers_sum")
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _botClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот обрабатывает текстовые сообщения одним из двух способов.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Можно посчитать количество символов в сообщении либо сложить числа, перечисленные через пробел.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
                default:
                    var resultMessage = _memoryStorage.GetSession(message.Chat.Id).TextMessageHandlerType switch
                    {
                        "message_length" => "Длина сообщения: " + TextUtils.CalculateMessageLength(message.Text),
                        "numbers_sum" => "Сумма чисел: " + TextUtils.SumNumbers(message.Text),
                        _ => string.Empty
                    };
                    await _botClient.SendTextMessageAsync(message.Chat.Id, resultMessage, cancellationToken: ct);
                    break;
            }
        }
    }
}

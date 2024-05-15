using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace UtilityBot.Controllers
{
    /// <summary>
    /// Контроллер текстовых сообщений
    /// </summary>
    internal sealed class TextMessageController : MessageController
    {        
        /// <inheritdoc />
        public TextMessageController(ITelegramBotClient botClient) : base (botClient)
        { }

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
                    await _botClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот превращает аудио в текст.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Можно записать сообщение и переслать другу, если лень печатать.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
                default:
                    await _botClient.SendTextMessageAsync(message.Chat.Id, "Отправьте аудио для превращения в текст.", cancellationToken: ct);
                    break;
            }
        }
    }
}

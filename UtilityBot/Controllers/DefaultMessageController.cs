using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace UtilityBot.Controllers
{
    /// <summary>
    /// Дефолтный контроллер
    /// </summary>
    internal sealed class DefaultMessageController : MessageController
    {
        /// <inheritdoc/>       
        public DefaultMessageController(ITelegramBotClient botClient) : base(botClient)
        { }
       
        /// <inheritdoc/>       
        public override async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
            await _botClient.SendTextMessageAsync(message.Chat.Id, $"Получено сообщение не поддерживаемого формата", cancellationToken: ct);
        }
    }
}

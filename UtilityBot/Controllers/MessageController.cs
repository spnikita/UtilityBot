using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace UtilityBot.Controllers
{
    internal abstract class MessageController
    {
        /// <summary>
        /// Клиент к Telegram Bot API
        /// </summary>
        protected readonly ITelegramBotClient _botClient;

        /// <summary>
        /// Параметризированный конструктор
        /// </summary>
        /// <param name="botClient"><inheritdoc cref="_botClient" path="/summary"/></param>
        protected MessageController(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        private MessageController()
        { }

        /// <summary>
        /// Обработать входящее сообщение
        /// </summary>
        /// <param name="message">Входящее сообщение</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns></returns>
        public abstract Task Handle(Message message, CancellationToken ct);
    }
}

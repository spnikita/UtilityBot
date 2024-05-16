using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot.Controllers;

namespace UtilityBot
{
    
    /// <summary>
    /// Telegram-бот
    /// </summary>
    internal class Bot : BackgroundService
    {
        /// <summary>
        /// Клиент к Telegram Bot API
        /// </summary>
        private readonly ITelegramBotClient _botClient;

        /// <summary>
        /// Контроллер нажатия кнопки
        /// </summary>
        private readonly InlineKeyboardController _inlineKeyboardController;

        /// <summary>
        /// Контроллер текстовых видов сообщений
        /// </summary>
        private readonly TextMessageController _textMessageController;      

        /// <summary>
        /// Дефолтный контроллер
        /// </summary>
        private readonly DefaultMessageController _defaultMessageController;

        public Bot(ITelegramBotClient botClient, InlineKeyboardController inlineKeyboardController, TextMessageController textMessageController, DefaultMessageController defaultMessageController)
        {
            _botClient = botClient;
            _inlineKeyboardController = inlineKeyboardController;
            _textMessageController = textMessageController;           
            _defaultMessageController = defaultMessageController;
        }

        /// <summary>
        /// Обработчик неошибочных событий
        /// </summary>
        /// <param name="botClient"><inheritdoc cref="_botClient" path="/summary"/></param>
        /// <param name="update">Событие</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns></returns>
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //  Обрабатываем нажатия на кнопки  из Telegram Bot API: https://core.telegram.org/bots/api#callbackquery
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }

            // Обрабатываем входящие сообщения из Telegram Bot API: https://core.telegram.org/bots/api#message
            if (update.Type == UpdateType.Message)
            {
                switch (update.Message!.Type)
                {                                            
                    case MessageType.Text:
                        await _textMessageController.Handle(update.Message, cancellationToken);
                        return;
                    default:
                        await _defaultMessageController.Handle(update.Message, cancellationToken);
                        return;
                }
            }
        }

        /// <summary>
        /// Обработчик ошибок
        /// </summary>
        /// <param name="botClient"><inheritdoc cref="_botClient" path="/summary"/></param>
        /// <param name="exception">Исключение</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns></returns>
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Задаем сообщение об ошибке в зависимости от того, какая именно ошибка произошла
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            // Выводим в консоль информацию об ошибке
            Console.WriteLine(errorMessage);

            // Задержка перед повторным подключением
            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>       
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, // Здесь выбираем, какие обновления хотим получать. В данном случае разрешены все
                cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен");
        }
    }
}

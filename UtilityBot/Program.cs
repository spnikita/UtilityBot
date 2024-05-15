using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace UtilityBot
{
    class Program
    {
        private const string Token = "6996726046:AAEDEgeeYdjHUnrdwbBhV5Yrhryc7bplg2U";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }

        static void ConfigureServices(IServiceCollection services)
        {

            // Подключаем контроллеры сообщений и кнопок
            services.AddTransient<DefaultMessageController>();            
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();
            // Регистрируем объект TelegramBotClient c токеном подключения
            var appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddSingleton<IFileHandler, AudioFileHandler>();
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }

        /// <summary>
        /// Получить конфигурацию сервиса
        /// </summary>
        /// <returns></returns>
        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {               
                BotToken = "6996726046:AAEDEgeeYdjHUnrdwbBhV5Yrhryc7bplg2U"               
            };
        }
    }
}

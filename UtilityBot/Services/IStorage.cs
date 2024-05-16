using UtilityBot.Models;

namespace UtilityBot.Services
{
    /// <summary>
    /// Определяет метод получения данных сессии пользователя по идентификатору
    /// </summary>
    internal interface IStorage
    {
        /// <summary>
        /// Получение данных сессии пользователя по идентификатору
        /// </summary>
        Session GetSession(long chatId);
    }
}

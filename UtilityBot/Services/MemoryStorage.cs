using System.Collections.Concurrent;
using UtilityBot.Models;

namespace UtilityBot.Services
{
    /// <summary>
    /// Хранилище данных сессии пользователей в памяти
    /// </summary>
    internal class MemoryStorage : IStorage
    {
        /// <summary>
        /// Хранилище сессий
        /// </summary>
        private readonly ConcurrentDictionary<long, Session> _sessions;

        public MemoryStorage()
        {
            _sessions = new ConcurrentDictionary<long, Session>();
        }

        /// <inheritdoc />        
        public Session GetSession(long chatId)
        {
            // Возвращаем сессию по ключу, если она существует
            if (_sessions.ContainsKey(chatId))
                return _sessions[chatId];

            // Создаем и возвращаем новую, если такой не было
            var newSession = new Session() { TextMessageHandlerType = "message_length" };
            _sessions.TryAdd(chatId, newSession);
            return newSession;
        }        
    }
}

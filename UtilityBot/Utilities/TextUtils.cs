using System.Collections.Generic;
using System.Linq;

namespace UtilityBot.Utilities
{
    /// <summary>
    /// Утилиты для работы с текстом
    /// </summary>
    internal static class TextUtils
    {
        /// <summary>
        /// Посчитать длину сообщения
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns>Длина сообщения</returns>
        public static int CalculateMessageLength(string message) => message.Length;

        /// <summary>
        /// Посчитать сумму числе в сообщении, введенныъ через пробел
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns>Сумма чисел</returns>
        public static long SumNumbers(string message)
        {
            var numbers = new List<long>();

            var numbersStr = message.Split(' ');

            foreach (var numberStr in numbersStr)
            {
                if (long.TryParse(numberStr, out var number))
                    numbers.Add(number);
            }

            return numbers.Any() ? numbers.Sum() : default;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addons
{
    /// <summary>
    /// Дополения для работы с консолью
    /// </summary>
    public static class ConsoleAdons
    {
        /// <summary>
        /// Получает с консоли целое число
        /// </summary>
        /// <param name="paramName">Имя параметра</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">
        /// Невозможно привести к целому числу
        /// </exception>
        public static int GetIntFromConsole(string paramName)
        {
            Console.WriteLine("Введите параметр '{0}'",paramName);
            var Str = Console.ReadLine();
            int NInt;
            if (Int32.TryParse(Str, out NInt))
                return NInt;
            Console.WriteLine("Ошибка:'Параметр {0} должен быть целым числом'",paramName);
            throw new InvalidOperationException();
        }
    }
}

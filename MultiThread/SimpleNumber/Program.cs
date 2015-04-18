using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Addons;

namespace SimpleNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var start = getStartRange();
                var end = getEndRange();
                if (start > end || start<1 || start == end)
                    throw new InvalidOperationException("Неверные данные относительно начала и конца диапазона");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                Console.ReadKey();
            }
            
        }

        /// <summary>
        /// Получает начало диапазона
        /// </summary>
        /// <returns></returns>
        private static int getStartRange()
        {
            try
            {
                return ConsoleAdons.GetIntFromConsole("Стартовый элемент начала диапазона");
            }
            catch (InvalidOperationException)
            {
                return getStartRange();
            }
        }

        /// <summary>
        /// Получает конец диапазона
        /// </summary>
        /// <returns></returns>
        private static int getEndRange()
        {
            try
            {
                return ConsoleAdons.GetIntFromConsole("Стартовый элемент начала диапазона");
            }
            catch (InvalidOperationException)
            {
                return getEndRange();
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Addons;

namespace SimpleNumber
{
    class Program
    {
        static IEnumerable<int> _baseNumbers;

        static void Main(string[] args)
        {
            try
            {
                var start = getStartRange();
                var end = getEndRange();
                if (start > end && start < 1 && start == end)
                    throw new InvalidOperationException("Неверные данные относительно начала и конца диапазона");
                
                var sqrtEnd = Convert.ToInt32(Math.Round(Math.Sqrt(end)));
                
                //базовые простые числа
                _baseNumbers = getBaseSimpleNumbers(2, sqrtEnd);
                var stWatch = new Stopwatch();
                
                stWatch.Restart();
                var simpleNumbers = getSimpleNumbers(sqrtEnd, end);
                stWatch.Stop();

                var time = stWatch.ElapsedMilliseconds;
                var allSimpleNumbers = _baseNumbers.Where(s => s >= start).Union(simpleNumbers).ToList();
                Console.WriteLine("Результат последовательного пересчета прав:{0}",time);

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                Console.ReadKey();
            }
            Console.ReadKey();
        }

        static IEnumerable<int> getSimpleNumbers(int sqrtEnd, int end)
        {
            var range = getRange(sqrtEnd, end).ToList();
            
            foreach(var baseNumber in _baseNumbers)
            {
                range.RemoveAll(rangeItem => rangeItem % baseNumber == 0);
            }

            return range;
        }

        /// <summary>
        /// Метод получает базовые простые числа  
        /// </summary>
        /// <param name="start">Начало диапазона</param>
        /// <param name="end">Конец диапазона</param>
        /// <returns>Простые числа</returns>
        static IEnumerable<int> getBaseSimpleNumbers(int start, int end)
        {
            if (start > end && start < 1 && start == end)
                throw new InvalidOperationException("Неверные данные относительно начала и конца диапазона");
            var range = getRange(start, end).ToList();
            var simpleNumbers = new List<int>();

            for (var i = start; range.Any(); i = range.FirstOrDefault())
            {
                simpleNumbers.Add(i);
                range.Remove(i);
                range.RemoveAll(rangeItem => rangeItem % i == 0);
            }
            return simpleNumbers;
        }

        /// <summary>
        /// Получает диапазон
        /// </summary>
        /// <param name="start">Начальный элемент</param>
        /// <param name="end">Конечный элемент</param>
        /// <returns>диапазон</returns>
        static IEnumerable<int> getRange(int start, int end)
        {
            for (var i = start; i <= end; i++)
            {
                yield return i;
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
                return ConsoleAdons.GetIntFromConsole("Элемент конца диапазона");
            }
            catch (InvalidOperationException)
            {
                return getEndRange();
            }
        }
    }
}
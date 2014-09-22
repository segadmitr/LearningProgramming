using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Task2
{
    class Program
    {
        /// <summary>
        /// Элементы которые необходимо обработать
        /// </summary>
        static List<int> s_elements;

        static void Main(string[] args)
        {
            var lenchElements = getCountElements();
            s_elements = new List<int>(lenchElements);

            var countThreads = getCountThreads();

            if (countThreads > lenchElements)
            {
                countThreads = lenchElements;
                Console.WriteLine("Количество потоков сокращено на количество элементов");
            }
            generateElements(lenchElements);

            var countElemnt = lenchElements / countThreads;
            var lost = lenchElements - countElemnt;

            var threadList = new List<Thread>();
            var startIndex = 0;
            var nextStartIndex = 0;
            var stWatch = new Stopwatch();
            stWatch.Start();

            for (var i = 0; i<countThreads; i++)
            {
                var thread = new Thread(threadWorking);
                threadList.Add(thread);
                nextStartIndex = startIndex + countElemnt;
                thread.Start(new Tuple<int, int>(startIndex, nextStartIndex));
                startIndex += nextStartIndex;
            }
            threadWorking(new Tuple<int, int>(nextStartIndex, lenchElements - 1));

            threadList.ForEach(s => s.Join());
            
            stWatch.Stop();
            Console.WriteLine("Время обработки = '{0}'", stWatch.Elapsed);
            Console.ReadKey();
        }

        private static void threadWorking(object startStopTuple)
        {
            var tuple = startStopTuple as Tuple<int, int>;
            if (tuple == null)
                throw new ArgumentException();
            var start = tuple.Item1;
            var end = tuple.Item2;
            for (var i = start; i < end; i++)
            {
                s_elements[i] = (int)Math.Pow(s_elements[i]+10, 10);
            }
        }

        /// <summary>
        /// Генерирует элементы массива
        /// </summary>
        /// <param name="lenchElements"></param>
        private static void generateElements(int lenchElements)
        {
            var rnd = new Random();
            for (var i = 0; i < lenchElements; i++)
            {
                s_elements.Add(rnd.Next(0, 100));
            }
        }        

        /// <summary>
        /// Получает количество элементов
        /// </summary>
        /// <returns></returns>
        private static int getCountElements()
        {            
            try
            {
                return getIntFromConsole("Kоличество Элементов массива");
            }
            catch (InvalidOperationException ex)
            {
                return getCountElements();
            } 
        }
        
        /// <summary>
        /// Получает количество потоков
        /// </summary>
        /// <returns></returns>
        private static int getCountThreads()
        {
            try
            {
                return getIntFromConsole("Kоличество потоков");
            }
            catch(InvalidOperationException ex)
            {
                return getCountThreads();
            } 
        }

        /// <summary>
        /// Получает с консоли целое число
        /// </summary>
        /// <param name="paramName">Имя параметра</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// Невозможно привести к целому числу
        /// </exception>
        private static int getIntFromConsole(string paramName)
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

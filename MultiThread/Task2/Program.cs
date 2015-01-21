using System;
using System.Collections.Generic;
using System.Diagnostics;
using Separators;

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
            IWorker<int> _worker = new Worker<int>();
            _worker.Separator = new RangeSeparator();
            var lenchElements = getCountElements();
            s_elements = new List<int>(lenchElements);

            var countThreads = getCountThreads();

            if (countThreads > lenchElements)
            {
                countThreads = lenchElements;
                Console.WriteLine("Количество потоков сокращено на количество элементов");
            }
            generateElements(lenchElements);

            var stWatch = new Stopwatch();
           
            stWatch.Start();
            
            _worker.CountThreads = countThreads;
            _worker.Calculate(s_elements, (s,index) =>(int) Math.Pow(s + 10, 10));
            
            stWatch.Stop();
            Console.WriteLine("Время обработки = '{0}'", stWatch.Elapsed);
            Console.ReadKey();
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
            catch (InvalidOperationException)
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
            catch(InvalidOperationException)
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

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
        static List<int> elements;

        static void Main(string[] args)
        {
            var lenchElements = getN();
            elements = new List<int>(lenchElements);

            var countThreads = getM();

            if (countThreads > lenchElements)
            {
                countThreads = lenchElements;
                Console.WriteLine("Количество потоков сокращено на количество элементов");
            }
            generateElements(lenchElements);

            var countElemnt = lenchElements / countThreads;
            var lost = lenchElements - countElemnt;

            List<Thread> threadList = new List<Thread>();
            var startIndex = 0;
           
            for (var i = 0; i<countThreads; i++)
            {
                var thread = new Thread(threadWorking);
                threadList.Add(thread);
                var nextStartIndex = startIndex + countElemnt;
                thread.Start(new Tuple<int, int>(startIndex, nextStartIndex));
                startIndex += nextStartIndex;
            }    

            var stWatch = new Stopwatch();
            stWatch.Start();
            elements.ForEach(s => s = (int)Math.Pow(s+10, 10));
            stWatch.Stop();
            Console.WriteLine("Время обработки = '{0}'", stWatch.Elapsed);
            Console.ReadKey();
        }

        private static void threadWorking(object obj)
        {
            throw new NotImplementedException();
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
                elements.Add(rnd.Next(0, 100));
            }
        }        

        /// <summary>
        /// Получает количество элементов
        /// </summary>
        /// <returns></returns>
        private static int getN()
        {            
            try
            {
                return getIntFromConsole("Kоличество Элементов массива");
            }
            catch (InvalidOperationException ex)
            {
                return getM();
            } 
        }
        
        /// <summary>
        /// Получает количество потоков
        /// </summary>
        /// <returns></returns>
        private static int getM()
        {
            try
            {
                return getIntFromConsole("Kоличество потоков");
            }
            catch(InvalidOperationException ex)
            {
                return getM();
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

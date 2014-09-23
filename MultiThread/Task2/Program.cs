using System;
using System.Collections.Generic;
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

            var stWatch = new Stopwatch();
           
            stWatch.Start();

            culateArray(lenchElements, countThreads);
            
            stWatch.Stop();
            Console.WriteLine("Время обработки = '{0}'", stWatch.Elapsed);
            Console.ReadKey();
        }

        static void culateArray(int lenchElements, int countThreads)
        {
            //Количество элементов для одного потока
            var lench4Tread = lenchElements / countThreads;
            //Количество нераспределенных элементов
            var lost = lenchElements - (lench4Tread*countThreads);

            var threadList = new List<Thread>();
            
            //инициализируем переменные индексов
            var curIndex = 0;
            var nextIndex = (curIndex + lench4Tread)-1;
            
            //начинаем отслеживать быстодействие

            for (var i = 1; i<countThreads; i++)
            {
                var thread = new Thread(threadWorking);
                threadList.Add(thread);
                thread.Start(new Tuple<int, int>(curIndex, nextIndex));
                if (curIndex >= lenchElements - 1)
                {
                    break;
                }
                curIndex = nextIndex + 1;
                nextIndex = curIndex + lench4Tread;
            }
            if (lost > 0)
            {
                threadWorking(new Tuple<int, int>(curIndex, lenchElements - 1));
            }
            
            //ожидаем завершение всех потоков
            threadList.ForEach(s => s.Join());
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

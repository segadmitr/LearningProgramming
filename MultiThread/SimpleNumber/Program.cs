using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Addons;
using Separators;

namespace SimpleNumber
{
    class Program
    {
        static IEnumerable<int> _baseNumbers;

        [ThreadStatic]
        static List<int> s_threadlist;

        
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

                #region Последовательный пересчет прав
                
                stWatch.Restart();
                var simpleNumbers = getSimpleNumbers(sqrtEnd, end);
                stWatch.Stop();

                Console.WriteLine("Результат последовательного пересчета прав:{0}", stWatch.ElapsedMilliseconds);

                #endregion

                #region Параллельный алгоритм №1: декомпозиция по данным

                IWorker<int> _worker = new Worker<int>();
                _worker.Separator = new RangeSeparator();
                _worker.CountThreads = _baseNumbers.Count();
                
                stWatch.Restart();
                var sourceList = getRange(sqrtEnd, end).ToList();
                var simpleNumbers2 = _worker.Calculate(sourceList, (s) =>
                    {
                        var list = s.ToList();
                        foreach (var baseNumber in _baseNumbers)
                        {
                            list.RemoveAll(rangeItem => rangeItem % baseNumber == 0);
                        }
                        return list;
                    });
                stWatch.Stop();

                Console.WriteLine("Параллельный алгоритм №1: декомпозиция по данным результат:{0}", stWatch.ElapsedMilliseconds);
                
                #endregion 

                #region Параллельный алгоритм №2: декомпозиция набора простых чисел
                IWorker<int> worker2 = new Worker<int>();
                worker2.Separator = new RangeSeparator();
                worker2.CountThreads = _baseNumbers.Count();
                var baseNumbers2 = _baseNumbers.ToList();
                
                stWatch.Restart();
                var sourceList2 = getRange(sqrtEnd, end).ToList();

                //объект для блокировки
                var loced = "Lock";
                
                worker2.Calculate(baseNumbers2, (baseNumber, index) =>
                {
                    (s_threadlist = new List<int>()).AddRange(sourceList2);
                    var forDel = s_threadlist.Where(rangeItem => rangeItem % baseNumber == 0).ToList();
                    lock (loced)
                    {
                        forDel.ForEach(s => sourceList2.Remove(s));
                    }
                });
                
                stWatch.Stop();

                Console.WriteLine("Параллельный алгоритм №2: декомпозиция набора простых чисел:{0}", stWatch.ElapsedMilliseconds);
                #endregion

                #region Параллельный алгоритм №3: применение пула потоков
                
                stWatch.Restart();
                var sourceList3 = getRange(sqrtEnd, end).ToList();
                var events = new List<ManualResetEvent>();
                WaitCallback action = objectParam =>
                    {
                        var baseNumber = (int) ((object[]) objectParam)[0];
                        var resetEvent = (ManualResetEvent)((object[])objectParam)[1];
                        (s_threadlist = new List<int>()).AddRange(sourceList3);
                        var forDel = s_threadlist.Where(rangeItem => rangeItem % baseNumber == 0).ToList();
                        lock (loced)
                        {
                            forDel.ForEach(s => sourceList3.Remove(s));
                        }
                        resetEvent.Set();
                    };
                

                foreach (var baseNumber in _baseNumbers)
                {
                    var manualEvent = new ManualResetEvent(false);
                    events.Add(manualEvent);
                    ThreadPool.QueueUserWorkItem(action, new object[] { baseNumber, manualEvent });
                }
                WaitHandle.WaitAll(events.ToArray());
                stWatch.Stop();

                Console.WriteLine("Параллельный алгоритм №3: применение пула потоков:{0}", stWatch.ElapsedMilliseconds);
                #endregion

                #region Validation

                if (simpleNumbers.Count() != simpleNumbers2.Count()
                    && sourceList2.Count() != simpleNumbers.Count()
                    && sourceList3.Count() != simpleNumbers.Count())
                    throw new ArgumentNullException();
                
                #endregion

                var allSimpleNumbers = _baseNumbers.Where(s => s >= start).Union(simpleNumbers).ToList();

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
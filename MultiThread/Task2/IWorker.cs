using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task2
{
    /// <summary>
    /// Интерфейс обработчика
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWorker<T>
    {
        /// <summary>
        /// Расчитать
        /// </summary>
        void Calculate(List<T> calculatedParam, Func<T,int,T> doWithParamItem);

        /// <summary>
        /// Количество потоков 
        /// </summary>
        int CountThreads { get; set; }

        /// <summary>
        /// Делитель диапазона
        /// </summary>
        ISeparator Separator { get; set; }
    }

    /// <summary>
    ///Обработчик элементов 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Worker<T> : IWorker<T>
    {
        List<T> _calculatedParam = new List<T>();
        Func<T,int, T> _doWithParamItem;
        int _countThreads;

        public void Calculate(List<T> calculatedParam, Func<T,int,T> doWithParamItem)
        {
            _calculatedParam = calculatedParam;
            _doWithParamItem = doWithParamItem;
        }

        void culateArray(int lenchElements, int countThreads)
        {
            //Количество элементов для одного потока
            var lench4Tread = lenchElements / countThreads;
            //Количество нераспределенных элементов
            var lost = lenchElements - (lench4Tread * countThreads);

            var threadList = new List<Thread>();

            //инициализируем переменные индексов
            var curIndex = 0;
            var nextIndex = (curIndex + lench4Tread) - 1;

            for (var i = 0; i < countThreads; i++)
            {
                var thread = new Thread(threadWorking);
                threadList.Add(thread);
                thread.Start(new Tuple<int, int>(curIndex, nextIndex));
                if (curIndex >= lenchElements - 1)
                {
                    break;
                }
                curIndex = nextIndex + 1;
                nextIndex = (curIndex + lench4Tread) - 1;
            }
            if (lost > 0)
            {
                threadWorking(new Tuple<int, int>(curIndex, lenchElements - 1));
            }

            //ожидаем завершение всех потоков
            threadList.ForEach(s => s.Join());
        }

        /// <summary>
        /// обработка для одного потока
        /// </summary>
        /// <param name="startStopTuple"></param>
        private  void threadWorking(object startStopTuple)
        {
            var tuple = startStopTuple as Tuple<int, int>;
            if (tuple == null)
                throw new ArgumentException();
            var start = tuple.Item1;
            var end = tuple.Item2;
            for (var i = start; i < end; i++)
            {
                _calculatedParam[i] = _doWithParamItem(_calculatedParam[i],i);
            }
        }

        public int CountThreads
        {
            get { return (_countThreads<0)?1:_countThreads; }
            set { _countThreads  = value; }
        }

        public ISeparator Separator { get; set; }
    }
}

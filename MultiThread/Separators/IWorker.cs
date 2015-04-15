using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Separators
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

        //TODO List<T> calculatedParam переделать на IEnumerable
        public void Calculate(List<T> calculatedParam, Func<T,int,T> doWithParamItem)
        {
            _calculatedParam = calculatedParam;
            _doWithParamItem = doWithParamItem;
            calculate(_calculatedParam.Count(), CountThreads);
        }

        private void calculate(int lenchElements, int countThreads)
        {
            var separateResult = Separator.Separate(lenchElements, countThreads);
            var threadList = new List<Thread>();
            foreach (var sepItemResult in separateResult)
            {
                var thread = new Thread(threadWorking);
                threadList.Add(thread);
                thread.Start(sepItemResult);
            }

            threadList.ForEach(s => s.Join());
        }

        /// <summary>
        /// обработка для одного потока
        /// </summary>
        private void threadWorking(object indexesParam)
        {
            var indexes = indexesParam as IEnumerable<int>;
            if (indexes == null)
                throw new ArgumentException();

            foreach (var itemIndex in indexes)
            {
                _calculatedParam[itemIndex] = _doWithParamItem(_calculatedParam[itemIndex], itemIndex);
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

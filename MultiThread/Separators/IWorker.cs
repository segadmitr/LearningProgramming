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
        /// Рассчитать и модифицировать элемент списка
        /// </summary>
        /// <param name="calculatedParam">Все данные</param>
        /// <param name="doWithParamItem">Модификатор элемента списка</param>
        void Calculate(List<T> calculatedParam, Func<T,int,T> doWithParamItem);
        
        /// <summary>
        /// Рассчитать разделенную часть данных
        /// </summary>
        /// <param name="calculatedParam">Все данные</param>
        /// <param name="doWithSeparatedPart">Разделенная часть данных</param>
        void Calculate(List<T> calculatedParam, Func<IEnumerable<int>, IEnumerable<T>> doWithSeparatedPart);

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
        int _countThreads;

        #region Модификация элемента списка
        
        /// <summary>
        /// Делегат обработки элемента
        /// </summary>
        Func<T, int, T> _doWithParamItem;

        //TODO List<T> calculatedParam переделать на IEnumerable
        public void Calculate(List<T> calculatedParam, Func<T, int, T> doWithParamItem)
        {
            _calculatedParam = calculatedParam;
            _doWithParamItem = doWithParamItem;
            calculate(_calculatedParam.Count(), CountThreads, threadWorkForModItem);
        }

        /// <summary>
        /// Модификация элементов для одного потока
        /// </summary>
        private void threadWorkForModItem(object indexesParam)
        {
            var indexes = indexesParam as IEnumerable<int>;
            if (indexes == null)
                throw new ArgumentException();

            foreach (var itemIndex in indexes)
            {
                _calculatedParam[itemIndex] = _doWithParamItem(_calculatedParam[itemIndex], itemIndex);
            }
        }

        #endregion

        #region Расчет части  списка, разделеннного делителем

        Func<IEnumerable<int>, IEnumerable<T>> _doWithSeparatedPart;

        public void Calculate(List<T> calculatedParam, Func<IEnumerable<int>, IEnumerable<T>> doWithSeparatedPart)
        {
            _calculatedParam = calculatedParam;
            _doWithSeparatedPart = doWithSeparatedPart;
            calculate(_calculatedParam.Count(), CountThreads, threadWorkForSepList);
        }

        /// <summary>
        /// Обработка элементов одного потока
        /// </summary>
        /// <param name="indexesParam"></param>
        void threadWorkForSepList(object indexesParam)
        {
            var indexes = indexesParam as IEnumerable<int>;
            if (indexes == null)
                throw new ArgumentException();
            var d = _doWithSeparatedPart(indexes);
        }

        #endregion

        private void calculate(int lenchElements, int countThreads, ParameterizedThreadStart threadStartMethod)
        {
            var separateResult = Separator.Separate(lenchElements, countThreads);
            var threadList = new List<Thread>();
            foreach (var sepItemResult in separateResult)
            {
                var thread = new Thread(threadStartMethod);
                threadList.Add(thread);
                thread.Start(sepItemResult);
            }

            threadList.ForEach(s => s.Join());
        }

        public int CountThreads
        {
            get { return (_countThreads<0)?1:_countThreads; }
            set { _countThreads  = value; }
        }

        public ISeparator Separator { get; set; }
    }
}

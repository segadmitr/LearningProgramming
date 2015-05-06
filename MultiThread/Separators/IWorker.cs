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
        List<T> Calculate(List<T> calculatedParam, Func<IEnumerable<T>, IEnumerable<T>> doWithSeparatedPart);

        /// <summary>
        /// Действие с элементом списка
        /// </summary>
        /// <param name="calculatedParam"></param>
        /// <param name="actionWithParamItem"></param>
        void Calculate(List<T> calculatedParam, Action<T, int> actionWithParamItem);

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
        /// <summary>
        /// Список элементов который надо преобразовать
        ///  или расчитать в нескольких потоках
        /// </summary>
        List<T> _calculatedParam = new List<T>();
        
        int _countThreads;

        #region Модификация элемента списка
        
        /// <summary>
        /// Делегат обработки элемента
        /// </summary>
        Func<T, int, T> _doWithParamItem;

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
        
        /// <summary>
        /// делегат расчета одной части
        /// </summary>
        Func<IEnumerable<T>, IEnumerable<T>> _doWithSeparatedPart;
        
        /// <summary>
        /// Результат расчета всех частей
        /// </summary>
        List<T> _result = new List<T>();
        //TODO сделать синхронизацию добавления 
        /// <summary>
        /// Список блокировок для синхронизации
        /// </summary>
        List<int> _locedList = new List<int>();

        public List<T> Calculate(List<T> calculatedParam, Func<IEnumerable<T>, IEnumerable<T>> doWithSeparatedPart)
        {
            _calculatedParam = calculatedParam;
            _doWithSeparatedPart = doWithSeparatedPart;
            calculate(_calculatedParam.Count(), CountThreads, threadWorkForSepList);
            return _result;
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
            var indexesResult = indexes.Select(index => _calculatedParam[index]);
            var separatedPartResult = _doWithSeparatedPart(indexesResult);
            lock (_locedList)
            {
                _result.AddRange(separatedPartResult);
            }
        }

        #endregion

        #region Расчет части  списка, разделеннного делителем

        Action<T, int> _actionWithParamItem;

        public void Calculate(List<T> calculatedParam, Action<T, int> actionWithParamItem)
        {
            _calculatedParam = calculatedParam;
            _actionWithParamItem = actionWithParamItem;
            calculate(_calculatedParam.Count(), CountThreads, threadWorkForActionItem);
        }

        /// <summary>
        /// Действие для элементов одного потока
        /// </summary>
        /// <param name="indexesParam"></param>
        void threadWorkForActionItem(object indexesParam)
        {
            var indexes = indexesParam as IEnumerable<int>;
            if (indexes == null)
                throw new ArgumentException();

            foreach (var itemIndex in indexes)
            {
                _actionWithParamItem(_calculatedParam[itemIndex], itemIndex);
            }
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

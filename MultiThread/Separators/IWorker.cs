using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Separators
{
    /// <summary>
    /// ��������� �����������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWorker<T>
    {
        /// <summary>
        /// ���������� � �������������� ������� ������
        /// </summary>
        /// <param name="calculatedParam">��� ������</param>
        /// <param name="doWithParamItem">����������� �������� ������</param>
        void Calculate(List<T> calculatedParam, Func<T,int,T> doWithParamItem);
        
        /// <summary>
        /// ���������� ����������� ����� ������
        /// </summary>
        /// <param name="calculatedParam">��� ������</param>
        /// <param name="doWithSeparatedPart">����������� ����� ������</param>
        List<T> Calculate(List<T> calculatedParam, Func<IEnumerable<T>, IEnumerable<T>> doWithSeparatedPart);

        /// <summary>
        /// �������� � ��������� ������
        /// </summary>
        /// <param name="calculatedParam"></param>
        /// <param name="actionWithParamItem"></param>
        void Calculate(List<T> calculatedParam, Action<T, int> actionWithParamItem);

        /// <summary>
        /// ���������� ������� 
        /// </summary>
        int CountThreads { get; set; }

        /// <summary>
        /// �������� ���������
        /// </summary>
        ISeparator Separator { get; set; }
    }

    /// <summary>
    ///���������� ��������� 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Worker<T> : IWorker<T>
    {
        /// <summary>
        /// ������ ��������� ������� ���� �������������
        ///  ��� ��������� � ���������� �������
        /// </summary>
        List<T> _calculatedParam = new List<T>();
        
        int _countThreads;

        #region ����������� �������� ������
        
        /// <summary>
        /// ������� ��������� ��������
        /// </summary>
        Func<T, int, T> _doWithParamItem;

        public void Calculate(List<T> calculatedParam, Func<T, int, T> doWithParamItem)
        {
            _calculatedParam = calculatedParam;
            _doWithParamItem = doWithParamItem;
            calculate(_calculatedParam.Count(), CountThreads, threadWorkForModItem);
        }

        /// <summary>
        /// ����������� ��������� ��� ������ ������
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

        #region ������ �����  ������, ������������� ���������
        
        /// <summary>
        /// ������� ������� ����� �����
        /// </summary>
        Func<IEnumerable<T>, IEnumerable<T>> _doWithSeparatedPart;
        
        /// <summary>
        /// ��������� ������� ���� ������
        /// </summary>
        List<T> _result = new List<T>();
        //TODO ������� ������������� ���������� 
        /// <summary>
        /// ������ ���������� ��� �������������
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
        /// ��������� ��������� ������ ������
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

        #region ������ �����  ������, ������������� ���������

        Action<T, int> _actionWithParamItem;

        public void Calculate(List<T> calculatedParam, Action<T, int> actionWithParamItem)
        {
            _calculatedParam = calculatedParam;
            _actionWithParamItem = actionWithParamItem;
            calculate(_calculatedParam.Count(), CountThreads, threadWorkForActionItem);
        }

        /// <summary>
        /// �������� ��� ��������� ������ ������
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

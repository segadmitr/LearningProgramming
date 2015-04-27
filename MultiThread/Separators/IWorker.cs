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
        void Calculate(List<T> calculatedParam, Func<IEnumerable<int>, IEnumerable<T>> doWithSeparatedPart);

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
        List<T> _calculatedParam = new List<T>();
        int _countThreads;

        #region ����������� �������� ������
        
        /// <summary>
        /// ������� ��������� ��������
        /// </summary>
        Func<T, int, T> _doWithParamItem;

        //TODO List<T> calculatedParam ���������� �� IEnumerable
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

        Func<IEnumerable<int>, IEnumerable<T>> _doWithSeparatedPart;

        public void Calculate(List<T> calculatedParam, Func<IEnumerable<int>, IEnumerable<T>> doWithSeparatedPart)
        {
            _calculatedParam = calculatedParam;
            _doWithSeparatedPart = doWithSeparatedPart;
            calculate(_calculatedParam.Count(), CountThreads, threadWorkForSepList);
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

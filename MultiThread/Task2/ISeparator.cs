using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    /// <summary>
    /// интерфейс делителя вектора
    /// </summary>
    public interface ISeparator : IEnumerable<IEnumerable<int>>
    {
        /// <summary>
        /// Находит индексы частей при делениии векора на части
        /// </summary>
        /// <param name="countElementsInArray">Количество элементов в массиве</param>
        /// <param name="countParts">Количество частей</param>
        void Separate(int countElementsInArray, int countParts);
    }

    public class RangeSeparator : ISeparator
    {
        int _lench4Part = 0;
        int _lost = 0;
        int _countElementsInArray = 0;
        int _countParts = 0;
        int _startIndex = 0;
        int _endIndex = 0;

        public void  Separate(int countElementsInArray, int countParts)
        {
            _countElementsInArray = countElementsInArray;
            _countParts = countParts;

            //Количество элементов в одной части
            _lench4Part = _countElementsInArray / _countParts;
            //Количество нераспределенных элементов
            _lost = _countElementsInArray - (_lench4Part * _countParts);
        }

        public IEnumerator<IEnumerable<int>> GetEnumerator()
        {
           _startIndex = 0;
           _endIndex = (_startIndex + _lench4Part) -1;
           for (var i = 0; i < _countParts; i++)
           {
               yield return new RangeEnumerator(_startIndex,_endIndex);
               _startIndex = _endIndex+1;
               _endIndex = (_startIndex + _lench4Part);
           }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class RangeEnumerator : IEnumerable<int>
    {
        readonly int _startIndex;
        readonly int _endIndex;

        public RangeEnumerator(int startIndex, int endIndex)
        {
            _startIndex = startIndex;
            _endIndex = endIndex;
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (var i = _startIndex; i < _endIndex; ++i)
            {
                yield return i;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    internal class RoundSepator:ISeparator
    {
        public void Separate(int countElementsInArray, int countParts)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IEnumerable<int>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

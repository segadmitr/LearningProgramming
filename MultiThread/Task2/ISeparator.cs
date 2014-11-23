using System.Collections;
using System.Collections.Generic;

namespace Task2
{
    /// <summary>
    /// Интерфейс делителя вектора
    /// </summary>
    public interface ISeparator 
    {
        /// <summary>
        /// Находит индексы частей при делениии векора на части
        /// </summary>
        /// <param name="countElementsInArray">Количество элементов в массиве</param>
        /// <param name="countParts">Количество частей</param>
        IEnumerable<IEnumerable<int>> Separate(int countElementsInArray, int countParts);
    }

    /// <summary>
    /// Декомпозитор по диапазону
    /// </summary>
    public class RangeSeparator : ISeparator
    {
        int _lench4Part = 0;
        int _lost = 0;
        int _countElementsInArray = 0;
        int _countParts = 0;
        int _startIndex = 0;
        int _endIndex = 0;

        public IEnumerable<IEnumerable<int>> Separate(int countElementsInArray, int countParts)
        {
            _countElementsInArray = countElementsInArray;
            _countParts = countParts;

            //Количество элементов в одной части
            _lench4Part = _countElementsInArray / _countParts;
            //Количество нераспределенных элементов
            _lost = _countElementsInArray - (_lench4Part * _countParts);
            if (_lench4Part == 1)
                _endIndex = 1;
            else _endIndex = _lench4Part;

            return GetEnumerator();
        }

        IEnumerable<IEnumerable<int>> GetEnumerator()
        {
            for (var i = 0; i < _countParts; i++)
            {
                yield return new RangeEnumerator(_startIndex, _endIndex);
                _startIndex = _endIndex;
                _endIndex = _startIndex + _lench4Part;
            }
            if (_lost > 0)
                yield return new RangeEnumerator(_startIndex, _countElementsInArray);
        }
    }

    public class RangeEnumerator : IEnumerable<int>
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


    /// <summary>
    /// Круговой декомпозитор
    /// </summary>
    public class RoundSeparator : ISeparator
    {
        int _countElementsInArray;
        int _countParts;
        int _lench4Part;
        int _lost;
        int _startIndex;

        public IEnumerable<IEnumerable<int>> Separate(int countElementsInArray, int countParts)
        {
            _countElementsInArray = countElementsInArray;
            _countParts = countParts;

            //Количество элементов в одной части
            _lench4Part = _countElementsInArray/_countParts;
            //Количество нераспределенных элементов
            _lost = _countElementsInArray - (_lench4Part*_countParts);
            
            return GetEnumerator();
        }

        public IEnumerable<IEnumerable<int>> GetEnumerator()
        {
            for (var i = 0; i < _countParts; i++)
            {
                yield return new RoundEnumerator(i, _lench4Part);
            }
        }

        public class RoundEnumerator : IEnumerable<int>
        {
            int _currIndex;
            readonly int _lench4Part;

            public RoundEnumerator(int startIndex,  int lench4Part)
            {
                _currIndex = startIndex;
                _lench4Part = lench4Part;
            }

            public IEnumerator<int> GetEnumerator()
            {
                for (var i = 0; i < _lench4Part; i++)
                {
                    yield return _currIndex;
                    _currIndex += _lench4Part;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}

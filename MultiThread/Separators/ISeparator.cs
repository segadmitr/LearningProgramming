using System.Collections;
using System.Collections.Generic;

namespace Separators
{
    /// <summary>
    /// ��������� �������� �������
    /// </summary>
    public interface ISeparator 
    {
        /// <summary>
        /// ������� ������� ������ ��� �������� ������ �� �����
        /// </summary>
        /// <param name="countElementsInArray">���������� ��������� � �������</param>
        /// <param name="countParts">���������� ������</param>
        IEnumerable<IEnumerable<int>> Separate(int countElementsInArray, int countParts);
    }

    /// <summary>
    /// ������������ �� ���������
    /// </summary>
    public class RangeSeparator : ISeparator
    {
        #region private

        /// <summary>
        /// ����������� ���������� ��������� � ����� �����
        /// </summary>
        int _minLench4Part;

        /// <summary>
        /// ������������ ���������� ��������� � ����� �����
        /// </summary>
        int _maxLench4Part;
        
        /// <summary>
        /// ���������� ���������� ���������������� ���������
        /// </summary>
        int _lost;
        
        /// <summary>
        /// ���������� �������������� ���������
        /// </summary>
        int _countElementsInArray;

        /// <summary>
        /// ���������� ������
        /// </summary>
        int _countParts;

        /// <summary>
        /// ��������� ������ ��� ������� �����
        /// </summary>
        int _curPartStartIndex;
        
        /// <summary>
        /// �������� ������ ��� ������� �����
        /// </summary>
        int _curPartEndIndex;

        #endregion

        public IEnumerable<IEnumerable<int>> Separate(int countElementsInArray, int countParts)
        {
            _countElementsInArray = countElementsInArray;
            _countParts = countParts;
            
            _minLench4Part = _countElementsInArray / _countParts;
            _lost = _countElementsInArray - (_minLench4Part * _countParts);
            if (_lost > 0)
                _maxLench4Part = _minLench4Part + 1;
            else
                _maxLench4Part = _minLench4Part;

            if (_minLench4Part == 1)
                _curPartEndIndex = 1;
            else _curPartEndIndex = length4CurrentPart;

            return GetEnumerator();
        }

        IEnumerable<IEnumerable<int>> GetEnumerator()
        {
            for (var i = 0; i < _countParts; i++)
            {
                yield return new RangeEnumerator(_curPartStartIndex, _curPartEndIndex);
                _lost--;
                _curPartStartIndex = _curPartEndIndex;
                _curPartEndIndex = _curPartStartIndex + length4CurrentPart;
            }
            if (_lost > 0)
                yield return new RangeEnumerator(_curPartStartIndex, _countElementsInArray);
        }
        
        int length4CurrentPart 
        {
            get
            {
                return _lost > 0 ? _maxLench4Part : _minLench4Part;
            }
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
    /// �������� ������������
    /// </summary>
    public class RoundSeparator : ISeparator
    {
        /// <summary>
        /// ���������� �������������� ���������
        /// </summary>
        int _countElementsInArray;
        
        /// <summary>
        /// ���������� ������
        /// </summary>
        int _countParts;

        /// <summary>
        /// ����������� ���������� ��������� � ����� �����
        /// </summary>
        int _minLench4Part;

        /// <summary>
        /// ������������ ���������� ��������� � ����� �����
        /// </summary>
        int _maxLench4Part;
        
        /// <summary>
        /// ���������� ���������� ���������������� ���������
        /// </summary>
        int _lost;

        public IEnumerable<IEnumerable<int>> Separate(int countElementsInArray, int countParts)
        {
            _countElementsInArray = countElementsInArray;
            _countParts = countParts;
            _minLench4Part = _countElementsInArray / _countParts;
            _lost = _countElementsInArray - (_minLench4Part * _countParts);
            if (_lost > 0)
                _maxLench4Part = _minLench4Part + 1;
            else
                _maxLench4Part = _minLench4Part;
            
            return GetEnumerator();
        }

        public IEnumerable<IEnumerable<int>> GetEnumerator()
        {
            for (var i = 0; i < _countParts; i++)
            {
                yield return new RoundEnumerator(i, length4CurrentPart, _countParts);
                _lost--;
            }
        }

        int length4CurrentPart
        {
            get
            {
                return _lost > 0 ? _maxLench4Part : _minLench4Part;
            }
        }

        public class RoundEnumerator : IEnumerable<int>
        {
            int _currIndex;
            readonly int _lench4Part;
            readonly int _countParts;

            public RoundEnumerator(int startIndex, int lench4Part, int countParts)
            {
                _currIndex = startIndex;
                _lench4Part = lench4Part;
                _countParts = countParts;
            }

            public IEnumerator<int> GetEnumerator()
            {
                for (var i = 0; i < _lench4Part; i++)
                {
                    yield return _currIndex;
                    _currIndex += _countParts;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}

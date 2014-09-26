using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    /// <summary>
    /// интерфейс делителя вектора
    /// </summary>
    public interface ISeparator
    {
        /// <summary>
        /// Находит индексы частей при делениии векора на части
        /// </summary>
        /// <param name="countElementsInArray">Количество элементов в массиве</param>
        /// <param name="countParts">Количество частей</param>
        IEnumerable<IEnumerable<int>>Separate(int countElementsInArray, int countParts);
    }

    internal class RangeSeparator : ISeparator
    {
        public IEnumerable<IEnumerable<int>> Separate(int countElementsInArray, int countParts)
        {
            //Количество элементов для одного потока
            var lench4Tread = lenchElements / countThreads;
            //Количество нераспределенных элементов
            var lost = lenchElements - (lench4Tread * countThreads);
            var startIndex = 0;
            var nextStartIndex = 0;
        }
    }

    internal class RoundSepator:ISeparator
    {
        public IEnumerable<IEnumerable<int>> Separate(int countElementsInArray, int countParts)
        {
            throw new NotImplementedException();
        }
    }
}

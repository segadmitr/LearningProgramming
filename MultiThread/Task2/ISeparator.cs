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
        List<List<int>>Separate(int countElementsInArray, int countParts);
    }

    internal class RangeSeparator : ISeparator
    {
        public List<List<int>> Separate(int countElementsInArray, int countParts)
        {
            throw new NotImplementedException();
        }
    }

    internal class RoundSepator:ISeparator
    {
        public List<List<int>> Separate(int countElementsInArray, int countParts)
        {
            throw new NotImplementedException();
        }
    }
}

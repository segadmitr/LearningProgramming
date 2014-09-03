using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MultiThread
{
    class Program
    {
        static void Main(string[] args)
        {            
            var lench = getN();
            var elements = new List<int>(lench);
            var rnd = new Random();
            for (var i = 0; i < lench; i++)
            {
                elements.Add(rnd.Next(0, 100));
            }            
            var stWatch = new Stopwatch();
            stWatch.Start();
            elements.ForEach(s => Math.Pow(s, 10)); 
            stWatch.Stop();
            Console.WriteLine("Время обработки = '{0}'",stWatch.Elapsed);
            Console.ReadKey();
        }

        private static int getN()
        {
            Console.WriteLine("Введите количество элементов массива:");
            var NStr = Console.ReadLine();
            int NInt;
            if (Int32.TryParse(NStr, out NInt))
                return NInt;
            Console.WriteLine("Ошибка:'Количество элементов массива должно быть целым числом'");
            return getN();
        }
    }
}

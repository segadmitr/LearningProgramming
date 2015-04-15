using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Separators;

namespace Task3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region private
        
        /// <summary>
        /// Длины элементов
        /// </summary>
        readonly List<int> _lengthsElements = new List<int> { 10, 100,1000,10000};

        /// <summary>
        /// Количества потоков
        /// </summary>
        readonly List<int> _countsThreads = new List<int> {1, 3, 4, 5, 10};

        /// <summary>
        /// Сгенерированные значения элементов
        /// </summary>
        List<int> _generatedElements = new List<int>();
        
        /// <summary>
        /// Результат пересчета
        /// </summary>
        ObservableCollection<ResultForThreads> _resultCalculation = new ObservableCollection<ResultForThreads>();

        const string c_PropertyPrefix = "P";
        
        /// <summary>
        /// Порог генерации
        /// </summary>
        const int c_maxGen = 100;

        #endregion
        
        public MainWindow()
        {
            
            InitializeComponent();

            generateElements(_lengthsElements.Max());

            //определяем динамические колонки
            foreach (var countsThread in _countsThreads)
            {
                var column = new DataGridTextColumn {Header = countsThread};
                var pathForBind = c_PropertyPrefix + countsThread;
                column.Binding = new Binding {Path = new PropertyPath(pathForBind, new object[0])};
                ResultDGrd.Columns.Add(column);
            }
        }

        /// <summary> 
        /// Генерирует элементы массива
        /// </summary>
        /// <param name="lenchElements"></param>
        private  void generateElements(int lenchElements)
        {
            var rnd = new Random();
            for (var i = 0; i < lenchElements; i++)
            {
                _generatedElements.Add(rnd.Next(0, c_maxGen));
            }
        }


        private void calculate()
        {
            var worker = new Worker<int> {Separator = new RangeSeparator()};
            var stWatch = new Stopwatch();

            foreach (var length in _lengthsElements)
            {
                var result = new ResultForThreads(length);

                var elements = _generatedElements.Take(length).ToList();
                foreach (var countThreads in _countsThreads)
                {
                    worker.CountThreads = countThreads;
                    
                    stWatch.Restart();
                    worker.Calculate(elements, hightCalc);
                    stWatch.Stop();

                    var elapsed = stWatch.Elapsed;
                    result.TrySetMember(new MemberBinder(c_PropertyPrefix + countThreads, false),elapsed.TotalMilliseconds);
                }
                ResultCalculation.Add(result);
            }
        }

        int hightCalc(int arg1, int arg2)
        {
            double sum = 0;
            for (var i = 0; i < arg2; i++)
            {
                sum +=Math.Pow(arg1, 1.789);
            }
            return 0;
        }

        /// <summary>
        /// Результат пересчета
        /// </summary>
        public ObservableCollection<ResultForThreads> ResultCalculation
        {
            get { return _resultCalculation; }
            set { _resultCalculation = value; }
        }

        /// <summary>
        /// Длины элементов
        /// </summary>
        public List<int> LengthsElements 
        {
            get { return _lengthsElements; }
        }
        
        /// <summary>
        /// Количества потоков
        /// </summary>
        public List<int> СountsThreads 
        {
            get { return _countsThreads; }
        }

        private void RunBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultCalculation.Clear();
            calculate();
        }
                
    }
}

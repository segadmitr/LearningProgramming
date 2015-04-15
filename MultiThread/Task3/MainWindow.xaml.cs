using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        readonly List<int> _lengthsElements = new List<int> { 10, 100, 1000, 100000 };
        
        /// <summary>
        /// Количества потоков
        /// </summary>
        readonly List<int> _countsThreads = new List<int> { 2, 3, 4, 5, 10 };

        /// <summary>
        /// Сгенерированные значения элементов
        /// </summary>
        List<int> _generatedElements = new List<int>();
        
        #endregion
        
        public MainWindow()
        {
            InitializeComponent();
            generateElements(_lengthsElements.Max());
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
                _generatedElements.Add(rnd.Next(0, 100));
            }
        }


        private void calculate()
        {
            foreach (var length in _lengthsElements)
            {
                var elements = _generatedElements.Take(length);
                foreach (var countThreads in _countsThreads)
                {
                }
            }
        }



        private void RunBtn_Click(object sender, RoutedEventArgs e)
        {

        }
                
    }
}

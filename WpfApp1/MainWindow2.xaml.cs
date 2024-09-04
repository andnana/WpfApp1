using LiveCharts.Wpf;
using LiveCharts;
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

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public MainWindow2()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Line Series",
                    Values = new ChartValues<double> { 0, 2, 1, 6, 5, 10 },
                    PointGeometry = null, // 使点不可见
                    Stroke = Brushes.Blue,
                    StrokeThickness = 3
                }
            };

            DataContext = this;
        }
    }
}

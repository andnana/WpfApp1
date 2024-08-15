using HandyControl.Controls;
using LiveCharts;
using LiveCharts.Wpf;
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
using System.Windows.Shapes;
using MessageBox = HandyControl.Controls.MessageBox;
namespace WpfApp1
{
    /// <summary>
    /// MessageBoxWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxWindow : System.Windows.Window
    {

        public SeriesCollection SeriesCollection2 { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string Name1 { get; set; } = "bbb";

        public MessageBoxWindow()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
             new LineSeries
                {
                    Title = "Line Series",
                    Values = new ChartValues<double> { 0, 200, 1000, 6000, 5899,  6100, 6111,  6000, 5666,  5999, 5888, 6055, 6122,  6001, 5889 },
                    PointGeometry = null, // 使点不可见
                    Stroke = Brushes.Silver,
                    StrokeThickness = 1,
                    Fill =Brushes.LightSkyBlue,
                   
                }
            };

            SeriesCollection2 = new SeriesCollection
        {
            new ColumnSeries
            {
                Title = "光强",
                Values = new ChartValues<double> { 700 },
                Fill =Brushes.LightSkyBlue

            }
       /*     ,
            new ColumnSeries
            {
                Title = "Series 2",
                Values = new ChartValues<double> { 1, 6, 4, 9 }
            }*/
        };

            this.DataContext = this;

        }

        private void SuccessAction(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void FailAction(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("失败", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        /*  private void Button_Click(object sender, RoutedEventArgs e)
          {
              Growl.Success("显示一条通知！");
          }*/
    /*    private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ImageBrowser(new Uri("pack://application:,,,/Resources/ImageBlock.png")).Show();
        }*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Screenshot().Start();
        }
        private void CloudPlatform(object sender, RoutedEventArgs e)
        {
            new CloudPlatform().Show();
        }
      private void DeviceSetup(object sender, RoutedEventArgs e)
        {
            new DeviceSetup().Show();
        }
      private void HistoryData(object sender, RoutedEventArgs e)
        {
            new HistoryData().Show();
        }
        private void AlarmHistory(object sender, RoutedEventArgs e)
        {
            new AlarmHistory().Show();
        }

        private void About(object sender, RoutedEventArgs e)
        {
            new About().Show();
        }
    }
}

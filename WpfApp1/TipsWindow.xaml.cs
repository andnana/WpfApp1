using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace WpfApp1
{


    /// <summary>
    /// TipsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TipsWindow : Window
    {
        public int Result { get; private set; }
        private DispatcherTimer timer;
        string msg = "";
        public TipsWindow(string msg, int interval, TipsEnum icon)
        {
            InitializeComponent();
            try
            {
                this.msg = msg;
                messageTextBlock.Text = msg;
                if (icon == TipsEnum.OK)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/ok_white.png", UriKind.Relative);
                    bitmapImage.EndInit();
                    TipsIcon.Source = bitmapImage;
                }
                else if (icon == TipsEnum.FAIL)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/fail_white.png", UriKind.Relative);
                    bitmapImage.EndInit();
                    TipsIcon.Source = bitmapImage;
                }
                else if (icon == TipsEnum.INFO)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/info.png", UriKind.Relative);
                    bitmapImage.EndInit();
                    TipsIcon.Source = bitmapImage;
                }
                timer = new DispatcherTimer();
                timer.Tick += Timer_Tick;
                timer.Interval = TimeSpan.FromSeconds(interval); // 设置时间为5秒
                timer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("aaaaa");
                Console.WriteLine(ex.Message);
            }
    
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            this.Close(); // 关闭窗口
        }


    }
}

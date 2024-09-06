using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace WpfApp1
{
    /// <summary>
    /// DeviceSetup.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceSetup : System.Windows.Window
    {
        public DeviceSetup()
        {
            InitializeComponent();
            TitleBar.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    DragMove();
            };

            BtMin.Click += (s, e) =>
            {
                WindowState = WindowState.Minimized;
            };

            //BtMax.Click += (s, e) =>
            //{
            //    WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            //};

            BtClose.Click += (s, e) =>
            {
                Close();
            };
            deviceNameLabel.Content = MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].Device_name;
            IPLabel.Content = MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].IP;
            deviceNumLabel.Content = MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].deviceNum;
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="s">发送内容</param>
        private void Send(string s)
        {
            /*            if (!CHCNetSDK.NET_DVR_SerialSend(Main_Form.real_PlayPOJOs[Main_Form.Chosen_device_num].ISerialHandle, 1, s, (uint)s.Length))
                        {
                            MessageBox.Show("发送失败" + CHCNetSDK.NET_DVR_GetLastError());
                        }
            */

            MainWindow.In_Main_Form.Tcp_Send(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].deviceNum, Encoding.Default.GetBytes(s));
            Thread.Sleep(3);
            MainWindow.In_Main_Form.Tcp_Send(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].deviceNum, Encoding.Default.GetBytes(s));
            Thread.Sleep(4);
            MainWindow.In_Main_Form.Tcp_Send(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].deviceNum, Encoding.Default.GetBytes(s));
            Thread.Sleep(3);
            MainWindow.In_Main_Form.Tcp_Send(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].deviceNum, Encoding.Default.GetBytes(s));
            Thread.Sleep(4);
            MainWindow.In_Main_Form.Tcp_Send(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].deviceNum, Encoding.Default.GetBytes(s));
        }
      

        private void openIndicatingLaser(object sender, EventArgs e)
        {
            Send("@lgk@");
        }

        private void closeIndicatingLaser(object sender, EventArgs e)
        {
            Send("@lgg@");
        }
        private void startDetector(object sender, EventArgs e)
        {
            Send("@start@");
        }

        private void restartDetector(object sender, EventArgs e)
        {
            Send("@stop@");
            Thread.Sleep(5000);
            Send("@start@");
        }

        private void closeDetector(object sender, EventArgs e)
        {
            Send("@stop@");
            MainWindow.real_PlayPOJOs[MainWindow.choose_device_num].work_if = false;
        }

        private void calibration(object sender, EventArgs e)
        {
            if (Password.Password.Equals("check"))
            {
                Send("@jaozheng@");
            }
            else
            {
                Growl.SuccessGlobal("密码错误。");
            }
        }
    }
}

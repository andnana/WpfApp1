using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using static WpfApp1.CHCNetSDK;

namespace WpfApp1
{
    /// <summary>
    /// AlarmHistoryDetail.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmHistoryDetail : System.Windows.Window
    {
        public static string videoPath = "";
        int index = 0;
        History_Message historyMessage = new History_Message();
        public AlarmHistoryDetail(int index)
        {

            InitializeComponent();
            this.index = index;
            TitleBar.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    DragMove();
            };

            BtClose.Click += (s, e) =>
            {
                Close();
            };


            ResourceDictionary resourceDictionary;
            string languageStr = ConfigurationManager.AppSettings["Language"];
            if (languageStr.Equals("english"))
            {
                string english = "pack://application:,,,/Language/English.xaml";
                resourceDictionary = new ResourceDictionary { Source = new Uri(english, UriKind.RelativeOrAbsolute) };

            }
            else
            {
                string chinese = "pack://application:,,,/Language/Chinese.xaml";
                resourceDictionary = new ResourceDictionary { Source = new Uri(chinese, UriKind.RelativeOrAbsolute) };


            }


            // 将当前的资源字典从应用程序资源中移除
            Resources.MergedDictionaries.Remove(resourceDictionary);
            // 将新的资源字典添加到应用程序资源中
            Resources.MergedDictionaries.Add(resourceDictionary);


            historyMessage = MainWindow.historyMessages[index];
            deviceIP.Status = historyMessage.device_IP.ToString();
            deviceName.Status = historyMessage.device_name.ToString();
            saveTime.Status = historyMessage.save_time;
            concentration.Status = historyMessage.concentration.ToString();
            horizontalAngle.Status = historyMessage.Horiz.ToString();
            verticalAngle.Status = historyMessage.Vert.ToString();
            presetPoint.Status = historyMessage.Preset_num.ToString();
            presetPointNotes.Status = historyMessage.Preset_notes.ToString();


       

        }
        private void seeVideo(object sender, RoutedEventArgs e)
        {
            videoPath = historyMessage.video_path + ".wmv";
            new Video(videoPath).Show();
        }

        /// <summary>
        /// 调用预置点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invokePreset(object sender, RoutedEventArgs e)
        {
            if (MainWindow.sbmc == "")
            {
                new TipsWindow("请先登录", 3, TipsEnum.FAIL).Show();
                return;
            }
            NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)CHCNetSDK.GOTO_PRESET, (uint)historyMessage.Preset_num);
        }
    }
}

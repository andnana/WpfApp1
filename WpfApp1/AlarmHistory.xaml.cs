using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static WpfApp1.CHCNetSDK;
namespace WpfApp1
{
    /// <summary>
    /// AlarmHistory.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmHistory : System.Windows.Window
    {
        public static string videoPath = "";
        public AlarmHistory()
        {
            InitializeComponent();
            //DataContext = new AlarmHistoryModelView();
            AlarmHistoryDataGrid.ItemsSource = MainWindow.historyMessages;
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


        }

        private void seeVideo(object sender, RoutedEventArgs e)
        {
            History_Message rowView = (History_Message)((Button)e.Source).DataContext;
            videoPath = rowView.video_path + ".wmv";
            new Video().Show();
        }

        /// <summary>
        /// 调用预置点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invokePreset(object sender, RoutedEventArgs e)
        {
            History_Message rowView = (History_Message)((Button)e.Source).DataContext;
            NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)CHCNetSDK.GOTO_PRESET, (uint)rowView.Preset_num);
        }

        private void remove(object sender, RoutedEventArgs e)
        {
            History_Message rowView = (History_Message)((Button)e.Source).DataContext;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "HistoryMessages.xml");
            XmlElement xe = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
            string strPath = string.Format("/historymessage/message[@save_time=\"{0}\"]", rowView.save_time.ToString("yyyy-MM-dd HH:mm:ss"));
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
            selectXe.ParentNode.RemoveChild(selectXe);
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "HistoryMessages.xml");
            Growl.SuccessGlobal("删除成功！");
        }

        private void refreshData(object sender, RoutedEventArgs e)
        {
            List<History_Message> historyMessagesTemp = new List<History_Message>();
            for (int i = 0; i < MainWindow.historyMessages.Count; i++)
            {
                string dateTimeStr = MainWindow.historyMessages[i].save_time.ToString("yyyy-MM-dd HH:mm:ss");
                string format = "yyyy-MM-dd HH:mm:ss";
                DateTime dateTime = DateTime.ParseExact(dateTimeStr, format, CultureInfo.CurrentCulture);
                //if (historyMessages[i].SaveTime)
                Console.WriteLine("dataTime:" + dateTime.ToString());
                if (DatePicker1.SelectedDate < dateTime && DatePicker2.SelectedDate > dateTime)
                {
                    historyMessagesTemp.Add(MainWindow.historyMessages[i]);
                }
                AlarmHistoryDataGrid.ItemsSource = historyMessagesTemp;
                AlarmHistoryDataGrid.Items.Refresh();
            }
        }

        private DataGridRow GetDataGridRow(Button button)
        {
            // 由于DataGrid不是VisualTree的一部分，所以需要使用ItemContainerGenerator来获取容器
            var container = button.TemplatedParent as ContentPresenter;
            if (container != null)
            {
                var item = container.Content;
                var generator = AlarmHistoryDataGrid.ItemContainerGenerator;
                if (generator.Status == GeneratorStatus.ContainersGenerated)
                {
                    var row = generator.ContainerFromItem(item) as DataGridRow;
                    return row;
                }
            }
            return null;
        }

    }
}

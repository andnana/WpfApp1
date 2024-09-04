using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Xml;

namespace WpfApp1
{

    /// <summary>
    /// HistoryData.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryData : Window
    {

        public static List<UsualhistoryMessage> usualhistoryMessages = new List<UsualhistoryMessage>();

        public HistoryData()
        {
            InitializeComponent();

            LoadUsualHistoryMessageFile();




            //this.DataContext = new DeviceModelView();
            HistoryMessageDataGrid.ItemsSource = usualhistoryMessages;

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

        private void refreshData(object sender, RoutedEventArgs e)
        {
            List<UsualhistoryMessage> usualhistoryMessagesTemp = new List<UsualhistoryMessage>();
            for (int i = 0; i < usualhistoryMessages.Count; i++)
            {
                string dateTimeStr = usualhistoryMessages[i].SaveTime;
                string format = "yyyy-MM-dd HH:mm:ss";
                DateTime dateTime = DateTime.ParseExact(dateTimeStr, format, CultureInfo.CurrentCulture);
                //if (usualhistoryMessages[i].SaveTime)
                Console.WriteLine("dataTime:" + dateTime.ToString());
                if(DatePicker1.SelectedDate < dateTime && DatePicker2.SelectedDate > dateTime)
                {
                    usualhistoryMessagesTemp.Add(usualhistoryMessages[i]);
                }
                HistoryMessageDataGrid.ItemsSource = usualhistoryMessagesTemp;
                HistoryMessageDataGrid.Items.Refresh();
            }
        }
        public static void LoadUsualHistoryMessageFile()
        {

            // 创建XmlDDocument对象，并装入xml文件
            XmlDocument xmlDoc = new XmlDocument();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "UsualHistoryMessage.xml", settings);
            xmlDoc.Load(reader);

            //xn 代表一个结点
            //xn.Name;//这个结点的名称
            //xn.Value;//这个结点的值
            //xn.ChildNodes;//这个结点的所有子结点
            //xn.ParentNode;//这个结点的父结点

            // 得到根节点bookstore
            XmlNode xn = xmlDoc.SelectSingleNode("usualhistorymessage");


            // 得到根节点的所有子节点
            XmlNodeList xnl = xn.ChildNodes;

            foreach (XmlNode xn1 in xnl)
            {
                UsualhistoryMessage usualhistoryMessage = new UsualhistoryMessage();
                // 将节点转换为元素，便于得到节点的属性值
                XmlElement xe = (XmlElement)xn1;
                // 得到Type和ISBN两个属性的属性值
                //bookModel.BookISBN = xe.GetAttribute("ISBN").ToString();
                //bookModel.BookType = xe.GetAttribute("Type").ToString();
                // 得到LoginInfo节点的所有子节点
                XmlNodeList xnl0 = xe.ChildNodes;
                usualhistoryMessage.Pid = xnl0.Item(0).InnerText;
                usualhistoryMessage.DeviceIp = xnl0.Item(1).InnerText;
                usualhistoryMessage.DeviceName = xnl0.Item(2).InnerText;
                usualhistoryMessage.SaveTime = xnl0.Item(3).InnerText;
                usualhistoryMessages.Add(usualhistoryMessage);
            }
            reader.Close();
        }


    }

}

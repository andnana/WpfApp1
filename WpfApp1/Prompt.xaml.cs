using HandyControl.Controls;
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
using System.Xml;

namespace WpfApp1
{
    /// <summary>
    /// Prompt.xaml 的交互逻辑
    /// </summary>
    public partial class Prompt : System.Windows.Window
    {
        public Prompt()
        {
            InitializeComponent();
            TitleBar.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    DragMove();
            };



            BtClose.Click += (s, e) =>
            {
                Close();
            };
        }
        private void Yes(Object sender, RoutedEventArgs e)
        {

            History_Message rowView = (History_Message)((Button)e.Source).DataContext;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "HistoryMessages.xml");
            XmlElement xe = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
            string strPath = string.Format("/historymessage/message[@save_time=\"{0}\"]", rowView.save_time.ToString("yyyy-MM-dd HH:mm:ss"));
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
            selectXe.ParentNode.RemoveChild(selectXe);
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "HistoryMessages.xml");

            int removeIndex = MainWindow.historyMessages.FindIndex(item => item.save_time.ToString("yyyy-MM-dd HH:mm:ss").Equals(rowView.save_time.ToString("yyyy-MM-dd HH:mm:ss")));
            MainWindow.historyMessages.RemoveAt(removeIndex);
            Growl.SuccessGlobal("删除成功！");
            this.Close();
        }
        private void No(Object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

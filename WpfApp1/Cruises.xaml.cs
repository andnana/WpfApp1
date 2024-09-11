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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace WpfApp1
{
    /// <summary>
    /// Cruises.xaml 的交互逻辑
    /// </summary>
    public partial class Cruises : System.Windows.Window
    {
        public List<Preset> presets = new List<Preset>();
        public List<CruisePOJO> cruises = new List<CruisePOJO>();
        public Cruises()
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
            LoadPresetsFile();
            LoadCruisesFile(MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip);
            CruisesDataGrid.ItemsSource = cruises;


        }

        private void remove(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("确认删除？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information);

            if (result == MessageBoxResult.OK)
            {
                CruisePOJO rowView = (CruisePOJO)((System.Windows.Controls.Button)e.Source).DataContext;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_cruises.xml");
                XmlElement xe = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
                string strPath = string.Format("/cruises/cruise[@save_time=\"{0}\"]", rowView.timeStr.ToString());
                XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
                selectXe.ParentNode.RemoveChild(selectXe);
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_cruises.xml");

                int removeIndex = cruises.FindIndex(item => item.timeStr.ToString().Equals(rowView.timeStr.ToString()));
                Console.WriteLine("----------");
                Console.WriteLine(removeIndex.ToString());
                cruises.RemoveAt(removeIndex);
                CruisesDataGrid.Items.Refresh();
                Growl.SuccessGlobal("删除成功！");
            }
        }


        private void CruiseDetail(object sender, RoutedEventArgs e)
        {
            CruisePOJO rowView = (CruisePOJO)((System.Windows.Controls.Button)e.Source).DataContext;
            

            new CruiseDetail(rowView).Show();
        }
        private void AddCruise(object sender, RoutedEventArgs e)
        {

            XmlDocument doc = new XmlDocument();

            try
            {
                //doc.LoadXml("<bookstore></bookstore>");//用这句话,会把以前的数据全部覆盖掉,只有你增加的数据
                doc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_cruises.xml");
                XmlNode root = doc.SelectSingleNode("cruises");


                XmlElement xelCruise = doc.CreateElement("cruise");
                string nowDateTimeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                XmlAttribute xelSaveTime = doc.CreateAttribute("save_time");
                xelSaveTime.InnerText = nowDateTimeStr;
                xelCruise.SetAttributeNode(xelSaveTime);


                XmlAttribute xelNotesAttr = doc.CreateAttribute("notes");
                xelNotesAttr.InnerText = NotesText.Text;
                xelCruise.SetAttributeNode(xelNotesAttr);

                CruisePOJO cruise = new CruisePOJO();
                cruise.timeStr = nowDateTimeStr;
                cruise.notes = NotesText.Text;

                root.AppendChild(xelCruise);

                doc.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_cruises.xml");
                Growl.SuccessGlobal("巡航信息保存成功！");
                cruises.Add(cruise);
                CruisesDataGrid.Items.Refresh();

            }
            catch (Exception e1)
            {
                Console.WriteLine(e.ToString());
            }




        }
        public void LoadCruisesFile(string fileNamePrefix)
        {
            try {

                // 创建XmlDDocument对象，并装入xml文件
                XmlDocument xmlDoc = new XmlDocument();

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + fileNamePrefix + "_cruises.xml", settings);
                xmlDoc.Load(reader);

                //xn 代表一个结点
                //xn.Name;//这个结点的名称
                //xn.Value;//这个结点的值
                //xn.ChildNodes;//这个结点的所有子结点
                //xn.ParentNode;//这个结点的父结点

                // 得到根节点bookstore
                XmlNode xn = xmlDoc.SelectSingleNode("cruises");


                // 得到根节点的所有子节点
                XmlNodeList xnl = xn.ChildNodes;

                foreach (XmlNode xn1 in xnl)
                {
                    CruisePOJO cruise = new CruisePOJO();
                    // 将节点转换为元素，便于得到节点的属性值
                    XmlElement xe = (XmlElement)xn1;
                    // 得到Type和ISBN两个属性的属性值
                    //bookModel.BookISBN = xe.GetAttribute("ISBN").ToString();
                    //bookModel.BookType = xe.GetAttribute("Type").ToString();


                    cruise.timeStr = xe.GetAttribute("save_time").ToString();
                    cruise.notes = xe.GetAttribute("notes").ToString();

                    cruises.Add(cruise);
                }

                reader.Close();
            }
            catch(Exception e)
            {
                //创建一个空的XML
                XmlDocument document = new XmlDocument();
                //声明头部
                XmlDeclaration dec = document.CreateXmlDeclaration("1.0", "utf-8", "yes");
                document.AppendChild(dec);

                //创建根节点
                XmlElement root = document.CreateElement("cruises");
                document.AppendChild(root);


                //保存文档
                document.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_cruises.xml");


                Console.WriteLine(e.Message);
            }

        }

        public void LoadPresetsFile()
        {

            // 创建XmlDDocument对象，并装入xml文件
            XmlDocument xmlDoc = new XmlDocument();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_presets.xml", settings);
            xmlDoc.Load(reader);

            //xn 代表一个结点
            //xn.Name;//这个结点的名称
            //xn.Value;//这个结点的值
            //xn.ChildNodes;//这个结点的所有子结点
            //xn.ParentNode;//这个结点的父结点

            // 得到根节点bookstore
            XmlNode xn = xmlDoc.SelectSingleNode("presets");


            // 得到根节点的所有子节点
            XmlNodeList xnl = xn.ChildNodes;

            foreach (XmlNode xn1 in xnl)
            {
                Preset preset = new Preset();
                // 将节点转换为元素，便于得到节点的属性值
                XmlElement xe = (XmlElement)xn1;
                // 得到Type和ISBN两个属性的属性值
                //bookModel.BookISBN = xe.GetAttribute("ISBN").ToString();
                //bookModel.BookType = xe.GetAttribute("Type").ToString();
                // 得到LoginInfo节点的所有子节点
                XmlNodeList xnl0 = xe.ChildNodes;
                preset.preset_num = int.Parse(xnl0.Item(0).InnerText);
                preset.notes = xnl0.Item(1).InnerText;
                presets.Add(preset);
            }

            reader.Close();
        }
    }
}

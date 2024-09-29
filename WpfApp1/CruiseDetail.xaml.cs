using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.IO;
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
using static NPOI.XSSF.UserModel.Helpers.ColumnHelper;

namespace WpfApp1
{
    /// <summary>
    /// CruiseDetail.xaml 的交互逻辑
    /// </summary>
    public partial class CruiseDetail : System.Windows.Window
    {
        CruisePOJO cruisePOJO;
        public List<Preset> presets = new List<Preset>();
        public List<Preset> cruisesPresets = new List<Preset>();
        public CruiseDetail(CruisePOJO cruisePOJO)
        {

            InitializeComponent();

            initCruisesFile();

            this.cruisePOJO = cruisePOJO;
            NotesLabel.Content = cruisePOJO.notes;
            SaveTimeLabel.Content = cruisePOJO.timeStr;


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
            for (int i = 0; i < presets.Count; i++)
            {
                PresetsCombobox.Items.Add(presets[i].notes);
            }
            LoadCruisesPresets(MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip);
            PresetsDataGrid.ItemsSource = cruisesPresets;



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
        public void LoadCruisesPresets(string fileNamePrefix)
        {

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

            /*           // 得到根节点bookstore
                       XmlNode xn = xmlDoc.SelectSingleNode("cruises");


                       // 得到根节点的所有子节点
                       XmlNodeList xnl = xn.ChildNodes;

                       foreach (XmlNode xn1 in xnl)
                       {

                           XmlElement xe = (XmlElement)xn1;
                           if (cruisePOJO.timeStr.Equals(xe.GetAttribute("save_time").ToString())){

                               XmlNodeList presetNodeList = xn1.ChildNodes;

                               foreach (XmlNode xn2 in presetNodeList)
                               {
                                   Preset preset = new Preset();
                                   XmlElement xmlPreset = (XmlElement)xn2;
                                   XmlNodeList xnl0 = xmlPreset.ChildNodes;
                                   preset.preset_num = int.Parse(xnl0.Item(0).InnerText);
                                   preset.notes = xnl0.Item(1).InnerText;
                                   cruisesPresets.Add(preset);

                               }
                           }




                       }*/
            XmlNode xn = xmlDoc.SelectSingleNode("cruises");
            XmlElement xe = xmlDoc.DocumentElement;
            string strPath = string.Format("/cruises/cruise[@save_time=\"{0}\"]", cruisePOJO.timeStr);
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
            XmlNodeList presetNodeList = selectXe.ChildNodes;

            foreach (XmlNode xn2 in presetNodeList)
            {
                Preset preset = new Preset();
                XmlElement xmlPreset = (XmlElement)xn2;
                XmlNodeList xnl0 = xmlPreset.ChildNodes;
                preset.preset_num = int.Parse(xnl0.Item(2).InnerText);
                preset.notes = xnl0.Item(8).InnerText;
                cruisesPresets.Add(preset);

            }
            PresetsDataGrid.ItemsSource = cruisesPresets;
            reader.Close();
        }

        private void remove(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("确认删除？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information);

            if (result == MessageBoxResult.OK)
            {
                Preset rowView = (Preset)((Button)e.Source).DataContext;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_cruises.xml");
                XmlElement xe = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
                string strPath = string.Format("/cruises/cruise[@save_time=\"{0}\"]/preset[@preset_num=\"{1}\"]", cruisePOJO.timeStr, rowView.preset_num.ToString());
                XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
                selectXe.ParentNode.RemoveChild(selectXe);
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_cruises.xml");

                int removeIndex = cruisesPresets.FindIndex(item => item.preset_num.ToString().Equals(rowView.preset_num.ToString()));
                cruisesPresets.RemoveAt(removeIndex);
                PresetsDataGrid.Items.Refresh();
                new TipsWindow("删除成功", 3, TipsEnum.OK).Show();
            }
        }
        private void addAllPreset(object sender, EventArgs e)
        {


            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                //doc.LoadXml("<bookstore></bookstore>");//用这句话,会把以前的数据全部覆盖掉,只有你增加的数据
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_cruises.xml");

                XmlElement xe = xmlDoc.DocumentElement;
                string strPath = string.Format("/cruises/cruise[@save_time=\"{0}\"]", cruisePOJO.timeStr);
                XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.

                for(int i = 0; i < presets.Count; i++)
                {

                    XmlElement xelPreset = xmlDoc.CreateElement("preset");


                    XmlAttribute xelPresetNumAttr = xmlDoc.CreateAttribute("preset_num");
                    xelPresetNumAttr.InnerText = presets[i].preset_num.ToString();
                    xelPreset.SetAttributeNode(xelPresetNumAttr);

                    XmlElement xelIsCurrent = xmlDoc.CreateElement("isCurrent");
                    xelIsCurrent.InnerText = "False";
                    xelPreset.AppendChild(xelIsCurrent);

                    XmlElement xelImagePath = xmlDoc.CreateElement("imagePath");
                    xelImagePath.InnerText = "";
                    xelPreset.AppendChild(xelImagePath);

                    XmlElement xelPresetNum = xmlDoc.CreateElement("preset_num");
                    xelPresetNum.InnerText = presets[i].preset_num.ToString();
                    xelPreset.AppendChild(xelPresetNum);

                    XmlElement xelTime = xmlDoc.CreateElement("time");
                    xelTime.InnerText = (TimeCombobox.SelectedIndex + 2).ToString();
                    xelPreset.AppendChild(xelTime);

                    XmlElement xelName = xmlDoc.CreateElement("name");
                    xelName.InnerText = "";
                    xelPreset.AppendChild(xelName);

                    XmlElement xelSpeed = xmlDoc.CreateElement("speed");
                    xelSpeed.InnerText = presets[i].speed.ToString();
                    xelPreset.AppendChild(xelSpeed);

                    XmlElement xelTimeStr = xmlDoc.CreateElement("timeStr");
                    xelTimeStr.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    xelPreset.AppendChild(xelTimeStr);

                    XmlElement xelSpeedStr = xmlDoc.CreateElement("speedStr");
                    xelSpeedStr.InnerText = "";
                    xelPreset.AppendChild(xelSpeedStr);

                    XmlElement xelNotes = xmlDoc.CreateElement("notes");
                    xelNotes.InnerText = presets[i].notes;
                    xelPreset.AppendChild(xelNotes);

                    cruisesPresets.Add(presets[i]);
                    selectXe.AppendChild(xelPreset);
                }


             

                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_cruises.xml");
                
                PresetsDataGrid.Items.Refresh();

            }
            catch (FileNotFoundException e2)
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


                Console.WriteLine(e2.Message);

            }
            catch (Exception e3)
            {
                Console.WriteLine(e3.ToString());
            }

        }

        private void initCruisesFile()
        {
            try
            {
                // 创建XmlDDocument对象，并装入xml文件
                XmlDocument xmlDoc = new XmlDocument();

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_cruises.xml", settings);
                //xmlDoc.Load(reader);
                reader.Close();
            }
            catch (FileNotFoundException e)
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

        private void AddPreset(object sender, RoutedEventArgs e)
        {

            int presetIndex = PresetsCombobox.SelectedIndex;
            Preset preset = presets[presetIndex];



            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                //doc.LoadXml("<bookstore></bookstore>");//用这句话,会把以前的数据全部覆盖掉,只有你增加的数据
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_cruises.xml");

                XmlElement xe = xmlDoc.DocumentElement;
                string strPath = string.Format("/cruises/cruise[@save_time=\"{0}\"]", cruisePOJO.timeStr);
                XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.



                XmlElement xelPreset = xmlDoc.CreateElement("preset");


                XmlAttribute xelPresetNumAttr = xmlDoc.CreateAttribute("preset_num");
                xelPresetNumAttr.InnerText = preset.preset_num.ToString();
                xelPreset.SetAttributeNode(xelPresetNumAttr);

                XmlElement xelIsCurrent = xmlDoc.CreateElement("isCurrent");
                xelIsCurrent.InnerText = "False";
                xelPreset.AppendChild(xelIsCurrent);

                XmlElement xelImagePath = xmlDoc.CreateElement("imagePath");
                xelImagePath.InnerText = "";
                xelPreset.AppendChild(xelImagePath);

                XmlElement xelPresetNum = xmlDoc.CreateElement("preset_num");
                xelPresetNum.InnerText = preset.preset_num.ToString();
                xelPreset.AppendChild(xelPresetNum);

                XmlElement xelTime = xmlDoc.CreateElement("time");
                xelTime.InnerText = (TimeCombobox.SelectedIndex + 2).ToString();
                xelPreset.AppendChild(xelTime);

                XmlElement xelName = xmlDoc.CreateElement("name");
                xelName.InnerText = "";
                xelPreset.AppendChild(xelName);

                XmlElement xelSpeed = xmlDoc.CreateElement("speed");
                xelSpeed.InnerText = preset.speed.ToString();
                xelPreset.AppendChild(xelSpeed);

                XmlElement xelTimeStr = xmlDoc.CreateElement("timeStr");
                xelTimeStr.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                xelPreset.AppendChild(xelTimeStr);

                XmlElement xelSpeedStr = xmlDoc.CreateElement("speedStr");
                xelSpeedStr.InnerText = "";
                xelPreset.AppendChild(xelSpeedStr);

                XmlElement xelNotes = xmlDoc.CreateElement("notes");
                xelNotes.InnerText = preset.notes;
                xelPreset.AppendChild(xelNotes);



                selectXe.AppendChild(xelPreset);

                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_cruises.xml");
                cruisesPresets.Add(preset);
                PresetsDataGrid.Items.Refresh();

            }
            catch (FileNotFoundException e2)
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


                Console.WriteLine(e2.Message);

            }
            catch (Exception e3)
            {
                Console.WriteLine(e3.ToString());
            }







        }


    }
}

using HandyControl.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using MessageBox = HandyControl.Controls.MessageBox;
using System.Xml;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Configuration;
namespace WpfApp1
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : System.Windows.Window
    {

        public static List<LoginInfo> loginInfoList = new List<LoginInfo>();

        /// <summary>
        /// 登录信息
        /// </summary>
        public static LoginPOJO[] Login_info;

        /// <summary>
        /// 保存登录信息
        /// </summary>
        public static LoginPOJO[] Save_login_info;

        /// <summary>
        /// 当前登录信息
        /// </summary>
        public static LoginPOJO Once_login_info;

        /// <summary>
        /// 保存登录信息
        /// </summary>
        private static bool Save_if;


        ObservableCollection<string> device_name_str_list = new ObservableCollection<string>();
        ObservableCollection<string> device_ip_str_list = new ObservableCollection<string>();
        public Login()
        {
            InitializeComponent();



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


            loginInfoList.Clear();



            //LoadFileToInstance("Login_info");
            LoadLoginFile();
            if (loginInfoList.Count > 0)
            {
                for (int i = 0; i < loginInfoList.Count; i++)
                {
                    device_name_str_list.Add(loginInfoList[i].Name);
                    device_ip_str_list.Add(loginInfoList[i].Ip);
                }

                deviceNameComboBox.ItemsSource = device_name_str_list;
                deviceNameComboBox.SelectedIndex = 0;
                ipComboBox.ItemsSource = device_ip_str_list;
                ipComboBox.SelectedIndex = 0;
            }

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

        private void addLoginInfo()
        {
            XmlDocument doc = new XmlDocument();
            //doc.LoadXml("<bookstore></bookstore>");//用这句话,会把以前的数据全部覆盖掉,只有你增加的数据
            doc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "LoginInfo.xml");
            XmlNode root = doc.SelectSingleNode("logininfo");


            XmlElement xelKey = doc.CreateElement("info");
            XmlAttribute xelIpAttr = doc.CreateAttribute("ip");
            xelIpAttr.InnerText = Once_login_info.Ip;
            xelKey.SetAttributeNode(xelIpAttr);

            XmlElement xelIp = doc.CreateElement("ip");
            xelIp.InnerText = Once_login_info.Ip;
            xelKey.AppendChild(xelIp);

            XmlElement xelName = doc.CreateElement("name");
            xelName.InnerText = Once_login_info.Name;
            xelKey.AppendChild(xelName);

            XmlElement xelPort = doc.CreateElement("port");
            xelPort.InnerText = Once_login_info.Port;
            xelKey.AppendChild(xelPort);

            XmlElement xelDeviceNum = doc.CreateElement("deviceNum");
            xelDeviceNum.InnerText = Once_login_info.Device_num;
            xelKey.AppendChild(xelDeviceNum);
            root.AppendChild(xelKey);

            doc.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "LoginInfo.xml");
            new TipsWindow("插入成功", 3, TipsEnum.OK).Show();


        }

        private void LoginAction(object sender, RoutedEventArgs e)
        {
            TD_INFO info = new TD_INFO();

            string ip = IPText.Text.ToString();

            if (MainWindow.real_PlayPOJOs.Count > 0)
            {
                for (int i = 0; i < MainWindow.real_PlayPOJOs.Count; i++)
                {
                    if (ip == MainWindow.real_PlayPOJOs[i].IP)
                    {
                        new TipsWindow("该设备已登录", 3, TipsEnum.FAIL).Show();
                        return;
                    }
                }
            }

            if (ip == "")
            {
                new TipsWindow("ip地址不可为空", 3, TipsEnum.FAIL).Show();
                return;
            }
            else
            {
                info.Ip = ip;
            }

            string prot = PortText.Text.ToString();
            if (prot == "")
            {
                new TipsWindow("端口不可为空", 3, TipsEnum.FAIL).Show();
                return;
            }
            else
            {
                info.Port = prot;
            }

            string name = Username.Text.ToString();
            if (name == "")
            {
                new TipsWindow("用户名不可为空", 3, TipsEnum.FAIL).Show();
                return;
            }
            else
            {
                info.Name = name;
            }
            string pw = Password.Password.Trim().ToString();
            if (pw == "")
            {
                new TipsWindow("密码不可为空", 3, TipsEnum.FAIL).Show();
                return;
            }
            else
            {
                Console.WriteLine(pw);
                info.Pw = pw;
            }
            string deviceNameStr = deviceNameTextBox.Text;
            info.deviceName = deviceNameStr;
            string deviceNumStr = deviceNum.Text.ToString();
            info.TDid = "1";
            //info.Ip = "12.52.12.135";
            //info.Port = "8000";
            //info.Name = "admin";
            //info.Pw = pw;
            MainWindow.sbmc = info.Ip;
            //创建海康相机sdk返回值集合
            //MessageBoxWindow.real_PlayPOJOs.Add(new Real_PlayPOJO { deviceNum = "60134" });
            MainWindow.real_PlayPOJOs.Add(new Real_PlayPOJO { deviceNum = deviceNumStr });
            MainWindow.sbmc = info.Ip; //deviceNameTextBox.Text.Trim().Equals("") ? info.Ip : deviceNameTextBox.Text.Trim();

            MainWindow.In_Main_Form.SetLoginInfo(info, MainWindow.real_PlayPOJOs.Count - 1, HighDefinitionToggle.IsChecked.GetValueOrDefault());
            Once_login_info = new LoginPOJO
            {
                Ip = ip,
                Name = deviceNameStr,
                Port = prot,
                Device_num = deviceNumStr
            };
            bool hasLoginInfo = false;
            //SaveInstanceToFile("Login_info");
            for (int i = 0; i < loginInfoList.Count; i++)
            {
                if (string.Equals(loginInfoList[i].Ip, ip, StringComparison.OrdinalIgnoreCase))
                {
                    hasLoginInfo = true;
                    break;
                }
            }
            if (!hasLoginInfo)
            {
                addLoginInfo();
            }

            Close();

        }
        private void cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 将json保存到本地
        /// </summary>
        /// <param name="ins"></param>
        /// <param name="fullPath"></param>
        public static void SaveInstanceToFile(string fullPath)
        {
            FileStream file = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + fullPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            using (var stream = new StreamWriter(file))
            {
                Save_login_info = new LoginPOJO[Login_info.Length + 1];
                if (Once_login_info.Ip.Trim() != "已删除")
                    Save_login_info[Save_login_info.Length - 1] = Once_login_info;

                #region 保存前去重

                for (int i = 0; i < Login_info.Count(); i++)
                {
                    Save_if = true;
                    for (int j = 0; j < Save_login_info.Count(); j++)
                    {
                        if (Save_login_info[j] == null) continue;
                        if (Login_info[i].Ip.Equals(Save_login_info[j].Ip) || Login_info[i].Ip.Trim().Equals("已删除"))
                        {
                            Save_if = false;
                        }
                    }
                    if (Save_if)
                        Save_login_info[i] = Login_info[i];
                }

                #endregion 保存前去重

                string str = JsonConvert.SerializeObject(Save_login_info.Where(sqlP => sqlP != null).ToArray());
                stream.Write(str);
                stream.Flush();
                stream.Close();
            }
        }



        public static void LoadLoginFile()
        {
            try
            {
                // 创建XmlDDocument对象，并装入xml文件
                XmlDocument xmlDoc = new XmlDocument();

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "LoginInfo.xml", settings);
                xmlDoc.Load(reader);

                //xn 代表一个结点
                //xn.Name;//这个结点的名称
                //xn.Value;//这个结点的值
                //xn.ChildNodes;//这个结点的所有子结点
                //xn.ParentNode;//这个结点的父结点

                // 得到根节点bookstore
                XmlNode xn = xmlDoc.SelectSingleNode("logininfo");


                // 得到根节点的所有子节点
                XmlNodeList xnl = xn.ChildNodes;

                foreach (XmlNode xn1 in xnl)
                {
                    LoginInfo loginInfo = new LoginInfo();
                    // 将节点转换为元素，便于得到节点的属性值
                    XmlElement xe = (XmlElement)xn1;
                    // 得到Type和ISBN两个属性的属性值
                    //bookModel.BookISBN = xe.GetAttribute("ISBN").ToString();
                    //bookModel.BookType = xe.GetAttribute("Type").ToString();
                    // 得到LoginInfo节点的所有子节点
                    XmlNodeList xnl0 = xe.ChildNodes;
                    loginInfo.Ip = xnl0.Item(0).InnerText;
                    loginInfo.Name = xnl0.Item(1).InnerText;
                    loginInfo.Port = xnl0.Item(2).InnerText;
                    loginInfo.DeviceNum = xnl0.Item(3).InnerText;
                    loginInfoList.Add(loginInfo);
                }
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
                XmlElement root = document.CreateElement("logininfo");
                document.AppendChild(root);


                //保存文档
                document.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "LoginInfo.xml");
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 读取本地json
        /// </summary>
        /// <param name="ins"></param>
        /// <param name="fullPath"></param>
        public static void LoadFileToInstance(string fullPath)
        {
            try
            {
                var fs = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (var stream = new StreamReader(fs))
                {
                    Login_info = JsonConvert.DeserializeObject<LoginPOJO[]>(stream.ReadToEnd());
                }
            }
            catch (FileNotFoundException)
            {
                Login_info = new LoginPOJO[]{
                new LoginPOJO{ Ip="192.168.1.64",
                               Name ="",
                               Port ="8000",
                               Device_num ="60001"},
                new LoginPOJO{ Ip="192.168.1.65",
                               Name ="",
                               Port ="8000",
                               Device_num ="60002"},
                new LoginPOJO{ Ip="192.168.1.66",
                               Name ="",
                               Port ="8000",
                               Device_num ="60003"},
                new LoginPOJO{ Ip="192.168.1.67",
                               Name ="",
                               Port ="8000",
                               Device_num ="60004"},
                 };
            }
        }


        public void deviceComboBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (deviceNameComboBox.SelectedIndex == -1)
            {
                deviceNameComboBox.SelectedIndex = 0;
            }
            Console.WriteLine(loginInfoList[deviceNameComboBox.SelectedIndex].Ip);
            if (loginInfoList.Count == 1)
            {

                IPText.Text = loginInfoList[0].Ip;
                deviceNameTextBox.Text = loginInfoList[0].Name;
                PortText.Text = loginInfoList[0].Port;
                deviceNum.Text = loginInfoList[0].DeviceNum;
            }
            else if (loginInfoList.Count > 0)
            {
                IPText.Text = loginInfoList[deviceNameComboBox.SelectedIndex].Ip;
                deviceNameTextBox.Text = loginInfoList[deviceNameComboBox.SelectedIndex].Name;
                PortText.Text = loginInfoList[deviceNameComboBox.SelectedIndex].Port;
                deviceNum.Text = loginInfoList[deviceNameComboBox.SelectedIndex].DeviceNum;
            }
            else
            {
                IPText.Text = "";
                deviceNameTextBox.Text = "";
                PortText.Text = "";
                deviceNum.Text = "";
            }

        }

        public void deviceNameComboBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (deviceNameComboBox.SelectedIndex == -1)
            {
                deviceNameComboBox.SelectedIndex = 0;
            }
            Console.WriteLine(loginInfoList[deviceNameComboBox.SelectedIndex].Ip);
            if (loginInfoList.Count == 1)
            {

                IPText.Text = loginInfoList[0].Ip;
                deviceNameTextBox.Text = loginInfoList[0].Name;
                PortText.Text = loginInfoList[0].Port;
                deviceNum.Text = loginInfoList[0].DeviceNum;
            }
            else if (loginInfoList.Count > 0)
            {
                IPText.Text = loginInfoList[deviceNameComboBox.SelectedIndex].Ip;
                deviceNameTextBox.Text = loginInfoList[deviceNameComboBox.SelectedIndex].Name;
                PortText.Text = loginInfoList[deviceNameComboBox.SelectedIndex].Port;
                deviceNum.Text = loginInfoList[deviceNameComboBox.SelectedIndex].DeviceNum;
                ipComboBox.SelectedIndex = deviceNameComboBox.SelectedIndex;
            }
            else
            {
                IPText.Text = "";
                deviceNameTextBox.Text = "";
                PortText.Text = "";
                deviceNum.Text = "";
            }

        }
        public void ipComboBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ipComboBox.SelectedIndex == -1)
            {
                ipComboBox.SelectedIndex = 0;
            }
            Console.WriteLine(loginInfoList[ipComboBox.SelectedIndex].Ip);
            if (loginInfoList.Count == 1)
            {

                IPText.Text = loginInfoList[0].Ip;
                deviceNameTextBox.Text = loginInfoList[0].Name;
                PortText.Text = loginInfoList[0].Port;
                deviceNum.Text = loginInfoList[0].DeviceNum;
            }
            else if (loginInfoList.Count > 0)
            {
                IPText.Text = loginInfoList[ipComboBox.SelectedIndex].Ip;
                deviceNameTextBox.Text = loginInfoList[ipComboBox.SelectedIndex].Name;
                PortText.Text = loginInfoList[ipComboBox.SelectedIndex].Port;
                deviceNum.Text = loginInfoList[ipComboBox.SelectedIndex].DeviceNum;
                deviceNameComboBox.SelectedIndex = ipComboBox.SelectedIndex;
            }
            else
            {
                IPText.Text = "";
                deviceNameTextBox.Text = "";
                PortText.Text = "";
                deviceNum.Text = "";
            }

        }

        private void delete_login_info(object sender, RoutedEventArgs e)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "LoginInfo.xml");
                XmlElement xe = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
                string strPath = string.Format("/logininfo/info[@ip=\"{0}\"]", loginInfoList[deviceNameComboBox.SelectedIndex].Ip);
                XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
                selectXe.ParentNode.RemoveChild(selectXe);
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "LoginInfo.xml");
                if (device_name_str_list.Count == 1)
                {
                    device_name_str_list[0] = "";
                    deviceNameComboBox.Items.Refresh();
                    device_ip_str_list[0] = "";
                    ipComboBox.Items.Refresh();
                }
                else
                {
                    int removeIndex = deviceNameComboBox.SelectedIndex;
                    device_ip_str_list.RemoveAt(removeIndex);
                    device_name_str_list.RemoveAt(removeIndex);

                    ipComboBox.Items.Refresh();
                    deviceNameComboBox.Items.Refresh();
                }

                if (device_name_str_list.Count != 0)
                {
                    deviceNameComboBox.SelectedIndex = 0;
                    ipComboBox.SelectedIndex = 0;
                }

                new TipsWindow("删除成功", 3, TipsEnum.FAIL).Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}

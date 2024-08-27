using HandyControl.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
namespace WpfApp1
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : System.Windows.Window
    {
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


        public Login()
        {
            InitializeComponent();



            LoadFileToInstance("Login_info");
            List<string> device_ip_str_list = new List<string>();
            for (int i = 0; i < Login_info.Length; i++)
            {
                device_ip_str_list.Add(Login_info[i].Ip.ToString());
            }
            deviceComboBox.ItemsSource = device_ip_str_list;
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
        private void LoginAction(object sender, RoutedEventArgs e)
        {
            TD_INFO info = new TD_INFO();

            String ip = IPText.Text.ToString();

            if (MessageBoxWindow.real_PlayPOJOs.Count > 0)
            {
                for (int i = 0; i < MessageBoxWindow.real_PlayPOJOs.Count; i++)
                {
                    if (ip == MessageBoxWindow.real_PlayPOJOs[i].IP)
                    {
                        Growl.Warning("该设备已登录");
                        return;
                    }
                }
            }

            if (ip == "")
            {
                Growl.Warning("ip地址不可为空");
                return;
            }
            else
            {
                info.Ip = ip;
            }

            String prot = PortText.Text.ToString();
            if (prot == "")
            {
                Growl.Warning("端口地址不可为空");
                return;
            }
            else
            {
                info.Port = prot;
            }

            String name = Username.Text.ToString();
            if (name == "")
            {
                Growl.Warning("用户名不可为空");
                return;
            }
            else
            {
                info.Name = name;
            }
            String pw = Password.Password.Trim().ToString();
            if (pw == "")
            {
                Growl.Warning("密码不可为空");
                return;
            }
            else
            {
                Console.WriteLine(pw);
                info.Pw = pw;
            }

            string deviceNumStr = deviceNum.ToString();
            info.TDid = "1";
            //info.Ip = "12.52.12.135";
            //info.Port = "8000";
            //info.Name = "admin";
            //info.Pw = pw;
            MessageBoxWindow.sbmc =  info.Ip;
            //创建海康相机sdk返回值集合
            //MessageBoxWindow.real_PlayPOJOs.Add(new Real_PlayPOJO { deviceNum = "60134" });
            MessageBoxWindow.real_PlayPOJOs.Add(new Real_PlayPOJO { deviceNum = deviceNumStr });
            MessageBoxWindow.sbmc = info.Ip;
            MessageBoxWindow.In_Main_Form.SetLoginInfo(info, 0, true);

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
            Console.WriteLine(Login_info[deviceComboBox.SelectedIndex].Ip.ToString());
            IPText.Text = Login_info[deviceComboBox.SelectedIndex].Ip.ToString();
            deviceName.Text = Login_info[deviceComboBox.SelectedIndex].Name.ToString();
            PortText.Text = Login_info[deviceComboBox.SelectedIndex].Port.ToString();
            deviceNum.Text = Login_info[deviceComboBox.SelectedIndex].Device_num.ToString();
        }

    }
}

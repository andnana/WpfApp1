using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Login()
        {
            InitializeComponent();
        }
        private void LoginAction(object sender, RoutedEventArgs e)
        {
            TD_INFO info = new TD_INFO();
            info.TDid = "1";
            info.Ip = "12.52.12.135";
            info.Port = "8000";
            info.Name = "admin";
            info.Pw = "1234qwer";

            //创建海康相机sdk返回值集合
            MessageBoxWindow.real_PlayPOJOs.Add(new Real_PlayPOJO { deviceNum = "60135" });
            MessageBoxWindow.sbmc = "12.52.12.135";
            MessageBoxWindow.In_Main_Form.SetLoginInfo(info, 0, true);

            Close();

        }
    }
}

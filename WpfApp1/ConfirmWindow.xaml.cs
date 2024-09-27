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

namespace WpfApp1
{
    /// <summary>
    /// ConfirmWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfirmWindow : Window
    {
        TipsEnum tipEnum;
        string msg = "";
       
        public int Result { get; private set; }
        public ConfirmWindow(string msg, TipsEnum tipsEnum)
        {
            this.tipEnum = tipsEnum;
            InitializeComponent();
            this.msg = msg;
            messageTextBlock.Text = msg;
            if (tipsEnum == TipsEnum.CANCEL)
            {
                OKBtn.Visibility = Visibility.Hidden;
            }

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


        }
        private void OK(object sender, EventArgs e)
        {
            Result = 1;
            this.Close(); // 关闭窗口
        }
        private void Cancel(object sender, EventArgs e)
        {
            Result = 0;
            this.Close(); // 关闭窗口
        }
    }
}

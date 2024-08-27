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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace WpfApp1
{
    /// <summary>
    /// Logout.xaml 的交互逻辑
    /// </summary>
    public partial class Logout : System.Windows.Window
    {
        public Logout()
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
           
            MessageBoxWindow.In_Main_Form.Close();
            this.Close();
        }
        private void No(Object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

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
using HandyControl.Controls;
namespace WpfApp1
{
    /// <summary>
    /// CloudPlatform.xaml 的交互逻辑
    /// </summary>
    public partial class CloudPlatform : System.Windows.Window
    {
        public static bool  stopCruiseWhenWarningIsChecked = false;
        public static CloudPlatform In_CloudPlat_Form;
        public CloudPlatform()
        {
            InitializeComponent();
            In_CloudPlat_Form = this;
           
            speedNumericUpDown.Value = double.Parse(MessageBoxWindow.presetPOJOList[MessageBoxWindow.Chosen_device_num].Speed);
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
        private void stopCruiseWhenWarningFun(object sender, RoutedEventArgs e)
        {
            MessageBoxWindow.stopCruiseWhenWarningIsChecked = stopCruiseWhenWarning.IsChecked.Value;
        }
        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            int iSeq = Cruise_comBox.SelectedIndex;    //+1

            MessageBoxWindow.real_PlayPOJOs[MessageBoxWindow.Chosen_device_num].I_cruise_path_num = iSeq;
            MessageBoxWindow.iSeq = iSeq;   
            MessageBoxWindow.real_PlayPOJOs[MessageBoxWindow.Chosen_device_num].cruise_num_list = new List<int>();
            for (int i = 0; i < MessageBoxWindow.presetPOJOList[MessageBoxWindow.Chosen_device_num].Cruises[iSeq].Count; i++)
            {
                if (MessageBoxWindow.presetPOJOList[MessageBoxWindow.Chosen_device_num].Cruises[iSeq][i].preset_num > 0)
                {
                    MessageBoxWindow.real_PlayPOJOs[MessageBoxWindow.Chosen_device_num].cruise_num_list.Add(MessageBoxWindow.presetPOJOList[MessageBoxWindow.Chosen_device_num].Cruises[iSeq][i].preset_num);
                }
            }

            MessageBoxWindow.real_PlayPOJOs[MessageBoxWindow.Chosen_device_num].B_isAuto = true;
            MessageBoxWindow.In_Main_Form.reloadCruiseData();
        }

        /*       private void speed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
               {
                   // 当Slider的值发生变化时，更新TextBlock的文本
                   if (speed_slider != null && speed_slider != null)
                   {
                       // 将值限制为整数
                       speed_slider.Value = Math.Round(e.NewValue);
                       //zoom_in_out_text.Text = Math.Round(e.NewValue) + "dps";
                       //MessageBoxWindow.In_Main_Form.Change_speed(Math.Round(e.NewValue).ToString(), MessageBoxWindow.choose_device_num);
                       speed_text.Text = Math.Round(e.NewValue) + "°/s";
                   }

               }*/
         private void speedSetup(object sender, RoutedEventArgs e)
        {
            // 当Slider的值发生变化时，更新TextBlock的文本
            MessageBoxWindow.In_Main_Form.Change_speed(speedNumericUpDown.Value.ToString(), MessageBoxWindow.choose_device_num);

        }
    }


}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static WpfApp1.CHCNetSDK;
namespace WpfApp1
{
    /// <summary>
    /// CloudPlatform.xaml 的交互逻辑
    /// </summary>
    public partial class CloudPlatform : System.Windows.Window
    {
        public static bool  stopCruiseWhenWarningIsChecked = false;
        public static CloudPlatform In_CloudPlat_Form;
        ObservableCollection<string> device_ip_str_list = new ObservableCollection<string>();
        public CloudPlatform()
        {
            InitializeComponent();
            In_CloudPlat_Form = this;

            DeviceIPCombobox.Items.Add(MainWindow.sbmc);
            DeviceIPCombobox.Items.Add("全部");

            HigherAlarmTextBox.Text = MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_gbyz.ToString();
            LowerAlarmTextBox.Text = MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_dbyz.ToString();
            speedNumericUpDown.Value = double.Parse(MainWindow.presetPOJOList[MainWindow.Chosen_device_num].Speed);

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

        /// <summary>
        /// 增加预置点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addPreset(object sender, RoutedEventArgs e)
        {
            //string str = PresetComboBox.Items[PresetComboBox.SelectedIndex].ToString();
            //int item = int.Parse(PresetComboBox.Items[PresetComboBox.SelectedIndex].ToString());
            ComboBoxItem selectedItem = PresetComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                //MessageBox.Show("选中的值是：" + selectedItem.Content.ToString());
                int presetInt = int.Parse(selectedItem.Content.ToString());  
                bool isOK = NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)CHCNetSDK.SET_PRESET, (uint)presetInt);
                if (!isOK)
                {
                    Growl.SuccessGlobal("预置点增加失败");
                }
                else
                {
                    Growl.SuccessGlobal("预置点增加成功");
                    MainWindow.presetPOJOList[MainWindow.Chosen_device_num].Presets[presetInt - 1] = PresetCommentTextBox.Text;
                    Tool.SaveInstanceToFile(MainWindow.presetPOJOList[MainWindow.Chosen_device_num], MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].IP);
                }

            }
            
        }

        /// <summary>
        /// 删除预置点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deletePreset(object sender, EventArgs e)
        {

            ComboBoxItem selectedItem = PresetComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                //MessageBox.Show("选中的值是：" + selectedItem.Content.ToString());
                int presetInt = int.Parse(selectedItem.Content.ToString());
                bool isOK = NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)CHCNetSDK.CLE_PRESET, (uint)presetInt);
                if (!isOK)
                {
                    Growl.SuccessGlobal("预置点删除失败");
                }
                else
                {
                    Growl.SuccessGlobal("预置点删除成功");
                    MainWindow.presetPOJOList[MainWindow.Chosen_device_num].Presets[Convert.ToInt16(PresetComboBox.Text) - 1] = "空";
                    Tool.SaveInstanceToFile(MainWindow.presetPOJOList[MainWindow.Chosen_device_num], MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].IP);
                }

            }
      
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            int iSeq = Cruise_comBox.SelectedIndex;    //+1

            MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_cruise_path_num = iSeq;
            MainWindow.iSeq = iSeq;
            MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].cruise_num_list = new List<int>();
            for (int i = 0; i < MainWindow.presetPOJOList[MainWindow.Chosen_device_num].Cruises[iSeq].Count; i++)
            {
                if (MainWindow.presetPOJOList[MainWindow.Chosen_device_num].Cruises[iSeq][i].preset_num > 0)
                {
                    MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].cruise_num_list.Add(MainWindow.presetPOJOList[MainWindow.Chosen_device_num].Cruises[iSeq][i].preset_num);
                }
            }

            MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].B_isAuto = true;
            MainWindow.In_Main_Form.reloadCruiseData();
        }


        /// <summary>
        /// 停止巡航路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cruiseStop(object sender, EventArgs e)
        {
            NET_DVR_PTZControlWithSpeed(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, CHCNetSDK.TILT_UP, 1, 1);
            MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].B_isAuto = false;
        }

        /// <summary>
        /// 设置报警阈值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setAlarmValue(object sender, EventArgs e)
        {
            if (DeviceIPCombobox.SelectedIndex == 0)
            {
                for (int i = 0; i < MainWindow.real_PlayPOJOs.Count; i++)
                {
                    MainWindow.real_PlayPOJOs[i].I_gbyz = Convert.ToInt32(HigherAlarmTextBox.Text);
                    MainWindow.real_PlayPOJOs[i].I_dbyz = Convert.ToInt32(LowerAlarmTextBox.Text);
                }
            }
            else
            {
                int device_num = MainWindow.real_PlayPOJOs.FindIndex(item => item.IP.Equals(DeviceIPCombobox.Text));
                MainWindow.real_PlayPOJOs[device_num].I_gbyz = Convert.ToInt32(HigherAlarmTextBox.Text);
                MainWindow.real_PlayPOJOs[device_num].I_dbyz = Convert.ToInt32(LowerAlarmTextBox.Text);
            }
            Growl.SuccessGlobal("设置成功");
        }

     
        /// <summary>
        /// 调用预置点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invokePreset(object sender, EventArgs e)
        {
            ComboBoxItem selectedItem = PresetComboBox.SelectedItem as ComboBoxItem;
            if( selectedItem != null)
            {
                int presetInt = int.Parse(selectedItem.Content.ToString());
                NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)CHCNetSDK.GOTO_PRESET, (uint)presetInt);
            }
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
            MainWindow.In_Main_Form.Change_speed(speedNumericUpDown.Value.ToString(), MainWindow.choose_device_num);

        }
        private void addCruisePoint(object sender, RoutedEventArgs e)
        {
            new AddCruisePoint().Show();
        }

    }


}

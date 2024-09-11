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
using static WpfApp1.CHCNetSDK;

namespace WpfApp1
{
    /// <summary>
    /// AddCruisePath.xaml 的交互逻辑
    /// </summary>
    public partial class AddCruisePoint : System.Windows.Window
    {

        /// <summary>
        /// 当前云台巡航点备注
        /// </summary>
        private PresetPOJO presetPOJO;

        public AddCruisePoint()
        {
            InitializeComponent();

            presetPOJO = MainWindow.presetPOJOList[MainWindow.choose_device_num];

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
                    //MainWindow.presetPOJOList[MainWindow.Chosen_device_num].Presets[presetInt - 1] = "1";//PresetCommentTextBox.Text;
                    //Tool.SaveInstanceToFile(MainWindow.presetPOJOList[MainWindow.Chosen_device_num], MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].IP);
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
                    //MainWindow.presetPOJOList[MainWindow.Chosen_device_num].Presets[Convert.ToInt16(PresetComboBox.Text) - 1] = "空";
                    //Tool.SaveInstanceToFile(MainWindow.presetPOJOList[MainWindow.Chosen_device_num], MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].IP);
                }

            }

        }


        /// <summary>
        /// 调用预置点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invokePreset(object sender, EventArgs e)
        {
            ComboBoxItem selectedItem = PresetComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                int presetInt = int.Parse(selectedItem.Content.ToString());
                NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)CHCNetSDK.GOTO_PRESET, (uint)presetInt);
            }
        }

        /// <summary>
        /// 添加巡航点按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addCruisePoint(object sender, RoutedEventArgs e)
        {
            bool bRet;
            int CruisePathIndex = CruisePathComboBox.SelectedIndex;
            int cruisePresetIndex = CruisePresetComboBox.SelectedIndex;
            int preset = Convert.ToInt16(PresetComboBox.Text);
            int time = TimeComboBox.SelectedIndex + 1;

            presetPOJO.Cruises[CruisePathIndex][cruisePresetIndex].preset_num = preset;
            presetPOJO.Cruises[CruisePathIndex][cruisePresetIndex].time = time;
            presetPOJO.Cruises[CruisePathIndex][cruisePresetIndex].name = presetPOJO.Presets[preset - 1];

            MainWindow.In_Main_Form.reloadCruiseData();
            Growl.SuccessGlobal("添加成功");
            Tool.SaveInstanceToFile(presetPOJO, MainWindow.real_PlayPOJOs[MainWindow.choose_device_num].IP);
        }

    }
}

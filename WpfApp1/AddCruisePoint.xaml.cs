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

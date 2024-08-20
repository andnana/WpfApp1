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
    /// CloudPlatform.xaml 的交互逻辑
    /// </summary>
    public partial class CloudPlatform : Window
    {
        public CloudPlatform()
        {
            InitializeComponent();
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
    }
}

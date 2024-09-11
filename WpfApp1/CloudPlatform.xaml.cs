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
using System.Xml;
using HandyControl.Controls;
using static WpfApp1.CHCNetSDK;
namespace WpfApp1
{
    /// <summary>
    /// CloudPlatform.xaml 的交互逻辑
    /// </summary>
    public partial class CloudPlatform : System.Windows.Window
    {

        internal static List<CruisePOJO> cruises2 = new List<CruisePOJO>();
        public static bool stopCruiseWhenWarningIsChecked = false;
        public static CloudPlatform In_CloudPlat_Form;
        ObservableCollection<string> device_ip_str_list = new ObservableCollection<string>();
        public CloudPlatform()
        {
            InitializeComponent();
            In_CloudPlat_Form = this;
            for (int i = 0; i < MainWindow.deviceInfoList.Count; i++)
            {
                DeviceIPCombobox.Items.Add(MainWindow.deviceInfoList[i].ip);
            }

            DeviceIPCombobox.Items.Add("全部");

            HigherAlarmTextBox.Text = MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_gbyz.ToString();
            LowerAlarmTextBox.Text = MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_dbyz.ToString();
            speedNumericUpDown.Value = double.Parse(MainWindow.deviceInfoList[MainWindow.Chosen_device_num].speed.ToString());
        
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
            LoadCruise(MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip);
            for (int i = 0; i < cruises2.Count; i++)
            {
                Cruise_comBox.Items.Add(cruises2[i].notes);
            }

        }


        private void presets(object sender, RoutedEventArgs e)
        {
            new Presets().Show();
        }


        private void ToCruises(object sender, RoutedEventArgs e)
        {
            new Cruises().Show();
        }



        public void LoadCruise(string fileNamePrefix)
        {
            try
            {
                // 创建XmlDDocument对象，并装入xml文件
                XmlDocument xmlDoc = new XmlDocument();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + fileNamePrefix + "_cruises.xml", settings);
            xmlDoc.Load(reader);

            //xn 代表一个结点
            //xn.Name;//这个结点的名称
            //xn.Value;//这个结点的值
            //xn.ChildNodes;//这个结点的所有子结点
            //xn.ParentNode;//这个结点的父结点

            // 得到根节点bookstore
            XmlNode xn = xmlDoc.SelectSingleNode("cruises");


            // 得到根节点的所有子节点
            XmlNodeList xnl = xn.ChildNodes;

                if(xnl.Count > 0)
                {
                    CruisePOJO cruise = new CruisePOJO();
                    // 将节点转换为元素，便于得到节点的属性值

                    XmlElement xe = (XmlElement)xnl[0];
                    // 得到Type和ISBN两个属性的属性值
                    //bookModel.BookISBN = xe.GetAttribute("ISBN").ToString();
                    //bookModel.BookType = xe.GetAttribute("Type").ToString();


                    cruise.timeStr = xe.GetAttribute("save_time").ToString();
                    cruise.notes = xe.GetAttribute("notes").ToString();

                    cruises2.Add(cruise);
                }
         


            reader.Close();
            }
            catch (Exception e)
            {
               


                Console.WriteLine(e.Message);
            }
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            int iSeq = Cruise_comBox.SelectedIndex;
            MainWindow.In_Main_Form.LoadCruisesPresets(MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip);
            MainWindow.LoadCruisesFile(MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip);
            MainWindow.deviceCruises[MainWindow.Chosen_device_num] = MainWindow.cruises[iSeq];

            MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_cruise_path_num = iSeq;
            MainWindow.iSeq = iSeq;
            MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].cruise_num_list = new List<int>();
            for (int i = 0; i < MainWindow.cruisesPresets.Count; i++)
            {
                if (MainWindow.cruisesPresets[i].preset_num > 0)
                {
                    MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].cruise_num_list.Add(MainWindow.cruisesPresets[i].preset_num);
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
            MainWindow.In_Main_Form.Change_speed(speedNumericUpDown.Value.ToString(), MainWindow.Chosen_device_num);

        }
        /*   private void addCruisePoint(object sender, RoutedEventArgs e)
           {
               new AddCruisePoint().Show();
           }
   */
    }


}

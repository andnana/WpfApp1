using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
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

        DispatcherTimer disapearSuccessTipsTimer;
        internal static List<CruisePOJO> cruises2 = new List<CruisePOJO>();
        public static bool stopCruiseWhenWarningIsChecked = false;
        public static CloudPlatform In_CloudPlat_Form;
        ObservableCollection<string> device_ip_str_list = new ObservableCollection<string>();
        public CloudPlatform()
        {
            InitializeComponent();


            disapearSuccessTipsTimer = new DispatcherTimer();
            In_CloudPlat_Form = this;
            /*       for (int i = 0; i < MainWindow.deviceInfoList.Count; i++)
                   {
                       DeviceIPCombobox.Items.Add(MainWindow.deviceInfoList[i].ip);
                   }*/

            //DeviceIPCombobox.Items.Add("全部");
            deviceIPLabel.Content = MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].Device_name;

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
            cruises2.Clear();
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

                for (int i = 0; i < xnl.Count; i++)
                {
                    CruisePOJO cruise = new CruisePOJO();

                    XmlElement xe = (XmlElement)xnl[i];
                    if (xe != null && xe.ChildNodes.Count > 0)
                    {
                        cruise.timeStr = xe.GetAttribute("save_time").ToString();
                        cruise.notes = xe.GetAttribute("notes").ToString();
                        cruises2.Add(cruise);
                    }
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
            try
            {
                int iSeq = Cruise_comBox.SelectedIndex;
                MainWindow.In_Main_Form.LoadCruisesPresets(MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip, iSeq);
                MainWindow.LoadCruisesFile(MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip);
                MainWindow.deviceCruises[MainWindow.Chosen_device_num] = MainWindow.cruises[iSeq];

                MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_cruise_path_num = iSeq;
                MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_cruise_num_next = 1;
                MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_cruise_num_now = 0;
                MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].cruise_num_list = new List<int>();
                for (int i = 0; i < MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].cruisesPresets.Count; i++)
                {
                    if (MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].cruisesPresets[i].preset_num > 0)
                    {
                        MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].cruise_num_list.Add(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].cruisesPresets[i].preset_num);
                    }
                }

                MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].B_isAuto = true;
                MainWindow.In_Main_Form.reloadCruiseData();
            }
            catch (Exception e2)
            {
                Console.WriteLine(e2.Message);
            }

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
            MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_cruise_num_next = 1;
            MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_cruise_num_now = 0;
        }

        public async Task hiddenLoading()
        {
            loading.Visibility = Visibility.Hidden;
            loadingTips.Text = "初始化成功";
            disapearSuccessTipsTimer.Interval = TimeSpan.FromSeconds(5);
            disapearSuccessTipsTimer.Tick += disapearSuccessTips;
            disapearSuccessTipsTimer.Start();
        }
        private delegate Task loadingDeletate();

        private void initSpeedThread()
        {


            if (!NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)SET_PRESET, 5))
                Growl.SuccessGlobal("5，err=" + NET_DVR_GetLastError());
            Thread.Sleep(500);

            if (!NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)SET_PRESET, 10))
                Growl.SuccessGlobal("10，err=" + NET_DVR_GetLastError());
            Thread.Sleep(500);

            if (!NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)SET_PRESET, 15))
                Growl.SuccessGlobal("15，err=" + NET_DVR_GetLastError());
            Thread.Sleep(500);

            if (!NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)SET_PRESET, 20))
                Growl.SuccessGlobal("20，err=" + NET_DVR_GetLastError());
            Thread.Sleep(500);

            if (!NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)SET_PRESET, 25))
                Growl.SuccessGlobal("25，err=" + NET_DVR_GetLastError());
            Thread.Sleep(500);

            if (!NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)SET_PRESET, 30))
                Growl.SuccessGlobal("30，err=" + NET_DVR_GetLastError());
            Thread.Sleep(500);

            if (!NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)SET_PRESET, 35))
                Growl.SuccessGlobal("35，err=" + NET_DVR_GetLastError());
            Thread.Sleep(500);

            if (!NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)SET_PRESET, 40))
                Growl.SuccessGlobal("40，err=" + NET_DVR_GetLastError());
            Thread.Sleep(500);

            if (!NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)SET_PRESET, 65))
                Growl.SuccessGlobal("65，err=" + NET_DVR_GetLastError());
            Thread.Sleep(500);

            if (!NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)SET_PRESET, 23))
                Growl.SuccessGlobal("23，err=" + NET_DVR_GetLastError());
            Thread.Sleep(500);

            if (!NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)SET_PRESET, 24))
                Growl.SuccessGlobal("24，err=" + NET_DVR_GetLastError());
            Thread.Sleep(500);

            if (!NET_DVR_PTZPreset(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].I_lRealHandle, (uint)SET_PRESET, 92))
                Growl.SuccessGlobal("92，err=" + NET_DVR_GetLastError());
            Thread.Sleep(500);

            this.Dispatcher.BeginInvoke(new loadingDeletate(hiddenLoading));

        }

        private void disapearSuccessTips(object sender, EventArgs e)
        {
            disapearSuccessTipsTimer.Stop();
            loadingTips.Text = "";
        }
        private void initSpeed(object sender, EventArgs e)
        {
            loading.Visibility = Visibility.Visible;

            ThreadStart threadStart = new ThreadStart(initSpeedThread);
            Thread thread1 = new Thread(threadStart);
            thread1.Start();
        }
        private void initQuestion(object sender, EventArgs e)
        {
            new InitQuestion().Show();
        }
        /// <summary>
        /// 设置报警阈值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setAlarmValue(object sender, EventArgs e)
        {
            /*            if (DeviceIPCombobox.SelectedIndex == 0)
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
                        }*/
            int device_num = MainWindow.real_PlayPOJOs.FindIndex(item => item.IP.Equals(deviceIPLabel.Content));
            MainWindow.real_PlayPOJOs[device_num].I_gbyz = Convert.ToInt32(HigherAlarmTextBox.Text);
            MainWindow.real_PlayPOJOs[device_num].I_dbyz = Convert.ToInt32(LowerAlarmTextBox.Text);
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

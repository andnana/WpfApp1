using HandyControl.Controls;
using LiveCharts;
using LiveCharts.Wpf;
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
using static WpfApp1.CHCNetSDK;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.Threading;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using ScottPlot.AxisLimitManagers;
using System.Net.Sockets;
using System.Net;
using LiveCharts.Wpf.Charts.Base;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Dml.Chart;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using NPOI.Util;
using System.Media;
using System.Reflection;
using Application = System.Windows.Forms.Application;
using System.Timers;
using System.Xml;
using Button = System.Windows.Controls.Button;
using System.Windows.Forms.Integration;
using System.Collections;
using ScottPlot.ArrowShapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using NPOI.SS.Formula.Functions;
using ScottPlot.Palettes;
using static WpfApp1.MainWindow;
using System.ComponentModel;
using System.Configuration;
using HelixToolkit.Wpf;

namespace WpfApp1
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        /* float anglexTemp = 0;
         float anglexTempaaa = 0;
         float angleyTemp = 0;
         Model3DGroup model3DGroup;

         RotateTransform3D rotate;
         RotateTransform3D rotate2;

         GeometryModel3D geometryModel3D;
         Transform3DGroup transform3DGroup;
         ModelVisual3D modelVisual3D;*/
        bool IndicatingLaserBool = true;
        bool AlgorithmABool = false;
        bool redPointBool = false;
        DispatcherTimer disapearSuccessTipsTimer;
        DispatcherTimer disapearTipsTimer;
        DeviceSetup deviceSetup = null;

        public static List<DeviceInfo> deviceInfoList = new List<DeviceInfo>();

        Dictionary<string, Status> map1 = new Dictionary<string, Status>();

        int presetInt = 0;
        string presetNotes = "";
        int maxPid = 0;

        public static List<History_Message> historyMessages = new List<History_Message>();
        public static List<HistoryMessage2> historyMessages0 = new List<HistoryMessage2>();
        public static List<HistoryMessage2> historyMessages1 = new List<HistoryMessage2>();
        public static List<HistoryMessage2> historyMessages2 = new List<HistoryMessage2>();
        public static List<HistoryMessage2> historyMessages3 = new List<HistoryMessage2>();
        public static List<HistoryMessage2> historyMessages4 = new List<HistoryMessage2>();

        /*  private List<List<int>> Usual_historys;

          /// <summary>
          /// 历史数据集合
          /// </summary>
          public static List<UsualhistoryMessage> UsualHistoryMessages;*/



        private static System.Timers.Timer aTimer;

        /// <summary>
        /// 高报报警音
        /// </summary>
        public SoundPlayer sp;

        /// <summary>
        /// 低报报警音
        /// </summary>
        public SoundPlayer sp1;

        public static bool stopCruiseBool = false;
        int ndTimesShowLength = 0;

        System.Windows.Forms.PictureBox m_pictureBoxTemp;
        List<WindowsFormsHost> windowsFormsHosts = new List<WindowsFormsHost>();
        System.Windows.Forms.PictureBox m_pictureBox0;
        System.Windows.Forms.PictureBox m_pictureBox1;
        System.Windows.Forms.PictureBox m_pictureBox2;
        System.Windows.Forms.PictureBox m_pictureBox3;
        System.Windows.Forms.PictureBox m_pictureBox4;
        System.Windows.Forms.ToolTip m_toolTip;
        //ToolTip m_tp;

        //绑定的X轴数据
        private ChartValues<double> ValueList { get; set; }

        private ChartValues<double> ValueList2 { get; set; }

        public static List<CruisePOJO> cruisePOJOs = new List<CruisePOJO>();

        /// <summary>
        /// 运行时选中的设备编号
        /// </summary>
        public static int Chosen_device_num;


        /// <summary>
        /// 码流数据回调函数
        /// </summary>
        private static CHCNetSDK.REALDATACALLBACK RealData { get; set; } = null;


        /// <summary>
        /// 视频窗口
        /// </summary>
        public List<PictureBox> cameras;


        /// <summary>
        /// 设备参数结构体
        /// </summary>
        public CHCNetSDK.NET_DVR_DEVICEINFO_V40 DeviceInfo;

        /// <summary>
        /// 登录回调函数
        /// </summary>
        private static CHCNetSDK.LOGINRESULTCALLBACK LoginCallBack { get; set; } = null;

        /// <summary>
        /// 用户登录参数结构体
        /// </summary>
        public CHCNetSDK.NET_DVR_USER_LOGIN_INFO struLogInfo;

        public static MainWindow In_Main_Form;

        /// <summary>
        /// 设备独立信息集合
        /// </summary>
        public static List<Real_PlayPOJO> real_PlayPOJOs;

        public static List<CruisePOJO> cruises = new List<CruisePOJO>();
        public static List<CruisePOJO> deviceCruises = new List<CruisePOJO>();


        /// <summary>
        /// 选中的设备编号
        /// </summary>
        public static int choose_device_num;

        /// <summary>
        /// 设备名称
        /// </summary>
        public static string sbmc = "";

        //public SeriesCollection SeriesCollection2 { get; set; }
        public SeriesCollection SeriesCollection { get; set; }


        public string Name1 { get; set; } = "bbb";




        private void updateCruiseCurrentIcon()
        {

            for (int j = 0; j < cruisePOJOs.Count; j++)
            {
                if (cruisePOJOs[j].preset_num == presetInt)
                {
                    cruisePOJOs[j].isCurrent = true;
                    cruisePOJOs[j].imagePath = "/WpfApp1;component/Resources/current_play.png";
                }
                else
                {
                    cruisePOJOs[j].imagePath = "/WpfApp1;component/Resources/current_play_empty.png";
                }
            }
            Presets.ItemsSource = cruisePOJOs;
            Presets.Items.Refresh();
        }

        private delegate void updateCruiseCurrentIconDelegate();

        private void cruiseTimer(object sender, EventArgs e)
        {
            try
            {
                /* if(real_PlayPOJOs == null)
                 {
                     real_PlayPOJOs = new List<Real_PlayPOJO> { };
                 }*/
                for (int i = 0; i < real_PlayPOJOs.Count; i++)
                {
                    //指定设备是否正在巡航
                    if (real_PlayPOJOs[i].B_isAuto)
                    {
                        //判断倒计时是否结束
                        if (real_PlayPOJOs[i].I_cruise_num_time <= 0)
                        {
                            //调用巡航点对应的预置点
                            NET_DVR_PTZPreset(real_PlayPOJOs[i].I_lRealHandle, 39,
                                (uint)real_PlayPOJOs[i].cruise_num_list[real_PlayPOJOs[i].I_cruise_num_now]);
                            presetInt = real_PlayPOJOs[i].cruise_num_list[real_PlayPOJOs[i].I_cruise_num_now];
                            presetNotes = real_PlayPOJOs[MainWindow.Chosen_device_num].cruisesPresets[real_PlayPOJOs[Chosen_device_num].I_cruise_num_now].notes;
                            Console.WriteLine();
                            // Console.WriteLine("设备" + i + "正在前往预置点" + real_PlayPOJOs[i].I_cruise_num_now);
                            Console.WriteLine("设备" + i + "正在前往预置点" + "cruise" + i + "_" + real_PlayPOJOs[i].cruise_num_list[real_PlayPOJOs[i].I_cruise_num_now]);
                            if (Chosen_device_num == i)
                            {
                                this.Dispatcher.BeginInvoke(new updateCruiseCurrentIconDelegate(updateCruiseCurrentIcon));
                            }

                            //更新当前巡航点编号
                            real_PlayPOJOs[i].I_cruise_num_now = real_PlayPOJOs[i].I_cruise_num_next;
                            //更新下一个巡航点编号和倒计时

                            try
                            {
                                if (real_PlayPOJOs[i].I_cruise_num_next < real_PlayPOJOs[i].cruise_num_list.Count - 1)
                                {
                                    real_PlayPOJOs[i].I_cruise_num_next++;
                                }
                                else
                                {
                                    real_PlayPOJOs[i].I_cruise_num_next = 0;
                                }

                                real_PlayPOJOs[i].I_cruise_num_time = real_PlayPOJOs[MainWindow.Chosen_device_num].cruisesPresets[real_PlayPOJOs[Chosen_device_num].I_cruise_num_now].time + 1;

                                Console.WriteLine("设备" + i + "巡航路径" + real_PlayPOJOs[i].I_cruise_path_num);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        else
                        {
                            //更新倒计时
                            Console.WriteLine();
                            Console.WriteLine("倒计时" + real_PlayPOJOs[i].I_cruise_num_time);
                            real_PlayPOJOs[i].I_cruise_num_time--;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }




        /*       private void zoom_in_out_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
               {
                   // 当Slider的值发生变化时，更新TextBlock的文本
                   if (zoom_in_out_slider != null && zoom_in_out_text != null)
                   {
                       // 将值限制为整数
                       zoom_in_out_slider.Value = Math.Round(e.NewValue);
                       zoom_in_out_text.Text = Math.Round(e.NewValue) + "dps";
                   }

               }*/

        private void Zoom_in_MouseDown(object sender, RoutedEventArgs e)
        {
            if (MainWindow.sbmc == "")
            {
                new TipsWindow("请先登录", 3, TipsEnum.FAIL).Show();
                return;
            }
            NET_DVR_PTZControl(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, CHCNetSDK.ZOOM_IN, 0);
        }
        private void Zoom_out_MouseDown(object sender, RoutedEventArgs e)
        {
            if (MainWindow.sbmc == "")
            {
                new TipsWindow("请先登录", 3, TipsEnum.FAIL).Show();
                return;
            }
            NET_DVR_PTZControl(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, CHCNetSDK.ZOOM_OUT, 0);
        }
        /// <summary>
        /// 抬起焦距控制控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Zoom_out_MouseUp(object sender, RoutedEventArgs e)
        {
            NET_DVR_PTZControl(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, CHCNetSDK.ZOOM_OUT, 1);
        }
        private void Zoom_in_MouseUp(object sender, RoutedEventArgs e)
        {
            NET_DVR_PTZControl(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, CHCNetSDK.ZOOM_IN, 1);
        }

        public void LoadCruisesPresets(string fileNamePrefix, int cruiseIndex)
        {
            try
            {
                real_PlayPOJOs[MainWindow.Chosen_device_num].cruisesPresets.Clear();
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
                XmlNodeList presetNodeList = xnl[cruiseIndex].ChildNodes;

                foreach (XmlNode xn2 in presetNodeList)
                {
                    Preset preset = new Preset();
                    XmlElement xmlPreset = (XmlElement)xn2;
                    XmlNodeList xnl0 = xmlPreset.ChildNodes;
                    preset.preset_num = int.Parse(xnl0.Item(2).InnerText);
                    preset.notes = xnl0.Item(8).InnerText;
                    preset.time = int.Parse(xnl0.Item(3).InnerText);
                    preset.speed = int.Parse(xnl0.Item(5).InnerText);
                    real_PlayPOJOs[MainWindow.Chosen_device_num].cruisesPresets.Add(preset);

                }

                reader.Close();
            }
            catch (FileNotFoundException e)
            {
                //创建一个空的XML
                XmlDocument document = new XmlDocument();
                //声明头部
                XmlDeclaration dec = document.CreateXmlDeclaration("1.0", "utf-8", "yes");
                document.AppendChild(dec);

                //创建根节点
                XmlElement root = document.CreateElement("cruises");
                document.AppendChild(root);


                //保存文档
                document.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_cruises.xml");


                Console.WriteLine(e.Message);
            }

        }


        public void reloadCruiseData()
        {
            cruisePOJOs.Clear();
            for (int i = 0; i < real_PlayPOJOs[MainWindow.Chosen_device_num].cruisesPresets.Count; i++)
            {
                if (real_PlayPOJOs[MainWindow.Chosen_device_num].cruisesPresets[i].preset_num >= 0)
                {
                    int time = real_PlayPOJOs[MainWindow.Chosen_device_num].cruisesPresets[i].time;
                    int preset_num = real_PlayPOJOs[MainWindow.Chosen_device_num].cruisesPresets[i].preset_num;
                    cruisePOJOs.Add(new CruisePOJO() { imagePath = "/WpfApp1;component/Resources/current_play_empty.png", speedStr = real_PlayPOJOs[MainWindow.Chosen_device_num].cruisesPresets[i].speed.ToString() + " °/s", timeStr = time.ToString() + "s", name = real_PlayPOJOs[MainWindow.Chosen_device_num].cruisesPresets[i].notes, preset_num = preset_num });
                }
            }

            Presets.ItemsSource = cruisePOJOs;
            Presets.Items.Refresh();

        }


        /// <summary>
        /// 按下方向控制按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void up(object sender, RoutedEventArgs e)
        {
            real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
            //StopYTAutoMove(false);
            NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, TILT_UP, 0, 7);
        }
        // 当鼠标按下按钮时触发（可以看作是开始点击）  
        private void Button_MouseDown_TILT_UP(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.sbmc == "")
            {

            }
            else
            {
                //System.Diagnostics.Debug.WriteLine("按钮点击（鼠标按下）");
                // 注意：这里不是严格意义上的“按钮点击”，而是鼠标按下的动作  
                real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
                //StopYTAutoMove(false);
                NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, TILT_UP, 0, 7);
            }







        }
        // 当鼠标按下按钮时触发（可以看作是开始点击）  
        private void Button_MouseDown_PAN_LEFT(object sender, RoutedEventArgs e)
        {

            if (MainWindow.sbmc == "")
            {


            }
            else
            {
                //System.Diagnostics.Debug.WriteLine("按钮点击（鼠标按下）");
                // 注意：这里不是严格意义上的“按钮点击”，而是鼠标按下的动作  
                real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
                //StopYTAutoMove(false);
                NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, PAN_LEFT, 0, 7);

            }




        }
        // 当鼠标按下按钮时触发（可以看作是开始点击）  
        private void Button_MouseDown_PAN_RIGHT(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.sbmc == "")
            {

            }
            else
            {
                //System.Diagnostics.Debug.WriteLine("按钮点击（鼠标按下）");
                // 注意：这里不是严格意义上的“按钮点击”，而是鼠标按下的动作  
                real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
                //StopYTAutoMove(false);
                NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, PAN_RIGHT, 0, 7);
            }





        }
        // 当鼠标按下按钮时触发（可以看作是开始点击）  
        private void Button_MouseDown_UP_LEFT(object sender, MouseButtonEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("按钮点击（鼠标按下）");
            // 注意：这里不是严格意义上的“按钮点击”，而是鼠标按下的动作  
            real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
            //StopYTAutoMove(false);
            NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, UP_LEFT, 0, 7);



        }
        // 当鼠标按下按钮时触发（可以看作是开始点击）  
        private void Button_MouseDown_UP_RIGHT(object sender, MouseButtonEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("按钮点击（鼠标按下）");
            // 注意：这里不是严格意义上的“按钮点击”，而是鼠标按下的动作  
            real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
            //StopYTAutoMove(false);
            NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, UP_RIGHT, 0, 7);
        }
        // 当鼠标按下按钮时触发（可以看作是开始点击）  
        private void Button_MouseDown_DOWN_LEFT(object sender, MouseButtonEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("按钮点击（鼠标按下）");
            // 注意：这里不是严格意义上的“按钮点击”，而是鼠标按下的动作  
            real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
            //StopYTAutoMove(false);
            NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, DOWN_LEFT, 0, 7);



        }
        // 当鼠标按下按钮时触发（可以看作是开始点击）  
        private void Button_MouseDown_DOWN_RIGHT(object sender, MouseButtonEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("按钮点击（鼠标按下）");
            // 注意：这里不是严格意义上的“按钮点击”，而是鼠标按下的动作  
            real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
            //StopYTAutoMove(false);
            NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, DOWN_RIGHT, 0, 7);


        }
        // 当鼠标按下按钮时触发（可以看作是开始点击）  
        private void Button_MouseDown_TILT_DOWN(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.sbmc == "")
            {

            }
            else
            {
                //System.Diagnostics.Debug.WriteLine("按钮点击（鼠标按下）");
                // 注意：这里不是严格意义上的“按钮点击”，而是鼠标按下的动作  
                real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
                //StopYTAutoMove(false);
                NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, TILT_DOWN, 0, 7);
            }



        }
        // 当鼠标松开按钮时触发  
        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.sbmc == "")
            {

            }
            else
            {          // System.Diagnostics.Debug.WriteLine("按钮松开");
                // 注意：如果你的按钮在MouseDown时触发了某些操作，并希望在MouseUp时撤销或完成这些操作，请在这里处理  
                real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
                //In_Main_Form.StopYTAutoMove(false);
                NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, TILT_UP, 1, 7);
            }

        }
        //绘制事件
        void picturebox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(@"C:\Users\Administrator\Pictures\left.png");
            System.Drawing.Point ulPoint = new System.Drawing.Point(0, 0);
            e.Graphics.DrawImage(bmp, ulPoint);
            //m_toolTip.Show("222", this);
        }
        public static void SaveLanguage(string language)
        {
            //打开可执行的配置文件的 *.exe.config
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //删除配置节
            config.AppSettings.Settings.Remove("Language");
            //添加新的配置节
            config.AppSettings.Settings.Add("Language", language);
            //保存对配置文件所作的修改
            config.Save(ConfigurationSaveMode.Modified);
            //强制重载已更改部分
            ConfigurationManager.RefreshSection("appSettings");

        }
        /// <summary>
        /// 语言选项
        /// </summary>
        public enum Language
        {
            Chinese,
            English
        }

        public new Language language { get; set; }

        /// <summary>
        /// 切换语言
        /// </summary>
        private void SwitchLanguage(object sender, RoutedEventArgs e)
        {

            switchLanguage2();
        }

        public void switchLanguage2()
        {
            /*      try
                             {
                                 if (language == Language.Chinese)
                                 {
                                     language = Language.English;

                               ResourceDictionary englishRes = new ResourceDictionary();
                                     englishRes.Source = new Uri(@"pack://application:,,,/Language/English.xaml", UriKind.Absolute);
                                     Resources[2] = englishRes;


                                 }
                                 else
                                 {
                                     language = Language.Chinese;
                                     string chinese = "pack://application:,,,/Language/Chinese.xaml";
                                     this.Resources.MergedDictionaries[2].Source = new Uri(chinese, UriKind.RelativeOrAbsolute);
                                 }
                             }
                             catch (Exception e2)
                             {
                                 //错误处理
                                 Console.WriteLine("e2" + e2.Message);
                             }*/

            ResourceDictionary resourceDictionary;
            if (language == Language.Chinese)
            {
                language = Language.English;
                SaveLanguage("english");
                string english = "pack://application:,,,/Language/English.xaml";
                resourceDictionary = new ResourceDictionary { Source = new Uri(english, UriKind.RelativeOrAbsolute) };
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                //bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/online_active_dot.png", UriKind.Relative);
                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/english-filled.png", UriKind.Relative);
                bitmapImage.EndInit();
                langImg.Source = bitmapImage;
                hdLabel1.Content = "hd";
                hdLabel2.Content = "hd";
                onlineLable1.Content = "online";
                onlineLable2.Content = "online";

            }
            else
            {
                language = Language.Chinese;
                SaveLanguage("chinese");
                string chinese = "pack://application:,,,/Language/Chinese.xaml";
                resourceDictionary = new ResourceDictionary { Source = new Uri(chinese, UriKind.RelativeOrAbsolute) };
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                //bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/online_active_dot.png", UriKind.Relative);
                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/chinese-filled.png", UriKind.Relative);
                bitmapImage.EndInit();
                langImg.Source = bitmapImage;
                hdLabel1.Content = "高清";
                hdLabel2.Content = "高清";
                onlineLable1.Content = "在线";
                onlineLable2.Content = "在线";


            }

            // 将当前的资源字典从应用程序资源中移除
            Resources.MergedDictionaries.Remove(resourceDictionary);
            // 将新的资源字典添加到应用程序资源中
            Resources.MergedDictionaries.Add(resourceDictionary);

        }
        /// <summary>
        /// 登录信息
        /// </summary>
        /// <param name="info">登录信息结构体</param>
        //internal void SetLoginInfo(TD_INFO info, int device_num)
        internal void SetLoginInfo(TD_INFO info, int device_num, bool HD_if)
        {
            choose_device_num = device_num;
            DeviceInfo deviceInfo = new DeviceInfo();
            deviceInfo.ip = info.Ip;
            deviceInfo.speed = 15;
            deviceInfoList.Add(deviceInfo);
            LoadCruisesFile(info.Ip);
            if (cruises.Count > 0)
            {
                deviceCruises.Add(cruises[0]);
            }
            else
            {
                deviceCruises.Add(new CruisePOJO());
            }

            if (real_PlayPOJOs[device_num].I_lUserID < 0)
            {
                struLogInfo = new NET_DVR_USER_LOGIN_INFO();

                //设备IP地址或者域名
                byte[] byIP = Encoding.Default.GetBytes(info.Ip);
                struLogInfo.sDeviceAddress = new byte[129];
                byIP.CopyTo(struLogInfo.sDeviceAddress, 0);
                real_PlayPOJOs[device_num].IP = info.Ip;

                //设备用户名
                byte[] byUserName = Encoding.Default.GetBytes(info.Name);
                struLogInfo.sUserName = new byte[64];
                byUserName.CopyTo(struLogInfo.sUserName, 0);

                //设备密码
                byte[] byPassword = Encoding.Default.GetBytes(info.Pw);
                struLogInfo.sPassword = new byte[64];
                byPassword.CopyTo(struLogInfo.sPassword, 0);

                struLogInfo.wPort = ushort.Parse(info.Port);//设备服务端口号

                if (LoginCallBack == null)
                {
                    LoginCallBack = new LOGINRESULTCALLBACK(R_LoginCallBack);//注册回调函数
                }
                struLogInfo.cbLoginResult = LoginCallBack;
                struLogInfo.bUseAsynLogin = false; //是否异步登录：0- 否，1- 是

                DeviceInfo = new NET_DVR_DEVICEINFO_V40();

                //登录设备 Login the device
                real_PlayPOJOs[device_num].I_lUserID = NET_DVR_Login_V40(ref struLogInfo, ref DeviceInfo);
                real_PlayPOJOs[device_num].Device_name = info.deviceName;
                if (real_PlayPOJOs[device_num].I_lUserID < 0)
                {
                    real_PlayPOJOs.RemoveAt(device_num);
                    //Growl.WarningGlobal("登录失败,err:" + NET_DVR_GetLastError());
                    if (language == Language.Chinese)
                    {
                        new TipsWindow("登录失败", 3, TipsEnum.OK).Show();
                    }
                    else
                    {
                        new TipsWindow("login failure", 3, TipsEnum.OK).Show();
                    }
                    return;
                }
                else
                {
                    map1[device_num.ToString()].hd = HD_if;

                    //开始预览
                    OpenPreview(Int32.Parse(info.TDid), device_num, HD_if);
                    if (cruises.Count > 0)
                    {
                        Change_speed(deviceInfoList[device_num].speed.ToString(), device_num);
                    }

                    real_PlayPOJOs[device_num].I_gbyz = 3000;
                    real_PlayPOJOs[device_num].I_dbyz = 1000;

                    if (language == Language.Chinese)
                    {
                        new TipsWindow("登录成功", 3, TipsEnum.OK).Show();
                    }
                    else
                    {
                        new TipsWindow("login successfully", 3, TipsEnum.OK).Show();
                    }


                    //Usual_historys.Add(new List<int>());
                    //OnlineText.Content = "在线";
                    //OnlineText.Background = (Brush)new BrushConverter().ConvertFrom("#afe484");
                }
                //开始预览




            }
        }




        public static void LoadCruisesFile(string fileNamePrefix)
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

                foreach (XmlNode xn1 in xnl)
                {
                    CruisePOJO cruise = new CruisePOJO();
                    // 将节点转换为元素，便于得到节点的属性值
                    XmlElement xe = (XmlElement)xn1;
                    // 得到Type和ISBN两个属性的属性值
                    //bookModel.BookISBN = xe.GetAttribute("ISBN").ToString();
                    //bookModel.BookType = xe.GetAttribute("Type").ToString();


                    cruise.timeStr = xe.GetAttribute("save_time").ToString();
                    cruise.notes = xe.GetAttribute("notes").ToString();

                    cruises.Add(cruise);
                }
                reader.Close();
            }
            catch (FileNotFoundException e)
            {
                //创建一个空的XML
                XmlDocument document = new XmlDocument();
                //声明头部
                XmlDeclaration dec = document.CreateXmlDeclaration("1.0", "utf-8", "yes");
                document.AppendChild(dec);

                //创建根节点
                XmlElement root = document.CreateElement("cruises");
                document.AppendChild(root);


                //保存文档
                document.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + MainWindow.deviceInfoList[MainWindow.Chosen_device_num].ip + "_cruises.xml");


                Console.WriteLine(e.Message);
            }


        }

        #region 速度

        public void Change_speed(string speed, int device_num)
        {
            Console.WriteLine("NET_DVR_PTZPreset1");
            Console.WriteLine(NET_DVR_PTZPreset(real_PlayPOJOs[device_num].I_lRealHandle, (uint)CHCNetSDK.GOTO_PRESET, 65));
            Thread.Sleep(500);
            Console.WriteLine("NET_DVR_PTZPreset2");
            Console.WriteLine(NET_DVR_PTZPreset(real_PlayPOJOs[device_num].I_lRealHandle, (uint)CHCNetSDK.GOTO_PRESET, 23));
            Thread.Sleep(500);
            Console.WriteLine("NET_DVR_PTZPreset3");
            Console.WriteLine(NET_DVR_PTZPreset(real_PlayPOJOs[device_num].I_lRealHandle, (uint)CHCNetSDK.GOTO_PRESET, Speed_convert(speed)));
            Thread.Sleep(500);
            Console.WriteLine("NET_DVR_PTZPreset4");
            Console.WriteLine(NET_DVR_PTZPreset(real_PlayPOJOs[device_num].I_lRealHandle, (uint)CHCNetSDK.GOTO_PRESET, 65));
            Thread.Sleep(500);
            Console.WriteLine("NET_DVR_PTZPreset5");
            Console.WriteLine(NET_DVR_PTZPreset(real_PlayPOJOs[device_num].I_lRealHandle, (uint)CHCNetSDK.GOTO_PRESET, 24));
            Thread.Sleep(500);
            Console.WriteLine("NET_DVR_PTZPreset6");
            Console.WriteLine(NET_DVR_PTZPreset(real_PlayPOJOs[device_num].I_lRealHandle, (uint)CHCNetSDK.GOTO_PRESET, Speed_convert(speed)));
            Thread.Sleep(500);
            real_PlayPOJOs[device_num].messageList[5] = speed + "";

            deviceInfoList[MainWindow.Chosen_device_num].speed = int.Parse(speed);
            new TipsWindow("速度设置成功", 3, TipsEnum.OK).Show();


        }
        #endregion 速度
        /// <summary>
        /// 速度换算
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public uint Speed_convert(string speed)
        {
            try
            {
                switch (Convert.ToInt32(speed))
                {
                    case 1:
                        return 5;

                    case 3:
                        return 10;

                    case 5:
                        return 15;

                    case 7:
                        return 20;

                    case 9:
                        return 25;

                    case 11:
                        return 30;

                    case 13:
                        return 35;

                    case 15:
                        return 40;

                    default:
                        return 20;
                }
            }
            catch
            {
                return 20;
            }
        }

        /// <summary>
        /// 打开视频监视器
        /// </summary>
        /// <param name="i"></param>
        public bool OpenPreview(int chan_num, int device_num, bool HD_if)
        {
            if (real_PlayPOJOs[device_num].I_lUserID < 0)
            {
                MessageBox.Show("Please login the device firstly");
                real_PlayPOJOs.RemoveAt(device_num);
                return false;
            }
            if (real_PlayPOJOs[device_num].I_lRealHandle < 0)
            {
                NET_DVR_PREVIEWINFO lpPreviewInfo = new NET_DVR_PREVIEWINFO();

                lpPreviewInfo.hPlayWnd = cameras[device_num].Handle;//分配预览窗口
                lpPreviewInfo.lChannel = 1;//预览的设备通道
                lpPreviewInfo.dwStreamType = Convert.ToUInt16(HD_if ? 0 : 1);//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                lpPreviewInfo.dwLinkMode = 1;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP
                lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                lpPreviewInfo.dwDisplayBufNum = 1; //播放库播放缓冲区最大缓冲帧数
                lpPreviewInfo.byProtoType = 0;
                lpPreviewInfo.byPreviewMode = 0;

                if (RealData == null)
                {
                    RealData = new REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                }

                IntPtr pUser = new IntPtr();//用户数据

                //打开预览 Start live view
                real_PlayPOJOs[device_num].I_lRealHandle = NET_DVR_RealPlay_V40(real_PlayPOJOs[device_num].I_lUserID, ref lpPreviewInfo, RealData, pUser);
                Console.WriteLine("打开预览失败:" + CHCNetSDK.NET_DVR_GetLastError());
                if (real_PlayPOJOs[device_num].I_lRealHandle < 0)
                {
                    real_PlayPOJOs.RemoveAt(device_num);
                    MessageBox.Show("NET_DVR_RealPlay_V40 failed, error code= " + NET_DVR_GetLastError(), "预览错误");
                    return false;
                }
                else
                {
                    real_PlayPOJOs[device_num].messagePOJO = new MessagePOJO
                    {
                        deviceNum = real_PlayPOJOs[device_num].deviceNum
                    };

                    //画图回调函数
                    bool b = CHCNetSDK.NET_DVR_RigisterDrawFun(real_PlayPOJOs[device_num].I_lRealHandle, drawFunCallBack, 0);
                }
            }
            return true;
        }

        #region DRAWFUN

        public static DRAWFUN drawFunCallBack = DrawFunCallBack;
        public static List<DRAWFUN> drawFunCallBacks = new List<DRAWFUN>();

        public static void DrawFunCallBack(int lRealHandle, IntPtr hDc, uint dwUser)
        {
            try
            {
                int index = real_PlayPOJOs.FindIndex(item => item.I_lRealHandle.Equals(lRealHandle));

                System.Drawing.Graphics g = System.Drawing.Graphics.FromHdc(hDc);
                if (null == g)
                    return;

                g.DrawString("·" + real_PlayPOJOs[index].Device_name, new System.Drawing.Font("Verdana", 15), new System.Drawing.SolidBrush(System.Drawing.Color.Crimson), new System.Drawing.PointF(20.0f, 75.0f));
                g.Dispose();
            }
            catch { }
        }

        #endregion DRAWFUN

        public void R_LoginCallBack(int lUserID, int dwResult, IntPtr lpDeviceInfo, IntPtr pUser)
        {
            string strLoginCallBack = "登录设备，lUserID：" + lUserID + "，dwResult：" + dwResult;

            if (dwResult == 0)
            {
                uint iErrCode = NET_DVR_GetLastError();
                strLoginCallBack = strLoginCallBack + "，错误号:" + iErrCode;
            }

            //下面代码注释掉也会崩溃
            /*        if (InvokeRequired)
                    {
                        object[] paras = new object[2];
                        paras[0] = strLoginCallBack;
                        paras[1] = lpDeviceInfo;
                    }*/
        }

        /// <summary>
        /// ？？？
        /// </summary>
        /// <param name="lRealHandle"></param>
        /// <param name="dwDataType"></param>
        /// <param name="pBuffer"></param>
        /// <param name="dwBufSize"></param>
        /// <param name="pUser"></param>
        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
            if (dwBufSize > 0)
            {
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Time.Text = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
        }

        public static string GetResourcePath(string resourceName)
        {
            return $"pack://application:,,,/Resources/{resourceName}";
        }

        private void initLoginInfoFile()
        {
            try
            {
                // 创建XmlDDocument对象，并装入xml文件
                XmlDocument xmlDoc = new XmlDocument();

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "LoginInfo.xml", settings);
                //xmlDoc.Load(reader);
                reader.Close();
            }
            catch (FileNotFoundException e)
            {
                //创建一个空的XML
                XmlDocument document = new XmlDocument();
                //声明头部
                XmlDeclaration dec = document.CreateXmlDeclaration("1.0", "utf-8", "yes");
                document.AppendChild(dec);

                //创建根节点
                XmlElement root = document.CreateElement("logininfo");
                document.AppendChild(root);


                //保存文档
                document.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "LoginInfo.xml");
                Console.WriteLine(e.Message);
            }
        }
        public void LoadHistoryMessageFile()
        {
            try
            {
                // 创建XmlDDocument对象，并装入xml文件
                XmlDocument xmlDoc = new XmlDocument();

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "HistoryMessages.xml", settings);
                xmlDoc.Load(reader);

                //xn 代表一个结点
                //xn.Name;//这个结点的名称
                //xn.Value;//这个结点的值
                //xn.ChildNodes;//这个结点的所有子结点
                //xn.ParentNode;//这个结点的父结点

                // 得到根节点bookstore
                XmlNode xn = xmlDoc.SelectSingleNode("historymessages");


                // 得到根节点的所有子节点
                XmlNodeList xnl = xn.ChildNodes;

                foreach (XmlNode xn1 in xnl)
                {
                    History_Message historyMessage = new History_Message();
                    // 将节点转换为元素，便于得到节点的属性值
                    XmlElement xe = (XmlElement)xn1;
                    // 得到Type和ISBN两个属性的属性值
                    //bookModel.BookISBN = xe.GetAttribute("ISBN").ToString();
                    //bookModel.BookType = xe.GetAttribute("Type").ToString();
                    // 得到LoginInfo节点的所有子节点
                    XmlNodeList xnl0 = xe.ChildNodes;
                    historyMessage.pid = int.Parse(xnl0.Item(0).InnerText);
                    historyMessage.device_IP = xnl0.Item(1).InnerText;
                    historyMessage.device_name = xnl0.Item(2).InnerText;
                    historyMessage.save_time = DateTime.Parse(xnl0.Item(3).InnerText);
                    historyMessage.Horiz = xnl0.Item(4).InnerText;
                    historyMessage.Vert = xnl0.Item(5).InnerText;
                    historyMessage.concentration = xnl0.Item(6).InnerText;
                    historyMessage.Preset_num = int.Parse(xnl0.Item(7).InnerText);
                    historyMessage.Preset_notes = xnl0.Item(8).InnerText;
                    historyMessage.video_path = xnl0.Item(9).InnerText;

                    if (bool.Parse(xnl0.Item(10).InnerText))
                    {
                        historyMessage.isManul = true;
                        if (language.Equals(Language.Chinese))
                        {
                            historyMessage.isManulStr = "手动";
                        }
                        else { historyMessage.isManulStr = "manul"; }

                    }
                    else
                    {
                        historyMessage.isManul = false;
                        if (language.Equals(Language.Chinese))
                        {
                            historyMessage.isManulStr = "巡航";
                        }
                        else { historyMessage.isManulStr = "cruise"; }

                    }

                    historyMessages.Add(historyMessage);
                }
                if (historyMessages.Count > 0)
                {
                    maxPid = historyMessages.Max(h => h.pid);
                }

                reader.Close();
            }
            catch (FileNotFoundException e)
            {
                //创建一个空的XML
                XmlDocument document = new XmlDocument();
                //声明头部
                XmlDeclaration dec = document.CreateXmlDeclaration("1.0", "utf-8", "yes");
                document.AppendChild(dec);

                //创建根节点
                XmlElement root = document.CreateElement("historymessages");
                document.AppendChild(root);


                //保存文档
                document.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "HistoryMessages.xml");
                Console.WriteLine(e.Message);
            }

        }
        public void changeWindow(object sender, EventArgs e)
        {
            System.Windows.Forms.PictureBox pictureBox1 = (System.Windows.Forms.PictureBox)sender;
            if (pictureBox1 != null && pictureBox1 == pictureBoxHost0.Child)
            {
                return;
            }

            // 将sender转型成PictureBox
            PictureBox pic = sender as PictureBox;

            if (null == pic) return;

            string name = pic.Name; // 取出pictureBox的名称
                                    // 以下就你读取到的名称去处理你要做的事情
            if (name.Equals("m_pictureBox1") && real_PlayPOJOs.Count >= 2)
            {
                string color1 = "nomal";
                bool hd1 = false;
                Status value0 = map1["0"];
                for (int i = 0; i < windowsFormsHosts.Count; i++)
                {
                    if (windowsFormsHosts[i] != null && windowsFormsHosts[i].Child == m_pictureBox1)
                    {
                        color1 = map1[i.ToString()].color;
                        hd1 = map1[i.ToString()].hd;
                        map1.Remove(i.ToString());
                        map1.Add(i.ToString(), value0);
                        windowsFormsHosts[i].Child = m_pictureBoxTemp;
                        map1.Remove(i.ToString());
                        Status status1 = new Status();
                        status1.color = map1["0"].color;
                        status1.hd = map1["0"].hd;
                        status1.index = Chosen_device_num;
                        map1.Add(i.ToString(), status1);
                    }
                }
                Status status10 = new Status();
                status10.color = color1;
                status10.index = 1;
                status10.hd = hd1;
                map1.Remove("0");
                map1.Add("0", status10);
                m_pictureBoxTemp = m_pictureBox1;
                Chosen_device_num = 1;
                pictureBoxHost0.Child = m_pictureBox1;


            }
            if (name.Equals("m_pictureBox2") && real_PlayPOJOs.Count >= 3)
            {
                string color2 = "nomal";
                bool hd2 = false;
                Status value0 = map1["0"];
                for (int i = 0; i < windowsFormsHosts.Count; i++)
                {
                    if (windowsFormsHosts[i] != null && windowsFormsHosts[i].Child == m_pictureBox2)
                    {
                        color2 = map1[i.ToString()].color;
                        hd2 = map1[i.ToString()].hd;
                        map1.Remove(i.ToString());
                        map1.Add(i.ToString(), value0);
                        windowsFormsHosts[i].Child = m_pictureBoxTemp;
                        map1.Remove(i.ToString());
                        Status status2 = new Status();
                        status2.color = map1["0"].color;
                        status2.hd = map1["0"].hd;
                        status2.index = Chosen_device_num;
                        map1.Add(i.ToString(), status2);
                    }
                }
                Status status20 = new Status();
                status20.color = color2;
                status20.index = 2;
                status20.hd = hd2;
                map1.Remove("0");
                map1.Add("0", status20);
                m_pictureBoxTemp = m_pictureBox2;
                Chosen_device_num = 2;
                pictureBoxHost0.Child = m_pictureBox2;


            }
            if (name.Equals("m_pictureBox3") && real_PlayPOJOs.Count >= 4)
            {
                string color3 = "nomal";
                bool hd3 = false;
                Status value0 = map1["0"];
                for (int i = 0; i < windowsFormsHosts.Count; i++)
                {
                    if (windowsFormsHosts[i] != null && windowsFormsHosts[i].Child == m_pictureBox3)
                    {
                        color3 = map1[i.ToString()].color;
                        hd3 = map1[i.ToString()].hd;
                        map1.Remove(i.ToString());
                        map1.Add(i.ToString(), value0);
                        windowsFormsHosts[i].Child = m_pictureBoxTemp;
                        map1.Remove(i.ToString());
                        Status status3 = new Status();
                        status3.color = map1["0"].color;
                        status3.hd = map1["0"].hd;
                        status3.index = Chosen_device_num;
                        map1.Add(i.ToString(), status3);
                    }
                }
                map1.Remove("0");
                Status status30 = new Status();
                status30.color = color3;
                status30.hd = hd3;
                status30.index = 3;
                map1.Add("0", status30);
                m_pictureBoxTemp = m_pictureBox3;
                Chosen_device_num = 3;
                pictureBoxHost0.Child = m_pictureBox3;


            }
            if (name.Equals("m_pictureBox4") && real_PlayPOJOs.Count >= 5)
            {
                string color4 = "nomal";
                bool hd4 = false;
                Status value0 = map1["0"];
                for (int i = 0; i < windowsFormsHosts.Count; i++)
                {
                    if (windowsFormsHosts[i] != null && windowsFormsHosts[i].Child == m_pictureBox4)
                    {
                        color4 = map1[i.ToString()].color;
                        hd4 = map1[i.ToString()].hd;
                        map1.Remove(i.ToString());
                        map1.Add(i.ToString(), value0);
                        windowsFormsHosts[i].Child = m_pictureBoxTemp;
                        map1.Remove(i.ToString());
                        Status status4 = new Status();
                        status4.color = map1["0"].color;
                        status4.hd = map1["0"].hd;
                        status4.index = Chosen_device_num;
                        map1.Add(i.ToString(), status4);
                    }
                }
                map1.Remove("0");
                Status status40 = new Status();
                status40.color = color4;
                status40.hd = hd4;
                status40.index = 4;
                map1.Add("0", status40);
                m_pictureBoxTemp = m_pictureBox4;
                Chosen_device_num = 4;
                pictureBoxHost0.Child = m_pictureBox4;


            }
            if (name.Equals("m_pictureBox0"))
            {
                string color0 = "nomal";
                bool hd0 = false;
                for (int i = 0; i < windowsFormsHosts.Count; i++)
                {
                    if (windowsFormsHosts[i] != null && windowsFormsHosts[i].Child == m_pictureBox0)
                    {

                        color0 = map1[i.ToString()].color;
                        hd0 = map1[i.ToString()].hd;
                        windowsFormsHosts[i].Child = m_pictureBoxTemp;
                        map1.Remove(i.ToString());
                        Status status0 = new Status();
                        status0.color = map1["0"].color;
                        status0.hd = map1["0"].hd;
                        status0.index = Chosen_device_num;
                        map1.Add(i.ToString(), status0);
                    }
                }
                Status status00 = new Status();
                status00.color = color0;
                status00.hd = hd0;
                status00.index = 0;
                map1.Remove("0");
                map1.Add("0", status00);
                pictureBoxHost0.Child = m_pictureBox0;
                m_pictureBoxTemp = m_pictureBox0;
                Chosen_device_num = 0;
            }
            try
            {
                foreach (KeyValuePair<string, Status> kvp in map1)
                {
                    Console.WriteLine("sddsfsd");
                    bool b0 = kvp.Key.Equals("0");
                    bool b1 = kvp.Key.Equals("1");
                    bool b2 = kvp.Key.Equals("2");
                    bool b3 = kvp.Key.Equals("3");
                    Console.WriteLine(kvp.Key + ":" + kvp.Key.Equals("0"));
                    Console.WriteLine(kvp.Key + ":" + kvp.Key.Equals("1"));
                    Console.WriteLine(kvp.Key + ":" + kvp.Key.Equals("2"));
                    Console.WriteLine(kvp.Key + ":" + kvp.Key.Equals("3"));
                    if (kvp.Key.Equals("0"))
                    {
                        Console.WriteLine("sddsfsd");
                        Console.WriteLine("nomal:" + kvp.Value.color.Equals("nomal"));
                        Console.WriteLine("red:" + kvp.Value.color.Equals("red"));
                        Console.WriteLine("green:" + kvp.Value.color.Equals("green"));
                        if (kvp.Value.color.Equals("nomal"))
                        {
                            MaxNDText.Background = Brushes.Transparent;
                        }
                        else if (kvp.Value.color.Equals("red"))
                        {
                            MaxNDText.Background = Brushes.Red;
                        }
                        else if (kvp.Value.color.Equals("green"))
                        {
                            MaxNDText.Background = Brushes.Green;
                        }
                    }
                    else if (kvp.Key.Equals("1"))
                    {
                        Console.WriteLine("sddsfsd");
                        Console.WriteLine("nomal:" + kvp.Value.color.Equals("nomal"));
                        Console.WriteLine("red:" + kvp.Value.color.Equals("red"));
                        Console.WriteLine("green:" + kvp.Value.color.Equals("green"));
                        if (kvp.Value.color.Equals("nomal"))
                        {
                            concentrationText1.Background = Brushes.Transparent;
                        }
                        else if (kvp.Value.color.Equals("red"))
                        {
                            concentrationText1.Background = Brushes.Red;
                        }
                        else if (kvp.Value.color.Equals("green"))
                        {
                            concentrationText1.Background = Brushes.Green;
                        }

                    }
                    else if (kvp.Key.Equals("2"))
                    {
                        Console.WriteLine("sddsfsd");
                        Console.WriteLine("nomal:" + kvp.Value.color.Equals("nomal"));
                        Console.WriteLine("red:" + kvp.Value.color.Equals("red"));
                        Console.WriteLine("green:" + kvp.Value.color.Equals("green"));
                        if (kvp.Value.color.Equals("nomal"))
                        {
                            concentrationText2.Background = Brushes.Transparent;
                        }
                        else if (kvp.Value.color.Equals("red"))
                        {
                            concentrationText2.Background = Brushes.Red;
                        }
                        else if (kvp.Value.color.Equals("green"))
                        {
                            concentrationText2.Background = Brushes.Green;
                        }


                    }
                    else if (kvp.Key.Equals("3"))
                    {
                        Console.WriteLine("sddsfsd");
                        Console.WriteLine("nomal:" + kvp.Value.color.Equals("nomal"));
                        Console.WriteLine("red:" + kvp.Value.color.Equals("red"));
                        Console.WriteLine("green:" + kvp.Value.color.Equals("green"));
                        if (kvp.Value.color.Equals("nomal"))
                        {
                            concentrationText3.Background = Brushes.Transparent;
                        }
                        else if (kvp.Value.color.Equals("red"))
                        {
                            concentrationText3.Background = Brushes.Red;
                        }
                        else if (kvp.Value.color.Equals("green"))
                        {
                            concentrationText3.Background = Brushes.Green;
                        }


                    }
                    else if (kvp.Key.Equals("4"))
                    {
                        Console.WriteLine("sddsfsd");
                        Console.WriteLine("nomal:" + kvp.Value.color.Equals("nomal"));
                        Console.WriteLine("red:" + kvp.Value.color.Equals("red"));
                        Console.WriteLine("green:" + kvp.Value.color.Equals("green"));
                        if (kvp.Value.color.Equals("nomal"))
                        {
                            concentrationText4.Background = Brushes.Transparent;
                        }
                        else if (kvp.Value.color.Equals("red"))
                        {
                            concentrationText4.Background = Brushes.Red;
                        }
                        else if (kvp.Value.color.Equals("green"))
                        {
                            concentrationText4.Background = Brushes.Green;
                        }


                    }


                }
            }
            catch (Exception e2)
            {
                Console.WriteLine(e2.ToString());
            }

            reloadCruiseData();
        }


        private void AlarmHistoryDetailWindow(object sender, RoutedEventArgs e)
        {
            History_Message rowView = (History_Message)((Button)e.Source).DataContext;
            int index = MainWindow.historyMessages.FindIndex(item => item.save_time.ToString("yyyy-MM-dd HH:mm:ss").Equals(rowView.save_time.ToString("yyyy-MM-dd HH:mm:ss")));
            new AlarmHistoryDetail(index).Show();
        }


        public MainWindow()
        {
            InitializeComponent();

            /*         string path = Environment.CurrentDirectory;

                     path = System.IO.Path.Combine(path, "prt0001.stl");
                     Console.Write("path");
                     Console.WriteLine(path);
                     ModelImporter modelImporter = new ModelImporter();

                     model3DGroup = modelImporter.Load(path);
                     geometryModel3D = (GeometryModel3D)model3DGroup.Children[0];
                     DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));
                     geometryModel3D.Material = material;

                     transform3DGroup = new Transform3DGroup();

                     double scalex = 1;
                     ScaleTransform3D scale = new ScaleTransform3D(scalex, scalex, scalex);
                     transform3DGroup.Children.Add(scale);
                     model3DGroup.Transform = transform3DGroup;
         */

            /*        TranslateTransform3D translateTransform = new TranslateTransform3D();
                    translateTransform.OffsetZ = -50; // 沿Z轴移动10个单位
                    model3DGroup.Transform = translateTransform;
        */
            /*
                        modelVisual3D = new ModelVisual3D();
                        modelVisual3D.Content = model3DGroup;
                        viewport.Children.Add(modelVisual3D);
            */





            initLoginInfoFile();

            ResourceDictionary resourceDictionary;
            string languageStr = ConfigurationManager.AppSettings["Language"];
            if (languageStr.Equals("english"))
            {
                language = Language.English;

                string english = "pack://application:,,,/Language/English.xaml";
                resourceDictionary = new ResourceDictionary { Source = new Uri(english, UriKind.RelativeOrAbsolute) };
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                //bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/online_active_dot.png", UriKind.Relative);
                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/english-filled.png", UriKind.Relative);
                bitmapImage.EndInit();
                langImg.Source = bitmapImage;
                hdLabel1.Content = "hd";
                hdLabel2.Content = "hd";
                onlineLable1.Content = "online";
                onlineLable2.Content = "online";
            }
            else
            {
                language = Language.Chinese;

                string chinese = "pack://application:,,,/Language/Chinese.xaml";
                resourceDictionary = new ResourceDictionary { Source = new Uri(chinese, UriKind.RelativeOrAbsolute) };
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                //bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/online_active_dot.png", UriKind.Relative);
                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/chinese-filled.png", UriKind.Relative);
                bitmapImage.EndInit();
                langImg.Source = bitmapImage;
                hdLabel1.Content = "高清";
                hdLabel2.Content = "高清";
                onlineLable1.Content = "在线";
                onlineLable2.Content = "在线";
            }


            // 将当前的资源字典从应用程序资源中移除
            Resources.MergedDictionaries.Remove(resourceDictionary);
            // 将新的资源字典添加到应用程序资源中
            Resources.MergedDictionaries.Add(resourceDictionary);


            disapearSuccessTipsTimer = new DispatcherTimer();
            disapearTipsTimer = new DispatcherTimer();
            Status status0 = new Status();
            status0.color = "nomal";
            status0.index = 0;
            status0.online = false;
            status0.hd = false;
            map1.Add("0", status0);
            Status status1 = new Status();
            status1.color = "nomal";
            status1.index = 1;
            status1.online = false;
            status1.hd = false;
            map1.Add("1", status1);
            Status status2 = new Status();
            status2.color = "nomal";
            status2.index = 2;
            status2.online = false;
            status2.hd = false;
            map1.Add("2", status2);
            Status status3 = new Status();
            status3.color = "nomal";
            status3.index = 3;
            status3.online = false;
            status3.hd = false;
            map1.Add("3", status3);
            Status status4 = new Status();
            status4.color = "nomal";
            status4.index = 4;
            status4.online = false;
            status4.hd = false;
            map1.Add("4", status4);
            LoadHistoryMessageFile();

            AlarmHistoryDataGrid.ItemsSource = historyMessages;


            /*       //初始化历史数据

                   UsualHistoryMessages = LoadUsualHistoryMessagesFileToInstance();
                   Usual_historys = new List<List<int>>();*/

            aTimer = new System.Timers.Timer(1000);

            // 设置Elapsed事件处理程序
            aTimer.Elapsed += cruiseTimer;

            // 设置定时器启动时是否立即调用Elapsed事件处理程序
            aTimer.AutoReset = true;

            // 启动定时器
            aTimer.Enabled = true;

            string namespaceName = Assembly.GetExecutingAssembly().GetName().Name.ToString();
            Assembly assembly = Assembly.GetExecutingAssembly();
            sp = new SoundPlayer(assembly.GetManifestResourceStream(namespaceName + ".Resources" + ".alarm.wav"));
            sp1 = new SoundPlayer(assembly.GetManifestResourceStream(namespaceName + ".Resources" + ".alarm1.wav"));

            NET_DVR_Init();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            real_PlayPOJOs = new List<Real_PlayPOJO>();

            TitleBar.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    DragMove();
            };

            BtMin.Click += (s, e) =>
            {
                WindowState = WindowState.Minimized;
            };

            BtMax.Click += (s, e) =>
            {
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            };

            BtClose.Click += (s, e) =>
            {
                Thread.CurrentThread.IsBackground = true;
                Environment.Exit(0);
                Close();
            };


            Tool.emptyCruises = new List<CruisePOJO>();
            for (int i = 0; i < 90; i++)
            {
                Tool.emptyCruises.Add(new CruisePOJO { preset_num = 0, time = 0, name = "空" });
            }

            In_Main_Form = this;
            real_PlayPOJOs = new List<Real_PlayPOJO>();
            SeriesCollection = new SeriesCollection
            {
                /*       new LineSeries
                          {
                              Title = "Line Series",
                              Values = new ChartValues<double> { 0, 200, 1000, 6000, 5899,  6100, 6111,  6000, 5666,  5999, 5888, 6055, 6122,  6001, 5889 },
                              PointGeometry = null, // 使点不可见
                              Stroke = Brushes.Silver,
                              StrokeThickness = 1,
                              Fill =Brushes.LightSkyBlue,

                          }*/
            };

            ValueList = new ChartValues<double>();

            LineSeries lineseries = new LineSeries
            {
                Title = "浓度123",
                PointGeometry = null, // 使点不可见
                Stroke = Brushes.Silver,
                StrokeThickness = 0,
                Fill = Brushes.LightSkyBlue,

            };
            //lineseries.DataLabels = true;
            lineseries.Values = ValueList;
            SeriesCollection.Add(lineseries);


      /*      SeriesCollection2 = new SeriesCollection
            {
                          new ColumnSeries
                           {
                               Title = "光强",
                               Values = new ChartValues<double> { 0, 0, 700, 0, 0 },
                               Fill =Brushes.LightSkyBlue,
                               Foreground=Brushes.White
                           }
                      ,
                      new ColumnSeries
                      {
                          Title = "Series 2",
                          Values = new ChartValues<double> { 900, 600, 400, 900 }
                      }
            };*/

      /*      ValueList2 = new ChartValues<double>();

            ColumnSeries columnSeries2 = new ColumnSeries
            {
                Title = "光强",
                Fill = Brushes.LightSkyBlue,
                Foreground = Brushes.White
            };
            //lineseries.DataLabels = true;
            columnSeries2.Values = ValueList2;
            SeriesCollection2.Add(columnSeries2);*/

            this.DataContext = this;

            m_pictureBox0 = new System.Windows.Forms.PictureBox();

            m_pictureBox0.Name = "m_pictureBox0";
            m_pictureBoxTemp = m_pictureBox0;
            m_pictureBox1 = new System.Windows.Forms.PictureBox();
            /*      m_tp = new ToolTip();
                  m_toolTip = new System.Windows.Forms.ToolTip();
                  m_toolTip.SetToolTip(m_pictureBox, "1");
                  m_tp.PlacementTarget = pictureBoxHost;*/
            m_pictureBox1.Name = "m_pictureBox1";

            m_pictureBox2 = new System.Windows.Forms.PictureBox();
            m_pictureBox2.Name = "m_pictureBox2";

            m_pictureBox3 = new System.Windows.Forms.PictureBox();
            m_pictureBox3.Name = "m_pictureBox3";

            m_pictureBox4 = new System.Windows.Forms.PictureBox();
            m_pictureBox4.Name = "m_pictureBox4";
            cameras = new List<PictureBox> { m_pictureBox0, m_pictureBox1, m_pictureBox2, m_pictureBox3, m_pictureBox4 };

            pictureBoxHost0.Child = m_pictureBox0;
            pictureBoxHost1.Child = m_pictureBox1;
            pictureBoxHost2.Child = m_pictureBox2;
            pictureBoxHost3.Child = m_pictureBox3;
            pictureBoxHost4.Child = m_pictureBox4;
            m_pictureBox0.Click += changeWindow;
            m_pictureBox1.Click += changeWindow;
            m_pictureBox2.Click += changeWindow;
            m_pictureBox3.Click += changeWindow;
            m_pictureBox4.Click += changeWindow;
            windowsFormsHosts = new List<WindowsFormsHost> { pictureBoxHost0, pictureBoxHost1, pictureBoxHost2, pictureBoxHost3, pictureBoxHost4 };



            //m_pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(picturebox_Paint);


            #region 云服务

            /*  System.Timers.Timer t = new System.Timers.Timer(5 * 1000 * 2);//实例化Timer类，设置间隔时间为10000毫秒；

              t.Elapsed += new System.Timers.ElapsedEventHandler(TCPTool.CreateClient);//到达时间的时候执行事件；

              t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；

              t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；*/

            #endregion 云服务

            #region 清理内存

            System.Timers.Timer tClear = new System.Timers.Timer(1 * 1000 * 60 * 5);//实例化Timer类，设置间隔时间为10000毫秒；

            tClear.Elapsed += new System.Timers.ElapsedEventHandler(ClearMemory);//到达时间的时候执行事件；

            tClear.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；

            tClear.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；

            #endregion 清理内存

            #region TcpServer




            /*      System.Timers.Timer saveHistoryTimer = new System.Timers.Timer(30 * 1000);//实例化Timer类，设置间隔时间为10000毫秒；

                  saveHistoryTimer.Elapsed += new System.Timers.ElapsedEventHandler(Save_usual_historys_timer_Tick);//到达时间的时候执行事件；

                  saveHistoryTimer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；

                  saveHistoryTimer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；*/





            //分配发送区与接收区大小
            // tcpServer = new TcpServer(2 * 1024, 2 * 1024);
            //获取本机IP，并以指定端口开启服务
            // tcpServer.Start(tcpServer.GetIP(), "5000");
            Tcp_Start(Tcp_GetIP(), "6001");

            #endregion TcpServer


        }
        #region 内存回收

        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory(object source, System.Timers.ElapsedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                //FrmMian为我窗体的类名
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        #endregion 内存回收


        public Socket Tcp_ServerSocket { get; set; }

        /// <summary>
        /// Sockets键值对数组
        /// </summary>

        /// <summary>
        /// Sockets键值对数组
        /// </summary>
        public static Dictionary<string, Socket> Tcp_Sockets { get; set; } = new Dictionary<string, Socket>();

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        public string Tcp_GetIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting local IP: " + ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Tcp_Start(string ip, string port)
        {
            //MessageBox.Show(ip + ":" + port);
            Tcp_ServerSocket = Tcp_CreateSocket(ip, port);
            Tcp_CreateThread();

        }

        /// <summary>
        /// 服务端向指定客户端发送消息
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <param name="data"></param>
        public void Tcp_Send(string key, byte[] data)
        {
            Socket socket = Tcp_Sockets[key];
            socket.Send(data);
        }


        /// <summary>
        /// 创建服务端Socket
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private Socket Tcp_CreateSocket(string ip, string port)
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress address = IPAddress.Parse(ip);
                IPEndPoint endPoint = new IPEndPoint(address, int.Parse(port));
                socket.Bind(endPoint);
                socket.Listen(50);
                return socket;
            }
            catch (System.Net.Sockets.SocketException e)
            {
                Console.WriteLine("socketException");
                Console.WriteLine(e);
                throw;
            }



        }

        /// <summary>
        /// 创建监听客户端的线程并启动
        /// </summary>
        private void Tcp_CreateThread()
        {
            Thread watchThread = new Thread(Tcp_WatchConnection);
            watchThread.IsBackground = true;
            watchThread.Start();
        }

        /// <summary>
        /// 监听客户端请求
        /// </summary>
        private void Tcp_WatchConnection()
        {
            while (true)
            {
                Socket clientSocket = null;
                try
                {
                    clientSocket = Tcp_ServerSocket.Accept();
                    IPEndPoint endPoint = (IPEndPoint)clientSocket.RemoteEndPoint;
                    string ip = endPoint.Address.ToString();
                    string port = endPoint.Port.ToString();
                    //保存客户端信息
                    //Tcp_Sockets.Add(ip + ":" + port, clientSocket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ddd");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("fff");
                    break;
                }
                ParameterizedThreadStart pts = new ParameterizedThreadStart(ClientReceiver);
                Thread clientThread = new Thread(pts)
                {
                    IsBackground = true
                };
                clientThread.Start(clientSocket);
            }
        }

        /// <summary>
        /// 接收到客户端的数据
        /// </summary>
        /// <param name="socket"></param>
        private void ClientReceiver(object socket)
        {
            Socket clientSocket = socket as Socket;
            int ReceiveBufferSize = 60;
            while (true)
            {
                byte[] buffer = new byte[ReceiveBufferSize];
                try
                {
                    if (clientSocket != null)
                    {
                        if (clientSocket.Receive(buffer) > 0)
                        {
                            // 测试
                            // clientSocket.Send(Tool.ModbusTCP_testResult(buffer));
                            //Console.WriteLine(Encoding.Default.GetString(buffer));

                            this.Dispatcher.BeginInvoke(new Change(R232Text), Encoding.Default.GetString(buffer), clientSocket);

                            //clientSocket.Send(System.Text.Encoding.Default.GetBytes("AABB 3 60000 1016 01 AABB"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ex123");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("ex123");
                    break;
                }
            }
        }




        #region 数据透传


        private void disapearSuccessTips(object sender, EventArgs e)
        {
            disapearSuccessTipsTimer.Stop();
            deviceSetup.loadingTips.Text = "";
        }

        private void disapearTips(object sender, EventArgs e)
        {
            disapearTipsTimer.Stop();

        }
        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="text"></param>
        private delegate void Change(string text, Socket socket);

        private string[] strArray;
        private string[] exStrArray = { "start", "stop", "beginjz", "wait", "failure", "okay", };
        private string strstart = "@start@";

        private void redPointSetup(object sender, RoutedEventArgs e)
        {
            redPointBool = !redPointBool;
            if (redPointBool)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/on.png", UriKind.Relative);
                bitmapImage.EndInit();
                redPointImage.Source = bitmapImage;
                redPointBlock.Visibility = Visibility.Visible;
            }
            else
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/off.png", UriKind.Relative);
                bitmapImage.EndInit();
                redPointImage.Source = bitmapImage;
                redPointBlock.Visibility = Visibility.Hidden;
            }
        }

        private void stopCruiseSetup(object sender, RoutedEventArgs e)
        {
            if (MainWindow.sbmc == "")
            {
                new TipsWindow("请先登录", 3, TipsEnum.FAIL).Show();
                return;
            }
            stopCruiseBool = !stopCruiseBool;
            if (stopCruiseBool)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/on.png", UriKind.Relative);
                bitmapImage.EndInit();
                stopCruiseImg.Source = bitmapImage;
            }
            else
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/off.png", UriKind.Relative);
                bitmapImage.EndInit();
                stopCruiseImg.Source = bitmapImage;
            }




        }

        private void algorithmaSetup(object sender, RoutedEventArgs e)
        {
            if (MainWindow.sbmc == "")
            {
                new TipsWindow("请先登录", 3, TipsEnum.FAIL).Show();
                return;
            }
            AlgorithmABool = !AlgorithmABool;
            if (AlgorithmABool)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/on.png", UriKind.Relative);
                bitmapImage.EndInit();
                algorithmaImg.Source = bitmapImage;
            }
            else
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/off.png", UriKind.Relative);
                bitmapImage.EndInit();
                algorithmaImg.Source = bitmapImage;
            }




        }
        private void R232Text(string text, Socket socket)
        {


            // Console.WriteLine("text:");
            Console.WriteLine("text:" + text);
            ndTimesShowLength++;
            string nmwd = "";
            double wd = 0;
            double spjd = 0;
            double czjd = 0;
            Random random = new Random();

            int device_num = -1;

            if (real_PlayPOJOs.Count < 1)
            {
                return;
            }

            #region 判断是否接收到异常字符

            for (int i = 0; i < exStrArray.Length; i++)
            {
                if (text.IndexOf(exStrArray[i]) >= 0)
                {
                    switch (i)
                    {
                        /*
                        case 0:
                            start_comboBox.Items.Add(system_time.Text);
                            break;

                        case 1:
                            stop_comboBox.Items.Add(system_time.Text);
                            break;
                         */
                        case 2:
                            //Growl.InfoGlobal("开始校准，请稍后。" + exStrArray[i]);
                            if (language == Language.Chinese)
                            {
                                deviceSetup.loadingTips.Text = "开始校准，请稍后。";
                            }
                            else
                            {
                                deviceSetup.loadingTips.Text = "please wait a moment";
                            }

                            break;
                        /*

                        case 3:
                            MessageBox.Show("校准中，请稍后。", "探头校准");
                            break;
                        */
                        case 4:
                            Tool.KillMessageBox("探头校准");
                            MessageBox.Show("校准失败。", "校准结束");
                            break;

                        case 5:
                            //Growl.SuccessGlobal("校准成功。");
                            if (language == Language.Chinese)
                            {
                                deviceSetup.loadingTips.Text = "校准成功。";
                            }
                            else
                            {
                                deviceSetup.loadingTips.Text = "operation successfully";
                            }

                            disapearSuccessTipsTimer.Interval = TimeSpan.FromSeconds(5);
                            disapearSuccessTipsTimer.Tick += disapearSuccessTips;
                            disapearSuccessTipsTimer.Start();
                            deviceSetup.loading.Visibility = Visibility.Hidden;
                            break;
                    }
                }
            }


            #endregion 判断是否接收到异常字符

            #region 处理字符串

            //用最后一个字符分割接收到的数据
            strArray = text.Split('C');
            //strArray = text.Split(strWall[strWall.Length - 1]);
            // strArray = text.Split(strWall[strWall.Length - 4]);
            try
            {
                if (nmwd.Length != 1)
                {
                    nmwd = Tool.TakeText(strArray);
                    if (nmwd.Length < 9)
                    {
                        return;
                    }
                }
                //  Console.WriteLine("nmwd" + nmwd);

                strArray = nmwd.Split('M');
                //平均值
                string ndStr = strArray[0];
                string maxValStr = "";
                //最大值
                //mnd = int.Parse(strArray[0]);

                //如果勾选最大值
                //if (AlgorithmABool)
                //{
                maxValStr = strArray[1].Split('W')[0];
                //}



                if (ndTimesShowLength % 1 == 0)
                {
                    strArray = strArray[1].Split('W');
                    //温度
                    strArray = strArray[1].Split('D');
                    wd = double.Parse(strArray[0]);
                    wd /= 100;
                    Console.WriteLine("温度：{0}", wd);
                    //temprature.Status = wd.ToString("0") + "℃";

                    //设备编号
                    // MessagePOJO.deviceNum = int.Parse(strArray[1]);
                    strArray = strArray[1].Split('E');
                    string deviceNum = strArray[0];
                    try
                    {
                        if (!Tcp_Sockets.ContainsKey(deviceNum))
                        {
                            Tcp_Sockets.Add(deviceNum, socket);
                        }
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("存过一次了");
                    }

                    //对device_num赋值
                    device_num = real_PlayPOJOs.FindIndex(item => item.messagePOJO.deviceNum.Equals(deviceNum));

                    Console.WriteLine(string.Format("device_num:{0}", device_num));
                    if (device_num < 0)
                    {
                        return;
                    }
                    double doubleND = 0;
                    if (AlgorithmABool)
                    {
                        doubleND = double.Parse(maxValStr);
                    }
                    else
                    {
                        doubleND = double.Parse(ndStr);
                    }





                    #region 给浓度赋值
                    if (double.Parse(real_PlayPOJOs[device_num].messageList[9].ToString()) < doubleND)
                    {
                        real_PlayPOJOs[device_num].messageList[9] = doubleND.ToString();
                    }

                    real_PlayPOJOs[device_num].messageList[7] = real_PlayPOJOs[device_num].messageList[6];
                    real_PlayPOJOs[device_num].messageList[6] = real_PlayPOJOs[device_num].messageList[0];
                    real_PlayPOJOs[device_num].messageList[0] = ndStr;
                    real_PlayPOJOs[device_num].messageList[1] = wd + "℃";
                    real_PlayPOJOs[device_num].messageList[2] = deviceNum;
                    real_PlayPOJOs[device_num].messageList[10] = maxValStr;

                    #endregion 给浓度赋值

                    //水平角度
                    strArray = strArray[1].Split('A');
                    spjd = Convert.ToDouble(int.Parse(strArray[0]) / 100 + "." + int.Parse(strArray[0]) % 100);

                    //horizontalAngle.Status = spjd.ToString("0.00") + "°";
                    //  horizontalValue_label.Text = spjd + "";
                    //real_PlayPOJOs[device_num].messageList[3] = spjd + "°";
                    real_PlayPOJOs[device_num].messageList[3] = spjd.ToString();
                    //垂直角度
                    strArray = strArray[1].Split('B');
                    if (strArray[0].Length > 1)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (strArray[0].Length <= 5)
                            {
                                break;
                            }
                            if (strArray[0].Length > 5)
                            {
                                strArray[0] = strArray[0].Substring(0, 5);
                            }
                        }

                        if (int.Parse(strArray[0]) > 18000)
                        {
                            czjd = 360 - Convert.ToDouble(int.Parse(strArray[0]) / 100 + "." + int.Parse(strArray[0]) % 100);
                        }
                        else if (int.Parse(strArray[0]) < 18000)
                        {
                            czjd = -Convert.ToDouble(int.Parse(strArray[0]) / 100 + "." + int.Parse(strArray[0]) % 100);
                        }
                    }
                    //verticalAngle.Status = czjd.ToString("0.00") + "°";
                    //real_PlayPOJOs[device_num].messageList[4] = czjd.ToString("0.00") + "°";
                    //real_PlayPOJOs[device_num].messageList[4] = czjd.ToString("0.00");
                    //光强

                    strArray = strArray[1].Split('C');
                    string lightIntensityStr = strArray[0];
                    real_PlayPOJOs[device_num].messageList[8] = lightIntensityStr;

                    real_PlayPOJOs[device_num].messageList[5] = deviceInfoList[Chosen_device_num].speed.ToString(); //todowrj

                    //后门
                    /*   if (device_num == Chosen_device_num)
                      {
                            if (roll_checkBox.Checked)
                             {
                                 nd += random.Next(0, 3000);
                                 real_PlayPOJOs[device_num].messageList[0] = nd + "";
                             }
                      }*/
                    Console.WriteLine("czjd");
                    Console.WriteLine(czjd);
                    real_PlayPOJOs[device_num].messageList[4] = Math.Round(czjd, 2).ToString();
                    //real_PlayPOJOs[device_num].I_nmwd_if = 0;
                    /*
                                        if (real_PlayPOJOs[device_num].messagePOJO.concentration < int.Parse(ndStr) && int.Parse(ndStr) < 100000)
                                        {
                                            real_PlayPOJOs[device_num].messagePOJO.concentration = int.Parse(ndStr);
                                        }
                                        real_PlayPOJOs[device_num].messagePOJO.temperature = (int)wd;
                                        real_PlayPOJOs[device_num].messagePOJO.alarmValue = bjz;*/
                    Message_update(device_num);
                    SystemWarning(device_num);

                }

            }
            catch (FormatException ex)
            {
                Console.WriteLine("1644" + ex + "");
                Console.WriteLine(ex.StackTrace + "");
                Console.WriteLine("nmwd" + nmwd);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "");
                Console.WriteLine(nmwd);
                Console.WriteLine(strArray.Length + text);
            }

            #endregion 处理字符串


            /*
                        if (device_num >= 0 && device_num < real_PlayPOJOs.Count)
                        {
                            if (nd > real_PlayPOJOs[device_num].I_concentration && nd > bjz)
                            {
                                real_PlayPOJOs[device_num].I_concentration = nd;
                            }
                        }*/
        }
        /// <summary>
        /// 更新浓度数据
        /// </summary>
        /// <param name="device_num"></param>
        private void Message_update(int device_num)
        {
            if (!map1[device_num.ToString()].online)
            {
                map1[device_num.ToString()].online = true;
            }


            foreach (KeyValuePair<string, Status> kvp in map1)
            {
                if (kvp.Value.index == device_num)
                {
                    if (kvp.Key.Equals("4"))
                    {
                        if (map1["4"].online == true)
                        {
                            /*          BitmapImage bitmapImage = new BitmapImage();
                                      bitmapImage.BeginInit();
                                      //bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/online_active_dot.png", UriKind.Relative);
                                      bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/online_active.png", UriKind.Relative);
                                      bitmapImage.EndInit();
                                      onlineImage2.Source = bitmapImage;*/
                            onlineLable4.Background = Brushes.Green;
                        }
                        concentrationText4.Text = real_PlayPOJOs[device_num].messageList[0];
                        if (language == Language.Chinese)
                        {
                            hdLabel4.Content = "高清";
                            onlineLable4.Content = "在线";
                        }
                        else
                        {
                            hdLabel4.Content = "hd";
                            onlineLable4.Content = "online";
                        }

                        if (map1["4"].hd == true)
                        {
                            /*    BitmapImage bitmapImage = new BitmapImage();
                                bitmapImage.BeginInit();
                                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/hd_active.png", UriKind.Relative);
                                bitmapImage.EndInit();
                                hd_image2.Source = bitmapImage;*/
                            hdLabel4.Background = Brushes.SkyBlue;
                        }
                        else
                        {
                            /*    BitmapImage bitmapImage = new BitmapImage();
                                bitmapImage.BeginInit();
                                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/hd.png", UriKind.Relative);
                                bitmapImage.EndInit();
                                hd_image2.Source = bitmapImage;*/
                            hdLabel4.Background = Brushes.Black;
                        }
                        deviceName4.Text = real_PlayPOJOs[device_num].Device_name;
                        break;
                    }

                    if (kvp.Key.Equals("3"))
                    {
                        if (map1["3"].online == true)
                        {
                            /*          BitmapImage bitmapImage = new BitmapImage();
                                      bitmapImage.BeginInit();
                                      //bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/online_active_dot.png", UriKind.Relative);
                                      bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/online_active.png", UriKind.Relative);
                                      bitmapImage.EndInit();
                                      onlineImage2.Source = bitmapImage;*/
                            onlineLable3.Background = Brushes.Green;

                        }
                        concentrationText3.Text = real_PlayPOJOs[device_num].messageList[0];
                        if (language == Language.Chinese)
                        {
                            hdLabel3.Content = "高清";
                            onlineLable3.Content = "在线";
                        }
                        else
                        {
                            hdLabel3.Content = "hd";
                            onlineLable3.Content = "online";
                        }

                        if (map1["3"].hd == true)
                        {
                            /*    BitmapImage bitmapImage = new BitmapImage();
                                bitmapImage.BeginInit();
                                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/hd_active.png", UriKind.Relative);
                                bitmapImage.EndInit();
                                hd_image2.Source = bitmapImage;*/
                            hdLabel3.Background = Brushes.SkyBlue;
                        }
                        else
                        {
                            /*    BitmapImage bitmapImage = new BitmapImage();
                                bitmapImage.BeginInit();
                                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/hd.png", UriKind.Relative);
                                bitmapImage.EndInit();
                                hd_image2.Source = bitmapImage;*/
                            hdLabel3.Background = Brushes.Black;
                        }
                        deviceName3.Text = real_PlayPOJOs[device_num].Device_name;
                        break;
                    }

                    if (kvp.Key.Equals("2"))
                    {
                        if (map1["2"].online == true)
                        {
                            /*          BitmapImage bitmapImage = new BitmapImage();
                                      bitmapImage.BeginInit();
                                      //bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/online_active_dot.png", UriKind.Relative);
                                      bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/online_active.png", UriKind.Relative);
                                      bitmapImage.EndInit();
                                      onlineImage2.Source = bitmapImage;*/
                            onlineLable2.Background = Brushes.Green;

                        }
                        concentrationText2.Text = real_PlayPOJOs[device_num].messageList[0];
                        if (language == Language.Chinese)
                        {
                            hdLabel2.Content = "高清";
                            onlineLable2.Content = "在线";
                        }
                        else
                        {
                            hdLabel2.Content = "hd";
                            onlineLable2.Content = "online";
                        }

                        if (map1["2"].hd == true)
                        {
                            /*    BitmapImage bitmapImage = new BitmapImage();
                                bitmapImage.BeginInit();
                                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/hd_active.png", UriKind.Relative);
                                bitmapImage.EndInit();
                                hd_image2.Source = bitmapImage;*/
                            hdLabel2.Background = Brushes.SkyBlue;
                        }
                        else
                        {
                            /*    BitmapImage bitmapImage = new BitmapImage();
                                bitmapImage.BeginInit();
                                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/hd.png", UriKind.Relative);
                                bitmapImage.EndInit();
                                hd_image2.Source = bitmapImage;*/
                            hdLabel2.Background = Brushes.Black;
                        }
                        deviceName2.Text = real_PlayPOJOs[device_num].Device_name;
                        break;
                    }
                    if (kvp.Key.Equals("1"))
                    {
                        if (map1["1"].online == true)
                        {
                            /*       BitmapImage bitmapImage = new BitmapImage();
                                   bitmapImage.BeginInit();
                                   //bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/online_active_dot.png", UriKind.Relative);
                                   bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/online_active.png", UriKind.Relative);
                                   bitmapImage.EndInit();
                                   onlineImage1.Source = bitmapImage;*/
                            onlineLable1.Background = Brushes.Green;
                        }
                        concentrationText1.Text = real_PlayPOJOs[device_num].messageList[0];

                        if (language == Language.Chinese)
                        {
                            hdLabel1.Content = "高清";
                            onlineLable1.Content = "在线";
                        }
                        else
                        {
                            hdLabel1.Content = "hd";
                            onlineLable1.Content = "online";
                        }
                        if (map1["1"].hd == true)
                        {
                            /*BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/hd_active.png", UriKind.Relative);
                            bitmapImage.EndInit();
                            hd_image1.Source = bitmapImage;*/
                            hdLabel1.Background = Brushes.SkyBlue;

                        }
                        else
                        {
                            /*    BitmapImage bitmapImage = new BitmapImage();
                                bitmapImage.BeginInit();
                                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/hd.png", UriKind.Relative);
                                bitmapImage.EndInit();
                                hd_image1.Source = bitmapImage;*/
                            hdLabel1.Background = Brushes.Black;

                        }
                        deviceName1.Text = real_PlayPOJOs[device_num].Device_name;
                        break;
                    }
                    else if (kvp.Key.Equals("0"))
                    {


                        if (map1["0"].hd == true)
                        {
                            /*   BitmapImage bitmapImage = new BitmapImage();
                               bitmapImage.BeginInit();
                               bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/hd_active.png", UriKind.Relative);
                               bitmapImage.EndInit();
                               hd_image0.Source = bitmapImage;*/
                            hdLabel.Background = Brushes.SkyBlue;
                        }
                        else
                        {
                            /*     BitmapImage bitmapImage = new BitmapImage();
                                 bitmapImage.BeginInit();
                                 bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/hd.png", UriKind.Relative);
                                 bitmapImage.EndInit();
                                 hd_image0.Source = bitmapImage;*/
                            hdLabel.Background = Brushes.Black;
                        }

                        if (map1["0"].online == true)
                        {
                            /* OnlineText.Content = "在线";
                             OnlineText.Background = (Brush)new BrushConverter().ConvertFrom("#afe484");*/
                            /*     BitmapImage bitmapImage = new BitmapImage();
                                 bitmapImage.BeginInit();
                                 bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/online_active.png", UriKind.Relative);
                                 bitmapImage.EndInit();
                                 onlineImage.Source = bitmapImage;*/
                            onlineLable.Background = Brushes.Green;

                        }

                        //lightIntensity.Status = real_PlayPOJOs[device_num].messageList[8]; //光强
                        double num1 = double.Parse(real_PlayPOJOs[device_num].messageList[8]);
                        double percentNum = Math.Round(num1 / 8 * 100);
                        lightIntensityPercent.Text = percentNum.ToString() + "%";
                        switch (num1)
                        {
                            case 0:
                                BitmapImage bitmapImage0 = new BitmapImage();
                                bitmapImage0.BeginInit();
                                bitmapImage0.UriSource = new Uri("/WpfApp1;component/Resources/line0.png", UriKind.Relative);
                                bitmapImage0.EndInit();
                                lightIntensityImg.Source = bitmapImage0;
                                break;
                            case 1:
                                BitmapImage bitmapImage1 = new BitmapImage();
                                bitmapImage1.BeginInit();
                                bitmapImage1.UriSource = new Uri("/WpfApp1;component/Resources/line1.png", UriKind.Relative);
                                bitmapImage1.EndInit();
                                lightIntensityImg.Source = bitmapImage1;
                                break;
                            case 2:
                                BitmapImage bitmapImage2 = new BitmapImage();
                                bitmapImage2.BeginInit();
                                bitmapImage2.UriSource = new Uri("/WpfApp1;component/Resources/line2.png", UriKind.Relative);
                                bitmapImage2.EndInit();
                                lightIntensityImg.Source = bitmapImage2;
                                break;
                            case 3:
                                BitmapImage bitmapImage3 = new BitmapImage();
                                bitmapImage3.BeginInit();
                                bitmapImage3.UriSource = new Uri("/WpfApp1;component/Resources/line3.png", UriKind.Relative);
                                bitmapImage3.EndInit();
                                lightIntensityImg.Source = bitmapImage3;
                                break;
                            case 4:
                                BitmapImage bitmapImage4 = new BitmapImage();
                                bitmapImage4.BeginInit();
                                bitmapImage4.UriSource = new Uri("/WpfApp1;component/Resources/line4.png", UriKind.Relative);
                                bitmapImage4.EndInit();
                                lightIntensityImg.Source = bitmapImage4;
                                break;

                            case 5:
                                BitmapImage bitmapImage5 = new BitmapImage();
                                bitmapImage5.BeginInit();
                                bitmapImage5.UriSource = new Uri("/WpfApp1;component/Resources/line5.png", UriKind.Relative);
                                bitmapImage5.EndInit();
                                lightIntensityImg.Source = bitmapImage5;
                                break;
                            case 6:
                                BitmapImage bitmapImage6 = new BitmapImage();
                                bitmapImage6.BeginInit();
                                bitmapImage6.UriSource = new Uri("/WpfApp1;component/Resources/line6.png", UriKind.Relative);
                                bitmapImage6.EndInit();
                                lightIntensityImg.Source = bitmapImage6;
                                break;
                            case 7:
                                BitmapImage bitmapImage7 = new BitmapImage();
                                bitmapImage7.BeginInit();
                                bitmapImage7.UriSource = new Uri("/WpfApp1;component/Resources/line7.png", UriKind.Relative);
                                bitmapImage7.EndInit();
                                lightIntensityImg.Source = bitmapImage7;
                                break;
                            case 8:
                                BitmapImage bitmapImage8 = new BitmapImage();
                                bitmapImage8.BeginInit();
                                bitmapImage8.UriSource = new Uri("/WpfApp1;component/Resources/line8.png", UriKind.Relative);
                                bitmapImage8.EndInit();
                                lightIntensityImg.Source = bitmapImage8;
                                break;

                        }
                        MaxNDText.Text = real_PlayPOJOs[device_num].messageList[9];
                        if (AlgorithmABool)
                        {
                            NDText.Text = real_PlayPOJOs[device_num].messageList[10];
                        }
                        else
                        {
                            NDText.Text = real_PlayPOJOs[device_num].messageList[0];
                        }
                        temprature.Status = real_PlayPOJOs[device_num].messageList[1];
                        horizontalAngle.Status = real_PlayPOJOs[device_num].messageList[3];
                        verticalAngle.Status = real_PlayPOJOs[device_num].messageList[4] + "°";
                        speedText.Status = real_PlayPOJOs[device_num].messageList[5] + "°/s";

                        /*              float anglex1 = float.Parse(real_PlayPOJOs[Chosen_device_num].messageList[3]);
                                      float angley1 = float.Parse(real_PlayPOJOs[Chosen_device_num].messageList[4]);

                                      if (anglexTemp != anglex1)
                                      {
                                          rotate = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -(anglex1 - anglexTemp)));
                                          transform3DGroup.Children.Add(rotate);
                                          anglexTemp = anglex1;
                                      }
                                      if (angleyTemp != angley1)
                                      {
                                          Console.WriteLine("bbb");
                                          Console.WriteLine(-(angley1 - angleyTemp));
                                          rotate = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), anglexTempaaa));
                                          transform3DGroup.Children.Add(rotate);
                                          rotate2 = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), -(angley1 - angleyTemp)));
                                          transform3DGroup.Children.Add(rotate2);
                                          anglexTempaaa = angleyTemp;
                                          angleyTemp = angley1;
                                      }*/





                        //transform3DGroup.Children.Clear();



                        /*     model3DGroup.Transform = transform3DGroup;


                             modelVisual3D.Content = model3DGroup;
                             //viewport.Children.Clear();
                             viewport.Children.Add(modelVisual3D);*/

                        if (ndTimesShowLength % 2 == 0)
                        {
                            double anglex = double.Parse(real_PlayPOJOs[device_num].messageList[3]);

                            double x2 = 90 + 60 * Math.Cos((anglex - 180) * 3.14 / 180);
                            double y2 = 60 + 60 * Math.Sin((anglex - 180) * 3.14 / 180);

                            // 创建Line对象
                            Line line = new Line
                            {
                                X1 = 90, // 起点横坐标
                                Y1 = 60, // 起点纵坐标
                                X2 = x2, // 终点横坐标
                                Y2 = y2, // 终点纵坐标
                                Stroke = Brushes.Green, // 线条颜色
                                StrokeThickness = 2 // 线条宽度
                            };
                            canvasx.Children.Clear();
                            // 将线条添加到Canvas
                            canvasx.Children.Add(line);


                            double angley = double.Parse(real_PlayPOJOs[device_num].messageList[4]);

                            double x3 = 268 + 60 * Math.Cos((angley - 180) * 3.14 / 180);
                            double y3 = 60 + 60 * Math.Sin((angley - 180) * 3.14 / 180);

                            // 创建Line对象
                            Line line2 = new Line
                            {
                                X1 = 268, // 起点横坐标
                                Y1 = 60, // 起点纵坐标
                                X2 = x3, // 终点横坐标
                                Y2 = y3, // 终点纵坐标
                                Stroke = Brushes.Green, // 线条颜色
                                StrokeThickness = 2 // 线条宽度
                            };

                            canvasy.Children.Clear();
                            // 将线条添加到Canvas
                            canvasy.Children.Add(line2);


                        }



                        Console.WriteLine("浓度：{0}", real_PlayPOJOs[device_num].messageList[0]);

                        double doubleND = double.Parse(real_PlayPOJOs[device_num].messageList[0]);

                        if (ValueList.Count > 20)
                        {
                            ValueList.RemoveAt(0);
                        }

                        ValueList.Add(doubleND);

                        Console.WriteLine("添加了一个浓度值 ");

                    /*    if (ValueList2.Count == 0)
                        {
                            ValueList2.Add(0);
                            ValueList2.Add(0);
                            ValueList2.Add(double.Parse(real_PlayPOJOs[device_num].messageList[8]));
                            ValueList2.Add(0);
                            ValueList2.Add(0);
                            ValueList2.Add(0);
                        }
                        else
                        {
                            ValueList2.Clear();
                            ValueList2.Add(0);
                            ValueList2.Add(0);
                            ValueList2.Add(double.Parse(real_PlayPOJOs[device_num].messageList[8]));
                            ValueList2.Add(0);
                            ValueList2.Add(0);
                            ValueList2.Add(0);

                        }*/


                        break;
                    }
                }
                //Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }

            try
            {



                #endregion 更新柱状图

                /*            #region 保存常态历史数据

                            try
                            {

                                Usual_historys[device_num].Add(Convert.ToInt32(real_PlayPOJOs[device_num].messageList[0]));

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                            #endregion 保存常态历史数据*/

            }
            catch (Exception ex)
            {
                Console.WriteLine("Message_update:" + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// 录像委托
        /// </summary>
        /// <param name="text"></param>
        private delegate Task VideoSave(bool messageBox_showIf, int device_num, bool isManul);




        /// <summary>
        /// 系统声光警告
        /// </summary>
        public void SystemWarning(int device_num)
        {
            /*            if (Convert.ToInt32(messageList[device_num][0]) >= Convert.ToInt32(gbyz[device_num].Text) &&
                            Convert.ToInt32(messageList[device_num][6]) >= Convert.ToInt32(gbyz[device_num].Text))
            */
            if (device_num < 0 || device_num >= real_PlayPOJOs.Count)
            {
                return;
            }
            double HighAlarmDouble = double.Parse(real_PlayPOJOs[MainWindow.Chosen_device_num].I_gbyz.ToString());
            double bottomDoubleHigh = (176 - 24) / 6000.0 * HighAlarmDouble;
            Console.WriteLine("bottomDoubleHigh: " + ((176 - 24) / 6000.0 * 24 + bottomDoubleHigh));
            HightAlarmStackPanel.Margin = new Thickness(20, 0, 0, (176 - 24) / 6000.0 * 24 + bottomDoubleHigh + 24);
            HighAlarmNum.Text = HighAlarmDouble.ToString();

            double LowerAlarmDouble = double.Parse(real_PlayPOJOs[MainWindow.Chosen_device_num].I_dbyz.ToString());
            double bottomDoubleLower = Math.Round((176 - 24) / 6000.0 * LowerAlarmDouble, 2);
            Console.WriteLine("bottomDoubleLower: " + (176 / 6000.0 * 24 + bottomDoubleLower));
            LowerAlarmStackPanel.Margin = new Thickness(20, 0, 0, (176 - 24) / 6000.0 * 24 + bottomDoubleLower + 24);
            LowerAlarmNum.Text = LowerAlarmDouble.ToString();


            if (real_PlayPOJOs[device_num].Save_if)
            {
                real_PlayPOJOs[device_num].Save_nd.Add(real_PlayPOJOs[device_num].messageList[0]);
            }
            if (Convert.ToInt32(real_PlayPOJOs[device_num].messageList[0]) >= real_PlayPOJOs[device_num].I_gbyz)
            {

                if (!real_PlayPOJOs[device_num].B_bRecord)
                {
                    this.Dispatcher.BeginInvoke(new VideoSave(AutoSaveAlarmInfo), true, device_num, !real_PlayPOJOs[MainWindow.Chosen_device_num].B_isAuto);
                }

                /*      if (real_PlayPOJOs[device_num].B_isAuto)
                      {
                          if (stopCruiseWhenWarning.IsChecked.GetValueOrDefault())
                          {
                              StopYTAutoMove(true, device_num);
                          }
                      }*/

                // this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                //sp1.Stop();
                //Camera_panels[device_num].BackColor = Color.FromArgb(255, 0, 0);
                //播放警报音
                if (!real_PlayPOJOs[device_num].B_isGBaoJingZhong)
                {
                    if (real_PlayPOJOs[device_num].B_isDBaoJingZhong)
                    {
                        sp1.Stop();
                    }

                    sp.PlayLooping();

                    foreach (KeyValuePair<string, Status> kvp in map1)
                    {
                        if (kvp.Value.index == device_num)
                        {
                            if (kvp.Key.Equals("0"))
                            {
                                map1["0"].color = "red";
                                MaxNDText.Background = Brushes.Red;
                                break;
                            }
                            else if (kvp.Key.Equals("1"))
                            {
                                map1["1"].color = "red";
                                concentrationText1.Background = Brushes.Red;
                                break;
                            }
                            else if (kvp.Key.Equals("2"))
                            {
                                map1["2"].color = "red";
                                concentrationText2.Background = Brushes.Red;
                                break;
                            }
                        }
                    }



                }

                real_PlayPOJOs[device_num].B_isGBaoJingZhong = true;
                real_PlayPOJOs[device_num].B_isDBaoJingZhong = false;
                //HighHistory_updata(device_num);
            }
            else if (Convert.ToInt32(real_PlayPOJOs[device_num].messageList[0]) >= real_PlayPOJOs[device_num].I_dbyz &&
                     Convert.ToInt32(real_PlayPOJOs[device_num].messageList[6]) >= real_PlayPOJOs[device_num].I_dbyz &&
                     Convert.ToInt32(real_PlayPOJOs[device_num].messageList[7]) >= real_PlayPOJOs[device_num].I_dbyz)
            {
                if (!real_PlayPOJOs[device_num].B_bRecord)
                {
                    this.Dispatcher.BeginInvoke(new VideoSave(AutoSaveAlarmInfo), true, device_num, !real_PlayPOJOs[MainWindow.Chosen_device_num].B_isAuto);
                }

                if (real_PlayPOJOs[device_num].B_isAuto)
                {
                    if (stopCruiseBool)
                    {
                        StopYTAutoMove(true, device_num);
                    }

                    // UnAuto_button_Click(null, null);
                    // SavePicture(true, device_num);
                }

                //播放警报音
                if (!real_PlayPOJOs[device_num].B_isDBaoJingZhong && !real_PlayPOJOs[device_num].B_isGBaoJingZhong)
                {

                    foreach (KeyValuePair<string, Status> kvp in map1)
                    {
                        if (kvp.Value.index == device_num)
                        {
                            if (kvp.Key.Equals("0"))
                            {
                                map1["0"].color = "green";
                                MaxNDText.Background = Brushes.Green;
                                break;
                            }
                            else if (kvp.Key.Equals("1"))
                            {
                                map1["1"].color = "green";
                                concentrationText1.Background = Brushes.Green;
                                break;
                            }
                            else if (kvp.Key.Equals("2"))
                            {
                                map1["2"].color = "green";
                                concentrationText2.Background = Brushes.Green;
                                break;
                            }
                        }
                    }


                    sp1.PlayLooping();
                }


                real_PlayPOJOs[device_num].B_isDBaoJingZhong = true;
                HighHistory_updata(device_num);
            }
        }

        /*        private void SaveRecord(object sender, RoutedEventArgs e)
                {
                    if (MainWindow.sbmc == "")
                    {
                        Growl.SuccessGlobal("请先登录");
                        return;
                    }

                    SaveRecord(true);
                }*/

        /*        public void SaveRecord(bool isShowMB)
                {
                    string sVideoFilePath = "";
                    string sVideoFileName = "";
                    if (real_PlayPOJOs[Chosen_device_num].Save_if)
                    {
                        MessageBox.Show("该设备正在自动录像中");
                        return;
                    }

                    //录像保存路径和文件名 the path and file name to save
                    //录像保存路径和文件名
                    sVideoFilePath = Application.StartupPath + "\\Record\\"
                                           + real_PlayPOJOs[Chosen_device_num].Device_name + "\\"
                                           + DateTime.Now.ToString("yyyy") + "\\"
                                           + DateTime.Now.ToString("MM") + "\\"
                                           + DateTime.Now.ToString("dd") + "\\";

                    Directory.CreateDirectory(sVideoFilePath);


                    *//*            string sVideoFileName;
                                sVideoFileName = "Record/" + DateTime.Now.ToString("yyyy-MM-dd-") + DateTime.Now.ToString("HH-mm-ss") + ".mp4";
                    *//*
                    string str = "";

                    if (real_PlayPOJOs[Chosen_device_num].B_bRecord == false)
                    {
                        sVideoFileName = sVideoFilePath + DateTime.Now.ToString("HH-mm-ss");
                        //强制I帧 Make a I frame
                        int lChannel = Int16.Parse("1"); //通道号 Channel number
                                                         //   CHCNetSDK.NET_DVR_MakeKeyFrame(real_PlayPOJOs[Chosen_device_num].I_lUserID, lChannel);

                        //开始录像 Start recording
                        if (!CHCNetSDK.NET_DVR_SaveRealData(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, sVideoFileName + ".wmv"))
                        {
                            if (isShowMB)
                            {
                                str = "图像录制错误";
                                Growl.SuccessGlobal(str);
                            }
                            return;
                        }
                        else
                        {
                            RecordBtn.Content = "停止";
                            real_PlayPOJOs[Chosen_device_num].B_bRecord = true;
                        }
                    }
                    else if (!sVideoFileName.Equals(""))
                    {
                        //停止录像 Stop recording
                        if (!CHCNetSDK.NET_DVR_StopSaveRealData(real_PlayPOJOs[Chosen_device_num].I_lRealHandle))
                        {
                            if (isShowMB)
                            {
                                str = "停止录像错误";
                                Growl.SuccessGlobal(str);
                            }

                            return;
                        }
                        else
                        {
                            if (isShowMB)
                            {
                                str = "录像保存成功，文件名为:" + sVideoFileName;
                                Growl.SuccessGlobal(str);
                            }
                            RecordBtn.Content = "录像";
                            real_PlayPOJOs[Chosen_device_num].B_bRecord = false;
                        }
                    }
                }*/

        /*        private void stopCruiseWhenWarningFun(object sender, RoutedEventArgs e)
                {
                    stopCruiseBool = stopCruiseWhenWarning.IsChecked.Value;
                }*/
        /// <summary>
        /// 自动录像
        /// </summary>
        /// <param name="messageShowToggleIsChecked"></param>
        /// <param name="device_num"></param>
        /// <param name="isManul"></param>
        public async Task AutoSaveAlarmInfo(bool messageShowToggleIsChecked, int device_num, bool isManul)
        {
            if (real_PlayPOJOs[device_num].B_bRecord)
            {
                //  MessageBox.Show("该设备正在录像中");
                return;
            }

            //录像保存路径和文件名
            string sVideoFilePath = Application.StartupPath + "\\Record\\"
                                    + real_PlayPOJOs[device_num].Device_name + "\\"
                                    + DateTime.Now.ToString("yyyy") + "\\"
                                    + DateTime.Now.ToString("MM") + "\\"
                                    + DateTime.Now.ToString("dd") + "\\";

            Directory.CreateDirectory(sVideoFilePath);

            string sVideoFileName;
            sVideoFileName = sVideoFilePath + DateTime.Now.ToString("HH-mm-ss");



            if (!isManul)
            {

                SaveHistoryMessagesToFile(device_num, sVideoFileName, isManul);

                //强制I帧 Make a I frame
                int lChannel = Int16.Parse("1"); //通道号 Channel number
                                                 // CHCNetSDK.NET_DVR_MakeKeyFrame(real_PlayPOJOs[device_num].I_lUserID, lChannel);

                //开始录像 Start recording
                if (!CHCNetSDK.NET_DVR_SaveRealData(real_PlayPOJOs[device_num].I_lRealHandle, sVideoFileName + ".wmv"))
                {
                    if (messageShowToggleIsChecked)
                    {
                        new TipsWindow("图像录制错误", 3, TipsEnum.FAIL).Show();
                    }
                    return;
                }
                else
                {
                    real_PlayPOJOs[device_num].B_bRecord = true;
                    real_PlayPOJOs[device_num].Save_if = true;
                }

                await Task.Delay(1010 * 18);

                //停止录像 Stop recording
                if (!CHCNetSDK.NET_DVR_StopSaveRealData(real_PlayPOJOs[device_num].I_lRealHandle))
                {
                    if (messageShowToggleIsChecked)
                    {
                        new TipsWindow("停止录像错误", 3, TipsEnum.FAIL).Show();
                    }

                    return;
                }
                else
                {
                    if (messageShowToggleIsChecked)
                    {
                        MessageBox.Show("录像保存成功，文件名为:" + sVideoFileName);

                    }
                    real_PlayPOJOs[device_num].B_bRecord = false;
                    real_PlayPOJOs[device_num].Save_if = false;

                    //  FileStream file = new FileStream(sVideoFilePath + DateTime.Now.ToString("/HH-mm-ss"), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    FileStream file = new FileStream(sVideoFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    using (var stream = new StreamWriter(file))
                    {
                        string str = JsonConvert.SerializeObject(real_PlayPOJOs[device_num].Save_nd);
                        stream.Write(str);
                        stream.Flush();
                        stream.Close();
                    }
                    real_PlayPOJOs[device_num].Save_nd = new List<string>();
                }
            }
            else
            {
                if (ndTimesShowLength % 1000 == 0)
                {
                    SaveHistoryMessagesToFile(device_num, sVideoFileName, isManul);
                }
            }

        }



        private void ResetAlarmValue(object sender, RoutedEventArgs e)
        {
            map1["0"].color = "nomal";
            if (real_PlayPOJOs[map1["0"].index].B_isGBaoJingZhong)
            {
                sp.Stop();
            }
            if (real_PlayPOJOs[map1["0"].index].B_isDBaoJingZhong)
            {
                sp1.Stop();
            }
            real_PlayPOJOs[map1["0"].index].B_isGBaoJingZhong = false;
            real_PlayPOJOs[map1["0"].index].B_isDBaoJingZhong = false;
            real_PlayPOJOs[map1["0"].index].messageList[9] = "0";
            MaxNDText.Text = "0";
            MaxNDText.Background = Brushes.Transparent;
        }
        /// <summary>
        /// 检测到泄漏，停止巡航并放大图像
        /// </summary>
        public void StopYTAutoMove(bool isSavePic, int device_num)
        {
            //停止巡台巡航旋转
            // int iSeq = Cruise_comBox.SelectedIndex + 1;    //+1
            if (!CHCNetSDK.NET_DVR_PTZCruise(real_PlayPOJOs[device_num].I_lRealHandle, CHCNetSDK.STOP_SEQ, (byte)1, 0, 0))
            {
                MessageBox.Show("调用巡航失败");
                return;
            }
            real_PlayPOJOs[device_num].B_isAuto = false;
            if (isSavePic)
            {
                //摄像头焦距放大
                CHCNetSDK.NET_DVR_PTZControl(real_PlayPOJOs[device_num].I_lRealHandle, CHCNetSDK.ZOOM_IN, 0);
                Thread.Sleep(1000);
                CHCNetSDK.NET_DVR_PTZControl(real_PlayPOJOs[device_num].I_lRealHandle, CHCNetSDK.ZOOM_IN, 1);
                Thread.Sleep(1000);
                //拍摄照片
                //SavePicture(true, int.Parse(nongdu_text.Text));
            }
        }

        private void Ndwd_if_timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < real_PlayPOJOs.Count; i++)
            {
                if (real_PlayPOJOs[i].I_nmwd_if > 20)
                {
                    real_PlayPOJOs[i].work_if = false;
                }
                if (real_PlayPOJOs[i].I_nmwd_if > 600)
                {
                    real_PlayPOJOs[i].I_nmwd_if = 0;
                    string str = "@start@";
                    if (!NET_DVR_SerialSend(real_PlayPOJOs[i].I_lUserID, 1, str, (uint)str.Length))
                    {
                        //TODO   Tool.KillMessageBox("开启激光探头");
                        //TODO   MessageBox.Show("发送失败,err=" + NET_DVR_GetLastError(), "开启激光探头");
                    }
                }
                real_PlayPOJOs[i].I_nmwd_if++;
            }
        }

        private bool Usual_historys_save_if = false;
        private int[] Chart_update_if = { 11, 9, 7, 5 };


        /// <summary>
        /// 保存常态历史数据定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*        private void Save_usual_historys_timer_Tick(object sender, EventArgs e)
                {
                    if (Usual_historys.Count > 0)
                    {
                        for (int i = 0; i < real_PlayPOJOs.Count; i++)
                        {

                            Usual_historys_save_if = false;
                            string FilePath = Application.StartupPath + "\\Usual_history\\"
                                    + real_PlayPOJOs[i].Device_name + "\\"
                                    + DateTime.Now.ToString("yyyy") + "\\"
                                    + DateTime.Now.ToString("MM") + "\\"
                                    + DateTime.Now.ToString("dd") + "\\";

                            Directory.CreateDirectory(FilePath);

                            string FileName = FilePath + DateTime.Now.ToString("HH-mm-ss");

                            FileStream file = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                            using (var stream = new StreamWriter(file))
                            {
                                string str = JsonConvert.SerializeObject(Usual_historys[i]);
                                stream.Write(str);
                                stream.Flush();
                                stream.Close();
                            }

                            Usual_historys[i] = new List<int>();
                            UsualhistoryMessage usualhistoryMessage = new UsualhistoryMessage{
                                Pid = UsualHistoryMessages.Count.ToString(),
                                DeviceIp = real_PlayPOJOs[i].IP,
                                DeviceName = real_PlayPOJOs[i].Device_name,
                                SaveTime = DateTime.Now.ToString(),
                                UsualHistoryPath = FileName,
                            };
                            UsualHistoryMessages.Add(usualhistoryMessage);

                            SaveUsualHistoryMessagesToFile();

                        }
                    }


                }*/


        /*  public void SaveUsualHistoryMessagesToFile2()
          {
              FileStream file = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "UsualHistoryMessages", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
              using (var stream = new StreamWriter(file))
              {
                  string str = JsonConvert.SerializeObject(UsualHistoryMessages);
                  stream.Write(str);
                  stream.Flush();
                  stream.Close();
              }
          }*/


        private void SaveHistoryMessagesToFile2(int device_num, string sVideoFileName, bool isManul)
        {

            HistoryMessage2 historyMessage = new HistoryMessage2
            {
                save_time = DateTime.Now/*.ToString("yyyy-MM-dd-HH-mm-ss")*/,
                concentration = real_PlayPOJOs[device_num].messageList[0],
                concentrationMax = real_PlayPOJOs[device_num].messageList[10]
            };



            string filePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\History\\"
                             + DateTime.Now.ToString("yyyy") + "\\"
                             + DateTime.Now.ToString("MM") + "\\"
                             + DateTime.Now.ToString("dd") + "\\";
            Directory.CreateDirectory(filePath);
            string sBmpPicFileName;
            sBmpPicFileName = filePath + "device_" + device_num.ToString() + ".xls";


            if (device_num == 0)
            {
                historyMessages0.Add(historyMessage);


                Tool.saveExcel2(MainWindow.historyMessages0, sBmpPicFileName, false);

            }
            else if (device_num == 1)
            {
                historyMessages1.Add(historyMessage);

                Tool.saveExcel2(MainWindow.historyMessages1, sBmpPicFileName, false);

            }
            else if (device_num == 2)
            {
                historyMessages2.Add(historyMessage);

                Tool.saveExcel2(MainWindow.historyMessages2, sBmpPicFileName, false);

            }
            else if (device_num == 3)
            {
                historyMessages3.Add(historyMessage);

                Tool.saveExcel2(MainWindow.historyMessages3, sBmpPicFileName, false);

            }
            else if (device_num == 4)
            {
                historyMessages4.Add(historyMessage);
                Tool.saveExcel2(MainWindow.historyMessages4, sBmpPicFileName, false);
            }

        }

        private void SaveHistoryMessagesToFile(int device_num, string sVideoFileName, bool isManul)
        {

            History_Message historyMessage = new History_Message
            {
                pid = maxPid,
                device_IP = real_PlayPOJOs[device_num].IP,
                device_name = real_PlayPOJOs[device_num].Device_name,
                save_time = DateTime.Now/*.ToString("yyyy-MM-dd-HH-mm-ss")*/,
                concentration = real_PlayPOJOs[device_num].messageList[0],
                Horiz = real_PlayPOJOs[device_num].messageList[3],
                Vert = real_PlayPOJOs[device_num].messageList[4] + "°",
                Preset_num = presetInt,
                Preset_notes = presetNotes,
                video_path = sVideoFileName,
                isManul = isManul,
                concentrationMax = real_PlayPOJOs[device_num].messageList[10]
            };

            XmlDocument doc = new XmlDocument();

            try
            {
                //doc.LoadXml("<bookstore></bookstore>");//用这句话,会把以前的数据全部覆盖掉,只有你增加的数据
                doc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "HistoryMessages.xml");
                XmlNode root = doc.SelectSingleNode("historymessages");


                XmlElement xelMessage = doc.CreateElement("message");
                string nowDateTimeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                XmlAttribute xelSaveTime = doc.CreateAttribute("save_time");
                xelSaveTime.InnerText = nowDateTimeStr;
                xelMessage.SetAttributeNode(xelSaveTime);

                XmlElement xelPid = doc.CreateElement("pid");
                maxPid++;
                xelPid.InnerText = maxPid.ToString();
                xelMessage.AppendChild(xelPid);

                XmlElement xelDeviceIP = doc.CreateElement("device_IP");
                xelDeviceIP.InnerText = historyMessage.device_IP;
                xelMessage.AppendChild(xelDeviceIP);

                XmlElement xelDeviceName = doc.CreateElement("device_name");
                xelDeviceName.InnerText = historyMessage.device_name;
                xelMessage.AppendChild(xelDeviceName);

                XmlElement xelSaveTime2 = doc.CreateElement("save_time");
                xelSaveTime2.InnerText = nowDateTimeStr;
                xelMessage.AppendChild(xelSaveTime2);


                XmlElement xelHorizontal = doc.CreateElement("horizontal");
                xelHorizontal.InnerText = historyMessage.Horiz;
                xelMessage.AppendChild(xelHorizontal);

                XmlElement xelVertical = doc.CreateElement("vertical");
                xelVertical.InnerText = historyMessage.Vert;
                xelMessage.AppendChild(xelVertical);

                XmlElement xelConcentration = doc.CreateElement("concentration");
                xelConcentration.InnerText = real_PlayPOJOs[device_num].messageList[0];
                xelMessage.AppendChild(xelConcentration);

                XmlElement xelPresetNum = doc.CreateElement("preset_num");
                xelPresetNum.InnerText = historyMessage.Preset_num.ToString();
                xelMessage.AppendChild(xelPresetNum);

                XmlElement xelPresetNotes = doc.CreateElement("preset_notes");
                xelPresetNotes.InnerText = historyMessage.Preset_notes;
                xelMessage.AppendChild(xelPresetNotes);

                XmlElement xelVideoPath = doc.CreateElement("video_path");
                xelVideoPath.InnerText = sVideoFileName;
                xelMessage.AppendChild(xelVideoPath);

                XmlElement xelIsManul = doc.CreateElement("is_manul");
                xelIsManul.InnerText = isManul.ToString();
                xelMessage.AppendChild(xelIsManul);
                historyMessage.isManul = isManul;

                if (isManul)
                {
                    historyMessage.isManul = true;
                    if (language.Equals(Language.Chinese))
                    {
                        historyMessage.isManulStr = "手动";
                    }
                    else { historyMessage.isManulStr = "manul"; }

                }
                else
                {
                    historyMessage.isManul = false;
                    if (language.Equals(Language.Chinese))
                    {
                        historyMessage.isManulStr = "巡航";
                    }
                    else { historyMessage.isManulStr = "cruise"; }

                }


                root.AppendChild(xelMessage);

                doc.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "HistoryMessages.xml");
                //new TipsWindow("报警信息插入成功", 3, TipsEnum.OK).Show();
                historyMessages.Add(historyMessage);
                //AlarmHistoryDataGrid.ItemsSource = historyMessages;
                AlarmHistoryDataGrid.Items.Refresh();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }



        }



        /// <summary>
        /// 更新浓度数据
        /// </summary>
        /// <param name="device_num"></param>
        private void HighHistory_updata(int device_num)
        {
            switch (device_num)
            {
                // "浓度", "温度", "设备编号", "水平角度", "垂直角度", "速度"
                /*                case 0:
                                    if (Convert.ToInt32(real_PlayPOJOs[0].messageList[0]) > Convert.ToInt32(Camera0_historynd_lable.Text))
                                    {
                                        Camera0_historynd_lable.Text = real_PlayPOJOs[0].messageList[0];
                                    }
                                    break;

                                case 1:
                                    if (Convert.ToInt32(real_PlayPOJOs[1].messageList[0]) > Convert.ToInt32(Camera1_historynd_lable.Text))
                                    {
                                        Camera1_historynd_lable.Text = real_PlayPOJOs[1].messageList[0];
                                    }
                                    break;

                                case 2:
                                    if (Convert.ToInt32(real_PlayPOJOs[2].messageList[0]) > Convert.ToInt32(Camera2_historynd_lable.Text))
                                    {
                                        Camera2_historynd_lable.Text = real_PlayPOJOs[2].messageList[0];
                                    }
                                    break;

                                case 3:
                                    if (Convert.ToInt32(real_PlayPOJOs[3].messageList[0]) > Convert.ToInt32(Camera3_historynd_lable.Text))
                                    {
                                        Camera3_historynd_lable.Text = real_PlayPOJOs[3].messageList[0];
                                    }
                                    break;*/
            }
        }

        /// <summary>
        /// 获取OSD
        /// </summary>
        /// <param name="device_num"></param>
        private void getOSD()
        {
            for (int i = 0; i < 4; i++)
            {
                //获取球机位置信息结构体大小
                int size = Marshal.SizeOf(typeof(NET_DVR_SHOWSTRING_V30));
                //设置指针空间大小
                IntPtr ptrOSD = Marshal.AllocHGlobal(size);
                uint result = 0;
                switch (i)
                {
                    case 0:
                        NET_DVR_GetDVRConfig(real_PlayPOJOs[Chosen_device_num].I_lUserID, 1030, 33, ptrOSD, (uint)size, ref result);
                        break;

                    case 1:
                        NET_DVR_GetDVRConfig(real_PlayPOJOs[Chosen_device_num].I_lUserID, 1030, 34, ptrOSD, (uint)size, ref result);
                        break;

                    case 2:
                        NET_DVR_GetDVRConfig(real_PlayPOJOs[Chosen_device_num].I_lUserID, 1030, 35, ptrOSD, (uint)size, ref result);
                        break;

                    case 3:
                        NET_DVR_GetDVRConfig(real_PlayPOJOs[Chosen_device_num].I_lUserID, 1030, 36, ptrOSD, (uint)size, ref result);
                        break;

                    default:
                        break;
                }
                NET_DVR_SHOWSTRING_V30 dVR_OSDPOS = (NET_DVR_SHOWSTRING_V30)Marshal.PtrToStructure(ptrOSD, typeof(NET_DVR_SHOWSTRING_V30));
                int PanPos = dVR_OSDPOS.struStringInfo.Length;

                Console.WriteLine("===================================================================================");
                for (int i1 = 0; i1 < PanPos; i1++)
                {
                    /*
                                        Console.WriteLine("预览的图象上是否显示字符,0-不显示,1-显示 区域大小704*576,单个字符的大小为32*3");
                                        Console.WriteLine("wShowString:" + dVR_OSDPOS.struStringInfo[i1].wShowString);

                                        Console.WriteLine(" 该行字符的长度，不能大于44个字符");
                                        Console.WriteLine("wStringSize:" + dVR_OSDPOS.struStringInfo[i1].wStringSize);

                                        Console.WriteLine("字符显示位置的x坐标");
                                        Console.WriteLine("wShowStringTopLeftX:" + dVR_OSDPOS.struStringInfo[i1].wShowStringTopLeftX);

                                        Console.WriteLine("字符名称显示位置的y坐标");
                                        Console.WriteLine("wShowStringTopLeftY:" + dVR_OSDPOS.struStringInfo[i1].wShowStringTopLeftY);

                                        Console.WriteLine("要显示的字符内容");
                    */
                    Console.WriteLine("sString:" + i1 + dVR_OSDPOS.struStringInfo[i1].sString);
                }
                //  Console.WriteLine("===================================================================================");
            }
        }


        private void SuccessAction(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void FailAction(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("失败", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        /*  private void Button_Click(object sender, RoutedEventArgs e)
          {
              Growl.SuccessGlobal("显示一条通知！");
          }*/
        /*    private void Button_Click(object sender, RoutedEventArgs e)
            {
                new ImageBrowser(new Uri("pack://application:,,,/Resources/ImageBlock.png")).Show();
            }*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Screenshot().Start();
        }
        private void CloudPlatform(object sender, RoutedEventArgs e)
        {
            if (MainWindow.sbmc == "")
            {
                new TipsWindow("请先登录", 3, TipsEnum.FAIL).Show();
                return;
            }

            new CloudPlatform().Show();
        }
        private void DeviceSetup(object sender, RoutedEventArgs e)
        {
            if (MainWindow.sbmc == "")
            {
                new TipsWindow("请先登录", 3, TipsEnum.FAIL).Show();
                return;
            }
            deviceSetup = new DeviceSetup();
            deviceSetup.Show();
        }
        private void HistoryData(object sender, RoutedEventArgs e)
        {
            new HistoryData().Show();
        }
        private void AlarmHistory(object sender, RoutedEventArgs e)
        {
            new AlarmHistory().Show();
        }

        private void About(object sender, RoutedEventArgs e)
        {
            new About().Show();
        }
        private void Login(Object sender, RoutedEventArgs e)
        {
            new Login().Show();
        }
        private void backup(object sender, RoutedEventArgs e)
        {
            new Backup().Show();
        }
        private void Logout(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirm = new ConfirmWindow("确认退出？", TipsEnum.BOTH);
            confirm.Closed += Confirm_Closed;
            confirm.Show();
            /*    if (MessageBox.Show("退出系统后将无法实施接收云台数据，确认退出？", "退出系统", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Debug.WriteLine("退出成功");
                    Growl.SuccessGlobal("退出成功");
                }
                else
                {
                    Debug.WriteLine("退出失败");
                }*/

            //new Logout().Show();
            /*   MessageBoxResult result = System.Windows.MessageBox.Show("确认退出？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information);

               if (result == MessageBoxResult.OK)
               {
                   Close();
               }*/

            /*       Growl.AskGlobal(null, isConfirmed =>
                   {
                       if (isConfirmed)
                       {
                           Close();
                       }
                       return true;
                   });*/

        }

        private void Confirm_Closed(object sender, EventArgs e)
        {
            ConfirmWindow confirmWindow = sender as ConfirmWindow;
            if (confirmWindow != null)
            {
                if (confirmWindow.Result == 1)
                {
                    Console.WriteLine("ok");
                    //MainWindow.In_Main_Form.Close();
                    //this.Close();
                    Thread.CurrentThread.IsBackground = true;
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("cancel");
                }
            }
        }

        private void openWindowWiper(object sender, EventArgs e)
        {
            if (MainWindow.sbmc == "")
            {
                new TipsWindow("请先登录", 3, TipsEnum.FAIL).Show();
                return;
            }
            CHCNetSDK.NET_DVR_PTZControl(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, CHCNetSDK.WIPER_PWRON, 0);
        }
        private void closeWindowWiper(object sender, EventArgs e)
        {
            if (MainWindow.sbmc == "")
            {
                new TipsWindow("请先登录", 3, TipsEnum.FAIL).Show();
                return;
            }
            CHCNetSDK.NET_DVR_PTZControl(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, CHCNetSDK.WIPER_PWRON, 1);
        }


        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="s">发送内容</param>
        private void Send(string s)
        {
            /*            if (!CHCNetSDK.NET_DVR_SerialSend(Main_Form.real_PlayPOJOs[Main_Form.Chosen_device_num].ISerialHandle, 1, s, (uint)s.Length))
                        {
                            MessageBox.Show("发送失败" + CHCNetSDK.NET_DVR_GetLastError());
                        }
            */

            MainWindow.In_Main_Form.Tcp_Send(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].deviceNum, Encoding.Default.GetBytes(s));
            Thread.Sleep(3);
            MainWindow.In_Main_Form.Tcp_Send(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].deviceNum, Encoding.Default.GetBytes(s));
            Thread.Sleep(4);
            MainWindow.In_Main_Form.Tcp_Send(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].deviceNum, Encoding.Default.GetBytes(s));
            Thread.Sleep(3);
            MainWindow.In_Main_Form.Tcp_Send(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].deviceNum, Encoding.Default.GetBytes(s));
            Thread.Sleep(4);
            MainWindow.In_Main_Form.Tcp_Send(MainWindow.real_PlayPOJOs[MainWindow.Chosen_device_num].deviceNum, Encoding.Default.GetBytes(s));
        }

        private void SetupIndicatingLaser(object sender, EventArgs e)
        {
            if (MainWindow.sbmc == "")
            {
                new TipsWindow("请先登录", 3, TipsEnum.FAIL).Show();
                return;
            }
            IndicatingLaserBool = !IndicatingLaserBool;
            if (IndicatingLaserBool)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/on.png", UriKind.Relative);
                bitmapImage.EndInit();
                indicatingLaserImg.Source = bitmapImage;
                openIndicatingLaser();
            }
            else
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("/WpfApp1;component/Resources/off.png", UriKind.Relative);
                bitmapImage.EndInit();
                indicatingLaserImg.Source = bitmapImage;
                closeIndicatingLaser();
            }
        }
        private void openIndicatingLaser()
        {
            Send("@lgk@");
        }



        private void closeIndicatingLaser()
        {
            Send("@lgg@");
        }
        private void Reset(object sender, EventArgs e)
        {

            if (MainWindow.sbmc == "")
            {
                new TipsWindow("请先登录", 3, TipsEnum.FAIL).Show();
                return;
            }
            Console.WriteLine("65");
            Console.WriteLine(NET_DVR_PTZPreset(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, (uint)CHCNetSDK.GOTO_PRESET, 65));
            Thread.Sleep(500);
            Console.WriteLine("92");
            Console.WriteLine(NET_DVR_PTZPreset(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, (uint)CHCNetSDK.GOTO_PRESET, 92));
        }


        /// <summary>
        /// 保存文件流至文件
        /// </summary>
        /*        public void SaveInstanceToFile()
                {
                    FileStream file = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "HistoryMessages", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    using (var stream = new StreamWriter(file))
                    {
                        string str = JsonConvert.SerializeObject(HistoryMessages);
                        stream.Write(str);
                        stream.Flush();
                        stream.Close();
                    }
                }*/


        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void screenshot(object sender, EventArgs e)
        {
            if (MainWindow.sbmc == "")
            {
                new TipsWindow("请先登录", 3, TipsEnum.FAIL).Show();
                return;
            }
            SavePicture(true, Chosen_device_num);
        }

        /// <summary>
        /// 保存照片截图
        /// </summary>
        private void SavePicture(bool isShowMB, int device_num)
        {
            string str;
            string ndsj = real_PlayPOJOs[device_num].messageList[0];
            //录像保存路径和文件名
            string sVideoFilePath = Application.StartupPath + "\\Picture\\"
                                    + real_PlayPOJOs[device_num].Device_name + "\\"
                                    + DateTime.Now.ToString("yyyy") + "\\"
                                    + DateTime.Now.ToString("MM") + "\\"
                                    + DateTime.Now.ToString("dd") + "\\";

            Directory.CreateDirectory(sVideoFilePath);

            string sBmpPicFileName;
            sBmpPicFileName = sVideoFilePath + DateTime.Now.ToString("HH-mm-ss") + "-CH4-" + ndsj;

            //BMP抓图 Capture a BMP picture
            if (!CHCNetSDK.NET_DVR_CapturePicture(real_PlayPOJOs[device_num].I_lRealHandle, sBmpPicFileName + ".bmp"))
            {
                if (isShowMB)
                {
                    str = "截图失败";
                    MessageBox.Show(str);
                }
                return;
            }
            else
            {
                if (isShowMB)
                {
                    Tool.GetPicThumbnail(sBmpPicFileName + ".bmp", sBmpPicFileName + ".jpg", 1080, 1920, 65, ndsj, device_num);
                    str = "图片路径:" + sBmpPicFileName;
                    //DialogResult res = System.Windows.Forms.MessageBox.Show(str, "截图成功");
                    MessageBox.Show("截图成功:" + str);



                    /*if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        // 添加水印，压缩图片
                        Tool.GetPicThumbnail(sBmpPicFileName + ".bmp", sBmpPicFileName + ".jpg", 1080, 1920, 65, ndsj, device_num);
                    }*/
                }
            }
            //SaveInstanceToFile();
        }


        private List<UsualhistoryMessage> LoadUsualHistoryMessagesFileToInstance()
        {
            try
            {
                var fs = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "UsualHistoryMessages", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (var stream = new StreamReader(fs))
                {
                    return JsonConvert.DeserializeObject<UsualhistoryMessage[]>(stream.ReadToEnd()).ToList();
                }
            }
            catch (FileNotFoundException)
            {
                return new List<UsualhistoryMessage>();
            }
        }

        /// <summary>
        /// 读取本地json
        /// </summary>
        /// <param name="ins"></param>
        /// <param name="fullPath"></param>
        private List<History_Message> LoadFileToInstance()
        {
            try
            {
                var fs = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "HistoryMessages", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (var stream = new StreamReader(fs))
                {
                    return JsonConvert.DeserializeObject<History_Message[]>(stream.ReadToEnd()).ToList();
                }
            }
            catch (FileNotFoundException)
            {
                return new List<History_Message>();
            }
        }

    }
}

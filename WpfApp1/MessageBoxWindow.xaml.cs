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

namespace WpfApp1
{

    /// <summary>
    /// MessageBoxWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxWindow : System.Windows.Window
    {
        int ndTimesShowLength = 0;
        int MaxND = 0;
        System.Windows.Forms.PictureBox m_pictureBox;
        System.Windows.Forms.ToolTip m_toolTip;
        //ToolTip m_tp;

        //绑定的X轴数据
        private ChartValues<double> ValueList { get; set; }

        private ChartValues<double> ValueList2 { get; set; }

        public static List<CruisePOJO> cruisePOJOs = new List<CruisePOJO>();

        private System.Windows.Forms.PictureBox camera0;

        /// <summary>
        /// 运行时选中的设备编号
        /// </summary>
        public static int Chosen_device_num;

        public static int iSeq;

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

        public static MessageBoxWindow In_Main_Form;

        /// <summary>
        /// 设备独立信息集合
        /// </summary>
        internal static List<Real_PlayPOJO> real_PlayPOJOs;

        /// <summary>
        /// 预置点备注
        /// </summary>
        internal static List<PresetPOJO> presetPOJOList;


        /// <summary>
        /// 选中的设备编号
        /// </summary>
        public static int choose_device_num;

        /// <summary>
        /// 设备名称
        /// </summary>
        public static string sbmc = "";

        public SeriesCollection SeriesCollection2 { get; set; }
        public SeriesCollection SeriesCollection { get; set; }


        public string Name1 { get; set; } = "bbb";










 
 

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
                    NET_DVR_PTZControl(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, CHCNetSDK.ZOOM_IN, 0);
        }
        private void Zoom_out_MouseDown(object sender, RoutedEventArgs e)
        {
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




        public void reloadCruiseData()
        {
            cruisePOJOs.Clear();
            for (int i = 0; i < presetPOJOList[Chosen_device_num].Cruises[iSeq].Count; i++)
            {
                if (presetPOJOList[Chosen_device_num].Cruises[iSeq][i].preset_num >= 0)
                {
                    int time = presetPOJOList[Chosen_device_num].Cruises[iSeq][i].time;
                    //int speed = presetPOJOList[Chosen_device_num].Cruises[iSeq][i].speed;
                    cruisePOJOs.Add(new CruisePOJO() { speedStr = 0.ToString() + " °/s", timeStr = time.ToString() + "s", name = "1122334455" });
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
            //System.Diagnostics.Debug.WriteLine("按钮点击（鼠标按下）");
            // 注意：这里不是严格意义上的“按钮点击”，而是鼠标按下的动作  
            real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
            //StopYTAutoMove(false);
            NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, TILT_UP, 0, 7);
        }
        // 当鼠标按下按钮时触发（可以看作是开始点击）  
        private void Button_MouseDown_PAN_LEFT(object sender, MouseButtonEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("按钮点击（鼠标按下）");
            // 注意：这里不是严格意义上的“按钮点击”，而是鼠标按下的动作  
            real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
            //StopYTAutoMove(false);
            NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, PAN_LEFT, 0, 7);
        }
        // 当鼠标按下按钮时触发（可以看作是开始点击）  
        private void Button_MouseDown_PAN_RIGHT(object sender, MouseButtonEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("按钮点击（鼠标按下）");
            // 注意：这里不是严格意义上的“按钮点击”，而是鼠标按下的动作  
            real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
            //StopYTAutoMove(false);
            NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, PAN_RIGHT, 0, 7);
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
            //System.Diagnostics.Debug.WriteLine("按钮点击（鼠标按下）");
            // 注意：这里不是严格意义上的“按钮点击”，而是鼠标按下的动作  
            real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
            //StopYTAutoMove(false);
            NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, TILT_DOWN, 0, 7);
        }
        // 当鼠标松开按钮时触发  
        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // System.Diagnostics.Debug.WriteLine("按钮松开");
            // 注意：如果你的按钮在MouseDown时触发了某些操作，并希望在MouseUp时撤销或完成这些操作，请在这里处理  
            real_PlayPOJOs[Chosen_device_num].B_isAuto = false;
            //In_Main_Form.StopYTAutoMove(false);
            NET_DVR_PTZControlWithSpeed(real_PlayPOJOs[Chosen_device_num].I_lRealHandle, TILT_UP, 1, 7);
        }
        //绘制事件
        void picturebox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(@"C:\Users\Administrator\Pictures\left.png");
            System.Drawing.Point ulPoint = new System.Drawing.Point(0, 0);
            e.Graphics.DrawImage(bmp, ulPoint);
            //m_toolTip.Show("222", this);
        }
        /// <summary>
        /// 登录信息
        /// </summary>
        /// <param name="info">登录信息结构体</param>
        //internal void SetLoginInfo(TD_INFO info, int device_num)
        internal void SetLoginInfo(TD_INFO info, int device_num, bool HD_if)
        {
            choose_device_num = device_num;
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
                real_PlayPOJOs[device_num].Device_name = sbmc;
                if (real_PlayPOJOs[device_num].I_lUserID < 0)
                {
                    real_PlayPOJOs.RemoveAt(device_num);
                    Growl.SuccessGlobal("登录失败,err:" + NET_DVR_GetLastError());
                    return;
                }
                else
                {

                    //开始预览
                    OpenPreview(Int32.Parse(info.TDid), device_num, HD_if);
                    presetPOJOList.Add(Tool.LoadFileToInstance(info.Ip));
                    real_PlayPOJOs[device_num].I_gbyz = 3000;

                    Growl.Success("登录成功");


                }
                //开始预览




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
        public MessageBoxWindow()
        {
            InitializeComponent();
          
            NET_DVR_Init();
            
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            real_PlayPOJOs = new List<Real_PlayPOJO>();
            presetPOJOList = new List<PresetPOJO>();

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
                StrokeThickness = 1,
                Fill = Brushes.LightSkyBlue,

            };
            //lineseries.DataLabels = true;
            lineseries.Values = ValueList;
            SeriesCollection.Add(lineseries);


            SeriesCollection2 = new SeriesCollection
        {
 /*           new ColumnSeries
            {
                Title = "光强",
                Values = new ChartValues<double> { 0, 0, 700, 0, 0 },
                Fill =Brushes.LightSkyBlue,
                Foreground=Brushes.White
            }*/
      /*      ,
            new ColumnSeries
            {
                Title = "Series 2",
                Values = new ChartValues<double> { 900, 600, 400, 900 }
            }*/
        };

            ValueList2 = new ChartValues<double>();

            ColumnSeries columnSeries2 = new ColumnSeries
            {
                Title = "光强",
                Fill = Brushes.LightSkyBlue,
                Foreground = Brushes.White
            };
            //lineseries.DataLabels = true;
            columnSeries2.Values = ValueList2;
            SeriesCollection2.Add(columnSeries2);


            /*     cruisePOJOs.Add(new CruisePOJO() { speed = 1, time = 42, name = "1122334455" });
                 cruisePOJOs.Add(new CruisePOJO() { speed = 1, time = 42, name = "1122334455" });
                 cruisePOJOs.Add(new CruisePOJO() { speed = 1, time = 42, name = "1122334455" });
                 cruisePOJOs.Add(new CruisePOJO() { speed = 1, time = 42, name = "1122334455" });
                 cruisePOJOs.Add(new CruisePOJO() { speed = 1, time = 42, name = "1122334455" });
                 cruisePOJOs.Add(new CruisePOJO() { speed = 1, time = 42, name = "1122334455" });
                 cruisePOJOs.Add(new CruisePOJO() { speed = 1, time = 42, name = "1122334455" });
                 cruisePOJOs.Add(new CruisePOJO() { speed = 1, time = 42, name = "1122334455" });
                 cruisePOJOs.Add(new CruisePOJO() { speed = 1, time = 42, name = "1122334455" });
                 cruisePOJOs.Add(new CruisePOJO() { speed = 1, time = 42, name = "1122334455" });
                 cruisePOJOs.Add(new CruisePOJO() { speed = 1, time = 42, name = "1122334455" });
                 cruisePOJOs.Add(new CruisePOJO() { speed = 1, time = 42, name = "1122334455" });
                 Presets.ItemsSource = cruisePOJOs;       */

            this.DataContext = this;

            m_pictureBox = new System.Windows.Forms.PictureBox();
            /*      m_tp = new ToolTip();
                  m_toolTip = new System.Windows.Forms.ToolTip();
                  m_toolTip.SetToolTip(m_pictureBox, "1");
                  m_tp.PlacementTarget = pictureBoxHost;*/

            cameras = new List<PictureBox> { m_pictureBox };

            pictureBoxHost.Child = m_pictureBox;
            //m_pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(picturebox_Paint);


            /* #region 云服务

             System.Timers.Timer t = new System.Timers.Timer(5 * 1000);//实例化Timer类，设置间隔时间为10000毫秒；

             t.Elapsed += new System.Timers.ElapsedEventHandler(TCPTool.CreateClient);//到达时间的时候执行事件；

             t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；

             t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；

             #endregion 云服务

             #region 清理内存

             System.Timers.Timer tClear = new System.Timers.Timer(1 * 1000 * 60 * 5);//实例化Timer类，设置间隔时间为10000毫秒；

             tClear.Elapsed += new System.Timers.ElapsedEventHandler(ClearMemory);//到达时间的时候执行事件；

             tClear.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；

             tClear.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；

             #endregion 清理内存

             #region TcpServer

             //分配发送区与接收区大小
              tcpServer = new TcpServer(2 * 1024, 2 * 1024);
             //获取本机IP，并以指定端口开启服务
              tcpServer.Start(tcpServer.GetIP(), "5000");
             
             StartTcpServer
             #endregion TcpServer*/
            //StartTcpServer();
            //TcpClientTest();
       
            Tcp_Start(Tcp_GetIP(), "6001");

            /*        IPAddress ip = IPAddress.Parse("12.52.12.30");
                    serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    serverSocket.Bind(new IPEndPoint(ip, 6001));  //绑定IP地址：端口  
                    serverSocket.Listen(10);    //设定最多10个排队连接请求             
                    //通过Clientsoket发送数据  
                    Thread myThread = new Thread(ListenClientConnect);
                    myThread.Start();  */


        }




        public Socket Tcp_ServerSocket { get; set; }

        /// <summary>
        /// Sockets键值对数组
        /// </summary>


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
        /// 创建服务端Socket
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private Socket Tcp_CreateSocket(string ip, string port)
        {
            /*            try
                        {
                        }
                        catch (System.Net.Sockets.SocketException)
                        {
                            throw;
                        }
            */

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse(ip);
            IPEndPoint endPoint = new IPEndPoint(address, int.Parse(port));
            socket.Bind(endPoint);
            socket.Listen(50);
            return socket;
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
                    Console.WriteLine(ex.ToString());
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
                             Console.WriteLine(Encoding.Default.GetString(buffer));
                            
                            this.Dispatcher.BeginInvoke(new Change(R232Text), Encoding.Default.GetString(buffer), clientSocket);

                            //clientSocket.Send(System.Text.Encoding.Default.GetBytes("AABB 3 60000 1016 01 AABB"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    break;
                }
            }
        }




        #region 数据透传

        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="text"></param>
        private delegate void Change(string text, Socket socket);

        private string[] strArray;
        private string[] exStrArray = { "start", "stop", "beginjz", "wait", "failure", "okay", };
        private string strstart = "@start@";

        private void R232Text(string text, Socket socket)
        {
            // Console.WriteLine("text:");
            Console.WriteLine("text:" + text);
            ndTimesShowLength++;
            int bjz = 0;
            string nmwd = "";
            int nd = 0;
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
                            MessageBox.Show("开始校准，请稍后。" + exStrArray[i], "探头校准");
                            try
                            {
                            }
                            catch (Exception)
                            {
                                throw;
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
                            Tool.KillMessageBox("探头校准");
                            MessageBox.Show("校准成功。", "校准结束");
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
                nd = int.Parse(strArray[0]);
                double doubleND = double.Parse(strArray[0]);
                Console.WriteLine("浓度：{0}", doubleND);
                if (ndTimesShowLength % 20 == 0) {
                    Console.WriteLine("添加了一个浓度值 ");
                    if (ValueList.Count > 20) { 
                    ValueList.RemoveAt(0);
                    }

                    ValueList.Add(doubleND);
                    NDText.Text = nd.ToString();
                   
                    if(MaxND < nd)
                    {
                        MaxND = nd;
                    }
                    MaxNDText.Text = MaxND.ToString();



                    strArray = strArray[1].Split('W');
                    //最大值
                    //  mnd = int.Parse(strArray[0]);

                    //如果勾选最大值
                    /*      if (Mndmode_checkBox.Checked)
                          {
                              nd = int.Parse(strArray[0]);
                          }*/

                    //温度
                    strArray = strArray[1].Split('D');
                    wd = double.Parse(strArray[0]);
                    wd /= 100;
                    Console.WriteLine("温度：{0}", wd);
                    temprature.Status = wd.ToString("0") + "℃";
                    //设备编号
                    // MessagePOJO.deviceNum = int.Parse(strArray[1]);
                    strArray = strArray[1].Split('E');
                    try
                    {
                        /*     if (!Tcp_Sockets.ContainsKey(strArray[0]))
                             {
                                 Tcp_Sockets.Add(strArray[0], socket);
                             }*/
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("存过一次了");
                    }

                    //对device_num赋值
                    device_num = real_PlayPOJOs.FindIndex(item => item.messagePOJO.deviceNum.Equals(strArray[0]));
                    if (device_num < 0)
                    {
                        return;
                    }

                    #region 给浓度赋值

                    real_PlayPOJOs[device_num].messageList[9] = real_PlayPOJOs[device_num].messageList[8];
                    real_PlayPOJOs[device_num].messageList[8] = real_PlayPOJOs[device_num].messageList[7];
                    real_PlayPOJOs[device_num].messageList[7] = real_PlayPOJOs[device_num].messageList[6];
                    real_PlayPOJOs[device_num].messageList[6] = real_PlayPOJOs[device_num].messageList[0];
                    real_PlayPOJOs[device_num].messageList[0] = nd + "";
                    real_PlayPOJOs[device_num].messageList[1] = wd + "";

                    #endregion 给浓度赋值

                    //水平角度
                    strArray = strArray[1].Split('A');
                    spjd = Convert.ToDouble(int.Parse(strArray[0]) / 100 + "." + int.Parse(strArray[0]) % 100);

                    horizontalAngle.Status = spjd.ToString("0.00") + "°";

                    //  horizontalValue_label.Text = spjd + "";
                    real_PlayPOJOs[device_num].messageList[3] = spjd + "";

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
                    verticalAngle.Status = czjd.ToString("0.00") + "°";

                    //光强
                    strArray = strArray[1].Split('C');
                    real_PlayPOJOs[device_num].messageList[8] = strArray[0];
                    lightIntensity.Status = strArray[0];

                    if (ValueList2.Count == 0)
                    {
                        ValueList2.Add(0);
                        ValueList2.Add(0);
                        ValueList2.Add(double.Parse(strArray[0])); ValueList2.Add(0);
                        ValueList2.Add(0);
                        ValueList2.Add(0);
                    }
                    else
                    {
                        ValueList2.Clear();
                        ValueList2.Add(0);
                        ValueList2.Add(0);
                        ValueList2.Add(double.Parse(strArray[0])); ValueList2.Add(0);
                        ValueList2.Add(0);
                        ValueList2.Add(0);

                    }

                    //后门
                    if (device_num == Chosen_device_num)
                    {
                        /*   if (roll_checkBox.Checked)
                           {
                               nd += random.Next(0, 3000);
                               real_PlayPOJOs[device_num].messageList[0] = nd + "";
                           }*/
                    }

                    real_PlayPOJOs[device_num].messageList[4] = Math.Round(czjd, 2) + "";
                    real_PlayPOJOs[device_num].I_nmwd_if = 0;

                    if (real_PlayPOJOs[device_num].messagePOJO.concentration < nd && nd < 100000)
                    {
                        real_PlayPOJOs[device_num].messagePOJO.concentration = nd;
                    }
                    real_PlayPOJOs[device_num].messagePOJO.temperature = (int)wd;
                    real_PlayPOJOs[device_num].messagePOJO.alarmValue = bjz;

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

            //Message_update(device_num);
            //SystemWarning(device_num);

            if (device_num >= 0 && device_num < real_PlayPOJOs.Count)
            {
                if (nd > real_PlayPOJOs[device_num].I_concentration && nd > bjz)
                {
                    real_PlayPOJOs[device_num].I_concentration = nd;
                }
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
        private void Save_usual_historys_timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.ToString("mm").EndsWith("9"))
            {
                Usual_historys_save_if = true;
            }

            if (Usual_historys_save_if)
            {
                for (int i = 0; i < real_PlayPOJOs.Count; i++)
                {
        /*            if (DateTime.Now.ToString("mm").EndsWith("0"))
                    //   if (DateTime.Now.ToString("mm").Equals("00"))
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

                        UsualHistoryMessages.Add(new Usual_history_Message
                        {
                            pid = HistoryMessages.Count,
                            device_IP = real_PlayPOJOs[i].IP,
                            device_name = real_PlayPOJOs[i].Device_name,
                            save_time = DateTime.Now,
                            usual_history_path = FileName,
                        });

                        SaveUsualHistoryMessagesToFile();
                    }*/
                }
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

        #endregion 数据透传





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
              Growl.Success("显示一条通知！");
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
            new CloudPlatform().Show();
        }
        private void DeviceSetup(object sender, RoutedEventArgs e)
        {
            new DeviceSetup().Show();
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
        private void Logout(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("退出系统后将无法实施接收云台数据，确认退出？", "退出系统", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Debug.WriteLine("退出成功");
                Growl.Success("退出成功");
            }
            else
            {
                Debug.WriteLine("退出失败");
            }


        }

    }
}

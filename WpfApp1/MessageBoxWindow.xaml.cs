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

namespace WpfApp1
{

    /// <summary>
    /// MessageBoxWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxWindow : System.Windows.Window
    {
        System.Windows.Forms.PictureBox m_pictureBox;
        System.Windows.Forms.ToolTip m_toolTip;
        //ToolTip m_tp;

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


        public void reloadCruiseData()
        {
            cruisePOJOs.Clear();
            for (int i = 0; i < MessageBoxWindow.presetPOJOList[MessageBoxWindow.Chosen_device_num].Cruises[iSeq].Count; i++)
            {
                if (MessageBoxWindow.presetPOJOList[MessageBoxWindow.Chosen_device_num].Cruises[iSeq][i].preset_num >= 0)
                {
                    int time = MessageBoxWindow.presetPOJOList[MessageBoxWindow.Chosen_device_num].Cruises[iSeq][i].time;
                    int speed = MessageBoxWindow.presetPOJOList[MessageBoxWindow.Chosen_device_num].Cruises[iSeq][i].speed;
                    cruisePOJOs.Add(new CruisePOJO() { speed = speed, time = time, name = "1122334455" }); 
                }
            }
       
            Presets.ItemsSource = cruisePOJOs;
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
            NET_DVR_Init();
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
                    MessageBox.Show("登录失败,err:" + NET_DVR_GetLastError());
                    return;
                }
                else
                {
            
                    //开始预览
                    OpenPreview(Int32.Parse(info.TDid), device_num, HD_if);
                    presetPOJOList.Add(Tool.LoadFileToInstance(info.Ip));

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


            NET_DVR_Init();
          
            In_Main_Form = this;
            real_PlayPOJOs = new List<Real_PlayPOJO>();
            SeriesCollection = new SeriesCollection
            {
             new LineSeries
                {
                    Title = "Line Series",
                    Values = new ChartValues<double> { 0, 200, 1000, 6000, 5899,  6100, 6111,  6000, 5666,  5999, 5888, 6055, 6122,  6001, 5889 },
                    PointGeometry = null, // 使点不可见
                    Stroke = Brushes.Silver,
                    StrokeThickness = 1,
                    Fill =Brushes.LightSkyBlue,
                   
                }
            };

            SeriesCollection2 = new SeriesCollection
        {
            new ColumnSeries
            {
                Title = "光强",
                Values = new ChartValues<double> { 700 },
                Fill =Brushes.LightSkyBlue,
                Foreground=Brushes.White
            }
       /*     ,
            new ColumnSeries
            {
                Title = "Series 2",
                Values = new ChartValues<double> { 1, 6, 4, 9 }
            }*/
        };
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
            if(MessageBox.Show("退出系统后将无法实施接收云台数据，确认退出？", "退出系统", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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

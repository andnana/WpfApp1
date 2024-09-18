using System.Collections.Generic;

namespace WpfApp1
{
    public class Real_PlayPOJO
    {

        public List<Preset> cruisesPresets = new List<Preset>();
        /// <summary>
        /// NET_DVR_SerialStart的返回值
        /// </summary>
        public int ISerialHandle { get; set; }

        /// <summary>
        /// 用户ID号，NET_DVR_Login_V40等登录接口的返回值
        /// </summary>
        public int I_lUserID { get; set; } = -1;

        /// <summary>
        /// 预览句柄，NET_DVR_RealPlay返回值
        /// </summary>
        public int I_lRealHandle { get; set; } = -1;

        /// <summary>
        /// 透传异常的时间
        /// </summary>
        public int I_nmwd_if { get; set; }

        /// <summary>
        /// 预置点备注
        /// </summary>
        public PresetPOJO presetPOJO { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 通道号
        /// </summary>
      //  internal int Chan { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Device_name { get; set; }

        /// <summary>
        /// 录像中
        /// </summary>
        public bool B_bRecord = false;

        /// <summary>
        /// 高报中
        /// </summary>
        public bool B_isGBaoJingZhong = false;

        /// <summary>
        /// 低报中
        /// </summary>
        public bool B_isDBaoJingZhong = false;

        /// <summary>
        /// 高报阈值
        /// </summary>
        public int I_gbyz = 3000;

        /// <summary>
        /// 低报阈值
        /// </summary>
        public int I_dbyz = 1000;

        public int I_concentration = 0;

        /// <summary>
        /// 是否自动巡航
        /// </summary>
        public bool B_isAuto = false;

        /// <summary>
        /// 巡航路径编号
        /// </summary>
        public int I_cruise_path_num = 0;

        /// <summary>
        /// 当前巡航点编号
        /// </summary>
        public int I_cruise_num_now = 0;

        /// <summary>
        /// 下一个巡航点编号
        /// </summary>
        public int I_cruise_num_next = 1;

        /// <summary>
        /// 巡航点编号集合
        /// </summary>
        public List<int> cruise_num_list = new List<int>();

        /// <summary>
        /// 巡航间隔时间
        /// </summary>
        public int I_cruise_num_time = 0;

        /// <summary>
        /// 是否高清
        /// </summary>
        public bool HD_if = true;

        /// <summary>
        /// 是否保存数据
        /// </summary>
        public bool Save_if = false;

        /// <summary>
        /// 浓度集合
        /// </summary>
        public List<string> Save_nd = new List<string>();

        /// <summary>
        /// 该设备接收到的当前数据
        /// </summary>
        public MessagePOJO messagePOJO;

        /// <summary>
        /// 该设备接收到的最近100个数据
        /// </summary>
        public List<MessagePOJO> messagePOJOList;

        /// <summary>
        /// 设备编号
        /// Sockets键值对数组的键
        /// </summary>
        public string deviceNum = "";

        /// <summary>
        /// 接收到的数据集合
        /// { "浓度", "温度", "设备编号", "水平角度", "垂直角度", "速度", "备用浓度", "备用浓度", "光强", "最大浓度" }
        /// </summary>
        public string[] messageList = new string[] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };

        /// <summary>
        /// 激光探头工作状态
        /// </summary>
        public bool work_if = false;
    }
}
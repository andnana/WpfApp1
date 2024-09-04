using System.Collections.Generic;

namespace WpfApp1
{
    internal class Real_PlayPOJO
    {
        /// <summary>
        /// NET_DVR_SerialStart的返回值
        /// </summary>
        internal int ISerialHandle { get; set; }

        /// <summary>
        /// 用户ID号，NET_DVR_Login_V40等登录接口的返回值
        /// </summary>
        internal int I_lUserID { get; set; } = -1;

        /// <summary>
        /// 预览句柄，NET_DVR_RealPlay返回值
        /// </summary>
        internal int I_lRealHandle { get; set; } = -1;

        /// <summary>
        /// 透传异常的时间
        /// </summary>
        internal int I_nmwd_if { get; set; }

        /// <summary>
        /// 预置点备注
        /// </summary>
        internal PresetPOJO presetPOJO { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        internal string IP { get; set; }

        /// <summary>
        /// 通道号
        /// </summary>
      //  internal int Chan { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        internal string Device_name { get; set; }

        /// <summary>
        /// 录像中
        /// </summary>
        internal bool B_bRecord = false;

        /// <summary>
        /// 高报中
        /// </summary>
        internal bool B_isGBaoJingZhong = false;

        /// <summary>
        /// 低报中
        /// </summary>
        internal bool B_isDBaoJingZhong = false;

        /// <summary>
        /// 高报阈值
        /// </summary>
        internal int I_gbyz = 3000;

        /// <summary>
        /// 低报阈值
        /// </summary>
        internal int I_dbyz = 1000;

        internal int I_concentration = 0;

        /// <summary>
        /// 是否自动巡航
        /// </summary>
        internal bool B_isAuto = false;

        /// <summary>
        /// 巡航路径编号
        /// </summary>
        internal int I_cruise_path_num = 0;

        /// <summary>
        /// 当前巡航点编号
        /// </summary>
        internal int I_cruise_num_now = 0;

        /// <summary>
        /// 下一个巡航点编号
        /// </summary>
        internal int I_cruise_num_next = 1;

        /// <summary>
        /// 巡航点编号集合
        /// </summary>
        internal List<int> cruise_num_list = new List<int>();

        /// <summary>
        /// 巡航间隔时间
        /// </summary>
        internal int I_cruise_num_time = 0;

        /// <summary>
        /// 是否高清
        /// </summary>
        internal bool HD_if = true;

        /// <summary>
        /// 是否保存数据
        /// </summary>
        internal bool Save_if = false;

        /// <summary>
        /// 浓度集合
        /// </summary>
        internal List<string> Save_nd = new List<string>();

        /// <summary>
        /// 该设备接收到的当前数据
        /// </summary>
        internal MessagePOJO messagePOJO;

        /// <summary>
        /// 该设备接收到的最近100个数据
        /// </summary>
        internal List<MessagePOJO> messagePOJOList;

        /// <summary>
        /// 设备编号
        /// Sockets键值对数组的键
        /// </summary>
        internal string deviceNum = "";

        /// <summary>
        /// 接收到的数据集合
        /// { "浓度", "温度", "设备编号", "水平角度", "垂直角度", "速度", "备用浓度", "备用浓度", "光强" }
        /// </summary>
        internal string[] messageList = new string[] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };

        /// <summary>
        /// 激光探头工作状态
        /// </summary>
        internal bool work_if = false;
    }
}
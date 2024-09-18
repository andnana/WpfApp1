using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class MessagePOJO
    {
        /// <summary>
        /// 前导和结束数据*
        /// <summary>
        public readonly String F_A_F = "AABB";

        /// <summary>
        /// 设备型号*
        /// <summary>
        public readonly int DEVICE_TYPE = 3;

        /// <summary>
        /// 设备编号
        /// <summary>
        public string deviceNum { get; set; } = "0";

        /// <summary>
        /// 控制数据*
        /// <summary>
        public int control_Data = 0;

        /// <summary>
        /// 状态数据*
        /// <summary>
        public int STATUE_DATA = 0;

        /// <summary>
        /// 数据数量*
        /// <summary>
        public readonly int DATA_NUMBER = 8;

        /// <summary>
        /// 浓度
        /// <summary>
        public int concentration { get; set; } = 0;

        /// <summary>
        /// 温度
        /// <summary>
        public int temperature { get; set; } = 0;

        /// <summary>
        /// 气压*
        /// <summary>
        public readonly int PRESSURE = 0;

        /// <summary>
        /// 上传时间间隔*
        /// <summary>
        public readonly int TIMER_INTERVAL = 5;

        /// <summary>
        /// 报警值 
        /// <summary>
        public int alarmValue { get; set; } = 0;

        /// <summary>
        /// 经度
        /// <summary>
        public readonly double longitude = 0;

        /// <summary>
        /// 纬度
        /// <summary>
        public readonly double latitude = 0;

        /// <summary>
        /// 图片路径
        /// <summary>
        public string imgpath { get; set; } = "0.jpg";

        public string order = "";

        /// <summary>
        ///拼接
        /// <summary>
        public string plus()
        {
            order = F_A_F.Trim() + " " + //前导数据
                    DEVICE_TYPE + " " + //设备型号
                    deviceNum + " " + //设备编号
                    control_Data + " " + //控制数据
                    STATUE_DATA + " " +//状态数据
                    DATA_NUMBER + " " + //数据数量
                    concentration + " " +//浓度
                    temperature + " " +//温度
                    PRESSURE + " " + //气压
                    TIMER_INTERVAL + " " +//上传时间间隔
                    alarmValue + " " + //报警值
                    longitude + " " + //经度
                    latitude + " " +//纬度
                    imgpath + " " +//图片路径
                    F_A_F.Trim(); //结束数据



            concentration = 0;
            alarmValue = 0;
            temperature = 0;
            imgpath = "0.jpg";
            //    Console.WriteLine(order);
            return order;
        }

    }
}

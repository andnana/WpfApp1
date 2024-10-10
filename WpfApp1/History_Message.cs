using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static WpfApp1.Tool;

namespace WpfApp1
{
    public class History_Message
    {
        /*        /// <summary>
                /// 图片名称
                /// </summary>
                [ExcelColumn("图片地址")]
                public string pic_name { get; set; }
        */

        /// <summary>
        /// 图片名称
        /// </summary>
        [ExcelColumn("序号")]
        public int pid { get; set; }

        /// <summary>
        /// 设备IP
        /// </summary>
        [ExcelColumn("设备IP")]
        public string device_IP { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [ExcelColumn("设备名称")]
        public string device_name { get; set; }

        /// <summary>
        /// 保存时间
        /// </summary>
        [ExcelColumn("保存时间")]
        public DateTime save_time { get; set; }

        /// <summary>
        /// 浓度
        /// </summary>
        [ExcelColumn("浓度")]
        public string concentration { get; set; }

        /// <summary>
        /// 最大浓度
        /// </summary>
        [ExcelColumn("最大浓度")]
        public string concentrationMax { get; set; }

        /// <summary>
        /// 水平角度
        /// </summary>
        [ExcelColumn("水平角度")]
        public string Horiz { get; set; }

        /// <summary>
        /// 垂直角度
        /// </summary>
        [ExcelColumn("垂直角度")]
        public string Vert { get; set; }

        /// <summary>
        /// 预置点编号
        /// </summary>
        [ExcelColumn("预置点编号")]
        public int Preset_num { get; set; }

        /// <summary>
        /// 预置点备注
        /// </summary>
        [ExcelColumn("预置点备注")]
        public string Preset_notes { get; set; }

        /// <summary>
        /// 视频路径
        /// </summary>
        [ExcelColumn("视频路径")]
        public string video_path { get; set; }

        public bool isManul {  get; set; }

        public string isManulStr { get; set; }

    }
}
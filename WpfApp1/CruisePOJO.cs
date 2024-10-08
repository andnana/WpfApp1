﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class CruisePOJO
    {
        public bool isCurrent { get; set; }

        public string imagePath { get; set; }
        /// <summary>
        /// 预置点
        /// </summary>
        public int preset_num { get; set; }

        /// <summary>
        /// 巡航时间
        /// </summary>
        public int time { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string name { get; set; }

        public int speed { get; set; }

        public string timeStr { get; set; }

        public string speedStr { get; set; }

        public string notes { get; set; }
    }
}

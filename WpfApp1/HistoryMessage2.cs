using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WpfApp1.Tool;

namespace WpfApp1
{
    public class HistoryMessage2
    {
        /// <summary>
        /// 保存时间
        /// </summary>
        [ExcelColumn("保存时间")]
        public string save_time { get; set; }

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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class PresetPOJO
    {
        /// <summary>
        /// 速度
        /// </summary>
        public string Speed { get; set; }

        /// <summary>
        /// 预置点备注集合
        /// </summary>
        public List<string> Presets { get; set; }

        /// <summary>
        /// 巡航路径集合
        /// </summary>
        public List<List<CruisePOJO>> Cruises { get; set; }
    }

  
}
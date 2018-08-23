using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.APIModel.JGS1810
{
    public class InPark
    {
        public string id { set; get; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string plate_num { set; get; }
        /// <summary>
        /// 车牌类型
        /// </summary>
        public string plate_type { set; get; }

        /// <summary>
        /// 入场时间
        /// </summary>
        public string in_time { set; get; }

        /// <summary>
        /// 入场口
        /// </summary>
        public string in_gate { set; get; }

        /// <summary>
        /// 入场图
        /// </summary>
        public string in_img { set; get; }
    }
}

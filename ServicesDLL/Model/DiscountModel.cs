using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model
{
    public class DiscountModel
    {
        /// <summary>
        ///  停车场 ID, 由平台统一分配，在本地系统上配置 
        /// </summary>
        public string parkid { set; get; }

        /// <summary>
        /// 进场时云端订单号 
        /// </summary>
        public string orderid { set; get; }

        /// <summary>
        /// 车牌号,车牌号为空或无牌车的按无牌车编码规则上报。如停车 场 ID+进场年月日时间秒，如：1234520170118153021 
        /// </summary>
        public string plate { set; get; }

        /// <summary>
        /// 优惠券编号，13 位编码规则： 1.10 位的 Unix timestamp 时间戳 2 3 位随机码 例如: 1512625295498 1512625411255 
        /// </summary>
        public string discount_no { set; get; }

        /// <summary>
        ///  优惠类型 1 全免 4 减免时间 5 减免金额 6 折扣 7 余额 8 VIP9 封顶 
        /// </summary>
        public int discount_type { set; get; }

        /// <summary>
        ///   discount_type 为 4、5 和 6 时有效,时间以分钟为 单位，金额以分为单位，折扣 0~1 比如打 9 折值为 0.9 
        /// </summary>
        public float discount_amount { set; get; }

        /// <summary>
        /// 优惠时间 时间戳
        /// </summary>
        public int discount_time { set; get; }
    }
}

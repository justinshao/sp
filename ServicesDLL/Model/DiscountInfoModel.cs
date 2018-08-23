using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model
{
    public  class DiscountInfoModel
    {
        /// <summary>
        /// 优惠券编号，13 位编码规则： 1.10 位的 Unix timestamp 时间戳 2 3 位随机码 例如: 1512625295498 1512625411255 
        /// </summary>
        public string discount_no { set; get; }

        /// <summary>
        /// 优惠类型 1 全免 4 减免时间 5减免金额 6 折 扣 7 余额 8VIP9 封顶 
        /// </summary>
        public int discount_type { set; get; }

        /// <summary>
        /// 优惠数额，时间以分钟为单位，金额以分为单位， 折扣 0~1 比如打 9 折值为 0.9 
        /// </summary>
        public float discount_amount { set; get; }

        /// <summary>
        /// 优惠时间 时间戳
        /// </summary>
        public int discount_time { set; get; }
    }
}

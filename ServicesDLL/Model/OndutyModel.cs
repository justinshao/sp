using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model
{
    public class OndutyModel
    {
        /// <summary>
        /// 停车场 ID, 由平台统一分配，在本地系统上配置 
        /// </summary>
        public string parkid {set;get;}

        /// <summary>
        ///  用户帐号
        /// </summary>
        public string useraccount { set; get; }

        /// <summary>
        /// 用户姓名 
        /// </summary>
        public string username { set; get; }

        /// <summary>
        /// 收费员类型： 0：出入口岗亭 1：中央交费处 2 ：自助 缴费机（预留）3：其他 
        /// </summary>
        public int type { set; get; }

        /// <summary>
        /// 出口名称或中央收费处名称 
        /// </summary>
        public string position { set; get; }

        /// <summary>
        /// 结账开始时间 时间戳 
        /// </summary>
        public int begintime { set; get; }

        /// <summary>
        /// 结账结束时间（结账时间） 时间戳
        /// </summary>
        public int endtime { set; get; }

        /// <summary>
        ///  现金实收总笔数，当班收费员结账时的现金实收总笔数 
        /// </summary>
        public int total_number { set; get; }

        /// <summary>
        /// 现金实收总金额，当班收费员结账时的现金实收总金额 
        /// </summary>
        public float total_amount { set; get; }

        /// <summary>
        ///  纸质优惠券优惠金额
        /// </summary>
        public float discount_amount { set; get; }

        /// <summary>
        /// 纸质优惠券优惠张数 
        /// </summary>
        public int discount_number { set; get; }

        /// <summary>
        /// 上报时间 时间戳
        /// </summary>
        public int time { set; get; }
    }
}

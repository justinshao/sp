using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class CloudPayModel
    {
        public string AppId { set; get; }
        public string AccessToken { set; get; }
        public string Nonce { set; get; }
        public string Sign { set; get; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        public string PlateId { set; get; }

        /// <summary>
        /// 入场产生订单ID
        /// </summary>
        public string SessionId { set; get; }

        /// <summary>
        /// 应缴金额
        /// </summary>
        public string Charge { set; get; }

        /// <summary>
        /// 线上累计已付
        /// </summary>
        public int OnlineCharge { set; get; }

        /// <summary>
        /// 线下累计已付
        /// </summary>
        public int OfflineCharge { set; get; }

        /// <summary>
        /// 累计优惠金额
        /// </summary>
        public int DiscountCharge { set; get; }

        /// <summary>
        /// 累计优惠时长
        /// </summary>
        public int DiscountTime { set; get; }

        /// <summary>
        /// 优惠代码(多个以”,”分隔)
        /// </summary>
        public string DiscountCode { set; get; }

        /// <summary>
        /// 最终应付金额
        /// </summary>
        public int PayCharge { set; get; }

    }
}

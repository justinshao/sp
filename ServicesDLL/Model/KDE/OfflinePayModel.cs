using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class OfflinePayModel
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
        /// 支付类别：0临时卡收费1红山一卡通支付2泊通云平台支付3免费用户4月卡用户
        /// </summary>
        public int PayType { set; get; }

        /// <summary>
        /// 发票编码
        /// </summary>
        public string InvoiceCode { set; get; }
        /// <summary>
        /// 支付编号
        /// </summary>
        public string PayNo { set; get; }
        /// <summary>
        /// 入场时间
        /// </summary>
        public string InTime { set; get; }
        /// <summary>
        /// 出场时间
        /// </summary>
        public string OutTime { set; get; }
        /// <summary>
        /// 停车时长(分钟)
        /// </summary>
        public int ParkingTime { set; get; }
        /// <summary>
        /// 出场实收
        /// </summary>
        public int RealCharge { set; get; }
        /// <summary>
        /// 出场应收
        /// </summary>
        public int PayCharge { set; get; }
        /// <summary>
        /// 免费类型
        /// </summary>
        public string FreeType { set; get; }
        /// <summary>
        /// 免费金额
        /// </summary>
        public int FreeCharge { set; get; }
    }
}

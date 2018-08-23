using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model
{
    public class PaidinfoModel
    {
        /// <summary>
        /// 交易流水号 如果本地缴费的账单号生成规则：年 月日时分秒+停车场 ID+5 位随机数，例如： 201707231449001234524681 
        /// </summary>
        public string tradeno { set; get; }

        /// <summary>
        /// 应收金额 本次的应收金额 
        /// </summary>
        public float total_price { set; get; }

        /// <summary>
        /// 实收金额 本次的实收金额 
        /// </summary>
        public float paid_price { set; get; }

        /// <summary>
        ///  优惠金额 本次的优惠金额 
        /// </summary>
        public float discount_price { set; get; }

        /// <summary>
        /// 缴费方式 1：现金 2：微信 3：支付宝 4： 交通卡 5：ETC 6：银联 7:其它
        /// </summary>
        public int pay_way { set; get; }

        /// <summary>
        /// 缴费渠道：0:出口岗亭 1:平台支付 2：自助缴费 机 3：中央收费站 4：第三方 5:其他 
        /// </summary>
        public int pay_channel { set; get; }

        /// <summary>
        /// 缴费终端，非必填字段，对缴费渠道的说明：
        ///本地系统与停车平台通用数据接口 （1）当缴费渠道是 0 时，此字段是出口名称； （2）当缴费渠道是 1 时，此字段是应用名称 如 H5 扫码支付、App 应用； （3）当缴费渠道是 2 时，此字段是缴费机编号； （4）当缴费渠道是 3 时，此是中央收费站编号； （5）当缴费渠道是 4 时，此是第三方名称； （6）当缴费渠道是 5 时，此字段是具体渠道。 
        /// </summary>
        public string pay_terminal { set; get; }

        /// <summary>
        /// 岗亭或中央收费处收费员的账户名 
        /// </summary>
        public string useraccount { set; get; }

        /// <summary>
        /// 支付时间 时间戳 
        /// </summary>
        public int pay_time { set; get; }

        /// <summary>
        /// 优惠券使用明细，discount_price 大于 0 时必填
        /// </summary>
        public List<DiscountInfoModel> discountInfo { set; get; }
    }
}

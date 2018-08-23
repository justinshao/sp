using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model
{
    public class OrderModel
    {
        /// <summary>
        /// 停车场 ID, 由平台统一分配，在本地系统上配置
        /// </summary>
        public string parkid { set; get; }

        /// <summary>
        /// 进场时云端订单号
        /// </summary>
        public string orderid { set; get; }

        /// <summary>
        /// 车牌号,车牌号为空或无牌车的按无牌车编码规则上报。 
        /// </summary>
        public string plate { set; get; }

        /// <summary>
        /// 交易流水号 
        /// </summary>
        public string tradeno { set; get; }

        /// <summary>
        /// 应收金额 本次应收金额 
        /// </summary>
        public string total_price { set; get; }

        /// <summary>
        /// 实收金额 本次实收金额
        /// </summary>
        public string paid_price { set; get; }

        /// <summary>
        /// 优惠金额 本次优惠金额 
        /// </summary>
        public string discount_price { set; get; }

        /// <summary>
        /// 本次预缴费使用的优惠券编号，当 discount_price 大于 0 时，必传，如果有多个，中间用英文逗号隔开
        /// </summary>
        public string discount_nos { set; get; }

        /// <summary>
        /// 缴费方式 1：现金 2：微信 3：支付宝 4：交通卡 5： ETC 6：银联 7:其它 
        /// </summary>
        public int pay_way { set; get; }

        /// <summary>
        /// 缴费渠道：0:出口岗亭 1:平台支付 2：自助缴费机 3： 中央收费站 4：第三方 5:其他 
        /// </summary>
        public int pay_channel { set; get; }

        /// <summary>
        /// 缴费终端，非必填字段，对缴费渠道的说明： （1）当缴费渠道是 0 时，此字段是出口名称； （2）当缴费渠道是 1 时，此字段是应用名称 如 H5 扫码 支付、App 应用； （3）当缴费渠道是 2 时，此字段是缴费机编号； （4）当缴费渠道是 3 时，此是中央收费站编号； （5）当缴费渠道是 4 时，此是第三方名称； （6）当缴费渠道是 5 时，此字段是具体渠道。 
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
    }
}

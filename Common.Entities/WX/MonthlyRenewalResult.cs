using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Newtonsoft.Json;

namespace Common.Entities.WX
{
    public class MonthlyRenewalResult
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PlateNumber { set; get; }

        /// <summary>
        /// 车场ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PKID { set; get; }

        /// <summary>
        /// 订单信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ParkOrder Pkorder { set; get; }

        /// <summary>
        /// 线上订单编号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OnlineOrderID { set; get; }

        /// <summary>
        /// 结果描述
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public APPResult Result { set; get; }
    }

    public enum APPResult
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 找不到入场记录
        /// </summary>
        NotFindIn = 1,
        /// <summary>
        /// 不需要缴费
        /// </summary>
        NoNeedPay = 2,
        /// <summary>
        /// 不支持手机缴费
        /// </summary>
        NotSupportedPay = 3,
        /// <summary>
        /// 代理网络异常
        /// </summary>
        ProxyException = 4,
        /// <summary>
        /// 其他异常
        /// </summary>
        OtherException = 5,
        /// <summary>
        /// 非临时卡
        /// </summary>
        NoTempCard = 6,
        /// <summary>
        /// 重复缴费
        /// </summary>
        RepeatPay = 10,
        /// <summary>
        /// 金额不对
        /// </summary>
        AmountIsNot = 11,
        /// <summary>
        /// 找不到卡片
        /// </summary>
        NotFindCard = 7,

        /// <summary>
        /// 订单失效
        /// </summary>
        OrderSX = 12,

        /// <summary>
        /// 找不到岗亭信息
        /// </summary>
        NoBox=13,

        /// <summary>
        /// 无车辆在岗亭
        /// </summary>
        NoCarInBox=14,

        /// <summary>
        /// 人工缴费
        /// </summary>
        ManualPay=15,

        /// <summary>
        /// 入口非无牌车扫码
        /// </summary>
        NotIn=16,

        /// <summary>
        /// 无牌车进出
        /// </summary>
        NoLp=17,
    }
}

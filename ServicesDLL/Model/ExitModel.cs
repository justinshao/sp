using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model
{
    public class ExitModel
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
        ///  本地订单号
        /// </summary>
        public string local_orderid { set; get; }

        /// <summary>
        /// 车牌号，车牌号为空或无牌车的按无牌车编码规 则上报。如停车场 ID+进场年月日时分秒，如： 1234520170118153021 
        /// </summary>
        public string plate { set; get; }

        /// <summary>
        /// 卡/票号信息
        /// </summary>
        public string ticketid { set; get; }

        /// <summary>
        ///  出口名称 
        /// </summary>
        public string exitname { set; get; }

        /// <summary>
        /// 出场时间 时间戳
        /// </summary>
        public int exittime { set; get; }

        /// <summary>
        /// 车 辆 入 场 图 片 文 件 路 径 格 式 为 ： 停 车 场 ID/image/ 年 月 / 日 / 图 片 名 称 ， 比 如:12345/image/201707/17/1640415447.jpg，出 场图片文件名命名规则为：hhmmss+四位随机数， 比如：1640415447.jpg， 图片按照文档规定的方 式上传（文档 2.3 节） 
        /// </summary>
        public string exitimage { set; get; }

        /// <summary>
        /// 应收总金额 此车辆从入场到出场产生的应收总 费用
        /// </summary>
        public float total_amount { set; get; }

        /// <summary>
        /// 实收总金额 此车辆从入场到出场产生的实收总 费用 
        /// </summary>
        public float paid_amount { set; get; }

        /// <summary>
        /// 优惠总金额 此车辆从入场到出场的优惠金额总 和 
        /// </summary>
        public float discount_amount { set; get; }

        /// <summary>
        /// 本地收费员帐号
        /// </summary>
        public string useraccount { set; get; }

        /// <summary>
        /// 所有支付信息，如果 total_amount 应收总金额为 0，支付信息无需上报。如果 total_amount 应收 总金额不为 0，则必须上报支付信息。 paidinfo 是一个数组，同一个订单用户进行了几 次支付操作，则 paidinfo 中就有几条支付记录。 具体信息格式如下表。
        /// </summary>
        public List<PaidinfoModel> paidinfo { set; get; }
    }
}

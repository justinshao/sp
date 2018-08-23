using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model
{
    public class CompletebillModel
    {
        /// <summary>
        /// 停车场 ID, 由平台统一分配，在本地系统上配置
        /// </summary>
        public string parkid { set; get; }

        /// <summary>
        /// 本地订单号
        /// </summary>
        public string local_orderid { set; get; }

        /// <summary>
        /// 车牌号，车牌号为空或无牌车的按无牌车编码规则 上报。如停车场 ID+进场年月日时分秒，如： 1234520170118153021 
        /// </summary>
        public string plate { set; get; }

        /// <summary>
        /// 卡/票号信息 只有 devicetype 为 0 和 2 时内容不 为空 
        /// </summary>
        public string ticketid { set; get; }

        /// <summary>
        /// 入场设备类型 0:取卡/票 1:车牌识别 2：车牌识别 和卡/票组合 
        /// </summary>
        public int devicetype { set; get; }

        /// <summary>
        /// 进口名称 
        /// </summary>
        public string entername { set; get; }

        /// <summary>
        /// 出口名称
        /// </summary>
        public string exitname { set; get; }

        /// <summary>
        /// 进场时间 时间戳
        /// </summary>
        public int entertime { set; get; }

        /// <summary>
        /// 出场时间 时间戳
        /// </summary>
        public int exittime { set; get; }

        /// <summary>
        /// 车辆类型，0：临时车 1：月卡车 3：预定车 4： 特殊车辆 5：其它车辆 
        /// </summary>
        public int type { set; get; }

        /// <summary>
        /// 车型，1：小型车 2：大型车，默认为 1
        /// </summary>
        public int cartype { set; get; }

        /// <summary>
        /// 车辆入场图片文件路径 格式为：停车场 ID/image/ 年月/日/图片名称，
        /// </summary>
        public string enterimage { set; get; }

        /// <summary>
        /// 车辆入场图片文件路径 格式为：停车场 ID/image/ 年月/日/图片名称，
        /// </summary>
        public string exitimage { set; get; }

        /// <summary>
        /// 若车辆类型是特殊车辆，此是对此车辆的描述信 息，例如：垃圾车、军警车、医务车 
        /// </summary>
        public string vehicledesc { set; get; }

        /// <summary>
        /// 车辆品牌 0:其它 1:大众 2:别克 3:宝马 4:本田 5:标致 6:丰田 7:福特 8:日产 9:奥迪 10:马自 达 11:雪佛兰 12:雪铁龙 13:现代 14:奇瑞 
        /// </summary>
        public string carbrand { set; get; }

        /// <summary>
        ///  车辆颜色 0：未知、1：蓝色、2：黄色、3： 白色、4：黑色 
        /// </summary>
        public int carcolor { set; get; }

        /// <summary>
        /// 应收总金额 此车辆从入场到出场产生的应收总费 用 
        /// </summary>
        public float total_amount { set; get; }

        /// <summary>
        /// 实收总金额 此车辆从入场到出场产生的实收总费 用 
        /// </summary>
        public float paid_amount { set; get; }

        /// <summary>
        /// 优惠总金额 此车辆从入场到出场的优惠金额总和 
        /// </summary>
        public float discount_amount { set; get; }

        /// <summary>
        /// 本地收费员帐号
        /// </summary>
        public string useraccount { set; get; }

        /// <summary>
        /// 所有支付信息
        /// </summary>
        public PaidinfoModel paidinfo { set; get; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model
{
    public class EnterModel
    {
        /// <summary>
        /// 停车场 ID, 由平台统一分配，在本地系统上配置
        /// </summary>
        public string parkid { set; get; }

        /// <summary>
        /// 本地订单号，确保本地订单号在此车场内的唯一性
        /// </summary>
        public string local_orderid { set; get; }

        /// <summary>
        /// 车牌号,车牌号为空或无牌车的按无牌车编码规则上报。 如 停 车 场 ID+ 进 场 年 月 日 时 分 秒 
        /// </summary>
        public string plate { set; get; }

        /// <summary>
        /// 卡/票号信息 只有 devicetype 为 0 和 2 时内容不为空
        /// </summary>
        public string ticketid { set; get; }

        /// <summary>
        /// 入场设备类型 0:取卡/票 1:车牌识别 2：车牌识别和卡/ 票组合 
        /// </summary>
        public int devicetype { set; get; }

        /// <summary>
        /// 入口名称 
        /// </summary>
        public string entername { set; get; }

        /// <summary>
        /// 进场时间 时间戳 
        /// </summary>
        public int entertime { set; get; }

        /// <summary>
        /// 车辆类型，0：临时车 1：月卡车 3：预定车 4：特殊 车辆 5：其它车辆 
        /// </summary>
        public int type { set; get; }

        /// <summary>
        /// 车型，1：小型车 2：大型车，默认为 1 
        /// </summary>
        public int cartype { set; get; }

        /// <summary>
        ///  车辆入场图片文件路径 格式为：停车场 ID/image/年月/ 日 / 图 片 名 称 
        /// </summary>
        public string enterimage { set; get; }
        
        /// <summary>
        ///  若车辆类型是特殊车辆，此是对此车辆的描述信息，例 如：垃圾车、军警车、医务车 
        /// </summary>
        public string vehicledesc { set; get; }

        /// <summary>
        /// 车辆品牌 0:其它 1:大众 2:别克 3:宝马 4:本田 5:标 致 6:丰田 7:福特 8:日产 9:奥迪 10:马自达 11:雪佛 兰 12:雪铁龙 13:现代 14:奇瑞 
        /// </summary>
        public int carbrand { set; get; }

        /// <summary>
        ///  车辆颜色 0：未知、1：蓝色、2：黄色、3： 白色、4：黑色
        /// </summary>
        public int carcolor { set; get; }

    }
}

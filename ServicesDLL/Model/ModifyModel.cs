using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model
{
    public class ModifyModel
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
        /// 车牌号,车牌号为空或无牌车的按无牌车编码规则上报。如 停车场 ID+进场年月日时间秒
        /// </summary>
        public string plate { set; get; }

        /// <summary>
        /// 0：临时车 1：月卡车 3：预定车 4：特殊车辆 5：其它车 辆 
        /// </summary>
        public int type { set; get; }

        /// <summary>
        /// 车型 1:小型车 2:大型车，默认为 1 
        /// </summary>
        public int  cartype { set; get; }

        /// <summary>
        /// 事件 1：修改 2：删除 默认为 1
        /// </summary>
        public string action { set; get; }

        /// <summary>
        /// 修正时间 时间戳
        /// </summary>
        public int time { set; get; }

        /// <summary>
        ///  若车辆类型是特殊车辆，此是对此车辆的描述信息，例如： 垃圾车、军警车、医务车 
        /// </summary>
        public string vehicledesc { set; get; }

        /// <summary>
        /// 进行此操作的用户帐号
        /// </summary>
        public string useraccount { set; get; }

    }
}

using Common.Core;
using Common.Entities.WX;
using Common.Utilities.Helpers;
using System;
using System.ServiceModel;

namespace PlatformWcfServers
{// 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“FoundationService”。
    [ServiceContract]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public class OISService
    {
        /// <summary>
        /// 日志记录对象
        /// </summary>
        protected Logger logger = LogManager.GetLogger("对外接口");

        /// <summary>
        /// 是否回调服务
        /// </summary>
        /// <returns></returns>
        public bool IsCallbackService()
        {
            return false;
        }

        /// <summary>
        /// 服务名称
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return "对外接口服务";
        }

        #region
        /// <summary>
        /// 车位预定
        /// </summary>
        /// <param name="AccountID">账户ID</param>
        /// <param name="BitNo">车位号(保留)</param>
        /// <param name="parking_id">车场ID</param>
        /// <param name="AreaID">停车区域ID</param>
        /// <param name="license_plate">车牌号码</param>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <returns></returns>
        [OperationContract]
        public string ReservePKBitZH(string ProxyNo, string AccountID, string BitNo, string parking_id, string AreaID, string license_plate, DateTime start_time, DateTime end_time)
        {
            WXReserveBitResult result = new WXReserveBitResult();
            result.code = -1;
            result.message = "预定车位失败";
            result.BitNo = BitNo;
            try
            {
                WXServiceCallback callback = new WXServiceCallback();
                return callback.ReservePKBit(ProxyNo, AccountID, BitNo, parking_id, AreaID, license_plate, start_time, end_time);
            }
            catch (Exception ex)
            {

                logger.Fatal("车位预定异常：WXReservePKBitZH()" + ex.Message + "\r\n");
            }
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 预约支付
        /// </summary>
        /// <param name="ReserveID">预约ID</param>
        /// <param name="OrderID">订单编号</param>
        /// <param name="Amount">金额</param>
        /// <param name="PKID">车场ID</param>
        /// <param name="OnlineOrderID">线上订单编码</param>
        /// <returns></returns>
        [OperationContract]
        public bool ReserveBitPayZH(string ProxyNo, string ReserveID, string OrderID, decimal Amount, string PKID, string OnlineOrderID)
        {
            try
            {
                WXServiceCallback callback = new WXServiceCallback();
                return callback.ReserveBitPay(ProxyNo, ReserveID, OrderID, Amount, PKID, OnlineOrderID);
            }
            catch (Exception ex)
            {

                logger.Fatal("预约支付异常：WXReserveBitPayZH()" + ex.Message + "\r\n");
            }
            return false;
        }

        #endregion
    }
}
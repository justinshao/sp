using System;
using System.ServiceModel;

namespace PlatformWcfServers
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IWXBoxServiceCallback
    {
        /// <summary>
        /// 测试代理连接是否正常
        /// </summary>
        /// <returns></returns>
        [OperationContract(IsOneWay = true)]
        void TestClientProxyConnectionCallBack();
        /// <summary>
        /// 锁车回调
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="PKID"></param>
        /// <param name="PlateNumber"></param>
        /// <returns></returns>
        [OperationContract]
        string LockCarInfoCallback(string AccountID, string PKID, string PlateNumber);

        /// <summary>
        /// 解锁回调
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="PKID"></param>
        /// <param name="PlateNumber"></param>
        /// <returns></returns>
        [OperationContract]
        string UnLockCarInfoCallback(string AccountID, string PKID, string PlateNumber);

        /// <summary>
        /// 获取临时卡费用
        /// </summary>
        /// <param name="RecordID"></param>
        /// <param name="PKID"></param>
        /// <param name="OrderSource"></param>
        /// <param name="CalculatDate"></param>
        /// <param name="AccountID"></param>
        /// <param name="CenterTime"></param>
        /// <param name="PlateNumber"></param>
        /// <returns></returns>
        [OperationContract]
        string TempParkingFeeCallback(string RecordID, string PKID, int OrderSource, DateTime CalculatDate, string AccountID, int CenterTime, string PlateNumber);

        /// <summary>
        /// 扫码获取临时车费用
        /// </summary>
        /// <param name="PKID"></param>
        /// <param name="PKName"></param>
        /// <param name="OrderSource"></param>
        /// <param name="BoxID"></param>
        /// <param name="ProxyNo"></param>
        /// <param name="AccountID"></param>
        /// <param name="CenterTime"></param>
        /// <returns></returns>
        [OperationContract]
        string ScanCodeTempParkingFeeCallback(string PKID, string PKName, int OrderSource, string BoxID, string ProxyNo, string AccountID, int CenterTime);

        /// <summary>
        /// 扫码获取临时车费用
        /// </summary>
        /// <param name="PKID"></param>
        /// <param name="PKName"></param>
        /// <param name="OrderSource"></param>
        /// <param name="BoxID"></param>
        /// <param name="ProxyNo"></param>
        /// <param name="AccountID"></param>
        /// <param name="CenterTime"></param>
        /// <returns></returns>
        [OperationContract]
        string ScanCodeTempParkingFeeByGateIDCallback(string PKID, string PKName, int OrderSource, string GateID, string ProxyNo, string AccountID, int CenterTime);
        /// <summary>
        /// 临停缴费
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="PayWay"></param>
        /// <param name="Amount"></param>
        /// <param name="PKID"></param>
        /// <param name="OnlineOrderID"></param>
        /// <param name="PayDate"></param>
        /// <returns></returns>
        [OperationContract]
        string TempStopPaymentCallback(string OrderID, int PayWay, decimal Amount, string PKID, string OnlineOrderID, DateTime PayDate);

        /// <summary>
        /// 月卡开卡
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [OperationContract]
        string CIAddMonthlyInfoCallback(string Model);
        /// <summary>
        /// 扫码进出
        /// </summary>
        /// <param name="GateNo"></param>
        /// <param name="OpenId"></param>
        /// <param name="PlateNo"></param>
        /// <returns></returns>
        [OperationContract]
        int ScanCodeInOutCallback(string GateNo, string OpenId, string PlateNo);
        #region 消费打折

        /// <summary>
        /// 获取商户打折信息
        /// </summary>
        /// <param name="sellerID"></param>
        /// <param name="VID"></param>
        /// <returns></returns>
        [OperationContract]
        string GetParkDerateCallback(string sellerID, string VID);

        /// <summary>
        /// 获取车牌进场信息
        /// </summary>
        /// <param name="PlateNumber"></param>
        /// <param name="VID"></param>
        /// <param name="SellerID"></param>
        /// <returns></returns>
        [OperationContract]
        string GetPKIORecordByPlateNumberCallback(string PlateNumber, string VID, string SellerID);

        /// <summary>
        /// 消费打折
        /// </summary>
        /// <param name="IORecordID"></param>
        /// <param name="DerateID"></param>
        /// <param name="VID"></param>
        /// <param name="SellerID"></param>
        /// <param name="DerateMoney"></param>
        /// <returns></returns>
        [OperationContract]
        string DiscountPlateNumberCallback(string IORecordID, string DerateID, string VID, string SellerID, decimal DerateMoney);

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="SellerID"></param>
        /// <returns></returns>
        [OperationContract]
        string GetSellerInfoCallback(string SellerID);

        /// <summary>
        /// 获取商户信息2
        /// </summary>
        /// <param name="SellerNo"></param>
        /// <param name="PWD"></param>
        /// <param name="SellerID"></param>
        /// <returns></returns>
        [OperationContract]
        string GetSellerInfoCallback2(string SellerNo, string PWD, string SellerID);

        /// <summary>
        /// 修改商户密码
        /// </summary>
        /// <param name="SellerID"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        [OperationContract]
        bool EditSellerPwd(string SellerID, string Pwd);

        /// <summary>
        /// 获取商家打折详情
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        [OperationContract]
        string WXParkCarDerateCallback(string parms, int rows, int pageindex, string ProxyNo);


        #endregion

        #region 车场运营

        /// <summary>
        /// 远程开闸
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PKID"></param>
        /// <param name="GateID"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        [OperationContract]
        int RemoteGateCallback(string UserID, string PKID, string GateID, string Remark);
        #endregion

        #region 车位预定

        /// <summary>
        /// 车位预定
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="parking_id"></param>
        /// <param name="license_plate"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        [OperationContract]
        string ReservePKBitCallback(string AccountID, string BitNo, string parking_id, string AreaID, string license_plate, DateTime start_time, DateTime end_time);

        /// <summary>
        /// 车位预定珠海定制
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="BitNo"></param>
        /// <param name="parking_id"></param>
        /// <param name="AreaID"></param>
        /// <param name="license_plate"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        [OperationContract]
        string ReservePKBitZHCallback(string AccountID, string BitNo, string parking_id, string AreaID, string license_plate, DateTime start_time, DateTime end_time);

        /// <summary>
        /// 预约支付
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="Amount"></param>
        /// <param name="PKID"></param>
        /// <param name="OnlineOrderID"></param>
        /// <returns></returns>
        [OperationContract]
        bool ReserveBitPayCallback(string ReserveID, string OrderID, decimal Amount, string PKID, string OnlineOrderID);
        #endregion
    }
}
using Common.Core;
using Common.Core.Helpers;
using Common.DataAccess;
using Common.Entities.WebAPI;
using Common.Entities.WX;
using Common.Utilities.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.ServiceModel;
using System.Threading;

namespace PlatformWcfServers
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“FoundationService”。
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IWXServiceCallback))]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class WXServiceCallback
    {
        private static Logger logger = LogManager.GetLogger("平台回调服务");
        
        /// <summary>
        /// 回调集合
        /// </summary>
        private static ConcurrentDictionary<string, WXClient> dit_callback = new ConcurrentDictionary<string, WXClient>();

        public static event EventHandler HeartChanged;
        public static event EventHandler HeartdelteChanged;

        private static void StartLog(string msg)
        {
            logger.Info($"======>>{msg} {DateTime.Now.ToString("HH:mm:ss fff")}");
        }

        private static void EndLog(string msg)
        {
            logger.Info($"<<======{msg} {DateTime.Now.ToString("HH:mm:ss fff")}\r\n");
        }

        public static bool IsOnline(string proxyNo)
        {
            return dit_callback.ContainsKey(proxyNo);
        }
        
        public bool IsCallbackService()
        {
            return true;
        }
        
        public string GetName()
        {
            return "平台回调服务";
        }

        /// <summary>
        /// 代理服务登录
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <returns></returns>
        [OperationContract]
        public int Login(string ProxyNo)
        {
            logger.Info("开始登陆平台:" + ProxyNo + "\r\n");
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select VName from BaseVillage where ProxyNo=@ProxyNo";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ProxyNo", ProxyNo);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {

                            WXClient agent = new WXClient(OperationContext.Current.GetCallbackChannel<IWXServiceCallback>(), DateTime.Now, ProxyNo);
                            agent.Villagename = reader["VName"].ToString();
                            dit_callback[ProxyNo] = agent;
                            reader.Close();
                            strsql = "update BaseVillage set IsOnLine=1 where ProxyNo=@ProxyNo";
                            dbOperator.ClearParameters();
                            dbOperator.AddParameter("ProxyNo", ProxyNo);
                            dbOperator.ExecuteNonQuery(strsql);

                            if (HeartChanged != null)
                            {
                                HeartChanged(agent, null);
                            }
                        }
                        else
                        {
                            logger.Fatal($"{ProxyNo} 登陆平台失败: 没有查到小区信息\r\n");
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal($"{ProxyNo} 登陆平台失败\r\n", ex);
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 测试代理连接是否正常
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool TestClientProxyConnection(string ProxyNo)
        {
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    client.callback.TestClientProxyConnectionCallBack();

                    return true;
                }
                catch (Exception ex)
                {
                    logger.Fatal($"{client.Villagename} {ProxyNo} 代理连接异常\r\n", ex);

                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #region 锁车解锁
        /// <summary>
        /// 锁车
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="PKID"></param>
        /// <param name="PlateNumber"></param>
        /// <returns></returns>
        [OperationContract]
        public int LockCarInfo(string AccountID, string PKID, string PlateNumber, string ProxyNo)
        {
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 锁车");

                    var result = client.callback.LockCarInfoCallback(AccountID, PKID, PlateNumber);
                    LockCarResult resultmodel = JsonHelper.GetJson<LockCarResult>(result);
                    if (resultmodel.reuslt)
                    {
                        try
                        {
                            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                            {
                                string strsql = "select * from WX_LockCar where RecordID=@RecordID and DataStatus<2";
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", resultmodel.RecordID);
                                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                                {
                                    if (reader.Read())
                                    {
                                        reader.Close();
                                        strsql = "update WX_LockCar set AccountID=@AccountID,PKID=@PKID,PlateNumber=@PlateNumber,Status=1,LockDate=getdate() where RecordID=@RecordID";
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("AccountID", AccountID);
                                        dbOperator.AddParameter("PKID", PKID);
                                        dbOperator.AddParameter("PlateNumber", PlateNumber);
                                        dbOperator.AddParameter("RecordID", resultmodel.RecordID);
                                        dbOperator.ExecuteNonQuery(strsql);
                                    }
                                    else
                                    {
                                        reader.Close();
                                        strsql = "insert into WX_LockCar(RecordID,AccountID,PKID,PlateNumber,Status,LockDate,DataStatus)";
                                        strsql += "values(@RecordID,@AccountID,@PKID,@PlateNumber,1,getdate(),0)";
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("RecordID", resultmodel.RecordID);
                                        dbOperator.AddParameter("AccountID", AccountID);
                                        dbOperator.AddParameter("PKID", PKID);
                                        dbOperator.AddParameter("PlateNumber", PlateNumber);
                                        dbOperator.ExecuteNonQuery(strsql);
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                        return 0;
                    }
                    else
                    {
                        logger.Fatal(ProxyNo + "锁车异常：LockCarInfo()\r\n");
                        return 2;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 锁车异常：LockCarInfo()\r\n", ex);
                    return 2;
                }
                finally
                {
                    EndLog(ProxyNo + " 锁车");
                }
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="PKID"></param>
        /// <param name="PlateNumber"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        [OperationContract]
        public int UlockCarInfo(string AccountID, string PKID, string PlateNumber, string ProxyNo)
        {
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 解锁");

                    var result = client.callback.UnLockCarInfoCallback(AccountID, PKID, PlateNumber);
                    LockCarResult resultmodel = JsonHelper.GetJson<LockCarResult>(result);
                    if (resultmodel.reuslt)
                    {
                        try
                        {
                            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                            {
                                string strsql = "select * from WX_LockCar where RecordID=@RecordID and DataStatus<2";
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", resultmodel.RecordID);
                                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                                {
                                    if (reader.Read())
                                    {
                                        reader.Close();
                                        strsql = "update WX_LockCar set AccountID=@AccountID,PKID=@PKID,PlateNumber=@PlateNumber,Status=0,UnLockDate=getdate() where RecordID=@RecordID";
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("AccountID", AccountID);
                                        dbOperator.AddParameter("PKID", PKID);
                                        dbOperator.AddParameter("PlateNumber", PlateNumber);
                                        dbOperator.AddParameter("RecordID", resultmodel.RecordID);
                                        dbOperator.ExecuteNonQuery(strsql);
                                    }
                                    else
                                    {
                                        logger.Warn("找不到解锁车辆信息：LockCarInfo()\r\n");
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                        return 0;
                    }
                    else
                    {
                        logger.Fatal(ProxyNo + " 解锁异常：UlockCarInfo()\r\n");
                        return 2;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 解锁异常：UlockCarInfo()\r\n", ex);
                    return 2;
                }
                finally
                {
                    EndLog(ProxyNo + " 解锁");
                }
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #region 线上缴费
        /// <summary>
        /// 月卡续期
        /// </summary>
        /// <param name="PlateNumber"></param>
        /// <param name="PKID"></param>
        /// <param name="MonthNum"></param>
        /// <param name="Amount"></param>
        /// <param name="AccountID"></param>
        /// <param name="PayWay"></param>
        /// <param name="OrderSource"></param>
        /// <param name="OnlineOrderID"></param>
        /// <param name="PayDate"></param>
        /// <param name="ProxyNo"></param>
        [OperationContract]
        public MonthlyRenewalResult MonthlyRenewals(string CardID, string PKID, int MonthNum, decimal Amount, string AccountID, int PayWay, int OrderSource, string OnlineOrderID, DateTime PayDate, string ProxyNo)
        {
            MonthlyRenewalResult result = new MonthlyRenewalResult();

            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 月卡续期");

                    string res = client.callback.MonthlyRenewalsCallback(CardID, PKID, MonthNum, Amount, AccountID, PayWay, OrderSource, OnlineOrderID, PayDate);
                    result = JsonHelper.GetJson<MonthlyRenewalResult>(res);
                    if (result.Result == APPResult.Normal)
                    {

                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 月卡续期异常：MonthlyRenewals()\r\n", ex);
                    result.Result = APPResult.OtherException;
                }
                finally
                {
                    EndLog(ProxyNo + " 月卡续期");
                }
            }
            else
            {
                result.Result = APPResult.ProxyException;
            }

            return result;
        }

        /// <summary>
        /// 获取临时卡费用
        /// </summary>
        /// <param name="RecordID"></param>
        /// <param name="PKID"></param>
        /// <param name="OrderSource"></param>
        /// <param name="CalculatDate"></param>
        /// <param name="ProxyNo"></param>
        /// <param name="AccountID"></param>
        /// <param name="CenterTime"></param>
        /// <param name="PlateNumber"></param>
        /// <returns></returns>
        [OperationContract]
        public TempParkingFeeResult TempParkingFee(string RecordID, string PKID, int OrderSource, DateTime CalculatDate, string ProxyNo, string AccountID, int CenterTime, string PlateNumber)
        {
            TempParkingFeeResult result = new TempParkingFeeResult();
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 获取临时卡费用");

                    string res = client.callback.TempParkingFeeCallback(RecordID, PKID, OrderSource, CalculatDate, AccountID, CenterTime, PlateNumber);
                    result = JsonHelper.GetJson<TempParkingFeeResult>(res);
                    if (result.Result == APPResult.Normal)
                    {
                        //try
                        //{
                        //    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                        //    {
                        //        dbOperator.BeginTransaction();
                        //        string strsql = "update ParkOrder set Status=-1 where TagID=@TagID and OrderType=@OrderType and PayAmount=0 and Status=0 and PKID=@PKID";
                        //        dbOperator.ClearParameters();
                        //        dbOperator.AddParameter("TagID", RecordID);
                        //        dbOperator.AddParameter("OrderType", result.Pkorder.OrderType);
                        //        dbOperator.AddParameter("PKID", PKID);
                        //        if (dbOperator.ExecuteNonQuery(strsql) < 0)
                        //        {
                        //            dbOperator.RollbackTransaction();
                        //            return result;
                        //        }
                        //        strsql = "insert into ParkOrder(RecordID,OrderNo,TagID,OrderType,PayWay,Amount,UnPayAmount,PayAmount,DiscountAmount,CarderateID,Status,OrderSource,OrderTime,PayTime,";
                        //        strsql += "OldUserBegin,OldUserulDate,NewUserBegin,NewUsefulDate,OldMoney,NewMoney,PKID,UserID,OnlineUserID,OnlineOrderNo,Remark,LastUpdateTime,HaveUpdate,DataStatus,";
                        //        strsql += "CashTime,CashMoney)values(@RecordID,@OrderNo,@TagID,@OrderType,@PayWay,@Amount,@UnPayAmount,@PayAmount,@DiscountAmount,@CarderateID,@Status,@OrderSource,@OrderTime,@PayTime,";
                        //        strsql += "@OldUserBegin,@OldUserulDate,@NewUserBegin,@NewUsefulDate,@OldMoney,@NewMoney,@PKID,@UserID,@OnlineUserID,@OnlineOrderNo,@Remark,@LastUpdateTime,@HaveUpdate,@DataStatus,";
                        //        strsql += "@CashTime,@CashMoney)";
                        //        dbOperator.ClearParameters();
                        //        dbOperator.AddParameter("RecordID", result.Pkorder.RecordID);

                        //        dbOperator.AddParameter("OrderNo", result.Pkorder.OrderNo);
                        //        dbOperator.AddParameter("TagID", result.Pkorder.TagID);
                        //        dbOperator.AddParameter("OrderType", result.Pkorder.OrderType);
                        //        dbOperator.AddParameter("PayWay", result.Pkorder.PayWay);
                        //        dbOperator.AddParameter("Amount", result.Pkorder.Amount);
                        //        dbOperator.AddParameter("UnPayAmount", result.Pkorder.UnPayAmount);
                        //        dbOperator.AddParameter("PayAmount", result.Pkorder.PayAmount);
                        //        dbOperator.AddParameter("DiscountAmount", result.Pkorder.DiscountAmount);
                        //        dbOperator.AddParameter("CarderateID", result.Pkorder.CarderateID);
                        //        dbOperator.AddParameter("Status", result.Pkorder.Status);
                        //        dbOperator.AddParameter("OrderSource", result.Pkorder.OrderSource);
                        //        if (result.Pkorder.OrderTime == DateTime.MinValue)
                        //        {
                        //            dbOperator.AddParameter("OrderTime", DBNull.Value);
                        //        }
                        //        else
                        //        {
                        //            dbOperator.AddParameter("OrderTime", result.Pkorder.OrderTime);
                        //        }
                        //        if (result.Pkorder.PayTime == DateTime.MinValue)
                        //        {
                        //            dbOperator.AddParameter("PayTime", DBNull.Value);
                        //        }
                        //        else
                        //        {
                        //            dbOperator.AddParameter("PayTime", result.Pkorder.PayTime);
                        //        }
                        //        if (result.Pkorder.OldUserBegin == DateTime.MinValue)
                        //        {
                        //            dbOperator.AddParameter("OldUserBegin", DBNull.Value);
                        //        }
                        //        else
                        //        {
                        //            dbOperator.AddParameter("OldUserBegin", result.Pkorder.OldUserBegin);
                        //        }
                        //        if (result.Pkorder.OldUserulDate == DateTime.MinValue)
                        //        {
                        //            dbOperator.AddParameter("OldUserulDate", DBNull.Value);
                        //        }
                        //        else
                        //        {
                        //            dbOperator.AddParameter("OldUserulDate", result.Pkorder.OldUserulDate);
                        //        }
                        //        if (result.Pkorder.NewUserBegin == DateTime.MinValue)
                        //        {
                        //            dbOperator.AddParameter("NewUserBegin", DBNull.Value);
                        //        }
                        //        else
                        //        {
                        //            dbOperator.AddParameter("NewUserBegin", result.Pkorder.NewUserBegin);
                        //        }
                        //        if (result.Pkorder.NewUsefulDate == DateTime.MinValue)
                        //        {
                        //            dbOperator.AddParameter("NewUsefulDate", DBNull.Value);
                        //        }
                        //        else
                        //        {
                        //            dbOperator.AddParameter("NewUsefulDate", result.Pkorder.NewUsefulDate);
                        //        }

                        //        dbOperator.AddParameter("OldMoney", result.Pkorder.OldMoney);
                        //        dbOperator.AddParameter("NewMoney", result.Pkorder.NewMoney);
                        //        dbOperator.AddParameter("PKID", result.Pkorder.PKID);
                        //        dbOperator.AddParameter("UserID", result.Pkorder.UserID);
                        //        dbOperator.AddParameter("OnlineUserID", result.Pkorder.OnlineUserID);
                        //        dbOperator.AddParameter("OnlineOrderNo", result.Pkorder.OnlineOrderNo);
                        //        dbOperator.AddParameter("Remark", result.Pkorder.Remark);
                        //        if (result.Pkorder.LastUpdateTime == DateTime.MinValue)
                        //        {
                        //            dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                        //        }
                        //        else
                        //        {
                        //            dbOperator.AddParameter("LastUpdateTime", result.Pkorder.LastUpdateTime);
                        //        }

                        //        dbOperator.AddParameter("HaveUpdate", 0);
                        //        dbOperator.AddParameter("DataStatus", result.Pkorder.DataStatus);
                        //        if (result.Pkorder.CashTime == DateTime.MinValue)
                        //        {
                        //            dbOperator.AddParameter("CashTime", DBNull.Value);
                        //        }
                        //        else
                        //        {
                        //            dbOperator.AddParameter("CashTime", result.Pkorder.CashTime);
                        //        }

                        //        dbOperator.AddParameter("CashMoney", result.Pkorder.CashMoney);
                        //        if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                        //        {
                        //            dbOperator.RollbackTransaction();
                        //            return result;
                        //        }
                        //        dbOperator.CommitTransaction();
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    logger.Fatal("获取临时卡费用修改数据库异常：TempParkingFee()" + ex.Message + "\r\n");
                        //}
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 获取临时卡费用异常：TempParkingFee()\r\n", ex);
                    result.Result = APPResult.OtherException;
                }
                finally
                {
                    EndLog(ProxyNo + " 获取临时卡费用");
                }
            }
            else
            {
                result.Result = APPResult.ProxyException;
            }
            return result;
        }

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
        public TempParkingFeeResult ScanCodeTempParkingFee(string PKID, string PKName, int OrderSource, string BoxID, string ProxyNo, string AccountID, int CenterTime)
        {
            TempParkingFeeResult result = new TempParkingFeeResult();
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 获取临时卡费用");

                    string res = client.callback.ScanCodeTempParkingFeeCallback(PKID, PKName, OrderSource, BoxID, ProxyNo, AccountID, CenterTime);
                    result = JsonHelper.GetJson<TempParkingFeeResult>(res);
                    if (result.Result == APPResult.Normal)
                    {

                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 获取临时卡费用异常：ScanCodeTempParkingFee()\r\n", ex);
                    result.Result = APPResult.OtherException; ;
                }
                finally
                {
                    EndLog(ProxyNo + " 获取临时卡费用");
                }
            }
            else
            {
                result.Result = APPResult.ProxyException;
            }
            return result;
        }

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
        public TempParkingFeeResult ScanCodeTempParkingFeebyGateID(string PKID, string PKName, int OrderSource, string GateID, string ProxyNo, string AccountID, int CenterTime)
        {
            TempParkingFeeResult result = new TempParkingFeeResult();
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 获取临时卡费用");

                    string res = client.callback.ScanCodeTempParkingFeeByGateIDCallback(PKID, PKName, OrderSource, GateID, ProxyNo, AccountID, CenterTime);
                    result = JsonHelper.GetJson<TempParkingFeeResult>(res);
                    if (result.Result == APPResult.Normal)
                    {

                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 获取临时卡费用异常：ScanCodeTempParkingFeebyGateID()\r\n", ex);
                    result.Result = APPResult.OtherException; ;
                }
                finally
                {
                    EndLog(ProxyNo + " 获取临时卡费用");
                }
            }
            else
            {
                result.Result = APPResult.ProxyException;
            }
            return result;
        }

        /// <summary>
        /// 临时卡缴费
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="PayWay"></param>
        /// <param name="Amount"></param>
        /// <param name="PKID"></param>
        /// <param name="ProxyNo"></param>
        /// <param name="OnlineOrderID"></param>
        /// <param name="PayDate"></param>
        /// <returns></returns>
        [OperationContract]
        public TempStopPaymentResult TempStopPayment(string OrderID, int PayWay, decimal Amount, string PKID, string ProxyNo, string OnlineOrderID, DateTime PayDate)
        {
            TempStopPaymentResult result = new TempStopPaymentResult();
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 临时卡缴费");

                    string res = client.callback.TempStopPaymentCallback(OrderID, PayWay, Amount, PKID, OnlineOrderID, PayDate);
                    result = JsonHelper.GetJson<TempStopPaymentResult>(res);
                    if (result.Result == APPResult.Normal)
                    {

                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 临时卡缴费异常：TempParkingFee()\r\n", ex);
                    result.Result = APPResult.OtherException; ;
                }
                finally
                {
                    EndLog(ProxyNo + " 临时卡缴费");
                }
            }
            else
            {
                result.Result = APPResult.ProxyException;
            }
            return result;
        }
        #endregion

        #region 月卡开卡
        [OperationContract]
        public AddMonthlyInfoResult CIAddMonthlyInfo(CIMonthlyInfo Model, string ProxyNo)
        {
            AddMonthlyInfoResult result = new AddMonthlyInfoResult();
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 临时卡缴费");

                    string res = client.callback.CIAddMonthlyInfoCallback(JsonHelper.GetJsonString(Model));
                    result = JsonHelper.GetJson<AddMonthlyInfoResult>(res);
                    if (result.Res == 0)
                    {

                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 临时卡缴费异常：TempParkingFee()\r\n", ex);
                    result.Res = -1;
                }
                finally
                {
                    EndLog(ProxyNo + " 临时卡缴费");
                }
            }
            else
            {
                result.Res = -1;
            }
            return result;
        }

        #endregion
        #region 扫码出入场
        /// <summary>
        /// 扫码出入场
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="GateNo"></param>
        /// <param name="OpenId"></param>
        /// <returns></returns>
        [OperationContract]
        public int ScanCodeInOut(string ProxyNo, string GateNo, string OpenId, string PlateNo)
        {
            MonthlyRenewalResult result = new MonthlyRenewalResult();
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 扫码出入场");

                    return client.callback.ScanCodeInOutCallback(GateNo, OpenId, PlateNo);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 扫码出入场异常！ScanCodeInOut()\r\n", ex);
                    return 1002;
                }
                finally
                {
                    EndLog(ProxyNo + " 扫码出入场");
                }
            }
            else
            {
                return 1003;
            }
        }

        #endregion

        #region 消费打折

        /// <summary>
        /// 获取商户打折信息
        /// </summary>
        /// <param name="sellerID"></param>
        /// <param name="VID"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetParkDerate(string sellerID, string VID, string ProxyNo)
        {
            string strderate = "";
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 获取商户打折信息");

                    strderate = client.callback.GetParkDerateCallback(sellerID, VID);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 获取商户打折信息异常：GetParkDerate()\r\n", ex);
                }
                finally
                {
                    EndLog(ProxyNo + " 获取商户打折信息");
                }
            }
            return strderate;
        }

        /// <summary>
        /// 获取车牌进场信息
        /// </summary>
        /// <param name="PlateNumber"></param>
        /// <param name="VID"></param>
        /// <param name="SellerID"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetPKIORecordByPlateNumber(string PlateNumber, string VID, string SellerID, string ProxyNo)
        {
            string strderate = "";
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 获取车牌进场信息");

                    strderate = client.callback.GetPKIORecordByPlateNumberCallback(PlateNumber, VID, SellerID);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + "获取车牌进场信息异常：GetPKIORecordByPlateNumber()\r\n", ex);
                }
                finally
                {
                    EndLog(ProxyNo + " 获取车牌进场信息");
                }
            }
            return strderate;
        }

        /// <summary>
        /// 消费打折
        /// </summary>
        /// <param name="IORecordID"></param>
        /// <param name="DerateID"></param>
        /// <param name="VID"></param>
        /// <param name="SellerID"></param>
        /// <param name="DerateMoney"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        [OperationContract]
        public string DiscountPlateNumber(string IORecordID, string DerateID, string VID, string SellerID, decimal DerateMoney, string ProxyNo)
        {
            string strderate = "";
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 消费打折");

                    strderate = client.callback.DiscountPlateNumberCallback(IORecordID, DerateID, VID, SellerID, DerateMoney);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + "消费打折异常：DiscountPlateNumber()\r\n", ex);
                }
                finally
                {
                    EndLog(ProxyNo + " 消费打折");
                }
            }
            return strderate;
        }

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="SellerID"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetSellerInfo(string SellerID, string ProxyNo)
        {
            string strderate = "";
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 获取商户信息");

                    strderate = client.callback.GetSellerInfoCallback(SellerID);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + "获取商户信息异常：GetSellerInfo()\r\n", ex);
                }
                finally
                {
                    EndLog(ProxyNo + " 获取商户信息");
                }
            }

            return strderate;
        }

        /// <summary>
        /// 获取商户信息2
        /// </summary>
        /// <param name="SellerNo"></param>
        /// <param name="PWD"></param>
        /// <param name="SellerID"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        public string GetSellerInfo2(string SellerNo, string PWD, string SellerID, string ProxyNo)
        {
            string strderate = "";
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 获取商户信息");

                    strderate = client.callback.GetSellerInfoCallback2(SellerNo, PWD, SellerID);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 获取商户信息异常：GetSellerInfo2()\r\n", ex);
                }
                finally
                {
                    EndLog(ProxyNo + " 获取商户信息");
                }
            }
            return strderate;
        }

        /// <summary>
        /// 修改商户密码
        /// </summary>
        /// <param name="SellerID"></param>
        /// <param name="Pwd"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        public bool EditSellerPwd(string SellerID, string Pwd, string ProxyNo)
        {
            bool result = false;
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 获取商户信息");

                    result = client.callback.EditSellerPwd(SellerID, Pwd);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 获取商户信息异常：GetSellerInfo2()\r\n", ex);
                }
                finally
                {
                    EndLog(ProxyNo + " 获取商户信息");
                }
            }
            return result;
        }

        /// <summary>
        /// 获取商家打折详情
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public string GetParkCarDerate(string parms, int rows, int pageindex, string ProxyNo)
        {
            string strderate = "";
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 获取商家打折详情");

                    strderate = client.callback.WXParkCarDerateCallback(parms, rows, pageindex, ProxyNo);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 获取商家打折详情异常：WXGetParkDerate()\r\n", ex);
                }
                finally
                {
                    EndLog(ProxyNo + " 获取商家打折详情");
                }
            }
            return strderate;
        }


        #endregion

        #region 车场运营

        /// <summary>
        /// 远程开闸
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="UserID"></param>
        /// <param name="PKID"></param>
        /// <param name="GateID"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        [OperationContract]
        public int RemoteGate(string ProxyNo, string UserID, string PKID, string GateID, string Remark)
        {
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 远程开闸");

                    return client.callback.RemoteGateCallback(UserID, PKID, GateID, Remark);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 远程开闸异常：RemoteGate()\r\n", ex);
                    return 3;
                }
                finally
                {
                    EndLog(ProxyNo + " 远程开闸");
                }
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #region 车位预定
        /// <summary>
        /// 预定车位
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="parking_id"></param>
        /// <param name="license_plate"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        [OperationContract]
        public string ReservePKBit(string ProxyNo, string AccountID, string BitNo, string parking_id, string AreaID, string license_plate, DateTime start_time, DateTime end_time)
        {
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 预定车位");

                    return client.callback.ReservePKBitCallback(AccountID, BitNo, parking_id, AreaID, license_plate, start_time, end_time);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + "预定车位异常：ReservePKBit()\r\n", ex);
                    return "";
                }
                finally
                {
                    EndLog(ProxyNo + " 预定车位");
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 预约支付
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="Amount"></param>
        /// <param name="PKID"></param>
        /// <param name="OnlineOrderID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool ReserveBitPay(string ProxyNo, string ReserveID, string OrderID, decimal Amount, string PKID, string OnlineOrderID)
        {
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 预约支付");

                    return client.callback.ReserveBitPayCallback(ReserveID, OrderID, Amount, PKID, OnlineOrderID);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 预约支付异常：ReservePKBit()\r\n", ex);
                    return false;
                }
                finally
                {
                    EndLog(ProxyNo + " 预约支付");
                }
            }
            else
            {
                return false;
            }
        }

        #endregion
        #region 心跳
        
        public static void StartListenClients()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                while (true)
                {
                    TestClientConnection();

                    Thread.Sleep(1000 * 3);
                }
            });
        }

        static void TestClientConnection()
        {
            foreach (var key in dit_callback.Keys)
            {
                if (dit_callback.TryGetValue(key, out WXClient client))
                {
                    try
                    {
                        client.callback.TestClientProxyConnectionCallBack();
                    }
                    catch(Exception ex)
                    {
                        try
                        {
                            dit_callback.TryRemove(key, out client);

                            if (HeartdelteChanged != null)
                            {
                                HeartdelteChanged(client, null);
                            }

                            var smsMobile = OptionHelper.ReadString("CenterServer", "SmsMobile", "");
                            if(!string.IsNullOrEmpty(smsMobile))
                            {
                                Util.SendSms("94552", new Dictionary<string, string>() { { "vname", client.Villagename } }, smsMobile);
                            }

                            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                            {
                                string strsql = "update BaseVillage set IsOnLine=0 where ProxyNo=@ProxyNo";
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("ProxyNo", key);
                                dbOperator.ExecuteNonQuery(strsql);
                            }
                        }
                        catch(Exception e)
                        {
                            logger.Fatal(e.Message, e);
                        }

                        logger.Fatal($"{client.Villagename} {key} 掉线\r\n", ex);
                    }
                }
            }
        }

        /// <summary>
        /// 车场代理主动断开连接
        /// </summary>
        /// <param name="AgencyName"></param>
        [OperationContract(IsOneWay = true)]
        public void Leave(string ProxyNo)
        {
            WXClient client = null;
            if (dit_callback.TryRemove(ProxyNo, out client))
            {
                if (HeartdelteChanged != null)
                {
                    HeartdelteChanged(client, null);
                }

                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "update BaseVillage set IsOnLine=0 where ProxyNo=@ProxyNo";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                        dbOperator.ExecuteNonQuery(strsql);
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 实时心跳
        /// </summary>
        /// <param name="AgencyName"></param>
        [OperationContract(IsOneWay = true)]
        public void Update(string ProxyNo)
        {
            WXClient client = null;

            if (dit_callback.TryGetValue(ProxyNo, out client))
            {
                client.updatetime = DateTime.Now;
                client.callback = OperationContext.Current.GetCallbackChannel<IWXServiceCallback>();
            }
            else
            {
                try
                {
                    WXClient agent = new WXClient(OperationContext.Current.GetCallbackChannel<IWXServiceCallback>(), DateTime.Now, ProxyNo);
                    dit_callback[ProxyNo] = agent;
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select VName from BaseVillage where ProxyNo=@ProxyNo";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                agent.Villagename = reader["VName"].ToString();
                                reader.Close();
                                strsql = "update BaseVillage set IsOnLine=1 where ProxyNo=@ProxyNo";
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("ProxyNo", ProxyNo);
                                dbOperator.ExecuteNonQuery(strsql);

                                if (HeartChanged != null)
                                {
                                    HeartChanged(agent, null);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        #endregion

        #region 珠海定制

        /// <summary>
        /// 预定车位
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="AccountID"></param>
        /// <param name="BitNo"></param>
        /// <param name="parking_id"></param>
        /// <param name="AreaID"></param>
        /// <param name="license_plate"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        [OperationContract]
        public string ReservePKBitZH(string ProxyNo, string AccountID, string BitNo, string parking_id, string AreaID, string license_plate, DateTime start_time, DateTime end_time)
        {
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 预定车位");

                    return client.callback.ReservePKBitZHCallback(AccountID, BitNo, parking_id, AreaID, license_plate, start_time, end_time);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 预定车位异常：ReservePKBit()\r\n", ex);
                    return "";
                }
                finally
                {
                    EndLog(ProxyNo + " 预定车位");
                }
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 获取月卡信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="city_id"></param>
        /// <param name="parking_id"></param>
        /// <param name="license_plate"></param>
        /// <param name="page"></param>
        /// <param name="per_page"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetMonthCard(string ProxyNo, string city_id, string parking_id, string license_plate, int page, int per_page)
        {
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 远程开闸");

                    return client.callback.GetMonthCardCallback(city_id, parking_id, license_plate, page, per_page);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 远程开闸异常：RemoteGate()" + ex.Message + "\r\n", ex);
                    return "";
                }
                finally
                {
                    EndLog(ProxyNo + " 远程开闸");
                }
            }
            else
            {
                return "";
            }
        }


        #endregion

        #region 中兴订制


        /// <summary>
        /// 支付完成通知
        /// </summary>
        /// <param name="IORec"></param>
        /// <param name="PKID"></param>
        /// <param name="ProxyNo"></param>
        /// <param name="amout"></param>
        /// <returns></returns>
        [OperationContract]
        public string IssuedZX(string IORec, string PKID, string carNumber, string ProxyNo, int amout)
        {
            if (dit_callback.TryGetValue(ProxyNo, out WXClient client))
            {
                try
                {
                    StartLog(ProxyNo + " 支付完成通知");

                    return client.callback.IssuedZXCallback(IORec, PKID, carNumber, amout);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ProxyNo + " 支付完成通知异常：QueryFeeZX()" + ex.Message + "\r\n", ex);
                    return "";
                }
                finally
                {
                    EndLog(ProxyNo + " 支付完成通知");
                }
            }
            else
            {
                return "";
            }
        }
        #endregion

        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        internal static void SendNotify(string title, string message)
        {
            foreach (var c in dit_callback)
            {
                try
                {
                    c.Value.callback.NotifyReceived(title, message);
                }
                catch (Exception ex)
                {
                    logger.Fatal($"通知发送失败 {c.Value.Villagename}：{c.Key}\r\n", ex);
                }
            }
        }
    }

    public class WXClient : INotifyPropertyChanged
    {
        public IWXServiceCallback callback;
        public DateTime updatetime;
        public string VillageID;
        private string villagename;
        public string Villagename
        {
            get { return villagename; }
            set
            {
                villagename = value;
                Notify("Villagename");
            }
        }

        private DateTime connectionDate = DateTime.Now;

        public DateTime ConnectionDate
        {
            get { return connectionDate; }
            set
            {
                connectionDate = value;
                Notify("ConnectionDate");
            }
        }

        public WXClient(IWXServiceCallback callback, DateTime updatetime, string VillageID)
        {
            this.callback = callback;
            this.updatetime = updatetime;
            this.VillageID = VillageID;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
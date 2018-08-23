using Common.CarInOutValidation;
using Common.Core;
using Common.DataAccess;
using Common.Entities;
using Common.Entities.Other;
using Common.Entities.Parking;
using Common.Entities.Statistics;
using Common.Entities.Validation;
using Common.Entities.WebAPI;
using Common.Entities.WX;
using Common.Services.BaseData;
using Common.Services.Park;
using Common.Services.Statistics;
using Common.Utilities;
using Common.Utilities.Helpers;
using ServicesDLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace PlatformWcfServers
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“FoundationService”。
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IWXBoxServiceCallback))]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class WXBoxServiceCallback
    {
        /// <summary>
        /// 日志记录对象
        /// </summary>
        protected Logger logger = LogManager.GetLogger("岗亭服务");
        public static event EventHandler HeartChanged;
        public static event EventHandler HeartdelteChanged;
        /// <summary>
        /// 定义一个静态对象用于线程部份代码块的锁定，用于lock操作
        /// </summary>
        private static Object syncObj = new Object();

        /// <summary>
        /// 回调集合
        /// </summary>
        public static Dictionary<string, WXBoxClient> dit_callback = new Dictionary<string, WXBoxClient>();

        /// <summary>
        /// 是否回调服务
        /// </summary>
        /// <returns></returns>
        public bool IsCallbackService()
        {
            return true;
        }

        /// <summary>
        /// 服务名称
        /// </summary>
        /// <returns></returns>
        //public string GetName()
        //{
        //    return "微信岗亭回调服务";
        //}

        /// <summary>
        /// 代理服务登录
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <returns></returns>
        [OperationContract]
        public int Login(string ProxyNo)
        {
            lock (syncObj)
            {
                dit_callback.Remove(ProxyNo);
                try
                {
                    try
                    {
                        using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                        {
                            string strsql = "select a.BoxID,BoxName,d.VID from ParkBox a left join ParkArea b on a.AreaID=b.AreaID ";
                            strsql += "left join BaseParkinfo c on b.PKID=c.PKID left join BaseVillage d on c.VID=d.VID where a.BoxID=@BoxID";

                            dbOperator.ClearParameters();
                            dbOperator.AddParameter("BoxID", ProxyNo);
                            using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                            {
                                if (reader.Read())
                                {

                                    WXBoxClient agent = new WXBoxClient(OperationContext.Current.GetCallbackChannel<IWXBoxServiceCallback>(), DateTime.Now);
                                    agent.BoxName = reader["BoxName"].ToString();
                                    agent.BoxID = ProxyNo;
                                    agent.VillageID = reader["VID"].ToString();
                                    dit_callback.Add(ProxyNo, agent);
                                    reader.Close();
                                    strsql = "update ParkBox set IsOnLine=1 where BoxID=@BoxID";
                                    dbOperator.ClearParameters();
                                    dbOperator.AddParameter("BoxID", ProxyNo);
                                    dbOperator.ExecuteNonQuery(strsql);

                                    if (HeartChanged != null)
                                    {
                                        HeartChanged(agent, null);
                                    }
                                }
                                else
                                {
                                    return 0;
                                }
                            }
                        }
                    }
                    catch
                    {
                        return 0;
                    }


                }
                catch
                {
                    return 0;
                }
                return 1;

            }
        }

        /// <summary>
        /// 测试代理连接是否正常
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool TestClientProxyConnection(string ProxyNo)
        {
            if (dit_callback.ContainsKey(ProxyNo))
            {
                try
                {
                    return dit_callback[ProxyNo].callback.TestClientProxyConnectionCallBack();
                }
                catch (Exception ex)
                {
                    logger.Fatal("测试代理服务器是否正常异常！TestClientProxyConnection()" + ex.StackTrace + "\r\n");
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
            int res = 2;
            string result = "";
            lock (syncObj)
            {
                var proxys = dit_callback.Where(a => a.Value.VillageID == ProxyNo);
                foreach (var obj in proxys)
                {
                    try
                    {
                        result = dit_callback[obj.Key].callback.LockCarInfoCallback(AccountID, PKID, PlateNumber);
                        LockCarResult resultmodel = JsonHelper.GetJson<LockCarResult>(result);
                        if (resultmodel.reuslt)
                        {
                            res = 0;
                        }
                        else
                        {
                            logger.Fatal("锁车异常：LockCarInfo()\r\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("锁车异常：LockCarInfo()" + ex.Message + "\r\n");
                    }
                }
            }
            return res;
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
            int res = 2;
            string result = "";
            lock (syncObj)
            {
                var proxys = dit_callback.Where(a => a.Value.VillageID == ProxyNo);
                foreach (var obj in proxys)
                {
                    try
                    {
                        result = dit_callback[obj.Key].callback.UnLockCarInfoCallback(AccountID, PKID, PlateNumber);
                        LockCarResult resultmodel = JsonHelper.GetJson<LockCarResult>(result);
                        if (resultmodel.reuslt)
                        {
                            res = 0;
                        }
                        else
                        {
                            logger.Fatal("解锁异常：UlockCarInfo()\r\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("解锁异常：UlockCarInfo()" + ex.Message + "\r\n");
                    }
                }
            }
            return res;
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
            MonthlyRenewalResult Result = new MonthlyRenewalResult();
            Result.PKID = PKID;
            bool ispay = false;
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select count(PKID) as countnum from ParkOrder where PKID=@PKID and OnlineOrderNo=@OnlineOrderNo";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", PKID);
                    dbOperator.AddParameter("OnlineOrderNo", OnlineOrderID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            ispay = reader["countnum"].ToInt() == 0 ? false : true;
                            if (ispay)
                            {
                                Result.Result = APPResult.RepeatPay;
                            }
                        }
                    }

                    if (!ispay)
                    {
                        ParkGrant grant = new ParkGrant();
                        strsql = "select a.GID,b.PlateNo,a.BeginDate,a.EndDate from ParkGrant a left join EmployeePlate b on a.PlateID=b.PlateID where CardID=@CardID and PKID=@PKID and a.DataStatus<2";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("CardID", CardID);
                        dbOperator.AddParameter("PKID", PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                grant.GID = reader["GID"].ToString();
                                grant.BeginDate = reader["BeginDate"].ToDateTime();
                                grant.EndDate = reader["EndDate"].ToDateTime();
                                Result.PlateNumber = reader["PlateNo"].ToString();
                            }
                            else
                            {
                                Result.Result = APPResult.NotFindCard;//找不到月卡信息
                                return Result;
                            }
                        }

                        DateTime OldUserulDate = DateTime.Now;
                        DateTime NewUsefulDate = DateTime.Now;
                        if (grant.EndDate == null || grant.EndDate.ToString() == "0001-01-01 00:00:00")
                        {
                            OldUserulDate = DateTime.Now;
                            NewUsefulDate = OldUserulDate.AddMonths(MonthNum);
                        }
                        else
                        {
                            OldUserulDate = grant.EndDate;
                            NewUsefulDate = grant.EndDate.AddMonths(MonthNum);
                        }

                        dbOperator.BeginTransaction();
                        strsql = "update ParkGrant set EndDate=@EndDate,HaveUpdate=3,LastUpdateTime=@LastUpdateTime";
                        strsql += " where GID=@GID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("EndDate", NewUsefulDate);
                        dbOperator.AddParameter("LastUpdateTime", PayDate);
                        dbOperator.AddParameter("GID", grant.GID);
                        if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                        {
                            logger.Fatal(Result.PlateNumber + "月租卡缴费修改卡片有效期失败！MonthlyRenewalsCallback()\r\n");
                            dbOperator.RollbackTransaction();
                            Result.Result = APPResult.OtherException;
                            return Result;
                        }

                        ParkOrder order = new ParkOrder();
                        order.RecordID = System.Guid.NewGuid().ToString();
                        order.OrderNo = IdGenerator.Instance.GetId().ToString();
                        order.TagID = grant.GID;
                        order.OrderType = (OrderType)2;
                        order.PayWay = (OrderPayWay)PayWay;
                        order.Amount = Amount;
                        order.UnPayAmount = 0;
                        order.PayAmount = Amount;
                        order.Status = 1;
                        order.OrderSource = (OrderSource)OrderSource;
                        order.OrderTime = PayDate;
                        order.Remark = "月卡缴费,缴费月数:" + MonthNum;
                        order.OldUserulDate = OldUserulDate;
                        order.NewUsefulDate = NewUsefulDate;
                        order.LastUpdateTime = PayDate;
                        order.HaveUpdate = 3;
                        order.PKID = PKID;
                        order.DataStatus = 0;
                        order.OnlineUserID = AccountID;
                        order.OnlineOrderNo = OnlineOrderID;
                        order.PayTime = PayDate;
                        strsql = "insert into ParkOrder(RecordID,OrderNo,TagID,OrderType,PayWay,Amount,UnPayAmount,PayAmount,Status,OrderSource,OrderTime,Remark,";
                        strsql += "OldUserulDate,NewUsefulDate,OldMoney,NewMoney,LastUpdateTime,HaveUpdate,PKID,DataStatus,OnlineUserID,OnlineOrderNo,PayTime)";
                        strsql += "values(@RecordID,@OrderNo,@TagID,@OrderType,@PayWay,@Amount,@UnPayAmount,@PayAmount,@Status,@OrderSource,@OrderTime,@Remark,";
                        strsql += "@OldUserulDate,@NewUsefulDate,@OldMoney,@NewMoney,@LastUpdateTime,@HaveUpdate,@PKID,@DataStatus,@OnlineUserID,@OnlineOrderNo,@PayTime)";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", order.RecordID);
                        dbOperator.AddParameter("OrderNo", order.OrderNo);
                        dbOperator.AddParameter("TagID", order.TagID);
                        dbOperator.AddParameter("OrderType", order.OrderType);
                        dbOperator.AddParameter("PayWay", order.PayWay);
                        dbOperator.AddParameter("Amount", order.Amount);
                        dbOperator.AddParameter("UnPayAmount", order.UnPayAmount);
                        dbOperator.AddParameter("PayAmount", order.PayAmount);
                        dbOperator.AddParameter("Status", order.Status);
                        dbOperator.AddParameter("OrderSource", order.OrderSource);
                        dbOperator.AddParameter("OrderTime", order.OrderTime);
                        dbOperator.AddParameter("Remark", order.Remark);
                        dbOperator.AddParameter("OldUserulDate", order.OldUserulDate);
                        dbOperator.AddParameter("NewUsefulDate", order.NewUsefulDate);
                        dbOperator.AddParameter("OldMoney", order.OldMoney);
                        dbOperator.AddParameter("NewMoney", order.NewMoney);
                        dbOperator.AddParameter("LastUpdateTime", order.LastUpdateTime);
                        dbOperator.AddParameter("HaveUpdate", order.HaveUpdate);
                        dbOperator.AddParameter("PKID", order.PKID);
                        dbOperator.AddParameter("DataStatus", order.DataStatus);
                        dbOperator.AddParameter("OnlineUserID", order.OnlineUserID);
                        dbOperator.AddParameter("OnlineOrderNo", order.OnlineOrderNo);
                        dbOperator.AddParameter("PayTime", order.PayTime);
                        if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                        {
                            logger.Fatal(Result.PlateNumber + "月租卡缴费保存订单失败！MonthlyRenewalsCallback()\r\n");
                            dbOperator.RollbackTransaction();
                            Result.Result = APPResult.OtherException;
                            return Result;
                        }
                        dbOperator.CommitTransaction();
                        Result.Pkorder = order;
                        Result.Result = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Fatal("月租卡缴费异常！MonthlyRenewalsCallback()" + ex.Message + "\r\n");
                Result.Result = APPResult.OtherException;
            }

            return Result;
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
            result.ParkingID = PKID;
            result.OutTime = CenterTime;
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select a.AreaID,a.CarTypeID,a.CardNo,a.EntranceTime,a.PlateNumber,b.BaseTypeID,b.MaxUseMoney,c.CarModelID as CarTypeIDDefault,e.PKName,e.OnlineDiscount,e.IsOnlineDiscount ";
                    strsql += "   from ParkIORecord a LEFT JOIN ParkCarType b on a.CarTypeID=b.CarTypeID  ";
                    strsql += "left join ParkCarModel c on a.CarModelID=c.CarModelID left join BaseParkinfo e on a.ParkingID=e.PKID ";
                    strsql += " where a.RecordID=@RecordID and c.IsDefault=1 and a.DataStatus<2 and b.DataStatus<2 and c.DataStatus<2 ";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", RecordID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            int MaxUseMoney = reader["MaxUseMoney"].ToInt();
                            string CardTypeID = reader["CarTypeID"].ToString();
                            string CarTypeID = reader["CarTypeIDDefault"].ToString();
                            result.ParkName = reader["PKName"].ToString();
                            result.CardNo = reader["CardNo"].ToString();
                            result.EntranceDate = reader["EntranceTime"].ToDateTime();
                            result.PlateNumber = reader["PlateNumber"].ToString();
                            int BaseTypeID = reader["BaseTypeID"].ToInt();
                            string AreaID = reader["AreaID"].ToString();
                            decimal OnlineDiscount = reader["OnlineDiscount"].ToDecimal();
                            bool IsOnlineDiscount = reader["IsOnlineDiscount"].ToBoolean();
                            reader.Close();
                            if (BaseTypeID != 1 && BaseTypeID != 3)//不是临时卡或储值卡不需要缴费
                            {
                                result.Result = APPResult.NoTempCard;
                            }
                            else
                            {
                                ParkIORecord ior = new ParkIORecord();
                                string strsql2 = "select * from ParkIORecord where RecordID=@RecordID";
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", RecordID);
                                using (DbDataReader reader2 = dbOperator.ExecuteReader(strsql2.ToString()))
                                {

                                    if (reader2.Read())
                                    {
                                        ior = DataReaderToModel<ParkIORecord>.ToModel(reader2);
                                    }
                                }
                                ParkOrder order = null;
                                #region 找不到5分钟内的订单
                                ParkGate outgate = new ParkGate();
                                outgate.IoState = IoState.GoOut;
                                ResultAgs resultags = RateProcesser.GetRateResult(ior, outgate, CalculatDate);
                                string CarderateID = "";
                                if (resultags.Carderates != null)
                                {
                                    foreach (var obj in resultags.Carderates)
                                    {
                                        CarderateID += obj.CarDerateID + ",";
                                    }
                                }

                                RateInfo rateinfo = resultags.Rate;
                                if (rateinfo.Amount != 0)
                                {
                                    if (IsOnlineDiscount && OnlineDiscount != 10)
                                    {
                                        rateinfo.DiscountAmount += rateinfo.Amount - (decimal)((double)rateinfo.Amount * ((double)OnlineDiscount * 0.1));
                                    }
                                    decimal PayAmount = 0;
                                    DateTime MaxOrderTime = DateTime.Now;
                                    strsql = "select sum(PayAmount) as PayAmount,max(OrderTime) as OrderTime from ParkOrder where OrderType=1 and Status=1 and TagID=@TagID and PKID=@PKID and DataStatus<2 ";
                                    dbOperator.ClearParameters();
                                    dbOperator.AddParameter("PKID", PKID);
                                    dbOperator.AddParameter("TagID", RecordID);
                                    using (DbDataReader reader2 = dbOperator.ExecuteReader(strsql.ToString()))
                                    {
                                        if (reader2.Read())
                                        {
                                            PayAmount = reader2["PayAmount"].ToDecimal();
                                            MaxOrderTime = reader2["OrderTime"].ToDateTime();
                                        }
                                    }
                                    strsql = "select * from ParkOrder where TagID=@TagID and OrderType=1 and PKID=@PKID and (Status=0 or Status=2)";
                                    dbOperator.ClearParameters();
                                    dbOperator.AddParameter("TagID", RecordID);
                                    dbOperator.AddParameter("OrderType", 1);
                                    dbOperator.AddParameter("PKID", PKID);
                                    bool ishave = false;
                                    using (DbDataReader reader2 = dbOperator.ExecuteReader(strsql.ToString()))
                                    {
                                        if (reader2.Read())
                                        {
                                            order = DataReaderToModel<ParkOrder>.ToModel(reader2);
                                            ishave = true;
                                        }
                                    }

                                    if (order == null)
                                    {
                                        order = new ParkOrder();
                                        order.RecordID = System.Guid.NewGuid().ToString();
                                        order.OrderNo = IdGenerator.Instance.GetId().ToString();
                                    }

                                    order.TagID = RecordID;
                                    order.PayWay = (OrderPayWay)2;
                                    order.Amount = rateinfo.Amount - rateinfo.DiscountAmount;
                                    order.UnPayAmount = rateinfo.Amount - rateinfo.DiscountAmount;
                                    order.DiscountAmount = rateinfo.DiscountAmount;
                                    order.PayAmount = 0;
                                    order.Status = 0;
                                    order.OrderSource = (OrderSource)OrderSource;
                                    order.OrderTime = CalculatDate;
                                    order.Remark = "临停缴费";
                                    order.LastUpdateTime = CalculatDate;
                                    order.HaveUpdate = 3;
                                    order.OrderType = (OrderType)1;
                                    order.OnlineUserID = AccountID;
                                    order.PKID = PKID;
                                    order.DataStatus = 0;
                                    order.CarderateID = CarderateID;
                                    order.UserID = "";
                                    if (ishave)
                                    {
                                        strsql = "update ParkOrder set PayWay=@PayWay,Amount=@Amount,UnPayAmount=@UnPayAmount,PayAmount=@PayAmount,Status=@Status,OrderSource=@OrderSource,OrderTime=@OrderTime,Remark=@Remark,";
                                        strsql += "LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,PKID=@PKID,DataStatus=@DataStatus,OnlineUserID=@OnlineUserID,CashTime=@CashTime,CashMoney=@CashMoney,DiscountAmount=@DiscountAmount,CarderateID=@CarderateID ";
                                        strsql += ",UserID=@UserID where RecordID=@RecordID";
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("RecordID", order.RecordID);
                                        dbOperator.AddParameter("PayWay", order.PayWay);
                                        dbOperator.AddParameter("Amount", order.Amount);
                                        dbOperator.AddParameter("UnPayAmount", order.UnPayAmount);
                                        dbOperator.AddParameter("PayAmount", order.PayAmount);
                                        dbOperator.AddParameter("Status", order.Status);
                                        dbOperator.AddParameter("OrderSource", order.OrderSource);
                                        dbOperator.AddParameter("OrderTime", order.OrderTime);
                                        dbOperator.AddParameter("Remark", order.Remark);
                                        dbOperator.AddParameter("LastUpdateTime", order.LastUpdateTime);
                                        dbOperator.AddParameter("HaveUpdate", order.HaveUpdate);
                                        dbOperator.AddParameter("PKID", order.PKID);
                                        dbOperator.AddParameter("DataStatus", order.DataStatus);
                                        dbOperator.AddParameter("OnlineUserID", order.OnlineUserID);
                                        dbOperator.AddParameter("CashTime", order.CashTime);
                                        dbOperator.AddParameter("CashMoney", order.CashMoney);
                                        dbOperator.AddParameter("DiscountAmount", order.DiscountAmount);
                                        dbOperator.AddParameter("CarderateID", order.CarderateID);
                                        dbOperator.AddParameter("UserID", order.UserID);
                                        if (dbOperator.ExecuteNonQuery(strsql) < 0)
                                        {
                                            logger.Fatal(result.PlateNumber + "修改原来订单状态错误！CalculatingTempCostCallBack()\r\n");
                                            result.Result = APPResult.OtherException;//生成订单错误
                                        }
                                        else
                                        {
                                            result.Pkorder = order;
                                            order.PayAmount = PayAmount;
                                            result.Result = APPResult.Normal;//生产订单正常
                                        }
                                    }
                                    else
                                    {
                                        strsql = "insert into ParkOrder(RecordID,OrderNo,TagID,OrderType,PayWay,Amount,UnPayAmount,PayAmount,Status,OrderSource,OrderTime,Remark,LastUpdateTime,HaveUpdate,PKID,DataStatus,OnlineUserID,CashTime,CashMoney,DiscountAmount,CarderateID)";
                                        strsql += " values (@RecordID,@OrderNo,@TagID,@OrderType,@PayWay,@Amount,@UnPayAmount,@PayAmount,@Status,@OrderSource,@OrderTime,@Remark,@LastUpdateTime,@HaveUpdate,@PKID,@DataStatus,@OnlineUserID,@CashTime,@CashMoney,@DiscountAmount,@CarderateID)";
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("RecordID", order.RecordID);
                                        dbOperator.AddParameter("OrderNo", order.OrderNo);
                                        dbOperator.AddParameter("TagID", order.TagID);
                                        dbOperator.AddParameter("OrderType", order.OrderType);
                                        dbOperator.AddParameter("PayWay", order.PayWay);
                                        dbOperator.AddParameter("Amount", order.Amount);
                                        dbOperator.AddParameter("UnPayAmount", order.UnPayAmount);
                                        dbOperator.AddParameter("PayAmount", order.PayAmount);
                                        dbOperator.AddParameter("Status", order.Status);
                                        dbOperator.AddParameter("OrderSource", order.OrderSource);
                                        dbOperator.AddParameter("OrderTime", order.OrderTime);
                                        dbOperator.AddParameter("Remark", order.Remark);
                                        dbOperator.AddParameter("LastUpdateTime", order.LastUpdateTime);
                                        dbOperator.AddParameter("HaveUpdate", order.HaveUpdate);
                                        dbOperator.AddParameter("PKID", order.PKID);
                                        dbOperator.AddParameter("DataStatus", order.DataStatus);
                                        dbOperator.AddParameter("OnlineUserID", order.OnlineUserID);
                                        dbOperator.AddParameter("CashTime", order.CashTime);
                                        dbOperator.AddParameter("CashMoney", order.CashMoney);
                                        dbOperator.AddParameter("DiscountAmount", order.DiscountAmount);
                                        dbOperator.AddParameter("CarderateID", order.CarderateID);
                                        if (dbOperator.ExecuteNonQuery(strsql) < 0)
                                        {
                                            logger.Fatal(result.PlateNumber + "生产订单错误！CalculatingTempCostCallBack()\r\n");
                                            result.Result = APPResult.OtherException;//生成订单错误
                                        }
                                        else
                                        {
                                            result.Pkorder = order;
                                            order.PayAmount = PayAmount;
                                            result.Result = APPResult.Normal;//生产订单正常
                                        }


                                    }
                                }
                                else
                                {
                                    result.Result = APPResult.NoNeedPay;//不需要缴费
                                }
                                #endregion
                            }

                        }
                        else
                        {
                            result.Result = APPResult.NotFindIn;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("临时卡缴费异常！TempParkingFeeCallback()" + ex.Message + "\r\n");
                result.Result = APPResult.OtherException;
            }
            return result;
        }

        /// <summary>
        /// 扫码获取临时车费用(废弃)
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
            result.ParkingID = PKID;
            result.OutTime = CenterTime;
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
        public TempParkingFeeResult ScanCodeTempParkingFeebyGateID(string PKID, string PKName, int OrderSource, string GateID, string ProxyNo, string AccountID, int CenterTime, string BoxID)
        {
            TempParkingFeeResult result = new TempParkingFeeResult();
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(BoxID))
                {
                    try
                    {
                        string res = dit_callback[BoxID].callback.ScanCodeTempParkingFeeByGateIDCallback(PKID, PKName, OrderSource, GateID, ProxyNo, AccountID, CenterTime);
                        result = JsonHelper.GetJson<TempParkingFeeResult>(res);
                        if (result.Result == APPResult.Normal)
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("获取临时卡费用异常：ScanCodeTempParkingFeebyGateID()" + ex.Message + "\r\n");
                        result.Result = APPResult.OtherException; ;
                    }
                }
                else
                {
                    result.Result = APPResult.ProxyException;
                }
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
            result.Result = APPResult.OtherException;
            lock (syncObj)
            {
                var proxys = dit_callback.Where(a => a.Value.VillageID == ProxyNo);
                foreach (var obj in proxys)
                {
                    try
                    {
                        string res = dit_callback[ProxyNo].callback.TempStopPaymentCallback(OrderID, PayWay, Amount, PKID, OnlineOrderID, PayDate);
                        TempStopPaymentResult result2 = JsonHelper.GetJson<TempStopPaymentResult>(res);
                        if (result2.Result == APPResult.Normal)
                        {
                            result = result2;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("临时卡缴费异常：TempParkingFee()" + ex.Message + "\r\n");

                    }
                }
            }
            return result;
        }
        #endregion

        #region 月卡开卡
        [OperationContract]
        public AddMonthlyInfoResult CIAddMonthlyInfo(CIMonthlyInfo Greant, string ProxyNo)
        {
            AddMonthlyInfoResult result = new AddMonthlyInfoResult();
            result.Res = -1;
            try
            {

                BaseEmployee model = new BaseEmployee();
                model.EmployeeName = Greant.EmployeeName;
                model.RegTime = DateTime.Now;
                model.MobilePhone = Greant.MobilePhone;
                model.VID = Greant.VID;
                model.EmployeeType = EmployeeType.Owner;
                model.EmployeeID = GuidGenerator.GetGuidString();
                model.FamilyAddr = model.Remark;
                model.Remark = "线上开卡";

                EmployeePlate modelplate = new EmployeePlate();
                modelplate.PlateNo = Greant.PlateNumber;

                string errorMsg = string.Empty;
                EmployeePlate dbPlate = EmployeePlateServices.GetEmployeePlateNumberByPlateNumber(Greant.VID, Greant.PlateNumber, out errorMsg);
                if (!string.IsNullOrWhiteSpace(errorMsg))
                {
                    logger.Fatal("开卡异常！获取车牌失败\r\n");
                    return result; ;
                }
                if (dbPlate != null)
                {
                    dbPlate.Color = PlateColor.Blue;
                    dbPlate.EmployeeID = model.EmployeeID;
                    modelplate = dbPlate;
                }
                else
                {
                    modelplate.Color = PlateColor.Blue;
                    modelplate.PlateID = GuidGenerator.GetGuidString();
                    modelplate.EmployeeID = model.EmployeeID;
                }

                BaseCard card = new BaseCard();
                card.CardID = GuidGenerator.GetGuidString();
                card.CardNo = modelplate.PlateNo;
                card.CardNumb = card.CardNo;
                card.VID = Greant.VID;
                BaseCard oldCard = BaseCardServices.QueryBaseCardByParkingId(Greant.PKID, card.CardNo);
                if (oldCard != null)
                {
                    logger.Fatal("开卡异常！车牌号已存在，不能重复添加\r\n");
                    result.Res = 2;
                    return result;
                }
                card.RegisterTime = DateTime.Now;
                card.OperatorID = "线上开卡";
                card.CardSystem = CardSystem.Park;
                card.EmployeeID = model.EmployeeID;
                card.CardType = CardType.Plate;

                ParkGrant modelg = new ParkGrant();
                modelg.PKID = Greant.PKID;
                modelg.CardID = card.CardID;
                modelg.PlateID = modelplate.PlateID;
                modelg.PlateNo = modelplate.PlateNo;
                modelg.PKLot = Greant.PKLot;
                modelg.CarModelID = Greant.CarModelID;
                modelg.CarTypeID = Greant.CarTypeID;
                modelg.AreaIDS = Greant.AreaID;
                modelg.GateID = "";
                modelg.BeginDate = DateTime.ParseExact(Greant.BeginDate, "yyyyMMdd", Thread.CurrentThread.CurrentCulture);
                modelg.EndDate = DateTime.ParseExact(Greant.EndDate, "yyyyMMdd", Thread.CurrentThread.CurrentCulture);
                modelg.EndDate = modelg.EndDate.AddDays(-1);
                bool res = ParkGrantServices.Add(model, modelplate, card, modelg);
                if (!res)
                {
                    logger.Fatal("开卡异常！保存失败\r\n");
                    result.Res = -1;
                }
                else
                {
                    result.Res = 0;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("开卡异常！CIAddMonthlyInfoCallback()" + ex.Message + "\r\n");
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
            string BoxID = "";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = "select BoxID from ParkGate where GateID=@GateID";

                dbOperator.ClearParameters();
                dbOperator.AddParameter("GateID", GateNo);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    if (reader.Read())
                    {
                        BoxID = reader["BoxID"].ToString();
                    }
                }
            }
            MonthlyRenewalResult result = new MonthlyRenewalResult();
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(BoxID))
                {
                    try
                    {

                        return dit_callback[BoxID].callback.ScanCodeInOutCallback(GateNo, OpenId, PlateNo);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("扫码出入场异常！ScanCodeInOut()" + ex.StackTrace + "\r\n");
                        return 1002;
                    }
                }
                else
                {
                    return 1003;
                }
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
            return ProxyCallBackDLL.GetParkDerate(sellerID, VID);
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
            return ProxyCallBackDLL.GetPKIORecordByPlateNumber(PlateNumber, VID, SellerID);
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
            var proxys = dit_callback.Where(a => a.Value.VillageID == ProxyNo);
            foreach (var obj in proxys)
            {
                try
                {
                    string strderate2 = dit_callback[ProxyNo].callback.DiscountPlateNumberCallback(IORecordID, DerateID, VID, SellerID, DerateMoney);
                    ConsumerDiscountResult result = JsonHelper.GetJson<ConsumerDiscountResult>(strderate);
                    if (result.Result == 1)
                    {
                        strderate = strderate2;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("消费打折异常：DiscountPlateNumber()" + ex.Message + "\r\n");
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
            return ProxyCallBackDLL.GetSellerInfo(SellerID);
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
            return ProxyCallBackDLL.GetSellerInfo2(SellerNo, PWD, SellerID);
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
            return ProxyCallBackDLL.EditSellerPwd(SellerID, Pwd);
        }

        /// <summary>
        /// 获取商家打折详情
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public string GetParkCarDerate(string parms, int rows, int pageindex, string ProxyNo)
        {
            InParams inparams = JsonHelper.GetJson<InParams>(parms);
            Pagination pagination = StatisticsServices.Search_CarDerates(inparams, rows, pageindex);
            return JsonHelper.GetJsonString(pagination);
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
            string BoxID = "";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = "select BoxID from ParkGate where GateID=@GateID";

                dbOperator.ClearParameters();
                dbOperator.AddParameter("GateID", GateID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    if (reader.Read())
                    {
                        BoxID = reader["BoxID"].ToString();
                    }
                }
            }

            lock (syncObj)
            {
                if (dit_callback.ContainsKey(BoxID))
                {
                    try
                    {
                        return dit_callback[BoxID].callback.RemoteGateCallback(UserID, PKID, GateID, Remark);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("远程开闸异常：RemoteGate()" + ex.Message + "\r\n");
                        return 3;
                    }
                }
                else
                {
                    return 1;
                }
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
            string reuslt = "";
            lock (syncObj)
            {
                var proxys = dit_callback.Where(a => a.Value.VillageID == ProxyNo);
                foreach (var obj in proxys)
                {
                    try
                    {
                        string reuslt2 = dit_callback[ProxyNo].callback.ReservePKBitCallback(AccountID, BitNo, parking_id, AreaID, license_plate, start_time, end_time);
                        WXReserveBitResult aaa = JsonHelper.GetJson<WXReserveBitResult>(reuslt2);
                        if (aaa.code == 0)
                        {
                            reuslt = reuslt2;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("预定车位异常：ReservePKBit()" + ex.Message + "\r\n");
                        return "";
                    }
                }
            }
            return reuslt;
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
            bool res = false;
            lock (syncObj)
            {
                var proxys = dit_callback.Where(a => a.Value.VillageID == ProxyNo);
                foreach (var obj in proxys)
                {
                    try
                    {
                        bool res2 = dit_callback[obj.Key].callback.ReserveBitPayCallback(ReserveID, OrderID, Amount, PKID, OnlineOrderID);
                        if (res2)
                        {
                            res = res2;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("预约支付异常：ReservePKBit()" + ex.Message + "\r\n");

                    }
                }
            }
            return res;
        }

        #endregion
        #region 心跳

        static System.Timers.Timer timer1;
        public static void StartListenClients()
        {
            timer1 = new System.Timers.Timer();
            timer1.Interval = 2000;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
            timer1.Start();

        }

        static void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (syncObj)
            {
                foreach (string key in new List<string>(dit_callback.Keys))
                {
                    if (dit_callback[key].updatetime.AddSeconds(30) < DateTime.Now)
                    {
                        dit_callback.Remove(key);
                        try
                        {
                            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                            {
                                string strsql = "update ParkBox set IsOnLine=1 where BoxID=@BoxID";
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("BoxID", key);
                                dbOperator.ExecuteNonQuery(strsql);
                            }
                        }
                        catch
                        {
                        }
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
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo))
                {
                    if (HeartdelteChanged != null)
                    {
                        HeartdelteChanged(dit_callback[ProxyNo], null);
                    }
                    dit_callback.Remove(ProxyNo);
                    try
                    {
                        using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                        {
                            string strsql = "update ParkBox set IsOnLine=0 where BoxID=@BoxID";
                            dbOperator.ClearParameters();
                            dbOperator.AddParameter("BoxID", ProxyNo);
                            dbOperator.ExecuteNonQuery(strsql);
                        }
                    }
                    catch
                    {
                    }
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
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo))
                {
                    dit_callback[ProxyNo].updatetime = DateTime.Now;
                }
                else
                {
                    try
                    {
                        WXBoxClient agent = new WXBoxClient(OperationContext.Current.GetCallbackChannel<IWXBoxServiceCallback>(), DateTime.Now);
                        dit_callback.Add(ProxyNo, agent);
                        using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                        {
                            string strsql = "select a.BoxID,BoxName,d.VID from ParkBox a left join ParkArea b on a.AreaID=b.AreaID ";
                            strsql += "left join BaseParkinfo c on b.PKID=c.PKID left join BaseVillage d on c.VID=d.VID where a.BoxID=@BoxID";

                            dbOperator.ClearParameters();
                            dbOperator.AddParameter("BoxID", ProxyNo);
                            using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                            {
                                if (reader.Read())
                                {
                                    agent.BoxName = reader["BoxName"].ToString();
                                    agent.BoxID = ProxyNo;
                                    agent.VillageID = reader["VID"].ToString();
                                    dit_callback.Add(ProxyNo, agent);
                                    reader.Close();
                                    strsql = "update ParkBox set IsOnLine=1 where BoxID=@BoxID";
                                    dbOperator.ClearParameters();
                                    dbOperator.AddParameter("BoxID", ProxyNo);
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
        }

        #endregion
    }

    public class WXBoxClient : INotifyPropertyChanged
    {
        public IWXBoxServiceCallback callback;
        public DateTime updatetime;
        public string VillageID;
        private string boxName;
        public string BoxName
        {
            get { return boxName; }
            set
            {
                boxName = value;
                Notify("BoxName");
            }
        }
        public string BoxID { set; get; }
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

        public WXBoxClient(IWXBoxServiceCallback callback, DateTime updatetime)
        {
            this.callback = callback;
            this.updatetime = updatetime;
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
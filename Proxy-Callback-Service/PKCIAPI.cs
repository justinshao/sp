using Common.Core;
using Common.DataAccess;
using Common.Entities;
using Common.Entities.JNAPI;
using Common.Entities.Parking;
using Common.Entities.WebAPI;
using Common.Entities.WebAPI.BBTC;
using Common.Entities.WebAPI.BBTC.Bookparkspace;
using Common.Entities.WebAPI.BBTC.Carddelay;
using Common.Entities.WebAPI.BBTC.CreateOrderByCarNo;
using Common.Entities.WebAPI.BBTC.Lock;
using Common.Entities.WebAPI.BBTC.Notifyorderresult;
using Common.Entities.WebAPI.BBTC.Querycarddealylist;
using Common.Entities.WebAPI.BBTC.Querylockedcar;
using Common.Entities.WebAPI.BBTC.Queryorder;
using Common.Entities.WebAPI.BBTC.Queryparkout;
using Common.Entities.WebAPI.BBTC.Queryparkspace;
using Common.Entities.WebAPI.BBTC.Querypersonsbycar;
using Common.Entities.WX;
using Common.Entities.ZXAPI;
using Common.Services;
using Common.Services.BaseData;
using Common.Services.Park;
using Common.Utilities;
using Common.Utilities.Helpers;
using ServicesDLL;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.ServiceModel;

namespace PlatformWcfServers
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“FoundationService”。
    [ServiceContract]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public class PKCIAPI
    {
        private static Object objlock = new Object();////定义一个静态对象用于线程部份代码块的锁定，用于lock操作
        /// <summary>
        /// 日志记录对象
        /// </summary>
        protected Logger logger = LogManager.GetLogger("对外公共接口");
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
            return "对外公共接口服务";
        }

        #region 标准接口
        /// <summary>
        /// 根据车牌号获取车辆锁车解锁状态
        /// </summary>
        /// <param name="PlateNumber"></param>
        /// <param name="CID"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIGetLockCarInfo(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";

            if (string.IsNullOrEmpty(jsondata))
            {
                return JsonHelper.GetJsonString(result);
            }

            CIUserLockCarInfoAgt indata = new CIUserLockCarInfoAgt();
            try
            {
                indata = JsonHelper.GetJson<CIUserLockCarInfoAgt>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }
            try
            {
                List<WX_LockCar> list = new List<WX_LockCar>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var PlateNumber in indata.PlateNumbers)
                    {
                        string strsql = "select a.ParkingID,a.PlateNumber,b.VID,b.PKName,a.EntranceTime  ";
                        strsql += " from ParkIORecord a left join BaseParkinfo b on a.ParkingID=b.PKID left join BaseVillage c on b.VID=c.VID ";
                        strsql += " where a.PlateNumber=@PlateNumber ";
                        if (!string.IsNullOrEmpty(indata.CPID))
                        {
                            strsql += " and c.CPID=@CPID ";
                        }
                        strsql += "and MobileLock=1 and a.IsExit=0 and a.DataStatus<2 order by a.EntranceTime Desc";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PlateNumber", PlateNumber);
                        if (!string.IsNullOrEmpty(indata.CPID))
                        {
                            dbOperator.AddParameter("CPID", indata.CPID);
                        }
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                string ParkingID = reader["ParkingID"].ToString();
                                string VID = reader["VID"].ToString();
                                string PKName = reader["PKName"].ToString();
                                string EntranceTime = reader["EntranceTime"].ToDateTime().ToString("yyyyMMddHHmmss");

                                reader.Close();
                                using (DbOperator dbOperator2 = ConnectionManager.CreateReadConnection())
                                {
                                    strsql = "select RecordID,Status,LockDate,PlateNumber,PKID from WX_LockCar  ";
                                    strsql += " where PlateNumber=@PlateNumber  and PKID=@PKID and DataStatus<2 ";
                                    dbOperator2.ClearParameters();
                                    dbOperator2.AddParameter("PlateNumber", PlateNumber);
                                    dbOperator2.AddParameter("PKID", ParkingID);
                                    using (DbDataReader reader2 = dbOperator2.ExecuteReader(strsql.ToString()))
                                    {
                                        if (reader2.Read())
                                        {
                                            WX_LockCar model = new WX_LockCar();
                                            model.PlateNumber = reader2["PlateNumber"].ToString();
                                            model.Status = reader2["Status"].ToInt();
                                            model.LockDate = reader2["LockDate"].ToDateTime();
                                            model.RecordID = reader2["RecordID"].ToString();
                                            model.PKID = reader2["PKID"].ToString();
                                            model.VID = VID;
                                            model.PKName = PKName;
                                            model.EntranceTime = EntranceTime;
                                            list.Add(model);

                                        }
                                        else
                                        {
                                            WX_LockCar model = new WX_LockCar();
                                            model.PlateNumber = PlateNumber;
                                            model.Status = 0;
                                            model.PKID = ParkingID;
                                            model.VID = VID;
                                            model.PKName = PKName;
                                            model.EntranceTime = EntranceTime;
                                            list.Add(model);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                result.result = "0";
                result.data = list;

            }
            catch (Exception ex)
            {
                result.result = "1008";
                logger.Fatal("获取车辆锁车解锁状态异常：GetLockCarByPNS()" + ex.Message + "\r\n");

            }
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 锁车
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIUserLockCar(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";

            if (string.IsNullOrEmpty(jsondata))
            {
                return JsonHelper.GetJsonString(result);
            }
            CIUserLockCarAgt indata = new CIUserLockCarAgt();
            try
            {
                indata = JsonHelper.GetJson<CIUserLockCarAgt>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }
            lock (objlock)
            {
                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select b.ProxyNo from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKID=@PKID";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PKID", indata.PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                int res = callback.LockCarInfo(indata.AccountID, indata.PKID, indata.PlateNumber, reader["ProxyNo"].ToString());
                                if (res == 0)
                                {
                                    result.result = "0";
                                }
                                else if (res == 1)
                                {
                                    result.result = "1003";
                                }
                                else
                                {
                                    result.result = "1008";
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("锁车异常：CIUserLockCar()" + ex.Message + "\r\n");
                    result.result = "1008";
                }
            }
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIUserUnLockCar(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";

            if (string.IsNullOrEmpty(jsondata))
            {
                return JsonHelper.GetJsonString(result);
            }
            CIUserLockCarAgt indata = new CIUserLockCarAgt();
            try
            {
                indata = JsonHelper.GetJson<CIUserLockCarAgt>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }
            lock (objlock)
            {
                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select b.ProxyNo from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKID=@PKID";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PKID", indata.PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                int res = callback.UlockCarInfo(indata.AccountID, indata.PKID, indata.PlateNumber, reader["ProxyNo"].ToString());
                                if (res == 0)
                                {
                                    result.result = "0";
                                }
                                else if (res == 1)
                                {
                                    result.result = "1003";
                                }
                                else
                                {
                                    result.result = "1008";
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("解锁异常：WXUlockCarInfo()" + ex.Message + "\r\n");
                    result.result = "1008";
                }
            }
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 根据车牌号获取月卡信息
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIGetUserCardCar(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";

            if (string.IsNullOrEmpty(jsondata))
            {
                return JsonHelper.GetJsonString(result);
            }
            CIGetUserCardCarAgt indata = new CIGetUserCardCarAgt();
            try
            {
                indata = JsonHelper.GetJson<CIGetUserCardCarAgt>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }

            List<ParkUserCarInfo> list = new List<ParkUserCarInfo>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var PlateNumber in indata.PlateNumbers)
                    {
                        string strsql = "select c.CardID,c.State,c.Balance,f.PlateNo,d.BeginDate,d.EndDate,d.CarTypeID,d.PKID,e.BaseTypeID,e.IsAllowOnlIne, ";
                        strsql += "e.Amount,e.MaxMonth,e.MaxValue,g.PKName,g.VID,g.MobilePay,g.MobileLock from ";
                        strsql += " BaseEmployee b ";
                        strsql += "left join BaseCard c on  b.EmployeeID=c.EmployeeID left join ParkGrant d on c.CardID=d.CardID ";
                        strsql += "left join ParkCarType e on d.CarTypeID=e.CarTypeID left join EmployeePlate f on d.PlateID=f.PlateID ";
                        strsql += "left join BaseParkinfo g on d.PKID=g.PKID left join BaseVillage s on g.VID=s.VID  ";
                        strsql += "where f.PlateNo=@PlateNumber and b.DataStatus<2 and (e.BaseTypeID=1 or e.BaseTypeID=2) ";
                        if (!string.IsNullOrEmpty(indata.CPID))
                        {
                            strsql += " and s.CPID=@CPID ";
                        }
                        strsql += "and c.DataStatus<2 and d.DataStatus<2 and e.DataStatus<2 and f.DataStatus<2 and g.DataStatus<2 ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PlateNumber", PlateNumber);
                        if (!string.IsNullOrEmpty(indata.CPID))
                        {
                            dbOperator.AddParameter("CPID", indata.CPID);
                        }
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            while (reader.Read())
                            {
                                ParkUserCarInfo model = new ParkUserCarInfo();
                                model.Amount = reader["Amount"].ToDecimal();
                                model.Balance = reader["Balance"].ToDecimal();
                                model.BaseTypeID = reader["BaseTypeID"].ToString();
                                model.BeginDate = reader["BeginDate"].ToDateTime();
                                model.CardID = reader["CardID"].ToString();
                                model.CarTypeID = reader["CarTypeID"].ToString();
                                model.EndDate = reader["EndDate"].ToDateTime();
                                model.IsAllowOnlIne = reader["IsAllowOnlIne"].ToBoolean();
                                model.MaxMonth = reader["MaxMonth"].ToInt();
                                model.MaxValue = reader["MaxValue"].ToDecimal();
                                model.MobileLock = reader["MobileLock"].ToInt();
                                model.MobilePay = reader["MobilePay"].ToInt();
                                model.PKID = reader["PKID"].ToString();
                                model.PKName = reader["PKName"].ToString();
                                model.PlateNumber = reader["PlateNo"].ToString();
                                model.State = reader["State"].ToInt();
                                model.VID = reader["VID"].ToString();
                                list.Add(model);
                            }
                        }
                    }

                }
                result.result = "0";
                result.data = list;

            }
            catch (Exception ex)
            {
                logger.Fatal("根据车牌号码获取长期卡信息异常：CIGetUserCardCar()" + ex.Message + "\r\n");
                result.result = "1008";
            }

            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 月租缴费
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIMonthlyPay(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";

            if (string.IsNullOrEmpty(jsondata))
            {
                return JsonHelper.GetJsonString(result);
            }
            CIMonthlyPayAgt indata = new CIMonthlyPayAgt();
            try
            {
                indata = JsonHelper.GetJson<CIMonthlyPayAgt>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }

            lock (objlock)
            {
                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select b.ProxyNo from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKID=@PKID";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PKID", indata.PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                DateTime paydate = DateTime.ParseExact(indata.PayDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                                MonthlyRenewalResult res = callback.MonthlyRenewals(indata.CardID, indata.PKID, indata.MonthNum, indata.Amount, indata.AccountID, indata.PayWay, 6, indata.OnlineOrderID, paydate, reader["ProxyNo"].ToString());
                                result.result = "0";
                                CIPayAgt agt = new CIPayAgt();
                                agt.PlateNumber = res.PlateNumber;
                                agt.PKID = res.PKID;
                                agt.OnlineOrderID = res.OnlineOrderID;
                                agt.Result = res.Result;
                                if (res.Pkorder != null)
                                {
                                    CIOrder order = new CIOrder();
                                    order.RecordID = res.Pkorder.RecordID;
                                    order.OrderNo = res.Pkorder.OrderNo;
                                    order.OrderType = (int)res.Pkorder.OrderType;
                                    order.PayWay = (int)res.Pkorder.PayWay;
                                    order.Amount = res.Pkorder.Amount;
                                    order.UnPayAmount = res.Pkorder.UnPayAmount;
                                    order.PayAmount = res.Pkorder.PayAmount;
                                    order.Status = (int)res.Pkorder.Status;
                                    if (res.Pkorder.OrderTime != null)
                                        order.OrderTime = res.Pkorder.OrderTime.ToString("yyyyMMddHHmmss");
                                    if (res.Pkorder.OldUserulDate != null)
                                        order.OldUserulDate = res.Pkorder.OldUserulDate.ToString("yyyyMMddHHmmss");
                                    if (res.Pkorder.NewUsefulDate != null)
                                        order.NewUsefulDate = res.Pkorder.NewUsefulDate.ToString("yyyyMMddHHmmss");
                                    order.OnlineUserID = res.Pkorder.OnlineUserID;
                                    order.OnlineOrderNo = res.Pkorder.OnlineOrderNo;
                                    agt.Pkorder = order;
                                }
                                result.data = agt;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("月租车辆续期异常：CIMonthlyPay()" + ex.Message + "\r\n");
                    result.result = "1008";

                }
            }
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 根据车牌号获取车辆在车场信息
        /// </summary>
        /// <param name="jsondate"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetMonthlyInfo(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";
            if (string.IsNullOrEmpty(jsondata))
            {
                return JsonHelper.GetJsonString(result);
            }
            CIGetMonthlyInfo indata = new CIGetMonthlyInfo();
            try
            {
                indata = JsonHelper.GetJson<CIGetMonthlyInfo>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }
            List<CIMonthlyInfo> list = new List<CIMonthlyInfo>();
            lock (objlock)
            {
                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select a.GID,a.PKID,b.PKName,c.CarTypeName,d.CarModelName as CarModel,e.PlateNo as PlateNumber,a.BeginDate,a.EndDate,a.PKLot,";
                        strsql += "a.State,f.EmployeeName,f.MobilePhone,f.FamilyAddr ";
                        strsql += "from ParkGrant a left join BaseParkinfo b on a.PKID=b.PKID ";
                        strsql += "left join ParkCarType c on a.CarTypeID=c.CarTypeID left join ParkCarModel d on a.CarModelID=d.CarModelID ";
                        strsql += "left join EmployeePlate e on a.PlateID=e.PlateID left join BaseEmployee f on e.EmployeeID=f.EmployeeID ";
                        strsql += " where e.PlateNo=@PlateNo and a.DataStatus!=2";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PlateNo", indata.PlateNumber);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            while (reader.Read())
                            {
                                list.Add(DataReaderToModel<CIMonthlyInfo>.ToModel(reader));
                            }
                        }
                        result.data = list;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("根据车牌号获取车辆在车场信息异常：GetMonthlyInfo()" + ex.Message + "\r\n");
                    result.result = "1008";

                }
            }
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 根据车牌号和车场获取车辆信息
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetMonthlyInfoPK(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";
            if (string.IsNullOrEmpty(jsondata))
            {
                return JsonHelper.GetJsonString(result);
            }
            CIGetMonthlyInfo indata = new CIGetMonthlyInfo();
            try
            {
                indata = JsonHelper.GetJson<CIGetMonthlyInfo>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }
            List<CIMonthlyInfo> list = new List<CIMonthlyInfo>();
            lock (objlock)
            {
                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select a.GID,a.PKID,b.PKName,c.CarTypeName,d.CarModelName as CarModel,e.PlateNo as PlateNumber,a.BeginDate,a.EndDate,a.PKLot,";
                        strsql += "a.State,f.EmployeeName,f.MobilePhone,f.FamilyAddr,a.CarTypeID,a.CarModelID,a.AreaIDS as AreaID ";
                        strsql += "from ParkGrant a left join BaseParkinfo b on a.PKID=b.PKID ";
                        strsql += "left join ParkCarType c on a.CarTypeID=c.CarTypeID left join ParkCarModel d on a.CarModelID=d.CarModelID ";
                        strsql += "left join EmployeePlate e on a.PlateID=e.PlateID left join BaseEmployee f on e.EmployeeID=f.EmployeeID ";
                        strsql += " where e.PlateNo=@PlateNo and a.PKID=@PKID and a.DataStatus!=2";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PlateNo", indata.PlateNumber);
                        dbOperator.AddParameter("PKID", indata.PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            while (reader.Read())
                            {
                                list.Add(DataReaderToModel<CIMonthlyInfo>.ToModel(reader));
                            }
                        }
                        result.result = "0";
                        result.data = list;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("根据车牌号和车场获取车辆信息异常：GetMonthlyInfoPK()" + ex.Message + "\r\n");
                    result.result = "1008";
                }
            }
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 根据车场查询车辆信息
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetMonthlyInfoByPK(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";
            if (string.IsNullOrEmpty(jsondata))
            {
                return JsonHelper.GetJsonString(result);
            }
            CIGetMonthlyInfoByPK indata = new CIGetMonthlyInfoByPK();
            try
            {
                indata = JsonHelper.GetJson<CIGetMonthlyInfoByPK>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }
            List<CIMonthlyInfo> list = new List<CIMonthlyInfo>();
            lock (objlock)
            {
                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        int Sumdata = 0;
                        string strsql = "select count(1) as Sumdata ";
                        strsql += "from ParkGrant a left join BaseParkinfo b on a.PKID=b.PKID ";
                        strsql += "left join ParkCarType c on a.CarTypeID=c.CarTypeID left join ParkCarModel d on a.CarModelID=d.CarModelID ";
                        strsql += "left join EmployeePlate e on a.PlateID=e.PlateID left join BaseEmployee f on e.EmployeeID=f.EmployeeID ";
                        strsql += " where  a.PKID=@PKID and a.DataStatus!=2";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PKID", indata.PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                Sumdata = int.Parse(reader["Sumdata"].ToString());
                            }
                        }
                        result.dataSum = Sumdata;
                        result.PageIndex = indata.PageIndex;
                        strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum,a.LastUpdateTime,
                        a.GID,a.PKID,b.PKName,c.CarTypeName,d.CarModelName as CarModel,e.PlateNo as PlateNumber,a.BeginDate,a.EndDate,a.PKLot, 
                        a.State,f.EmployeeName,f.MobilePhone,f.FamilyAddr 
                       from ParkGrant a left join BaseParkinfo b on a.PKID=b.PKID 
                        left join ParkCarType c on a.CarTypeID=c.CarTypeID left join ParkCarModel d on a.CarModelID=d.CarModelID 
                        left join EmployeePlate e on a.PlateID=e.PlateID left join BaseEmployee f on e.EmployeeID=f.EmployeeID 
                        where  a.PKID=@PKID and a.DataStatus!=2)
                        AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc", 50 * (indata.PageIndex - 1) + 1, 50 * indata.PageIndex);
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PKID", indata.PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            while (reader.Read())
                            {
                                list.Add(DataReaderToModel<CIMonthlyInfo>.ToModel(reader));
                            }
                        }
                        result.result = "0";
                        result.data = list;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("根据车场查询车辆信息异常：GetMonthlyInfoByPK()" + ex.Message + "\r\n");
                    result.result = "1008";
                }
            }
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 月卡开卡
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string AddMonthlyInfo(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";
            if (string.IsNullOrEmpty(jsondata))
            {
                return JsonHelper.GetJsonString(result);
            }
            CIMonthlyInfo indata = new CIMonthlyInfo();
            try
            {
                indata = JsonHelper.GetJson<CIMonthlyInfo>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }
            AddMonthlyInfoResult res = new AddMonthlyInfoResult();
            res.Res = -1;
            lock (objlock)
            {
                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select b.ProxyNo,a.VID from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKID=@PKID";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PKID", indata.PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                indata.VID = reader["VID"].ToString();
                                res = callback.CIAddMonthlyInfo(indata, reader["ProxyNo"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("月卡开卡异常：AddMonthlyInfo()" + ex.Message + "\r\n");
                    result.result = "1008";

                }
            }
            result.result = "0";
            result.data = res;
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 获取车场信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        public string GetParkinfo()
        {
            APIResult result = new APIResult();
            result.result = "1001";
            List<CIParkinfo> list = new List<CIParkinfo>();
            lock (objlock)
            {
                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select PKID,PKName,CarBitNum,CarBitNumLeft,CarBitNumFixed,SpaceBitNum,FeeRemark from BaseParkinfo where DataStatus!=2";
                        dbOperator.ClearParameters();

                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            while (reader.Read())
                            {
                                List<CIArea> listarea = new List<CIArea>();
                                CIParkinfo model = DataReaderToModel<CIParkinfo>.ToModel(reader);
                                using (DbOperator dbOperator2 = ConnectionManager.CreateReadConnection())
                                {
                                    string strsql2 = "select * from ParkArea where DataStatus!=2 and PKID=@PKID";
                                    dbOperator2.ClearParameters();
                                    dbOperator2.AddParameter("PKID", model.PKID);
                                    using (DbDataReader reader2 = dbOperator2.ExecuteReader(strsql2.ToString()))
                                    {
                                        while (reader2.Read())
                                        {
                                            CIArea area = new CIArea();
                                            area.AreaID = reader2["AreaID"].ToString();
                                            area.AreaName = reader2["AreaName"].ToString();
                                            model.CarBitNum += reader2["CarbitNum"].ToInt();
                                            listarea.Add(area);
                                        }
                                    }
                                }
                                model.Areas = listarea;

                                list.Add(model);
                            }
                        }
                        result.result = "0";
                        result.data = list;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("获取车场信息异常：GetParkinfo()" + ex.Message + "\r\n");
                    result.result = "1008";
                }
            }
            return JsonHelper.GetJsonString(result);
        }


        /// <summary>
        /// 根据车场ID获取车场信息
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetParkinfoByID(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";
            CIPKInfoByID indata = new CIPKInfoByID();
            try
            {
                indata = JsonHelper.GetJson<CIPKInfoByID>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }
            List<CIParkinfo> list = new List<CIParkinfo>();
            lock (objlock)
            {
                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select PKID,PKName,CarBitNum,CarBitNumLeft,CarBitNumFixed,SpaceBitNum,FeeRemark from BaseParkinfo where DataStatus!=2 and PKID=@PKID";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PKID", indata.PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            while (reader.Read())
                            {
                                CIParkinfo model = DataReaderToModel<CIParkinfo>.ToModel(reader);

                                using (DbOperator dbOperator2 = ConnectionManager.CreateReadConnection())
                                {
                                    string strsql2 = "select sum(CarbitNum) as bitnum from ParkArea where DataStatus!=2 and PKID=@PKID";
                                    dbOperator2.ClearParameters();
                                    dbOperator2.AddParameter("PKID", indata.PKID);
                                    using (DbDataReader reader2 = dbOperator2.ExecuteReader(strsql2.ToString()))
                                    {
                                        if (reader2.Read())
                                        {
                                            model.CarBitNum = reader2["bitnum"].ToString();
                                        }
                                    }

                                }

                                list.Add(model);
                            }
                        }
                        result.result = "0";
                        result.data = list;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("根据车场ID获取车场信息异常：GetParkinfoByID()" + ex.Message + "\r\n");
                    result.result = "1008";
                }
            }
            return JsonHelper.GetJsonString(result);
        }
        /// <summary>
        /// 获取车场车类信息
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetCarType(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";
            if (string.IsNullOrEmpty(jsondata))
            {
                return JsonHelper.GetJsonString(result);
            }
            ICGetCarType indata = new ICGetCarType();
            try
            {
                indata = JsonHelper.GetJson<ICGetCarType>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }
            List<CItCarType> list = new List<CItCarType>();
            lock (objlock)
            {
                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select CarTypeID,CarTypeName ,PKID ,BaseTypeID ,Amount from ParkCarType where PKID=@PKID and DataStatus!=2";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PKID", indata.PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            while (reader.Read())
                            {
                                list.Add(DataReaderToModel<CItCarType>.ToModel(reader));
                            }
                        }
                        result.result = "0";
                        result.data = list;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("获取车场车类信息异常：GetCarType()" + ex.Message + "\r\n");
                    result.result = "1008";
                }
            }
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 获取车场车型信息
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetCarModel(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";
            if (string.IsNullOrEmpty(jsondata))
            {
                return JsonHelper.GetJsonString(result);
            }
            ICGetCarType indata = new ICGetCarType();
            try
            {
                indata = JsonHelper.GetJson<ICGetCarType>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }
            List<ICCarModel> list = new List<ICCarModel>();
            lock (objlock)
            {
                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select CarModelID ,CarModelName  ,PKID   from ParkCarModel where PKID=@PKID and DataStatus!=2";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PKID", indata.PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            while (reader.Read())
                            {
                                list.Add(DataReaderToModel<ICCarModel>.ToModel(reader));
                            }
                        }
                        result.result = "0";
                        result.data = list;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("获取车场车型信息异常：GetCarModel()" + ex.Message + "\r\n");
                    result.result = "1008";
                }
            }
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 临停缴费
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CICalculatingTempCost(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";

            if (string.IsNullOrEmpty(jsondata))
            {
                return JsonHelper.GetJsonString(result);
            }
            CICalculatingTempCostAgt indata = new CICalculatingTempCostAgt();
            try
            {
                indata = JsonHelper.GetJson<CICalculatingTempCostAgt>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }

            lock (objlock)
            {
                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "";
                        strsql = " select a.RecordID,b.PKID,a.PlateNumber,a.EntranceTime,b.PKName,b.MobilePay,e.ProxyNo,b.CenterTime ";
                        strsql += " from ParkIORecord a left join BaseParkinfo b on a.ParkingID=b.PKID left join BaseVillage e on b.VID=e.VID  ";
                        strsql += " left join  BaseCompany f on e.CPID=f.CPID ";
                        strsql += " where a.PlateNumber=@PlateNumber and a.IsExit=0 ";
                        if (!string.IsNullOrEmpty(indata.CPID))
                        {
                            strsql += " and f.CPID=@CPID ";
                        }
                        strsql += "  ORDER BY a.EntranceTime DESC";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PlateNumber", indata.PlateNumber);
                        if (!string.IsNullOrEmpty(indata.CPID))
                        {
                            dbOperator.AddParameter("CPID", indata.CPID);
                        }
                        TempParkingFeeResult res = new TempParkingFeeResult();
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                DateTime CalculatDate = DateTime.ParseExact(indata.CalculatDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                                res = callback.TempParkingFee(reader["RecordID"].ToString(), reader["PKID"].ToString(), 6, CalculatDate, reader["ProxyNo"].ToString(), indata.AccountID, reader["CenterTime"].ToInt(), indata.PlateNumber);
                                result.result = "0";
                                CIPayAgt agt = new CIPayAgt();
                                agt.PlateNumber = res.PlateNumber;
                                agt.PKID = res.ParkingID;
                                agt.PKName = res.ParkName;
                                if (res.EntranceDate != null)
                                    agt.EntranceTime = res.EntranceDate.ToString("yyyyMMddHHmmss");
                                agt.OutTime = res.OutTime;
                                if (res.PayDate != null)
                                    agt.PayDate = res.PayDate.ToString("yyyyMMddHHmmss");

                                agt.Result = res.Result;
                                if (res.Pkorder != null)
                                {
                                    CIOrder order = new CIOrder();
                                    order.RecordID = res.Pkorder.RecordID;
                                    order.OrderNo = res.Pkorder.OrderNo;
                                    order.OrderType = (int)res.Pkorder.OrderType;
                                    order.PayWay = (int)res.Pkorder.PayWay;
                                    order.Amount = res.Pkorder.Amount;
                                    order.UnPayAmount = res.Pkorder.UnPayAmount;
                                    order.PayAmount = res.Pkorder.PayAmount;
                                    order.Status = (int)res.Pkorder.Status;
                                    if (res.Pkorder.OrderTime != null)
                                        order.OrderTime = res.Pkorder.OrderTime.ToString("yyyyMMddHHmmss");
                                    if (res.Pkorder.OldUserulDate != null)
                                        order.OldUserulDate = res.Pkorder.OldUserulDate.ToString("yyyyMMddHHmmss");
                                    if (res.Pkorder.NewUsefulDate != null)
                                        order.NewUsefulDate = res.Pkorder.NewUsefulDate.ToString("yyyyMMddHHmmss");
                                    order.OnlineUserID = res.Pkorder.OnlineUserID;
                                    order.OnlineOrderNo = res.Pkorder.OnlineOrderNo;
                                    agt.Pkorder = order;
                                }
                                result.data = agt;
                            }
                            else
                            {
                                result.result = "0";
                                res.Result = APPResult.NotFindIn;
                                CIPayAgt agt = new CIPayAgt();
                                agt.PlateNumber = res.PlateNumber;
                                agt.PKID = res.ParkingID;
                                agt.PKName = res.ParkName;
                                agt.Result = res.Result;
                                result.data = agt;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("获取临停费用异常：CICalculatingTempCost()" + ex.Message + "\r\n");
                    result.result = "1008";
                }
            }
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 扫码录入车牌支付
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CICalculatingTempCostPN(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";

            if (string.IsNullOrEmpty(jsondata))
            {
                return JsonHelper.GetJsonString(result);
            }
            CICalculatingTempCostPNAgt indata = new CICalculatingTempCostPNAgt();
            try
            {
                indata = JsonHelper.GetJson<CICalculatingTempCostPNAgt>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }
            lock (objlock)
            {
                try
                {
                    TempParkingFeeResult res = new TempParkingFeeResult();
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "";
                        strsql = " select a.RecordID,b.PKID,a.PlateNumber,a.EntranceTime,b.PKName,b.MobilePay,e.ProxyNo,b.CenterTime ";
                        strsql += " from ParkIORecord a left join BaseParkinfo b on a.ParkingID=b.PKID left join BaseVillage e on b.VID=e.VID  ";
                        strsql += " where a.PlateNumber=@PlateNumber and a.IsExit=0 ";
                        strsql += " and b.PKID=@PKID ";
                        strsql += "  ORDER BY a.EntranceTime DESC";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PlateNumber", indata.PlateNumber);
                        dbOperator.AddParameter("PKID", indata.PKID);

                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                DateTime CalculatDate = DateTime.ParseExact(indata.CalculatDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                                res = callback.TempParkingFee(reader["RecordID"].ToString(), reader["PKID"].ToString(), 6, CalculatDate, reader["ProxyNo"].ToString(), indata.AccountID, reader["CenterTime"].ToInt(), indata.PlateNumber);
                                result.result = "0";
                                CIPayAgt agt = new CIPayAgt();
                                agt.PlateNumber = res.PlateNumber;
                                agt.PKID = res.ParkingID;
                                agt.PKName = res.ParkName;
                                if (res.EntranceDate != null)
                                    agt.EntranceTime = res.EntranceDate.ToString("yyyyMMddHHmmss");
                                agt.OutTime = res.OutTime;
                                if (res.PayDate != null)
                                    agt.PayDate = res.PayDate.ToString("yyyyMMddHHmmss");

                                agt.Result = res.Result;
                                if (res.Pkorder != null)
                                {
                                    CIOrder order = new CIOrder();
                                    order.RecordID = res.Pkorder.RecordID;
                                    order.OrderNo = res.Pkorder.OrderNo;
                                    order.OrderType = (int)res.Pkorder.OrderType;
                                    order.PayWay = (int)res.Pkorder.PayWay;
                                    order.Amount = res.Pkorder.Amount;
                                    order.UnPayAmount = res.Pkorder.UnPayAmount;
                                    order.PayAmount = res.Pkorder.PayAmount;
                                    order.Status = (int)res.Pkorder.Status;
                                    if (res.Pkorder.OrderTime != null)
                                        order.OrderTime = res.Pkorder.OrderTime.ToString("yyyyMMddHHmmss");
                                    if (res.Pkorder.OldUserulDate != null)
                                        order.OldUserulDate = res.Pkorder.OldUserulDate.ToString("yyyyMMddHHmmss");
                                    if (res.Pkorder.NewUsefulDate != null)
                                        order.NewUsefulDate = res.Pkorder.NewUsefulDate.ToString("yyyyMMddHHmmss");
                                    order.OnlineUserID = res.Pkorder.OnlineUserID;
                                    order.OnlineOrderNo = res.Pkorder.OnlineOrderNo;
                                    agt.Pkorder = order;
                                }
                                result.data = agt;
                            }
                            else
                            {
                                result.result = "0";
                                res.Result = APPResult.NotFindIn;
                                CIPayAgt agt = new CIPayAgt();
                                agt.PlateNumber = res.PlateNumber;
                                agt.PKID = res.ParkingID;
                                agt.PKName = res.ParkName;
                                agt.Result = res.Result;
                                result.data = agt;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("获取临停费用异常：MonthlyRenewals()" + ex.Message + "\r\n");
                    result.result = "1008";
                }
            }
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 临停支付
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CITempPay(string jsondata)
        {
            APIResult result = new APIResult();
            result.result = "1001";

            if (string.IsNullOrEmpty(jsondata))
            {
                return JsonHelper.GetJsonString(result);
            }
            CITempPayAgt indata = new CITempPayAgt();
            try
            {
                indata = JsonHelper.GetJson<CITempPayAgt>(jsondata);
            }
            catch (Exception)
            {
                result.result = "1001";//格式错误
                return JsonHelper.GetJsonString(result);
            }
            lock (objlock)
            {
                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select b.ProxyNo from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKID=@PKID";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PKID", indata.PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                DateTime PayDate = DateTime.ParseExact(indata.PayDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                                TempStopPaymentResult res = callback.TempStopPayment(indata.OrderNO, indata.PayWay, indata.Amount, indata.PKID, reader["ProxyNo"].ToString(), indata.OnlineOrderID, PayDate);
                                result.result = "0";
                                CIPayAgt agt = new CIPayAgt();
                                agt.Result = res.Result;

                                agt.OnlineOrderID = res.OnlineOrderID;
                                if (res.PayDate != null)
                                    agt.PayDate = res.PayDate.ToString("yyyyMMddHHmmss");
                                result.data = agt;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("临时卡缴费异常：CITempPay()" + ex.Message + "\r\n");
                    result.result = "1008";
                }
            }
            return JsonHelper.GetJsonString(result);
        }

        #endregion

        #region 商户打折

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="SellerNo"></param>
        /// <param name="PWD"></param>
        /// <param name="SellerID"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIGetSellerInfo(string jsondata)
        {
            APIResult result = new APIResult();

            try
            {
                CIGetSellerInfo indata = JsonHelper.GetJson<CIGetSellerInfo>(jsondata);
                WXService service = new WXService();
                result.data = JsonHelper.GetJson<ParkSeller>(service.WXGetSellerInfo(indata.SellerNo, indata.PWD, indata.SellerID, indata.ProxyNo));
                result.result = "0";
                //string dddd=JsonHelper.GetJsonString(result);
                //logger.Fatal("获取商户信息异常：CIGetSellerInfo()" + dddd + ":" + jsondata + "\r\n");
                return JsonHelper.GetJsonString(result);
            }
            catch (Exception ex)
            {
                logger.Fatal("获取商户信息异常：CIGetSellerInfo()" + ex.Message + ":" + jsondata + "\r\n");
                result.result = "1001";
                return JsonHelper.GetJsonString(result);
            }

        }

        /// <summary>
        /// 修改商户密码
        /// </summary>
        /// <param name="SellerID"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIEditSellerPwd(string jsondata)
        {
            APIResult result = new APIResult();
            try
            {
                CIEditSellerPwd indata = JsonHelper.GetJson<CIEditSellerPwd>(jsondata);
                WXService service = new WXService();
                if (service.WXEditSellerPwd(indata.SellerID, indata.Pwd, indata.ProxyNo))
                {

                    result.result = "0";
                }
                else
                {
                    result.result = "1008";
                }
                return JsonHelper.GetJsonString(result);
            }
            catch (Exception)
            {
                result.result = "1001";
                return JsonHelper.GetJsonString(result);
            }

        }

        /// <summary>
        /// 获取进出记录
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIGetIORecordByPlateNumber(string jsondata)
        {
            APIResult result = new APIResult();

            try
            {
                CIGetIORecordByPlateNumber indata = JsonHelper.GetJson<CIGetIORecordByPlateNumber>(jsondata);
                WXService service = new WXService();
                List<CISeller_IORecord> list = new List<CISeller_IORecord>();
                List<ParkIORecord> list2 = JsonHelper.GetJson<List<ParkIORecord>>(service.WXGetIORecordByPlateNumber(indata.PlateNumber, indata.VID, indata.SellerID, indata.ProxyNo));
                foreach (var obj in list2)
                {
                    CISeller_IORecord model = new CISeller_IORecord();
                    model.RecordID = obj.RecordID;
                    model.PlateNumber = obj.PlateNumber;
                    model.EntranceTime = obj.EntranceTime.ToString("yyyyMMddHHmmss");
                    TimeSpan ts = DateTime.Now - obj.EntranceTime;

                    model.LongTime = (ts.Days * 24 + ts.Hours) + "时" + ts.Minutes + "分";
                    model.PKName = obj.PKName;
                    model.InimgData = obj.InimgData;
                    model.IsDiscount = obj.IsDiscount ? "1" : "0";
                    model.DiscountTime = obj.DiscountTime == null ? "" : obj.DiscountTime.ToString("yyyyMMddHHmmss");
                    list.Add(model);
                }
                result.data = list;
                result.result = "0";
                return JsonHelper.GetJsonString(result);
            }
            catch (Exception)
            {
                result.result = "1001";
                return JsonHelper.GetJsonString(result);
            }
        }

        /// <summary>
        /// 获取消费打折规则
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIGetParkDerate(string jsondata)
        {
            APIResult result = new APIResult();
            try
            {
                CIGetParkDerate indata = JsonHelper.GetJson<CIGetParkDerate>(jsondata);
                WXService service = new WXService();
                List<CISeller_Derate> list = new List<CISeller_Derate>();
                List<ParkDerate> list2 = JsonHelper.GetJson<List<ParkDerate>>(service.WXGetParkDerate(indata.sellerID, indata.VID, indata.ProxyNo));
                foreach (var obj in list2)
                {
                    CISeller_Derate model = new CISeller_Derate();
                    model.DerateID = obj.DerateID;
                    model.DerateType = obj.DerateType.ToString();
                    model.Name = obj.Name;
                    model.SellerID = obj.SellerID;
                    list.Add(model);
                }
                result.data = list;
                result.result = "0";
                return JsonHelper.GetJsonString(result);
            }
            catch (Exception)
            {
                result.result = "1001";
                return JsonHelper.GetJsonString(result);
            }
        }

        /// <summary>
        /// 开始打折
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIDiscountPlateNumber(string jsondata)
        {

            APIResult result = new APIResult();
            try
            {
                CIDiscountPlateNumber indata = JsonHelper.GetJson<CIDiscountPlateNumber>(jsondata);
                WXService service = new WXService();
                result.data = JsonHelper.GetJson<ConsumerDiscountResult>(service.WXDiscountPlateNumber(indata.IORecordID, indata.DerateID, indata.VID, indata.SellerID, decimal.Parse(indata.DerateMoney), indata.ProxyNo));
                result.result = "0";
                return JsonHelper.GetJsonString(result);
            }
            catch (Exception)
            {
                result.result = "1001";
                return JsonHelper.GetJsonString(result);
            }
        }
        #endregion

        #region 中兴

        /// <summary>
        /// 查询空闲车位
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIQueryParkInfo(string jsondata)
        {
            ZXResult res = new ZXResult();

            QueryParkInfoModel model = JsonHelper.GetJson<QueryParkInfoModel>(jsondata);
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "";//parkCode
                    strsql = "select b.ProxyNo,a.* from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKNo=@PKNo and a.DataStatus!=2";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKNo", model.parkCode);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            res.code = "0";
                            QueryParkInfoReuslt data = new QueryParkInfoReuslt();
                            data.parkCode = model.parkCode;
                            data.totalParkingSpace = reader["CarBitNum"].ToString();
                            data.freeParkingSpace = reader["SpaceBitNum"].ToString();
                            res.data = data;
                            res.message = "success";
                        }
                        else
                        {
                            res.code = "1003";
                            res.message = "找不到车场信息";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("查询空闲车位异常：CIQueryParkInfo()" + ex.Message + "\r\n");
                res.code = "1001";
                res.message = "失败";
            }

            return JsonHelper.GetJsonString(res);
        }

        /// <summary>
        /// 会员卡通知接口
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIVipCard(string jsondata)
        {
            ZXResult res = new ZXResult();
            VipCardModel model = JsonHelper.GetJson<VipCardModel>(jsondata);
            try
            {
                BaseParkinfo parkinfo = null;
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "";
                    strsql = " select * from BaseParkinfo where PKNo=@PKNo ";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKNo", model.parkCode);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            parkinfo = DataReaderToModel<BaseParkinfo>.ToModel(reader);
                        }
                    }
                }

                foreach (var obj in model.carNumbers)
                {
                    BaseEmployee modelEmp = new BaseEmployee();
                    modelEmp.EmployeeName = obj.carNumber;
                    modelEmp.RegTime = DateTime.Now;
                    modelEmp.MobilePhone = "";
                    modelEmp.VID = parkinfo.VID;
                    modelEmp.EmployeeType = EmployeeType.Owner;
                    modelEmp.EmployeeID = GuidGenerator.GetGuidString();
                    modelEmp.FamilyAddr = "";
                    modelEmp.Remark = "线上开卡";

                    EmployeePlate modelplate = new EmployeePlate();
                    modelplate.PlateNo = obj.carNumber;

                    string errorMsg = string.Empty;
                    EmployeePlate dbPlate = EmployeePlateServices.GetEmployeePlateNumberByPlateNumber(parkinfo.VID, obj.carNumber, out errorMsg);
                    if (!string.IsNullOrWhiteSpace(errorMsg))
                    {
                        res.code = "1001";
                        res.message = "失败";
                        return JsonHelper.GetJsonString(res);
                    }
                    if (dbPlate != null)
                    {
                        dbPlate.Color = PlateColor.Blue;
                        dbPlate.EmployeeID = modelEmp.EmployeeID;
                        modelplate = dbPlate;
                    }
                    else
                    {
                        modelplate.Color = PlateColor.Blue;
                        modelplate.PlateID = GuidGenerator.GetGuidString();
                        modelplate.EmployeeID = modelEmp.EmployeeID;
                    }

                    BaseCard card = new BaseCard();
                    card.CardID = GuidGenerator.GetGuidString();
                    card.CardNo = modelplate.PlateNo;
                    card.CardNumb = card.CardNo;
                    card.VID = parkinfo.VID;
                    BaseCard oldCard = BaseCardServices.QueryBaseCard(parkinfo.VID, card.CardNo);
                    if (oldCard != null)
                    {
                        logger.Fatal("开卡异常！车牌号已存在，不能重复添加\r\n");
                        res.code = "1101";
                        res.message = "车牌号已存在";
                        return JsonHelper.GetJsonString(res);
                    }
                    card.RegisterTime = DateTime.Now;
                    card.OperatorID = "线上开卡";
                    card.CardSystem = CardSystem.Park;
                    card.EmployeeID = modelEmp.EmployeeID;
                    card.CardType = CardType.Plate;

                    ParkGrant modelg = new ParkGrant();
                    modelg.PKID = parkinfo.PKID;
                    modelg.CardID = card.CardID;
                    modelg.PlateID = modelplate.PlateID;
                    modelg.PlateNo = modelplate.PlateNo;
                    modelg.PKLot = "";
                    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                    DateTime dt = startTime.AddMilliseconds(double.Parse(model.effDate));
                    modelg.BeginDate = dt;
                    startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                    dt = startTime.AddMilliseconds(double.Parse(model.expDate));
                    modelg.EndDate = dt;
                    List<ParkCarType> listcartype = ParkCarTypeServices.QueryParkCarTypeByParkingId(parkinfo.PKID);
                    if (listcartype.Count(a => a.BaseTypeID == BaseCarType.MonthlyRent) > 0)
                    {
                        modelg.CarTypeID = listcartype.First(a => a.BaseTypeID == BaseCarType.MonthlyRent).CarTypeID;
                    }

                    List<ParkCarModel> listcarmodel = ParkCarModelServices.QueryByParkingId(parkinfo.PKID);
                    if (listcarmodel.Count(a => a.IsDefault == YesOrNo.Yes) > 0)
                    {
                        modelg.CarModelID = listcarmodel.First(a => a.IsDefault == YesOrNo.Yes).CarModelID;
                    }

                    modelg.AreaIDS = "";
                    modelg.GateID = "";
                    bool ress = ParkGrantServices.Add(modelEmp, modelplate, card, modelg);
                    if (!ress)
                    {
                        logger.Fatal("开卡异常！保存失败\r\n");
                        res.code = "1001";
                        res.message = "失败";
                        return JsonHelper.GetJsonString(res);
                    }
                    else
                    {
                        res.code = "0";
                        res.message = "success";
                        return JsonHelper.GetJsonString(res);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("会员卡通知接口异常：CIVipCard()" + ex.Message + "\r\n");
                res.code = "1001";
                res.message = "失败";
            }
            return JsonHelper.GetJsonString(res);
        }

        /// <summary>
        /// 续费
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIVipCardXQ(string jsondata)
        {
            ZXResult res = new ZXResult();
            VipCardModel model = JsonHelper.GetJson<VipCardModel>(jsondata);
            try
            {
                BaseParkinfo parkinfo = null;
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "";
                    strsql = " select * from BaseParkinfo where PKNo=@PKNo ";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKNo", model.parkCode);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            parkinfo = DataReaderToModel<BaseParkinfo>.ToModel(reader);
                        }
                        else
                        {
                            res.code = "2001";
                            res.message = "找不到车场信息";
                            return JsonHelper.GetJsonString(res);
                        }
                    }
                }
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.BeginTransaction();
                    foreach (var obj in model.carNumbers)
                    {

                        ParkGrant grant = new ParkGrant();
                        string strsql = "select a.GID,b.PlateNo,a.BeginDate,a.EndDate from ParkGrant a left join EmployeePlate b on a.PlateID=b.PlateID where PlateNo=@PlateNo and PKID=@PKID and a.DataStatus<2";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PlateNo", obj.carNumber);
                        dbOperator.AddParameter("PKID", parkinfo.PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                grant.GID = reader["GID"].ToString();
                                grant.BeginDate = reader["BeginDate"].ToDateTime();
                                grant.EndDate = reader["EndDate"].ToDateTime();
                            }
                            else
                            {
                                res.code = "5008";
                                res.message = "找不到月卡信息";
                                return JsonHelper.GetJsonString(res);
                            }
                        }

                        DateTime OldUserulDate = DateTime.Now;
                        DateTime NewUsefulDate = DateTime.Now;
                        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                        DateTime dt = startTime.AddMilliseconds(double.Parse(model.effDate));
                        DateTime BeginDate = dt;
                        startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                        dt = startTime.AddMilliseconds(double.Parse(model.expDate));
                        NewUsefulDate = dt;
                        OldUserulDate = grant.EndDate;
                        strsql = "update ParkGrant set BeginDate=@BeginDate,EndDate=@EndDate,HaveUpdate=1,LastUpdateTime=@LastUpdateTime";
                        strsql += " where GID=@GID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("BeginDate", BeginDate);
                        dbOperator.AddParameter("EndDate", NewUsefulDate);
                        dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                        dbOperator.AddParameter("GID", grant.GID);
                        if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                        {
                            logger.Fatal(obj.carNumber + "月租卡缴费修改卡片有效期失败！CIVipCardXQ()\r\n");
                            dbOperator.RollbackTransaction();
                            res.code = "5004";
                            res.message = "其他异常";
                            return JsonHelper.GetJsonString(res);
                        }
                        DateTime PayDate = DateTime.Now;
                        ParkOrder order = new ParkOrder();
                        order.RecordID = System.Guid.NewGuid().ToString();
                        order.OrderNo = IdGenerator.Instance.GetId().ToString();
                        order.TagID = grant.GID;
                        order.OrderType = (OrderType)2;
                        order.PayWay = (OrderPayWay)2;
                        order.Amount = 0;
                        order.UnPayAmount = 0;
                        order.PayAmount = 0;
                        order.Status = 1;
                        order.OrderSource = (OrderSource)6;
                        order.OrderTime = PayDate;
                        order.Remark = "月卡缴费";
                        order.OldUserulDate = OldUserulDate;
                        order.NewUsefulDate = NewUsefulDate;
                        order.LastUpdateTime = PayDate;
                        order.HaveUpdate = 1;
                        order.PKID = parkinfo.PKID;
                        order.DataStatus = 0;
                        order.OnlineUserID = "";
                        order.OnlineOrderNo = "";
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
                            logger.Fatal(obj.carNumber + "月租卡缴费保存订单失败！MonthlyRenewalsCallback()\r\n");
                            dbOperator.RollbackTransaction();
                            res.code = "5004";
                            res.message = "其他异常";
                            return JsonHelper.GetJsonString(res);
                        }
                        res.code = "0";
                        res.message = "success";
                        return JsonHelper.GetJsonString(res);
                    }
                    dbOperator.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("会员卡续费通知接口异常：CIVipCardXQ()" + ex.Message + "\r\n");
                res.code = "1001";
                res.message = "失败";
            }
            return JsonHelper.GetJsonString(res);
        }
        /// <summary>
        /// 第二笔费用产生时间间隔
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CISendTime(string jsondata)
        {
            ZXResult res = new ZXResult();
            res.code = "1001";
            res.message = "失败";
            SendTimeModel model = JsonHelper.GetJson<SendTimeModel>(jsondata);
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "";
                    strsql = " update BaseParkinfo set CenterTime=@CenterTime where PKNo=@PKNo ";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKNo", model.parkCode);
                    dbOperator.AddParameter("CenterTime", int.Parse(model.duration) / 60);
                    if (dbOperator.ExecuteNonQuery(strsql) > 0)
                    {
                        res.code = "0";
                        res.message = "success";

                    }
                    else
                    {
                        res.code = "1001";
                        res.message = "失败";

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("第二笔费用产生时间间隔异常：CISendTime()" + ex.Message + "\r\n");
                res.code = "1001";
                res.message = "失败";
            }
            return JsonHelper.GetJsonString(res);
        }

        /// <summary>
        /// 查询停车费用
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIQueryFee(string jsondata)
        {
            ZXResult res = new ZXResult();
            res.code = "1001";
            res.message = "失败";
            QueryFee model = JsonHelper.GetJson<QueryFee>(jsondata);

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "";
                    strsql = " select a.PKID,a.PKName,e.ProxyNo,a.CenterTime from BaseParkinfo a left join BaseVillage e on a.VID=e.VID where PKNo=@PKNo ";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKNo", model.parkCode);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            WXServiceCallback callback = new WXServiceCallback();
                            DateTime jsdate = DateTime.Now;
                            TempParkingFeeResult tem = callback.TempParkingFee(model.recordId, reader["PKID"].ToString(), 2, jsdate, reader["ProxyNo"].ToString(), "", int.Parse(reader["CenterTime"].ToString()), model.carNumber);

                            switch (tem.Result)
                            {
                                case APPResult.Normal:
                                    QueryFeeResult resmodel = new QueryFeeResult();
                                    resmodel.parkCode = model.carNumber;
                                    resmodel.inTime = tem.EntranceDate.ToString();
                                    resmodel.outTtime = jsdate.ToString();
                                    TimeSpan ts = jsdate - tem.EntranceDate;
                                    resmodel.staytime = (ts.Days * 24 * 60 * 60 + ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds).ToString();
                                    Charge charg = new Charge();
                                    charg.due = (tem.Pkorder.Amount * 100).ToString();
                                    charg.paid = (tem.Pkorder.PayAmount * 100).ToString();
                                    charg.unpaid = (tem.Pkorder.UnPayAmount * 100).ToString();
                                    charg.duration = (ts.Days * 24 * 60 * 60 + ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds).ToString();
                                    resmodel.charge = charg;
                                    res.data = resmodel;
                                    res.code = "0";
                                    res.message = "success";
                                    break;
                                case APPResult.NotFindIn:
                                    res.code = "5001";
                                    res.message = "找不到入场记录";
                                    break;
                                case APPResult.NoNeedPay:
                                    res.code = "5002";
                                    res.message = "不需要缴费";
                                    break;
                                case APPResult.ProxyException:
                                    res.code = "5003";
                                    res.message = "代理网络异常";
                                    break;
                                case APPResult.OtherException:
                                    res.code = "5004";
                                    res.message = "其他异常";
                                    break;
                                case APPResult.NoTempCard:
                                    res.code = "5005";
                                    res.message = "非临时卡";
                                    break;
                                case APPResult.RepeatPay:
                                    res.code = "5006";
                                    res.message = "重复缴费";
                                    break;
                                default:
                                    res.code = "5007";
                                    res.message = "未知错误";
                                    break;
                            }


                        }
                        else
                        {
                            res.code = "2001";
                            res.message = "找不到车场信息";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("查询停车费用异常：CIQueryFee()" + ex.Message + "\r\n");
                res.code = "1001";
                res.message = "失败";
            }
            return JsonHelper.GetJsonString(res);
        }

        /// <summary>
        /// 支付完成通知
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string CIIssued(string jsondata)
        {
            IssuedResult res = new IssuedResult();
            res.code = "1001";
            res.message = "失败";
            IssuedModel model = JsonHelper.GetJson<IssuedModel>(jsondata);

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "";
                    strsql = " select a.PKID,a.PKName,e.ProxyNo,a.CenterTime from BaseParkinfo a left join BaseVillage e on a.VID=e.VID where PKNo=@PKNo ";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKNo", model.parkCode);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            WXServiceCallback callback = new WXServiceCallback();
                            string reslt = callback.IssuedZX(model.recordId, reader["PKID"].ToString(), model.carNumber, reader["ProxyNo"].ToString(), model.amout);
                            return reslt;
                        }
                        else
                        {
                            res.code = "2001";
                            res.message = "找不到车场信息";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("支付完成通知异常：CIIssued()" + ex.Message + "\r\n");
                res.code = "1001";
                res.message = "失败";
            }
            return JsonHelper.GetJsonString(res);
        }
        #endregion

        #region 济南数据上传
        /// <summary>
        /// 上传进出记录
        /// </summary>
        /// <param name="begintime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetParkIORecord(string begintime, string endtime, string pageindex)
        {
            List<JNParkIORecord> list = new List<JNParkIORecord>();

            try
            {
                int num = 0;
                int pagecount = 0;
                int PageIndex = int.Parse(pageindex);
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {

                    string strsql = "";//parkCode
                    strsql = "select count(1) as numcount ";
                    strsql += "from ParkEvent a left join ParkGate b on a.GateID=b.GateID ";
                    strsql += "left join BaseParkinfo c on  a.ParkingID=c.PKID ";
                    strsql += "left join ParkCarModel d on a.CarModelID=d.CarModelID ";
                    strsql += " where EventID=8 or EventID=10 and RecTime>='" + begintime + "' and RecTime<='" + endtime + "'";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            num = int.Parse(reader["numcount"].ToString());
                        }
                    }
                    pagecount = num / 50;
                    if (num % 50 > 0)
                    {
                        pagecount++;
                    }

                    strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.RecTime desc ) AS rownum,a.RecTime,a.IOState,a.PlateNumber,a.PlateColor,b.GateName,c.PKNO ,d.CarModelName
                       from ParkEvent a left join ParkGate b on a.GateID=b.GateID 
                        left join BaseParkinfo c on  a.ParkingID=c.PKID
                        left join ParkCarModel d on a.CarModelID=d.CarModelID
                        where  EventID=8 or EventID=10 and RecTime>=@begintime and RecTime<=@endtime)
                        AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by RecTime desc", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("begintime", begintime);
                    dbOperator.AddParameter("endtime", endtime);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        while (reader.Read())
                        {
                            JNParkIORecord model = new JNParkIORecord();
                            model.brand = "未知";
                            model.channel = reader["GateName"].ToString();
                            model.color = "未知";
                            model.dev = reader["PKNO"].ToString();
                            model.direction = reader["IOState"].ToString() == "1" ? "进" : "出";
                            model.pcolor = reader["PlateColor"].ToString();
                            model.plate = reader["PlateNumber"].ToString();
                            model.time = reader["RecTime"].ToString();
                            model.type = reader["CarModelName"].ToString();
                            model.pagecount = pagecount.ToString();
                            list.Add(model);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("济南上传进出记录异常：GetParkIORecord()" + ex.Message + "\r\n");

            }

            return JsonHelper.GetJsonString(list);
        }
        #endregion

        #region 宝宝停车
        /// <summary>
        /// 根据车牌号码获取缴费信息
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        [OperationContract]
        public string BBPKCreateorderbycarno(string postdata)
        {

            CreateOrderByCarNoRequstData reqstdata = new CreateOrderByCarNoRequstData();
            List<CreateOrderByCarNoDataItems> listagt = new List<CreateOrderByCarNoDataItems>();
            reqstdata.dataItems = listagt;
            reqstdata.serviceId = "";
            reqstdata.resultCode = 5001;
            reqstdata.message = "失败";
            if (string.IsNullOrEmpty(postdata))
            {
                reqstdata.resultCode = 5002;
                reqstdata.message = "传入参数格式失败";
                return JsonHelper.GetJsonString(reqstdata);
            }
            try
            {
                CreateOrderByCarNoPostData data = JsonHelper.GetJson<CreateOrderByCarNoPostData>(postdata);
                reqstdata.serviceId = data.serviceId;
                lock (objlock)
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = " select a.EntranceTime,a.RecordID,b.PKID,a.PlateNumber,a.EntranceTime,b.PKName,b.MobilePay,e.ProxyNo,b.CenterTime ";
                        strsql += " from ParkIORecord a left join BaseParkinfo b on a.ParkingID=b.PKID left join BaseVillage e on b.VID=e.VID  ";
                        strsql += " where a.PlateNumber=@PlateNumber and a.IsExit=0 ";
                        if (!string.IsNullOrEmpty(data.attributes.parkCode))
                        {
                            strsql += " and b.PKNo=@PKNo ";
                        }
                        strsql += "  ORDER BY a.EntranceTime DESC";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PlateNumber", data.attributes.carNo);
                        if (!string.IsNullOrEmpty(data.attributes.parkCode))
                        {
                            dbOperator.AddParameter("PKNo", data.attributes.parkCode);
                        }
                        CreateOrderByCarNoDataItems agt = new CreateOrderByCarNoDataItems();
                        agt.operateType = "READ";
                        CreateOrderByCarNoAttributesRes btres = new CreateOrderByCarNoAttributesRes();
                        TempParkingFeeResult res = new TempParkingFeeResult();
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                DateTime CalculatDate = DateTime.Now;
                                res = callback.TempParkingFee(reader["RecordID"].ToString(), reader["PKID"].ToString(), 6, CalculatDate, reader["ProxyNo"].ToString(), "", reader["CenterTime"].ToInt(), data.attributes.carNo);
                                if (res.Pkorder != null)
                                {
                                    switch (res.Result)
                                    {
                                        case APPResult.Normal:
                                            btres.retcode = 0;
                                            break;
                                        case APPResult.NotFindIn:
                                            btres.retcode = 2;
                                            break;
                                        case APPResult.NoTempCard:
                                            btres.retcode = 5;
                                            break;
                                        case APPResult.OtherException:
                                            btres.retcode = 6;
                                            break;
                                        case APPResult.NoNeedPay:
                                            btres.retcode = 9;
                                            break;
                                        default:
                                            btres.retcode = 9999;
                                            break;
                                    }

                                    btres.businesserCode = "";
                                    btres.businesserName = "";
                                    btres.businesserName = "";
                                    btres.parkCode = data.attributes.parkCode;
                                    btres.parkName = reader["PKName"].ToString();
                                    btres.orderNo = res.Pkorder.OrderNo;
                                    btres.goodName = "停车费";
                                    btres.cardNo = data.attributes.carNo;
                                    btres.carNo = data.attributes.carNo;
                                    btres.startTime = reader["EntranceTime"].ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                                    TimeSpan ts = CalculatDate - reader["EntranceTime"].ToDateTime();
                                    btres.serviceTime = ts.Days * 24 * 60 * 60 + ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds;
                                    btres.createTime = CalculatDate.ToString("yyyy-MM-dd HH:mm:ss");
                                    btres.endTime = CalculatDate.ToString("yyyy-MM-dd HH:mm:ss");
                                    btres.serviceFee = (double)res.Pkorder.Amount;
                                    btres.deductFee = 0;
                                    btres.discountFee = (double)res.Pkorder.DiscountAmount;
                                    btres.transportFee = 0;
                                    btres.otherFee = 0;
                                    btres.totalFee = (double)res.Pkorder.PayAmount;
                                    btres.tradeStatus = -1;
                                    btres.validTimeLen = 15000;
                                    btres.freeMinute = 0;
                                    btres.surplusMinute = 0;
                                    btres.retmsg = "";
                                }
                                agt.attributes = btres;
                                reqstdata.resultCode = 0;
                                listagt.Add(agt);
                            }
                            else
                            {
                                btres.retcode = 2;
                                agt.attributes = btres;
                                reqstdata.resultCode = 0;
                                listagt.Add(agt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                reqstdata.resultCode = 1;
                reqstdata.message = ex.Message;
            }
            return JsonHelper.GetJsonString(reqstdata);
        }

        /// <summary>
        /// 支付结果通知
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        [OperationContract]
        public string BBPKNotifyorderresult(string postdata)
        {
            NotifyorderresultRequstData reqstdata = new NotifyorderresultRequstData();
            List<NotifyorderresultDataItems> listagt = new List<NotifyorderresultDataItems>();
            reqstdata.dataItems = listagt;
            reqstdata.serviceId = "";
            reqstdata.resultCode = 1;
            reqstdata.message = "失败";
            if (string.IsNullOrEmpty(postdata))
            {
                reqstdata.resultCode = 5002;
                reqstdata.message = "传入参数格式失败";
                return JsonHelper.GetJsonString(reqstdata);
            }

            lock (objlock)
            {
                try
                {
                    NotifyorderresultPostData data = JsonHelper.GetJson<NotifyorderresultPostData>(postdata);
                    reqstdata.serviceId = data.serviceId;
                    if (data.attributes.tradeStatus == 1)
                    {
                        NotifyorderresultDataItems agt = new NotifyorderresultDataItems();
                        agt.operateType = "READ";
                        NotifyorderresultAttributesRes btres = new NotifyorderresultAttributesRes();
                        btres.retCode = 0;
                        btres.orderNo = data.attributes.orderNo;
                        listagt.Add(agt);
                        return JsonHelper.GetJsonString(reqstdata);
                    }
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select b.ProxyNo,a.PKID from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKNo=@PKNo";
                        dbOperator.ClearParameters();
                        // dbOperator.AddParameter("PKNo", indata.PKID);
                        NotifyorderresultDataItems agt = new NotifyorderresultDataItems();
                        agt.operateType = "READ";
                        NotifyorderresultAttributesRes btres = new NotifyorderresultAttributesRes();
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {

                                WXServiceCallback callback = new WXServiceCallback();
                                DateTime PayDate = DateTime.Now;
                                TempStopPaymentResult res = callback.TempStopPayment(data.attributes.orderNo, 9, 0, reader["PKID"].ToString(), reader["ProxyNo"].ToString(), "", PayDate);
                                switch (res.Result)
                                {
                                    case APPResult.Normal:
                                        btres.retCode = 0;
                                        break;
                                    default:
                                        btres.retCode = 1;
                                        break;
                                }
                                btres.orderNo = data.attributes.orderNo;
                                reqstdata.resultCode = 0;
                                agt.attributes = btres;
                            }
                            else
                            {
                                btres.retCode = 1;
                                reqstdata.message = "找不到车场信息";
                                btres.orderNo = data.attributes.orderNo;
                                reqstdata.resultCode = 0;
                                agt.attributes = btres;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal("临时卡缴费异常：BBPKNotifyorderresult()" + ex.Message + "\r\n");
                    reqstdata.resultCode = 1;
                    reqstdata.message = ex.Message;
                }
            }
            return JsonHelper.GetJsonString(reqstdata);

        }

        /// <summary>
        /// 订单结果查询
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        [OperationContract]
        public string BBPKQueryorder(string postdata)
        {
            QueryorderRequstData reqstdata = new QueryorderRequstData();
            List<QueryorderDataItems> listagt = new List<QueryorderDataItems>();
            reqstdata.dataItems = listagt;
            reqstdata.serviceId = "";
            reqstdata.resultCode = 1;
            reqstdata.message = "失败";
            if (string.IsNullOrEmpty(postdata))
            {
                reqstdata.resultCode = 5002;
                reqstdata.message = "传入参数格式失败";
                return JsonHelper.GetJsonString(reqstdata);
            }
            lock (objlock)
            {
                try
                {
                    QueryorderPostData data = JsonHelper.GetJson<QueryorderPostData>(postdata);
                    reqstdata.serviceId = data.serviceId;
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select * from ParkOrder a left join ParkIORecord b on a.TagID=b.RecordID ";
                        strsql += " left join BaseParkinfo c on a.PKID=c.PKID ";
                        strsql += " where a.OrderNo=@OrderNo ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("OrderNo", data.attributes.orderNo);
                        QueryorderDataItems agt = new QueryorderDataItems();
                        agt.operateType = "READ";
                        QueryorderAttributesRes btres = new QueryorderAttributesRes();
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                btres.orderNo = data.attributes.orderNo;
                                btres.businesserCode = "";
                                btres.businesserName = "";
                                btres.goodName = "停车费";
                                btres.parkCode = reader["PKNo"].ToString();
                                btres.parkName = reader["PKName"].ToString();
                                btres.cardNo = reader["PlateNumber"].ToString();
                                btres.carNo = reader["PlateNumber"].ToString();
                                btres.startTime = reader["EntranceTime"].ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                                if (reader["IsExit"].ToBoolean())
                                {
                                    TimeSpan ts = reader["ExitTime"].ToDateTime() - reader["EntranceTime"].ToDateTime();
                                    btres.serviceTime = ts.Days * 24 * 60 * 60 + ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds;
                                    btres.createTime = reader["OrderTime"].ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                                    btres.endTime = reader["ExitTime"].ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    TimeSpan ts = reader["OrderTime"].ToDateTime() - reader["EntranceTime"].ToDateTime();
                                    btres.serviceTime = ts.Days * 24 * 60 * 60 + ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds;
                                    btres.createTime = reader["OrderTime"].ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                                    btres.endTime = "";
                                }
                                btres.serviceFee = (double)reader["Amount"].ToDecimal();
                                btres.deductFee = 0;
                                btres.discountFee = (double)reader["DiscountAmount"].ToDecimal();//
                                btres.transportFee = 0;
                                btres.otherFee = 0;
                                btres.totalFee = (double)reader["PayAmount"].ToDecimal();
                                if (reader["Status"].ToInt() == 0 || reader["Status"].ToInt() == 2)
                                {
                                    btres.tradeStatus = -1;
                                }
                                else if (reader["Status"].ToInt() == 1)
                                {
                                    btres.tradeStatus = 0;
                                }
                                reqstdata.resultCode = 0;
                                reqstdata.message = "成功";
                                agt.attributes = btres;
                                listagt.Add(agt);
                            }
                            else
                            {
                                reqstdata.resultCode = 5001;
                                reqstdata.message = "找不到订单信息";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    reqstdata.resultCode = 1;
                    reqstdata.message = ex.Message;
                }
            }
            return JsonHelper.GetJsonString(reqstdata);
        }

        /// <summary>
        /// 停车场空车位数查询
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        [OperationContract]
        public string BBPKQueryparkspace(string postdata)
        {
            QueryparkspaceRequstData reqstdata = new QueryparkspaceRequstData();
            List<QueryparkspaceDataItems> listagt = new List<QueryparkspaceDataItems>();
            reqstdata.dataItems = listagt;
            reqstdata.serviceId = "";
            reqstdata.resultCode = 1;
            reqstdata.message = "失败";
            if (string.IsNullOrEmpty(postdata))
            {
                reqstdata.resultCode = 5002;
                reqstdata.message = "传入参数格式失败";
                return JsonHelper.GetJsonString(reqstdata);
            }
            lock (objlock)
            {
                try
                {
                    QueryparkspacePostData data = JsonHelper.GetJson<QueryparkspacePostData>(postdata);
                    reqstdata.serviceId = data.serviceId;
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select PKName,CarBitNum,SpaceBitNum from BaseParkinfo where PKNo in (@PKNo) ";
                        dbOperator.ClearParameters();
                        string[] strs = data.attributes.parkCodes.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        string strcode = "";
                        foreach (var obj in strs)
                        {
                            strcode += obj + ",";
                        }
                        strcode = strcode.Substring(0, strcode.Length - 1);

                        dbOperator.AddParameter("PKNo", strcode);

                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            while (reader.Read())
                            {
                                QueryparkspaceDataItems agt = new QueryparkspaceDataItems();
                                agt.operateType = "READ";
                                QueryparkspaceAttributesRes btres = new QueryparkspaceAttributesRes();
                                btres.parkCode = reader["PKNo"].ToString();
                                btres.parkName = reader["PKName"].ToString();
                                btres.totalSpace = reader["CarBitNum"].ToInt();
                                btres.restSpace = reader["SpaceBitNum"].ToInt();
                                reqstdata.resultCode = 0;
                                reqstdata.message = "成功";
                                agt.attributes = btres;
                                listagt.Add(agt);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    reqstdata.resultCode = 1;
                    reqstdata.message = ex.Message;
                }
            }
            return JsonHelper.GetJsonString(reqstdata);
        }

        /// <summary>
        /// 根据小区编号和车牌号查询人员卡信息
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        [OperationContract]
        public string BBPKQuerypersonsbycar(string postdata)
        {
            QuerypersonsbycarRequstData reqstdata = new QuerypersonsbycarRequstData();
            List<QuerypersonsbycarDataItems> listagt = new List<QuerypersonsbycarDataItems>();
            reqstdata.dataItems = listagt;
            reqstdata.serviceId = "";
            reqstdata.resultCode = 1;
            reqstdata.message = "失败";
            if (string.IsNullOrEmpty(postdata))
            {
                reqstdata.resultCode = 5002;
                reqstdata.message = "传入参数格式失败";
                return JsonHelper.GetJsonString(reqstdata);
            }
            lock (objlock)
            {
                try
                {
                    QuerypersonsbycarPostData data = JsonHelper.GetJson<QuerypersonsbycarPostData>(postdata);
                    reqstdata.serviceId = data.serviceId;
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = " select d.EmployeeName,d.CertifNo,d.sex,d.MobilePhone,d.Email,d.RegTime,a.EndDate,c.CardNo,c.CardID,e.CarTypeName ";
                        strsql += " from ParkGrant a left join BaseParkinfo b on a.PKID=b.PKID ";
                        strsql += "left join BaseCard c on a.CardID=c.CardID ";
                        strsql += "left join BaseEmployee d on c.EmployeeID=d.EmployeeID ";
                        strsql += "left join ParkCarType e on a.CarTypeID=e.CarTypeID ";
                        strsql += " where c.CardNo=@CardNo and b.PKNo=@PKNo";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("CardNo", data.attributes.areaCode);
                        dbOperator.AddParameter("PKNo", data.attributes.carNo);

                        QuerypersonsbycarDataItems agt = new QuerypersonsbycarDataItems();
                        agt.operateType = "READ";
                        agt.objectId = "PERSON";
                        QuerypersonsbycarAttributesRes btres = new QuerypersonsbycarAttributesRes();
                        List<QuerypersonsbycarSubItems> subagts = new List<QuerypersonsbycarSubItems>();
                        agt.subItems = subagts;
                        QuerypersonsbycarSubItems subitems = new QuerypersonsbycarSubItems();
                        subitems.objectId = "CARD";
                        subitems.operateType = "READ";
                        QuerypersonsbycarSubItemsAttributes subitme = new QuerypersonsbycarSubItemsAttributes();
                        subitems.attributes = subitme;
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                btres.personCode = "";
                                btres.personName = reader["EmployeeName"].ToString();
                                btres.identityCode = reader["CertifNo"].ToString();
                                btres.sex = "UN_KNOW";
                                btres.birthday = "";
                                btres.telephone = reader["MobilePhone"].ToString();
                                btres.email = reader["Email"].ToString();
                                btres.secretKey = "";
                                btres.areaId = "";

                                subitme.cardId = reader["CardID"].ToString();
                                subitme.physicalNo = "";
                                subitme.issueTime = reader["RegTime"].ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                                subitme.endTime = reader["EndDate"].ToDateTime().ToString("yyyy-MM-dd");
                                subitme.carNo = reader["CardNo"].ToString();
                                subitme.cardType = reader["CarTypeName"].ToString();
                                subagts.Add(subitems);

                                reqstdata.resultCode = 0;
                                reqstdata.message = "成功";
                                agt.attributes = btres;
                                listagt.Add(agt);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    reqstdata.resultCode = 1;
                    reqstdata.message = ex.Message;
                }
            }
            return JsonHelper.GetJsonString(reqstdata);
        }

        /// <summary>
        /// 锁车解锁
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        [OperationContract]
        public string BBPKlock(string postdata)
        {
            LockRequstData reqstdata = new LockRequstData();
            List<LockDataItems> listagt = new List<LockDataItems>();
            reqstdata.dataItems = listagt;
            reqstdata.serviceId = "";
            reqstdata.resultCode = 1;
            reqstdata.message = "失败";
            if (string.IsNullOrEmpty(postdata))
            {
                reqstdata.resultCode = 5002;
                reqstdata.message = "传入参数格式失败";
                return JsonHelper.GetJsonString(reqstdata);
            }
            lock (objlock)
            {
                try
                {
                    LockPostData data = JsonHelper.GetJson<LockPostData>(postdata);
                    reqstdata.serviceId = data.serviceId;
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        LockDataItems agt = new LockDataItems();
                        agt.operateType = "READ";
                        LockAttributesRes btres = new LockAttributesRes();
                        string strsql = "select b.ProxyNo,a.PKID from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKNo=@PKNo";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PKNo", data.attributes.parkCode);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                if (data.attributes.lockFlag == 0)//锁车
                                {
                                    WXServiceCallback callback = new WXServiceCallback();
                                    int res = callback.LockCarInfo("", reader["PKID"].ToString(), data.attributes.carNo, reader["ProxyNo"].ToString());
                                    if (res == 0)
                                    {
                                        btres.lockId = "";
                                        btres.lockFlag = "0";
                                        reqstdata.resultCode = 0;
                                        agt.attributes = btres;
                                        listagt.Add(agt);

                                    }
                                    else
                                    {
                                        btres.lockId = "";
                                        btres.lockFlag = "0";
                                        reqstdata.resultCode = 1;
                                        agt.attributes = btres;
                                        listagt.Add(agt);
                                    }
                                }
                                else if (data.attributes.lockFlag == 1)
                                {
                                    WXServiceCallback callback = new WXServiceCallback();
                                    int res = callback.UlockCarInfo("", reader["PKID"].ToString(), data.attributes.carNo, reader["ProxyNo"].ToString());
                                    if (res == 0)
                                    {
                                        btres.lockId = "";
                                        btres.lockFlag = "1";
                                        reqstdata.resultCode = 0;
                                        agt.attributes = btres;
                                        listagt.Add(agt);
                                    }
                                    else
                                    {
                                        btres.lockId = "";
                                        btres.lockFlag = "1";
                                        reqstdata.resultCode = 1;
                                        agt.attributes = btres;
                                        listagt.Add(agt);

                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    reqstdata.resultCode = 1;
                    reqstdata.message = ex.Message;
                }
            }
            return JsonHelper.GetJsonString(reqstdata);
        }

        /// <summary>
        /// 查询已锁定车辆
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        [OperationContract]
        public string BBPKQuerylockedcar(string postdata)
        {
            QuerylockedcarRequstData reqstdata = new QuerylockedcarRequstData();
            List<QuerylockedcarDataItems> listagt = new List<QuerylockedcarDataItems>();
            reqstdata.dataItems = listagt;
            reqstdata.serviceId = "";
            reqstdata.resultCode = 1;
            reqstdata.message = "失败";
            if (string.IsNullOrEmpty(postdata))
            {
                reqstdata.resultCode = 5002;
                reqstdata.message = "传入参数格式失败";
                return JsonHelper.GetJsonString(reqstdata);
            }
            lock (objlock)
            {
                try
                {
                    QuerylockedcarPostData data = JsonHelper.GetJson<QuerylockedcarPostData>(postdata);
                    reqstdata.serviceId = data.serviceId;
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string strsql = "select a.* from WX_LockCar a left join BaseParkinfo b on a.PKID=b.PKID  ";
                        strsql += "where a.PlateNumber=@PlateNumber and b.PKNo=@PKNo and Status=1";
                        dbOperator.ClearParameters();


                        dbOperator.AddParameter("PlateNumber", data.attributes.parkCode);
                        dbOperator.AddParameter("PKNo", data.attributes.carNo);
                        QuerylockedcarDataItems agt = new QuerylockedcarDataItems();
                        agt.operateType = "READ";
                        QuerylockedcarAttributesRes btres = new QuerylockedcarAttributesRes();
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                btres.parkCode = reader["PKNo"].ToString();
                                btres.lockId = "";
                                btres.carNo = reader["PlateNumber"].ToString();
                                btres.lockTime = reader["LockDate"].ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                                btres.featureCode = "";

                                reqstdata.resultCode = 0;
                                reqstdata.message = "成功";
                                agt.attributes = btres;
                                listagt.Add(agt);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    reqstdata.resultCode = 1;
                    reqstdata.message = ex.Message;
                }
            }
            return JsonHelper.GetJsonString(reqstdata);
        }

        /// <summary>
        /// 月卡延期（按卡号）
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        [OperationContract]
        public string BBPKCarddelay(string postdata)
        {
            CarddelayRequstData reqstdata = new CarddelayRequstData();
            List<CarddelayDataItems> listagt = new List<CarddelayDataItems>();
            reqstdata.dataItems = listagt;
            reqstdata.serviceId = "";
            reqstdata.resultCode = 1;
            reqstdata.message = "失败";
            if (string.IsNullOrEmpty(postdata))
            {
                reqstdata.resultCode = 5002;
                reqstdata.message = "传入参数格式失败";
                return JsonHelper.GetJsonString(reqstdata);
            }

            lock (objlock)
            {
                try
                {
                    CarddelayPostData data = JsonHelper.GetJson<CarddelayPostData>(postdata);
                    reqstdata.serviceId = data.serviceId;
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        CarddelayDataItems agt = new CarddelayDataItems();
                        agt.operateType = "READ";
                        CarddelayAttributesRes btres = new CarddelayAttributesRes();
                        string strsql = "select b.ProxyNo,a.PKID from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKNO=@PKNO";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PKNO", data.attributes.parkCode);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                DateTime paydate = DateTime.Now;
                                MonthlyRenewalResult res = callback.MonthlyRenewals(data.attributes.cardId, reader["PKID"].ToString(), data.attributes.month, (decimal)data.attributes.money, "", 9, 6, "", paydate, reader["ProxyNo"].ToString());
                                if (res.Result == APPResult.Normal)
                                {
                                    btres.cardId = data.attributes.cardId;
                                    reqstdata.resultCode = 0;
                                    reqstdata.message = "成功";
                                    agt.attributes = btres;
                                    listagt.Add(agt);
                                }
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    reqstdata.resultCode = 1;
                    reqstdata.message = ex.Message;
                }
            }
            return JsonHelper.GetJsonString(reqstdata);
        }

        /// <summary>
        /// 月卡缴费明细查询
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        [OperationContract]
        public string BBPKquerycarddealylist(string postdata)
        {
            QuerycarddealylistRequstData reqstdata = new QuerycarddealylistRequstData();
            List<QuerycarddealylistDataItems> listagt = new List<QuerycarddealylistDataItems>();
            reqstdata.dataItems = listagt;
            reqstdata.serviceId = "";
            reqstdata.resultCode = 1;
            reqstdata.message = "失败";
            if (string.IsNullOrEmpty(postdata))
            {
                reqstdata.resultCode = 5002;
                reqstdata.message = "传入参数格式失败";
                return JsonHelper.GetJsonString(reqstdata);
            }
            lock (objlock)
            {
                try
                {
                    QuerycarddealylistPostData data = JsonHelper.GetJson<QuerycarddealylistPostData>(postdata);
                    reqstdata.serviceId = data.serviceId;
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {

                        string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY a.ordertime desc ) AS rownum, c.RegisterTime,c.CardID,c.CardNo,
                                                    a.OrderTime,b.EndDate from ParkOrder a left join ParkGrant b on a.TagID=b.GID 
                                                    left join BaseCard c on b.CardID=c.CardID left join BaseEmployee d on c.EmployeeID=d.EmployeeID
                                                    where c.CardID=@CardID and Status=1 and  a.OrderTime>@beginDate and a.OrderTime<@endDate                                      
                                                    ", data.attributes.pageSize.ToInt() * data.attributes.pageIndex.ToInt());

                        strSql += string.Format(" order by a.ordertime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by ordertime desc", data.attributes.pageSize.ToInt() * (data.attributes.pageIndex.ToInt() - 1) + 1, data.attributes.pageSize.ToInt() * data.attributes.pageIndex.ToInt());
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("CardID", data.attributes.cardId);
                        dbOperator.AddParameter("beginDate", data.attributes.beginDate.ToDateTime());
                        dbOperator.AddParameter("OrderTime", data.attributes.endDate.ToDateTime());
                        using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                        {
                            while (reader.Read())
                            {
                                QuerycarddealylistDataItems agt = new QuerycarddealylistDataItems();
                                agt.operateType = "READ";
                                QuerycarddealylistAttributesRes btres = new QuerycarddealylistAttributesRes();
                                btres.cardId = reader["CardID"].ToString();
                                btres.physicalNo = "";
                                btres.payTime = reader["OrderTime"].ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                                btres.issueTime = reader["RegisterTime"].ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                                btres.money = reader["PayAmount"].ToFloat();
                                btres.endDate = reader["BeginDate"].ToDateTime().ToString("yyyy-MM-dd");
                                btres.beginDate = reader["EndDate"].ToDateTime().ToString("yyyy-MM-dd");
                                btres.carNo = reader["CardNo"].ToString();

                                reqstdata.resultCode = 0;
                                reqstdata.message = "成功";
                                agt.attributes = btres;
                                listagt.Add(agt);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    reqstdata.resultCode = 1;
                    reqstdata.message = ex.Message;
                }
            }
            return JsonHelper.GetJsonString(reqstdata);
        }

        /// <summary>
        /// 预定车位
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        [OperationContract]
        public string BBPKBookparkspace(string postdata)
        {
            BookparkspaceRequstData reqstdata = new BookparkspaceRequstData();
            List<BookparkspaceDataItems> listagt = new List<BookparkspaceDataItems>();
            reqstdata.dataItems = listagt;
            reqstdata.serviceId = "";
            reqstdata.resultCode = 1;
            reqstdata.message = "失败";
            if (string.IsNullOrEmpty(postdata))
            {
                reqstdata.resultCode = 5002;
                reqstdata.message = "传入参数格式失败";
                return JsonHelper.GetJsonString(reqstdata);
            }

            //lock (objlock)
            //{
            //    try
            //    {
            //        BookparkspacePostData data = JsonHelper.GetJson<BookparkspacePostData>(postdata);
            //        reqstdata.serviceId = data.serviceId;
            //        using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            //        {
            //            BookparkspaceDataItems agt = new BookparkspaceDataItems();
            //            agt.operateType = "READ";
            //            BookparkspaceAttributesRes btres = new BookparkspaceAttributesRes();
            //            string strsql = "select b.ProxyNo,a.PKID from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKNO=@PKNO";
            //            dbOperator.ClearParameters();
            //            dbOperator.AddParameter("PKNO", data.attributes.parkCode);
            //            using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
            //            {
            //                if (reader.Read())
            //                {
            //                    WXServiceCallback callback = new WXServiceCallback();
            //                    DateTime paydate = DateTime.Now;
            //                    string res = callback.ReservePKBit(reader["ProxyNo"].ToString(),"","", reader["PKID"].ToString(),"", data.attributes.carNo,DateTime.Now,DateTime.Now.AddDays(1));
            //                    WXReserveBitResult resmodel=JsonHelper.GetJson<WXReserveBitResult>(res);
            //                    if (res.code == 0)
            //                    {
            //                        btres.cardId = data.attributes.cardId;
            //                        reqstdata.resultCode = 0;
            //                        reqstdata.message = "成功";
            //                        agt.attributes = btres;
            //                        listagt.Add(agt);
            //                    }
            //                }
            //            }
            //        }


            //    }
            //    catch (Exception ex)
            //    {
            //        reqstdata.resultCode = 1;
            //        reqstdata.message = ex.Message;
            //    }
            //}
            return JsonHelper.GetJsonString(reqstdata);
        }

        /// <summary>
        /// 车辆出场信息查询
        /// </summary>
        /// <param name="postdata"></param>
        /// <returns></returns>
        [OperationContract]
        public string BBPKQueryparkout(string postdata)
        {
            QueryparkoutRequstData reqstdata = new QueryparkoutRequstData();
            List<QueryparkoutDataItems> listagt = new List<QueryparkoutDataItems>();
            reqstdata.dataItems = listagt;
            reqstdata.serviceId = "";
            reqstdata.resultCode = 1;
            reqstdata.message = "失败";
            if (string.IsNullOrEmpty(postdata))
            {
                reqstdata.resultCode = 5002;
                reqstdata.message = "传入参数格式失败";
                return JsonHelper.GetJsonString(reqstdata);
            }
            lock (objlock)
            {
                try
                {
                    QueryparkoutPostData data = JsonHelper.GetJson<QueryparkoutPostData>(postdata);
                    reqstdata.serviceId = data.serviceId;
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {

                        string strSql = string.Format(@"SELECT * FROM (SELECT top {0} ROW_NUMBER() OVER ( ORDER BY a.ExitTime desc ) AS rownum, 
                                                    b.PKNo,b.PKName,a.CardNo,d.CarTypeName,a.PlateNumber,a.ExitTime,e.GateName as exitgate, 
                                                   f.UserName,c.Amount,c.DiscountAmount,c.PayAmount,a.EntranceTime,g.GateName as ingate  
                                                    from ParkIORecord a left join BaseParkinfo b on a.ParkingID=b.PKID  
                                                    left join ParkOrder c on a.RecordID=c.TagID left join ParkCarType d on a.CarTypeID=d.CarTypeID 
                                                    left join ParkGate e on a.ExitGateID=e.GateID left join SysUser f on a.ExitOperatorID=f.RecordID 
                                                    left join ParkGate g on a.EntranceGateID=g.GateID
                                                    where a.PlateNumber=@PlateNumber and a.IsExit=1 and b.PKNo=@PKNo and  a.ExitTime>@beginDate and  a.ExitTime<@endDate                                    
                                                    ", data.attributes.pageSize.ToInt() * data.attributes.pageIndex.ToInt());

                        strSql += string.Format(" order by a.ExitTime desc) AS temp WHERE temp.rownum BETWEEN {0} AND {1} order by ordertime desc", data.attributes.pageSize.ToInt() * (data.attributes.pageIndex.ToInt() - 1) + 1, data.attributes.pageSize.ToInt() * data.attributes.pageIndex.ToInt());
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PlateNumber", data.attributes.carNo);
                        dbOperator.AddParameter("PKNo", data.attributes.parkCode);
                        dbOperator.AddParameter("beginDate", data.attributes.beginDate.ToDateTime());
                        dbOperator.AddParameter("endDate", data.attributes.endDate.ToDateTime());

                        using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                        {
                            while (reader.Read())
                            {
                                QueryparkoutDataItems agt = new QueryparkoutDataItems();
                                agt.operateType = "READ";
                                QueryparkoutAttributesRes btres = new QueryparkoutAttributesRes();
                                btres.parkCode = reader["PKNo"].ToString();
                                btres.parkName = reader["PKName"].ToString();
                                btres.cardNo = reader["CardNo"].ToString();
                                btres.cardType = reader["CarTypeName"].ToString();
                                btres.carNo = reader["PlateNumber"].ToString();
                                btres.outTime = reader["ExitTime"].ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                                btres.outEventType = "";
                                btres.outEquip = reader["exitgate"].ToString();
                                btres.outOperator = reader["UserName"].ToString();
                                btres.payTypeName = "";
                                btres.ysMoney = reader["Amount"].ToFloat();
                                btres.yhMoney = reader["DiscountAmount"].ToFloat();
                                btres.hgMoney = 0;
                                btres.ssMoney = reader["PayAmount"].ToFloat();
                                btres.inTime = reader["EntranceTime"].ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                                btres.inEquip = reader["ingate"].ToString();
                                TimeSpan ts = reader["ExitTime"].ToDateTime() - reader["EntranceTime"].ToDateTime();
                                btres.parkingTime = ts.Days * 24 * 60 * 60 + ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds;
                                reqstdata.resultCode = 0;
                                reqstdata.message = "成功";
                                agt.attributes = btres;
                                listagt.Add(agt);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    reqstdata.resultCode = 1;
                    reqstdata.message = ex.Message;
                }
            }
            return JsonHelper.GetJsonString(reqstdata);
        }


        #endregion
    }
}
using Common.Core;
using Common.DataAccess;
using Common.Entities;
using Common.Entities.BaseData;
using Common.Entities.Parking;
using Common.Entities.WX;
using Common.Utilities;
using Common.Utilities.Helpers;
using PlatformWcfServers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;

namespace PlatformServers
{
    public class PlatformDownloadData
    {
        private static int PageNum = 1;
        private static bool IsStartDownload = true;
        
        private static Logger logger = LogManager.GetLogger("数据下发");

        /// <summary>
        /// 开始上传数据
        /// </summary>
        public static void StartDownload()
        {
            ThreadPool.QueueUserWorkItem(_ => {
                IsStartDownload = true;

                DownloadData();
            });
        }

        /// <summary>
        /// 下发数据
        /// </summary>
        public static void DownloadData()
        {
            while (IsStartDownload)
            {
                //BaseCompanyDownload();
                BaseVillageDownload();
                //SysRolesDownload();
                //SysUserDownload();
                //SysUserRolesMappingDownload();
                //SysScopeDownload();
                //SysScopeAuthorizeDownload();
                //SysUserScopeMappingDownload();
                //SysRoleAuthorizeDowmload();
                List<BaseVillage> listVillage = new List<BaseVillage>();
                try
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = "select * from BaseVillage";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            while (reader.Read())
                            {
                                listVillage.Add(new BaseVillage() {
                                    VID = reader["VID"].ToString(),
                                    ProxyNo = reader["ProxyNo"].ToString()
                                });
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    logger.Fatal("下发数据，获取小区信息失败\r\n", ex);
                }

                foreach (var v in listVillage)
                {
                    if(!WXServiceCallback.IsOnline(v.ProxyNo))
                    {
                        continue;
                    }

                    BaseParkinfoDownload(v);
                    BasePassRemarkDownload(v);
                    ParkAreaDownload(v);
                    ParkBoxDownload(v);
                    ParkGateDownload(v);
                    ParkDeviceDownload(v);
                    ParkCarTypeDownload(v);
                    ParkCarModelDownload(v);
                    ParkFeeRuleDownload(v);
                    ParkFeeRuleDetailDownload(v);
                    BaseCardDownload(v);
                    ParkGrantDownload(v);
                    ParkCardSuspendPlanDownload(v);
                    BaseEmployeeDownload(v);
                    EmployeePlateDownload(v);
                    ParkSellerDownload(v);
                    ParkDerateDownload(v);
                    ParkDerateIntervarDownload(v);
                    ParkCarDerateDownload(v);
                    ParkBlacklistDownload(v);
                    ParkOrderDownload(v);
                    ParkReserveBitDownload(v);
                    // ParkParkCarBitGroupDownload(obj);
                    // ParkDeviceDetectionDownload(obj);
                }
                Thread.Sleep(5 * 1000);
            }
        }
        
        /// <summary>
        /// 下发小区信息
        /// </summary>
        public static void BaseVillageDownload()
        {
            try
            {
                List<BaseVillage> list = new List<BaseVillage>();

                foreach (string key in new List<string>(ParkDownloadDataServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = @"select top " + PageNum + " * from BaseVillage where HaveUpdate=3 and ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                        {
                            while (reader.Read())
                            {
                                list.Add(DataReaderToModel<BaseVillage>.ToModel(reader));
                            }
                        }
                    }
                }

                foreach (var obj in list)
                {
                    bool result = new ParkDownloadDataService().DownloadVillage(obj.ProxyNo, JsonHelper.GetJsonString(new List<BaseVillage>() { obj }));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载小区信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载车场信息
        /// </summary>
        public static void BaseParkinfoDownload(BaseVillage village)
        {
            try
            {
                List<BaseParkinfo> list = new List<BaseParkinfo>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " * from BaseParkinfo where HaveUpdate=3 and VID='" + village.VID + "'";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<BaseParkinfo>.ToModel(reader));
                        }
                    }
                }

                if (list.Any())
                {
                    var json = JsonHelper.GetJsonString(list);

                    bool result = new ParkDownloadDataService().DownloadParkinfo(village.ProxyNo, json);

                    logger.Info($"{village.VName} 已下发车场信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载车场信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载车场放行描述
        /// </summary>
        /// <param name="village"></param>
        public static void BasePassRemarkDownload(BaseVillage village)
        {
            try
            {
                List<BasePassRemark> list = new List<BasePassRemark>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from BasePassRemark a left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<BasePassRemark>.ToModel(reader));
                        }
                    }
                }

                if (list.Any())
                {
                    var json = JsonHelper.GetJsonString(list);

                    bool result = new ParkDownloadDataService().DownloadPassremark(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发车场放行描述信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载车场放行描述信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载车场区域信息
        /// </summary>
        /// <param name="village"></param>
        public static void ParkAreaDownload(BaseVillage village)
        {
            try
            {
                List<ParkArea> list = new List<ParkArea>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from ParkArea a left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkArea>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkArea(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发车场区域信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载车场区域信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载车场岗亭信息
        /// </summary>
        /// <param name="village"></param>
        public static void ParkBoxDownload(BaseVillage village)
        {
            try
            {
                List<ParkBox> list = new List<ParkBox>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from ParkBox a left join  ParkArea b on a.AreaID=b.AreaID left join BaseParkinfo c on b.PKID=c.PKID where c.VID='" + village.VID + "' and a.HaveUpdate=3";;
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkBox>.ToModel(reader));
                        }
                    }
                }

                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkBox(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发下载车场岗亭信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载车场岗亭信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载车场通道信息
        /// </summary>
        /// <param name="village"></param>
        public static void ParkGateDownload(BaseVillage village)
        {
            try
            {
                List<ParkGate> list = new List<ParkGate>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from ParkGate a left join  ParkBox b on a.BoxID=b.BoxID left join  ParkArea c on b.AreaID=c.AreaID left join BaseParkinfo d on c.PKID=d.PKID where d.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkGate>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadPKGate(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下载车场通道信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载车场通道信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载车场设备信息
        /// </summary>
        /// <param name="village"></param>
        public static void ParkDeviceDownload(BaseVillage village)
        {
            try
            {
                List<ParkDevice> list = new List<ParkDevice>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from ParkDevice a left join ParkGate b on a.GateID=b.GateID left join  ParkBox c on b.BoxID=c.BoxID left join  ParkArea d on c.AreaID=d.AreaID left join BaseParkinfo e on d.PKID=e.PKID where e.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkDevice>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkDevice(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发车场设备信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载车场设备信息异常\r\n", ex);
            }
        }

        public static void ParkDeviceParamDownload(BaseVillage village)
        {
            try
            {
                List<ParkDeviceParam> list = new List<ParkDeviceParam>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from ParkDeviceParam a left join  ParkDevice g on a.DeviceID=g.DeviceID left join ParkGate b on g.GateID=b.GateID left join  ParkBox c on b.BoxID=c.BoxID left join  ParkArea d on c.AreaID=d.AreaID left join BaseParkinfo e on d.PKID=e.PKID where e.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkDeviceParam>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().ParkDeviceParamDownload(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发车场设备参数信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 已下发车场设备参数异常\r\n", ex);
            }
        }
        
        /// <summary>
        /// 下载车场设备连接
        /// </summary>
        /// <param name="village"></param>
        public static void ParkDeviceDetectionDownload(BaseVillage village)
        {
            try
            {
                List<ParkDevice> list = new List<ParkDevice>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from ParkDeviceDetection a left join  ParkDevice b on a.DeviceID=b.DeviceID left join ParkGate c on b.GateID=c.GateID left join  ParkBox d on c.BoxID=d.BoxID left join  ParkArea e on d.AreaID=e.AreaID left join BaseParkinfo f on e.PKID=f.PKID where f.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkDevice>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkDevice(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发车场设备连接信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载车场设备信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载车类信息
        /// </summary>
        /// <param name="village"></param>
        public static void ParkCarTypeDownload(BaseVillage village)
        {
            try
            {
                List<ParkCarType> list = new List<ParkCarType>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from ParkCarType a left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkCarType>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkCarType(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发车类信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载车类信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载车型信息
        /// </summary>
        /// <param name="village"></param>
        public static void ParkCarModelDownload(BaseVillage village)
        {
            try
            {
                List<ParkCarModel> list = new List<ParkCarModel>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from ParkCarModel a left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkCarModel>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkCarModel(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发车型信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载车型信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载收费规则信息
        /// </summary>
        /// <param name="village"></param>
        public static void ParkFeeRuleDownload(BaseVillage village)
        {
            try
            {
                List<ParkFeeRule> list = new List<ParkFeeRule>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from ParkFeeRule a left join ParkArea b on a.AreaID=b.AreaID left join  BaseParkinfo c on b.PKID=c.PKID where c.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkFeeRule>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkFeeRule(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发收费规则信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载收费规则信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载收费规则明细信息
        /// </summary>
        /// <param name="village"></param>
        public static void ParkFeeRuleDetailDownload(BaseVillage village)
        {
            try
            {
                List<ParkFeeRuleDetail> list = new List<ParkFeeRuleDetail>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from ParkFeeRuleDetail a left join  ParkFeeRule b on a.RuleID=b.FeeRuleID  left join ParkArea c on b.AreaID=c.AreaID left join  BaseParkinfo d on c.PKID=d.PKID where d.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkFeeRuleDetail>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkFeeRuleDetail(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发收费规则明细信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载收费规则明细信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载小区卡片信息
        /// </summary>
        /// <param name="village"></param>
        public static void BaseCardDownload(BaseVillage village)
        {
            try
            {
                List<BaseCard> list = new List<BaseCard>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " * from BaseCard where VID='" + village.VID + "' and HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<BaseCard>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadBaseCard(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发小区卡片信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载小区卡片信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载车场授权信息
        /// </summary>
        /// <param name="village"></param>
        public static void ParkGrantDownload(BaseVillage village)
        {
            try
            {
                List<ParkGrant> list = new List<ParkGrant>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from ParkGrant a left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkGrant(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发车场授权信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载车场授权信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载车场授权暂停信息
        /// </summary>
        /// <param name="village"></param>
        public static void ParkCardSuspendPlanDownload(BaseVillage village)
        {
            try
            {
                List<ParkCardSuspendPlan> list = new List<ParkCardSuspendPlan>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from  ParkCardSuspendPlan a left join  ParkGrant b on a.GrantID=b.GID left join BaseParkinfo c on b.PKID=c.PKID where c.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkCardSuspendPlan>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkCardSuspendPlan(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发车场授权暂停信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载车场授权暂停信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载业主信息
        /// </summary>
        /// <param name="village"></param>
        public static void BaseEmployeeDownload(BaseVillage village)
        {
            try
            {
                List<BaseEmployee> list = new List<BaseEmployee>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " * from BaseEmployee where VID='" + village.VID + "' and HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<BaseEmployee>.ToModel(reader));
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadBaseEmployee(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发业主信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载业主信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载业主车牌信息
        /// </summary>
        /// <param name="village"></param>
        public static void EmployeePlateDownload(BaseVillage village)
        {
            try
            {
                List<EmployeePlate> list = new List<EmployeePlate>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from EmployeePlate a left join  BaseEmployee b on a.EmployeeID=b.EmployeeID where b.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<EmployeePlate>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadEmployeePlate(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发业主车牌信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载业主车牌信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载商家信息
        /// </summary>
        /// <param name="village"></param>
        public static void ParkSellerDownload(BaseVillage village)
        {
            try
            {
                List<ParkSeller> list = new List<ParkSeller>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " * from ParkSeller where VID='" + village.VID + "' and HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkSeller>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkSeller(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发商家信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载商家信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载商家优免信息
        /// </summary>
        /// <param name="village"></param>
        public static void ParkDerateDownload(BaseVillage village)
        {
            try
            {
                List<ParkDerate> list = new List<ParkDerate>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from ParkDerate a left join  ParkSeller b on a.SellerID=b.SellerID where b.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkDerate>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkDerate(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发商家优免信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + "下载商家优免信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载优免时段
        /// </summary>
        /// <param name="village"></param>
        public static void ParkDerateIntervarDownload(BaseVillage village)
        {
            try
            {
                List<ParkDerateIntervar> list = new List<ParkDerateIntervar>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkDerateIntervar a left join ParkDerate b on a.DerateID=b.DerateID left join  ParkSeller c on b.SellerID=c.SellerID where c.VID='" + village.VID + "' and a.HaveUpdate=3");
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkDerateIntervar>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadPKDerateintervar(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发优免时段：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载优免时段异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载商家优免券
        /// </summary>
        /// <param name="village"></param>
        public static void ParkCarDerateDownload(BaseVillage village)
        {
            try
            {
                List<ParkCarDerate> list = new List<ParkCarDerate>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkCarDerate a  left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate=3");
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkCarDerate>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkCarDerate(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发商家优免券：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载商家优免券\r\n", ex);
            }
        }

        /// <summary>
        /// 下载黑名单
        /// </summary>
        /// <param name="village"></param>
        public static void ParkBlacklistDownload(BaseVillage village)
        {
            try
            {
                List<ParkBlacklist> list = new List<ParkBlacklist>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkBlacklist a  left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate=3");
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkBlacklist>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkBlack(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发黑名单：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载黑名单异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载订单信息
        /// </summary>
        /// <param name="village"></param>
        public static void ParkOrderDownload(BaseVillage village)
        {
            try
            {
                List<ParkOrder> list = new List<ParkOrder>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkOrder a  left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate=3");
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkOrder(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发订单信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载订单信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载车位预定
        /// </summary>
        /// <param name="village"></param>
        public static void ParkReserveBitDownload(BaseVillage village)
        {
            try
            {
                List<ParkReserveBit> list = new List<ParkReserveBit>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkReserveBit a  left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate=3");
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkReserveBit>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkReserveBit(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发车位预定信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载车位预定信息异常\r\n", ex);
            }
        }

        /// <summary>
        /// 下载车位组
        /// </summary>
        /// <param name="village"></param>
        public static void ParkParkCarBitGroupDownload(BaseVillage village)
        {
            try
            {
                List<ParkCarBitGroup> list = new List<ParkCarBitGroup>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " a.* from ParkCarBitGroup a  left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<ParkCarBitGroup>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadParkCarBitGroup(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发车位组信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载车位组信息异常\r\n", ex);
            }
        }
        
        /// <summary>
        /// 下载访客信息
        /// </summary>
        /// <param name="village"></param>
        public static void VisitorInfoDownload(BaseVillage village)
        {
            try
            {
                List<VisitorInfo> list = new List<VisitorInfo>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = @"select top " + PageNum + " * from VisitorInfo where VID='" + village.VID + "' and HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            list.Add(DataReaderToModel<VisitorInfo>.ToModel(reader));
                        }
                    }
                }
                if (list.Any())
                {
                    string json = JsonHelper.GetJsonString(list);
                    bool result = new ParkDownloadDataService().DownloadVisitorInfo(village.ProxyNo, json);
                    
                    logger.Info($"{village.VName} 已下发访客信息：{json}\r\n");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(village.VName + " 下载访客信息异常\r\n", ex);
            }
        }

        #region 用户角色

        /// <summary>
        /// 下载角色
        /// </summary>
        public static void SysRolesDownload()
        {
            try
            {
                List<SysRoles> list = new List<SysRoles>();

                foreach (string key in new List<string>(ParkDownloadDataServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = @"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from SysRoles a left join UploadDataState b on a.RecordID=b.RecordID WHERE b.HaveUpdate=3 and b.TableName='sysroles' and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                        {
                            while (reader.Read())
                            {
                                list.Add(new SysRoles
                                {
                                    CPID = reader["CPID"].ToString(),
                                    DataStatus = (DataStatus)reader["DataStatus"].ToInt(),
                                    HaveUpdate = reader["HaveUpdate"].ToInt(),
                                    ID = reader["ID"].ToInt(),
                                    IsDefaultRole = (YesOrNo)reader["IsDefaultRole"].ToInt(),
                                    LastUpdateTime = reader["LastUpdateTime"].ToDateTime(),
                                    RecordID = reader["RecordID"].ToString(),
                                    RoleName = reader["RoleName"].ToString(),
                                    ProxyNo = reader["ProxyNo"].ToString(),
                                    HaveUpdate2 = reader["HaveUpdate2"].ToInt(),
                                    LastUpdateTime2 = reader["LastUpdateTime2"].ToDateTime()
                                });
                            }
                        }
                    }
                }

                foreach (var obj in list)
                {
                    bool result = new ParkDownloadDataService().DownloadSysRoles(obj.ProxyNo, JsonHelper.GetJsonString(new List<SysRoles>() { obj }));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载角色" + ex.Message + "\r\n");
            }
        }

        /// <summary>
        /// 下载用户信息
        /// </summary>
        public static void SysUserDownload()
        {
            List<SysUser> list = new List<SysUser>();
            try
            {
                foreach (string key in new List<string>(ParkDownloadDataServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = @"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from SysUser a left join UploadDataState b on a.RecordID=b.RecordID WHERE b.HaveUpdate=3 and b.TableName='sysuser' and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                        {
                            while (reader.Read())
                            {
                                SysUser model = new SysUser();
                                model.CPID = reader["CPID"].ToString();
                                model.DataStatus = (DataStatus)reader["DataStatus"].ToInt();
                                model.HaveUpdate = reader["HaveUpdate"].ToInt();
                                model.ID = reader["ID"].ToInt();
                                model.IsDefaultUser = (YesOrNo)reader["IsDefaultUser"].ToInt();
                                model.LastUpdateTime = reader["LastUpdateTime"].ToDateTime();
                                model.Password = reader["Password"].ToString();
                                model.PwdErrorCount = reader["PwdErrorCount"].ToInt();
                                model.PwdErrorTime = reader["PwdErrorCount"].ToDateTime();
                                model.RecordID = reader["RecordID"].ToString();
                                model.UserAccount = reader["UserAccount"].ToString();
                                model.UserName = reader["UserName"].ToString();
                                model.ProxyNo = reader["ProxyNo"].ToString();
                                model.HaveUpdate2 = reader["HaveUpdate2"].ToInt();
                                model.LastUpdateTime2 = reader["LastUpdateTime2"].ToDateTime();
                                list.Add(model);
                            }
                            foreach (var obj in list)
                            {
                                ParkDownloadDataService service = new ParkDownloadDataService();
                                bool result = service.DownloadSysUser(obj.ProxyNo, JsonHelper.GetJsonString(new List<SysUser>() { obj }));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载用户信息" + ex.Message + "\r\n");
            }
        }

        /// <summary>
        /// 下载用户拥有的角色
        /// </summary>
        public static void SysUserRolesMappingDownload()
        {
            List<SysUserRolesMapping> list = new List<SysUserRolesMapping>();
            try
            {
                foreach (string key in new List<string>(ParkDownloadDataServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = @"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from SysUserRolesMapping a left join UploadDataState b on a.RecordID=b.RecordID WHERE b.HaveUpdate=3 and b.TableName='sysuserrolesmapping' and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                        {
                            while (reader.Read())
                            {
                                SysUserRolesMapping model = new SysUserRolesMapping();
                                model.DataStatus = (DataStatus)reader["DataStatus"].ToInt();
                                model.HaveUpdate = reader["HaveUpdate"].ToInt();
                                model.ID = reader["ID"].ToInt();
                                model.LastUpdateTime = reader["LastUpdateTime"].ToDateTime();
                                model.RecordID = reader["RecordID"].ToString();
                                model.RoleID = reader["RoleID"].ToString();
                                model.UserRecordID = reader["UserRecordID"].ToString();

                                model.ProxyNo = reader["ProxyNo"].ToString();
                                model.HaveUpdate2 = reader["HaveUpdate2"].ToInt();
                                model.LastUpdateTime2 = reader["LastUpdateTime2"].ToDateTime();
                                list.Add(model);
                            }
                            foreach (var obj in list)
                            {
                                ParkDownloadDataService service = new ParkDownloadDataService();
                                bool result = service.DownloadSysUserRolesMapping(obj.ProxyNo, JsonHelper.GetJsonString(new List<SysUserRolesMapping>() { obj }));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载用户拥有的角色" + ex.Message + "\r\n");
            }
        }

        /// <summary>
        /// 下载作用域
        /// </summary>
        public static void SysScopeDownload()
        {
            List<SysScope> list = new List<SysScope>();
            try
            {
                foreach (string key in new List<string>(ParkDownloadDataServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = @"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from SysScope a left join UploadDataState b on a.ASID=b.RecordID WHERE b.HaveUpdate=3 and b.TableName='sysscope' and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                        {
                            while (reader.Read())
                            {
                                SysScope model = new SysScope();
                                model.ASID = reader["ASID"].ToString();
                                model.ASName = reader["ASName"].ToString();
                                model.CPID = reader["CPID"].ToString();
                                model.DataStatus = (DataStatus)reader["DataStatus"].ToInt();
                                model.HaveUpdate = reader["HaveUpdate"].ToInt();
                                model.ID = reader["ID"].ToInt();
                                model.IsDefaultScope = (YesOrNo)reader["IsDefaultScope"].ToInt();
                                model.LastUpdateTime = reader["LastUpdateTime"].ToDateTime();
                                model.ProxyNo = reader["ProxyNo"].ToString();
                                model.HaveUpdate2 = reader["HaveUpdate2"].ToInt();
                                model.LastUpdateTime2 = reader["LastUpdateTime2"].ToDateTime();
                                list.Add(model);
                            }
                            foreach (var obj in list)
                            {
                                ParkDownloadDataService service = new ParkDownloadDataService();
                                bool result = service.DownloadSysScope(obj.ProxyNo, JsonHelper.GetJsonString(new List<SysScope>() { obj }));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载作用域" + ex.Message + "\r\n");
            }
        }

        /// <summary>
        /// 下载作用域对应的场所
        /// </summary>
        public static void SysScopeAuthorizeDownload()
        {
            List<SysScopeAuthorize> list = new List<SysScopeAuthorize>();
            try
            {
                foreach (string key in new List<string>(ParkDownloadDataServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = @"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from SysScopeAuthorize a left join UploadDataState b on a.ASDID=b.RecordID WHERE b.HaveUpdate=3 and b.TableName='sysscopeauthorize' and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                        {
                            while (reader.Read())
                            {
                                SysScopeAuthorize model = new SysScopeAuthorize();
                                model.ASDID = reader["ASDID"].ToString();
                                model.ASID = reader["ASID"].ToString();
                                model.ASType = (ASType)reader["ASType"].ToInt();
                                model.CPID = reader["CPID"].ToString();
                                model.DataStatus = (DataStatus)reader["DataStatus"].ToInt();
                                model.HaveUpdate = reader["HaveUpdate"].ToInt();
                                model.ID = reader["ID"].ToInt();
                                model.LastUpdateTime = reader["LastUpdateTime"].ToDateTime();
                                model.TagID = reader["TagID"].ToString();
                                model.ProxyNo = reader["ProxyNo"].ToString();
                                model.HaveUpdate2 = reader["HaveUpdate2"].ToInt();
                                model.LastUpdateTime2 = reader["LastUpdateTime2"].ToDateTime();
                                list.Add(model);
                            }
                            foreach (var obj in list)
                            {
                                ParkDownloadDataService service = new ParkDownloadDataService();
                                bool result = service.DownloadSysScopeAuthorize(obj.ProxyNo, JsonHelper.GetJsonString(new List<SysScopeAuthorize>() { obj }));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载作用域对应的场所" + ex.Message + "\r\n");
            }
        }

        /// <summary>
        /// 下载用户对应的作用域
        /// </summary>
        public static void SysUserScopeMappingDownload()
        {
            List<SysUserScopeMapping> list = new List<SysUserScopeMapping>();
            try
            {
                foreach (string key in new List<string>(ParkDownloadDataServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = @"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from SysUserScopeMapping a left join UploadDataState b on a.RecordID=b.RecordID WHERE b.HaveUpdate=3 and b.TableName='sysuserscopemapping' and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                        {
                            while (reader.Read())
                            {
                                SysUserScopeMapping model = new SysUserScopeMapping();
                                model.ASID = reader["ASID"].ToString();
                                model.DataStatus = (DataStatus)reader["DataStatus"].ToInt();
                                model.HaveUpdate = reader["HaveUpdate"].ToInt();
                                model.ID = reader["ID"].ToInt();
                                model.LastUpdateTime = reader["LastUpdateTime"].ToDateTime();
                                model.RecordID = reader["RecordID"].ToString();
                                model.UserRecordID = reader["UserRecordID"].ToString();

                                model.ProxyNo = reader["ProxyNo"].ToString();
                                model.HaveUpdate2 = reader["HaveUpdate2"].ToInt();
                                model.LastUpdateTime2 = reader["LastUpdateTime2"].ToDateTime();
                                list.Add(model);
                            }
                            foreach (var obj in list)
                            {
                                ParkDownloadDataService service = new ParkDownloadDataService();
                                bool result = service.DownloadSysUserScopeMapping(obj.ProxyNo, JsonHelper.GetJsonString(new List<SysUserScopeMapping>() { obj }));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载用户对应的作用域" + ex.Message + "\r\n");
            }
        }

        /// <summary>
        /// 下载角色拥有的权限
        /// </summary>
        public static void SysRoleAuthorizeDowmload()
        {
            List<SysRoleAuthorize> list = new List<SysRoleAuthorize>();
            try
            {

                foreach (string key in new List<string>(ParkDownloadDataServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = @"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from SysRoleAuthorize a left join UploadDataState b on a.RecordID=b.RecordID WHERE b.HaveUpdate=3 and b.HaveUpdate=3 and b.TableName='sysroleauthorize' and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                        {
                            while (reader.Read())
                            {
                                SysRoleAuthorize model = new SysRoleAuthorize();
                                model.DataStatus = (DataStatus)reader["DataStatus"].ToInt();
                                model.HaveUpdate = reader["HaveUpdate"].ToInt();
                                model.ID = reader["ID"].ToInt();
                                model.LastUpdateTime = reader["LastUpdateTime"].ToDateTime();
                                model.ModuleID = reader["ModuleID"].ToString();
                                model.ParentID = reader["ParentID"].ToString();
                                model.RecordID = reader["RecordID"].ToString();
                                model.RoleID = reader["RoleID"].ToString();

                                model.ProxyNo = reader["ProxyNo"].ToString();
                                model.HaveUpdate2 = reader["HaveUpdate2"].ToInt();
                                model.LastUpdateTime2 = reader["LastUpdateTime2"].ToDateTime();
                                list.Add(model);
                            }
                            foreach (var obj in list)
                            {
                                ParkDownloadDataService service = new ParkDownloadDataService();
                                bool result = service.DownloadSysRoleAuthorize(obj.ProxyNo, JsonHelper.GetJsonString(new List<SysRoleAuthorize>() { obj }));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载角色拥有的权限" + ex.Message + "\r\n");
            }
        }
        #endregion
    }
}

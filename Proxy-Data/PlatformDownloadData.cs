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
        static int PageNum = 1;
        static bool IsStartDownload = true;

        /// <summary>
        /// 日志记录对象
        /// </summary>
        static Logger logger = LogManager.GetLogger("PlatformDownloadData");

        /// <summary>
        /// 开始上传数据
        /// </summary>
        public static void StartDownload()
        {
            IsStartDownload = true;
            Thread thread = new Thread(new ThreadStart(DownloadData));
            thread.IsBackground = true;
            thread.Start();
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
                                BaseVillage model = new BaseVillage();
                                model.VID = reader["VID"].ToString();
                                model.ProxyNo = reader["ProxyNo"].ToString();
                                listVillage.Add(model);
                            }
                        }
                    }
                }
                catch
                {
                }

                foreach (var obj in listVillage)
                {
                    if (WXServiceCallback.dit_callback.ContainsKey(obj.ProxyNo))
                    {
                        BaseParkinfoDownload(obj);
                        BasePassRemarkDownload(obj);
                        ParkAreaDownload(obj);
                        ParkBoxDownload(obj);
                        ParkGateDownload(obj);
                        ParkDeviceDownload(obj);
                        ParkCarTypeDownload(obj);
                        ParkCarModelDownload(obj);
                        ParkFeeRuleDownload(obj);
                        ParkFeeRuleDetailDownload(obj);
                        BaseCardDownload(obj);
                        ParkGrantDownload(obj);
                        ParkCardSuspendPlanDownload(obj);
                        BaseEmployeeDownload(obj);
                        EmployeePlateDownload(obj);
                        ParkSellerDownload(obj);
                        ParkDerateDownload(obj);
                        ParkDerateIntervarDownload(obj);
                        ParkCarDerateDownload(obj);
                        ParkBlacklistDownload(obj);
                        ParkOrderDownload(obj);
                        ParkReserveBitDownload(obj);
                        // ParkParkCarBitGroupDownload(obj);
                        // ParkDeviceDetectionDownload(obj);
                    }
                }
                Thread.Sleep(5 * 1000);
            }
        }

        /// <summary>
        /// 下载公司信息
        /// </summary>
        public static void BaseCompanyDownload()
        {
            List<BaseCompany> list = new List<BaseCompany>();
            try
            {
                foreach (string key in new List<string>(WXServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = string.Format(@"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from BaseCompany a left join UploadDataState b on a.CPID=b.RecordID WHERE b.HaveUpdate!=0 and b.TableName='basecompany'");
                        sql += " and b.HaveUpdate = 3 and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            while (reader.Read())
                            {
                                BaseCompany model = DataReaderToModel<BaseCompany>.ToModel(reader);
                                BaseVillage village = new BaseVillage();
                                village.ProxyNo = reader["ProxyNo"].ToString();
                                model.ListVillage = village;
                                list.Add(model);
                            }
                        }
                        foreach (var obj in list)
                        {
                            if (obj.ListVillage != null)
                            {
                                ParkDownloadDataService service = new ParkDownloadDataService();
                                bool result = service.DownloadCompany(obj.ListVillage.ProxyNo, JsonHelper.GetJsonString(new List<BaseCompany>() { obj }));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下发公司信息" + ex.Message + "\r\n");
            }
        }

        /// <summary>
        /// 下发小区信息
        /// </summary>
        public static void BaseVillageDownload()
        {

            try
            {
                foreach (string key in new List<string>(WXServiceCallback.dit_callback.Keys))
                {
                    List<BaseVillage> list = new List<BaseVillage>();
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = string.Format(@"select top " + PageNum + " * from BaseVillage where HaveUpdate!=0 and ProxyNo='" + key + "'");
                        sql += " and HaveUpdate = 3";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            while (reader.Read())
                            {
                                BaseVillage model = DataReaderToModel<BaseVillage>.ToModel(reader);
                                list.Add(model);
                            }
                        }

                    }

                    foreach (var obj in list)
                    {
                        ParkDownloadDataService service = new ParkDownloadDataService();
                        bool result = service.DownloadVillage(obj.ProxyNo, JsonHelper.GetJsonString(new List<BaseVillage>() { obj }));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载小区信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " * from BaseParkinfo where HaveUpdate!=0 and VID='" + village.VID + "' ");
                    sql += " and HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            BaseParkinfo model = DataReaderToModel<BaseParkinfo>.ToModel(reader);
                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {

                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkinfo(village.ProxyNo, JsonHelper.GetJsonString(list));

                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载车场信息" + ex.StackTrace + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from BasePassRemark a left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            BasePassRemark model = DataReaderToModel<BasePassRemark>.ToModel(reader);
                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadPassremark(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载车场放行描述信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkArea a left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkArea model = DataReaderToModel<ParkArea>.ToModel(reader);
                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkArea(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载车场区域信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkBox a left join  ParkArea b on a.AreaID=b.AreaID left join BaseParkinfo c on b.PKID=c.PKID where c.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkBox model = DataReaderToModel<ParkBox>.ToModel(reader);
                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkBox(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载车场岗亭信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkGate a left join  ParkBox b on a.BoxID=b.BoxID left join  ParkArea c on b.AreaID=c.AreaID left join BaseParkinfo d on c.PKID=d.PKID where d.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkGate model = DataReaderToModel<ParkGate>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadPKGate(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载车场通道信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkDevice a left join ParkGate b on a.GateID=b.GateID left join  ParkBox c on b.BoxID=c.BoxID left join  ParkArea d on c.AreaID=d.AreaID left join BaseParkinfo e on d.PKID=e.PKID where e.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkDevice model = DataReaderToModel<ParkDevice>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkDevice(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载车场设备信息" + ex.Message + "\r\n");
            }
        }

        public static void ParkDeviceParamDownload(BaseVillage village)
        {
            try
            {
                List<ParkDeviceParam> list = new List<ParkDeviceParam>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkDeviceParam a left join  ParkDevice g on a.DeviceID=g.DeviceID left join ParkGate b on g.GateID=b.GateID left join  ParkBox c on b.BoxID=c.BoxID left join  ParkArea d on c.AreaID=d.AreaID left join BaseParkinfo e on d.PKID=e.PKID where e.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkDeviceParam model = DataReaderToModel<ParkDeviceParam>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.ParkDeviceParamDownload(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载车场设备信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkDeviceDetection a left join  ParkDevice b on a.DeviceID=b.DeviceID left join ParkGate c on b.GateID=c.GateID left join  ParkBox d on c.BoxID=d.BoxID left join  ParkArea e on d.AreaID=e.AreaID left join BaseParkinfo f on e.PKID=f.PKID where f.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkDevice model = DataReaderToModel<ParkDevice>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkDevice(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载车场设备信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkCarType a left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkCarType model = DataReaderToModel<ParkCarType>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkCarType(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载车类信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkCarModel a left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkCarModel model = DataReaderToModel<ParkCarModel>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkCarModel(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载车型信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkFeeRule a left join ParkArea b on a.AreaID=b.AreaID left join  BaseParkinfo c on b.PKID=c.PKID where c.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkFeeRule model = DataReaderToModel<ParkFeeRule>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkFeeRule(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载收费规则信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkFeeRuleDetail a left join  ParkFeeRule b on a.RuleID=b.FeeRuleID  left join ParkArea c on b.AreaID=c.AreaID left join  BaseParkinfo d on c.PKID=d.PKID where d.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkFeeRuleDetail model = DataReaderToModel<ParkFeeRuleDetail>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkFeeRuleDetail(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载收费规则明细信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " * from BaseCard where VID='" + village.VID + "' and HaveUpdate!=0");
                    sql += " and HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            BaseCard model = DataReaderToModel<BaseCard>.ToModel(reader);


                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadBaseCard(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载小区卡片信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkGrant a left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkGrant model = DataReaderToModel<ParkGrant>.ToModel(reader);
                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkGrant(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载车场授权信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from  ParkCardSuspendPlan a left join  ParkGrant b on a.GrantID=b.GID left join BaseParkinfo c on b.PKID=c.PKID where c.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkCardSuspendPlan model = DataReaderToModel<ParkCardSuspendPlan>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkCardSuspendPlan(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载车场授权暂停信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " * from BaseEmployee where VID='" + village.VID + "' and HaveUpdate!=0");
                    sql += " and HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            BaseEmployee model = DataReaderToModel<BaseEmployee>.ToModel(reader);


                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadBaseEmployee(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载业主信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from EmployeePlate a left join  BaseEmployee b on a.EmployeeID=b.EmployeeID where b.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            EmployeePlate model = DataReaderToModel<EmployeePlate>.ToModel(reader);


                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadEmployeePlate(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载业主车牌信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " * from ParkSeller where VID='" + village.VID + "' and HaveUpdate!=0");
                    sql += " and HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkSeller model = DataReaderToModel<ParkSeller>.ToModel(reader);
                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkSeller(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载商家信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkDerate a left join  ParkSeller b on a.SellerID=b.SellerID where b.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkDerate model = DataReaderToModel<ParkDerate>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkDerate(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载商家优免信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkDerateIntervar a left join ParkDerate b on a.DerateID=b.DerateID left join  ParkSeller c on b.SellerID=c.SellerID where c.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkDerateIntervar model = DataReaderToModel<ParkDerateIntervar>.ToModel(reader);
                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadPKDerateintervar(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载优免时段" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkCarDerate a  left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkCarDerate model = DataReaderToModel<ParkCarDerate>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkCarDerate(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载商家优免券" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkBlacklist a  left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkBlacklist model = DataReaderToModel<ParkBlacklist>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {

                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkBlack(village.ProxyNo, JsonHelper.GetJsonString(list));

                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载黑名单" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkOrder a  left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkOrder model = DataReaderToModel<ParkOrder>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkOrder(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载订单信息" + ex.StackTrace + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkReserveBit a  left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkReserveBit model = DataReaderToModel<ParkReserveBit>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkReserveBit(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载车位预定信息" + ex.Message + "\r\n");
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
                    string sql = string.Format(@"select top " + PageNum + " a.* from ParkCarBitGroup a  left join BaseParkinfo b on a.PKID=b.PKID where b.VID='" + village.VID + "' and a.HaveUpdate!=0");
                    sql += " and a.HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkCarBitGroup model = DataReaderToModel<ParkCarBitGroup>.ToModel(reader);

                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadParkCarBitGroup(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载车位预定信息" + ex.Message + "\r\n");
            }
        }
        #region 访客信息
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
                    string sql = string.Format(@"select top " + PageNum + " * from VisitorInfo where VID='" + village.VID + "' and HaveUpdate!=0");
                    sql += " and HaveUpdate=3";
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                    {
                        while (reader.Read())
                        {
                            VisitorInfo model = DataReaderToModel<VisitorInfo>.ToModel(reader);
                            list.Add(model);
                        }
                    }
                }
                if (list.Count() > 0)
                {
                    ParkDownloadDataService service = new ParkDownloadDataService();
                    bool result = service.DownloadVisitorInfo(village.ProxyNo, JsonHelper.GetJsonString(list));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("下载访客信息" + ex.Message + "\r\n");
            }
        }
        #endregion
        #region 用户角色

        /// <summary>
        /// 下载角色
        /// </summary>
        public static void SysRolesDownload()
        {
            List<SysRoles> list = new List<SysRoles>();
            try
            {
                foreach (string key in new List<string>(WXServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = string.Format(@"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from SysRoles a left join UploadDataState b on a.RecordID=b.RecordID WHERE b.HaveUpdate!=0 and b.TableName='sysroles'");
                        sql += " and b.HaveUpdate=3 and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            while (reader.Read())
                            {
                                SysRoles model = new SysRoles();
                                model.CPID = reader["CPID"].ToString();
                                model.DataStatus = (DataStatus)reader["DataStatus"].ToInt();
                                model.HaveUpdate = reader["HaveUpdate"].ToInt();
                                model.ID = reader["ID"].ToInt();
                                model.IsDefaultRole = (YesOrNo)reader["IsDefaultRole"].ToInt();
                                model.LastUpdateTime = reader["LastUpdateTime"].ToDateTime();
                                model.RecordID = reader["RecordID"].ToString();
                                model.RoleName = reader["RoleName"].ToString();
                                model.ProxyNo = reader["ProxyNo"].ToString();
                                model.HaveUpdate2 = reader["HaveUpdate2"].ToInt();
                                model.LastUpdateTime2 = reader["LastUpdateTime2"].ToDateTime();
                                list.Add(model);
                            }
                            foreach (var obj in list)
                            {
                                ParkDownloadDataService service = new ParkDownloadDataService();
                                bool result = service.DownloadSysRoles(obj.ProxyNo, JsonHelper.GetJsonString(new List<SysRoles>() { obj }));
                            }
                        }
                    }
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
                foreach (string key in new List<string>(WXServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = string.Format(@"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from SysUser a left join UploadDataState b on a.RecordID=b.RecordID WHERE b.HaveUpdate!=0 and b.TableName='sysuser'");
                        sql += " and b.HaveUpdate=3 and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
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
                foreach (string key in new List<string>(WXServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = string.Format(@"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from SysUserRolesMapping a left join UploadDataState b on a.RecordID=b.RecordID WHERE b.HaveUpdate!=0 and b.TableName='sysuserrolesmapping'");
                        sql += " and b.HaveUpdate=3 and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
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
                foreach (string key in new List<string>(WXServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = string.Format(@"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from SysScope a left join UploadDataState b on a.ASID=b.RecordID WHERE b.HaveUpdate!=0 and b.TableName='sysscope'");
                        sql += " and b.HaveUpdate=3 and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
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
                foreach (string key in new List<string>(WXServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = string.Format(@"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from SysScopeAuthorize a left join UploadDataState b on a.ASDID=b.RecordID WHERE b.HaveUpdate!=0 and b.TableName='sysscopeauthorize'");
                        sql += " and b.HaveUpdate=3 and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
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
                foreach (string key in new List<string>(WXServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = string.Format(@"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from SysUserScopeMapping a left join UploadDataState b on a.RecordID=b.RecordID WHERE b.HaveUpdate!=0 and b.TableName='sysuserscopemapping'");
                        sql += " and b.HaveUpdate=3 and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
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

                foreach (string key in new List<string>(WXServiceCallback.dit_callback.Keys))
                {
                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                    {
                        string sql = string.Format(@"select top " + PageNum + " a.*,b.ProxyNo,b.HaveUpdate as HaveUpdate2,b.LastUpdateTime as LastUpdateTime2 from SysRoleAuthorize a left join UploadDataState b on a.RecordID=b.RecordID WHERE b.HaveUpdate!=0 and b.HaveUpdate=3 and b.TableName='sysroleauthorize'");
                        sql += " and b.HaveUpdate=3 and b.ProxyNo='" + key + "'";
                        dbOperator.ClearParameters();
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
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

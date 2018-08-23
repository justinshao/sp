using Common.Core;
using Common.DataAccess;
using Common.Entities;
using Common.Entities.BaseData;
using Common.Entities.Parking;
using Common.Entities.Servers;
using Common.Utilities;
using Common.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.ServiceModel;

namespace PlatformWcfServers
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“FoundationService”。
    [ServiceContract]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public class ClientDownDataService
    {
        /// <summary>
        /// 日志记录对象
        /// </summary>
        protected Logger logger = LogManager.GetLogger("数据下载服务");

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
            return "数据下载服务";
        }

        /// <summary>
        /// 监控端主动拉取平台数据
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="ProxyNo"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract]
        public string DownServiceData(string cmd, string ProxyNo)
        {
            string jsondata = "";
            string VID = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = string.Format(@"select * from BaseVillage where ProxyNo=@ProxyNo and DataStatus<2 ");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ProxyNo", ProxyNo);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            VID = reader["VID"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("监控端主动拉取平台数据异常！DownServiceData()\r\n", ex);
            }
            if (string.IsNullOrEmpty(VID))
            {
                return jsondata;
            }

            try
            {
                switch (cmd)
                {
                    case "BaseCard":
                        return DownBaseCard(VID);
                    case "BaseCompany":
                        return DownBaseCompany(VID);
                    case "BaseEmployee":
                        return DownBaseEmployee(VID);
                    case "BaseParkinfo":
                        return DownBaseParkinfo(VID);
                    case "BasePassRemark":
                        return DownBasePassRemark(VID);
                    case "BaseVillage":
                        return DownBaseVillage(VID);
                    case "EmployeePlate":
                        return DownEmployeePlate(VID);
                    case "ParkArea":
                        return DownParkArea(VID);
                    case "ParkBlacklist":
                        return DownParkBlacklist(VID);
                    case "ParkBox":
                        return DownParkBox(VID);
                    case "ParkCardSuspendPlan":
                        return DownParkCardSuspendPlan(VID);
                    case "ParkCarModel":
                        return DownParkCarModel(VID);
                    case "ParkCarType":
                        return DownParkCarType(VID);
                    case "ParkDerate":
                        return DownParkDerate(VID);
                    case "ParkDevice":
                        return DownParkDevice(VID);
                    case "ParkDeviceParam":
                        return DownParkDeviceParam(VID);
                    case "ParkFeeRule":
                        return DownParkFeeRule(VID);
                    case "ParkFeeRuleDetail":
                        return DownParkFeeRuleDetail(VID);
                    case "ParkGate":
                        return DownParkGate(VID);
                    case "ParkGrant":
                        return DownParkGrant(VID);
                    case "ParkSeller":
                        return DownParkSeller(VID);
                    case "SysRoleAuthorize":
                        return DownSysRoleAuthorize(VID);
                    case "SysRoles":
                        return DownSysRoles(VID);
                    case "SysScope":
                        return DownSysScope(VID);
                    case "SysScopeAuthorize":
                        return DownSysScopeAuthorize(VID);
                    case "SysUser":
                        return DownSysUser(VID);
                    case "SysUserRolesMapping":
                        return DownSysUserRolesMapping(VID);
                    case "SysUserScopeMapping":
                        return DownSysUserScopeMapping(VID);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal($"监控端主动拉取平台数据异常({cmd})", ex);
            }

            return jsondata;
        }

        /// <summary>
        /// 监控端主动拉取平台数据(分页)
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="ProxyNo"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        [OperationContract]
        public string DownServiceDataPage(string cmd, string ProxyNo, string VID, int PageIndex)
        {
            string jsondata = "";
            try
            {
                switch (cmd)
                {
                    case "BaseCard":
                        return DownBaseCard(VID, PageIndex);
                    case "BaseCompany":
                        return DownBaseCompany(VID, PageIndex);
                    case "BaseEmployee":
                        return DownBaseEmployee(VID, PageIndex);
                    case "BaseParkinfo":
                        return DownBaseParkinfo(VID, PageIndex);
                    case "BasePassRemark":
                        return DownBasePassRemark(VID, PageIndex);
                    case "BaseVillage":
                        return DownBaseVillage(VID, PageIndex);
                    case "EmployeePlate":
                        return DownEmployeePlate(VID, PageIndex);
                    case "ParkArea":
                        return DownParkArea(VID, PageIndex);
                    case "ParkBlacklist":
                        return DownParkBlacklist(VID, PageIndex);
                    case "ParkBox":
                        return DownParkBox(VID, PageIndex);
                    case "ParkCardSuspendPlan":
                        return DownParkCardSuspendPlan(VID, PageIndex);
                    case "ParkCarModel":
                        return DownParkCarModel(VID, PageIndex);
                    case "ParkCarType":
                        return DownParkCarType(VID, PageIndex);
                    case "ParkDerate":
                        return DownParkDerate(VID, PageIndex);
                    case "ParkDevice":
                        return DownParkDevice(VID, PageIndex);
                    case "ParkDeviceParam":
                        return DownParkDeviceParam(VID, PageIndex);
                    case "ParkFeeRule":
                        return DownParkFeeRule(VID, PageIndex);
                    case "ParkFeeRuleDetail":
                        return DownParkFeeRuleDetail(VID, PageIndex);
                    case "ParkGate":
                        return DownParkGate(VID, PageIndex);
                    case "ParkGrant":
                        return DownParkGrant(VID, PageIndex);
                    case "ParkSeller":
                        return DownParkSeller(VID, PageIndex);
                    case "SysRoleAuthorize":
                        return DownSysRoleAuthorize(VID, PageIndex);
                    case "SysRoles":
                        return DownSysRoles(VID, PageIndex);
                    case "SysScope":
                        return DownSysScope(VID, PageIndex);
                    case "SysScopeAuthorize":
                        return DownSysScopeAuthorize(VID, PageIndex);
                    case "SysUser":
                        return DownSysUser(VID, PageIndex);
                    case "SysUserRolesMapping":
                        return DownSysUserRolesMapping(VID, PageIndex);
                    case "SysUserScopeMapping":
                        return DownSysUserScopeMapping(VID, PageIndex);
                    case "ParkIORecord":
                        return DowLoadParkIORecord(VID, PageIndex);

                }
            }
            catch (Exception ex)
            {
                logger.Fatal($"监控端主动拉取平台数据异常({cmd})", ex);
            }
            return jsondata;
        }

        /// <summary>
        /// 获取表数据数量
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="StrTable"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetTableCount(string ProxyNo, string StrTable)
        {
            TableDataCount model = new TableDataCount();
            string jsondata = "";
            string VID = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = string.Format(@"select * from BaseVillage where ProxyNo=@ProxyNo and DataStatus<2 ");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("ProxyNo", ProxyNo);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            VID = reader["VID"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("监控端主动拉取平台表数据数量异常！GetTableCount()\r\n", ex);
            }
            if (string.IsNullOrEmpty(VID))
            {
                return jsondata;
            }
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    int DataCount = 0;
                    string strsql = "";
                    switch (StrTable)
                    {
                        case "BaseCard":
                            strsql = string.Format(@"select  count(1) as DataCount from " + StrTable + " where VID=@VID ");
                            break;
                        case "BaseCompany":
                            strsql = string.Format(@"select   count(1) as DataCount from BaseCompany a left join BaseVillage b on a.CPID=b.CPID where b.VID=@VID ");
                            break;
                        case "BaseEmployee":
                            strsql = string.Format(@"select count(1) as DataCount from BaseEmployee where VID=@VID  ");
                            break;
                        case "BaseParkinfo":
                            strsql = string.Format(@"select count(1) as DataCount from BaseParkinfo where VID=@VID  ");
                            break;
                        case "BasePassRemark":
                            strsql = string.Format(@"select count(1) as DataCount from BasePassRemark a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID ");
                            break;
                        case "BaseVillage":
                            strsql = string.Format(@"select count(1) as DataCount from BaseVillage where VID=@VID ");
                            break;
                        case "EmployeePlate":
                            strsql = string.Format(@"select count(1) as DataCount from EmployeePlate a left join BaseEmployee b on a.EmployeeID=b.EmployeeID where b.VID=@VID ");
                            break;
                        case "ParkArea":
                            strsql = string.Format(@"select count(1) as DataCount from ParkArea a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID ");
                            break;
                        case "ParkBlacklist":
                            strsql = string.Format(@"select count(1) as DataCount from ParkBlacklist a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID ");
                            break;
                        case "ParkBox":
                            strsql = string.Format(@"select count(1) as DataCount from ParkBox a left join ParkArea b on a.AreaID=b.AreaID left join BaseParkinfo c on b.PKID=c.PKID where c.VID=@VID ");
                            break;
                        case "ParkCardSuspendPlan":
                            strsql = string.Format(@"select count(1) as DataCount from ParkCardSuspendPlan a left join ParkGrant b on a.GrantID=b.GID left join BaseParkinfo c on b.PKID=c.PKID where c.VID=@VID ");
                            break;
                        case "ParkCarModel":
                            strsql = string.Format(@"select count(1) as DataCount from ParkCarModel a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID ");
                            break;
                        case "ParkCarType":
                            strsql = string.Format(@"select count(1) as DataCount from ParkCarType a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID ");
                            break;
                        case "ParkDerate":
                            strsql = string.Format(@"select count(1) as DataCount from ParkDerate a left join ParkSeller b on a.SellerID=b.SellerID where b.VID=@VID ");
                            break;
                        case "ParkDevice":
                            strsql = "select count(1) as DataCount from ParkDevice a left join ParkGate b on a.GateID=b.GateID left join  ";
                            strsql += "ParkBox c on b.BoxID=c.BoxID left join ParkArea d on c.AreaID=d.AreaID left join ";
                            strsql += "BaseParkinfo e on d.PKID=e.PKID where e.VID=@VID";
                            break;
                        case "ParkDeviceParam":
                            strsql = "select count(1) as DataCount from ParkDeviceParam a left join ParkDevice g on a.DeviceID=g.DeviceID left join ParkGate b on g.GateID=b.GateID left join  ";
                            strsql += "ParkBox c on b.BoxID=c.BoxID left join ParkArea d on c.AreaID=d.AreaID left join ";
                            strsql += "BaseParkinfo e on d.PKID=e.PKID where e.VID=@VID";
                            break;
                        case "ParkFeeRule":
                            strsql = "select count(1) as DataCount from ParkFeeRule a left join ParkArea b on a.AreaID=b.AreaID left join BaseParkinfo c on b.PKID=c.PKID  where c.VID=@VID";
                            break;
                        case "ParkFeeRuleDetail":
                            strsql = "select count(1) as DataCount from ParkFeeRuleDetail a left join ParkFeeRule b on a.RuleID=b.FeeRuleID left join ParkArea c on b.AreaID=c.AreaID left join BaseParkinfo e on c.PKID=e.PKID  where e.VID=@VID";
                            break;
                        case "ParkGate":
                            strsql = "select count(1) as DataCount from ParkDevice a left join ParkGate b on a.GateID=b.GateID left join  ";
                            strsql += "ParkBox c on b.BoxID=c.BoxID left join ParkArea d on c.AreaID=d.AreaID left join ";
                            strsql += "BaseParkinfo e on d.PKID=e.PKID where e.VID=@VID";
                            break;
                        case "ParkGrant":
                            strsql = "select count(1) as DataCount from ParkGrant a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID ";
                            break;
                        case "ParkSeller":
                            strsql = "select count(1) as DataCount from ParkSeller where VID=@VID ";
                            break;
                        case "SysRoleAuthorize":
                            strsql = "select count(1) as DataCount from SysRoleAuthorize a left join SysRoles b on a.RoleID=b.RecordID ";
                            strsql += " left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID  where VID=@VID";
                            break;
                        case "SysRoles":
                            strsql = "select count(1) as DataCount from SysRoles b left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID where VID=@VID ";
                            break;
                        case "SysScope":
                            strsql = "select count(1) as DataCount from SysScope b left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID where VID=@VID ";
                            break;
                        case "SysScopeAuthorize":
                            strsql = "select count(1) as DataCount from SysScopeAuthorize b left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID where VID=@VID ";
                            break;
                        case "SysUser":
                            strsql = "select count(1) as DataCount from SysUser b left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID where VID=@VID ";
                            break;
                        case "SysUserRolesMapping":
                            strsql = "select count(1) as DataCount from SysUserRolesMapping a left join SysRoles b on a.RoleID=b.RecordID left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID where VID=@VID ";
                            break;
                        case "SysUserScopeMapping":
                            strsql = "select count(1) as DataCount from SysUserScopeMapping a left join SysScope b on a.ASID=b.ASID left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID where VID=@VID ";
                            break;
                        case "ParkIORecord":
                            strsql = "select count(1) as DataCount from ParkIORecord a left join BaseParkinfo b on a.ParkingID=b.PKID where b.VID=@VID and IsExit=0";
                            break;

                    }
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("VID", VID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            DataCount = int.Parse(reader["DataCount"].ToString());
                        }
                    }
                    int PageCount = DataCount / 50;
                    if (DataCount % 50 > 0)
                    {

                        PageCount++;
                    }
                    model.DataCount = DataCount;
                    model.PageCount = PageCount;

                    model.VID = VID;
                    jsondata = JsonHelper.GetJsonString(model);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("监控端主动拉取平台表数据数量异常！GetTableCount()\r\n", ex);
            }

            return jsondata;
        }
        
        /// <summary>
        /// 获取卡片信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        public string DownBaseCard(string VID)
        {
            string strlist = "";
            List<BaseCard> list = new List<BaseCard>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select  * from BaseCard where VID=@VID ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        BaseCard model = DataReaderToModel<BaseCard>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 分页查询卡片信息
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownBaseCard(string VID, int PageIndex)
        {
            string strlist = "";
            List<BaseCard> list = new List<BaseCard>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY LastUpdateTime desc ) AS rownum, *
                                        from BaseCard where VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        BaseCard model = DataReaderToModel<BaseCard>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载公司信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        public string DownBaseCompany(string VID)
        {
            string strlist = "";
            List<BaseCompany> list = new List<BaseCompany>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select  a.* from BaseCompany a left join BaseVillage b on a.CPID=b.CPID where b.VID=@VID ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        BaseCompany model = DataReaderToModel<BaseCompany>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        ///主动下载公司信息分页查询 
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownBaseCompany(string VID, int PageIndex)
        {
            string strlist = "";
            List<BaseCompany> list = new List<BaseCompany>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from BaseCompany a left join BaseVillage b on a.CPID=b.CPID
                                        where b.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        BaseCompany model = DataReaderToModel<BaseCompany>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载住户信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        public string DownBaseEmployee(string VID)
        {
            string strlist = "";
            List<BaseEmployee> list = new List<BaseEmployee>();
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select * from BaseEmployee where VID=@VID  ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        BaseEmployee model = DataReaderToModel<BaseEmployee>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载住户信息分页查询
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownBaseEmployee(string VID, int PageIndex)
        {
            string strlist = "";
            List<BaseEmployee> list = new List<BaseEmployee>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY LastUpdateTime desc ) AS rownum, *
                                        from BaseEmployee where VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        BaseEmployee model = DataReaderToModel<BaseEmployee>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载车场信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        public string DownBaseParkinfo(string VID)
        {
            string strlist = "";
            List<BaseParkinfo> list = new List<BaseParkinfo>();
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select * from BaseParkinfo where VID=@VID  ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        BaseParkinfo model = DataReaderToModel<BaseParkinfo>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载车场信息
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownBaseParkinfo(string VID, int PageIndex)
        {
            string strlist = "";
            List<BaseParkinfo> list = new List<BaseParkinfo>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY LastUpdateTime desc ) AS rownum, *
                                        from BaseParkinfo where VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        BaseParkinfo model = DataReaderToModel<BaseParkinfo>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载车场放行备注信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownBasePassRemark(string VID)
        {
            string strlist = "";
            List<BasePassRemark> list = new List<BasePassRemark>();
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select a.* from BasePassRemark a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        BasePassRemark model = DataReaderToModel<BasePassRemark>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载车场放行备注信息
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownBasePassRemark(string VID, int PageIndex)
        {
            string strlist = "";
            List<BasePassRemark> list = new List<BasePassRemark>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from BasePassRemark a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        BasePassRemark model = DataReaderToModel<BasePassRemark>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载小区信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownBaseVillage(string VID)
        {
            string strlist = "";
            List<BaseVillage> list = new List<BaseVillage>();
            
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select * from BaseVillage where VID=@VID ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        BaseVillage model = DataReaderToModel<BaseVillage>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载小区信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownBaseVillage(string VID, int PageIndex)
        {
            string strlist = "";
            List<BaseVillage> list = new List<BaseVillage>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY LastUpdateTime desc ) AS rownum, *
                                        from BaseVillage where VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        BaseVillage model = DataReaderToModel<BaseVillage>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载住户车牌信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownEmployeePlate(string VID)
        {
            string strlist = "";
            List<EmployeePlate> list = new List<EmployeePlate>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select a.* from EmployeePlate a left join BaseEmployee b on a.EmployeeID=b.EmployeeID where b.VID=@VID ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        EmployeePlate model = DataReaderToModel<EmployeePlate>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载住户车牌信息分页查询
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownEmployeePlate(string VID, int PageIndex)
        {
            string strlist = "";
            List<EmployeePlate> list = new List<EmployeePlate>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from EmployeePlate a left join BaseEmployee b on a.EmployeeID=b.EmployeeID where b.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        EmployeePlate model = DataReaderToModel<EmployeePlate>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载停车区域信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkArea(string VID)
        {
            string strlist = "";
            List<ParkArea> list = new List<ParkArea>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select a.* from ParkArea a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkArea model = DataReaderToModel<ParkArea>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载停车区域信息
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownParkArea(string VID, int PageIndex)
        {
            string strlist = "";
            List<ParkArea> list = new List<ParkArea>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from ParkArea a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkArea model = DataReaderToModel<ParkArea>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载车场黑名单信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkBlacklist(string VID)
        {
            string strlist = "";
            List<ParkBlacklist> list = new List<ParkBlacklist>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select a.* from ParkBlacklist a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkBlacklist model = DataReaderToModel<ParkBlacklist>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载车场黑名单信息
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownParkBlacklist(string VID, int PageIndex)
        {
            string strlist = "";
            List<ParkBlacklist> list = new List<ParkBlacklist>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from ParkBlacklist a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkBlacklist model = DataReaderToModel<ParkBlacklist>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载岗亭信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkBox(string VID)
        {
            string strlist = "";
            List<ParkBox> list = new List<ParkBox>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select a.* from ParkBox a left join ParkArea b on a.AreaID=b.AreaID left join BaseParkinfo c on b.PKID=c.PKID where c.VID=@VID ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkBox model = DataReaderToModel<ParkBox>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载岗亭信息
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownParkBox(string VID, int PageIndex)
        {
            string strlist = "";
            List<ParkBox> list = new List<ParkBox>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from ParkBox a left join ParkArea b on a.AreaID=b.AreaID left join BaseParkinfo c on b.PKID=c.PKID where c.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkBox model = DataReaderToModel<ParkBox>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载卡片暂停信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkCardSuspendPlan(string VID)
        {
            string strlist = "";
            List<ParkCardSuspendPlan> list = new List<ParkCardSuspendPlan>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select a.* from ParkCardSuspendPlan a left join ParkGrant b on a.GrantID=b.GID left join BaseParkinfo c on b.PKID=c.PKID where c.VID=@VID ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkCardSuspendPlan model = DataReaderToModel<ParkCardSuspendPlan>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载卡片暂停信息
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownParkCardSuspendPlan(string VID, int PageIndex)
        {
            string strlist = "";
            List<ParkCardSuspendPlan> list = new List<ParkCardSuspendPlan>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from ParkCardSuspendPlan a left join ParkGrant b on a.GrantID=b.GID left join BaseParkinfo c on b.PKID=c.PKID where c.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkCardSuspendPlan model = DataReaderToModel<ParkCardSuspendPlan>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载车型信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkCarModel(string VID)
        {
            string strlist = "";
            List<ParkCarModel> list = new List<ParkCarModel>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select a.* from ParkCarModel a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkCarModel model = DataReaderToModel<ParkCarModel>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载车型信息
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownParkCarModel(string VID, int PageIndex)
        {
            string strlist = "";
            List<ParkCarModel> list = new List<ParkCarModel>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from ParkCarModel a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkCarModel model = DataReaderToModel<ParkCarModel>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载车类型信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkCarType(string VID)
        {
            string strlist = "";
            List<ParkCarType> list = new List<ParkCarType>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select a.* from ParkCarType a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkCarType model = DataReaderToModel<ParkCarType>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载车类型信息
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownParkCarType(string VID, int PageIndex)
        {
            string strlist = "";
            List<ParkCarType> list = new List<ParkCarType>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from ParkCarType a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkCarType model = DataReaderToModel<ParkCarType>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载打折方式信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkDerate(string VID)
        {
            string strlist = "";
            List<ParkDerate> list = new List<ParkDerate>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select a.* from ParkDerate a left join ParkSeller b on a.SellerID=b.SellerID where b.VID=@VID ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkDerate model = DataReaderToModel<ParkDerate>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载打折方式信息
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownParkDerate(string VID, int PageIndex)
        {
            string strlist = "";
            List<ParkDerate> list = new List<ParkDerate>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from ParkDerate a left join ParkSeller b on a.SellerID=b.SellerID where b.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkDerate model = DataReaderToModel<ParkDerate>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载设备信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkDevice(string VID)
        {
            string strlist = "";
            List<ParkDevice> list = new List<ParkDevice>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = "select a.* from ParkDevice a left join ParkGate b on a.GateID=b.GateID left join  ";
                strsql += "ParkBox c on b.BoxID=c.BoxID left join ParkArea d on c.AreaID=d.AreaID left join ";
                strsql += "BaseParkinfo e on d.PKID=e.PKID where e.VID=@VID";

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkDevice model = DataReaderToModel<ParkDevice>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        public string DownParkDevice(string VID, int PageIndex)
        {
            string strlist = "";
            List<ParkDevice> list = new List<ParkDevice>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from ParkDevice a left join ParkGate b on a.GateID=b.GateID left join 
                                        ParkBox c on b.BoxID=c.BoxID left join ParkArea d on c.AreaID=d.AreaID left join
                                        BaseParkinfo e on d.PKID=e.PKID 
                                        where e.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkDevice model = DataReaderToModel<ParkDevice>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载设备参数信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkDeviceParam(string VID)
        {
            string strlist = "";
            List<ParkDeviceParam> list = new List<ParkDeviceParam>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = "select a.* from ParkDeviceParam a left join ParkDevice g on a.DeviceID=g.DeviceID left join ParkGate b on g.GateID=b.GateID left join  ";
                strsql += "ParkBox c on b.BoxID=c.BoxID left join ParkArea d on c.AreaID=d.AreaID left join ";
                strsql += "BaseParkinfo e on d.PKID=e.PKID where e.VID=@VID";

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkDeviceParam model = DataReaderToModel<ParkDeviceParam>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载设备参数信息
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownParkDeviceParam(string VID, int PageIndex)
        {
            string strlist = "";
            List<ParkDeviceParam> list = new List<ParkDeviceParam>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from ParkDeviceParam a left join ParkDevice g on a.DeviceID=g.DeviceID left join ParkGate b on g.GateID=b.GateID left join 
                                       ParkBox c on b.BoxID=c.BoxID left join ParkArea d on c.AreaID=d.AreaID left join
                                       BaseParkinfo e on d.PKID=e.PKID 
                                        where e.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkDeviceParam model = DataReaderToModel<ParkDeviceParam>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载收费规则信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkFeeRule(string VID)
        {
            string strlist = "";
            List<ParkFeeRule> list = new List<ParkFeeRule>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = "select a.* from ParkFeeRule a left join ParkArea b on a.AreaID=b.AreaID left join BaseParkinfo c on b.PKID=c.PKID  where c.VID=@VID";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkFeeRule model = DataReaderToModel<ParkFeeRule>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载收费规则信息
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownParkFeeRule(string VID, int PageIndex)
        {
            string strlist = "";
            List<ParkFeeRule> list = new List<ParkFeeRule>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from ParkFeeRule a left join ParkArea b on a.AreaID=b.AreaID left join BaseParkinfo c on b.PKID=c.PKID                                     
                                        where c.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkFeeRule model = DataReaderToModel<ParkFeeRule>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载收费规则明细
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkFeeRuleDetail(string VID)
        {
            string strlist = "";
            List<ParkFeeRuleDetail> list = new List<ParkFeeRuleDetail>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = "select a.* from ParkFeeRuleDetail a left join ParkFeeRule b on a.RuleID=b.FeeRuleID left join ParkArea c on b.AreaID=c.AreaID left join BaseParkinfo e on c.PKID=e.PKID  where e.VID=@VID";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkFeeRuleDetail model = DataReaderToModel<ParkFeeRuleDetail>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载收费规则明细
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkFeeRuleDetail(string VID, int PageIndex)
        {
            string strlist = "";

            List<ParkFeeRuleDetail> list = new List<ParkFeeRuleDetail>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from ParkFeeRuleDetail a left join ParkFeeRule b on a.RuleID=b.FeeRuleID left join ParkArea c on b.AreaID=c.AreaID left join BaseParkinfo e on c.PKID=e.PKID                                     
                                        where e.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkFeeRuleDetail model = DataReaderToModel<ParkFeeRuleDetail>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载通道信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkGate(string VID)
        {
            string strlist = "";
            List<ParkGate> list = new List<ParkGate>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = "select b.* from ParkGate b  left join  ";
                strsql += "ParkBox c on b.BoxID=c.BoxID left join ParkArea d on c.AreaID=d.AreaID left join ";
                strsql += "BaseParkinfo e on d.PKID=e.PKID where e.VID=@VID";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkGate model = DataReaderToModel<ParkGate>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载通道信息
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownParkGate(string VID, int PageIndex)
        {
            string strlist = "";
            List<ParkGate> list = new List<ParkGate>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY b.LastUpdateTime desc ) AS rownum, b.*
                                        from ParkGate b  left join ParkBox c on b.BoxID=c.BoxID left join ParkArea d on c.AreaID=d.AreaID left join  
                                        BaseParkinfo e on d.PKID=e.PKID                                   
                                        where e.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkGate model = DataReaderToModel<ParkGate>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载授权信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkGrant(string VID)
        {
            string strlist = "";
            List<ParkGrant> list = new List<ParkGrant>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = "select a.* from ParkGrant a left join BaseParkinfo b on a.PKID=b.PKID where b.VID=@VID ";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkGrant model = DataReaderToModel<ParkGrant>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载授权信息分页
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownParkGrant(string VID, int PageIndex)
        {
            string strlist = "";
            List<ParkGrant> list = new List<ParkGrant>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from ParkGrant a left join BaseParkinfo b on a.PKID=b.PKID                                     
                                        where b.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkGrant model = DataReaderToModel<ParkGrant>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载商户信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownParkSeller(string VID)
        {
            string strlist = "";
            List<ParkSeller> list = new List<ParkSeller>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = "select * from ParkSeller where VID=@VID ";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkSeller model = DataReaderToModel<ParkSeller>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载商户信息分页
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownParkSeller(string VID, int PageIndex)
        {
            string strlist = "";
            List<ParkSeller> list = new List<ParkSeller>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY LastUpdateTime desc ) AS rownum, *
                                        from ParkSeller                                     
                                        where VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkSeller model = DataReaderToModel<ParkSeller>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载角色授权信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownSysRoleAuthorize(string VID)
        {
            string strlist = "";
            List<SysRoleAuthorize> list = new List<SysRoleAuthorize>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                string strsql = "select a.* from SysRoleAuthorize a left join SysRoles b on a.RoleID=b.RecordID ";
                strsql += " left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID  where VID=@VID";

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        SysRoleAuthorize model = DataReaderToModel<SysRoleAuthorize>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载角色授权信息分页
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownSysRoleAuthorize(string VID, int PageIndex)
        {
            string strlist = "";
            List<SysRoleAuthorize> list = new List<SysRoleAuthorize>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from SysRoleAuthorize a left join SysRoles b on a.RoleID=b.RecordID    
                                        left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID                                 
                                        where VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        SysRoleAuthorize model = DataReaderToModel<SysRoleAuthorize>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }
        /// <summary>
        /// 主动下载角色信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownSysRoles(string VID)
        {
            string strlist = "";
            List<SysRoles> list = new List<SysRoles>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                string strsql = "select b.* from SysRoles b left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID where VID=@VID ";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        SysRoles model = DataReaderToModel<SysRoles>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载角色信息分页
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownSysRoles(string VID, int PageIndex)
        {
            string strlist = "";
            List<SysRoles> list = new List<SysRoles>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY b.LastUpdateTime desc ) AS rownum, b.*
                                        from SysRoles b left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID                              
                                        where d.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        SysRoles model = DataReaderToModel<SysRoles>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载作用域
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownSysScope(string VID)
        {
            string strlist = "";
            List<SysScope> list = new List<SysScope>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                string strsql = "select b.* from SysScope b left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID where VID=@VID ";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        SysScope model = DataReaderToModel<SysScope>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载作用域分页
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownSysScope(string VID, int PageIndex)
        {
            string strlist = "";
            List<SysScope> list = new List<SysScope>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY b.LastUpdateTime desc ) AS rownum, b.*
                                        from SysScope b left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID                              
                                        where d.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        SysScope model = DataReaderToModel<SysScope>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载作用域对应的小区
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownSysScopeAuthorize(string VID)
        {
            string strlist = "";
            List<SysScopeAuthorize> list = new List<SysScopeAuthorize>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                string strsql = "select b.* from SysScopeAuthorize b left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID where VID=@VID ";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        SysScopeAuthorize model = DataReaderToModel<SysScopeAuthorize>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载作用域对应的小区分页
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownSysScopeAuthorize(string VID, int PageIndex)
        {
            string strlist = "";
            List<SysScopeAuthorize> list = new List<SysScopeAuthorize>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY b.LastUpdateTime desc ) AS rownum, b.*
                                        from SysScopeAuthorize b left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID                              
                                        where d.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        SysScopeAuthorize model = DataReaderToModel<SysScopeAuthorize>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载登陆用户信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownSysUser(string VID)
        {
            string strlist = "";
            List<SysUser> list = new List<SysUser>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                string strsql = "select b.* from SysUser b left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID where VID=@VID ";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        SysUser model = DataReaderToModel<SysUser>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载登陆用户信息分页
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownSysUser(string VID, int PageIndex)
        {
            string strlist = "";
            List<SysUser> list = new List<SysUser>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY b.LastUpdateTime desc ) AS rownum, b.*
                                        from SysUser b left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID                              
                                        where d.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        SysUser model = DataReaderToModel<SysUser>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载用户对应角色信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownSysUserRolesMapping(string VID)
        {
            string strlist = "";
            List<SysUserRolesMapping> list = new List<SysUserRolesMapping>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                string strsql = "select a.* from SysUserRolesMapping a left join SysRoles b on a.RoleID=b.RecordID left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID where VID=@VID ";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        SysUserRolesMapping model = DataReaderToModel<SysUserRolesMapping>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载用户对应角色信息分页
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownSysUserRolesMapping(string VID, int PageIndex)
        {
            string strlist = "";
            List<SysUserRolesMapping> list = new List<SysUserRolesMapping>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from SysUserRolesMapping a left join SysRoles b on a.RoleID=b.RecordID left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID                              
                                        where d.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        SysUserRolesMapping model = DataReaderToModel<SysUserRolesMapping>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载用户对应作用域信息
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        public string DownSysUserScopeMapping(string VID)
        {
            string strlist = "";
            List<SysUserScopeMapping> list = new List<SysUserScopeMapping>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                string strsql = "select a.* from SysUserScopeMapping a left join SysScope b on a.ASID=b.ASID left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID where VID=@VID ";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        SysUserScopeMapping model = DataReaderToModel<SysUserScopeMapping>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        /// <summary>
        /// 主动下载用户对应作用域信息分页
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DownSysUserScopeMapping(string VID, int PageIndex)
        {
            string strlist = "";
            List<SysUserScopeMapping> list = new List<SysUserScopeMapping>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from SysUserScopeMapping a left join SysScope b on a.ASID=b.ASID left join BaseCompany c on b.CPID=c.CPID left join BaseVillage d on c.CPID=d.CPID                              
                                        where d.VID=@VID)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        SysUserScopeMapping model = DataReaderToModel<SysUserScopeMapping>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }

        #region 记录信息

        /// <summary>
        /// 在场记录
        /// </summary>
        /// <param name="VID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public string DowLoadParkIORecord(string VID, int PageIndex)
        {
            string strlist = "";
            List<ParkIORecord> list = new List<ParkIORecord>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {

                string strsql = string.Format(@"SELECT * FROM (SELECT top {1} ROW_NUMBER() OVER ( ORDER BY a.LastUpdateTime desc ) AS rownum, a.*
                                        from ParkIORecord a left join BaseParkinfo b on a.ParkingID=b.PKID                              
                                        where b.VID=@VID and IsExit=0)AS temp WHERE   temp.rownum BETWEEN {0} AND {1} order by LastUpdateTime desc  ", 50 * (PageIndex - 1) + 1, 50 * PageIndex);

                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkIORecord model = DataReaderToModel<ParkIORecord>.ToModel(reader);
                        list.Add(model);
                    }
                }
            }

            if (list.Count > 0)
            {
                strlist = JsonHelper.GetJsonString(list);
            }

            return strlist;
        }
        #endregion
    }
}
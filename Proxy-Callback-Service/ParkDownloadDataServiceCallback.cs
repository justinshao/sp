using Common.Core;
using Common.DataAccess;
using Common.Entities;
using Common.Entities.BaseData;
using Common.Entities.Parking;
using Common.Entities.WX;
using Common.Utilities.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace PlatformWcfServers
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“FoundationService”。
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IParkDownloadDataCallback))]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class ParkDownloadDataServiceCallback
    {
        /// <summary>
        /// 回调集合
        /// </summary>
        public static ConcurrentDictionary<string, ProxyDownloadClient> dit_callback = new ConcurrentDictionary<string, ProxyDownloadClient>();

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
        public string GetName()
        {
            return "平台数据下发服务";
        }

        /// <summary>
        /// 代理服务登录
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <returns></returns>
        [OperationContract]
        public int Login(string ProxyNo)
        {
            try
            {
                dit_callback[ProxyNo] =
                    new ProxyDownloadClient(OperationContext.Current.GetCallbackChannel<IParkDownloadDataCallback>(), DateTime.Now, ProxyNo);
            }
            catch
            {
                return 0;
            }
            return 1;
        }

        [OperationContract]
        public bool DownloadData(string ProxyNo, string cmd, string listdata)
        {
            if (dit_callback.ContainsKey(ProxyNo))
            {
                switch (cmd)
                {
                    case "Company":
                        List<BaseCompany> companylist = JsonHelper.GetJson<List<BaseCompany>>(listdata);
                        DownloadCompany(ProxyNo, "Company", companylist);
                        break;
                    case "Village":
                        List<BaseVillage> villagelist = JsonHelper.GetJson<List<BaseVillage>>(listdata);
                        DownloadVillage(ProxyNo, "Village", villagelist);
                        break;
                    case "Parkinfo":
                        List<BaseParkinfo> parkinfolist = JsonHelper.GetJson<List<BaseParkinfo>>(listdata);
                        DownloadParkinfo(ProxyNo, "Parkinfo", parkinfolist);
                        break;
                    case "Passremark":
                        List<BasePassRemark> PKPassRemarklist = JsonHelper.GetJson<List<BasePassRemark>>(listdata);
                        DownloadPassremark(ProxyNo, "Passremark", PKPassRemarklist);
                        break;
                    case "ParkArea":
                        List<ParkArea> list = JsonHelper.GetJson<List<ParkArea>>(listdata);
                        DownloadParkArea(ProxyNo, "ParkArea", list);
                        break;
                    case "ParkBox":
                        List<ParkBox> listbox = JsonHelper.GetJson<List<ParkBox>>(listdata);
                        DownloadParkBox(ProxyNo, "ParkBox", listbox);
                        break;
                    case "PKGate":
                        List<ParkGate> listParkGate = JsonHelper.GetJson<List<ParkGate>>(listdata);
                        DownloadPKGate(ProxyNo, "PKGate", listParkGate);
                        break;
                    case "ParkDevice":
                        List<ParkDevice> listParkDevice = JsonHelper.GetJson<List<ParkDevice>>(listdata);
                        DownloadParkDevice(ProxyNo, "ParkDevice", listParkDevice);
                        break;
                    case "ParkDeviceParam":
                        List<ParkDeviceParam> listParkDeviceParam = JsonHelper.GetJson<List<ParkDeviceParam>>(listdata);
                        DownloadParkDeviceParam(ProxyNo, "ParkDeviceParam", listParkDeviceParam);
                        break;
                    case "ParkDeviceDetection":
                        List<ParkDeviceDetection> listParkDeviceDetection = JsonHelper.GetJson<List<ParkDeviceDetection>>(listdata);
                        DownloadParkDeviceDetection(ProxyNo, "ParkDeviceDetection", listParkDeviceDetection);
                        break;
                    case "ParkCarType":
                        List<ParkCarType> listParkCarType = JsonHelper.GetJson<List<ParkCarType>>(listdata);
                        DownloadParkCarType(ProxyNo, "ParkCarType", listParkCarType);
                        break;
                    case "ParkCarModel":
                        List<ParkCarModel> listParkCarModel = JsonHelper.GetJson<List<ParkCarModel>>(listdata);
                        DownloadParkCarModel(ProxyNo, "ParkCarModel", listParkCarModel);
                        break;
                    case "ParkFeeRule":
                        List<ParkFeeRule> listParkFeeRule = JsonHelper.GetJson<List<ParkFeeRule>>(listdata);
                        DownloadParkFeeRule(ProxyNo, "ParkFeeRule", listParkFeeRule);
                        break;
                    case "ParkFeeRuleDetail":
                        List<ParkFeeRuleDetail> listParkFeeRuleDetail = JsonHelper.GetJson<List<ParkFeeRuleDetail>>(listdata);
                        DownloadParkFeeRuleDetail(ProxyNo, "ParkFeeRuleDetail", listParkFeeRuleDetail);
                        break;
                    case "BaseCard":
                        List<BaseCard> listBaseCard = JsonHelper.GetJson<List<BaseCard>>(listdata);
                        DownloadBaseCard(ProxyNo, "BaseCard", listBaseCard);
                        break;
                    case "ParkGrant":
                        List<ParkGrant> listParkGrant = JsonHelper.GetJson<List<ParkGrant>>(listdata);
                        DownloadParkGrant(ProxyNo, "ParkGrant", listParkGrant);
                        break;
                    case "ParkCardSuspendPlan":
                        List<ParkCardSuspendPlan> listParkCardSuspendPlan = JsonHelper.GetJson<List<ParkCardSuspendPlan>>(listdata);
                        DownloadParkCardSuspendPlan(ProxyNo, "ParkCardSuspendPlan", listParkCardSuspendPlan);
                        break;
                    case "BaseEmployee":
                        List<BaseEmployee> listBaseEmployee = JsonHelper.GetJson<List<BaseEmployee>>(listdata);
                        DownloadBaseEmployee(ProxyNo, "BaseEmployee", listBaseEmployee);
                        break;
                    case "EmployeePlate":
                        List<EmployeePlate> listEmployeePlate = JsonHelper.GetJson<List<EmployeePlate>>(listdata);
                        DownloadEmployeePlate(ProxyNo, "EmployeePlate", listEmployeePlate);
                        break;
                    case "ParkSeller":
                        List<ParkSeller> listParkSeller = JsonHelper.GetJson<List<ParkSeller>>(listdata);
                        DownloadParkSeller(ProxyNo, "ParkSeller", listParkSeller);
                        break;
                    case "ParkDerate":
                        List<ParkDerate> listParkDerate = JsonHelper.GetJson<List<ParkDerate>>(listdata);
                        DownloadParkDerate(ProxyNo, "ParkDerate", listParkDerate);
                        break;
                    case "ParkDerateIntervar":
                        List<ParkDerateIntervar> listParkDerateIntervar = JsonHelper.GetJson<List<ParkDerateIntervar>>(listdata);
                        DownloadPKDerateintervar(ProxyNo, "ParkDerateIntervar", listParkDerateIntervar);
                        break;
                    case "ParkCarDerate":
                        List<ParkCarDerate> listParkCarDerate = JsonHelper.GetJson<List<ParkCarDerate>>(listdata);
                        DownloadParkCarDerate(ProxyNo, "ParkCarDerate", listParkCarDerate);
                        break;
                    case "ParkBlacklist":
                        List<ParkBlacklist> listParkBlacklist = JsonHelper.GetJson<List<ParkBlacklist>>(listdata);
                        DownloadParkBlack(ProxyNo, "ParkBlacklist", listParkBlacklist);
                        break;
                    case "ParkOrder":
                        List<ParkOrder> listParkOrder = JsonHelper.GetJson<List<ParkOrder>>(listdata);
                        DownloadParkOrder(ProxyNo, "ParkOrder", listParkOrder);
                        break;
                    case "VisitorInfo":
                        List<VisitorInfo> listVisitorInfo = JsonHelper.GetJson<List<VisitorInfo>>(listdata);
                        DownloadVisitorInfo(ProxyNo, "VisitorInfo", listVisitorInfo);
                        break;
                    case "ParkVisitor":
                        List<ParkVisitor> listParkVisitor = JsonHelper.GetJson<List<ParkVisitor>>(listdata);
                        DownloadParkVisitor(ProxyNo, "ParkVisitor", listParkVisitor);
                        break;
                    case "SysRoles":
                        List<SysRoles> listSysRoles = JsonHelper.GetJson<List<SysRoles>>(listdata);
                        DownloadSysRoles(ProxyNo, "SysRoles", listSysRoles);
                        break;
                    case "SysUser":
                        List<SysUser> listSysUser = JsonHelper.GetJson<List<SysUser>>(listdata);
                        DownloadSysUser(ProxyNo, "SysUser", listSysUser);
                        break;
                    case "SysUserRolesMapping":
                        List<SysUserRolesMapping> listSysUserRolesMapping = JsonHelper.GetJson<List<SysUserRolesMapping>>(listdata);
                        DownloadSysUserRolesMapping(ProxyNo, "SysUserRolesMapping", listSysUserRolesMapping);
                        break;
                    case "SysScope":
                        List<SysScope> listSysScope = JsonHelper.GetJson<List<SysScope>>(listdata);
                        DownloadSysScope(ProxyNo, "SysScope", listSysScope);
                        break;
                    case "SysScopeAuthorize":
                        List<SysScopeAuthorize> listSysScopeAuthorize = JsonHelper.GetJson<List<SysScopeAuthorize>>(listdata);
                        DownloadSysScopeAuthorize(ProxyNo, "SysScopeAuthorize", listSysScopeAuthorize);
                        break;
                    case "SysUserScopeMapping":
                        List<SysUserScopeMapping> listSysUserScopeMapping = JsonHelper.GetJson<List<SysUserScopeMapping>>(listdata);
                        DownloadSysUserScopeMapping(ProxyNo, "SysUserScopeMapping", listSysUserScopeMapping);
                        break;
                    case "SysRoleAuthorize":
                        List<SysRoleAuthorize> listSysRoleAuthorize = JsonHelper.GetJson<List<SysRoleAuthorize>>(listdata);
                        DownloadSysRoleAuthorize(ProxyNo, "SysRoleAuthorize", listSysRoleAuthorize);
                        break;
                    case "ParkReserveBit":
                        List<ParkReserveBit> listParkReserveBit = JsonHelper.GetJson<List<ParkReserveBit>>(listdata);
                        DownloadParkReserveBit(ProxyNo, "ParkReserveBit", listParkReserveBit);
                        break;
                    case "ParkCarBitGroup":
                        List<ParkCarBitGroup> listParkCarBitGroup = JsonHelper.GetJson<List<ParkCarBitGroup>>(listdata);
                        DownloadParkCarBitGroup(ProxyNo, "ParkCarBitGroup", listParkCarBitGroup);
                        break;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool DownloadDataHelper(string ProxyNo, string cmd, string listdata)
        {
            return new ParkDownloadDataServiceCallback().DownloadData(ProxyNo, cmd, listdata);
        }

        #region 基础信息
        /// <summary> 
        /// 下载公司信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="parkinfolist"></param>
        /// <returns></returns>
        private static void DownloadCompany(string ProxyNo, string cmd, List<BaseCompany> companylist)
        {
            List<BaseCompany> listtemp = (from a in companylist where (a.HaveUpdate2 & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"delete from UploadDataState where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='basecompany'");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载小区信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="villagelist"></param>
        /// <returns></returns>
        private static void DownloadVillage(string ProxyNo, string cmd, List<BaseVillage> villagelist)
        {
            List<BaseVillage> listtemp = (from a in villagelist where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update BaseVillage set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载车场信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        private static void DownloadParkinfo(string ProxyNo, string cmd, List<BaseParkinfo> parkinfolist)
        {
            List<BaseParkinfo> listtemp = (from a in parkinfolist where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update BaseParkinfo set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载车场放行描述
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="PKPassRemarklist"></param>
        /// <returns></returns>
        private static void DownloadPassremark(string ProxyNo, string cmd, List<BasePassRemark> PKPassRemarklist)
        {
            List<BasePassRemark> listtemp = (from a in PKPassRemarklist where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update BasePassRemark set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        #endregion

        #region 车场信息
        /// <summary>
        /// 下载区域信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        private static void DownloadParkArea(string ProxyNo, string cmd, List<ParkArea> list)
        {
            List<ParkArea> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkArea set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载岗亭信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        private static void DownloadParkBox(string ProxyNo, string cmd, List<ParkBox> list)
        {
            List<ParkBox> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkBox set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }
        
        /// <summary>
        /// 下载通道信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        private static void DownloadPKGate(string ProxyNo, string cmd, List<ParkGate> list)
        {
            List<ParkGate> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkGate set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载设备信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        private static void DownloadParkDevice(string ProxyNo, string cmd, List<ParkDevice> list)
        {
            List<ParkDevice> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkDevice set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载设备参数信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="cmd"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static void DownloadParkDeviceParam(string ProxyNo, string cmd, List<ParkDeviceParam> list)
        {
            List<ParkDeviceParam> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkDeviceParam set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        private static void DownloadParkDeviceDetection(string ProxyNo, string cmd, List<ParkDeviceDetection> list)
        {
            List<ParkDeviceDetection> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkDeviceDetection set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载车类信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        private static void DownloadParkCarType(string ProxyNo, string cmd, List<ParkCarType> list)
        {
            List<ParkCarType> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkCarType set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载车类型信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <param name="AgencyName"></param>
        /// <returns></returns>
        private static void DownloadParkCarModel(string ProxyNo, string cmd, List<ParkCarModel> list)
        {
            List<ParkCarModel> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkCarModel set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载收费规则信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <param name="AgencyName"></param>
        /// <returns></returns>
        private static void DownloadParkFeeRule(string ProxyNo, string cmd, List<ParkFeeRule> list)
        {
            List<ParkFeeRule> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkFeeRule set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载收费规则明细信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <param name="AgencyName"></param>
        /// <returns></returns>
        private static void DownloadParkFeeRuleDetail(string ProxyNo, string cmd, List<ParkFeeRuleDetail> list)
        {
            List<ParkFeeRuleDetail> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkFeeRuleDetail set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载用户卡片信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        private static void DownloadBaseCard(string ProxyNo, string cmd, List<BaseCard> list)
        {
            List<BaseCard> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update BaseCard set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载车场授权信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static void DownloadParkGrant(string ProxyNo, string cmd, List<ParkGrant> list)
        {
            List<ParkGrant> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkGrant set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载车场授权暂停信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static void DownloadParkCardSuspendPlan(string ProxyNo, string cmd, List<ParkCardSuspendPlan> list)
        {
            List<ParkCardSuspendPlan> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkCardSuspendPlan set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载人员信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static void DownloadBaseEmployee(string ProxyNo, string cmd, List<BaseEmployee> list)
        {
            List<BaseEmployee> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update BaseEmployee set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载业主车牌信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static void DownloadEmployeePlate(string ProxyNo, string cmd, List<EmployeePlate> list)
        {
            List<EmployeePlate> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update EmployeePlate set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载商家信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static void DownloadParkSeller(string ProxyNo, string cmd, List<ParkSeller> list)
        {
            List<ParkSeller> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkSeller set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载商家优免信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static void DownloadParkDerate(string ProxyNo, string cmd, List<ParkDerate> list)
        {
            List<ParkDerate> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkDerate set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载优免时段
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static void DownloadPKDerateintervar(string ProxyNo, string cmd, List<ParkDerateIntervar> list)
        {
            List<ParkDerateIntervar> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkDerateIntervar set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载商家优免券
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static void DownloadParkCarDerate(string ProxyNo, string cmd, List<ParkCarDerate> list)
        {
            List<ParkCarDerate> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkCarDerate set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载黑名单
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static void DownloadParkBlack(string ProxyNo, string cmd, List<ParkBlacklist> list)
        {
            List<ParkBlacklist> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkBlacklist set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载订单信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static void DownloadParkOrder(string ProxyNo, string cmd, List<ParkOrder> list)
        {
            List<ParkOrder> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkOrder set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载车位组信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static void DownloadParkReserveBit(string ProxyNo, string cmd, List<ParkReserveBit> list)
        {
            List<ParkReserveBit> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkReserveBit set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        private static void DownloadParkCarBitGroup(string ProxyNo, string cmd, List<ParkCarBitGroup> list)
        {
            List<ParkCarBitGroup> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkCarBitGroup set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        #endregion

        #region 访客信息
        /// <summary>
        /// 下载访客信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        private static void DownloadVisitorInfo(string ProxyNo, string cmd, List<VisitorInfo> visitorinfolist)
        {
            List<VisitorInfo> listtemp = (from a in visitorinfolist where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update VisitorInfo set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载访客车辆信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="parkvisitorlist"></param>
        /// <returns></returns>
        private static void DownloadParkVisitor(string ProxyNo, string cmd, List<ParkVisitor> parkvisitorlist)
        {
            List<ParkVisitor> listtemp = (from a in parkvisitorlist where (a.HaveUpdate & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"update ParkVisitor set HaveUpdate=0 where ID=@ID");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ID", obj.ID);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }
        #endregion

        #region 权限角色

        /// <summary>
        /// 下载角色
        /// </summary>
        /// <param name="SmRoleslist"></param>
        /// <returns></returns>
        private static void DownloadSysRoles(string ProxyNo, string cmd, List<SysRoles> list)
        {
            List<SysRoles> listtemp = (from a in list where (a.HaveUpdate2 & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"delete from UploadDataState  where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='sysroles'");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载用户信息
        /// </summary>
        /// <param name="SmUserslist"></param>
        /// <returns></returns>
        private static void DownloadSysUser(string ProxyNo, string cmd, List<SysUser> list)
        {
            List<SysUser> listtemp = (from a in list where (a.HaveUpdate2 & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"delete from UploadDataState where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='sysuser'");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载用户拥有的角色
        /// </summary>
        /// <param name="SmUserrolelist"></param>
        /// <returns></returns>
        private static void DownloadSysUserRolesMapping(string ProxyNo, string cmd, List<SysUserRolesMapping> list)
        {
            List<SysUserRolesMapping> listtemp = (from a in list where (a.HaveUpdate2 & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"delete from UploadDataState where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='sysuserrolesmapping'");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载作用域
        /// </summary>
        /// <param name="ActionScopelist"></param>
        /// <returns></returns>
        private static void DownloadSysScope(string ProxyNo, string cmd, List<SysScope> list)
        {
            List<SysScope> listtemp = (from a in list where (a.HaveUpdate2 & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"delete from UploadDataState where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='sysscope'");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载作用域对应的场所
        /// </summary>
        /// <param name="ActionScopeDatalist"></param>
        /// <returns></returns>
        private static void DownloadSysScopeAuthorize(string ProxyNo, string cmd, List<SysScopeAuthorize> list)
        {
            List<SysScopeAuthorize> listtemp = (from a in list where (a.HaveUpdate2 & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"delete from UploadDataState where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='sysscopeauthorize'");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载用户对应的作用域
        /// </summary>
        /// <param name="ActionScopeDatalist"></param>
        /// <returns></returns>
        private static void DownloadSysUserScopeMapping(string ProxyNo, string cmd, List<SysUserScopeMapping> list)
        {
            List<SysUserScopeMapping> listtemp = (from a in list where (a.HaveUpdate2 & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"delete from UploadDataState where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='sysuserscopemapping'");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }

        /// <summary>
        /// 下载角色拥有的权限
        /// </summary>
        /// <param name="SmRoleRightlist"></param>
        /// <returns></returns>
        private static void DownloadSysRoleAuthorize(string ProxyNo, string cmd, List<SysRoleAuthorize> list)
        {
            List<SysRoleAuthorize> listtemp = (from a in list where (a.HaveUpdate2 & 1) == 1 select a).ToList();
            if (listtemp.Count > 0)
            {
                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in result)
                    {
                        string sql = string.Format(@"delete from UploadDataState where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='sysroleauthorize'");
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                }
            }
        }
        #endregion
        
        #region 心跳
        
        public static void StartListenClients()
        {
            //timer1 = new System.Timers.Timer();
            //timer1.Interval = 2000;
            //timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
            //timer1.Start();

        }

        /// <summary>
        /// 车场代理主动断开连接
        /// </summary>
        /// <param name="AgencyName"></param>
        [OperationContract(IsOneWay = true)]
        public void Leave(string ProxyNo)
        {
            ProxyDownloadClient client = null;

            dit_callback.TryRemove(ProxyNo, out client);
        }

        /// <summary>
        /// 实时心跳
        /// </summary>
        /// <param name="AgencyName"></param>
        [OperationContract(IsOneWay = true)]
        public void Update(string ProxyNo)
        {
            ProxyDownloadClient client = null;

            if (dit_callback.TryGetValue(ProxyNo, out client))
            {
                client.updatetime = DateTime.Now;
                client.callback = OperationContext.Current.GetCallbackChannel<IParkDownloadDataCallback>();
            }
            else
            {
                dit_callback[ProxyNo] =
                    new ProxyDownloadClient(OperationContext.Current.GetCallbackChannel<IParkDownloadDataCallback>(), DateTime.Now, ProxyNo);
            }
        }

        #endregion
    }

    public class ProxyDownloadClient
    {
        public IParkDownloadDataCallback callback;
        public DateTime updatetime;
        public string VillageID;
        public ProxyDownloadClient(IParkDownloadDataCallback callback, DateTime updatetime, string VillageID)
        {
            this.callback = callback;
            this.updatetime = updatetime;
            this.VillageID = VillageID;


        }
    }
}

using Common.Core;
using Common.DataAccess;
using Common.Entities;
using Common.Entities.BaseData;
using Common.Entities.Parking;
using Common.Entities.WX;
using Common.Utilities.Helpers;
using System;
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
        /// 日志记录对象
        /// </summary>
        protected Logger logger = LogManager.GetLogger("下载平台信息");

        /// <summary>
        /// 定义一个静态对象用于线程部份代码块的锁定，用于lock操作
        /// </summary>
        private static Object syncObj = new Object();

        /// <summary>
        /// 回调集合
        /// </summary>
        public static Dictionary<string, ProxyDownloadClient> dit_callback = new Dictionary<string, ProxyDownloadClient>();

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
        //    return "数据下载回调服务";
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
                    dit_callback.Add(ProxyNo, new ProxyDownloadClient(OperationContext.Current.GetCallbackChannel<IParkDownloadDataCallback>(), DateTime.Now, ProxyNo));

                }
                catch
                {
                    return 0;
                }
                return 1;

            }
        }

        [OperationContract]
        public bool DownloadData(string ProxyNo, string cmd, string listdata)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo))
                {
                    try
                    {
                        switch (cmd)
                        {
                            case "Company":
                                List<BaseCompany> companylist = JsonHelper.GetJson<List<BaseCompany>>(listdata);
                                return DownloadCompany(ProxyNo, "Company", companylist);

                            case "Village":
                                List<BaseVillage> villagelist = JsonHelper.GetJson<List<BaseVillage>>(listdata);
                                return DownloadVillage(ProxyNo, "Village", villagelist);

                            case "Parkinfo":
                                List<BaseParkinfo> parkinfolist = JsonHelper.GetJson<List<BaseParkinfo>>(listdata);
                                return DownloadParkinfo(ProxyNo, "Parkinfo", parkinfolist);

                            case "Passremark":
                                List<BasePassRemark> PKPassRemarklist = JsonHelper.GetJson<List<BasePassRemark>>(listdata);
                                return DownloadPassremark(ProxyNo, "Passremark", PKPassRemarklist);

                            case "ParkArea":
                                List<ParkArea> list = JsonHelper.GetJson<List<ParkArea>>(listdata);
                                return DownloadParkArea(ProxyNo, "ParkArea", list);

                            case "ParkBox":
                                List<ParkBox> listbox = JsonHelper.GetJson<List<ParkBox>>(listdata);
                                return DownloadParkBox(ProxyNo, "ParkBox", listbox);

                            case "PKGate":
                                List<ParkGate> listParkGate = JsonHelper.GetJson<List<ParkGate>>(listdata);
                                return DownloadPKGate(ProxyNo, "PKGate", listParkGate);

                            case "ParkDevice":
                                List<ParkDevice> listParkDevice = JsonHelper.GetJson<List<ParkDevice>>(listdata);
                                return DownloadParkDevice(ProxyNo, "ParkDevice", listParkDevice);
                            case "ParkDeviceParam":
                                List<ParkDeviceParam> listParkDeviceParam = JsonHelper.GetJson<List<ParkDeviceParam>>(listdata);
                                return DownloadParkDeviceParam(ProxyNo, "ParkDeviceParam", listParkDeviceParam);
                            case "ParkDeviceDetection":
                                List<ParkDeviceDetection> listParkDeviceDetection = JsonHelper.GetJson<List<ParkDeviceDetection>>(listdata);
                                return DownloadParkDeviceDetection(ProxyNo, "ParkDeviceDetection", listParkDeviceDetection);

                            case "ParkCarType":
                                List<ParkCarType> listParkCarType = JsonHelper.GetJson<List<ParkCarType>>(listdata);
                                return DownloadParkCarType(ProxyNo, "ParkCarType", listParkCarType);

                            case "ParkCarModel":
                                List<ParkCarModel> listParkCarModel = JsonHelper.GetJson<List<ParkCarModel>>(listdata);
                                return DownloadParkCarModel(ProxyNo, "ParkCarModel", listParkCarModel);

                            case "ParkFeeRule":
                                List<ParkFeeRule> listParkFeeRule = JsonHelper.GetJson<List<ParkFeeRule>>(listdata);
                                return DownloadParkFeeRule(ProxyNo, "ParkFeeRule", listParkFeeRule);

                            case "ParkFeeRuleDetail":
                                List<ParkFeeRuleDetail> listParkFeeRuleDetail = JsonHelper.GetJson<List<ParkFeeRuleDetail>>(listdata);
                                return DownloadParkFeeRuleDetail(ProxyNo, "ParkFeeRuleDetail", listParkFeeRuleDetail);

                            case "BaseCard":
                                List<BaseCard> listBaseCard = JsonHelper.GetJson<List<BaseCard>>(listdata);
                                return DownloadBaseCard(ProxyNo, "BaseCard", listBaseCard);

                            case "ParkGrant":
                                List<ParkGrant> listParkGrant = JsonHelper.GetJson<List<ParkGrant>>(listdata);
                                return DownloadParkGrant(ProxyNo, "ParkGrant", listParkGrant);

                            case "ParkCardSuspendPlan":
                                List<ParkCardSuspendPlan> listParkCardSuspendPlan = JsonHelper.GetJson<List<ParkCardSuspendPlan>>(listdata);
                                return DownloadParkCardSuspendPlan(ProxyNo, "ParkCardSuspendPlan", listParkCardSuspendPlan);

                            case "BaseEmployee":
                                List<BaseEmployee> listBaseEmployee = JsonHelper.GetJson<List<BaseEmployee>>(listdata);
                                return DownloadBaseEmployee(ProxyNo, "BaseEmployee", listBaseEmployee);

                            case "EmployeePlate":
                                List<EmployeePlate> listEmployeePlate = JsonHelper.GetJson<List<EmployeePlate>>(listdata);
                                return DownloadEmployeePlate(ProxyNo, "EmployeePlate", listEmployeePlate);

                            case "ParkSeller":
                                List<ParkSeller> listParkSeller = JsonHelper.GetJson<List<ParkSeller>>(listdata);
                                return DownloadParkSeller(ProxyNo, "ParkSeller", listParkSeller);

                            case "ParkDerate":
                                List<ParkDerate> listParkDerate = JsonHelper.GetJson<List<ParkDerate>>(listdata);
                                return DownloadParkDerate(ProxyNo, "ParkDerate", listParkDerate);

                            case "ParkDerateIntervar":
                                List<ParkDerateIntervar> listParkDerateIntervar = JsonHelper.GetJson<List<ParkDerateIntervar>>(listdata);
                                return DownloadPKDerateintervar(ProxyNo, "ParkDerateIntervar", listParkDerateIntervar);

                            case "ParkCarDerate":
                                List<ParkCarDerate> listParkCarDerate = JsonHelper.GetJson<List<ParkCarDerate>>(listdata);
                                return DownloadParkCarDerate(ProxyNo, "ParkCarDerate", listParkCarDerate);

                            case "ParkBlacklist":
                                List<ParkBlacklist> listParkBlacklist = JsonHelper.GetJson<List<ParkBlacklist>>(listdata);
                                return DownloadParkBlack(ProxyNo, "ParkBlacklist", listParkBlacklist);

                            case "ParkOrder":
                                List<ParkOrder> listParkOrder = JsonHelper.GetJson<List<ParkOrder>>(listdata);
                                return DownloadParkOrder(ProxyNo, "ParkOrder", listParkOrder);

                            case "VisitorInfo":
                                List<VisitorInfo> listVisitorInfo = JsonHelper.GetJson<List<VisitorInfo>>(listdata);
                                return DownloadVisitorInfo(ProxyNo, "VisitorInfo", listVisitorInfo);

                            case "ParkVisitor":
                                List<ParkVisitor> listParkVisitor = JsonHelper.GetJson<List<ParkVisitor>>(listdata);
                                return DownloadParkVisitor(ProxyNo, "ParkVisitor", listParkVisitor);

                            case "SysRoles":
                                List<SysRoles> listSysRoles = JsonHelper.GetJson<List<SysRoles>>(listdata);
                                return DownloadSysRoles(ProxyNo, "SysRoles", listSysRoles);

                            case "SysUser":
                                List<SysUser> listSysUser = JsonHelper.GetJson<List<SysUser>>(listdata);
                                return DownloadSysUser(ProxyNo, "SysUser", listSysUser);

                            case "SysUserRolesMapping":
                                List<SysUserRolesMapping> listSysUserRolesMapping = JsonHelper.GetJson<List<SysUserRolesMapping>>(listdata);
                                return DownloadSysUserRolesMapping(ProxyNo, "SysUserRolesMapping", listSysUserRolesMapping);

                            case "SysScope":
                                List<SysScope> listSysScope = JsonHelper.GetJson<List<SysScope>>(listdata);
                                return DownloadSysScope(ProxyNo, "SysScope", listSysScope);

                            case "SysScopeAuthorize":
                                List<SysScopeAuthorize> listSysScopeAuthorize = JsonHelper.GetJson<List<SysScopeAuthorize>>(listdata);
                                return DownloadSysScopeAuthorize(ProxyNo, "SysScopeAuthorize", listSysScopeAuthorize);

                            case "SysUserScopeMapping":
                                List<SysUserScopeMapping> listSysUserScopeMapping = JsonHelper.GetJson<List<SysUserScopeMapping>>(listdata);
                                return DownloadSysUserScopeMapping(ProxyNo, "SysUserScopeMapping", listSysUserScopeMapping);

                            case "SysRoleAuthorize":
                                List<SysRoleAuthorize> listSysRoleAuthorize = JsonHelper.GetJson<List<SysRoleAuthorize>>(listdata);
                                return DownloadSysRoleAuthorize(ProxyNo, "SysRoleAuthorize", listSysRoleAuthorize);

                            case "ParkReserveBit":
                                List<ParkReserveBit> listParkReserveBit = JsonHelper.GetJson<List<ParkReserveBit>>(listdata);
                                return DownloadParkReserveBit(ProxyNo, "ParkReserveBit", listParkReserveBit);

                            case "ParkCarBitGroup":
                                List<ParkCarBitGroup> listParkCarBitGroup = JsonHelper.GetJson<List<ParkCarBitGroup>>(listdata);
                                return DownloadParkCarBitGroup(ProxyNo, "ParkCarBitGroup", listParkCarBitGroup);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return false;
        }

        #region 基础信息
        /// <summary> 
        /// 下载公司信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="parkinfolist"></param>
        /// <returns></returns>
        public bool DownloadCompany(string ProxyNo, string cmd, List<BaseCompany> companylist)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && companylist.Count > 0)
                {
                    try
                    {
                        List<BaseCompany> listtemp = (from a in companylist where (a.HaveUpdate2 & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"delete from UploadDataState where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='basecompany'");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("RecordID", obj.RecordID);
                                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载公司信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载小区信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="villagelist"></param>
        /// <returns></returns>
        public bool DownloadVillage(string ProxyNo, string cmd, List<BaseVillage> villagelist)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && villagelist.Count > 0)
                {
                    try
                    {
                        List<BaseVillage> listtemp = (from a in villagelist where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update BaseVillage set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载小区信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载车场信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        public bool DownloadParkinfo(string ProxyNo, string cmd, List<BaseParkinfo> parkinfolist)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && dit_callback[ProxyNo] != null && parkinfolist.Count > 0)
                {

                    try
                    {
                        List<BaseParkinfo> listtemp = (from a in parkinfolist where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            string aaaa = JsonHelper.GetJsonString(listtemp);
                            try
                            {

                                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                                foreach (var obj in result)
                                {
                                    try
                                    {
                                        using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                        {
                                            string sql = string.Format(@"update BaseParkinfo set HaveUpdate=0 where ID=@ID");
                                            dbOperator.ClearParameters();
                                            dbOperator.AddParameter("ID", obj.ID);
                                            dbOperator.ExecuteNonQuery(sql);
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            catch (Exception eeee)
                            {
                                logger.Fatal("下载车场信息失败22222:" + aaaa);
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载车场信息失败:" + ex);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载车场放行描述
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="PKPassRemarklist"></param>
        /// <returns></returns>
        public bool DownloadPassremark(string ProxyNo, string cmd, List<BasePassRemark> PKPassRemarklist)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && PKPassRemarklist.Count > 0)
                {
                    try
                    {
                        List<BasePassRemark> listtemp = (from a in PKPassRemarklist where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update BasePassRemark set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载车场放行描述" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
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
        public bool DownloadParkArea(string ProxyNo, string cmd, List<ParkArea> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count > 0)
                {
                    try
                    {
                        List<ParkArea> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkArea set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载区域信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载岗亭信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        public bool DownloadParkBox(string ProxyNo, string cmd, List<ParkBox> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count > 0)
                {
                    try
                    {
                        List<ParkBox> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkBox set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载岗亭信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// 下载通道信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        public bool DownloadPKGate(string ProxyNo, string cmd, List<ParkGate> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count > 0)
                {
                    try
                    {
                        List<ParkGate> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {

                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkGate set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载通道信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载设备信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        public bool DownloadParkDevice(string ProxyNo, string cmd, List<ParkDevice> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count > 0)
                {
                    try
                    {
                        List<ParkDevice> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkDevice set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载设备信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
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
        public bool DownloadParkDeviceParam(string ProxyNo, string cmd, List<ParkDeviceParam> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count > 0)
                {
                    try
                    {
                        List<ParkDeviceParam> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkDeviceParam set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载设备信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DownloadParkDeviceDetection(string ProxyNo, string cmd, List<ParkDeviceDetection> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count > 0)
                {
                    try
                    {
                        List<ParkDeviceDetection> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkDeviceDetection set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载设备连接信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载车类信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        public bool DownloadParkCarType(string ProxyNo, string cmd, List<ParkCarType> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count > 0)
                {
                    try
                    {
                        List<ParkCarType> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {

                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkCarType set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载车类信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
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
        public bool DownloadParkCarModel(string ProxyNo, string cmd, List<ParkCarModel> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count > 0)
                {
                    try
                    {
                        List<ParkCarModel> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkCarModel set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载车类型信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
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
        public bool DownloadParkFeeRule(string ProxyNo, string cmd, List<ParkFeeRule> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count > 0)
                {
                    try
                    {
                        List<ParkFeeRule> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkFeeRule set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载收费规则信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
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
        public bool DownloadParkFeeRuleDetail(string ProxyNo, string cmd, List<ParkFeeRuleDetail> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count > 0)
                {
                    try
                    {
                        List<ParkFeeRuleDetail> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkFeeRuleDetail set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载收费规则明细信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
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
        public bool DownloadBaseCard(string ProxyNo, string cmd, List<BaseCard> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<BaseCard> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update BaseCard set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载用户卡片信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载车场授权信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DownloadParkGrant(string ProxyNo, string cmd, List<ParkGrant> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<ParkGrant> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        try
                        {
                            if (listtemp.Count > 0)
                            {
                                List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                                foreach (var obj in result)
                                {
                                    try
                                    {
                                        using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                        {
                                            string sql = string.Format(@"update ParkGrant set HaveUpdate=0 where ID=@ID");
                                            dbOperator.ClearParameters();
                                            dbOperator.AddParameter("ID", obj.ID);
                                            dbOperator.ExecuteNonQuery(sql);
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            return true;
                        }
                        catch
                        {
                            logger.Fatal("下载车场授权信息失败" + JsonHelper.GetJsonString(listtemp) + "\r\n");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载车场授权信息" + ex + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载车场授权暂停信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DownloadParkCardSuspendPlan(string ProxyNo, string cmd, List<ParkCardSuspendPlan> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<ParkCardSuspendPlan> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkCardSuspendPlan set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载车场授权暂停信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载人员信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DownloadBaseEmployee(string ProxyNo, string cmd, List<BaseEmployee> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<BaseEmployee> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update BaseEmployee set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载人员信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载业主车牌信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DownloadEmployeePlate(string ProxyNo, string cmd, List<EmployeePlate> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<EmployeePlate> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update EmployeePlate set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载业主车牌信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载商家信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DownloadParkSeller(string ProxyNo, string cmd, List<ParkSeller> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<ParkSeller> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkSeller set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载商家信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载商家优免信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DownloadParkDerate(string ProxyNo, string cmd, List<ParkDerate> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<ParkDerate> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {

                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkDerate set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载商家优免信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载优免时段
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DownloadPKDerateintervar(string ProxyNo, string cmd, List<ParkDerateIntervar> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<ParkDerateIntervar> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {

                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkDerateIntervar set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载优免时段" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载商家优免券
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DownloadParkCarDerate(string ProxyNo, string cmd, List<ParkCarDerate> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<ParkCarDerate> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {

                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkCarDerate set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载商家优免券" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载黑名单
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DownloadParkBlack(string ProxyNo, string cmd, List<ParkBlacklist> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<ParkBlacklist> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {

                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkBlacklist set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载黑名单" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载订单信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DownloadParkOrder(string ProxyNo, string cmd, List<ParkOrder> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<ParkOrder> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkOrder set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载订单信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载车位组信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DownloadParkReserveBit(string ProxyNo, string cmd, List<ParkReserveBit> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<ParkReserveBit> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkReserveBit set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载车位预定信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DownloadParkCarBitGroup(string ProxyNo, string cmd, List<ParkCarBitGroup> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<ParkCarBitGroup> listtemp = (from a in list where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkCarBitGroup set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载车位组信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
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
        public bool DownloadVisitorInfo(string ProxyNo, string cmd, List<VisitorInfo> visitorinfolist)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && visitorinfolist.Count > 0)
                {
                    try
                    {
                        List<VisitorInfo> listtemp = (from a in visitorinfolist where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update VisitorInfo set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载访客信息失败:" + ex.Message);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载访客车辆信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="parkvisitorlist"></param>
        /// <returns></returns>
        public bool DownloadParkVisitor(string ProxyNo, string cmd, List<ParkVisitor> parkvisitorlist)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && parkvisitorlist.Count > 0)
                {
                    try
                    {
                        List<ParkVisitor> listtemp = (from a in parkvisitorlist where (a.HaveUpdate & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"update ParkVisitor set HaveUpdate=0 where ID=@ID");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("ID", obj.ID);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载访客车辆信息失败:" + ex.Message);
                        return false;
                    }
                }
                else
                {
                    return false;
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
        public bool DownloadSysRoles(string ProxyNo, string cmd, List<SysRoles> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<SysRoles> listtemp = (from a in list where (a.HaveUpdate2 & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"delete from UploadDataState  where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='sysroles'");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("RecordID", obj.RecordID);
                                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载角色" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载用户信息
        /// </summary>
        /// <param name="SmUserslist"></param>
        /// <returns></returns>
        public bool DownloadSysUser(string ProxyNo, string cmd, List<SysUser> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<SysUser> listtemp = (from a in list where (a.HaveUpdate2 & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"delete from UploadDataState where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='sysuser'");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("RecordID", obj.RecordID);
                                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载用户信息" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载用户拥有的角色
        /// </summary>
        /// <param name="SmUserrolelist"></param>
        /// <returns></returns>
        public bool DownloadSysUserRolesMapping(string ProxyNo, string cmd, List<SysUserRolesMapping> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<SysUserRolesMapping> listtemp = (from a in list where (a.HaveUpdate2 & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"delete from UploadDataState where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='sysuserrolesmapping'");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("RecordID", obj.RecordID);
                                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载用户拥有的角色" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

        }

        /// <summary>
        /// 下载作用域
        /// </summary>
        /// <param name="ActionScopelist"></param>
        /// <returns></returns>
        public bool DownloadSysScope(string ProxyNo, string cmd, List<SysScope> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<SysScope> listtemp = (from a in list where (a.HaveUpdate2 & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {

                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"delete from UploadDataState where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='sysscope'");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("RecordID", obj.RecordID);
                                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载作用域" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

        }

        /// <summary>
        /// 下载作用域对应的场所
        /// </summary>
        /// <param name="ActionScopeDatalist"></param>
        /// <returns></returns>
        public bool DownloadSysScopeAuthorize(string ProxyNo, string cmd, List<SysScopeAuthorize> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<SysScopeAuthorize> listtemp = (from a in list where (a.HaveUpdate2 & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"delete from UploadDataState where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='sysscopeauthorize'");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("RecordID", obj.RecordID);
                                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载作用域对应的场所" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 下载用户对应的作用域
        /// </summary>
        /// <param name="ActionScopeDatalist"></param>
        /// <returns></returns>
        public bool DownloadSysUserScopeMapping(string ProxyNo, string cmd, List<SysUserScopeMapping> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<SysUserScopeMapping> listtemp = (from a in list where (a.HaveUpdate2 & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {

                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"delete from UploadDataState where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='sysuserscopemapping'");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("RecordID", obj.RecordID);
                                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }

                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载用户对应的作用域" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

        }

        /// <summary>
        /// 下载角色拥有的权限
        /// </summary>
        /// <param name="SmRoleRightlist"></param>
        /// <returns></returns>
        public bool DownloadSysRoleAuthorize(string ProxyNo, string cmd, List<SysRoleAuthorize> list)
        {
            lock (syncObj)
            {
                if (dit_callback.ContainsKey(ProxyNo) && list.Count() > 0)
                {
                    try
                    {
                        List<SysRoleAuthorize> listtemp = (from a in list where (a.HaveUpdate2 & 1) == 1 select a).ToList();
                        if (listtemp.Count > 0)
                        {
                            List<UploadResult> result = dit_callback[ProxyNo].callback.DownloadDataCallBack(cmd, JsonHelper.GetJsonString(listtemp));
                            foreach (var obj in result)
                            {
                                try
                                {
                                    using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                                    {
                                        string sql = string.Format(@"delete from UploadDataState where RecordID=@RecordID and ProxyNo=@ProxyNo and TableName='sysroleauthorize'");
                                        dbOperator.ClearParameters();
                                        dbOperator.AddParameter("RecordID", obj.RecordID);
                                        dbOperator.AddParameter("ProxyNo", ProxyNo);
                                        dbOperator.ExecuteNonQuery(sql);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("下载角色拥有的权限" + ex.Message + "\r\n");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
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
                    dit_callback.Remove(ProxyNo);
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
                    dit_callback.Add(ProxyNo, new ProxyDownloadClient(OperationContext.Current.GetCallbackChannel<IParkDownloadDataCallback>(), DateTime.Now, ProxyNo));
                }
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

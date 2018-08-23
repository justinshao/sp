using System.ServiceModel;

namespace PlatformWcfServers
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“FoundationService”。
    [ServiceContract]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public class ParkDownloadDataService
    {
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
        //public string GetName()
        //{
        //    return "数据下载服务";
        //}


        #region 基础信息
        /// <summary> 
        /// 下载公司信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="parkinfolist"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadCompany(string ProxyNo, string companylist)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "Company", companylist);
        }

        /// <summary>
        /// 下载小区信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="villagelist"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadVillage(string ProxyNo, string villagelist)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "Village", villagelist);
        }

        /// <summary>
        /// 下载车场信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkinfo(string ProxyNo, string parkinfolist)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "Parkinfo", parkinfolist);
        }

        /// <summary>
        /// 下载车场放行描述
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="PKPassRemarklist"></param>
        /// <returns></returns>
        public bool DownloadPassremark(string ProxyNo, string PKPassRemarklist)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "Passremark", PKPassRemarklist);
        }

        #endregion

        #region 车场信息
        /// <summary>
        /// 下载区域信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkArea(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkArea", list);
        }

        /// <summary>
        /// 下载岗亭信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkBox(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkBox", list);
        }


        /// <summary>
        /// 下载通道信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadPKGate(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "PKGate", list);
        }

        /// <summary>
        /// 下载设备信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkDevice(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkDevice", list);
        }
        [OperationContract]
        public bool ParkDeviceParamDownload(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkDeviceParamDownload", list);
        }

        [OperationContract]
        public bool DownloadParkDeviceDetection(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkDeviceDetection", list);
        }

        /// <summary>
        /// 下载车类信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkCarType(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkCarType", list);
        }

        /// <summary>
        /// 下载车类型信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <param name="AgencyName"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkCarModel(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkCarModel", list);
        }

        /// <summary>
        /// 下载收费规则信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <param name="AgencyName"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkFeeRule(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkFeeRule", list);
        }

        /// <summary>
        /// 下载收费规则明细信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <param name="AgencyName"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkFeeRuleDetail(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkFeeRuleDetail", list);
        }

        /// <summary>
        /// 下载用户卡片信息
        /// </summary>
        /// <param name="ParkingID"></param>
        /// <param name="SystemID"></param>
        /// <param name="ProxyNo"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadBaseCard(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "BaseCard", list);
        }

        /// <summary>
        /// 下载车场授权信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkGrant(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkGrant", list);
        }

        /// <summary>
        /// 下载车场授权暂停信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkCardSuspendPlan(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkCardSuspendPlan", list);
        }
        /// <summary>
        /// 下载人员信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadBaseEmployee(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "BaseEmployee", list);
        }

        /// <summary>
        /// 下载业主车牌信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadEmployeePlate(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "EmployeePlate", list);
        }

        /// <summary>
        /// 下载商家信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkSeller(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkSeller", list);
        }

        /// <summary>
        /// 下载商家优免信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkDerate(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkDerate", list);
        }

        /// <summary>
        /// 下载优免时段
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadPKDerateintervar(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkDerateIntervar", list);
        }

        /// <summary>
        /// 下载商家优免券
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkCarDerate(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkCarDerate", list);
        }

        /// <summary>
        /// 下载黑名单
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkBlack(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkBlacklist", list);
        }

        /// <summary>
        /// 下载订单信息
        /// </summary>
        /// <param name="AgencyName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkOrder(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkOrder", list);
        }

        /// <summary>
        /// 下载车位预定
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkReserveBit(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkReserveBit", list);
        }

        /// <summary>
        /// 下载车位组信息
        /// </summary>
        /// <param name="ProxyNo"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkCarBitGroup(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkCarBitGroup", list);
        }
        #endregion

        #region 访客信息

        /// <summary>
        /// 下载访客信息
        /// </summary>
        /// <param name="SmRoleslist"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadVisitorInfo(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "VisitorInfo", list);
        }

        /// <summary>
        /// 下载访客车辆信息
        /// </summary>
        /// <param name="SmRoleslist"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadParkVisitor(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "ParkVisitor", list);
        }

        #endregion

        #region 权限角色

        /// <summary>
        /// 下载角色
        /// </summary>
        /// <param name="SmRoleslist"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadSysRoles(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "SysRoles", list);
        }

        /// <summary>
        /// 下载用户信息
        /// </summary>
        /// <param name="SmUserslist"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadSysUser(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "SysUser", list);
        }

        /// <summary>
        /// 下载用户拥有的角色
        /// </summary>
        /// <param name="SmUserrolelist"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadSysUserRolesMapping(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "SysUserRolesMapping", list);

        }

        /// <summary>
        /// 下载作用域
        /// </summary>
        /// <param name="ActionScopelist"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadSysScope(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();

            return service.DownloadData(ProxyNo, "SysScope", list);

        }

        /// <summary>
        /// 下载作用域对应的场所
        /// </summary>
        /// <param name="ActionScopeDatalist"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadSysScopeAuthorize(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();
            return service.DownloadData(ProxyNo, "SysScopeAuthorize", list);
        }

        /// <summary>
        /// 下载用户对应的作用域
        /// </summary>
        /// <param name="ActionScopeDatalist"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadSysUserScopeMapping(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();
            return service.DownloadData(ProxyNo, "SysUserScopeMapping", list);

        }

        /// <summary>
        /// 下载角色拥有的权限
        /// </summary>
        /// <param name="SmRoleRightlist"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DownloadSysRoleAuthorize(string ProxyNo, string list)
        {
            ParkDownloadDataServiceCallback service = new ParkDownloadDataServiceCallback();
            return service.DownloadData(ProxyNo, "SysRoleAuthorize", list);
        }
        #endregion


    }
}
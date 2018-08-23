using Common.Core;
using Common.Entities;
using Common.Entities.BaseData;
using Common.Entities.Other;
using Common.Entities.Parking;
using Common.Entities.Statistics;
using Common.Entities.ZHDZ;
using Common.Utilities.Helpers;
using ServicesDLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.Text;

namespace PlatformWcfServers
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“FoundationService”。
    [ServiceContract]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public class ParkUploadDataService
    {
        /// <summary>
        /// 日志记录对象
        /// </summary>
        protected Logger logger = LogManager.GetLogger("数据上传");

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
            return "客户端数据上传服务";
        }

        /// <summary>
        /// 上传线下数据
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="listData"></param>
        /// <returns></returns>
        [OperationContract]
        public List<UploadResult> UplaodClientData(string cmd, string listData)
        {
            List<UploadResult> result = new List<UploadResult>();
            if (string.IsNullOrEmpty(cmd))
            {
                return result;
            }
            switch (cmd)
            {
                case "ParkOrder"://上传订单
                    result = UploadParkOrder(listData);
                    break;
                case "ParkEvent"://上传通道事件
                    result = UploadParkEvent(listData);
                    break;
                case "ParkIORecord"://上传进出纪录
                    result = UploadParkIORecord(listData);
                    break;
                case "ParkTimeseries"://上传进出时序
                    result = UploadParkTimeseries(listData);
                    break;
                case "ParkChangeshiftrecord"://上传交接班纪录
                    result = UploadParkChangeshiftrecord(listData);
                    break;
                case "ParkDeviceDetection"://上传设备实时检测信息
                    result = UploadParkDeviceDetection(listData);
                    break;
                case "Statistics_ChangeShift"://上传当班统计信息
                    result = UploadStatistics_ChangeShift(listData);
                    break;
                case "Statistics_Gather"://上传实时统计
                    result = UploadStatistics_Gather(listData);
                    break;
                case "Statistics_GatherLongTime"://上传统计时长
                    result = UploadStatistics_GatherLongTime(listData);
                    break;
                case "Statistics_GatherGate"://上传通道每小时统计数据
                    result = UploadStatistics_GatherGate(listData);
                    break;
                case "ParkCarDerate"://上传消费打折信息
                    result = UploadParkCarDerate(listData);
                    break;
                case "BaseCompany"://上传公司信息
                    result = UploadBaseCompany(listData);
                    break;
                case "BaseVillage"://上传小区信息
                    result = UploadBaseVillage(listData);
                    break;
                case "BaseParkinfo"://上传车场信息
                    result = UploadBaseParkinfo(listData);
                    break;
                case "BasePassRemark"://上传车场放行备注信息
                    result = UploadBasePassRemark(listData);
                    break;
                case "ParkArea"://上传车场区域信息
                    result = UploadParkArea(listData);
                    break;
                case "ParkBox"://上传车场岗亭信息
                    result = UploadParkBox(listData);
                    break;
                case "ParkGate"://上传车场通道信息
                    result = UploadParkGate(listData);
                    break;
                case "ParkDevice"://上传车场通道设备信息
                    result = UploadParkDevice(listData);
                    break;
                case "ParkDeviceParam"://上传车场通道设备信息
                    result = UploadParkDeviceParam(listData);
                    break;
                case "ParkCarType"://上传车场车类型信息
                    result = UploadParkCarType(listData);
                    break;
                case "ParkCarModel"://上传车场车型信息
                    result = UploadParkCarModel(listData);
                    break;
                case "ParkFeeRule"://上传车场收费规则信息
                    result = UploadParkFeeRule(listData);
                    break;
                case "ParkFeeRuleDetail"://上传车场收费规则明细信息
                    result = UploadParkFeeRuleDetail(listData);
                    break;
                case "BaseCard"://上传车场卡片信息
                    result = UploadBaseCard(listData);
                    break;
                case "ParkGrant"://上传车场授权信息
                    result = UploadParkGrant(listData);
                    break;
                case "ParkCardSuspendPlan"://上传车场车辆暂停计划信息
                    result = UploadParkCardSuspendPlan(listData);
                    break;
                case "BaseEmployee"://上传车场车人员信息
                    result = UploadBaseEmployee(listData);
                    break;
                case "EmployeePlate"://上传人员车牌信息
                    result = UploadEmployeePlate(listData);
                    break;
                case "ParkSeller"://上传商家信息
                    result = UploadParkSeller(listData);
                    break;
                case "ParkDerate"://上传商家优免信息
                    result = UploadParkDerate(listData);
                    break;
                case "ParkDerateIntervar"://上传商家优免时段信息
                    result = UploadParkDerateIntervar(listData);
                    break;
                case "ParkBlacklist"://上传黑名单信息
                    result = UploadParkBlacklist(listData);
                    break;
                case "SysRoles"://上传角色信息
                    result = UploadSysRoles(listData);
                    break;
                case "SysUser"://上传用户信息
                    result = UploadSysUser(listData);
                    break;
                case "SysUserRolesMapping"://上传用户对应角色
                    result = UploadSysUserRolesMapping(listData);
                    break;
                case "SysScope"://上传作用域
                    result = UploadSysScope(listData);
                    break;
                case "SysUserScopeMapping"://上传用户对应的作用域
                    result = UploadSysUserScopeMapping(listData);
                    break;
                case "SysRoleAuthorize"://上传角色对应的权限
                    result = UploadSysRoleAuthorize(listData);
                    break;
                case "ParkReserveBit":
                    result = UploadParkReserveBit(listData);
                    break;
                case "ParkCarBitGroup":
                    result = UploadParkCarBitGroup(listData);
                    break;
            }
            return result;
        }

        #region 基本信息
        /// <summary>
        /// 上传公司信息
        /// </summary>
        /// <param name="listBaseCompany"></param>
        /// <returns></returns>
        public List<UploadResult> UploadBaseCompany(string listBaseCompany)
        {
            List<BaseCompany> list = JsonHelper.GetJson<List<BaseCompany>>(listBaseCompany);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadBaseCompany(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传小区信息
        /// </summary>
        /// <param name="listBaseVillage"></param>
        /// <returns></returns>
        public List<UploadResult> UploadBaseVillage(string listBaseVillage)
        {
            List<BaseVillage> list = JsonHelper.GetJson<List<BaseVillage>>(listBaseVillage);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadBaseVillage(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传车场信息
        /// </summary>
        /// <param name="listBaseParkinfo"></param>
        /// <returns></returns>
        public List<UploadResult> UploadBaseParkinfo(string listBaseParkinfo)
        {
            List<BaseParkinfo> list = JsonHelper.GetJson<List<BaseParkinfo>>(listBaseParkinfo);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadBaseParkinfo(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传车场备注
        /// </summary>
        /// <param name="listBasePassRemark"></param>
        /// <returns></returns>
        public List<UploadResult> UploadBasePassRemark(string listBasePassRemark)
        {
            List<BasePassRemark> list = JsonHelper.GetJson<List<BasePassRemark>>(listBasePassRemark);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadBasePassRemark(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传车场区域
        /// </summary>
        /// <param name="listParkArea"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkArea(string listParkArea)
        {
            List<ParkArea> list = JsonHelper.GetJson<List<ParkArea>>(listParkArea);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkArea(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传车场岗亭信息
        /// </summary>
        /// <param name="listParkBox"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkBox(string listParkBox)
        {
            List<ParkBox> list = JsonHelper.GetJson<List<ParkBox>>(listParkBox);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkBox(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传车场通道信息
        /// </summary>
        /// <param name="listParkGate"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkGate(string listParkGate)
        {
            List<ParkGate> list = JsonHelper.GetJson<List<ParkGate>>(listParkGate);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkGate(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传车场通道设备信息
        /// </summary>
        /// <param name="listParkDevice"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkDevice(string listParkDevice)
        {
            List<ParkDevice> list = JsonHelper.GetJson<List<ParkDevice>>(listParkDevice);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkDevice(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传车场设备参数信息
        /// </summary>
        /// <param name="listParkDevice"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkDeviceParam(string listParkDevice)
        {
            List<ParkDeviceParam> list = JsonHelper.GetJson<List<ParkDeviceParam>>(listParkDevice);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkDeviceParam(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传车场车类信息
        /// </summary>
        /// <param name="listParkCarType"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkCarType(string listParkCarType)
        {
            List<ParkCarType> list = JsonHelper.GetJson<List<ParkCarType>>(listParkCarType);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkCarType(list);
            return listreuslt;
        }

        /// <summary>
        /// 下载车场车型信息
        /// </summary>
        /// <param name="listParkCarModel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkCarModel(string listParkCarModel)
        {
            List<ParkCarModel> list = JsonHelper.GetJson<List<ParkCarModel>>(listParkCarModel);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkCarModel(list);
            return listreuslt;
        }

        /// <summary>
        /// 下载车场收费规则
        /// </summary>
        /// <param name="listParkFeeRule"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkFeeRule(string listParkFeeRule)
        {
            List<ParkFeeRule> list = JsonHelper.GetJson<List<ParkFeeRule>>(listParkFeeRule);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkFeeRule(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传车场收费规则明细
        /// </summary>
        /// <param name="listParkFeeRuleDetail"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkFeeRuleDetail(string listParkFeeRuleDetail)
        {
            List<ParkFeeRuleDetail> list = JsonHelper.GetJson<List<ParkFeeRuleDetail>>(listParkFeeRuleDetail);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkFeeRuleDetail(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传卡片信息
        /// </summary>
        /// <param name="listBaseCard"></param>
        /// <returns></returns>
        public List<UploadResult> UploadBaseCard(string listBaseCard)
        {
            List<BaseCard> list = JsonHelper.GetJson<List<BaseCard>>(listBaseCard);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadBaseCard(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传车场授权信息
        /// </summary>
        /// <param name="listParkGrant"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkGrant(string listParkGrant)
        {
            List<ParkGrant> list = JsonHelper.GetJson<List<ParkGrant>>(listParkGrant);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkGrant(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传车辆暂停计划
        /// </summary>
        /// <param name="listParkCardSuspendPlan"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkCardSuspendPlan(string listParkCardSuspendPlan)
        {
            List<ParkCardSuspendPlan> list = JsonHelper.GetJson<List<ParkCardSuspendPlan>>(listParkCardSuspendPlan);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkCardSuspendPlan(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传人员信息
        /// </summary>
        /// <param name="listBaseEmployee"></param>
        /// <returns></returns>
        public List<UploadResult> UploadBaseEmployee(string listBaseEmployee)
        {
            List<BaseEmployee> list = JsonHelper.GetJson<List<BaseEmployee>>(listBaseEmployee);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadBaseEmployee(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传人员车牌信息
        /// </summary>
        /// <param name="listEmployeePlate"></param>
        /// <returns></returns>
        public List<UploadResult> UploadEmployeePlate(string listEmployeePlate)
        {
            List<EmployeePlate> list = JsonHelper.GetJson<List<EmployeePlate>>(listEmployeePlate);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadEmployeePlate(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传商家信息
        /// </summary>
        /// <param name="listParkSeller"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkSeller(string listParkSeller)
        {
            List<ParkSeller> list = JsonHelper.GetJson<List<ParkSeller>>(listParkSeller);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkSeller(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传商家优免信息
        /// </summary>
        /// <param name="listParkCarDerate"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkDerate(string listParkDerate)
        {
            List<ParkDerate> list = JsonHelper.GetJson<List<ParkDerate>>(listParkDerate);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkDerate(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传优免时段
        /// </summary>
        /// <param name="listParkDerateIntervar"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkDerateIntervar(string listParkDerateIntervar)
        {
            List<ParkDerateIntervar> list = JsonHelper.GetJson<List<ParkDerateIntervar>>(listParkDerateIntervar);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkDerateIntervar(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传黑名单信息
        /// </summary>
        /// <param name="listParkBlacklist"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkBlacklist(string listParkBlacklist)
        {
            List<ParkBlacklist> list = JsonHelper.GetJson<List<ParkBlacklist>>(listParkBlacklist);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkBlacklist(list);
            return listreuslt;
        }

        #endregion

        #region 事件记录
        /// <summary>
        /// 上传事件纪录
        /// </summary>
        /// <param name="listParkEvent"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkEvent(string listParkEvent)
        {
            List<ParkEvent> list = JsonHelper.GetJson<List<ParkEvent>>(listParkEvent);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkEvent(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传进出纪录
        /// </summary>
        /// <param name="listParkIORecord"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkIORecord(string listParkIORecord)
        {
            List<ParkIORecord> list = JsonHelper.GetJson<List<ParkIORecord>>(listParkIORecord);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkIORecord(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传进出时序
        /// </summary>
        /// <param name="listParkTimeseries"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkTimeseries(string listParkTimeseries)
        {
            List<ParkTimeseries> list = JsonHelper.GetJson<List<ParkTimeseries>>(listParkTimeseries);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkTimeseries(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传订单信息
        /// </summary>
        /// <param name="listParkOrder"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkOrder(string listParkOrder)
        {
            List<ParkOrder> list = JsonHelper.GetJson<List<ParkOrder>>(listParkOrder);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkOrder(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传交接班纪录
        /// </summary>
        /// <param name="listParkChangeshiftrecord"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkChangeshiftrecord(string listParkChangeshiftrecord)
        {
            List<ParkChangeshiftrecord> list = JsonHelper.GetJson<List<ParkChangeshiftrecord>>(listParkChangeshiftrecord);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkChangeshiftrecord(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传当班统计信息
        /// </summary>
        /// <param name="ListStatistics_ChangeShift"></param>
        /// <returns></returns>
        public List<UploadResult> UploadStatistics_ChangeShift(string ListStatistics_ChangeShift)
        {
            List<Statistics_ChangeShift> list = JsonHelper.GetJson<List<Statistics_ChangeShift>>(ListStatistics_ChangeShift);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadChangeShift(list);
            return listreuslt;

        }

        /// <summary>
        /// 上传实时统计
        /// </summary>
        /// <param name="ListStatistics_Gather"></param>
        /// <returns></returns>
        public List<UploadResult> UploadStatistics_Gather(string ListStatistics_Gather)
        {
            List<Statistics_Gather> list = JsonHelper.GetJson<List<Statistics_Gather>>(ListStatistics_Gather);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadGather(list);
            return listreuslt;

        }

        /// <summary>
        /// 上传统计时长
        /// </summary>
        /// <param name="listStatistics_GatherLongTime"></param>
        /// <returns></returns>
        public List<UploadResult> UploadStatistics_GatherLongTime(string listStatistics_GatherLongTime)
        {
            List<Statistics_GatherLongTime> list = JsonHelper.GetJson<List<Statistics_GatherLongTime>>(listStatistics_GatherLongTime);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadGatherLongTime(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传通道每小时统计数据
        /// </summary>
        /// <param name="listStatistics_GatherGate"></param>
        /// <returns></returns>
        public List<UploadResult> UploadStatistics_GatherGate(string listStatistics_GatherGate)
        {
            List<Statistics_GatherGate> list = JsonHelper.GetJson<List<Statistics_GatherGate>>(listStatistics_GatherGate);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadGatherGate(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传设备实时检测信息
        /// </summary>
        /// <param name="listParkDeviceDetection"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkDeviceDetection(string listParkDeviceDetection)
        {
            List<ParkDeviceDetection> list = JsonHelper.GetJson<List<ParkDeviceDetection>>(listParkDeviceDetection);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkDeviceDetection(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传消费打折信息
        /// </summary>
        /// <param name="listUploadParkCarDerate"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkCarDerate(string listUploadParkCarDerate)
        {
            List<ParkCarDerate> list = JsonHelper.GetJson<List<ParkCarDerate>>(listUploadParkCarDerate);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkCarDerate(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传图片到服务器
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OperationContract]
        public bool UploadIOImage(ImgModel imgmodel)
        {
            string root = SystemInfo.ImgPath;
            bool result = false;
            try
            {
                string str = root;
                string[] strpath = imgmodel.ImgPath.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                if (strpath.Length > 0)
                {

                    for (int i = 0; i < strpath.Length - 1; i++)
                    {
                        str += "\\" + strpath[i];
                        try
                        {
                            if (!Directory.Exists(str))
                            {

                                Directory.CreateDirectory(str);

                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Fatal("上传图片到服务器异常！UploadIOImage()" + ex.Message + str + "\r\n");
                        }
                    }

                    try
                    {
                        MemoryStream stream = new MemoryStream(Convert.FromBase64String(imgmodel.ImgStream));
                        using (FileStream outputStream = new FileStream(root + "\\" + imgmodel.ImgPath, FileMode.Create, FileAccess.Write))
                        {
                            stream.CopyTo(outputStream);
                            outputStream.Flush();
                        }
                        stream.Close();
                        stream.Dispose();
                        System.Threading.Thread.Sleep(100);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("上传图片到服务器异常！UploadIOImage()" + ex.Message + root + "\\" + imgmodel.ImgPath + "\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传图片到服务器异常！UploadIOImage()" + ex.Message + "\r\n");
            }

            return result;
        }

        /// <summary>
        /// 上传车场车位信息
        /// </summary>
        /// <param name="PKID"></param>
        /// <param name="CarBitNumLeft"></param>
        /// <param name="CarBitNumFixed"></param>
        /// <param name="SpaceBitNum"></param>
        /// <returns></returns>
        [OperationContract]
        public bool UploadParkinfoCarBitNum(string PKID, int CarBitNumLeft, int CarBitNumFixed, int SpaceBitNum)
        {
            UploadDataDLL dll = new UploadDataDLL();
            bool res = dll.UploadParkinfoCarBitNum(PKID, CarBitNumLeft, CarBitNumFixed, SpaceBitNum);
            return res;
        }

        public static List<UploadResult> UploadParkCarBitGroup(string listData)
        {
            List<ParkCarBitGroup> list = JsonHelper.GetJson<List<ParkCarBitGroup>>(listData);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkCarBitGroup(list);
            return listreuslt;
        }
        #endregion

        #region 角色权限

        /// <summary>
        /// 上传角色信息
        /// </summary>
        /// <param name="listSysRoles"></param>
        /// <returns></returns>
        public List<UploadResult> UploadSysRoles(string listSysRoles)
        {
            List<SysRoles> list = JsonHelper.GetJson<List<SysRoles>>(listSysRoles);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadSysRoles(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传用户信息
        /// </summary>
        /// <param name="listSysUser"></param>
        /// <returns></returns>
        public List<UploadResult> UploadSysUser(string listSysUser)
        {
            List<SysUser> list = JsonHelper.GetJson<List<SysUser>>(listSysUser);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadSysUser(list);
            return listreuslt;
        }
        /// <summary>
        /// 上传用户对应角色
        /// </summary>
        /// <param name="listSysUserRolesMapping"></param>
        /// <returns></returns>
        public List<UploadResult> UploadSysUserRolesMapping(string listSysUserRolesMapping)
        {
            List<SysUserRolesMapping> list = JsonHelper.GetJson<List<SysUserRolesMapping>>(listSysUserRolesMapping);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadSysUserRolesMapping(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传作用域
        /// </summary>
        /// <param name="listSysScope"></param>
        /// <returns></returns>
        public List<UploadResult> UploadSysScope(string listSysScope)
        {
            List<SysScope> list = JsonHelper.GetJson<List<SysScope>>(listSysScope);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadSysScope(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传作用域对应的区域
        /// </summary>
        /// <param name="listSysScopeAuthorize"></param>
        /// <returns></returns>
        public List<UploadResult> UploadSysScopeAuthorize(string listSysScopeAuthorize)
        {
            List<SysScopeAuthorize> list = JsonHelper.GetJson<List<SysScopeAuthorize>>(listSysScopeAuthorize);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadSysScopeAuthorize(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传用户对应的作用域
        /// </summary>
        /// <param name="listSysUserScopeMapping"></param>
        /// <returns></returns>
        public List<UploadResult> UploadSysUserScopeMapping(string listSysUserScopeMapping)
        {
            List<SysUserScopeMapping> list = JsonHelper.GetJson<List<SysUserScopeMapping>>(listSysUserScopeMapping);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadSysUserScopeMapping(list);
            return listreuslt;
        }

        /// <summary>
        /// 上传角色对应的权限
        /// </summary>
        /// <param name="listSysRoleAuthorize"></param>
        /// <returns></returns>
        public List<UploadResult> UploadSysRoleAuthorize(string listSysRoleAuthorize)
        {
            List<SysRoleAuthorize> list = JsonHelper.GetJson<List<SysRoleAuthorize>>(listSysRoleAuthorize);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadSysRoleAuthorize(list);
            return listreuslt;
        }
        #endregion

        #region 车位预定

        public static List<UploadResult> UploadParkReserveBit(string listData)
        {
            List<ParkReserveBit> list = JsonHelper.GetJson<List<ParkReserveBit>>(listData);
            foreach (var obj in list)
            {
                obj.HaveUpdate = 0;
            }
            UploadDataDLL dll = new UploadDataDLL();
            List<UploadResult> listreuslt = dll.UploadParkReserveBit(list);
            return listreuslt;
        }



        #endregion

        #region 珠海定制
        /// <summary>
        /// 上传进出场记录
        /// </summary>
        /// <param name="listParkIORecord"></param>
        /// <returns></returns>
        [OperationContract]
        public List<UploadResult> UploadParkIORecordZH(string listParkIORecord)
        {
            List<UploadResult> result = new List<UploadResult>();
            List<ZHIORecord> list = JsonHelper.GetJson<List<ZHIORecord>>(listParkIORecord);
            foreach (var obj in list)
            {
                string str = PostData("", JsonHelper.GetJsonString(obj));
                ZHResult model = JsonHelper.GetJson<ZHResult>(str);
                if (model.code == 200)
                {
                    result.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                }
            }
            return result;
        }

        /// <summary>
        /// 上传车场车信息
        /// </summary>
        /// <param name="listBaseParkinfo"></param>
        /// <returns></returns>
        [OperationContract]
        public bool UploadBaseParkinfoZH(string listBaseParkinfo)
        {
            string str = PostData("", listBaseParkinfo);
            ZHResult model = JsonHelper.GetJson<ZHResult>(str);
            if (model.code == 200)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 上传车位信息
        /// </summary>
        /// <param name="Bits"></param>
        /// <returns></returns>
        [OperationContract]
        public bool UploadParkinfoBitZH(string ZHParkBit)
        {
            string str = PostData("", ZHParkBit);
            ZHResult model = JsonHelper.GetJson<ZHResult>(str);
            if (model.code == 200)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 上传收费区域信息
        /// </summary>
        /// <param name="ZHParkArea"></param>
        /// <returns></returns>
        [OperationContract]
        public bool UploadParkAreaZH(string ZHParkArea)
        {
            string str = PostData("", ZHParkArea);
            ZHResult model = JsonHelper.GetJson<ZHResult>(str);
            if (model.code == 200)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string PostData(string cmd, string data)
        {
            try
            {
                string url = "http://api.bsgoal.net.cn:8088/AppApi/" + cmd;
                WebClient clinet = new WebClient();
                clinet.Headers.Add("Content-Type", "application/x-www-form-urlencoded;");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可 
                string datastr = "data=" + data;
                byte[] postData = Encoding.UTF8.GetBytes(datastr);
                byte[] responseData = clinet.UploadData(url, "POST", postData);//得到返回字符流  
                string srcString = Encoding.UTF8.GetString(responseData);//解码  
                return srcString;
            }
            catch (Exception ex)
            {
                ZHResult model = new ZHResult();
                model.code = -1;
                model.message = "";
                return JsonHelper.GetJsonString(model);
            }
        }
        #endregion
    }
}
namespace HostingService
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SpsDb : DbContext
    {
        public SpsDb()
            : base("name=SpsConnection")
        {
        }

        public virtual DbSet<AdvanceParking> AdvanceParkings { get; set; }
        public virtual DbSet<Ali_ApiConfig> Ali_ApiConfig { get; set; }
        public virtual DbSet<BaseCard> BaseCards { get; set; }
        public virtual DbSet<BaseCity> BaseCities { get; set; }
        public virtual DbSet<BaseCompany> BaseCompanies { get; set; }
        public virtual DbSet<BaseEmployee> BaseEmployees { get; set; }
        public virtual DbSet<BaseParkinfo> BaseParkinfoes { get; set; }
        public virtual DbSet<BasePassRemark> BasePassRemarks { get; set; }
        public virtual DbSet<BaseProvince> BaseProvinces { get; set; }
        public virtual DbSet<BaseVillage> BaseVillages { get; set; }
        public virtual DbSet<BWYGateMapping> BWYGateMappings { get; set; }
        public virtual DbSet<EmployeePlate> EmployeePlates { get; set; }
        public virtual DbSet<Exception> Exceptions { get; set; }
        public virtual DbSet<LoginLog> LoginLogs { get; set; }
        public virtual DbSet<OnlineOrder> OnlineOrders { get; set; }
        public virtual DbSet<OperateLog> OperateLogs { get; set; }
        public virtual DbSet<ParkArea> ParkAreas { get; set; }
        public virtual DbSet<ParkBlacklist> ParkBlacklists { get; set; }
        public virtual DbSet<ParkBox> ParkBoxes { get; set; }
        public virtual DbSet<ParkCarBitGroup> ParkCarBitGroups { get; set; }
        public virtual DbSet<ParkCarDerate> ParkCarDerates { get; set; }
        public virtual DbSet<ParkCardSuspendPlan> ParkCardSuspendPlans { get; set; }
        public virtual DbSet<ParkCarlineInfo> ParkCarlineInfoes { get; set; }
        public virtual DbSet<ParkCarModel> ParkCarModels { get; set; }
        public virtual DbSet<ParkCarType> ParkCarTypes { get; set; }
        public virtual DbSet<ParkCarTypeSingle> ParkCarTypeSingles { get; set; }
        public virtual DbSet<ParkChangeshiftrecord> ParkChangeshiftrecords { get; set; }
        public virtual DbSet<ParkDerate> ParkDerates { get; set; }
        public virtual DbSet<ParkDerateConfig> ParkDerateConfigs { get; set; }
        public virtual DbSet<ParkDerateIntervar> ParkDerateIntervars { get; set; }
        public virtual DbSet<ParkDerateQRcode> ParkDerateQRcodes { get; set; }
        public virtual DbSet<ParkDevice> ParkDevices { get; set; }
        public virtual DbSet<ParkDeviceDetection> ParkDeviceDetections { get; set; }
        public virtual DbSet<ParkDeviceParam> ParkDeviceParams { get; set; }
        public virtual DbSet<ParkEvent> ParkEvents { get; set; }
        public virtual DbSet<ParkFeeRule> ParkFeeRules { get; set; }
        public virtual DbSet<ParkFeeRuleDetail> ParkFeeRuleDetails { get; set; }
        public virtual DbSet<ParkGate> ParkGates { get; set; }
        public virtual DbSet<ParkGateIOTime> ParkGateIOTimes { get; set; }
        public virtual DbSet<ParkGrant> ParkGrants { get; set; }
        public virtual DbSet<ParkInterim> ParkInterims { get; set; }
        public virtual DbSet<ParkIORecord> ParkIORecords { get; set; }
        public virtual DbSet<ParkLpPlan> ParkLpPlans { get; set; }
        public virtual DbSet<ParkMonthlyCarApply> ParkMonthlyCarApplies { get; set; }
        public virtual DbSet<ParkOrder> ParkOrders { get; set; }
        public virtual DbSet<ParkReserveBit> ParkReserveBits { get; set; }
        public virtual DbSet<ParkSeller> ParkSellers { get; set; }
        public virtual DbSet<ParkTimesery> ParkTimeseries { get; set; }
        public virtual DbSet<ParkVisitor> ParkVisitors { get; set; }
        public virtual DbSet<PersonTg> PersonTgs { get; set; }
        public virtual DbSet<SetupVersion> SetupVersions { get; set; }
        public virtual DbSet<SetupVersionFile> SetupVersionFiles { get; set; }
        public virtual DbSet<Statistics_ChangeShift> Statistics_ChangeShift { get; set; }
        public virtual DbSet<Statistics_Gather> Statistics_Gather { get; set; }
        public virtual DbSet<Statistics_GatherGate> Statistics_GatherGate { get; set; }
        public virtual DbSet<Statistics_GatherLongTime> Statistics_GatherLongTime { get; set; }
        public virtual DbSet<SysCPModelRight> SysCPModelRights { get; set; }
        public virtual DbSet<SysDictionary> SysDictionaries { get; set; }
        public virtual DbSet<SysRoleAuthorize> SysRoleAuthorizes { get; set; }
        public virtual DbSet<SysRole> SysRoles { get; set; }
        public virtual DbSet<SysScope> SysScopes { get; set; }
        public virtual DbSet<SysScopeAuthorize> SysScopeAuthorizes { get; set; }
        public virtual DbSet<SysUser> SysUsers { get; set; }
        public virtual DbSet<SysUserRolesMapping> SysUserRolesMappings { get; set; }
        public virtual DbSet<SysUserScopeMapping> SysUserScopeMappings { get; set; }
        public virtual DbSet<TgCount> TgCounts { get; set; }
        public virtual DbSet<UploadDataState> UploadDataStates { get; set; }
        public virtual DbSet<UploadImg> UploadImgs { get; set; }
        public virtual DbSet<VisitorInfo> VisitorInfoes { get; set; }
        public virtual DbSet<WX_Account> WX_Account { get; set; }
        public virtual DbSet<WX_ApiConfig> WX_ApiConfig { get; set; }
        public virtual DbSet<WX_Article> WX_Article { get; set; }
        public virtual DbSet<WX_CarInfo> WX_CarInfo { get; set; }
        public virtual DbSet<WX_Info> WX_Info { get; set; }
        public virtual DbSet<WX_InteractionInfo> WX_InteractionInfo { get; set; }
        public virtual DbSet<WX_Keyword> WX_Keyword { get; set; }
        public virtual DbSet<WX_LockCar> WX_LockCar { get; set; }
        public virtual DbSet<WX_Menu> WX_Menu { get; set; }
        public virtual DbSet<WX_MenuAccessRecord> WX_MenuAccessRecord { get; set; }
        public virtual DbSet<WX_OpinionFeedback> WX_OpinionFeedback { get; set; }
        public virtual DbSet<WX_OtherConfig> WX_OtherConfig { get; set; }
        public virtual DbSet<WX_UserLocation> WX_UserLocation { get; set; }
        public virtual DbSet<ParkSettlement> ParkSettlements { get; set; }
        public virtual DbSet<ParkSettlementApprovalFlow> ParkSettlementApprovalFlows { get; set; }
        public virtual DbSet<TV_ParkIORecord_countpk> TV_ParkIORecord_countpk { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdvanceParking>()
                .Property(e => e.OrderId)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AdvanceParking>()
                .Property(e => e.Amount)
                .HasPrecision(18, 1);

            modelBuilder.Entity<AdvanceParking>()
                .Property(e => e.WxOpenId)
                .IsUnicode(false);

            modelBuilder.Entity<AdvanceParking>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<Ali_ApiConfig>()
                .Property(e => e.PublicKey)
                .IsUnicode(false);

            modelBuilder.Entity<Ali_ApiConfig>()
                .Property(e => e.PrivateKey)
                .IsUnicode(false);

            modelBuilder.Entity<Ali_ApiConfig>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<BaseCard>()
                .Property(e => e.EmployeeID)
                .IsUnicode(false);

            modelBuilder.Entity<BaseParkinfo>()
                .Property(e => e.PKNo)
                .IsUnicode(false);

            modelBuilder.Entity<BaseParkinfo>()
                .Property(e => e.VID)
                .IsUnicode(false);

            modelBuilder.Entity<BaseParkinfo>()
                .Property(e => e.OnlineDiscount)
                .HasPrecision(18, 1);

            modelBuilder.Entity<BaseParkinfo>()
                .Property(e => e.HandlingFee)
                .HasPrecision(18, 1);

            modelBuilder.Entity<BaseParkinfo>()
                .Property(e => e.MaxAmountOfCash)
                .HasPrecision(18, 1);

            modelBuilder.Entity<BasePassRemark>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<BasePassRemark>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<BasePassRemark>()
                .Property(e => e.PKID)
                .IsUnicode(false);

            modelBuilder.Entity<BWYGateMapping>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<BWYGateMapping>()
                .Property(e => e.ParkingID)
                .IsUnicode(false);

            modelBuilder.Entity<BWYGateMapping>()
                .Property(e => e.ParkingName)
                .IsUnicode(false);

            modelBuilder.Entity<BWYGateMapping>()
                .Property(e => e.GateID)
                .IsUnicode(false);

            modelBuilder.Entity<BWYGateMapping>()
                .Property(e => e.GateName)
                .IsUnicode(false);

            modelBuilder.Entity<OnlineOrder>()
                .Property(e => e.OrderID)
                .HasPrecision(20, 0);

            modelBuilder.Entity<OnlineOrder>()
                .Property(e => e.PlateNo)
                .IsUnicode(false);

            modelBuilder.Entity<OnlineOrder>()
                .Property(e => e.Amount)
                .HasPrecision(18, 1);

            modelBuilder.Entity<OnlineOrder>()
                .Property(e => e.Balance)
                .HasPrecision(18, 1);

            modelBuilder.Entity<OnlineOrder>()
                .Property(e => e.BookingAreaID)
                .IsUnicode(false);

            modelBuilder.Entity<OnlineOrder>()
                .Property(e => e.BookingBitNo)
                .IsUnicode(false);

            modelBuilder.Entity<OnlineOrder>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkBlacklist>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkBlacklist>()
                .Property(e => e.PKID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkBlacklist>()
                .Property(e => e.PlateNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ParkBlacklist>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<ParkBox>()
                .Property(e => e.ProxyNo)
                .IsUnicode(false);

            modelBuilder.Entity<ParkCarBitGroup>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkCarBitGroup>()
                .Property(e => e.PKID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkCarBitGroup>()
                .Property(e => e.CarBitName)
                .IsUnicode(false);

            modelBuilder.Entity<ParkCarDerate>()
                .Property(e => e.Reason)
                .IsUnicode(false);

            modelBuilder.Entity<ParkCarDerate>()
                .Property(e => e.Identity)
                .IsUnicode(false);

            modelBuilder.Entity<ParkCarlineInfo>()
                .Property(e => e.Gateid)
                .IsUnicode(false);

            modelBuilder.Entity<ParkCarlineInfo>()
                .Property(e => e.PKID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkCarlineInfo>()
                .Property(e => e.PlateNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ParkCarlineInfo>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<ParkCarModel>()
                .Property(e => e.PlateColor)
                .IsUnicode(false);

            modelBuilder.Entity<ParkCarTypeSingle>()
                .Property(e => e.SingleID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkCarTypeSingle>()
                .Property(e => e.CarTypeID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkChangeshiftrecord>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkChangeshiftrecord>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkChangeshiftrecord>()
                .Property(e => e.BoxID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkChangeshiftrecord>()
                .Property(e => e.PKID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDerateConfig>()
                .Property(e => e.PKID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDerateConfig>()
                .Property(e => e.ConsumeStartAmount)
                .HasPrecision(18, 1);

            modelBuilder.Entity<ParkDerateConfig>()
                .Property(e => e.ConsumeEndAmount)
                .HasPrecision(18, 1);

            modelBuilder.Entity<ParkDerateConfig>()
                .Property(e => e.DerateValue)
                .HasPrecision(18, 1);

            modelBuilder.Entity<ParkDerateQRcode>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDerateQRcode>()
                .Property(e => e.DerateID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDerateQRcode>()
                .Property(e => e.DerateValue)
                .HasPrecision(18, 1);

            modelBuilder.Entity<ParkDerateQRcode>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDevice>()
                .Property(e => e.DeviceID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDevice>()
                .Property(e => e.GateID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDevice>()
                .Property(e => e.SerialPort)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDevice>()
                .Property(e => e.IpAddr)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDevice>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDevice>()
                .Property(e => e.UserPwd)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDevice>()
                .Property(e => e.DeviceNo)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDeviceDetection>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDeviceDetection>()
                .Property(e => e.DeviceID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDeviceDetection>()
                .Property(e => e.PKID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDeviceParam>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkDeviceParam>()
                .Property(e => e.DeviceID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.CardNo)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.CardNum)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.EmployeeName)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.PlateNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.CarTypeID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.CarModelID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.GateID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.OperatorID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.PictureName)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.IORecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.ParkingID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.CertName)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.CertNo)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.Nation)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<ParkEvent>()
                .Property(e => e.CertificateImage)
                .IsUnicode(false);

            modelBuilder.Entity<ParkFeeRule>()
                .Property(e => e.FeeRuleID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkFeeRule>()
                .Property(e => e.RuleName)
                .IsUnicode(false);

            modelBuilder.Entity<ParkFeeRule>()
                .Property(e => e.CarTypeID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkFeeRule>()
                .Property(e => e.CarModelID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkFeeRule>()
                .Property(e => e.AreaID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkFeeRuleDetail>()
                .Property(e => e.RuleDetailID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkFeeRuleDetail>()
                .Property(e => e.RuleID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkFeeRuleDetail>()
                .Property(e => e.StartTime)
                .IsUnicode(false);

            modelBuilder.Entity<ParkFeeRuleDetail>()
                .Property(e => e.EndTime)
                .IsUnicode(false);

            modelBuilder.Entity<ParkGateIOTime>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkGateIOTime>()
                .Property(e => e.GateID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkGateIOTime>()
                .Property(e => e.StartTime)
                .IsUnicode(false);

            modelBuilder.Entity<ParkGateIOTime>()
                .Property(e => e.EndTime)
                .IsUnicode(false);

            modelBuilder.Entity<ParkInterim>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkInterim>()
                .Property(e => e.IORecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkInterim>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.CardID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.PlateNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.CardNo)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.CardNumb)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.EntranceImage)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.EntranceGateID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.EntranceOperatorID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.ExitImage)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.ExitGateID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.ExitOperatorID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.CarTypeID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.CarModelID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.AreaID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.ParkingID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.EntranceCertificateNo)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.ExitCertificateNo)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.EntranceCertificateImage)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.ExitcertificateImage)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.EntranceIDCardPhoto)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.ExitIDCardPhoto)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.LogName)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.EnterDistinguish)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.ExitDistinguish)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.ReserveBitID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.EntranceCertName)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.EntranceSex)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.EntranceNation)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.EntranceAddress)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.ExitCertName)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.ExitSex)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.ExitNation)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.ExitAddress)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.DHOnlienOrderID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.NetWeight)
                .HasPrecision(18, 1);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.Tare)
                .HasPrecision(18, 1);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.Goods)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.Shipper)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.Shippingspace)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.DocumentsNo)
                .IsUnicode(false);

            modelBuilder.Entity<ParkIORecord>()
                .Property(e => e.VisitorID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkLpPlan>()
                .Property(e => e.PlanID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkLpPlan>()
                .Property(e => e.PKID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkLpPlan>()
                .Property(e => e.PlateNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ParkLpPlan>()
                .Property(e => e.EmpName)
                .IsUnicode(false);

            modelBuilder.Entity<ParkLpPlan>()
                .Property(e => e.Reason)
                .IsUnicode(false);

            modelBuilder.Entity<ParkLpPlan>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<ParkLpPlan>()
                .Property(e => e.ZB_User)
                .IsUnicode(false);

            modelBuilder.Entity<ParkLpPlan>()
                .Property(e => e.SH_User)
                .IsUnicode(false);

            modelBuilder.Entity<ParkMonthlyCarApply>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkMonthlyCarApply>()
                .Property(e => e.AccountID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkMonthlyCarApply>()
                .Property(e => e.PKID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkMonthlyCarApply>()
                .Property(e => e.CarTypeID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkMonthlyCarApply>()
                .Property(e => e.CarModelID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkMonthlyCarApply>()
                .Property(e => e.ApplyName)
                .IsUnicode(false);

            modelBuilder.Entity<ParkMonthlyCarApply>()
                .Property(e => e.ApplyMoblie)
                .IsUnicode(false);

            modelBuilder.Entity<ParkMonthlyCarApply>()
                .Property(e => e.PlateNo)
                .IsUnicode(false);

            modelBuilder.Entity<ParkMonthlyCarApply>()
                .Property(e => e.PKLot)
                .IsUnicode(false);

            modelBuilder.Entity<ParkMonthlyCarApply>()
                .Property(e => e.FamilyAddress)
                .IsUnicode(false);

            modelBuilder.Entity<ParkMonthlyCarApply>()
                .Property(e => e.ApplyRemark)
                .IsUnicode(false);

            modelBuilder.Entity<ParkMonthlyCarApply>()
                .Property(e => e.AuditRemark)
                .IsUnicode(false);

            modelBuilder.Entity<ParkOrder>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkOrder>()
                .Property(e => e.OrderNo)
                .IsUnicode(false);

            modelBuilder.Entity<ParkOrder>()
                .Property(e => e.TagID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkOrder>()
                .Property(e => e.CarderateID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkOrder>()
                .Property(e => e.PKID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkOrder>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkOrder>()
                .Property(e => e.OnlineUserID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkOrder>()
                .Property(e => e.OnlineOrderNo)
                .IsUnicode(false);

            modelBuilder.Entity<ParkOrder>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<ParkOrder>()
                .Property(e => e.FeeRuleID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkReserveBit>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkReserveBit>()
                .Property(e => e.AccountID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkReserveBit>()
                .Property(e => e.PKID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkReserveBit>()
                .Property(e => e.BitNo)
                .IsUnicode(false);

            modelBuilder.Entity<ParkReserveBit>()
                .Property(e => e.PlateNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ParkReserveBit>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<ParkSeller>()
                .Property(e => e.PPSellerID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkTimesery>()
                .Property(e => e.TimeseriesID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkTimesery>()
                .Property(e => e.IORecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkTimesery>()
                .Property(e => e.EnterImage)
                .IsUnicode(false);

            modelBuilder.Entity<ParkTimesery>()
                .Property(e => e.ExitImage)
                .IsUnicode(false);

            modelBuilder.Entity<ParkTimesery>()
                .Property(e => e.ParkingID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkTimesery>()
                .Property(e => e.ExitGateID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkTimesery>()
                .Property(e => e.EnterGateID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkTimesery>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkVisitor>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkVisitor>()
                .Property(e => e.VisitorID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkVisitor>()
                .Property(e => e.PKID)
                .IsUnicode(false);

            modelBuilder.Entity<ParkVisitor>()
                .Property(e => e.VID)
                .IsUnicode(false);

            modelBuilder.Entity<SetupVersion>()
                .Property(e => e.SetupVersionID)
                .IsUnicode(false);

            modelBuilder.Entity<SetupVersion>()
                .Property(e => e.VersionCode)
                .IsUnicode(false);

            modelBuilder.Entity<SetupVersion>()
                .Property(e => e.Villages)
                .IsUnicode(false);

            modelBuilder.Entity<SetupVersion>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<SetupVersion>()
                .Property(e => e.PubUser)
                .IsUnicode(false);

            modelBuilder.Entity<SetupVersionFile>()
                .Property(e => e.SetupVersionFilesID)
                .IsUnicode(false);

            modelBuilder.Entity<SetupVersionFile>()
                .Property(e => e.SetupVersionID)
                .IsUnicode(false);

            modelBuilder.Entity<SetupVersionFile>()
                .Property(e => e.FName)
                .IsUnicode(false);

            modelBuilder.Entity<SetupVersionFile>()
                .Property(e => e.FType)
                .IsUnicode(false);

            modelBuilder.Entity<SetupVersionFile>()
                .Property(e => e.FSize)
                .IsUnicode(false);

            modelBuilder.Entity<SetupVersionFile>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.ParkingID)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.BoxID)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.ChangeShiftID)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.AdminID)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.Receivable_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.Real_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.Diff_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.Cash_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.Temp_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.OnLineTemp_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.StordCard_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.OnLine_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.Discount_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.OnLineMonthCardExtend_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.MonthCardExtend_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.OnLineStordCard_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.StordCardRecharge_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.CashDiscount_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.OnLineDiscount_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_ChangeShift>()
                .Property(e => e.ParkingName)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.StatisticsGatherID)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.ParkingID)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.ParkingName)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.Receivable_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.Real_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.Diff_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.Cash_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.Temp_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.OnLineTemp_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.StordCard_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.OnLine_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.Discount_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.OnLineMonthCardExtend_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.MonthCardExtend_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.OnLineStordCard_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.StordCardRecharge_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.CashDiscount_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_Gather>()
                .Property(e => e.OnLineDiscount_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.StatisticsGatherID)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.ParkingID)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.ParkingName)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.BoxID)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.BoxName)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.GateID)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.GateName)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.Receivable_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.Real_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.Diff_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.Cash_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.Temp_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.OnLineTemp_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.StordCard_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.OnLine_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.Discount_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.OnLineMonthCardExtend_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.MonthCardExtend_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.OnLineStordCard_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.StordCardRecharge_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.CashDiscount_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherGate>()
                .Property(e => e.OnLineDiscount_Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Statistics_GatherLongTime>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<Statistics_GatherLongTime>()
                .Property(e => e.ParkingID)
                .IsUnicode(false);

            modelBuilder.Entity<SysUser>()
                .Property(e => e.UserAccount)
                .IsUnicode(false);

            modelBuilder.Entity<UploadDataState>()
                .Property(e => e.TableName)
                .IsUnicode(false);

            modelBuilder.Entity<UploadDataState>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<UploadDataState>()
                .Property(e => e.ProxyNo)
                .IsUnicode(false);

            modelBuilder.Entity<UploadImg>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<UploadImg>()
                .Property(e => e.UploadFailedFile)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.PlateNumber)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.VisitorMobilePhone)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.AccountID)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.VID)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.VisitorName)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.VisitorSex)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.CertifNo)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.CertifAddr)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.EmployeeID)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.CardNo)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.OperatorID)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.VisitorCompany)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.VistorPhoto)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.CertifPhoto)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<VisitorInfo>()
                .Property(e => e.CarModelID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Account>()
                .Property(e => e.AccountID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Account>()
                .Property(e => e.AccountName)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Account>()
                .Property(e => e.TradePWD)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Account>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Account>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Account>()
                .Property(e => e.MobilePhone)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Account>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_ApiConfig>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Article>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_CarInfo>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_CarInfo>()
                .Property(e => e.AccountID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_CarInfo>()
                .Property(e => e.PlateID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_CarInfo>()
                .Property(e => e.PlateNo)
                .IsUnicode(false);

            modelBuilder.Entity<WX_CarInfo>()
                .Property(e => e.CarMonitor)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Info>()
                .Property(e => e.OpenID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Info>()
                .Property(e => e.AccountID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Info>()
                .Property(e => e.NickName)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Info>()
                .Property(e => e.Language)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Info>()
                .Property(e => e.Province)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Info>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Info>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Info>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Info>()
                .Property(e => e.Headimgurl)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Info>()
                .Property(e => e.LastPlateNumber)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Info>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_InteractionInfo>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Keyword>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_LockCar>()
                .Property(e => e.RecordID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_LockCar>()
                .Property(e => e.AccountID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_LockCar>()
                .Property(e => e.PKID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_LockCar>()
                .Property(e => e.PlateNumber)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Menu>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Menu>()
                .Property(e => e.MinIprogramAppId)
                .IsUnicode(false);

            modelBuilder.Entity<WX_Menu>()
                .Property(e => e.MinIprogramPagePath)
                .IsUnicode(false);

            modelBuilder.Entity<WX_MenuAccessRecord>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_OpinionFeedback>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_OtherConfig>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<WX_UserLocation>()
                .Property(e => e.CompanyID)
                .IsUnicode(false);

            modelBuilder.Entity<TV_ParkIORecord_countpk>()
                .Property(e => e.pkid)
                .IsUnicode(false);
        }
    }
}

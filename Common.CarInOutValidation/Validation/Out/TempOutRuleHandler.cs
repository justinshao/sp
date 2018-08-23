
using Common.Entities;
using Common.Services.Park;
using Common.Entities.Enum;
using Common.Entities.Validation;

namespace Common.CarInOutValidation.Validation.Out
{
    class TempOutRuleHandler : RuleHandler
    {
        public string[] FreeCarStartContains = new string[] { "WJ", "军", "空", "海", "北", "沈", "兰", "济", "南", "广", "成", "甲", "V", "Z", "B", "N", "G", "S", "C", "L", "J", "K", "H" };
        public string[] FreeCarEndContains = new string[] { "警" };
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            rst.InOutBaseCardType = BaseCarType.TempCar;
            string errorMsg = "";
            if (args.CarTypeInfo.IsIgnoreHZ && args.Plateinfo.LicenseNum.Length >= 7)
            {
                var plateNumber = args.Plateinfo.LicenseNum.Substring(1, args.Plateinfo.LicenseNum.Length - 1);
                var iorecord = ParkIORecordServices.QueryInCarTempIORecordByLikePlateNumber(args.AreadInfo.PKID, plateNumber, out errorMsg);
                if (iorecord != null)
                {
                    args.Plateinfo.LicenseNum = iorecord.PlateNumber;
                }
            }
            IsPoliceFree(args, rst);

            var blackList = ParkBlacklistServices.GetBlacklist(args.AreadInfo.PKID, args.Plateinfo.LicenseNum, out errorMsg);
            if (blackList != null)
            {
                if (blackList.Status == BlackListStatus.NotEnterAndOut)
                {
                    rst.ResCode = ResultCode.BlacklistCar;
                    return;
                }
                if (args.GateInfo.IoState == IoState.GoOut
                    && blackList.Status == BlackListStatus.CanEnterAndNotOut)
                {
                    rst.ResCode = ResultCode.BlacklistCar;
                    return;
                }
            }
            if (args.GateInfo.IsTempInOut == YesOrNo.No)//子区域允许月卡进出
            {
                if (!rst.IsPoliceFree)
                {
                    rst.ResCode = ResultCode.NoPermissionsInOut;
                    return;
                }
            }
        }

    }
}

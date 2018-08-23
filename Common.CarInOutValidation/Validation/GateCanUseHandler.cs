using Common.Entities;
using Common.Entities.Validation;
using Common.Services;
using System.Collections.Generic;

namespace Common.CarInOutValidation.Validation
{
    /// <summary>
    /// 验证通道能否进出
    /// </summary>
    class GateCanUseHandler : RuleHandler
    {
        protected override void ProcessRule(InputAgs args, ResultAgs rst)
        {
            if (args.GateInfo == null)
            {
                return;
            }
            List<ParkGateIOTime> parkGateIOTimes = ParkGateServices.QueryGateIOTime(args.GateInfo.GateID);
            if (parkGateIOTimes == null || parkGateIOTimes.Count <= 0)
            {
                return;
            }
            bool isSpeacildate = false;
            foreach (var item in parkGateIOTimes)
            {
                if (item.EndTime == "23:59")
                {
                    item.EndTime = "23:59:59";
                }
                if (item.RuleType == 0)
                {
                    continue;
                }
                if (item.RuleDate != args.Plateinfo.TriggerTime.Date)//先检查特殊日期
                {
                    continue;
                }

                isSpeacildate = true;
                if (args.Plateinfo.TriggerTime.TimeOfDay > item.StartTime.ToTimeSpan()
                    && args.Plateinfo.TriggerTime.TimeOfDay < item.EndTime.ToTimeSpan())
                {
                    if (item.InOutState == 1)
                    {
                        rst.ResCode = ResultCode.NoPermissionsInOut;
                        rst.ValidMsg = "通道进出时段限制";
                        return;
                    }
                }
                else//时段外 如果时段内是允许进的，时段外就不允许进
                {
                    if (item.InOutState == 0)
                    {
                        rst.ResCode = ResultCode.NoPermissionsInOut;
                        rst.ValidMsg = "通道进出时段限制";
                        return;
                    }
                }
            }
            if (isSpeacildate)
            {
                return;
            }
            foreach (var item in parkGateIOTimes)//再检查星期
            {
                if (item.RuleType == 1)
                {
                    continue;
                }
                if ((int)args.Plateinfo.TriggerTime.DayOfWeek != item.WeekIndex)
                {
                    continue;
                }
                isSpeacildate = true;
                if (args.Plateinfo.TriggerTime.TimeOfDay > item.StartTime.ToTimeSpan()
                    && args.Plateinfo.TriggerTime.TimeOfDay < item.EndTime.ToTimeSpan())
                {
                    if (item.InOutState == 1)
                    {
                        rst.ResCode = ResultCode.NoPermissionsInOut;
                        rst.ValidMsg = "通道进出时段限制";
                        return;
                    }
                }
                else//时段外 如果时段内是允许进的，时段外就不允许进
                {
                    if (item.InOutState == 0)
                    {
                        rst.ResCode = ResultCode.NoPermissionsInOut;
                        rst.ValidMsg = "通道进出时段限制";
                        return;
                    }
                }
            }
        }
    }
}

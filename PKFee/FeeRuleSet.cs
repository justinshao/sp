using System;
using Base.SqlDML.Parking;


namespace PKFee
{
    public class FeeRuleSet
    {
        public int CompanyID { get; set; }
        public int PlaceID { get; set; }
        public int CarType { get; set; }
        public int CardType { get; set; }
        /// <summary>
        /// 入场时间
        /// </summary>
        public DateTime ParkingBeginTime { get; set; }
        /// <summary>
        /// 出场时间
        /// </summary>
        public DateTime ParkingEndTime { get; set; }

        public FeeRuleSet()
        {
        }

        public decimal CalcParkingFee()
        {
            string errorMsg = "";
            var ruleset = FeeCommand.GetFeeRule(string.Empty,out errorMsg);
            var feeruledetails = FeeCommand.GetFeeRuleDetails(out errorMsg);
            IFeeRule rule;
            switch (ruleset.FeeType)
            {
                case 1:
                    rule = new Hours24();
                    break;
                case 2:
                    rule = new Hours12();
                    break;
                case 3:
                    rule = new DayNight();
                    break;
                case 4:
                    rule = new NaturalDay();
                    break;
                default:
                    rule = new Hours24();
                    break;
            }
            rule.ParkingBeginTime = this.ParkingBeginTime;
            rule.ParkingEndTime = this.ParkingEndTime;
            rule.FeeRule = ruleset;// (from a in db.PKFeeRule where a.RecordID == ruleset.FeeRuleID select a).FirstOrDefault();
            rule.listRuleDetail = feeruledetails;// (from a in db.PKFeeRuleDetail where a.RuleID == ruleset.FeeRuleID select a).ToList();

            return rule.CalcFee();
        }
    }
}

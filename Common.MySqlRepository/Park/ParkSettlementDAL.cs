using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.IRepository.Park;
using Common.Entities.Parking;
using Common.DataAccess;
using System.Data.Common;
using Common.Utilities;

namespace Common.MySqlRepository.Park
{
    public class ParkSettlementDAL : BaseDAL, IParkSettlement
    {
        /// <summary>
        /// 获取所有帐期
        /// </summary>
        /// <param name="PKID">车场编号</param>
        /// <returns></returns>
        public List<string> GetPriods(string PKID)
        {
            List<string> priods = new List<string>();
            string strSql = "select Priod from parksettlement where pkid=@PKID and settlestatus!=-1 order by priod asc";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", PKID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        priods.Add(reader["Priod"].ToString());
                    }
                }
            }
            return priods;
        }
        /// <summary>
        /// 获得最大帐期
        /// </summary>
        /// <param name="PKID">车场编号</param>
        /// <returns></returns>
        public ParkSettlementModel GetMaxPriodSettlement(string PKID)
        {
            ParkSettlementModel settlement = null;
            string strSql = "select  top 1 * from parksettlement where pkid=@PKID and settlestatus!=-1 order by priod desc";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", PKID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        settlement = new ParkSettlementModel();
                        settlement = DataReaderToModel<ParkSettlementModel>.ToModel(reader);
                    }
                }
            }
            return settlement;
        }
        public bool UpdateSettlementStatus(string RecordID, int SettleStatus)
        {
            ParkSettlementModel settlement = new ParkSettlementModel();
            string strSql = "update parksettlement set SettleStatus=@SettleStatus where recordid=@RecordID";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("SettleStatus", SettleStatus);
                dbOperator.AddParameter("RecordID", RecordID);
                return dbOperator.ExecuteNonQuery(strSql) > 0 ? true : false;
            }
        }

      


        /// <summary>
        /// 获取所有线上支付的金额
        /// </summary>
        /// <param name="PKID">车场编号</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public List<ParkOrder> GetSettlementPayAmount(string PKID, DateTime StartTime, DateTime EndTime)
        {
            List<ParkOrder> orderlist = new List<ParkOrder>();
            string strSql = "select RecordID,OrderNo,PayAmount from parkorder where pkid=@PKID and ordertime>=@StartTime and ordertime<=@EndTime and status=1 and DataStatus!=2 and payway!=1";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", PKID);
                dbOperator.AddParameter("StartTime", StartTime);
                dbOperator.AddParameter("EndTime", EndTime);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        orderlist.Add(DataReaderToModel<ParkOrder>.ToModel(reader));
                    }
                }
            }
            return orderlist;
        }
        /// <summary>
        /// 生成结算单
        /// </summary>
        /// <param name="settlement">结算单对象</param>
        /// <returns></returns>
        public bool BuildSettlement(ParkSettlementModel settlement)
        {
            bool flag = false; 
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                string strSql = "insert into parksettlement(recordid,pkid,priod,settlestatus,totalamount,handlingfeeamount,receivableamount,starttime,endtime,createtime,remark,createuser) values(@recordid,@pkid,@priod,0,@totalamount,@handlingfeeamount,@receivableamount,@starttime,@endtime,getdate(),@remark,@createuser)";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("recordid", System.Guid.NewGuid().ToString());
                dbOperator.AddParameter("pkid", settlement.PKID);
                dbOperator.AddParameter("priod", settlement.Priod);
                dbOperator.AddParameter("totalamount", settlement.TotalAmount);
                dbOperator.AddParameter("handlingfeeamount", settlement.HandlingFeeAmount);
                dbOperator.AddParameter("receivableamount", settlement.ReceivableAmount);
                dbOperator.AddParameter("starttime", settlement.StartTime);
                dbOperator.AddParameter("endtime", settlement.EndTime);
                dbOperator.AddParameter("remark", settlement.Remark);
                dbOperator.AddParameter("createuser", settlement.CreateUser);
                flag = dbOperator.ExecuteNonQuery(strSql) > 0;
            }
            return flag;
        }
        public bool IsApprovalFlow(string pkid, string userid,int flowid)
        {
            bool flag = false;
            string strSql = " select count(1) Count from ParkSettlementApprovalFlow where pkid=@pkid  and flowuser=@userid and flowid=@flowid";
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("pkid", pkid);
                dbOperator.AddParameter("userid", userid);
                dbOperator.AddParameter("flowid", flowid);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        flag =int.Parse(reader["Count"].ToString()) == 1 ? true : false;
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// 获得所有结算单
        /// </summary>
        /// <param name="PKID">车场编号</param>
        /// <param name="SettleStatus">结算单状态</param>
        /// <param name="Priod">帐期</param>
        /// <returns></returns>
        public List<ParkSettlementModel> GetSettlements(string PKID, int SettleStatus, string Priod,string UserID)
        {
            List<ParkSettlementModel> settlements = new List<ParkSettlementModel>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select s.ID,s.RecordID,s.PKID,s.Priod,s.SettleStatus,s.TotalAmount,s.HandlingFeeAmount,s.ReceivableAmount,s.StartTime,s.EndTime,s.PayTime,s.CreateTime,s.Receipt,s.CreateUser,p.pkname ParkName,u.UserName,s.Remark from parksettlement s left join baseparkinfo p on s.pkid=p.pkid left join sysuser u on u.RecordID=s.CreateUser where s.pkid=@PKID and settlestatus in (
select flowid from ParkSettlementApprovalFlow where flowuser=@userid
 and pkid=@PKID)");
            if (SettleStatus != -1)
            {
                strSql.Append(" and settlestatus=@settlestatus");
            }
            if (Priod != "-1")
            {
                strSql.Append(" and Priod=@Priod");
            }

            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append(@"select s.ID,s.RecordID,s.PKID,s.Priod,s.SettleStatus,s.TotalAmount,s.HandlingFeeAmount,s.ReceivableAmount,s.StartTime,s.EndTime,s.PayTime,s.CreateTime,s.Receipt,s.CreateUser,p.pkname ParkName,u.UserName,s.Remark from parksettlement s left join baseparkinfo p on s.pkid=p.pkid left join sysuser u on u.RecordID=s.CreateUser where s.pkid=@PKID and createuser=@UserID");
            if (SettleStatus != -1)
            {
                strSql1.Append(" and settlestatus=@settlestatus");
            }
            if (Priod != "-1")
            {
                strSql1.Append(" and Priod=@Priod");
            }

            string s = strSql.ToString() + " union " + strSql1.ToString();


            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", PKID);
                dbOperator.AddParameter("SettleStatus", SettleStatus);
                dbOperator.AddParameter("Priod", Priod);
                dbOperator.AddParameter("userid", UserID);
                using (DbDataReader reader = dbOperator.ExecuteReader(s))
                {
                    while (reader.Read())
                    {
                        ParkSettlementModel sm = DataReaderToModel<ParkSettlementModel>.ToModel(reader);
                        if (sm.CreateTime > DateTime.MinValue)
                        {
                            sm.CreateTimeName = sm.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (sm.EndTime > DateTime.MinValue)
                        {
                            sm.EndTimeName = sm.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (sm.PayTime > DateTime.MinValue)
                        {
                            sm.PayTimeName = sm.PayTime.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (sm.StartTime > DateTime.MinValue)
                        {
                            sm.StartTimeName = sm.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        //申请人为当前人
                        if ( UserID == sm.CreateUser)
                        {
                            if (sm.SettleStatus != 0)
                            {
                                if(!IsApprovalFlow(PKID,UserID,sm.SettleStatus))
                                {
                                    sm.IsHide = true;

                                }
                            }
                        }
                        settlements.Add(sm);
                    }
                }
            }
            return settlements;
        }
    }
}

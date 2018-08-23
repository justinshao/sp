using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using Common.DataAccess;
using System.Data.Common;
using Common.Utilities;
using Common.IRepository.WeiXin;

namespace Common.MySqlRepository.WeiXin
{
    public class WXAccountDAL:IWXAccount
    {
        /// <summary>
        /// 获取微信帐户
        /// </summary>
        /// <param name="accountname">帐户名称</param>
        /// <param name="mobile">手机号码</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public List<WX_Account> Search_WXAccount(string companyId,string accountname, string mobile, DateTime? starttime, DateTime? endtime,int PageSize,int PageIndex)
        {
            List<WX_Account> wxaccountlist = new List<WX_Account>();
            string strSql = string.Format(@"select top {0} temp.* from (select  top {1} * from wx_account where status!=1", PageSize, PageSize * PageIndex);
            if (!string.IsNullOrEmpty(accountname))
            {
                strSql += " and accountname like @accountname";
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                strSql += " and mobilephone like @mobile";
            }
            if (starttime != null)
            {
                strSql += " and regtime>=@starttime";
            }
            if (endtime != null)
            {
                strSql += " and regtime<=@endtime";
            }
            if (!string.IsNullOrEmpty(companyId))
            {
                strSql += " and companyid = @companyid";
            }
            strSql += " order by regtime asc) temp order by temp.regtime desc";
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("accountname", "%" + accountname + "%");
                dboperator.AddParameter("mobile", "%" + mobile + "%");
                if (starttime != null)
                {
                    dboperator.AddParameter("starttime", starttime.Value);
                }
                if (endtime != null)
                {
                    dboperator.AddParameter("endtime", endtime.Value);
                }
                if (!string.IsNullOrEmpty(companyId))
                {
                    dboperator.AddParameter("companyid", companyId);
                }
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    while (dr.Read())
                    {
                        wxaccountlist.Add(DataReaderToModel<WX_Account>.ToModel(dr));
                    }
                }
            }
            return wxaccountlist;
        }
        /// <summary>
        /// 获取微信帐户数量
        /// </summary>
        /// <param name="accountname">帐户名称</param>
        /// <param name="mobile">手机号码</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public int Search_WXAccount_Count(string companyId, string accountname, string mobile, DateTime? starttime, DateTime? endtime)
        {
            int _total = 0;
            string strSql = "select count(1) Count from wx_account where status!=1";
            if (!string.IsNullOrEmpty(accountname))
            {
                strSql += " and accountname like @accountname";
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                strSql += " and mobilephone like @mobile";
            }
            if (starttime != null)
            {
                strSql += " and regtime>=@starttime";
            }
            if (endtime != null)
            {
                strSql += " and regtime<=@endtime";
            }
            if (!string.IsNullOrEmpty(companyId))
            {
                strSql += " and companyid = @companyid";
            }
            using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            {
                dboperator.ClearParameters();
                dboperator.AddParameter("accountname", "%" + accountname + "%");
                dboperator.AddParameter("mobile", "%" + mobile + "%");
                dboperator.AddParameter("starttime", starttime);
                dboperator.AddParameter("endtime", endtime);
                if (!string.IsNullOrEmpty(companyId))
                {
                    dboperator.AddParameter("companyid", companyId);
                }
                using (DbDataReader dr = dboperator.ExecuteReader(strSql))
                {
                    if (dr.Read())
                    {
                        _total = int.Parse(dr["Count"].ToString());
                    }
                }
            }
            return _total;
        }
    }
}

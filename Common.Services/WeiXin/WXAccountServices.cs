using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Entities.WX;
using Common.Entities.Other;
using Common.IRepository.WeiXin;
using Common.Factory.WeiXin;
namespace Common.Services.WeiXin
{
    public class WXAccountServices
    {
        /// <summary>
        /// 查询微信帐户
        /// </summary>
        /// <param name="accountname">帐户名称</param>
        /// <param name="mobile">手机号码</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="pageindex">页索引</param>
        /// <param name="pagesize">页大小</param>
        /// <returns></returns>
        public static Pagination Search_WXAccount(string companyId,string accountname, string mobile, DateTime? starttime, DateTime? endtime, int pageindex, int pagesize)
        {
            Pagination _pagination = new Pagination();
            IWXAccount factory = WXAccountFactory.GetFactory();
            _pagination.Total = factory.Search_WXAccount_Count(companyId, accountname, mobile, starttime, endtime);
            _pagination.WXAccountList = factory.Search_WXAccount(companyId, accountname, mobile, starttime, endtime, pagesize, pageindex);
            return _pagination;
        }
    }
}

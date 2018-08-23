using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;

namespace Common.IRepository.WeiXin
{
    public interface IWXAccount
    {
        List<WX_Account> Search_WXAccount(string companyId,string accountname, string mobile, DateTime? starttime, DateTime? endtime, int PageSize, int PageIndex);
        int Search_WXAccount_Count(string companyId,string accountname, string mobile, DateTime? starttime, DateTime? endtime);
    }
}

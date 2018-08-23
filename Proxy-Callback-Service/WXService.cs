using Common.Core;
using Common.DataAccess;
using Common.Entities;
using Common.Entities.Other;
using Common.Entities.Parking;
using Common.Entities.Statistics;
using Common.Entities.WX;
using Common.Services.Statistics;
using Common.Utilities;
using Common.Utilities.Helpers;
using ServicesDLL;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.ServiceModel;


namespace PlatformWcfServers
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“FoundationService”。
    [ServiceContract]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class WXService
    {
        private static readonly Logger logger = LogManager.GetLogger("微信服务");

        private static readonly string _exceptionPrefix = "***异常***";
        
        public bool IsCallbackService()
        {
            return false;
        }
        
        public string GetName()
        {
            return "微信服务";
        }

        private static void StartLog(string msg)
        {
            logger.Info($"======>>{msg} {DateTime.Now.ToString("HH:mm:ss fff")}");
        }

        private static void EndLog(string msg)
        {
            logger.Info($"<<======{msg} {DateTime.Now.ToString("HH:mm:ss fff")}\r\n");
        }

        private static void ExceptionLog(string msg, Exception ex)
        {
            logger.Fatal($"{_exceptionPrefix} {msg}\r\n", ex);
        }

        #region 基本操作
        /// <summary>
        /// 关注微信号
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [OperationContract]
        public bool RegisterAccount(string strwxinfo)
        {
            StartLog($"关注微信号 {nameof(RegisterAccount)} (strwxinfo={strwxinfo})");

            if (string.IsNullOrEmpty(strwxinfo))
            {
                return false;
            }

            string strsql = "";
            try
            {
                WX_Info model = JsonHelper.GetJson<WX_Info>(strwxinfo);
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    strsql = "select * from WX_Info where OpenId=@OpenId";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("OpenId", model.OpenID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            int id = reader["ID"].ToInt();
                            reader.Close();
                            strsql = "update WX_Info set FollowState=1,NickName=@NickName,Language=@Language,Province=@Province,City=@City,";
                            strsql += "Country=@Country,Headimgurl=@Headimgurl,SubscribeTimes=SubscribeTimes+1,LastSubscribeDate=@LastSubscribeDate,";
                            strsql += "LastUnsubscribeDate=@LastUnsubscribeDate,LastVisitDate=@LastVisitDate,CompanyID=@CompanyID where ID=@ID";
                            dbOperator.ClearParameters();
                            dbOperator.AddParameter("NickName", model.NickName);
                            dbOperator.AddParameter("Language", model.Language);
                            dbOperator.AddParameter("Province", model.Province);
                            dbOperator.AddParameter("City", model.City);
                            dbOperator.AddParameter("Country", model.Country);
                            dbOperator.AddParameter("Sex", model.Sex);
                            dbOperator.AddParameter("Headimgurl", model.Headimgurl);
                            logger.Info("LastSubscribeDate\r\n");
                            dbOperator.AddParameter("LastSubscribeDate", DateTime.Now);
                            logger.Info("LastUnsubscribeDate\r\n");
                            dbOperator.AddParameter("LastUnsubscribeDate", DateTime.Now);
                            logger.Info("LastVisitDate\r\n");
                            dbOperator.AddParameter("LastVisitDate", DateTime.Now);
                            dbOperator.AddParameter("CompanyID", model.CompanyID);
                            dbOperator.AddParameter("ID", id);

                            if (dbOperator.ExecuteNonQuery(strsql) > 0)
                            {
                                return true;
                            }
                            else
                            {
                                logger.Fatal("修改用户信息出错！更新数据为0 RegisterAccount()\r\n");
                                return false;
                            }
                        }
                        else
                        {
                            reader.Close();
                            string AccountID = System.Guid.NewGuid().ToString();
                            dbOperator.BeginTransaction();
                            strsql = "insert into WX_Account(AccountID,AccountName,AccountModel,TradePWD,MobilePhone,Status,RegTime,OpenAnswerPhone,IsAutoLock,CompanyID)";
                            strsql += "values(@AccountID,@AccountName,@AccountModel,@TradePWD,@MobilePhone,@Status,@RegTime,@OpenAnswerPhone,@IsAutoLock,@CompanyID)";
                            dbOperator.ClearParameters();
                            dbOperator.AddParameter("AccountID", AccountID);
                            dbOperator.AddParameter("AccountName", model.NickName);
                            dbOperator.AddParameter("AccountModel", 1);
                            dbOperator.AddParameter("TradePWD", "123456");
                            dbOperator.AddParameter("MobilePhone", model.MobilePhone);
                            dbOperator.AddParameter("Status", 0);
                            dbOperator.AddParameter("RegTime", DateTime.Now);
                            dbOperator.AddParameter("OpenAnswerPhone", false);
                            dbOperator.AddParameter("IsAutoLock", false);
                            dbOperator.AddParameter("CompanyID", model.CompanyID);
                            if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                            {
                                dbOperator.RollbackTransaction();
                                return false;
                            }
                            strsql = "insert into WX_Info(OpenID,AccountID,UserType,FollowState,NickName,Language,Province,City,Country,Sex,Headimgurl,SubscribeTimes,LastSubscribeDate,LastUnsubscribeDate,LastVisitDate,CompanyID)";
                            strsql += "values(@OpenID,@AccountID,@UserType,@FollowState,@NickName,@Language,@Province,@City,@Country,@Sex,@Headimgurl,@SubscribeTimes,@LastSubscribeDate,@LastUnsubscribeDate,@LastVisitDate,@CompanyID)";
                            dbOperator.ClearParameters();
                            dbOperator.AddParameter("OpenID", model.OpenID);
                            dbOperator.AddParameter("AccountID", AccountID);
                            dbOperator.AddParameter("UserType", model.UserType);
                            dbOperator.AddParameter("FollowState", 1);
                            dbOperator.AddParameter("NickName", model.NickName);
                            dbOperator.AddParameter("Language", model.Language);
                            dbOperator.AddParameter("Province", model.Province);
                            dbOperator.AddParameter("City", model.City);
                            dbOperator.AddParameter("Country", model.Country);
                            dbOperator.AddParameter("Sex", model.Sex);
                            dbOperator.AddParameter("Headimgurl", model.Headimgurl);
                            dbOperator.AddParameter("SubscribeTimes", 1);
                            logger.Info("LastSubscribeDate\r\n");
                            dbOperator.AddParameter("LastSubscribeDate", DateTime.Now);
                            logger.Info("LastUnsubscribeDate\r\n");
                            dbOperator.AddParameter("LastUnsubscribeDate", DateTime.Now);
                            logger.Info("LastVisitDate\r\n");
                            dbOperator.AddParameter("LastVisitDate", DateTime.Now);
                            dbOperator.AddParameter("CompanyID", model.CompanyID);
                            if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                            {
                                logger.Fatal("增加用户信息出错！更新数据为0 RegisterAccount()" + "\r\n");
                                dbOperator.RollbackTransaction();
                                return false;
                            }
                            dbOperator.CommitTransaction();
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"关注微信号 {nameof(RegisterAccount)}", ex);
                return false;
            }
            finally
            {
                EndLog($"关注微信号 {nameof(RegisterAccount)}");
            }
        }

        /// <summary>
        /// 绑定手机号码
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="MobilePhone"></param>
        /// <returns></returns>
        [OperationContract]
        public bool WXBindingMobilePhone(string AccountID, string MobilePhone)
        {
            StartLog($"绑定手机号码 {nameof(WXBindingMobilePhone)} (AccountID={AccountID},MobilePhone={MobilePhone})");

            if (string.IsNullOrEmpty(AccountID) || string.IsNullOrEmpty(MobilePhone))
            {
                return false;
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select * from WX_Account where AccountID=@AccountID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AccountID", AccountID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            reader.Close();
                            dbOperator.BeginTransaction();
                            strsql = "update WX_Account set MobilePhone=@MobilePhone where AccountID=@AccountID";
                            dbOperator.ClearParameters();
                            dbOperator.AddParameter("MobilePhone", MobilePhone);
                            dbOperator.AddParameter("AccountID", AccountID);
                            if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                            {
                                dbOperator.RollbackTransaction();
                                logger.Fatal("绑定手机号码修改账号手机信息失败！更新数据为0 WXBindingMobilePhone()\r\n");
                                return false;
                            }

                            #region
                            ////存在未绑定的车牌
                            //List<WX_CarInfo> listcarinfo = new List<WX_CarInfo>();
                            //strsql = "select * from WX_CarInfo where AccountID=@AccountID and Status!=1";
                            //dbOperator.ClearParameters();
                            //dbOperator.AddParameter("AccountID", AccountID);
                            //using (DbDataReader reader3 = dbOperator.ExecuteReader(strsql.ToString()))
                            //{
                            //    while (reader3.Read())
                            //    {
                            //        WX_CarInfo model = new WX_CarInfo();
                            //        model.RecordID = reader3["RecordID"].ToString();
                            //        model.PlateNo = reader3["PlateNo"].ToString();
                            //        listcarinfo.Add(model);
                            //    }
                            //    reader3.Close();
                            //}
                            //strsql = "delete from WX_CarInfo where AccountID=@AccountID and Status=1";
                            //dbOperator.ClearParameters();
                            //dbOperator.AddParameter("AccountID", AccountID);
                            //if (dbOperator.ExecuteNonQuery(strsql) < 0)
                            //{
                            //    dbOperator.RollbackTransaction();
                            //    logger.Fatal("绑定手机号码清空绑定车牌信息失败！WXBindingMobilePhone()\r\n");
                            //    return false;
                            //}
                            //using (DbOperator dbOperator2 = ConnectionManager.CreateReadConnection())
                            //{
                            //    List<EmployeePlate> listPlate = new List<EmployeePlate>();
                            //    strsql = "select d.PlateNo,d.PlateID from BaseEmployee a left join BaseCard b on a.EmployeeID=b.EmployeeID ";
                            //    strsql += " left join ParkGrant c on b.CardID=c.CardID LEFT JOIN EmployeePlate d on c.PlateID=d.PlateID ";
                            //    strsql += " where a.MobilePhone=@MobilePhone and a.DataStatus<2 and b.DataStatus<2 and c.DataStatus<2  ";
                            //    strsql += " and d.PlateNo!='' group by d.PlateNo,d.PlateID ";
                            //    dbOperator2.ClearParameters();
                            //    dbOperator2.AddParameter("MobilePhone", MobilePhone);
                            //    using (DbDataReader reader2 = dbOperator2.ExecuteReader(strsql.ToString()))
                            //    {
                            //        while (reader2.Read())
                            //        {
                            //            EmployeePlate model = new EmployeePlate();
                            //            model.PlateID = reader2["PlateID"].ToString();
                            //            model.PlateNo = reader2["PlateNo"].ToString();
                            //            listPlate.Add(model);
                            //        }
                            //    }

                            //    foreach (var obj in listPlate)
                            //    {
                            //        if (listcarinfo.Count(a => a.PlateNo == obj.PlateNo) > 0)
                            //        {
                            //            strsql = "update WX_CarInfo set Status=1 where RecordID=@RecordID";
                            //            dbOperator.ClearParameters();
                            //            dbOperator.AddParameter("RecordID",listcarinfo.First(a=>a.PlateNo==obj.PlateNo).RecordID );
                            //            if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                            //            {
                            //                dbOperator.RollbackTransaction();
                            //                logger.Fatal("绑定手机号码绑定车牌信息失败！WXBindingMobilePhone()\r\n");
                            //                return false;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            strsql = "insert into WX_CarInfo(RecordID,AccountID,PlateID,PlateNo,Status,CarMonitor,BindTime)";
                            //            strsql += "values(@RecordID,@AccountID,@PlateID,@PlateNo,@Status,@CarMonitor,@BindTime)";
                            //            dbOperator.ClearParameters();
                            //            dbOperator.AddParameter("RecordID", Guid.NewGuid().ToString());
                            //            dbOperator.AddParameter("AccountID", AccountID);
                            //            dbOperator.AddParameter("PlateID", obj.PlateID);
                            //            dbOperator.AddParameter("PlateNo", obj.PlateNo);
                            //            dbOperator.AddParameter("Status", 1);
                            //            dbOperator.AddParameter("CarMonitor", "");
                            //            dbOperator.AddParameter("BindTime", DateTime.Now);
                            //            if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                            //            {
                            //                dbOperator.RollbackTransaction();
                            //                logger.Fatal("绑定手机号码绑定车牌信息失败！WXBindingMobilePhone()\r\n");
                            //                return false;
                            //            }
                            //        }

                            //    }
                            //}

                            #endregion
                            dbOperator.CommitTransaction();
                            return true;
                        }
                        else
                        {
                            logger.Fatal("绑定手机号码找不到账户信息！WXBindingMobilePhone()\r\n");
                            return false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"绑定手机号码 {nameof(WXBindingMobilePhone)}", ex);
            }
            finally
            {
                EndLog($"绑定手机号码 {nameof(WXBindingMobilePhone)}");
            }
            return false;
        }

        /// <summary>
        /// 微信取消关注
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool WXUnsubscribe(string OpenID)
        {
            StartLog($"微信取消关注 {nameof(WXUnsubscribe)} (OpenID={OpenID})");

            if (string.IsNullOrEmpty(OpenID))
            {
                return false;
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "update WX_Info  set LastUnsubscribeDate=getdate(),FollowState=0 where OpenID=@OpenID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("OpenId", OpenID);
                    dbOperator.ExecuteNonQuery(strsql);
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLog($"微信取消关注 {nameof(WXUnsubscribe)}", ex);
                return false;
            }
            finally
            {
                EndLog($"微信取消关注 {nameof(WXUnsubscribe)}");
            }
        }

        /// <summary>
        /// 根据微信ID获取账户及微信信息
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        [OperationContract]
        public string QueryWXByOpenId(string OpenID)
        {
            StartLog($"根据微信ID获取账户及微信信息 {nameof(QueryWXByOpenId)} (OpenID={OpenID})");

            if (string.IsNullOrEmpty(OpenID))
            {
                return "";
            }

            WX_Info model = new WX_Info();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select a.*,b.MobilePhone from WX_Info a left join WX_Account b on a.AccountID=b.AccountID where OpenId=@OpenId";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("OpenId", OpenID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            model.AccountID = reader["AccountID"].ToString();
                            model.City = reader["City"].ToString();
                            model.Country = reader["Country"].ToString();
                            model.FollowState = reader["FollowState"].ToInt();
                            model.Headimgurl = reader["Headimgurl"].ToString();
                            model.ID = reader["ID"].ToInt();
                            model.Language = reader["Language"].ToString();
                            model.LastSubscribeDate = reader["LastSubscribeDate"].ToDateTime();
                            model.LastUnsubscribeDate = reader["LastUnsubscribeDate"].ToDateTime();
                            model.LastVisitDate = reader["LastVisitDate"].ToDateTime();
                            model.MobilePhone = reader["MobilePhone"].ToString();
                            model.NickName = reader["NickName"].ToString();
                            model.OpenID = reader["OpenID"].ToString();
                            model.Province = reader["Province"].ToString();
                            model.Sex = reader["Sex"].ToString();
                            model.SubscribeTimes = reader["SubscribeTimes"].ToInt();
                            model.UserType = reader["UserType"].ToInt();
                            model.CompanyID = reader["CompanyID"].ToString();

                            return JsonHelper.GetJsonString(model);
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"根据微信ID获取账户及微信信息 {nameof(QueryWXByOpenId)}", ex);
                return "";
            }
            finally
            {
                EndLog($"根据微信ID获取账户及微信信息 {nameof(QueryWXByOpenId)}");
            }
        }

        /// <summary>
        /// 修改微信信息
        /// </summary>
        /// <param name="wx_info"></param>
        /// <returns></returns>
        [OperationContract]
        public bool EditWXInfo(string wx_info)
        {
            StartLog($"修改微信信息 {nameof(EditWXInfo)} (wx_info={wx_info})");

            if (string.IsNullOrEmpty(wx_info))
            {
                return false;
            }
            try
            {
                WX_Info model = JsonHelper.GetJson<WX_Info>(wx_info);
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "update WX_Info  set  FollowState=1,NickName=@NickName,Language=@Language,Province=@Province,City=@City,Country=@Country,Sex=@Sex,Headimgurl=@Headimgurl where OpenID=@OpenID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("OpenId", model.OpenID);
                    dbOperator.AddParameter("NickName", model.NickName);
                    dbOperator.AddParameter("Language", model.Language);
                    dbOperator.AddParameter("Province", model.Province);
                    dbOperator.AddParameter("City", model.City);
                    dbOperator.AddParameter("Country", model.Country);
                    dbOperator.AddParameter("Sex", model.Sex);
                    dbOperator.AddParameter("Headimgurl", model.Headimgurl);
                    dbOperator.ExecuteNonQuery(strsql);
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLog($"修改微信信息 {nameof(EditWXInfo)}", ex);
                return false;
            }
            finally
            {
                EndLog($"修改微信信息 {nameof(EditWXInfo)}");
            }
        }

        /// <summary>
        /// 根据账户ID获取账户信息
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        [OperationContract]
        public string QueryAccountByAccountID(string AccountID)
        {
            StartLog($"根据账户ID获取账户信息 {nameof(QueryAccountByAccountID)} (AccountID={AccountID})");

            if (string.IsNullOrEmpty(AccountID))
            {
                return "";
            }

            WX_Account model = new WX_Account();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select * from  WX_Account  where AccountID=@AccountID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AccountID", AccountID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            model.AccountID = reader["AccountID"].ToString();
                            model.AccountModel = reader["AccountModel"].ToInt();
                            model.AccountName = reader["AccountName"].ToString();
                            model.CityID = reader["CityID"].ToInt();
                            model.Email = reader["Email"].ToString();
                            model.ID = reader["ID"].ToInt();
                            model.IsAutoLock = reader["IsAutoLock"].ToBoolean();
                            model.MobilePhone = reader["MobilePhone"].ToString();
                            model.OpenAnswerPhone = reader["OpenAnswerPhone"].ToBoolean();
                            model.RegTime = reader["RegTime"].ToDateTime();
                            model.Sex = reader["Sex"].ToString();
                            model.Status = reader["Status"].ToInt();
                            model.TradePWD = reader["TradePWD"].ToString();
                            model.CompanyID = reader["CompanyID"].ToString();

                            return JsonHelper.GetJsonString(model);
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"根据账户ID获取账户信息 {nameof(QueryAccountByAccountID)}", ex);
                return "";
            }
            finally
            {
                EndLog($"根据账户ID获取账户信息 {nameof(QueryAccountByAccountID)}");
            }
        }

        /// <summary>
        /// 根据手机号码获取账户信息
        /// </summary>
        /// <param name="MobilePhone"></param>
        /// <returns></returns>
        [OperationContract]
        public string QueryAccountByMobilePhone(string MobilePhone)
        {
            StartLog($"根据手机号码获取账户信息 {nameof(QueryAccountByMobilePhone)} (MobilePhone={MobilePhone})");

            if (string.IsNullOrEmpty(MobilePhone))
            {
                return "";
            }

            WX_Account model = new WX_Account();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select * from  WX_Account  where MobilePhone=@MobilePhone";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("MobilePhone", model.MobilePhone);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            model.AccountID = reader["AccountID"].ToString();
                            model.AccountModel = reader["AccountModel"].ToInt();
                            model.AccountName = reader["AccountName"].ToString();
                            model.CityID = reader["CityID"].ToInt();
                            model.Email = reader["Email"].ToString();
                            model.ID = reader["ID"].ToInt();
                            model.IsAutoLock = reader["IsAutoLock"].ToBoolean();
                            model.MobilePhone = reader["MobilePhone"].ToString();
                            model.OpenAnswerPhone = reader["OpenAnswerPhone"].ToBoolean();
                            model.RegTime = reader["RegTime"].ToDateTime();
                            model.Sex = reader["Sex"].ToString();
                            model.Status = reader["Status"].ToInt();
                            model.TradePWD = reader["TradePWD"].ToString();
                            model.CompanyID = reader["CompanyID"].ToString();
                            return JsonHelper.GetJsonString(model);
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"根据手机号码获取账户信息 {nameof(QueryAccountByMobilePhone)}", ex);
                return "";
            }
            finally
            {
                EndLog($"根据手机号码获取账户信息 {nameof(QueryAccountByMobilePhone)}");
            }
        }

        /// <summary>
        /// 修改微信设置信息
        /// </summary>
        /// <param name="wx_account"></param>
        /// <returns></returns>
        [OperationContract]
        public bool EditAccountOption(string wx_account)
        {
            StartLog($"修改微信设置信息 {nameof(EditAccountOption)} (wx_account={wx_account})");

            if (string.IsNullOrEmpty(wx_account))
            {
                return false;
            }
            try
            {
                WX_Account model = JsonHelper.GetJson<WX_Account>(wx_account);
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "update WX_Account  set OpenAnswerPhone=@OpenAnswerPhone,IsAutoLock=@IsAutoLock where AccountID=@AccountID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AccountID", model.AccountID);
                    dbOperator.AddParameter("OpenAnswerPhone", model.OpenAnswerPhone);
                    dbOperator.AddParameter("IsAutoLock", model.IsAutoLock);
                    if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLog($"修改微信设置信息 {nameof(EditAccountOption)}", ex);
                return false;
            }
            finally
            {
                EndLog($"修改微信设置信息 {nameof(EditAccountOption)}");
            }
        }

        /// <summary>
        /// 修改微信最后访问时间
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool EidtWX_infoVisitDate(string OpenID)
        {
            StartLog($"修改微信最后访问时间 {nameof(EidtWX_infoVisitDate)} (OpenID={OpenID})");

            if (string.IsNullOrEmpty(OpenID))
            {
                return false;
            }
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "update WX_Info  set LastVisitDate=getdate() where OpenID=@OpenID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("OpenId", OpenID);
                    if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLog($"修改微信最后访问时间 {nameof(EidtWX_infoVisitDate)}", ex);
                return false;
            }
            finally
            {
                EndLog($"修改微信最后访问时间 {nameof(EidtWX_infoVisitDate)}");
            }
        }

        /// <summary>
        /// 修改最后一次缴费车牌号码
        /// </summary>
        /// <param name="OpenID"></param>
        /// <param name="LastPlateNumber"></param>
        /// <returns></returns>
        [OperationContract]
        public bool EidtWX_infoLastPlateNumber(string OpenID, string LastPlateNumber)
        {
            StartLog($"修改最后一次缴费车牌号码 {nameof(EidtWX_infoLastPlateNumber)} (OpenID={OpenID},LastPlateNumber={LastPlateNumber})");

            if (string.IsNullOrEmpty(OpenID))
            {
                return false;
            }
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "update WX_Info  set LastPlateNumber=@LastPlateNumber where OpenID=@OpenID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("OpenId", OpenID);
                    if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLog($"修改最后一次缴费车牌号码 {nameof(EidtWX_infoLastPlateNumber)}", ex);
                return false;
            }
            finally
            {
                EndLog($"修改最后一次缴费车牌号码 {nameof(EidtWX_infoLastPlateNumber)}");
            }
        }
        #endregion

        #region 车场信息
        /// <summary>
        /// 获取车场信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        public string GetPkparkinfoList(double currX, double currY, int Leng)
        {
            StartLog($"获取车场信息 {nameof(GetPkparkinfoList)} (currX={currX},currY={currY},Leng={Leng})");

            List<BaseParkinfo> list = new List<BaseParkinfo>();

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select * from BaseParkinfo where DataStatus<2";
                    dbOperator.ClearParameters();

                    /// <summary>
                    /// 1米的经度偏移量
                    /// </summary>
                    double MeterOfLongitudeOffset = 0.000009;

                    /// <summary>
                    /// 1米的纬度偏移量
                    /// </summary>
                    double MeterOfLatitudeOffset = 0.0000099;
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        while (reader.Read())
                        {
                            if (reader["Coordinate"] != null)
                            {
                                string[] LongitudeLatitude = reader["Coordinate"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                if (LongitudeLatitude.Length == 2)
                                {
                                    double toY = LongitudeLatitude[0].ToDouble();
                                    double toX = LongitudeLatitude[1].ToDouble();

                                    double distanceX = (currX - toX) / MeterOfLongitudeOffset;//经度距离，单位米
                                    double distanceY = (currY - toY) / MeterOfLatitudeOffset;//纬度距离，单位米

                                    if (Math.Sqrt(distanceX * distanceX + distanceY * distanceY) <= Leng)
                                    {
                                        string msg = "";
                                        BaseParkinfo model = DataReaderToModel<BaseParkinfo>.ToModel(reader);
                                        model.Lat = toX;
                                        model.Lng = toY;
                                        list.Add(model);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"获取车场信息 {nameof(GetPkparkinfoList)}", ex);
            }
            finally
            {
                EndLog($"获取车场信息 {nameof(GetPkparkinfoList)}");
            }

            return JsonHelper.GetJsonString(list);
        }

        /// <summary>
        /// 根据账户ID获取车场信息
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetPkparkinByAccountIDList(string AccountID)
        {
            StartLog($"根据账户ID获取车场信息 {nameof(GetPkparkinByAccountIDList)} (AccountID={AccountID})");

            if (string.IsNullOrEmpty(AccountID))
            {
                return "";
            }
            List<BaseParkinfo> list = new List<BaseParkinfo>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select * from WX_Account where AccountID=@AccountID ";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AccountID", AccountID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (!string.IsNullOrEmpty(reader["MobilePhone"].ToString()))
                            {
                                strsql = "select d.* from BaseEmployee a left join BaseCard b on a.EmployeeID=b.EmployeeID ";
                                strsql += " left join ParkGrant c on b.CardID=c.CardID right join BaseParkinfo d on c.PKID=d.PKID  where  MobilePhone=@MobilePhone ";
                                strsql += " and a.DataStatus!=2 and b.DataStatus!=2 and c.DataStatus!=2 and d.DataStatus!=2";
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("MobilePhone", reader["MobilePhone"].ToString());
                                using (DbDataReader reader2 = dbOperator.ExecuteReader(strsql.ToString()))
                                {
                                    while (reader2.Read())
                                    {
                                        BaseParkinfo model = DataReaderToModel<BaseParkinfo>.ToModel(reader);
                                        list.Add(model);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"根据账户ID获取车场信息 {nameof(GetPkparkinByAccountIDList)}", ex);
            }
            finally
            {
                EndLog($"根据账户ID获取车场信息 {nameof(GetPkparkinByAccountIDList)}");
            }

            return JsonHelper.GetJsonString(list);
        }
        #endregion

        #region 车辆管理

        /// <summary>
        /// 增加车辆信息
        /// </summary>
        /// <param name="CarInfo"></param>
        /// <returns>0已经存在该车牌号，1增加成功，2增加失败</returns>
        [OperationContract]
        public int AddWX_CarInfo(string CarInfo)
        {
            StartLog($"增加车辆信息 {nameof(AddWX_CarInfo)} (CarInfo={CarInfo})");

            if (string.IsNullOrEmpty(CarInfo))
            {
                return 2;
            }
            try
            {
                WX_CarInfo model = JsonHelper.GetJson<WX_CarInfo>(CarInfo);
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select * from  WX_CarInfo  where PlateNo=@PlateNo";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PlateNo", model.PlateNo);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return 0;
                        }
                        else
                        {
                            reader.Close();
                            string RecordID = System.Guid.NewGuid().ToString();
                            strsql = "insert into WX_CarInfo(RecordID,AccountID,PlateID,PlateNo,Status,CarMonitor,BindTime)";
                            strsql += "values(@RecordID,@AccountID,@PlateID,@PlateNo,2,@CarMonitor,getdate())";
                            dbOperator.ClearParameters();
                            dbOperator.AddParameter("RecordID", RecordID);
                            dbOperator.AddParameter("AccountID", model.AccountID);
                            dbOperator.AddParameter("PlateID", model.PlateID);
                            dbOperator.AddParameter("PlateNo", model.PlateNo);
                            dbOperator.AddParameter("CarMonitor", model.CarMonitor);
                            if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                            {
                                return 2;
                            }
                        }
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                ExceptionLog($"增加车辆信息 {nameof(AddWX_CarInfo)}", ex);
                return 2;
            }
            finally
            {
                EndLog($"增加车辆信息 {nameof(AddWX_CarInfo)}");
            }
        }

        /// <summary>
        /// 删除车辆信息
        /// </summary>
        /// <param name="RecordID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DelWX_CarInfo(string RecordID)
        {
            StartLog($"删除车辆信息 {nameof(DelWX_CarInfo)} (RecordID={RecordID})");

            if (string.IsNullOrEmpty(RecordID))
            {
                return false;
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "delete WX_CarInfo where RecordID=@RecordID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", RecordID);
                    if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                    {
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"删除车辆信息 {nameof(DelWX_CarInfo)}", ex);
                return false;
            }
            finally
            {
                EndLog($"删除车辆信息 {nameof(DelWX_CarInfo)}");
            }
        }

        /// <summary>
        /// 根据账户ID获取车辆信息
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetCarInfoByAccountID(string AccountID)
        {
            StartLog($"根据账户ID获取车辆信息 {nameof(GetCarInfoByAccountID)} (AccountID={AccountID})");

            if (string.IsNullOrEmpty(AccountID))
            {
                return "";
            }
            List<WX_CarInfo> list = new List<WX_CarInfo>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select * from WX_CarInfo where AccountID=@AccountID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AccountID", AccountID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        while (reader.Read())
                        {
                            WX_CarInfo model = new WX_CarInfo();
                            model.AccountID = reader["AccountID"].ToString();
                            model.BindTime = reader["BindTime"].ToDateTime();
                            model.CarMonitor = reader["CarMonitor"].ToString();
                            model.ID = reader["ID"].ToInt();
                            model.PlateID = reader["PlateID"].ToString();
                            model.PlateNo = reader["PlateNo"].ToString();
                            model.RecordID = reader["RecordID"].ToString();
                            model.Status = reader["Status"].ToInt();
                            list.Add(model);

                        }
                        if (list.Count > 0)
                        {
                            return JsonHelper.GetJsonString(list);
                        }
                        else
                        {
                            return "";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"根据账户ID获取车辆信息 {nameof(GetCarInfoByAccountID)}", ex);
                return "";
            }
            finally
            {
                EndLog($"根据账户ID获取车辆信息 {nameof(GetCarInfoByAccountID)}");
            }
        }

        /// <summary>
        /// 根据车牌号码获取车辆信息
        /// </summary>
        /// <param name="PlateNo"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetCarInfoByPlateNo(string AccountID, string PlateNo)
        {
            StartLog($"根据车牌号码获取车辆信息 {nameof(GetCarInfoByPlateNo)} (AccountID={AccountID},PlateNo={PlateNo})");

            if (string.IsNullOrEmpty(AccountID) || string.IsNullOrEmpty(PlateNo))
            {
                return "";
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select * from WX_CarInfo where AccountID=@AccountID and PlateNo=@PlateNo";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AccountID", AccountID);
                    dbOperator.AddParameter("PlateNo", PlateNo);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            WX_CarInfo model = new WX_CarInfo();
                            model.AccountID = reader["AccountID"].ToString();
                            model.BindTime = reader["BindTime"].ToDateTime();
                            model.CarMonitor = reader["CarMonitor"].ToString();
                            model.ID = reader["ID"].ToInt();
                            model.PlateID = reader["PlateID"].ToString();
                            model.PlateNo = reader["PlateNo"].ToString();
                            model.RecordID = reader["RecordID"].ToString();
                            model.Status = reader["Status"].ToInt();
                            return JsonHelper.GetJsonString(model);
                        }
                        else
                        {
                            return "";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"根据车牌号码获取车辆信息 {nameof(GetCarInfoByPlateNo)}", ex);
                return "";
            }
            finally
            {
                EndLog($"根据车牌号码获取车辆信息 {nameof(GetCarInfoByPlateNo)}");
            }
        }

        /// <summary>
        /// 根据账户ID获取在场临停车辆
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        [OperationContract]
        public List<string> GetTempCarInfoIn(string AccountID)
        {
            StartLog($"根据账户ID获取在场临停车辆 {nameof(GetTempCarInfoIn)} (AccountID={AccountID})");

            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(AccountID))
            {
                return list;
            }
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select a.PlateNumber from ParkIORecord a left join WX_CarInfo b on a.PlateNumber=b.PlateNo ";
                    strsql += "left join ParkCarType c on a.CarTypeID=c.CarTypeID ";
                    strsql += " where b.AccountID=@AccountID and a.IsExit=0 and c.BaseTypeID=3 and c.DataStatus<2 group by a.PlateNumber";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AccountID", AccountID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        while (reader.Read())
                        {
                            list.Add(reader["PlateNumber"].ToString());
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                ExceptionLog($"根据账户ID获取在场临停车辆 {nameof(GetTempCarInfoIn)}", ex);
                return list;
            }
            finally
            {
                EndLog($"根据账户ID获取在场临停车辆 {nameof(GetTempCarInfoIn)}");
            }
        }

        /// <summary>
        /// 根据账户ID获取长期车信息
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetMonthCarInfoByAccountID(string AccountID)
        {
            StartLog($"根据账户ID获取长期车信息 {nameof(GetMonthCarInfoByAccountID)} (AccountID={AccountID})");

            if (string.IsNullOrEmpty(AccountID))
            {
                return "";
            }
            List<ParkUserCarInfo> list = new List<ParkUserCarInfo>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select c.CardID,c.State,c.Balance,f.PlateNo,d.BeginDate,d.EndDate,d.CarTypeID,d.PKID,e.BaseTypeID,e.IsAllowOnlIne,e.OnlineUnit, ";
                    strsql += "e.Amount,e.MaxMonth,e.MaxValue,g.PKName,g.VID,g.MobilePay,g.MobileLock from ";
                    strsql += " WX_Account a left join BaseEmployee b on a.MobilePhone=b.MobilePhone ";
                    strsql += "left join BaseCard c on  b.EmployeeID=c.EmployeeID left join ParkGrant d on c.CardID=d.CardID ";
                    strsql += "left join ParkCarType e on d.CarTypeID=e.CarTypeID left join EmployeePlate f on d.PlateID=f.PlateID ";
                    strsql += "left join BaseParkinfo g on d.PKID=g.PKID ";
                    strsql += "where a.AccountID=@AccountID  and b.DataStatus<2 and (e.BaseTypeID=1 or e.BaseTypeID=2) ";
                    strsql += "and c.DataStatus<2 and d.DataStatus<2 and e.DataStatus<2 and f.DataStatus<2 and g.DataStatus<2 ";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AccountID", AccountID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkUserCarInfo model = new ParkUserCarInfo();
                            model.Amount = reader["Amount"].ToDecimal();
                            model.Balance = reader["Balance"].ToDecimal();
                            model.BaseTypeID = reader["BaseTypeID"].ToString();
                            model.BeginDate = reader["BeginDate"].ToDateTime();
                            model.CardID = reader["CardID"].ToString();
                            model.CarTypeID = reader["CarTypeID"].ToString();
                            model.EndDate = reader["EndDate"].ToDateTime();
                            model.IsAllowOnlIne = reader["IsAllowOnlIne"].ToBoolean();
                            model.MaxMonth = reader["MaxMonth"].ToInt();
                            model.MaxValue = reader["MaxValue"].ToDecimal();
                            model.MobileLock = reader["MobileLock"].ToInt();
                            model.MobilePay = reader["MobilePay"].ToInt();
                            model.PKID = reader["PKID"].ToString();
                            model.PKName = reader["PKName"].ToString();
                            model.PlateNumber = reader["PlateNo"].ToString();
                            model.State = reader["State"].ToInt();
                            model.VID = reader["VID"].ToString();
                            model.OnlineUnit = reader["OnlineUnit"].ToInt();
                            list.Add(model);
                        }
                        if (list.Count > 0)
                        {
                            return JsonHelper.GetJsonString(list);
                        }
                        else
                        {
                            return "";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLog($"根据账户ID获取长期车信息 {nameof(GetMonthCarInfoByAccountID)}", ex);
                return "";
            }
            finally
            {
                EndLog($"根据账户ID获取长期车信息 {nameof(GetMonthCarInfoByAccountID)}");
            }
        }

        /// <summary>
        /// 根据车牌号码获取长期车信息
        /// </summary>
        /// <param name="PlateNumber"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetMonthCarInfoByPlateNumber(string PlateNumber)
        {
            StartLog($"根据车牌号码获取长期车信息 {nameof(GetMonthCarInfoByPlateNumber)} (PlateNumber={PlateNumber})");

            if (string.IsNullOrEmpty(PlateNumber))
            {
                return "";
            }
            List<ParkUserCarInfo> list = new List<ParkUserCarInfo>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select c.CardID,c.State,c.Balance,f.PlateNo,d.BeginDate,d.EndDate,d.CarTypeID,d.PKID,e.BaseTypeID,e.IsAllowOnlIne,e.OnlineUnit, ";
                    strsql += "e.Amount,e.MaxMonth,e.MaxValue,g.PKName,g.VID,g.MobilePay,g.MobileLock from ";
                    strsql += " BaseCard c  left join ParkGrant d on c.CardID=d.CardID ";
                    strsql += "left join ParkCarType e on d.CarTypeID=e.CarTypeID left join EmployeePlate f on d.PlateID=f.PlateID ";
                    strsql += "left join BaseParkinfo g on d.PKID=g.PKID ";
                    strsql += "where f.PlateNo=@PlateNumber  and (e.BaseTypeID=1 or e.BaseTypeID=2) ";
                    strsql += "and c.DataStatus<2 and d.DataStatus<2 and e.DataStatus<2 and f.DataStatus<2 and g.DataStatus<2 ";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PlateNumber", PlateNumber);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkUserCarInfo model = new ParkUserCarInfo();
                            model.Amount = reader["Amount"].ToDecimal();
                            model.Balance = reader["Balance"].ToDecimal();
                            model.BaseTypeID = reader["BaseTypeID"].ToString();
                            model.BeginDate = reader["BeginDate"].ToDateTime();
                            model.CardID = reader["CardID"].ToString();
                            model.CarTypeID = reader["CarTypeID"].ToString();
                            model.EndDate = reader["EndDate"].ToDateTime();
                            model.IsAllowOnlIne = reader["IsAllowOnlIne"].ToBoolean();
                            model.MaxMonth = reader["MaxMonth"].ToInt();
                            model.MaxValue = reader["MaxValue"].ToDecimal();
                            model.MobileLock = reader["MobileLock"].ToInt();
                            model.MobilePay = reader["MobilePay"].ToInt();
                            model.PKID = reader["PKID"].ToString();
                            model.PKName = reader["PKName"].ToString();
                            model.PlateNumber = reader["PlateNo"].ToString();
                            model.State = reader["State"].ToInt();
                            model.VID = reader["VID"].ToString();
                            model.OnlineUnit = reader["OnlineUnit"].ToInt();
                            list.Add(model);

                        }
                        if (list.Count > 0)
                        {
                            return JsonHelper.GetJsonString(list);
                        }
                        else
                        {
                            return "";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLog($"根据车牌号码获取长期车信息 {nameof(GetMonthCarInfoByPlateNumber)}", ex);
                return "";
            }
            finally
            {
                EndLog($"根据车牌号码获取长期车信息 {nameof(GetMonthCarInfoByPlateNumber)}");
            }
        }

        /// <summary>
        /// 根据车场ID获取车场信息
        /// </summary>
        /// <param name="PKID"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetBaseParkinfoByPKID(string PKID)
        {
            StartLog($"根据车场ID获取车场信息 {nameof(GetBaseParkinfoByPKID)} (PKID={PKID})");

            if (string.IsNullOrEmpty(PKID))
            {
                return "";
            }
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select * from BaseParkinfo where PKID=@PKID and DataStatus<2";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", PKID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            BaseParkinfo model = new BaseParkinfo();
                            model.Address = reader["Address"].ToString();
                            model.AllowLoseDisplay = (YesOrNo)reader["AllowLoseDisplay"].ToInt();
                            model.CarBitNum = reader["CarBitNum"].ToInt();
                            model.CarBitNumFixed = reader["CarBitNumFixed"].ToInt();
                            model.CarBitNumLeft = reader["CarBitNumLeft"].ToInt();
                            model.CenterTime = reader["CenterTime"].ToInt();
                            model.CityID = reader["CityID"].ToInt();
                            model.Coordinate = reader["Coordinate"].ToString();
                            model.DataSaveDays = reader["DataSaveDays"].ToInt();
                            model.DataStatus = (DataStatus)reader["DataStatus"].ToInt();
                            model.ExpiredAdvanceRemindDay = reader["ExpiredAdvanceRemindDay"].ToInt();
                            model.FeeRemark = reader["FeeRemark"].ToString();
                            model.HaveUpdate = reader["HaveUpdate"].ToInt();
                            model.ID = reader["ID"].ToInt();
                            model.IsParkingSpace = (YesOrNo)reader["IsParkingSpace"].ToInt();
                            model.IsReverseSeekingVehicle = (YesOrNo)reader["IsReverseSeekingVehicle"].ToInt();
                            model.LastUpdateTime = reader["LastUpdateTime"].ToDateTime();
                            model.LinkMan = reader["LinkMan"].ToString();
                            model.Mobile = reader["Mobile"].ToString();
                            model.MobileLock = (YesOrNo)reader["MobileLock"].ToInt();
                            model.MobilePay = (YesOrNo)reader["MobilePay"].ToInt();
                            model.NeedFee = (YesOrNo)reader["NeedFee"].ToInt();
                            model.OnLine = (YesOrNo)reader["OnLine"].ToInt();
                            model.PKID = reader["PKID"].ToString();
                            model.PKName = reader["PKName"].ToString();
                            model.PKNo = reader["PKNo"].ToString();
                            model.Remark = reader["Remark"].ToString();
                            model.SpaceBitNum = reader["SpaceBitNum"].ToInt();
                            model.VID = reader["VID"].ToString();
                            return JsonHelper.GetJsonString(model);
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"根据车场ID获取车场信息 {nameof(GetBaseParkinfoByPKID)}", ex);
                return "";
            }
            finally
            {
                EndLog($"根据车场ID获取车场信息 {nameof(GetBaseParkinfoByPKID)}");
            }

        }

        #endregion

        #region 锁车解锁

        /// <summary>
        /// 获取车辆锁车解锁状态
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetLockCarByAccountID(string AccountID)
        {
            StartLog($"获取车辆锁车解锁状态 {nameof(GetLockCarByAccountID)} (AccountID={AccountID})");

            if (string.IsNullOrEmpty(AccountID))
            {
                return "";
            }
            try
            {
                List<WX_LockCar> list = new List<WX_LockCar>();
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select PlateNo from WX_CarInfo  where AccountID=@AccountID and Status>0";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AccountID", AccountID);
                    List<string> plateno = new List<string>();
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        while (reader.Read())
                        {
                            plateno.Add(reader["PlateNo"].ToString());
                        }
                    }
                    foreach (var PlateNumber in plateno)
                    {
                        strsql = "select a.ParkingID,a.PlateNumber,b.VID from ParkIORecord a left join BaseParkinfo b on a.ParkingID=b.PKID where a.PlateNumber=@PlateNumber and MobileLock=1 and a.IsExit=0 and a.DataStatus<2 order by a.EntranceTime Desc";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PlateNumber", PlateNumber);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                string ParkingID = reader["ParkingID"].ToString();
                                string VID = reader["VID"].ToString();
                                reader.Close();
                                using (DbOperator dbOperator2 = ConnectionManager.CreateReadConnection())
                                {
                                    strsql = "select RecordID,Status,LockDate,PlateNumber,PKID from WX_LockCar  ";
                                    strsql += " where PlateNumber=@PlateNumber  and PKID=@PKID and DataStatus<2 ";
                                    dbOperator2.ClearParameters();
                                    dbOperator2.AddParameter("PlateNumber", PlateNumber);
                                    dbOperator2.AddParameter("PKID", ParkingID);
                                    using (DbDataReader reader2 = dbOperator2.ExecuteReader(strsql.ToString()))
                                    {
                                        if (reader2.Read())
                                        {
                                            WX_LockCar model = new WX_LockCar();
                                            model.PlateNumber = reader2["PlateNumber"].ToString();
                                            model.Status = reader2["Status"].ToInt();
                                            model.LockDate = reader2["LockDate"].ToDateTime();
                                            model.RecordID = reader2["RecordID"].ToString();
                                            model.PKID = reader2["PKID"].ToString();
                                            model.VID = VID;
                                            list.Add(model);

                                        }
                                        else
                                        {
                                            WX_LockCar model = new WX_LockCar();
                                            model.PlateNumber = PlateNumber;
                                            model.Status = 0;
                                            model.PKID = ParkingID;
                                            model.VID = VID;
                                            list.Add(model);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (list.Count > 0)
                {
                    return JsonHelper.GetJsonString(list);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"获取车辆锁车解锁状态 {nameof(GetLockCarByAccountID)}", ex);
                return "";
            }
            finally
            {
                EndLog($"获取车辆锁车解锁状态 {nameof(GetLockCarByAccountID)}");
            }
        }

        /// <summary>
        /// 锁车
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="PKID"></param>
        /// <param name="PlateNumber"></param>
        /// <returns>0成功,1代理断开,2失败</returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public int WXLockCarInfo(string AccountID, string PKID, string PlateNumber)
        {
            StartLog($"锁车 {nameof(WXLockCarInfo)} (AccountID={AccountID},PKID={PKID},PlateNumber={PlateNumber})");

            if (string.IsNullOrEmpty(AccountID) || string.IsNullOrEmpty(PKID) || string.IsNullOrEmpty(PlateNumber))
            {
                return 2;
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select b.ProxyNo,b.IsBoxWatch,b.VID from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKID=@PKID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", PKID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                return new WXBoxServiceCallback().LockCarInfo(AccountID, PKID, PlateNumber, reader["VID"].ToString());
                            }
                            else
                            {
                                return new WXServiceCallback().LockCarInfo(AccountID, PKID, PlateNumber, reader["ProxyNo"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"锁车 {nameof(WXLockCarInfo)}", ex);
                return 2;
            }
            finally
            {
                EndLog($"锁车 {nameof(WXLockCarInfo)}");
            }

            return 2;
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="PKID"></param>
        /// <param name="PlateNumber"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public int WXUlockCarInfo(string AccountID, string PKID, string PlateNumber)
        {
            StartLog($"解锁 {nameof(WXUlockCarInfo)} (AccountID={AccountID},PKID={PKID},PlateNumber={PlateNumber})");

            if (string.IsNullOrEmpty(AccountID) || string.IsNullOrEmpty(PKID) || string.IsNullOrEmpty(PlateNumber))
            {
                return 2;
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select b.ProxyNo,b.IsBoxWatch,b.VID from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKID=@PKID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", PKID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                return new WXBoxServiceCallback().UlockCarInfo(AccountID, PKID, PlateNumber, reader["VID"].ToString());
                            }
                            else
                            {
                                return new WXServiceCallback().UlockCarInfo(AccountID, PKID, PlateNumber, reader["ProxyNo"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"解锁 {nameof(WXUlockCarInfo)}", ex);
                return 2;
            }
            finally
            {
                EndLog($"解锁 {nameof(WXUlockCarInfo)}");
            }

            return 2;
        }
        #endregion

        #region 车辆缴费

        /// <summary>
        /// 判断订单是否有效0订单已失效、1正常订单、2重复支付、3其他异常
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public int OrderWhetherEffective(string OrderNo)
        {
            StartLog($"判断订单是否有效 {nameof(OrderWhetherEffective)} (OrderNo={OrderNo})");

            if (string.IsNullOrEmpty(OrderNo))
            {
                return 0;
            }
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select Status from ParkOrder where OrderNo=@OrderNo ";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("OrderNo", OrderNo);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["Status"] != null && int.Parse(reader["Status"].ToString()) == -1)
                            {
                                return 0;
                            }
                            else if (reader["Status"] != null && int.Parse(reader["Status"].ToString()) == 1)
                            {
                                return 2;
                            }
                            else
                            {
                                return 1;
                            }
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"判断订单是否有效 {nameof(OrderWhetherEffective)}", ex);
                return 3;
            }
            finally
            {
                EndLog($"判断订单是否有效 {nameof(OrderWhetherEffective)}");
            }
        }

        /// <summary>
        /// 判断订单是否有效0订单已失效、1正常订单、2重复支付、3其他异常
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="ParkID"></param>
        /// <param name="IORecordID"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public int OrderWhetherEffectiveNew(string OrderNo, string ParkID, string IORecordID)
        {
            StartLog($"判断订单是否有效 {nameof(OrderWhetherEffectiveNew)} (OrderNo={OrderNo},ParkID={ParkID},IORecordID={IORecordID})");

            if (string.IsNullOrEmpty(OrderNo) || ParkID.IsEmpty() || IORecordID.IsEmpty())
            {
                return 0;
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select Status,TagID,PKID from ParkOrder where OrderNo=@OrderNo ";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("OrderNo", OrderNo);

                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        while (reader.Read())
                        {
                            if (reader["TagID"] != null && reader["TagID"].ToString() != IORecordID)
                            {
                                continue;
                            }

                            if (reader["PKID"] != null && reader["PKID"].ToString() != ParkID)
                            {
                                continue;
                            }

                            if (reader["Status"] != null && int.Parse(reader["Status"].ToString()) == -1)
                            {
                                return 0;
                            }
                            else if (reader["Status"] != null && int.Parse(reader["Status"].ToString()) == 1)
                            {
                                return 2;
                            }
                            else
                            {
                                return 1;
                            }
                        }

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"判断订单是否有效 {nameof(OrderWhetherEffectiveNew)}", ex);
                return 3;
            }
            finally
            {
                EndLog($"判断订单是否有效 {nameof(OrderWhetherEffectiveNew)}");
            }
        }

        /// <summary>
        /// 月租车辆续期
        /// </summary>
        /// <param name="PlateNumber"></param>
        /// <param name="ParkingID"></param>
        /// <param name="MonthNum"></param>
        /// <param name="Amount"></param>
        /// <param name="AccountID"></param>
        /// <param name="PayWay"></param>
        /// <param name="OrderSource"></param>
        /// <param name="OnlineOrderID"></param>
        /// <param name="PayDate"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public string WXMonthlyRenewals(string CardID, string PKID, int MonthNum, decimal Amount, string AccountID, int PayWay, int OrderSource, string OnlineOrderID, DateTime PayDate)
        {
            StartLog($"月租车辆续期 {nameof(WXMonthlyRenewals)} (CardID={CardID},PKID={PKID},MonthNum={MonthNum},Amount={Amount},AccountID={AccountID},PayWay={PayWay},OrderSource={OrderSource},OnlineOrderID={OnlineOrderID},PayDate={PayDate})");

            MonthlyRenewalResult result = new MonthlyRenewalResult();
            if (string.IsNullOrEmpty(PKID) || string.IsNullOrEmpty(CardID))
            {
                result.Result = APPResult.OtherException;
                return JsonHelper.GetJsonString(result);
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select b.ProxyNo,b.IsBoxWatch,b.VID from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKID=@PKID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", PKID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                WXBoxServiceCallback callback = new WXBoxServiceCallback();
                                result = callback.MonthlyRenewals(CardID, PKID, MonthNum, Amount, AccountID, PayWay, OrderSource, OnlineOrderID, PayDate, reader["VID"].ToString());
                                if (result.Result != APPResult.Normal)
                                {
                                    logger.Fatal("月租车辆续期失败：MonthlyRenewals()卡ID:" + CardID + "原因:" + result.Result + "\r\n");
                                }
                                return JsonHelper.GetJsonString(result);
                            }
                            else
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                result = callback.MonthlyRenewals(CardID, PKID, MonthNum, Amount, AccountID, PayWay, OrderSource, OnlineOrderID, PayDate, reader["ProxyNo"].ToString());
                                if (result.Result != APPResult.Normal)
                                {
                                    logger.Fatal("月租车辆续期失败：MonthlyRenewals()卡ID:" + CardID + "原因:" + result.Result + "\r\n");
                                }
                                return JsonHelper.GetJsonString(result);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"月租车辆续期 {nameof(WXMonthlyRenewals)}", ex);
                result.Result = APPResult.OtherException;
                return JsonHelper.GetJsonString(result);
            }
            finally
            {
                EndLog($"月租车辆续期 {nameof(WXMonthlyRenewals)}");
            }
            result.Result = APPResult.OtherException;
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 获取临停费用
        /// </summary>
        /// <param name="PlateNumber"></param>
        /// <param name="PKID"></param>
        /// <param name="CalculatDate"></param>
        /// <param name="AccountID"></param>
        /// <param name="OrderSource"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public string WXTempParkingFee(string PlateNumber, string PKID, DateTime CalculatDate, string AccountID, int OrderSource)
        {
            StartLog($"获取临停费用 {nameof(WXTempParkingFee)} (PlateNumber={PlateNumber},PKID={PKID},CalculatDate={CalculatDate},AccountID={AccountID},OrderSource={OrderSource})");

            TempParkingFeeResult result = new TempParkingFeeResult();
            if (string.IsNullOrEmpty(PlateNumber))
            {
                result.Result = APPResult.OtherException;
                return JsonHelper.GetJsonString(result);
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "";
                    strsql = " select a.RecordID,b.PKID,a.PlateNumber,a.EntranceTime,b.PKName,b.MobilePay,e.ProxyNo,b.CenterTime,e.IsBoxWatch,b.VID ";
                    strsql += " from ParkIORecord a left join BaseParkinfo b on a.ParkingID=b.PKID left join BaseVillage e on b.VID=e.VID  ";
                    strsql += " where a.PlateNumber=@PlateNumber and a.IsExit=0 ";
                    strsql += "  ORDER BY a.EntranceTime DESC";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PlateNumber", PlateNumber);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                WXBoxServiceCallback callback = new WXBoxServiceCallback();
                                result = callback.TempParkingFee(reader["RecordID"].ToString(), reader["PKID"].ToString(), OrderSource, CalculatDate, reader["VID"].ToString(), AccountID, reader["CenterTime"].ToInt(), PlateNumber);
                                return JsonHelper.GetJsonString(result);
                            }
                            else
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                result = callback.TempParkingFee(reader["RecordID"].ToString(), reader["PKID"].ToString(), OrderSource, CalculatDate, reader["ProxyNo"].ToString(), AccountID, reader["CenterTime"].ToInt(), PlateNumber);

                                return JsonHelper.GetJsonString(result);
                            }
                        }
                        else
                        {
                            result.Result = APPResult.NotFindIn;
                            return JsonHelper.GetJsonString(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"获取临停费用 {nameof(WXTempParkingFee)}", ex);
                result.Result = APPResult.OtherException;
                return JsonHelper.GetJsonString(result);
            }
            finally
            {
                EndLog($"获取临停费用 {nameof(WXTempParkingFee)}");
            }
        }

        /// <summary>
        /// 临时卡缴费
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="PayWay"></param>
        /// <param name="Amount"></param>
        /// <param name="PKID"></param>
        /// <param name="OnlineOrderID"></param>
        /// <param name="AccountID"></param>
        /// <param name="PayDate"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public string WXTempStopPayment(string OrderID, int PayWay, decimal Amount, string PKID, string OnlineOrderID, string AccountID, DateTime PayDate)
        {
            StartLog($"临时卡缴费 {nameof(WXTempStopPayment)} (OrderID={OrderID},PayWay={PayWay},Amount={Amount},PKID={PKID},OnlineOrderID={OnlineOrderID},AccountID={AccountID},PayDate={PayDate})");

            TempStopPaymentResult result = new TempStopPaymentResult();
            if (string.IsNullOrEmpty(OrderID) || string.IsNullOrEmpty(PKID))
            {
                result.Result = APPResult.OtherException;
                return JsonHelper.GetJsonString(result);
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select b.ProxyNo,b.IsBoxWatch,b.VID from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKID=@PKID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", PKID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                WXBoxServiceCallback callback = new WXBoxServiceCallback();
                                result = callback.TempStopPayment(OrderID, PayWay, Amount, PKID, reader["VID"].ToString(), OnlineOrderID, PayDate);
                                if (result.Result != APPResult.Normal)
                                {
                                    logger.Fatal("临时卡缴费失败：WXTempStopPayment()订单号:" + OrderID + "金额:" + Amount + "原因:" + result.Result + "\r\n");
                                }
                                return JsonHelper.GetJsonString(result);
                            }
                            else
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                result = callback.TempStopPayment(OrderID, PayWay, Amount, PKID, reader["ProxyNo"].ToString(), OnlineOrderID, PayDate);
                                if (result.Result != APPResult.Normal)
                                {
                                    logger.Fatal("临时卡缴费失败：WXTempStopPayment()订单号:" + OrderID + "金额:" + Amount + "原因:" + result.Result + "\r\n");
                                }
                                return JsonHelper.GetJsonString(result);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"临时卡缴费 {nameof(WXTempStopPayment)}", ex);

                result.Result = APPResult.OtherException;
                return JsonHelper.GetJsonString(result);
            }
            finally
            {
                EndLog($"临时卡缴费 {nameof(WXTempStopPayment)}");
            }

            result.Result = APPResult.OtherException;
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 扫码获取临停订单信息（岗亭）
        /// </summary>
        /// <param name="BoxID"></param>
        /// <param name="AccountID"></param>
        /// <param name="OrderSource"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public string WXScanCodeTempParkingFee(string BoxID, string AccountID, int OrderSource)
        {
            StartLog($"扫码获取临停订单信息（岗亭） {nameof(WXScanCodeTempParkingFee)} (BoxID={BoxID},AccountID={AccountID},OrderSource={OrderSource})");

            TempParkingFeeResult result = new TempParkingFeeResult();
            if (string.IsNullOrEmpty(BoxID))
            {
                result.Result = APPResult.OtherException;
                return JsonHelper.GetJsonString(result);
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "";
                    strsql = " select c.PKID,c.PKName,c.MobilePay,c.CenterTime,d.ProxyNo,d.IsBoxWatch,d.VID from ParkBox a left join ";
                    strsql += " ParkArea b on a.AreaID=b.AreaID left join BaseParkinfo c   ";
                    strsql += " on b.pkid=c.pkid left join BaseVillage d on c.VID=d.VID ";

                    strsql += "  where a.BoxID=@BoxID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("BoxID", BoxID);

                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                WXBoxServiceCallback callback = new WXBoxServiceCallback();
                                result = callback.ScanCodeTempParkingFee(reader["PKID"].ToString(), reader["PKName"].ToString(), OrderSource, BoxID, reader["VID"].ToString(), AccountID, reader["CenterTime"].ToInt());
                                return JsonHelper.GetJsonString(result);
                            }
                            else
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                result = callback.ScanCodeTempParkingFee(reader["PKID"].ToString(), reader["PKName"].ToString(), OrderSource, BoxID, reader["ProxyNo"].ToString(), AccountID, reader["CenterTime"].ToInt());
                                return JsonHelper.GetJsonString(result);
                            }
                        }
                        else
                        {
                            result.Result = APPResult.NoBox;
                            return JsonHelper.GetJsonString(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"扫码获取临停订单信息（岗亭） {nameof(WXScanCodeTempParkingFee)}", ex);

                result.Result = APPResult.OtherException;
                return JsonHelper.GetJsonString(result);
            }
            finally
            {
                EndLog($"扫码获取临停订单信息（岗亭） {nameof(WXScanCodeTempParkingFee)}");
            }
        }

        /// <summary>
        /// 扫码获取临停订单信息（通道）
        /// </summary>
        /// <param name="GateID"></param>
        /// <param name="AccountID"></param>
        /// <param name="OrderSource"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public string WXScanCodeTempParkingFeeByGateID(string GateID, string AccountID, int OrderSource)
        {
            StartLog($"扫码获取临停订单信息（通道） {nameof(WXScanCodeTempParkingFeeByGateID)} (GateID={GateID},AccountID={AccountID},OrderSource={OrderSource})");

            TempParkingFeeResult result = new TempParkingFeeResult();
            if (string.IsNullOrEmpty(GateID))
            {
                result.Result = APPResult.OtherException;
                return JsonHelper.GetJsonString(result);
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "";
                    strsql = " select f.BoxID ,c.PKID,c.PKName,c.MobilePay,c.CenterTime,d.ProxyNo,d.IsBoxWatch,d.VID from ParkGate f left join ParkBox a on  f.BoxID=a.BoxID left join ";
                    strsql += " ParkArea b on a.AreaID=b.AreaID left join BaseParkinfo c   ";
                    strsql += " on b.pkid=c.pkid left join BaseVillage d on c.VID=d.VID ";

                    strsql += "  where f.GateID=@GateID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("GateID", GateID);

                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                WXBoxServiceCallback callback = new WXBoxServiceCallback();
                                result = callback.ScanCodeTempParkingFeebyGateID(reader["PKID"].ToString(), reader["PKName"].ToString(), OrderSource, GateID, reader["VID"].ToString(), AccountID, reader["CenterTime"].ToInt(), reader["BoxID"].ToString());
                                return JsonHelper.GetJsonString(result);
                            }
                            else
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                result = callback.ScanCodeTempParkingFeebyGateID(reader["PKID"].ToString(), reader["PKName"].ToString(), OrderSource, GateID, reader["ProxyNo"].ToString(), AccountID, reader["CenterTime"].ToInt());
                                return JsonHelper.GetJsonString(result);
                            }
                        }
                        else
                        {
                            result.Result = APPResult.NoBox;
                            return JsonHelper.GetJsonString(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"扫码获取临停订单信息（通道） {nameof(WXScanCodeTempParkingFeeByGateID)}", ex);

                result.Result = APPResult.OtherException;

                return JsonHelper.GetJsonString(result);
            }
            finally
            {
                EndLog($"扫码获取临停订单信息（通道） {nameof(WXScanCodeTempParkingFeeByGateID)}");
            }
        }
        #endregion

        #region 缴费纪录查询

        /// <summary>
        /// 根据账户ID获取临停缴费纪录
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetPkOrderTempByAccountID(string AccountID)
        {
            StartLog($"根据账户ID获取临停缴费纪录 {nameof(GetPkOrderTempByAccountID)} (AccountID={AccountID})");

            if (string.IsNullOrEmpty(AccountID))
            {
                return "";
            }
            List<PkOrderTemp> list = new List<PkOrderTemp>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select a.OnlineOrderNo,d.PKName,c.PlateNumber, c.EntranceTime,a.PayTime,";
                    strsql += "datediff(MINUTE,c.EntranceTime,a.OrderTime)as Minutenum,a.PayAmount,a.OrderTime ";
                    strsql += "from ParkOrder a  left join ParkIORecord c on a.TagID=c.RecordID  and a.PKID=c.ParkingID ";
                    strsql += "left join BaseParkinfo d on a.PKID=d.PKID ";
                    strsql += "where a.OrderType=1  and a.Status=1 and a.OrderSource=1 and a.OnlineUserID=@OnlineUserID order by a.OrderTime Desc ";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("OnlineUserID", AccountID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        while (reader.Read())
                        {
                            PkOrderTemp model = new PkOrderTemp();
                            model.Amount = reader["PayAmount"].ToDecimal();
                            model.EntranceDate = reader["EntranceTime"].ToDateTime();
                            model.MinuteNum = reader["Minutenum"].ToInt();
                            model.OnlineOrderNo = reader["OnlineOrderNo"].ToString();
                            model.PayTime = reader["PayTime"].ToDateTime();
                            model.PKName = reader["PKName"].ToString();
                            model.PlateNumber = reader["PlateNumber"].ToString();
                            list.Add(model);
                        }
                        if (list.Count > 0)
                        {
                            return JsonHelper.GetJsonString(list);
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"根据账户ID获取临停缴费纪录 {nameof(GetPkOrderTempByAccountID)}", ex);
                return "";
            }
            finally
            {
                EndLog($"根据账户ID获取临停缴费纪录 {nameof(GetPkOrderTempByAccountID)}");
            }

        }

        /// <summary>
        /// 根据账户ID获取月卡续期纪录
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetPkOrderMonthByAccountID(string AccountID)
        {
            StartLog($"根据账户ID获取月卡续期纪录 {nameof(GetPkOrderMonthByAccountID)} (AccountID={AccountID})");

            if (string.IsNullOrEmpty(AccountID))
            {
                return "";
            }
            List<PkOrderMonth> list = new List<PkOrderMonth>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select a.OnlineOrderNo,c.PlateNo,d.PKName,e.CarTypeName,datediff(MONTH,OldUserulDate,NewUsefulDate)as MonthNum,";
                    strsql += "a.NewUsefulDate,a.PayAmount,a.PayTime ";
                    strsql += "from ParkOrder a left join ParkGrant b on a.TagID=b.GID left join EmployeePlate c on b.PlateID=c.PlateID ";
                    strsql += "left join BaseParkinfo d on b.PKID=d.PKID left join ParkCarType e on b.CarTypeID=e.CarTypeID ";
                    strsql += "where a.OnlineUserID=@OnlineUserID and b.DataStatus<2 and e.BaseTypeID=2 and a.OrderType=2 and OrderSource=1 ";
                    strsql += " and a.Status=1  order by a.OrderTime Desc ";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("OnlineUserID", AccountID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        while (reader.Read())
                        {
                            PkOrderMonth model = new PkOrderMonth();
                            model.CarTypeName = reader["CarTypeName"].ToString();
                            model.MonthNum = reader["MonthNum"].ToInt();
                            model.NewUsefulDate = reader["NewUsefulDate"].ToDateTime();
                            model.OnlineOrderNo = reader["OnlineOrderNo"].ToString();
                            model.PayAmount = reader["PayAmount"].ToDecimal();
                            model.PKName = reader["PKName"].ToString();
                            model.PlateNumber = reader["PlateNo"].ToString();
                            model.PayTime = reader["PayTime"].ToDateTime();
                            list.Add(model);
                        }
                        if (list.Count > 0)
                        {
                            return JsonHelper.GetJsonString(list);
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"根据账户ID获取月卡续期纪录 {nameof(GetPkOrderMonthByAccountID)}", ex);
                return "";
            }
            finally
            {
                EndLog($"根据账户ID获取月卡续期纪录 {nameof(GetPkOrderMonthByAccountID)}");
            }
        }
        #endregion

        #region 消费打折
        /// <summary>
        /// 获取商户打折信息
        /// </summary>
        /// <param name="sellerID"></param>
        /// <param name="VID"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public string WXGetParkDerate(string sellerID, string VID, string ProxyNo)
        {
            StartLog($"获取商户打折信息 {nameof(WXGetParkDerate)} (sellerID={sellerID},VID={VID},ProxyNo={ProxyNo})");

            string strderate = "";
            if (string.IsNullOrEmpty(sellerID) || string.IsNullOrEmpty(VID))
            {
                return strderate;
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select ProxyNo,IsBoxWatch,VID from BaseVillage where DataStatus<2 and VID=@VID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("VID", VID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                WXBoxServiceCallback callback = new WXBoxServiceCallback();
                                return callback.GetParkDerate(sellerID, VID, reader["VID"].ToString());
                            }
                            else
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                return callback.GetParkDerate(sellerID, VID, reader["ProxyNo"].ToString());
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLog($"获取商户打折信息 {nameof(WXGetParkDerate)}", ex);
            }
            finally
            {
                EndLog($"获取商户打折信息 {nameof(WXGetParkDerate)}");
            }

            return strderate;
        }

        /// <summary>
        /// 获取车牌进场信息
        /// </summary>
        /// <param name="PlateNumber"></param>
        /// <param name="VID"></param>
        /// <param name="SellerID"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public string WXGetIORecordByPlateNumber(string PlateNumber, string VID, string SellerID, string ProxyNo)
        {
            StartLog($"获取车牌进场信息 {nameof(WXGetIORecordByPlateNumber)} (PlateNumber={PlateNumber},VID={VID},SellerID={SellerID},ProxyNo={ProxyNo})");

            string strderate = "";
            if (string.IsNullOrEmpty(SellerID) || string.IsNullOrEmpty(VID))
            {
                return strderate;
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select ProxyNo,IsBoxWatch,VID from BaseVillage where DataStatus<2 and VID=@VID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("VID", VID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                WXBoxServiceCallback callback = new WXBoxServiceCallback();
                                return callback.GetPKIORecordByPlateNumber(PlateNumber, VID, SellerID, reader["VID"].ToString());
                            }
                            else
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                return callback.GetPKIORecordByPlateNumber(PlateNumber, VID, SellerID, reader["ProxyNo"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"获取车牌进场信息 {nameof(WXGetIORecordByPlateNumber)}", ex);
            }
            finally
            {
                EndLog($"获取车牌进场信息 {nameof(WXGetIORecordByPlateNumber)}");
            }

            return strderate;
        }

        /// <summary>
        /// 消费打折
        /// </summary>
        /// <param name="IORecordID"></param>
        /// <param name="DerateID"></param>
        /// <param name="VillageID"></param>
        /// <param name="SellerID"></param>
        /// <param name="DerateMoney"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public string WXDiscountPlateNumber(string IORecordID, string DerateID, string VID, string SellerID, decimal DerateMoney, string ProxyNo)
        {
            StartLog($"消费打折 {nameof(WXDiscountPlateNumber)} (IORecordID={IORecordID},DerateID={DerateID},VID={VID}),SellerID={SellerID},DerateMoney={DerateMoney},ProxyNo={ProxyNo}");

            string strderate = "";
            if (string.IsNullOrEmpty(SellerID) || string.IsNullOrEmpty(VID) || string.IsNullOrEmpty(IORecordID) || string.IsNullOrEmpty(DerateID))
            {
                return strderate;
            }

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select ProxyNo,IsBoxWatch,VID from BaseVillage where DataStatus<2 and VID=@VID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("VID", VID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                WXBoxServiceCallback callback = new WXBoxServiceCallback();
                                return callback.DiscountPlateNumber(IORecordID, DerateID, VID, SellerID, DerateMoney, reader["VID"].ToString());
                            }
                            else
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                return callback.DiscountPlateNumber(IORecordID, DerateID, VID, SellerID, DerateMoney, reader["ProxyNo"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"消费打折 {nameof(WXDiscountPlateNumber)}", ex);
            }
            finally
            {
                EndLog($"消费打折 {nameof(WXDiscountPlateNumber)}");
            }

            return strderate;
        }

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="SellerNo"></param>
        /// <param name="PWD"></param>
        /// <param name="SellerID"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public string WXGetSellerInfo(string SellerNo, string PWD, string SellerID, string ProxyNo)
        {
            StartLog($"获取商户信息 {nameof(WXGetSellerInfo)} (SellerNo={SellerNo},PWD={PWD},SellerID={SellerID},ProxyNo={ProxyNo})");

            string strderate = "";
            try
            {
                return ProxyCallBackDLL.GetSellerInfo2(SellerNo, PWD, SellerID);
            }
            catch (Exception ex)
            {
                ExceptionLog($"获取商户信息 {nameof(WXGetSellerInfo)}", ex);
            }
            finally
            {
                EndLog($"获取商户信息 {nameof(WXGetSellerInfo)}");
            }

            return strderate;
        }

        /// <summary>
        /// 修改商户密码
        /// </summary>
        /// <param name="SellerID"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public bool WXEditSellerPwd(string SellerID, string Pwd, string ProxyNo)
        {
            StartLog($"修改商户密码 {nameof(WXEditSellerPwd)} (SellerID={SellerID},Pwd={Pwd},ProxyNo={ProxyNo})");

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select a.SellerID,b.ProxyNo,b.IsBoxWatch,b.VID from ParkSeller a left join  BaseVillage b on a.VID=b.VID where a.DataStatus<2 and b.DataStatus<2";
                    strsql += "and a.SellerID=@SellerID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("SellerID", SellerID);

                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            SellerID = reader["SellerID"].ToString();
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                WXBoxServiceCallback callback = new WXBoxServiceCallback();
                                return callback.EditSellerPwd(SellerID, Pwd, reader["VID"].ToString());
                            }
                            else
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                return callback.EditSellerPwd(SellerID, Pwd, reader["ProxyNo"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"修改商户密码 {nameof(WXEditSellerPwd)}", ex);
                return false;
            }
            finally
            {
                EndLog($"修改商户密码 {nameof(WXEditSellerPwd)}");
            }

            return false;

        }

        /// <summary>
        /// 获取商家打折详情
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public string WXGetParkCarDerate(string parms, int rows, int pageindex, string ProxyNo)
        {
            StartLog($"获取商家打折详情 {nameof(WXGetParkCarDerate)} (parms={parms},rows={rows},pageindex={pageindex},ProxyNo={ProxyNo})");

            try
            {
                InParams inparams = JsonHelper.GetJson<InParams>(parms);
                Pagination pagination = StatisticsServices.Search_CarDerates(inparams, rows, pageindex);
                return JsonHelper.GetJsonString(pagination);
            }
            catch (Exception ex)
            {
                ExceptionLog($"获取商家打折详情 {nameof(WXGetParkCarDerate)}", ex);
                return null;
            }
            finally
            {
                EndLog($"获取商家打折详情 {nameof(WXGetParkCarDerate)}");
            }
        }

        /// <summary>
        /// 获取商家打折详情
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public string WXGetParkCarDerateByVID(string parms, int rows, int pageindex, string ProxyNo)
        {
            StartLog($"获取商家打折详情 {nameof(WXGetParkCarDerateByVID)} (parms={parms},rows={rows},pageindex={pageindex},ProxyNo={ProxyNo})");

            try
            {
                InParams inparams = JsonHelper.GetJson<InParams>(parms);
                Pagination pagination = StatisticsServices.Search_CarDerates(inparams, rows, pageindex);
                return JsonHelper.GetJsonString(pagination);
            }
            catch (Exception ex)
            {
                ExceptionLog($"获取商家打折详情 {nameof(WXGetParkCarDerateByVID)}", ex);
                return null;
            }
            finally
            {
                EndLog($"获取商家打折详情 {nameof(WXGetParkCarDerateByVID)}");
            }
        }
        #endregion
        
        #region 扫码出入场

        /// <summary>
        /// 扫码出入场
        /// </summary>
        /// <param name="PKID"></param>
        /// <param name="GateNo"></param>
        /// <param name="OpenId"></param>
        /// <returns>0进成功、1出成功、1001参数错误、1002扫码入场失败、1003代理连接断开，1004通道无车</returns>
        [OperationContract]
        public int WXScanCodeInOut(string PKID, string GateNo, string OpenId)
        {
            StartLog($"扫码出入场 {nameof(WXScanCodeInOut)} (PKID={PKID},GateNo={GateNo},OpenId={OpenId})");

            if (string.IsNullOrEmpty(PKID) || string.IsNullOrEmpty(OpenId))
            {
                return 1001;
            }
            try
            {
                string AccountID = "";
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select b.AccountID from WX_Info a left join WX_Account b on a.AccountID=B.AccountID where OpenID=@OpenID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("OpenId", OpenId);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            AccountID = reader["AccountID"].ToString();
                        }
                        reader.Close();
                    }

                    if (AccountID != "")//是否有月卡
                    {
                        strsql = "select c.CardID,c.State,c.Balance,f.PlateNo,d.BeginDate,d.EndDate,d.CarTypeID,d.PKID,e.BaseTypeID,e.IsAllowOnlIne, ";
                        strsql += "e.Amount,e.MaxMonth,e.MaxValue,g.PKName,g.VID,g.MobilePay,g.MobileLock,h.ProxyNo,h.IsBoxWatch from ";
                        strsql += " WX_Account a left join BaseEmployee b on a.MobilePhone=b.MobilePhone ";
                        strsql += "left join BaseCard c on  b.EmployeeID=c.EmployeeID left join ParkGrant d on c.CardID=d.CardID ";
                        strsql += "left join ParkCarType e on d.CarTypeID=e.CarTypeID left join EmployeePlate f on d.PlateID=f.PlateID ";
                        strsql += "left join BaseParkinfo g on d.PKID=g.PKID left join BaseVillage h on g.VID=h.VID ";
                        strsql += "where a.AccountID=@AccountID  and b.DataStatus<2 and (e.BaseTypeID=1 or e.BaseTypeID=2) ";
                        strsql += "and c.DataStatus<2 and d.DataStatus<2 and e.DataStatus<2 and f.DataStatus<2 and g.DataStatus<2 and g.PKID=@PKID ";
                        strsql += "and d.BeginDate<=getdate() and d.EndDate>=getdate()";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("AccountID", AccountID);
                        dbOperator.AddParameter("PKID", PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())//有月卡
                            {
                                if (reader["IsBoxWatch"].ToBoolean())
                                {
                                    WXBoxServiceCallback callback = new WXBoxServiceCallback();
                                    return callback.ScanCodeInOut(reader["VID"].ToString(), GateNo, OpenId, reader["PlateNo"].ToString());
                                }
                                else
                                {
                                    WXServiceCallback callback = new WXServiceCallback();
                                    return callback.ScanCodeInOut(reader["ProxyNo"].ToString(), GateNo, OpenId, reader["PlateNo"].ToString());
                                }
                            }
                        }
                    }

                    strsql = "select b.ProxyNo,b.IsBoxWatch,b.VID from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKID=@PKID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", PKID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                WXBoxServiceCallback callback = new WXBoxServiceCallback();
                                return callback.ScanCodeInOut(reader["VID"].ToString(), GateNo, OpenId, "无车牌");
                            }
                            else
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                return callback.ScanCodeInOut(reader["ProxyNo"].ToString(), GateNo, OpenId, "无车牌");
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"扫码出入场 {nameof(WXScanCodeInOut)}", ex);
            }
            finally
            {
                EndLog($"扫码出入场 {nameof(WXScanCodeInOut)}");
            }
            return 1002;
        }
        #endregion

        #region 来访管理

        /// <summary>
        /// 根据账户获取访客信息
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetParkVisitor(string AccountID)
        {
            var callCtx = $"根据账户获取访客信息 {nameof(GetParkVisitor)} (AccountID={AccountID})";

            List<ParkVisitor> list = new List<ParkVisitor>();
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = "select * from ParkVisitor where RegAccountID=@RegAccountID ";
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RegAccountID", AccountID);
                using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkVisitor model = new ParkVisitor();
                        list.Add(model);
                    }
                }
            }
            return JsonHelper.GetJsonString(list);
        }

        /// <summary>
        /// 增加访客信息
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        [OperationContract]
        public string AddParkVisitor(string jsondata)
        {
            StartLog($"增加访客信息 {nameof(AddParkVisitor)} (jsondata={jsondata}");

            try
            {
                ParkVisitor vismodel = JsonHelper.GetJson<ParkVisitor>(jsondata);
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "insert into ParkVisitor(RecordID,VisitorID,PKID,VID,LastUpdateTime,HaveUpdate,DataStatus,AlreadyVisitorCount,BeginTime,EndTime,RegAccountID,PlateNumber)";
                    strsql += "values(@RecordID,@VisitorID,@PKID,@VID,@LastUpdateTime,@HaveUpdate,@DataStatus,@AlreadyVisitorCount,@BeginTime,@EndTime,@RegAccountID,@PlateNumber)";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", vismodel.RecordID);
                    dbOperator.AddParameter("VisitorID", vismodel.VisitorID);
                    dbOperator.AddParameter("PKID", vismodel.PKID);
                    dbOperator.AddParameter("VID", vismodel.VID);
                    dbOperator.AddParameter("LastUpdateTime", vismodel.LastUpdateTime);
                    dbOperator.AddParameter("HaveUpdate", vismodel.HaveUpdate);
                    dbOperator.AddParameter("DataStatus", vismodel.DataStatus);
                    dbOperator.AddParameter("AlreadyVisitorCount", vismodel.AlreadyVisitorCount);
                    dbOperator.AddParameter("BeginTime", vismodel.BeginTime);
                    dbOperator.AddParameter("EndTime", vismodel.EndTime);
                    dbOperator.AddParameter("RegAccountID", vismodel.RegAccountID);
                    dbOperator.AddParameter("PlateNumber", vismodel.PlateNumber);

                }
                //ParkVisitorServices.Add(vismodel);
            }
            catch (Exception ex)
            {
                ExceptionLog($"增加访客信息 {nameof(AddParkVisitor)}", ex);
            }
            finally
            {
                EndLog($"增加访客信息 {nameof(AddParkVisitor)}");
            }
            return "0";
        }

        #endregion

        #region 车场运营

        /// <summary>
        /// 远程开闸
        /// </summary>
        /// <param name="UserID">用户GUID</param>
        /// <param name="PKID">车场ID</param>
        /// <param name="GateID">通道ID</param>
        /// <param name="Remark">备注</param>
        /// <returns>0成功、1车场网络异常、2通道不支持远程开闸、3开闸失败</returns>
        [OperationContract]
        public int WXRemoteGate(string UserID, string PKID, string GateID, string Remark)
        {
            StartLog($"远程开闸 {nameof(WXRemoteGate)} (UserID={UserID},PKID={PKID},GateID={GateID},Remark={Remark})");

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select b.ProxyNo,b.IsBoxWatch,b.VID from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKID=@PKID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", PKID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                WXBoxServiceCallback callback = new WXBoxServiceCallback();
                                return callback.RemoteGate(reader["VID"].ToString(), UserID, PKID, GateID, Remark);
                            }
                            else
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                return callback.RemoteGate(reader["ProxyNo"].ToString(), UserID, PKID, GateID, Remark);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"远程开闸 {nameof(WXRemoteGate)}", ex);
            }
            finally
            {
                EndLog($"远程开闸 {nameof(WXRemoteGate)}");
            }

            return 3;
        }
        #endregion

        #region 车位预定

        /// <summary>
        /// 车位预定
        /// </summary>
        /// <param name="AccountID">账户ID</param>
        /// <param name="BitNo">车位号(保留)</param>
        /// <param name="parking_id">车场ID</param>
        /// <param name="AreaID">停车区域ID</param>
        /// <param name="license_plate">车牌号码</param>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <returns></returns>
        [OperationContract]
        public string WXReservePKBit(string AccountID, string BitNo, string parking_id, string AreaID, string license_plate, DateTime start_time, DateTime end_time)
        {
            StartLog($"车位预定 {nameof(WXReservePKBit)} (AccountID={AccountID},BitNo={BitNo},parking_id={parking_id},AreaID={AreaID},license_plate={license_plate},start_time={start_time},end_time={end_time})");

            WXReserveBitResult result = new WXReserveBitResult();
            result.code = -1;
            result.message = "预定车位失败";
            result.BitNo = BitNo;
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select b.ProxyNo,b.IsBoxWatch,b.VID from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKID=@PKID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", parking_id);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                WXBoxServiceCallback callback = new WXBoxServiceCallback();
                                return callback.ReservePKBit(reader["VID"].ToString(), AccountID, BitNo, parking_id, AreaID, license_plate, start_time, end_time);
                            }
                            else
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                return callback.ReservePKBit(reader["ProxyNo"].ToString(), AccountID, BitNo, parking_id, AreaID, license_plate, start_time, end_time);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"车位预定 {nameof(WXReservePKBit)}", ex);
            }
            finally
            {
                EndLog($"车位预定 {nameof(WXReservePKBit)}");
            }
            return JsonHelper.GetJsonString(result);
        }

        /// <summary>
        /// 预约支付
        /// </summary>
        /// <param name="ReserveID">预约ID</param>
        /// <param name="OrderID">订单编号</param>
        /// <param name="Amount">金额</param>
        /// <param name="PKID">车场ID</param>
        /// <param name="OnlineOrderID">线上订单编码</param>
        /// <returns></returns>
        [OperationContract]
        public bool WXReserveBitPay(string ReserveID, string OrderID, decimal Amount, string PKID, string OnlineOrderID)
        {
            StartLog($"预约支付 {nameof(WXReserveBitPay)} (=ReserveID={ReserveID},OrderID={OrderID},Amount={Amount},PKID={PKID},OnlineOrderID={OnlineOrderID})");

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select b.ProxyNo,b.IsBoxWatch,b.VID from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where a.PKID=@PKID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", PKID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            if (reader["IsBoxWatch"].ToBoolean())
                            {
                                WXBoxServiceCallback callback = new WXBoxServiceCallback();
                                return callback.ReserveBitPay(reader["VID"].ToString(), ReserveID, OrderID, Amount, PKID, OnlineOrderID);
                            }
                            else
                            {
                                WXServiceCallback callback = new WXServiceCallback();
                                return callback.ReserveBitPay(reader["ProxyNo"].ToString(), ReserveID, OrderID, Amount, PKID, OnlineOrderID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"预约支付 {nameof(WXReserveBitPay)}", ex);
            }
            finally
            {
                EndLog($"预约支付 {nameof(WXReserveBitPay)}");
            }
            return false;
        }

        /// <summary>
        /// 根据账号获取预定信息
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        [OperationContract]
        public string WXGetReservePKBit(string AccountID)
        {
            StartLog($"根据账号获取预定信息 {nameof(WXGetReservePKBit)} (AccountID={AccountID})");

            List<ParkReserveBit> list = new List<ParkReserveBit>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select a.*,b.PKName from ParkReserveBit a left join BaseParkinfo b on a.PKID=b.PKID where a.AccountID=@AccountID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AccountID", AccountID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkReserveBit model = DataReaderToModel<ParkReserveBit>.ToModel(reader);
                            list.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog($"根据账号获取预定信息 {nameof(WXGetReservePKBit)}", ex);
            }
            finally
            {
                EndLog($"根据账号获取预定信息 {nameof(WXGetReservePKBit)}");
            }

            return JsonHelper.GetJsonString(list);
        }
        #endregion

        #region 珠海定制
        /// <summary>
        /// 车位预定
        /// </summary>
        /// <param name="AccountID">账户ID</param>
        /// <param name="BitNo">车位号(保留)</param>
        /// <param name="parking_id">车场ID</param>
        /// <param name="AreaID">停车区域ID</param>
        /// <param name="license_plate">车牌号码</param>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <returns></returns>
        [OperationContract]
        public string WXReservePKBitZH(string ProxyNo, string AccountID, string BitNo, string parking_id, string AreaID, string license_plate, DateTime start_time, DateTime end_time)
        {
            StartLog($"车位预定 {nameof(WXReservePKBitZH)} (ProxyNo={ProxyNo},AccountID={AccountID},BitNo={BitNo},parking_id={parking_id},AreaID={AreaID},license_plate={license_plate},start_time={start_time},end_time={end_time})");

            WXReserveBitResult result = new WXReserveBitResult();
            result.code = -1;
            result.message = "预定车位失败";
            result.BitNo = BitNo;
            try
            {
                WXServiceCallback callback = new WXServiceCallback();
                return callback.ReservePKBit(ProxyNo, AccountID, BitNo, parking_id, AreaID, license_plate, start_time, end_time);
            }
            catch (Exception ex)
            {
                ExceptionLog($"车位预定 {nameof(WXReservePKBitZH)}", ex);
            }
            finally
            {
                EndLog($"车位预定 {nameof(WXReservePKBitZH)}");
            }

            return JsonHelper.GetJsonString(result);
        }

        #endregion
        /// <summary>
        /// 根据小区ID测试代理服务是否连接正常
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public bool WXTestClientProxyConnectionByVID(string VID)
        {
            StartLog($"根据小区ID测试代理服务是否连接正常 {nameof(WXTestClientProxyConnectionByVID)} (VID={VID})");

            bool res = false;
            if (string.IsNullOrEmpty(VID))
            {
                return res;
            }
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select ProxyNo,IsBoxWatch,VID from BaseVillage where VID=@VID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("VID", VID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            //if (!reader["IsBoxWatch"].ToBoolean())
                            //{
                            //    WXBoxServiceCallback callback = new WXBoxServiceCallback();
                            //    return callback.TestClientProxyConnection(reader["VID"].ToString());
                            //}
                            //else
                            //{
                            WXServiceCallback callback = new WXServiceCallback();
                            return callback.TestClientProxyConnection(reader["ProxyNo"].ToString());
                            //}
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ExceptionLog($"根据小区ID测试代理服务是否连接正常 {nameof(WXTestClientProxyConnectionByVID)}", ex);
            }
            finally
            {
                EndLog($"根据小区ID测试代理服务是否连接正常 {nameof(WXTestClientProxyConnectionByVID)}");
            }

            return res;
        }

        /// <summary>
        /// 根据车场ID测试代理服务是否连接正常
        /// </summary>
        /// <param name="VID"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        public bool WXTestClientProxyConnectionByPKID(string PKID)
        {
            StartLog($"根据车场ID测试代理服务是否连接正常 {nameof(WXTestClientProxyConnectionByPKID)} (PKID={PKID})");

            bool res = false;
            if (string.IsNullOrEmpty(PKID))
            {
                return res;
            }
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select b.ProxyNo,b.IsBoxWatch,b.VID from BaseParkinfo a left join BaseVillage b on a.VID=b.VID where PKID=@PKID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", PKID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            //if (!reader["IsBoxWatch"].ToBoolean())
                            //{
                            //    WXBoxServiceCallback callback = new WXBoxServiceCallback();
                            //    return callback.TestClientProxyConnection(reader["VID"].ToString());
                            //}
                            //else
                            //{
                            WXServiceCallback callback = new WXServiceCallback();
                            return callback.TestClientProxyConnection(reader["ProxyNo"].ToString());
                            // }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ExceptionLog($"根据车场ID测试代理服务是否连接正常 {nameof(WXTestClientProxyConnectionByPKID)}", ex);
            }
            finally
            {
                EndLog($"根据车场ID测试代理服务是否连接正常 {nameof(WXTestClientProxyConnectionByPKID)}");
            }

            return res;
        }

        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        [OperationContract]
        public void SendNotify(string title, string message)
        {
            StartLog($"发送通知 {nameof(SendNotify)} (title={title},message={message})");

            try
            {
                WXServiceCallback.SendNotify(title, message);

                EndLog($"发送通知 {nameof(SendNotify)}");
            }
            catch (Exception ex)
            {
                ExceptionLog($"发送通知 {nameof(SendNotify)}", ex);
            }
        }
    }
}
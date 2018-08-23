using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;
using System.Data.Common;
using Common;
using Common.Entities.Parking;
using System.IO;
using Common.Entities.WX;
using Common.Utilities;
using Common.Utilities.Helpers;
using Common.Core;

namespace ServicesDLL
{
    public class ProxyCallBackDLL
    {
        public static Logger logger = new Logger("ClientProxy");

        /// <summary>
        /// 获取商户打折信息
        /// </summary>
        /// <param name="sellerID"></param>
        /// <param name="VID"></param>
        /// <returns></returns>
        public static string GetParkDerate(string sellerID, string VID)
        {
            string strderate = "";
            List<ParkDerate> list = new List<ParkDerate>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = string.Format(@"select * from ParkDerate where SellerID='{0}' and DataStatus!=2 ", sellerID); ;
                    dbOperator.ClearParameters();
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkDerate model = DataReaderToModel<ParkDerate>.ToModel(reader);
                            list.Add(model);
                        }
                        strderate = JsonHelper.GetJsonString(list);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("获取商户打折信息回调异常！GetParkDerateCallback()" + ex.Message + "\r\n");
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
        public static string GetPKIORecordByPlateNumber(string PlateNumber, string VID, string SellerID)
        {
            string striorecord = "";
            List<ParkIORecord> list = new List<ParkIORecord>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = string.Format(@"select a.*,b.PKName  from ParkIORecord a left join BaseParkinfo b on a.parkingid=b.PKID 
                            left join ParkCarType d on a.CarTypeID=d.CarTypeID where IsExit=0 and b.VID=@VID and a.DataStatus<2 and b.DataStatus<2 
                            and a.PlateNumber like '%" + PlateNumber + "%' and (d.BaseTypeID=1 or d.BaseTypeID=3) ");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("VID", VID);
                    //dbOperator.AddParameter("PlateNumber", "'%"+PlateNumber+"%'");
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkIORecord model = DataReaderToModel<ParkIORecord>.ToModel(reader);
                            using (DbOperator dbOperator2 = ConnectionManager.CreateReadConnection())
                            {
                                strsql = "select a.DerateID,a.CreateTime as DiscountTime  from ParkCarDerate a left join ParkDerate b on a.DerateID=b.DerateID where  a.IORecordID=@IORecordID and b.SellerID=@SellerID";
                                dbOperator2.ClearParameters();
                                dbOperator2.AddParameter("IORecordID", model.RecordID);
                                dbOperator2.AddParameter("SellerID", SellerID);
                                using (DbDataReader reader2 = dbOperator2.ExecuteReader(strsql.ToString()))
                                {
                                    if (reader2.Read())
                                    {
                                        model.IsDiscount = true;
                                        model.DiscountTime = reader2["DiscountTime"].ToDateTime();
                                        model.DerateID = reader2["DerateID"].ToString();
                                    }
                                    else
                                    {
                                        model.IsDiscount = false;
                                    }
                                }
                            }
                            if (Directory.Exists(SystemInfo.ImgPath ))
                            {
                                if (File.Exists(SystemInfo.ImgPath  + "\\" + model.EntranceImage))
                                {
                                    try
                                    {
                                        MemoryStream mstream = new MemoryStream(File.ReadAllBytes(SystemInfo.ImgPath  + "\\" + model.EntranceImage));
                                        model.InimgData = System.Convert.ToBase64String(mstream.ToArray());
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            list.Add(model);
                        }
                        striorecord = JsonHelper.GetJsonString(list);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("获取车牌进场信息回调异常！GetPKIORecordByPlateNumberCallback()" + ex.Message + "\r\n");
            }
            return striorecord;
        }

        /// <summary>
        /// 消费打折
        /// </summary>
        /// <param name="IORecordID"></param>
        /// <param name="DerateID"></param>
        /// <param name="VID"></param>
        /// <param name="SellerID"></param>
        /// <param name="DerateMoney"></param>
        /// <returns></returns>
        public static string DiscountPlateNumber(string IORecordID, string DerateID, string VID, string SellerID, decimal DerateMoney)
        {
            ConsumerDiscountResult result = new ConsumerDiscountResult();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    ParkIORecord IOmodel = null;
                    string strsql = "select * from ParkIORecord where RecordID=@RecordID and DataStatus<2";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", IORecordID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            IOmodel = DataReaderToModel<ParkIORecord>.ToModel(reader);
                        }
                    }
                    if (IOmodel != null)
                    {
                        if (IOmodel.IsExit)
                        {
                            result.Result = 3;
                           
                            return JsonHelper.GetJsonString(result);//车辆已出场
                        }

                        strsql = "select a.*,b.SellerID from ParkCarDerate a left join ParkDerate b on a.DerateID=b.DerateID where IORecordID=@IORecordID and b.SellerID=@SellerID  and a.DataStatus<2  and Status!=3";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("IORecordID", IORecordID);
                        dbOperator.AddParameter("SellerID", SellerID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                result.Result = 4;
                                return JsonHelper.GetJsonString(result);//已打折
                            }
                        }
                        ParkDerate derate = null;
                        strsql = "select * from ParkDerate where DataStatus<2 and DerateID=@DerateID";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("DerateID", DerateID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                derate = DataReaderToModel<ParkDerate>.ToModel(reader);
                            }
                            else
                            {
                                result.Result = 5;
                                return JsonHelper.GetJsonString(result);//找不到优免规则
                            }
                        }

                        ParkSeller seller = null;
                        strsql = "select * from ParkSeller where DataStatus<2 and SellerID=@SellerID";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("SellerID", SellerID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                seller = DataReaderToModel<ParkSeller>.ToModel(reader);
                            }
                            else
                            {
                                result.Result = 6;
                                return JsonHelper.GetJsonString(result);//找不到商家信息
                            }
                        }

                        decimal SumFreeMoney = 0;
                        strsql = "select SUM(FreeMoney) as SumFreeMoney from ParkCarDerate a left join ParkDerate b on a.DerateID=b.DerateID  where a.DataStatus<2 and a.Status=1 and b.SellerID=@SellerID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("SellerID", SellerID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                        {
                            if (reader.Read())
                            {
                                SumFreeMoney = reader["SumFreeMoney"].ToDecimal();
                            }
                        }

                        if (derate.DerateType == DerateType.TimesPayment || derate.DerateType == DerateType.TimePeriodAndTimesPayment)
                        {
                            DerateMoney = (decimal)derate.DerateMoney;
                        }
                        if (derate.DerateType != DerateType.ReliefTime && derate.DerateType != DerateType.VotePayment && derate.DerateType != DerateType.DayFree)
                        {
                            if (seller.Balance + seller.Creditline < DerateMoney + SumFreeMoney)
                            {
                                result.Result = 7;
                                return JsonHelper.GetJsonString(result);//商家余额不足
                            }
                        }else  if (derate.DerateType == DerateType.DayFree)
                        {
                            if (seller.Balance + seller.Creditline < (DerateMoney * derate.DerateMoney) + SumFreeMoney)
                            {
                                result.Result = 7;
                                return JsonHelper.GetJsonString(result);//商家余额不足
                            }
                        }

                        ParkCarDerate Carderate = new ParkCarDerate();
                        Carderate.AreaID = IOmodel.AreaID;
                        Carderate.CarDerateID = System.Guid.NewGuid().ToString();
                        Carderate.CarDerateNo = IdGenerator.Instance.GetId().ToString();
                        Carderate.CardNo = IOmodel.CardNo;
                        Carderate.CreateTime = DateTime.Now;
                        Carderate.DerateID = DerateID;
                        Carderate.ExpiryTime = DateTime.Now.AddDays(7);
                        Carderate.Status = CarDerateStatus.Used;
                        if (derate.DerateType != DerateType.ReliefTime && derate.DerateType != DerateType.VotePayment && derate.DerateType != DerateType.DayFree)
                        {
                            Carderate.FreeMoney = DerateMoney;
                        }
                        else if (derate.DerateType == DerateType.DayFree)
                        {
                            Carderate.FreeMoney = DerateMoney * derate.DerateMoney;
                            Carderate.ExpiryTime = IOmodel.EntranceTime.AddDays((int)DerateMoney);
                           
                        }
                        else
                        {
                            Carderate.FreeTime = (int)DerateMoney;
                        }

                      
                        Carderate.IORecordID = IOmodel.RecordID;
                        Carderate.HaveUpdate = 1;
                        Carderate.PKID = IOmodel.ParkingID;
                        Carderate.PlateNumber = IOmodel.PlateNumber;
                        Carderate.DataStatus = 0;
                      
                        Carderate.LastUpdateTime = DateTime.Now;
                        dbOperator.BeginTransaction();
                        strsql = "insert into ParkCarDerate (CarDerateID,CarDerateNo,DerateID,FreeTime,PlateNumber,IORecordID,CardNo,ExpiryTime,CreateTime,Status,FreeMoney,AreaID,PKID,LastUpdateTime,HaveUpdate,DataStatus)";
                        strsql += "values(@CarDerateID,@CarDerateNo,@DerateID,@FreeTime,@PlateNumber,@IORecordID,@CardNo,@ExpiryTime,@CreateTime,@Status,@FreeMoney,@AreaID,@PKID,@LastUpdateTime,@HaveUpdate,@DataStatus)";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("CarDerateID", Carderate.CarDerateID);
                     
                        dbOperator.AddParameter("CarDerateNo", Carderate.CarDerateNo);
                        dbOperator.AddParameter("DerateID", Carderate.DerateID);
                        dbOperator.AddParameter("FreeTime", Carderate.FreeTime);
                        dbOperator.AddParameter("PlateNumber", Carderate.PlateNumber);
                        dbOperator.AddParameter("IORecordID", Carderate.IORecordID);
                        dbOperator.AddParameter("CardNo", Carderate.CardNo);
                        dbOperator.AddParameter("ExpiryTime", Carderate.ExpiryTime);
                        dbOperator.AddParameter("CreateTime", Carderate.CreateTime);
                        dbOperator.AddParameter("Status", Carderate.Status);
                        dbOperator.AddParameter("FreeMoney", Carderate.FreeMoney);
                     
                        dbOperator.AddParameter("AreaID", Carderate.AreaID);
                        dbOperator.AddParameter("PKID", Carderate.PKID);
                        dbOperator.AddParameter("LastUpdateTime", Carderate.LastUpdateTime);
                        dbOperator.AddParameter("HaveUpdate", Carderate.HaveUpdate);
                        dbOperator.AddParameter("DataStatus", Carderate.DataStatus);
                        if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                        {
                            dbOperator.RollbackTransaction();
                            result.Result = 8;
                            return JsonHelper.GetJsonString(result);//打折失败
                        }
                        if (derate.DerateType == DerateType.DayFree)
                        {
                            strsql = "update ParkSeller set Balance=Balance-" + Carderate.FreeMoney + "where SellerID=@SellerID";
                            dbOperator.ClearParameters();
                            dbOperator.AddParameter("SellerID", seller.SellerID);
                            if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                            {
                                dbOperator.RollbackTransaction();
                                result.Result = 8;
                                return JsonHelper.GetJsonString(result);//打折失败
                            }
                        }
                        dbOperator.CommitTransaction();
                        result.Result = 1;
                        result.ResModel = Carderate;
                        return JsonHelper.GetJsonString(result);
                    }
                    else
                    {
                        result.Result = 2;
                        return JsonHelper.GetJsonString(result);//找不到进出记录信息
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("消费打折回调异常！DiscountPlateNumberCallback()" + ex.Message + "\r\n");
                result.Result = -1;
                return JsonHelper.GetJsonString(result);//异常
            }
        }

        /// <summary>
        /// 修改商户密码
        /// </summary>
        /// <param name="SellerID"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static bool EditSellerPwd(string SellerID, string Pwd)
        {
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "update ParkSeller set PWD=@PWD where SellerID=@SellerID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PWD", Pwd);
                    dbOperator.AddParameter("SellerID", SellerID);
                    if (dbOperator.ExecuteNonQuery(strsql) <= 0)
                    {
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("修改商户密码异常！EditSellerPwd()" + ex.Message + "\r\n");
                return false;
            }
        }

        /// <summary
        /// 获取商户信息
        /// </summary>
        /// <param name="SellerID"></param>
        /// <returns></returns>
        public static string GetSellerInfo(string SellerID)
        {
            string strseller = "";
            ParkSeller model = null;
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select * from ParkSeller where DataStatus<2 and SellerID=@SellerID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("SellerID", SellerID);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            model = DataReaderToModel<ParkSeller>.ToModel(reader);
                            using (DbOperator dbOperator2 = ConnectionManager.CreateReadConnection())
                            {
                                strsql = "select SUM(FreeMoney) as SumFreeMoney from ParkCarDerate a left join ParkDerate b on a.DerateID=b.DerateID  where a.DataStatus<2 and a.Status=1 and b.SellerID=@SellerID ";
                                dbOperator2.ClearParameters();
                                dbOperator2.AddParameter("SellerID", SellerID);
                                using (DbDataReader reader2 = dbOperator2.ExecuteReader(strsql.ToString()))
                                {
                                    if (reader.Read())
                                    {
                                        model.EnableBalance = model.Balance - reader2["SumFreeMoney"].ToDecimal();
                                    }
                                    else
                                    {
                                        model.EnableBalance = model.Balance;
                                    }
                                }
                            }
                            strseller = JsonHelper.GetJsonString(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("获取商户信息回调异常！GetSellerInfoCallback()" + ex.Message + "\r\n");
            }
            return strseller;
        }

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="SellerNo"></param>
        /// <param name="PWD"></param>
        /// <param name="SellerID"></param>
        /// <returns></returns>
        public static string GetSellerInfo2(string SellerNo, string PWD, string SellerID)
        {
            string strderate = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "select a.SellerID,b.ProxyNo from ParkSeller a left join  BaseVillage b on a.VID=b.VID where a.DataStatus<2 and b.DataStatus<2";
                    if (string.IsNullOrEmpty(SellerID))
                    {
                        strsql += "and a.SellerNo=@SellerNo and a.PWD=@PWD";
                    }
                    else
                    {
                        strsql += "and a.SellerID=@SellerID";
                    }
                    dbOperator.ClearParameters();
                    if (string.IsNullOrEmpty(SellerID))
                    {
                        dbOperator.AddParameter("SellerNo", SellerNo);
                        dbOperator.AddParameter("PWD", PWD);
                    }
                    else
                    {
                        dbOperator.AddParameter("SellerID", SellerID);
                    }
                    using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
                    {
                        if (reader.Read())
                        {
                            SellerID = reader["SellerID"].ToString();
                            return GetSellerInfo(SellerID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("获取商户信息异常：GetSellerInfo()" + ex.Message + "\r\n");
            }
            return strderate;
        }
    }
}

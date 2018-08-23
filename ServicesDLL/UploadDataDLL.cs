using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Common.Entities;
using Common.Entities.BaseData;
using Common.DataAccess;
using Common.Entities.Parking;
using Common.Entities.Statistics;
using Common;
using Common.Entities.WX;
using Common.Core;
using Common.Utilities.Helpers;

namespace ServicesDLL
{
    public class UploadDataDLL
    {
        /// <summary>
        /// 日志记录对象
        /// </summary>
        protected Logger logger = LogManager.GetLogger("上传下载");
           
        /// <summary>
        /// 上传下载公司信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadBaseCompany(List<BaseCompany> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from BaseCompany where CPID=@CPID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("CPID", obj.CPID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update BaseCompany set CPName=@CPName,CityID=@CityID,Address=@Address,LinkMan=@LinkMan,Mobile=@Mobile,MasterID=@MasterID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus 
                                                where CPID=@CPID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("CPName", obj.CPName);
                                dbOperator.AddParameter("CityID", obj.CityID);
                                dbOperator.AddParameter("Address", obj.Address);
                                dbOperator.AddParameter("LinkMan", obj.LinkMan);
                                dbOperator.AddParameter("Mobile", obj.Mobile);
                                dbOperator.AddParameter("MasterID", obj.MasterID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("CPID", obj.CPID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.CPID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into BaseCompany(CPID,CPName,CityID,Address,LinkMan,Mobile,MasterID,LastUpdateTime,HaveUpdate,DataStatus )
                                                                values(@CPID,@CPName,@CityID,@Address,@LinkMan,@Mobile,@MasterID,@LastUpdateTime,@HaveUpdate,@DataStatus)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("CPID", obj.CPID);
                                dbOperator.AddParameter("CPName", obj.CPName);
                                dbOperator.AddParameter("CityID", obj.CityID);
                                dbOperator.AddParameter("Address", obj.Address);
                                dbOperator.AddParameter("LinkMan", obj.LinkMan);
                                dbOperator.AddParameter("Mobile", obj.Mobile);
                                dbOperator.AddParameter("MasterID", obj.MasterID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                               
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.CPID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Fatal("上传下载公司信息异常：UploadBaseCompany()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载小区信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadBaseVillage(List<BaseVillage> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from BaseVillage where VID=@VID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("VID", obj.VID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update BaseVillage set VNo=@VNo,VName=@VName,CPID=@CPID,LinkMan=@LinkMan,Mobile=@Mobile,Address=@Address,Coordinate=@Coordinate,ProxyNo=@ProxyNo,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus  
                                               ,IsBoxWatch=@IsBoxWatch where VID=@VID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("VNo", obj.VNo);
                                dbOperator.AddParameter("VName", obj.VName);
                                dbOperator.AddParameter("CPID", obj.CPID);
                                dbOperator.AddParameter("LinkMan", obj.LinkMan);
                                dbOperator.AddParameter("Mobile", obj.Mobile);
                                dbOperator.AddParameter("Address", obj.Address);
                                dbOperator.AddParameter("Coordinate", obj.Coordinate);
                                dbOperator.AddParameter("ProxyNo", obj.ProxyNo);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("VID", obj.VID);
                                dbOperator.AddParameter("IsBoxWatch", obj.IsBoxWatch);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.VID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into BaseVillage(VID,VNo,VName,CPID,LinkMan,Mobile,Address,Coordinate,ProxyNo,LastUpdateTime,HaveUpdate,DataStatus,IsBoxWatch  )
                                                                values(@VID,@VNo,@VName,@CPID,@LinkMan,@Mobile,@Address,@Coordinate,@ProxyNo,@LastUpdateTime,@HaveUpdate,@DataStatus,@IsBoxWatch)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("VID", obj.VID);
                                dbOperator.AddParameter("VNo", obj.VNo);
                                dbOperator.AddParameter("VName", obj.VName);
                                dbOperator.AddParameter("CPID", obj.CPID);
                                dbOperator.AddParameter("LinkMan", obj.LinkMan);
                                dbOperator.AddParameter("Mobile", obj.Mobile);
                                dbOperator.AddParameter("Address", obj.Address);
                                dbOperator.AddParameter("Coordinate", obj.Coordinate);
                                dbOperator.AddParameter("ProxyNo", obj.ProxyNo);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("IsBoxWatch", obj.IsBoxWatch);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.VID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载小区信息异常：UploadBaseVillage()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载车场信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadBaseParkinfo(List<BaseParkinfo> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from BaseParkinfo where PKID=@PKID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PKID", obj.PKID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update BaseParkinfo set PKNo=@PKNo,PKName=@PKName,CarBitNum=@CarBitNum,CarBitNumLeft=@CarBitNumLeft,CarBitNumFixed=@CarBitNumFixed,SpaceBitNum=@SpaceBitNum,CenterTime=@CenterTime,AllowLoseDisplay=@AllowLoseDisplay,LinkMan=@LinkMan,
                                                Mobile=@Mobile ,Address=@Address,Coordinate=@Coordinate,MobilePay=@MobilePay,MobileLock=@MobileLock,IsParkingSpace=@IsParkingSpace,IsReverseSeekingVehicle=@IsReverseSeekingVehicle,
                                                 FeeRemark=@FeeRemark,OnLine=@OnLine,Remark=@Remark,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,CityID=@CityID,VID=@VID,DataStatus=@DataStatus,DataSaveDays=@DataSaveDays,
                                                PictureSaveDays=@PictureSaveDays,IsOnLineGathe=@IsOnLineGathe,IsLine=@IsLine,ExpiredAdvanceRemindDay=@ExpiredAdvanceRemindDay,PoliceFree=@PoliceFree,OnlineDiscount=@OnlineDiscount,IsOnlineDiscount=@IsOnlineDiscount,IsNoPlateConfirm=@IsNoPlateConfirm    
                                                ,OuterringCharge=@OuterringCharge where PKID=@PKID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("PKNo", obj.PKNo);
                                dbOperator.AddParameter("PKName", obj.PKName);
                                dbOperator.AddParameter("CarBitNum", obj.CarBitNum);
                                dbOperator.AddParameter("CarBitNumLeft", obj.CarBitNumLeft);
                                dbOperator.AddParameter("CarBitNumFixed", obj.CarBitNumFixed);
                                dbOperator.AddParameter("SpaceBitNum", obj.SpaceBitNum);
                                dbOperator.AddParameter("CenterTime", obj.CenterTime);
                                dbOperator.AddParameter("AllowLoseDisplay", obj.AllowLoseDisplay);
                                dbOperator.AddParameter("LinkMan", obj.LinkMan);
                                dbOperator.AddParameter("Mobile", obj.Mobile);
                                dbOperator.AddParameter("Address", obj.Address);
                                dbOperator.AddParameter("Coordinate", obj.Coordinate);
                                dbOperator.AddParameter("MobilePay", obj.MobilePay);
                                dbOperator.AddParameter("MobileLock", obj.MobileLock);
                                dbOperator.AddParameter("IsParkingSpace", obj.IsParkingSpace);
                                dbOperator.AddParameter("IsReverseSeekingVehicle", obj.IsReverseSeekingVehicle);
                                dbOperator.AddParameter("FeeRemark", obj.FeeRemark);
                                dbOperator.AddParameter("OnLine", obj.OnLine);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("CityID", obj.CityID);
                                dbOperator.AddParameter("VID", obj.VID);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("DataSaveDays", obj.DataSaveDays);
                                dbOperator.AddParameter("PictureSaveDays", obj.PictureSaveDays);
                                dbOperator.AddParameter("IsOnLineGathe", obj.IsOnLineGathe);
                                dbOperator.AddParameter("IsLine", obj.IsLine);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("ExpiredAdvanceRemindDay", obj.ExpiredAdvanceRemindDay);
                                dbOperator.AddParameter("PoliceFree", obj.PoliceFree);
                                 dbOperator.AddParameter("OnlineDiscount", obj.OnlineDiscount);
                                 dbOperator.AddParameter("IsOnlineDiscount", obj.IsOnlineDiscount);
                                 dbOperator.AddParameter("IsNoPlateConfirm", obj.IsNoPlateConfirm);
                                 dbOperator.AddParameter("OuterringCharge", obj.OuterringCharge);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.PKID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into BaseParkinfo(PKID,PKNo,PKName,CarBitNum,CarBitNumLeft,CarBitNumFixed,SpaceBitNum,CenterTime,AllowLoseDisplay,LinkMan,
                                                                Mobile ,Address,Coordinate,MobilePay,MobileLock,IsParkingSpace,IsReverseSeekingVehicle,
                                                                FeeRemark,OnLine,Remark,LastUpdateTime,HaveUpdate,CityID,VID,DataStatus,DataSaveDays,
                                                                PictureSaveDays,IsOnLineGathe,IsLine,ExpiredAdvanceRemindDay,PoliceFree,OnlineDiscount,IsOnlineDiscount,IsNoPlateConfirm,OuterringCharge)
                                                                values(@PKID,@PKNo,@PKName,@CarBitNum,@CarBitNumLeft,@CarBitNumFixed,@SpaceBitNum,@CenterTime,@AllowLoseDisplay,@LinkMan,
                                                                @Mobile ,@Address,@Coordinate,@MobilePay,@MobileLock,@IsParkingSpace,@IsReverseSeekingVehicle,
                                                                @FeeRemark,@OnLine,@Remark,@LastUpdateTime,@HaveUpdate,@CityID,@VID,@DataStatus,@DataSaveDays,
                                                                @PictureSaveDays,@IsOnLineGathe,@IsLine,@ExpiredAdvanceRemindDay,@PoliceFree,@OnlineDiscount,@IsOnlineDiscount,@IsNoPlateConfirm,@OuterringCharge)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("PKNo", obj.PKNo);
                                dbOperator.AddParameter("PKName", obj.PKName);
                                dbOperator.AddParameter("CarBitNum", obj.CarBitNum);
                                dbOperator.AddParameter("CarBitNumLeft", obj.CarBitNumLeft);
                                dbOperator.AddParameter("CarBitNumFixed", obj.CarBitNumFixed);
                                dbOperator.AddParameter("SpaceBitNum", obj.SpaceBitNum);
                                dbOperator.AddParameter("CenterTime", obj.CenterTime);
                                dbOperator.AddParameter("AllowLoseDisplay", obj.AllowLoseDisplay);
                                dbOperator.AddParameter("LinkMan", obj.LinkMan);
                                dbOperator.AddParameter("Mobile", obj.Mobile);
                                dbOperator.AddParameter("Address", obj.Address);
                                dbOperator.AddParameter("Coordinate", obj.Coordinate);
                                dbOperator.AddParameter("MobilePay", obj.MobilePay);
                                dbOperator.AddParameter("MobileLock", obj.MobileLock);
                                dbOperator.AddParameter("IsParkingSpace", obj.IsParkingSpace);
                                dbOperator.AddParameter("IsReverseSeekingVehicle", obj.IsReverseSeekingVehicle);
                                dbOperator.AddParameter("FeeRemark", obj.FeeRemark);
                                dbOperator.AddParameter("OnLine", obj.OnLine);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("CityID", obj.CityID);
                                dbOperator.AddParameter("VID", obj.VID);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("DataSaveDays", obj.DataSaveDays);
                                dbOperator.AddParameter("PictureSaveDays", obj.PictureSaveDays);
                                dbOperator.AddParameter("IsOnLineGathe", obj.IsOnLineGathe);
                                dbOperator.AddParameter("IsLine", obj.IsLine);
                                dbOperator.AddParameter("ExpiredAdvanceRemindDay", obj.ExpiredAdvanceRemindDay);
                                dbOperator.AddParameter("PoliceFree", obj.PoliceFree);
                                dbOperator.AddParameter("OnlineDiscount", obj.OnlineDiscount);
                                dbOperator.AddParameter("IsOnlineDiscount", obj.IsOnlineDiscount);
                                dbOperator.AddParameter("IsNoPlateConfirm", obj.IsNoPlateConfirm);
                                dbOperator.AddParameter("OuterringCharge", obj.OuterringCharge);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.PKID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载车场信息异常：UploadBaseParkinfo()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传车场车位信息
        /// </summary>
        /// <param name="PKID"></param>
        /// <param name="CarBitNumLeft"></param>
        /// <param name="CarBitNumFixed"></param>
        /// <param name="SpaceBitNum"></param>
        /// <returns></returns>
        public bool UploadParkinfoCarBitNum(string PKID, int CarBitNumLeft, int CarBitNumFixed, int SpaceBitNum)
        {
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string sql = "update BaseParkinfo set CarBitNumLeft=@CarBitNumLeft,CarBitNumFixed=@CarBitNumFixed,SpaceBitNum=@SpaceBitNum where  PKID=@PKID";
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", PKID);
                    dbOperator.AddParameter("CarBitNumLeft", CarBitNumLeft);
                    dbOperator.AddParameter("CarBitNumFixed", CarBitNumFixed);
                    dbOperator.AddParameter("SpaceBitNum", SpaceBitNum);
                    if (dbOperator.ExecuteNonQuery(sql) <= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传车场车位信息异常：UploadParkinfoCarBitNum()" + ex.Message + "\r\n");
                return false;
            }
        }

        /// <summary>
        /// 上传下载车场放行描述
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadBasePassRemark(List<BasePassRemark> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from BasePassRemark where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update BasePassRemark set PassType=@PassType,Remark=@Remark,PKID=@PKID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus
                                where RecordID=@RecordID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("PassType", obj.PassType);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into BasePassRemark(RecordID,PassType,Remark,PKID,LastUpdateTime,HaveUpdate,DataStatus)
                                                                values(@RecordID,@PassType,@Remark,@PKID,@LastUpdateTime,@HaveUpdate,@DataStatus)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("PassType", obj.PassType);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载车场放行描述异常：UploadBasePassRemark()" + ex.Message + "\r\n");
               
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载车场区域
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkArea(List<ParkArea> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkArea where AreaID=@AreaID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("AreaID", obj.AreaID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkArea set AreaName=@AreaName,MasterID=@MasterID,PKID=@PKID,CarbitNum=@CarbitNum,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,
                                NeedToll=@NeedToll,CameraWaitTime=@CameraWaitTime,TwoCameraWait=@TwoCameraWait,Remark=@Remark,DataStatus=@DataStatus
                                where AreaID=@AreaID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("AreaName", obj.AreaName);
                                dbOperator.AddParameter("MasterID", obj.MasterID);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("CarbitNum", obj.CarbitNum);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("NeedToll", obj.NeedToll);
                                dbOperator.AddParameter("CameraWaitTime", obj.CameraWaitTime);
                                dbOperator.AddParameter("TwoCameraWait", obj.TwoCameraWait);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("AreaID", obj.AreaID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.AreaID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkArea(AreaID,AreaName,MasterID,PKID,CarbitNum,LastUpdateTime,HaveUpdate,
                                                                NeedToll,CameraWaitTime,TwoCameraWait,Remark,DataStatus)
                                                                values(@AreaID,@AreaName,@MasterID,@PKID,@CarbitNum,@LastUpdateTime,@HaveUpdate,
                                                                @NeedToll,@CameraWaitTime,@TwoCameraWait,@Remark,@DataStatus)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("AreaID", obj.AreaID);
                                dbOperator.AddParameter("AreaName", obj.AreaName);
                                dbOperator.AddParameter("MasterID", obj.MasterID);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("CarbitNum", obj.CarbitNum);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("NeedToll", obj.NeedToll);
                                dbOperator.AddParameter("CameraWaitTime", obj.CameraWaitTime);
                                dbOperator.AddParameter("TwoCameraWait", obj.TwoCameraWait);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.AreaID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载车场区域异常：UploadParkArea()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载车场岗亭
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkBox(List<ParkBox> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkBox where BoxID=@BoxID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("BoxID", obj.BoxID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkBox set BoxNo=@BoxNo,BoxName=@BoxName,ComputerIP=@ComputerIP,AreaID=@AreaID,Remark=@Remark,DataStatus=@DataStatus,
                                HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime,IsOnLine=@IsOnLine,ProxyNo=@ProxyNo  
                                where BoxID=@BoxID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("BoxNo", obj.BoxNo);
                                dbOperator.AddParameter("BoxName", obj.BoxName);
                                dbOperator.AddParameter("ComputerIP", obj.ComputerIP);
                                dbOperator.AddParameter("AreaID", obj.AreaID);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("BoxID", obj.BoxID);
                                dbOperator.AddParameter("IsOnLine", obj.IsOnLine);
                                dbOperator.AddParameter("ProxyNo", obj.ProxyNo);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.BoxID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkBox(BoxID,BoxNo,BoxName,ComputerIP,AreaID,Remark,DataStatus,
                                                                HaveUpdate,LastUpdateTime,IsOnLine,ProxyNo)
                                                                values(@BoxID,@BoxNo,@BoxName,@ComputerIP,@AreaID,@Remark,@DataStatus,
                                                                @HaveUpdate,@LastUpdateTime,@IsOnLine,@ProxyNo)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("BoxID", obj.BoxID);
                                dbOperator.AddParameter("BoxNo", obj.BoxNo);
                                dbOperator.AddParameter("BoxName", obj.BoxName);
                                dbOperator.AddParameter("ComputerIP", obj.ComputerIP);
                                dbOperator.AddParameter("AreaID", obj.AreaID);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }

                                dbOperator.AddParameter("IsOnLine", obj.IsOnLine);
                                dbOperator.AddParameter("ProxyNo", obj.ProxyNo);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.BoxID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载车场岗亭异常：UploadParkBox()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载通道信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkGate(List<ParkGate> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkGate where GateID=@GateID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("GateID", obj.GateID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkGate set GateNo=@GateNo,GateName=@GateName,BoxID=@BoxID,IoState=@IoState,IsTempInOut=@IsTempInOut,IsEnterConfirm=@IsEnterConfirm,
                                OpenPlateBlurryMatch=@OpenPlateBlurryMatch,Remark=@Remark,DataStatus=@DataStatus,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,IsNeedCapturePaper=@IsNeedCapturePaper,PlateNumberAndCard=@PlateNumberAndCard  
                                where GateID=@GateID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("GateNo", obj.GateNo);
                                dbOperator.AddParameter("GateName", obj.GateName);
                                dbOperator.AddParameter("BoxID", obj.BoxID);
                                dbOperator.AddParameter("IoState", obj.IoState);
                                dbOperator.AddParameter("IsTempInOut", obj.IsTempInOut);
                                dbOperator.AddParameter("IsEnterConfirm", obj.IsEnterConfirm);
                                dbOperator.AddParameter("OpenPlateBlurryMatch", obj.OpenPlateBlurryMatch);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("GateID", obj.GateID);
                                dbOperator.AddParameter("IsNeedCapturePaper", obj.IsNeedCapturePaper);
                                dbOperator.AddParameter("PlateNumberAndCard", obj.PlateNumberAndCard);
                                
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.GateID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkGate(GateID,GateNo,GateName,BoxID,IoState,IsTempInOut,IsEnterConfirm,
                                                                OpenPlateBlurryMatch,Remark,DataStatus,LastUpdateTime,HaveUpdate,IsNeedCapturePaper,PlateNumberAndCard)
                                                                values(@GateID,@GateNo,@GateName,@BoxID,@IoState,@IsTempInOut,@IsEnterConfirm,
                                                                @OpenPlateBlurryMatch,@Remark,@DataStatus,@LastUpdateTime,@HaveUpdate,@IsNeedCapturePaper,@PlateNumberAndCard)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("GateID", obj.GateID);
                                dbOperator.AddParameter("GateNo", obj.GateNo);
                                dbOperator.AddParameter("GateName", obj.GateName);
                                dbOperator.AddParameter("BoxID", obj.BoxID);
                                dbOperator.AddParameter("IoState", obj.IoState);
                                dbOperator.AddParameter("IsTempInOut", obj.IsTempInOut);
                                dbOperator.AddParameter("IsEnterConfirm", obj.IsEnterConfirm);
                                dbOperator.AddParameter("OpenPlateBlurryMatch", obj.OpenPlateBlurryMatch);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("IsNeedCapturePaper", obj.IsNeedCapturePaper);
                                dbOperator.AddParameter("PlateNumberAndCard", obj.PlateNumberAndCard);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.GateID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载通道信息异常：UploadParkGate()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载通道设备信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkDevice(List<ParkDevice> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkDevice where DeviceID=@DeviceID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("DeviceID", obj.DeviceID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkDevice set GateID=@GateID,DeviceType=@DeviceType,PortType=@PortType,Baudrate=@Baudrate,SerialPort=@SerialPort,IpAddr=@IpAddr,
                                IpPort=@IpPort,UserName=@UserName,UserPwd=@UserPwd,NetID=@NetID,LedNum=@LedNum,DataStatus=@DataStatus,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate
                                ,DeviceNo=@DeviceNo,OfflinePort=@OfflinePort,IsCapture=@IsCapture,IsSVoice=@IsSVoice,IsCarBit=@IsCarBit,ControllerType=@ControllerType,IsContestDev=@IsContestDev,DisplayMode=@DisplayMode,IsMonitor=@IsMonitor 
                                where DeviceID=@DeviceID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("GateID", obj.GateID);
                                dbOperator.AddParameter("DeviceType", obj.DeviceType);
                                dbOperator.AddParameter("PortType", obj.PortType);
                                dbOperator.AddParameter("Baudrate", obj.Baudrate);
                                dbOperator.AddParameter("SerialPort", obj.SerialPort);
                                dbOperator.AddParameter("IpAddr", obj.IpAddr);
                                dbOperator.AddParameter("IpPort", obj.IpPort);
                                dbOperator.AddParameter("UserName", obj.UserName);
                                dbOperator.AddParameter("UserPwd", obj.UserPwd);
                                dbOperator.AddParameter("NetID", obj.NetID);
                                dbOperator.AddParameter("LedNum", obj.LedNum);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("DeviceNo", obj.DeviceNo);
                                dbOperator.AddParameter("OfflinePort", obj.OfflinePort);
                                dbOperator.AddParameter("DeviceID", obj.DeviceID);
                                dbOperator.AddParameter("IsCapture", obj.IsCapture);
                                dbOperator.AddParameter("IsSVoice", obj.IsSVoice);
                                dbOperator.AddParameter("IsCarBit", obj.IsCarBit);
                                dbOperator.AddParameter("ControllerType", obj.ControllerType);
                                dbOperator.AddParameter("IsContestDev", obj.IsContestDev);
                                dbOperator.AddParameter("DisplayMode", obj.DisplayMode);
                                dbOperator.AddParameter("IsMonitor", obj.IsMonitor);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.DeviceID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkDevice(DeviceID,GateID,DeviceType,PortType,Baudrate,SerialPort,IpAddr,
                                                                IpPort,UserName,UserPwd,NetID,LedNum,DataStatus,LastUpdateTime,HaveUpdate,DeviceNo,OfflinePort,IsCapture,IsSVoice,IsCarBit,ControllerType,IsContestDev,DisplayMode,IsMonitor)
                                                                values(@DeviceID,@GateID,@DeviceType,@PortType,@Baudrate,@SerialPort,@IpAddr,
                                                                @IpPort,@UserName,@UserPwd,@NetID,@LedNum,@DataStatus,@LastUpdateTime,@HaveUpdate,@DeviceNo,@OfflinePort,@IsCapture,@IsSVoice,@IsCarBit,@ControllerType,@IsContestDev,@DisplayMode,@IsMonitor)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("DeviceID", obj.DeviceID);
                                dbOperator.AddParameter("GateID", obj.GateID);
                                dbOperator.AddParameter("DeviceType", obj.DeviceType);
                                dbOperator.AddParameter("PortType", obj.PortType);
                                dbOperator.AddParameter("Baudrate", obj.Baudrate);
                                dbOperator.AddParameter("SerialPort", obj.SerialPort);
                                dbOperator.AddParameter("IpAddr", obj.IpAddr);
                                dbOperator.AddParameter("IpPort", obj.IpPort);
                                dbOperator.AddParameter("UserName", obj.UserName);
                                dbOperator.AddParameter("UserPwd", obj.UserPwd);
                                dbOperator.AddParameter("NetID", obj.NetID);
                                dbOperator.AddParameter("LedNum", obj.LedNum);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("DeviceNo", obj.DeviceNo);
                                dbOperator.AddParameter("OfflinePort", obj.OfflinePort);
                                dbOperator.AddParameter("IsCapture", obj.IsCapture);
                                dbOperator.AddParameter("IsSVoice", obj.IsSVoice);
                                dbOperator.AddParameter("IsCarBit", obj.IsCarBit);
                                dbOperator.AddParameter("ControllerType", obj.ControllerType);
                                dbOperator.AddParameter("IsContestDev", obj.IsContestDev);
                                dbOperator.AddParameter("DisplayMode", obj.DisplayMode);
                                dbOperator.AddParameter("IsMonitor", obj.IsMonitor);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.DeviceID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载通道设备信息异常：UploadParkDevice()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载设备参数信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkDeviceParam(List<ParkDeviceParam> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkDeviceParam where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkDeviceParam set DeviceID=@DeviceID,VipMode=@VipMode,TempMode=@TempMode,NetOffMode=@NetOffMode,VipDevMultIn=@VipDevMultIn,PloicFree=@PloicFree,
                                VipDutyDay=@VipDutyDay,OverDutyYorN=@OverDutyYorN,OverDutyDay=@OverDutyDay,SysID=@SysID,DevID=@DevID,SysInDev=@SysInDev,SysOutDev=@SysOutDev,SysParkNumber=@SysParkNumber
                                ,DevInorOut=@DevInorOut,SwipeInterval=@SwipeInterval,UnKonwCardType=@UnKonwCardType,LEDNumber=@LEDNumber,DataStatus=@DataStatus,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where RecordID=@RecordID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("DeviceID", obj.DeviceID);
                                dbOperator.AddParameter("VipMode", obj.VipMode);
                                dbOperator.AddParameter("TempMode", obj.TempMode);
                                dbOperator.AddParameter("NetOffMode", obj.NetOffMode);
                                dbOperator.AddParameter("VipDevMultIn", obj.VipDevMultIn);
                                dbOperator.AddParameter("PloicFree", obj.PloicFree);
                                dbOperator.AddParameter("VipDutyDay", obj.VipDutyDay);
                                dbOperator.AddParameter("OverDutyYorN", obj.OverDutyYorN);
                                dbOperator.AddParameter("OverDutyDay", obj.OverDutyDay);
                                dbOperator.AddParameter("SysID", obj.SysID);
                                dbOperator.AddParameter("DevID", obj.DevID);
                                dbOperator.AddParameter("SysInDev", obj.SysInDev);
                                dbOperator.AddParameter("SysOutDev", obj.SysOutDev);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("SysParkNumber", obj.SysParkNumber);
                                dbOperator.AddParameter("DevInorOut", obj.DevInorOut);
                                dbOperator.AddParameter("SwipeInterval", obj.SwipeInterval);
                                dbOperator.AddParameter("UnKonwCardType", obj.UnKonwCardType);
                                dbOperator.AddParameter("LEDNumber", obj.LEDNumber);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                
                                
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.DeviceID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkDeviceParam(RecordID,DeviceID,VipMode,TempMode,NetOffMode,VipDevMultIn,PloicFree,
                                VipDutyDay,OverDutyYorN,OverDutyDay,SysID,DevID,SysInDev,SysOutDev,SysParkNumber
                                ,DevInorOut,SwipeInterval,UnKonwCardType,LEDNumber,DataStatus,LastUpdateTime,HaveUpdate)
                                                                values(@RecordID,@DeviceID,@VipMode,@TempMode,@NetOffMode,@VipDevMultIn,@PloicFree,
                                @VipDutyDay,@OverDutyYorN,@OverDutyDay,@SysID,@DevID,@SysInDev,@SysOutDev,@SysParkNumber
                                ,@DevInorOut,@SwipeInterval,@UnKonwCardType,@LEDNumber,@DataStatus,@LastUpdateTime,@HaveUpdate)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("DeviceID", obj.DeviceID);
                                dbOperator.AddParameter("VipMode", obj.VipMode);
                                dbOperator.AddParameter("TempMode", obj.TempMode);
                                dbOperator.AddParameter("NetOffMode", obj.NetOffMode);
                                dbOperator.AddParameter("VipDevMultIn", obj.VipDevMultIn);
                                dbOperator.AddParameter("PloicFree", obj.PloicFree);
                                dbOperator.AddParameter("VipDutyDay", obj.VipDutyDay);
                                dbOperator.AddParameter("OverDutyYorN", obj.OverDutyYorN);
                                dbOperator.AddParameter("OverDutyDay", obj.OverDutyDay);
                                dbOperator.AddParameter("SysID", obj.SysID);
                                dbOperator.AddParameter("DevID", obj.DevID);
                                dbOperator.AddParameter("SysInDev", obj.SysInDev);
                                dbOperator.AddParameter("SysOutDev", obj.SysOutDev);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("SysParkNumber", obj.SysParkNumber);
                                dbOperator.AddParameter("DevInorOut", obj.DevInorOut);
                                dbOperator.AddParameter("SwipeInterval", obj.SwipeInterval);
                                dbOperator.AddParameter("UnKonwCardType", obj.UnKonwCardType);
                                dbOperator.AddParameter("LEDNumber", obj.LEDNumber);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.DeviceID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载通道设备参数信息异常：UploadParkDevice()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }
        /// <summary>
        /// 上传设备实时检测信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkDeviceDetection(List<ParkDeviceDetection> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkDeviceDetection where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkDeviceDetection set DeviceID=@DeviceID,ConnectionState=@ConnectionState,DisconnectTime=@DisconnectTime,
                                    DataStatus=@DataStatus,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,PKID=@PKID
                                where RecordID=@RecordID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("DeviceID", obj.DeviceID);
                                dbOperator.AddParameter("ConnectionState", obj.ConnectionState);
                                if (obj.DisconnectTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("DisconnectTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("DisconnectTime", obj.DisconnectTime);
                                }
                             
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                               
                            
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.DeviceID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkDeviceDetection(RecordID,DeviceID,ConnectionState,DisconnectTime,
                                    DataStatus,LastUpdateTime,HaveUpdate,PKID)values(@RecordID,@DeviceID,@ConnectionState,@DisconnectTime,
                                    @DataStatus,@LastUpdateTime,@HaveUpdate,@PKID)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("DeviceID", obj.DeviceID);
                                dbOperator.AddParameter("ConnectionState", obj.ConnectionState);
                                if (obj.DisconnectTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("DisconnectTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("DisconnectTime", obj.DisconnectTime);
                                }
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                              
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.DeviceID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传设备实时检测信息异常：UploadParkDeviceDetection()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载车类信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkCarType(List<ParkCarType> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkCarType where CarTypeID=@CarTypeID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("CarTypeID", obj.CarTypeID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkCarType set CarTypeName=@CarTypeName,PKID=@PKID,BaseTypeID=@BaseTypeID,RepeatIn=@RepeatIn,RepeatOut=@RepeatOut,AffirmIn=@AffirmIn,
                                AffirmOut=@AffirmOut,InBeginTime=@InBeginTime,InEdnTime=@InEdnTime,MaxUseMoney=@MaxUseMoney,AllowLose=@AllowLose,LpDistinguish=@LpDistinguish,InOutEditCar=@InOutEditCar,InOutTime=@InOutTime,
                                CarNoLike=@CarNoLike,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,IsAllowOnlIne=@IsAllowOnlIne,Amount=@Amount,MaxMonth=@MaxMonth,MaxValue=@MaxValue,DataStatus=@DataStatus,OverdueToTemp=@OverdueToTemp,
                                LotOccupy=@LotOccupy,Deposit=@Deposit,MonthCardExpiredEnterDay=@MonthCardExpiredEnterDay,AffirmBegin=@AffirmBegin,AffirmEnd=@AffirmEnd,IsNeedCapturePaper=@IsNeedCapturePaper,IsDispatch=@IsDispatch,OnlineUnit=@OnlineUnit,    
                                IsIgnoreHZ=@IsIgnoreHZ where CarTypeID=@CarTypeID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("CarTypeName", obj.CarTypeName);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("BaseTypeID", obj.BaseTypeID);
                                dbOperator.AddParameter("RepeatIn", obj.RepeatIn);
                                dbOperator.AddParameter("RepeatOut", obj.RepeatOut);
                                dbOperator.AddParameter("AffirmIn", obj.AffirmIn);
                                dbOperator.AddParameter("AffirmOut", obj.AffirmOut);
                                if (obj.InBeginTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("InBeginTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("InBeginTime", obj.InBeginTime);
                                }
                                if (obj.InEdnTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("InEdnTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("InEdnTime", obj.InEdnTime);
                                }
                               
                                dbOperator.AddParameter("MaxUseMoney", obj.MaxUseMoney);
                                dbOperator.AddParameter("AllowLose", obj.AllowLose);
                                dbOperator.AddParameter("LpDistinguish", obj.LpDistinguish);
                                dbOperator.AddParameter("InOutEditCar", obj.InOutEditCar);
                                dbOperator.AddParameter("InOutTime", obj.InOutTime);
                                dbOperator.AddParameter("CarNoLike", obj.CarNoLike);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                              
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("IsAllowOnlIne", obj.IsAllowOnlIne);
                                dbOperator.AddParameter("Amount", obj.Amount);
                                dbOperator.AddParameter("MaxMonth", obj.MaxMonth);
                                dbOperator.AddParameter("MaxValue", obj.MaxValue);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("OverdueToTemp", obj.OverdueToTemp);
                                dbOperator.AddParameter("LotOccupy", obj.LotOccupy);
                                dbOperator.AddParameter("Deposit", obj.Deposit);
                                dbOperator.AddParameter("MonthCardExpiredEnterDay", obj.MonthCardExpiredEnterDay);
                                dbOperator.AddParameter("AffirmBegin", obj.AffirmBegin);
                                dbOperator.AddParameter("AffirmEnd", obj.AffirmEnd);
                                dbOperator.AddParameter("CarTypeID", obj.CarTypeID);
                                dbOperator.AddParameter("IsNeedCapturePaper", obj.IsNeedCapturePaper);
                                dbOperator.AddParameter("IsDispatch", obj.IsDispatch);
                                dbOperator.AddParameter("OnlineUnit", obj.OnlineUnit);
                                dbOperator.AddParameter("IsIgnoreHZ", obj.IsIgnoreHZ);
                                
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.CarTypeID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkCarType( CarTypeID,CarTypeName,PKID,BaseTypeID,RepeatIn,RepeatOut,AffirmIn,
                                                            AffirmOut,InBeginTime,InEdnTime,MaxUseMoney,AllowLose,LpDistinguish,InOutEditCar,InOutTime,
                                                            CarNoLike,LastUpdateTime,HaveUpdate,IsAllowOnlIne,Amount,MaxMonth,MaxValue,DataStatus,OverdueToTemp,
                                                            LotOccupy,Deposit,MonthCardExpiredEnterDay,AffirmBegin,AffirmEnd,IsNeedCapturePaper,IsDispatch,OnlineUnit,IsIgnoreHZ)
                                                                values(@CarTypeID,@CarTypeName,@PKID,@BaseTypeID,@RepeatIn,@RepeatOut,@AffirmIn,
                                                            @AffirmOut,@InBeginTime,@InEdnTime,@MaxUseMoney,@AllowLose,@LpDistinguish,@InOutEditCar,@InOutTime,
                                                            @CarNoLike,@LastUpdateTime,@HaveUpdate,@IsAllowOnlIne,@Amount,@MaxMonth,@MaxValue,@DataStatus,@OverdueToTemp,
                                                            @LotOccupy,@Deposit,@MonthCardExpiredEnterDay,@AffirmBegin,@AffirmEnd,@IsNeedCapturePaper,@IsDispatch,@OnlineUnit,@IsIgnoreHZ)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("CarTypeID", obj.CarTypeID);
                                dbOperator.AddParameter("CarTypeName", obj.CarTypeName);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("BaseTypeID", obj.BaseTypeID);
                                dbOperator.AddParameter("RepeatIn", obj.RepeatIn);
                                dbOperator.AddParameter("RepeatOut", obj.RepeatOut);
                                dbOperator.AddParameter("AffirmIn", obj.AffirmIn);
                                dbOperator.AddParameter("AffirmOut", obj.AffirmOut);
                                if (obj.InBeginTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("InBeginTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("InBeginTime", obj.InBeginTime);
                                }
                                if (obj.InEdnTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("InEdnTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("InEdnTime", obj.InEdnTime);
                                }
                                dbOperator.AddParameter("MaxUseMoney", obj.MaxUseMoney);
                                dbOperator.AddParameter("AllowLose", obj.AllowLose);
                                dbOperator.AddParameter("LpDistinguish", obj.LpDistinguish);
                                dbOperator.AddParameter("InOutEditCar", obj.InOutEditCar);
                                dbOperator.AddParameter("InOutTime", obj.InOutTime);
                                dbOperator.AddParameter("CarNoLike", obj.CarNoLike);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("IsAllowOnlIne", obj.IsAllowOnlIne);
                                dbOperator.AddParameter("Amount", obj.Amount);
                                dbOperator.AddParameter("MaxMonth", obj.MaxMonth);
                                dbOperator.AddParameter("MaxValue", obj.MaxValue);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("OverdueToTemp", obj.OverdueToTemp);
                                dbOperator.AddParameter("LotOccupy", obj.LotOccupy);
                                dbOperator.AddParameter("Deposit", obj.Deposit);
                                dbOperator.AddParameter("MonthCardExpiredEnterDay", obj.MonthCardExpiredEnterDay);
                                dbOperator.AddParameter("AffirmBegin", obj.AffirmBegin);
                                dbOperator.AddParameter("AffirmEnd", obj.AffirmEnd);
                                dbOperator.AddParameter("IsNeedCapturePaper", obj.IsNeedCapturePaper);
                                dbOperator.AddParameter("IsDispatch", obj.IsDispatch);
                                dbOperator.AddParameter("OnlineUnit", obj.OnlineUnit);
                                dbOperator.AddParameter("IsIgnoreHZ", obj.IsIgnoreHZ);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.CarTypeID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载车类信息异常：UploadParkCarType()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载车型信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkCarModel(List<ParkCarModel> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkCarModel where CarModelID=@CarModelID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("CarModelID", obj.CarModelID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkCarModel set CarModelName=@CarModelName,PKID=@PKID,IsDefault=@IsDefault,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,
                                DataStatus=@DataStatus,MaxUseMoney=@MaxUseMoney,IsNaturalDay=@IsNaturalDay,PlateColor=@PlateColor  
                                ,DayMaxMoney=@DayMaxMoney,NightMaxMoney=@NightMaxMoney,DayStartTime=@DayStartTime,DayEndTime=@DayEndTime,NightStartTime=@NightStartTime,NightEndTime=@NightEndTime,NaturalTime=@NaturalTime where CarModelID=@CarModelID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("CarModelName", obj.CarModelName);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("IsDefault", obj.IsDefault);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("CarModelID", obj.CarModelID);
                                dbOperator.AddParameter("MaxUseMoney", obj.MaxUseMoney);
                                dbOperator.AddParameter("IsNaturalDay", obj.IsNaturalDay);
                                dbOperator.AddParameter("PlateColor", obj.PlateColor);

                                dbOperator.AddParameter("DayMaxMoney", obj.DayMaxMoney);
                                dbOperator.AddParameter("NightMaxMoney", obj.NightMaxMoney);
                                dbOperator.AddParameter("DayStartTime", obj.DayStartTime);
                                dbOperator.AddParameter("DayEndTime", obj.DayEndTime);
                                dbOperator.AddParameter("NightStartTime", obj.NightStartTime);
                                dbOperator.AddParameter("NightEndTime", obj.NightEndTime);
                                dbOperator.AddParameter("NaturalTime", obj.NaturalTime);
                                
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.CarModelID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkCarModel(CarModelID,CarModelName,PKID,IsDefault,LastUpdateTime,HaveUpdate,DataStatus,MaxUseMoney,IsNaturalDay,PlateColor
                                                    ,DayMaxMoney,NightMaxMoney,DayStartTime,DayEndTime,NightStartTime,NightEndTime,NaturalTime)
                                                    values(@CarModelID,@CarModelName,@PKID,@IsDefault,@LastUpdateTime,@HaveUpdate,@DataStatus,@MaxUseMoney,@IsNaturalDay,@PlateColor
                                                    ,@DayMaxMoney,@NightMaxMoney,@DayStartTime,@DayEndTime,@NightStartTime,@NightEndTime,@NaturalTime)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("CarModelName", obj.CarModelName);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("IsDefault", obj.IsDefault);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("CarModelID", obj.CarModelID);
                                dbOperator.AddParameter("MaxUseMoney", obj.MaxUseMoney);
                                dbOperator.AddParameter("IsNaturalDay", obj.IsNaturalDay);
                                dbOperator.AddParameter("PlateColor", obj.PlateColor);

                                dbOperator.AddParameter("DayMaxMoney", obj.DayMaxMoney);
                                dbOperator.AddParameter("NightMaxMoney", obj.NightMaxMoney);
                                dbOperator.AddParameter("DayStartTime", obj.DayStartTime);
                                dbOperator.AddParameter("DayEndTime", obj.DayEndTime);
                                dbOperator.AddParameter("NightStartTime", obj.NightStartTime);
                                dbOperator.AddParameter("NightEndTime", obj.NightEndTime);
                                dbOperator.AddParameter("NaturalTime", obj.NaturalTime);
                                
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.CarModelID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载车型信息异常：UploadParkCarModel()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载收费规则
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkFeeRule(List<ParkFeeRule> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkFeeRule where FeeRuleID=@FeeRuleID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("FeeRuleID", obj.FeeRuleID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkFeeRule set RuleName=@RuleName,FeeType=@FeeType,CarTypeID=@CarTypeID,CarModelID=@CarModelID,AreaID=@AreaID,
                                LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus,RuleText=@RuleText,IsOffline=@IsOffline   where FeeRuleID=@FeeRuleID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RuleName", obj.RuleName);
                                dbOperator.AddParameter("FeeType", obj.FeeType);
                                dbOperator.AddParameter("CarTypeID", obj.CarTypeID);
                                dbOperator.AddParameter("CarModelID", obj.CarModelID);
                                dbOperator.AddParameter("RuleText", obj.RuleText);
                                dbOperator.AddParameter("AreaID", obj.AreaID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("FeeRuleID", obj.FeeRuleID);
                                dbOperator.AddParameter("IsOffline", obj.IsOffline);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.FeeRuleID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkFeeRule(FeeRuleID,RuleName,FeeType,CarTypeID,CarModelID,AreaID,LastUpdateTime,HaveUpdate,DataStatus,RuleText,IsOffline)
                                                                values(@FeeRuleID,@RuleName,@FeeType,@CarTypeID,@CarModelID,@AreaID,@LastUpdateTime,@HaveUpdate,@DataStatus,@RuleText,@IsOffline)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("FeeRuleID", obj.FeeRuleID);
                                dbOperator.AddParameter("RuleName", obj.RuleName);
                                dbOperator.AddParameter("FeeType", obj.FeeType);
                                dbOperator.AddParameter("CarTypeID", obj.CarTypeID);
                                dbOperator.AddParameter("CarModelID", obj.CarModelID);
                                dbOperator.AddParameter("RuleText", obj.RuleText);
                                dbOperator.AddParameter("AreaID", obj.AreaID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("IsOffline", obj.IsOffline);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.FeeRuleID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载收费规则异常：UploadParkFeeRule()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载收费规则明细
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkFeeRuleDetail(List<ParkFeeRuleDetail> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkFeeRuleDetail where RuleDetailID=@RuleDetailID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RuleDetailID", obj.RuleDetailID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkFeeRuleDetail set RuleID=@RuleID,StartTime=@StartTime,EndTime=@EndTime,Supplement=@Supplement,LoopType=@LoopType,
                                Limit=@Limit,FreeTime=@FreeTime,FirstTime=@FirstTime,FirstFee=@FirstFee,Loop1PerTime=@Loop1PerTime,Loop1PerFee=@Loop1PerFee,
                                Loop2Start=@Loop2Start,Loop2PerTime=@Loop2PerTime,Loop2PerFee=@Loop2PerFee,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus 
                                where RuleDetailID=@RuleDetailID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RuleID", obj.RuleID);
                                dbOperator.AddParameter("StartTime", obj.StartTime);
                                dbOperator.AddParameter("EndTime", obj.EndTime);
                                dbOperator.AddParameter("Supplement", obj.Supplement);
                                dbOperator.AddParameter("LoopType", obj.LoopType);

                                dbOperator.AddParameter("Limit", obj.Limit);
                                dbOperator.AddParameter("FreeTime", obj.FreeTime);
                                dbOperator.AddParameter("FirstTime", obj.FirstTime);
                                dbOperator.AddParameter("FirstFee", obj.FirstFee);
                                dbOperator.AddParameter("Loop1PerTime", obj.Loop1PerTime);
                                dbOperator.AddParameter("Loop1PerFee", obj.Loop1PerFee);
                                dbOperator.AddParameter("Loop2Start", obj.Loop2Start);
                                dbOperator.AddParameter("Loop2PerTime", obj.Loop2PerTime);
                                dbOperator.AddParameter("Loop2PerFee", obj.Loop2PerFee);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("RuleDetailID", obj.RuleDetailID);

                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RuleDetailID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkFeeRuleDetail(RuleDetailID,RuleID,StartTime,EndTime,Supplement,LoopType,Limit,FreeTime,FirstTime,FirstFee,
                                                                Loop1PerTime,Loop1PerFee,Loop2Start,Loop2PerTime,Loop2PerFee,LastUpdateTime,HaveUpdate,DataStatus)
                                                                values(@RuleDetailID,@RuleID,@StartTime,@EndTime,@Supplement,@LoopType,@Limit,@FreeTime,@FirstTime,@FirstFee,
                                                                @Loop1PerTime,@Loop1PerFee,@Loop2Start,@Loop2PerTime,@Loop2PerFee,@LastUpdateTime,@HaveUpdate,@DataStatus)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RuleDetailID", obj.RuleDetailID);
                                dbOperator.AddParameter("RuleID", obj.RuleID);
                                dbOperator.AddParameter("StartTime", obj.StartTime);
                                dbOperator.AddParameter("EndTime", obj.EndTime);
                                dbOperator.AddParameter("Supplement", obj.Supplement);
                                dbOperator.AddParameter("LoopType", obj.LoopType);
                                dbOperator.AddParameter("Limit", obj.Limit);
                                dbOperator.AddParameter("FreeTime", obj.FreeTime);
                                dbOperator.AddParameter("FirstTime", obj.FirstTime);
                                dbOperator.AddParameter("FirstFee", obj.FirstFee);
                                dbOperator.AddParameter("Loop1PerTime", obj.Loop1PerTime);
                                dbOperator.AddParameter("Loop1PerFee", obj.Loop1PerFee);
                                dbOperator.AddParameter("Loop2Start", obj.Loop2Start);
                                dbOperator.AddParameter("Loop2PerTime", obj.Loop2PerTime);
                                dbOperator.AddParameter("Loop2PerFee", obj.Loop2PerFee);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RuleDetailID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载收费规则明细异常：UploadParkFeeRuleDetail()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载卡片信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadBaseCard(List<BaseCard> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from BaseCard where CardID=@CardID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("CardID", obj.CardID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update BaseCard set CardNo=@CardNo,CardNumb=@CardNumb,CardType=@CardType,Balance=@Balance,State=@State,EmployeeID=@EmployeeID,
                                LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus,Deposit=@Deposit,RegisterTime=@RegisterTime,OperatorID=@OperatorID,
                                CardSystem=@CardSystem,VID=@VID 
                                where CardID=@CardID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("CardNo", obj.CardNo);
                                dbOperator.AddParameter("CardNumb", obj.CardNumb);
                                dbOperator.AddParameter("CardType", obj.CardType);
                                dbOperator.AddParameter("Balance", obj.Balance);
                                dbOperator.AddParameter("State", obj.State);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("Deposit", obj.Deposit);
                                if (obj.RegisterTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("RegisterTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("RegisterTime", obj.RegisterTime);
                                }
                               
                                dbOperator.AddParameter("OperatorID", obj.OperatorID);
                                dbOperator.AddParameter("CardSystem", obj.CardSystem);
                                dbOperator.AddParameter("VID", obj.VID);
                                dbOperator.AddParameter("CardID", obj.CardID);
                                dbOperator.AddParameter("EmployeeID", obj.EmployeeID);
                                
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.CardID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into BaseCard(CardID,CardNo,CardNumb,CardType,Balance,State,
                                                                LastUpdateTime,HaveUpdate,DataStatus,Deposit,RegisterTime,OperatorID,CardSystem,VID,EmployeeID )
                                                                values(@CardID,@CardNo,@CardNumb,@CardType,@Balance,@State,
                                                                @LastUpdateTime,@HaveUpdate,@DataStatus,@Deposit,@RegisterTime,@OperatorID,@CardSystem,@VID,@EmployeeID)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("CardID", obj.CardID);
                                dbOperator.AddParameter("CardNo", obj.CardNo);
                                dbOperator.AddParameter("CardNumb", obj.CardNumb);
                                dbOperator.AddParameter("CardType", obj.CardType);
                                dbOperator.AddParameter("Balance", obj.Balance);
                                dbOperator.AddParameter("State", obj.State);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("Deposit", obj.Deposit);
                                if (obj.RegisterTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("RegisterTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("RegisterTime", obj.RegisterTime);
                                }
                                dbOperator.AddParameter("OperatorID", obj.OperatorID);
                                dbOperator.AddParameter("CardSystem", obj.CardSystem);
                                dbOperator.AddParameter("VID", obj.VID);
                                dbOperator.AddParameter("EmployeeID", obj.EmployeeID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.CardID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载卡片信息异常：UploadBaseCard()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载车场授权信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkGrant(List<ParkGrant> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        if (obj.GID != null)
                        {
                            string sql = "select ID from ParkGrant where GID=@GID ";
                            dbOperator.ClearParameters();
                            dbOperator.AddParameter("GID", obj.GID);
                            using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                            {
                                if (reader.Read())
                                {
                                    reader.Close();
                                    sql = string.Format(@"update ParkGrant set CardID=@CardID,PKID=@PKID,BeginDate=@BeginDate,EndDate=@EndDate,CarTypeID=@CarTypeID,
                                CarModelID=@CarModelID,PKLot=@PKLot,PlateID=@PlateID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,ComdState=@ComdState,
                                AreaIDS=@AreaIDS,GateID=@GateID,State=@State,DataStatus=@DataStatus,PKLotNum=@PKLotNum  
                                where GID=@GID ");
                                    dbOperator.ClearParameters();
                                    dbOperator.AddParameter("CardID", obj.CardID);
                                    dbOperator.AddParameter("PKID", obj.PKID);
                                    if (obj.BeginDate == DateTime.MinValue)
                                    {
                                        dbOperator.AddParameter("BeginDate", DBNull.Value);
                                    }
                                    else
                                    {
                                        dbOperator.AddParameter("BeginDate", obj.BeginDate);
                                    }
                                    if (obj.EndDate == DateTime.MinValue)
                                    {
                                        dbOperator.AddParameter("EndDate", DBNull.Value);
                                    }
                                    else
                                    {
                                        dbOperator.AddParameter("EndDate", obj.EndDate);
                                    }

                                    dbOperator.AddParameter("CarTypeID", obj.CarTypeID);
                                    dbOperator.AddParameter("CarModelID", obj.CarModelID);
                                    dbOperator.AddParameter("PKLot", obj.PKLot);
                                    dbOperator.AddParameter("PlateID", obj.PlateID);
                                    if (obj.LastUpdateTime == DateTime.MinValue)
                                    {
                                        dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                    }
                                    else
                                    {
                                        dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                    }
                                    dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                    dbOperator.AddParameter("ComdState", obj.ComdState);
                                    dbOperator.AddParameter("AreaIDS", obj.AreaIDS);
                                    dbOperator.AddParameter("GateID", obj.GateID);
                                    dbOperator.AddParameter("State", obj.State);
                                    dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                    dbOperator.AddParameter("GID", obj.GID);
                                    dbOperator.AddParameter("PKLotNum", obj.PKLotNum);
                                    
                                    if (dbOperator.ExecuteNonQuery(sql) > 0)
                                    {
                                        listresult.Add(new UploadResult() { RecordID = obj.GID, ID = obj.ID });
                                    }
                                }
                                else
                                {
                                    reader.Close();
                                    sql = string.Format(@"insert into ParkGrant(GID,CardID,PKID,BeginDate,EndDate,CarTypeID,
                                                                CarModelID,PKLot,PlateID,LastUpdateTime,HaveUpdate,ComdState,AreaIDS,GateID,State,DataStatus )
                                                                values(@GID,@CardID,@PKID,@BeginDate,@EndDate,@CarTypeID,
                                                                @CarModelID,@PKLot,@PlateID,@LastUpdateTime,@HaveUpdate,@ComdState,@AreaIDS,@GateID,@State,@DataStatus)");
                                    dbOperator.ClearParameters();
                                    dbOperator.AddParameter("GID", obj.GID);
                                    dbOperator.AddParameter("CardID", obj.CardID);
                                    dbOperator.AddParameter("PKID", obj.PKID);
                                    if (obj.BeginDate == DateTime.MinValue)
                                    {
                                        dbOperator.AddParameter("BeginDate", DBNull.Value);
                                    }
                                    else
                                    {
                                        dbOperator.AddParameter("BeginDate", obj.BeginDate);
                                    }
                                    if (obj.EndDate == DateTime.MinValue)
                                    {
                                        dbOperator.AddParameter("EndDate", DBNull.Value);
                                    }
                                    else
                                    {
                                        dbOperator.AddParameter("EndDate", obj.EndDate);
                                    }
                                    dbOperator.AddParameter("CarTypeID", obj.CarTypeID);
                                    dbOperator.AddParameter("CarModelID", obj.CarModelID);
                                    dbOperator.AddParameter("PKLot", obj.PKLot);
                                    dbOperator.AddParameter("PlateID", obj.PlateID);
                                    if (obj.LastUpdateTime == DateTime.MinValue)
                                    {
                                        dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                    }
                                    else
                                    {
                                        dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                    }
                                    dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                    dbOperator.AddParameter("ComdState", obj.ComdState);
                                    dbOperator.AddParameter("AreaIDS", obj.AreaIDS);
                                    dbOperator.AddParameter("GateID", obj.GateID);
                                    dbOperator.AddParameter("State", obj.State);
                                    dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                    if (dbOperator.ExecuteNonQuery(sql) > 0)
                                    {
                                        listresult.Add(new UploadResult() { RecordID = obj.GID, ID = obj.ID });
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载车场授权信息异常：UploadParkGrant()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载车场授权暂停信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkCardSuspendPlan(List<ParkCardSuspendPlan> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkCardSuspendPlan where RecordId=@RecordId ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordId", obj.RecordId);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkCardSuspendPlan set GrantID=@GrantID,StartDate=@StartDate,EndDate=@EndDate,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime,
                                DataStatus=@DataStatus,CreateTime=@CreateTime where RecordId=@RecordId ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("GrantID", obj.GrantID);
                                if (obj.StartDate == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("StartDate", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("StartDate", obj.StartDate);
                                }
                                if (obj.EndDate == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("EndDate", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("EndDate", obj.EndDate);
                                }
                              
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                if (obj.CreateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("CreateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("CreateTime", obj.CreateTime);
                                }
                              
                                dbOperator.AddParameter("RecordId", obj.RecordId);
                               
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordId, ID = obj.Id });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkCardSuspendPlan(RecordId,GrantID,StartDate,EndDate,HaveUpdate,LastUpdateTime, DataStatus,CreateTime )
                                                                values(@RecordId,@GrantID,@StartDate,@EndDate,@HaveUpdate,@LastUpdateTime, @DataStatus,@CreateTime )");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordId", obj.RecordId);
                                dbOperator.AddParameter("GrantID", obj.GrantID);
                                if (obj.StartDate == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("StartDate", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("StartDate", obj.StartDate);
                                }
                                if (obj.EndDate == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("EndDate", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("EndDate", obj.EndDate);
                                }
                              
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                if (obj.CreateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("CreateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("CreateTime", obj.CreateTime);
                                }

                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordId, ID = obj.Id });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载车场授权暂停信息异常：UploadParkCardSuspendPlan()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载人员信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadBaseEmployee(List<BaseEmployee> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from BaseEmployee where EmployeeID=@EmployeeID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("EmployeeID", obj.EmployeeID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update BaseEmployee set EmployeeName=@EmployeeName,Sex=@Sex,CertifType=@CertifType,CertifNo=@CertifNo,MobilePhone=@MobilePhone,
                                HomePhone=@HomePhone,Email=@Email,FamilyAddr=@FamilyAddr,RegTime=@RegTime,EmployeeType=@EmployeeType,Remark=@Remark,
                                LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus,AreaName=@AreaName, DeptID=@DeptID,VID=@VID
                                where EmployeeID=@EmployeeID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("EmployeeName", obj.EmployeeName);
                                dbOperator.AddParameter("Sex", obj.Sex);
                                dbOperator.AddParameter("CertifType", obj.CertifType);
                                dbOperator.AddParameter("CertifNo", obj.CertifNo);
                                dbOperator.AddParameter("MobilePhone", obj.MobilePhone);
                                dbOperator.AddParameter("HomePhone", obj.HomePhone);
                                dbOperator.AddParameter("Email", obj.Email);
                                dbOperator.AddParameter("FamilyAddr", obj.FamilyAddr);
                                if (obj.RegTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("RegTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("RegTime", obj.RegTime);
                                }
                              
                                dbOperator.AddParameter("EmployeeType", obj.EmployeeType);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("AreaName", obj.AreaName);
                                dbOperator.AddParameter("DeptID", obj.DeptID);
                                dbOperator.AddParameter("VID", obj.VID);
                                dbOperator.AddParameter("EmployeeID", obj.EmployeeID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.EmployeeID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into BaseEmployee(EmployeeID,EmployeeName,Sex,CertifType,CertifNo,MobilePhone,
                                                                HomePhone,Email,FamilyAddr,RegTime,EmployeeType,Remark,LastUpdateTime,HaveUpdate,DataStatus,AreaName, DeptID,VID )
                                                                values(@EmployeeID,@EmployeeName,@Sex,@CertifType,@CertifNo,@MobilePhone,
                                                                @HomePhone,@Email,@FamilyAddr,@RegTime,@EmployeeType,@Remark,@LastUpdateTime,@HaveUpdate,@DataStatus,@AreaName, @DeptID,@VID)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("EmployeeID", obj.EmployeeID);
                                dbOperator.AddParameter("EmployeeName", obj.EmployeeName);
                                dbOperator.AddParameter("Sex", obj.Sex);
                                dbOperator.AddParameter("CertifType", obj.CertifType);
                                dbOperator.AddParameter("CertifNo", obj.CertifNo);
                                dbOperator.AddParameter("MobilePhone", obj.MobilePhone);
                                dbOperator.AddParameter("HomePhone", obj.HomePhone);
                                dbOperator.AddParameter("Email", obj.Email);
                                dbOperator.AddParameter("FamilyAddr", obj.FamilyAddr);
                                if (obj.RegTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("RegTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("RegTime", obj.RegTime);
                                }
                                dbOperator.AddParameter("EmployeeType", obj.EmployeeType);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("AreaName", obj.AreaName);
                                dbOperator.AddParameter("DeptID", obj.DeptID);
                                dbOperator.AddParameter("VID", obj.VID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.EmployeeID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载人员信息异常：UploadBaseEmployee()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载人员车牌信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadEmployeePlate(List<EmployeePlate> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from EmployeePlate where PlateID=@PlateID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("PlateID", obj.PlateID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update EmployeePlate set EmployeeID=@EmployeeID,PlateNo=@PlateNo,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus,
                                Color=@Color,CarBrand=@CarBrand where PlateID=@PlateID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("EmployeeID", obj.EmployeeID);
                                dbOperator.AddParameter("PlateNo", obj.PlateNo);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("Color", obj.Color);
                                dbOperator.AddParameter("CarBrand", obj.CarBrand);
                                dbOperator.AddParameter("PlateID", obj.PlateID);
                            
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.PlateID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into EmployeePlate(PlateID, EmployeeID,PlateNo,LastUpdateTime,HaveUpdate,DataStatus,Color,CarBrand )
                                                                values(@PlateID, @EmployeeID,@PlateNo,@LastUpdateTime,@HaveUpdate,@DataStatus,@Color,@CarBrand )");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("PlateID", obj.PlateID);
                                dbOperator.AddParameter("EmployeeID", obj.EmployeeID);
                                dbOperator.AddParameter("PlateNo", obj.PlateNo);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("Color", obj.Color);
                                dbOperator.AddParameter("CarBrand", obj.CarBrand);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.PlateID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载人员车牌信息异常：UploadEmployeePlate()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载商家信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkSeller(List<ParkSeller> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkSeller where SellerID=@SellerID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("SellerID", obj.SellerID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkSeller set SellerNo=@SellerNo,SellerName=@SellerName,PWD=@PWD,VID=@VID,Addr=@Addr,
                                LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus,Creditline=@Creditline,Balance=@Balance where SellerID=@SellerID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("SellerNo", obj.SellerNo);
                                dbOperator.AddParameter("SellerName", obj.SellerName);
                                dbOperator.AddParameter("PWD", obj.PWD);
                                dbOperator.AddParameter("VID", obj.VID);
                                dbOperator.AddParameter("Addr", obj.Addr);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("Creditline", obj.Creditline);
                                dbOperator.AddParameter("Balance", obj.Balance);
                                dbOperator.AddParameter("SellerID", obj.SellerID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.SellerID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkSeller(SellerID,SellerNo,SellerName,PWD,VID,Addr,
                                                                LastUpdateTime,HaveUpdate,DataStatus,Creditline,Balance )
                                                                values(@SellerID,@SellerNo,@SellerName,@PWD,@VID,@Addr,
                                                                @LastUpdateTime,@HaveUpdate,@DataStatus,@Creditline,@Balance )");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("SellerID", obj.SellerID);
                                dbOperator.AddParameter("SellerNo", obj.SellerNo);
                                dbOperator.AddParameter("SellerName", obj.SellerName);
                                dbOperator.AddParameter("PWD", obj.PWD);
                                dbOperator.AddParameter("VID", obj.VID);
                                dbOperator.AddParameter("Addr", obj.Addr);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("Creditline", obj.Creditline);
                                dbOperator.AddParameter("Balance", obj.Balance);
                                
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.SellerID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载商家信息异常：UploadParkSeller()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载商家优免券
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkCarDerate(List<ParkCarDerate> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkCarDerate where CarDerateID=@CarDerateID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("CarDerateID", obj.CarDerateID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkCarDerate set CarDerateNo=@CarDerateNo,DerateID=@DerateID,FreeTime=@FreeTime,FreeMoney=@FreeMoney,PlateNumber=@PlateNumber,
                                CardNo=@CardNo,IORecordID=@IORecordID,Status=@Status,CreateTime=@CreateTime,ExpiryTime=@ExpiryTime,PKID=@PKID,AreaID=@AreaID,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus,LastUpdateTime=@LastUpdateTime 
                                where CarDerateID=@CarDerateID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("CarDerateNo", obj.CarDerateNo);
                                dbOperator.AddParameter("DerateID", obj.DerateID);
                                dbOperator.AddParameter("FreeTime", obj.FreeTime);
                                dbOperator.AddParameter("FreeMoney", obj.FreeMoney);
                                dbOperator.AddParameter("PlateNumber", obj.PlateNumber);
                                dbOperator.AddParameter("CardNo", obj.CardNo);
                                dbOperator.AddParameter("IORecordID", obj.IORecordID);
                                dbOperator.AddParameter("Status", obj.Status);
                                if (obj.CreateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("CreateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("CreateTime", obj.CreateTime);
                                }
                                if (obj.ExpiryTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("ExpiryTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("ExpiryTime", obj.ExpiryTime);
                                }
                          
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("AreaID", obj.AreaID);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("CarDerateID", obj.CarDerateID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.CarDerateID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkCarDerate( CarDerateID,CarDerateNo,DerateID,FreeTime,FreeMoney,PlateNumber,
                                                                CardNo,IORecordID,Status,CreateTime,ExpiryTime,PKID,AreaID,HaveUpdate,DataStatus,LastUpdateTime )
                                                                values(@CarDerateID,@CarDerateNo,@DerateID,@FreeTime,@FreeMoney,@PlateNumber,
                                                                @CardNo,@IORecordID,@Status,@CreateTime,@ExpiryTime,@PKID,@AreaID,@HaveUpdate,@DataStatus,@LastUpdateTime )");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("CarDerateID", obj.CarDerateID);
                                dbOperator.AddParameter("CarDerateNo", obj.CarDerateNo);
                                dbOperator.AddParameter("DerateID", obj.DerateID);
                                dbOperator.AddParameter("FreeTime", obj.FreeTime);
                                dbOperator.AddParameter("FreeMoney", obj.FreeMoney);
                                dbOperator.AddParameter("PlateNumber", obj.PlateNumber);
                                dbOperator.AddParameter("CardNo", obj.CardNo);
                                dbOperator.AddParameter("IORecordID", obj.IORecordID);
                                dbOperator.AddParameter("Status", obj.Status);
                                if (obj.CreateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("CreateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("CreateTime", obj.CreateTime);
                                }
                                if (obj.ExpiryTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("ExpiryTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("ExpiryTime", obj.ExpiryTime);
                                }
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("AreaID", obj.AreaID);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.CarDerateID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载商家优免券异常：UploadParkCarDerate()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载下载商家优免信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkDerate(List<ParkDerate> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkDerate where DerateID=@DerateID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("DerateID", obj.DerateID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkDerate set SellerID=@SellerID,Name=@Name,DerateSwparate=@DerateSwparate,DerateType=@DerateType,DerateMoney=@DerateMoney,
                                FreeTime=@FreeTime,StartTime=@StartTime,EndTime=@EndTime,FeeRuleID=@FeeRuleID,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime,DataStatus=@DataStatus
                                ,MaxTimesCycle=@MaxTimesCycle,MaxTimes=@MaxTimes,ValidityTime=@ValidityTime where DerateID=@DerateID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("SellerID", obj.SellerID);
                                dbOperator.AddParameter("Name", obj.Name);
                                dbOperator.AddParameter("DerateSwparate", obj.DerateSwparate);
                                dbOperator.AddParameter("DerateType", obj.DerateType);
                                dbOperator.AddParameter("DerateMoney", obj.DerateMoney);
                                dbOperator.AddParameter("FreeTime", obj.FreeTime);
                                if (obj.StartTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("StartTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("StartTime", obj.StartTime);
                                }
                                if (obj.EndTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("EndTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("EndTime", obj.EndTime);
                                }
                              
                                dbOperator.AddParameter("FeeRuleID", obj.FeeRuleID);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("DerateID", obj.DerateID);
                                dbOperator.AddParameter("MaxTimesCycle", obj.MaxTimesCycle);
                                dbOperator.AddParameter("MaxTimes", obj.MaxTimes);
                                dbOperator.AddParameter("ValidityTime", obj.ValidityTime);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.DerateID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkDerate( DerateID,SellerID,Name,DerateSwparate,DerateType,DerateMoney,
                                                                FreeTime,StartTime,EndTime,FeeRuleID,HaveUpdate,LastUpdateTime,DataStatus,MaxTimesCycle,MaxTimes,ValidityTime )
                                                                values(@DerateID,@SellerID,@Name,@DerateSwparate,@DerateType,@DerateMoney,
                                                                @FreeTime,@StartTime,@EndTime,@FeeRuleID,@HaveUpdate,@LastUpdateTime,@DataStatus,@MaxTimesCycle,@MaxTimes,@ValidityTime)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("DerateID", obj.DerateID);
                                dbOperator.AddParameter("SellerID", obj.SellerID);
                                dbOperator.AddParameter("Name", obj.Name);
                                dbOperator.AddParameter("DerateSwparate", obj.DerateSwparate);
                                dbOperator.AddParameter("DerateType", obj.DerateType);
                                dbOperator.AddParameter("DerateMoney", obj.DerateMoney);
                                dbOperator.AddParameter("FreeTime", obj.FreeTime);
                                if (obj.StartTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("StartTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("StartTime", obj.StartTime);
                                }
                                if (obj.EndTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("EndTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("EndTime", obj.EndTime);
                                }
                                dbOperator.AddParameter("FeeRuleID", obj.FeeRuleID);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("MaxTimesCycle", obj.MaxTimesCycle);
                                dbOperator.AddParameter("MaxTimes", obj.MaxTimes);
                                dbOperator.AddParameter("ValidityTime", obj.ValidityTime);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.DerateID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载下载商家优免信息异常：UploadParkDerate()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载优免时段
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkDerateIntervar(List<ParkDerateIntervar> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkDerateIntervar where DerateIntervarID=@DerateIntervarID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("DerateIntervarID", obj.DerateIntervarID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkDerateIntervar set DerateID=@DerateID,FreeTime=@FreeTime,Monetry=@Monetry,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime,
                                DataStatus=@DataStatus where DerateIntervarID=@DerateIntervarID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("DerateID", obj.DerateID);
                                dbOperator.AddParameter("FreeTime", obj.FreeTime);
                                dbOperator.AddParameter("Monetry", obj.Monetry);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("DerateIntervarID", obj.DerateIntervarID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.DerateIntervarID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkDerateIntervar( DerateIntervarID,DerateID,FreeTime,Monetry,HaveUpdate,LastUpdateTime,DataStatus )
                                                                values( @DerateIntervarID,@DerateID,@FreeTime,@Monetry,@HaveUpdate,@LastUpdateTime,@DataStatus )");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("DerateIntervarID", obj.DerateIntervarID);
                                dbOperator.AddParameter("DerateID", obj.DerateID);
                                dbOperator.AddParameter("FreeTime", obj.FreeTime);
                                dbOperator.AddParameter("Monetry", obj.Monetry);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.DerateIntervarID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载优免时段异常：UploadParkDerateIntervar()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }


        /// <summary>
        /// 上传下载黑名单
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkBlacklist(List<ParkBlacklist> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkBlacklist where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkBlacklist set PKID=@PKID,PlateNumber=@PlateNumber,Remark=@Remark,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,
                                DataStatus=@DataStatus,Status=@Status where RecordID=@RecordID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("PlateNumber", obj.PlateNumber);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("Status", obj.Status);
                             
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkBlacklist(RecordID,PKID,PlateNumber,Remark,LastUpdateTime,HaveUpdate,DataStatus,Status )
                                                                values(@RecordID,@PKID,@PlateNumber,@Remark,@LastUpdateTime,@HaveUpdate,@DataStatus,@Status)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("PlateNumber", obj.PlateNumber);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("Status", obj.Status);

                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载黑名单异常：UploadParkBlacklist()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }


        /// <summary>
        /// 上传下载订单信息
        /// </summary>
        /// <param name="ListOrder"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkOrder(List<ParkOrder> ListOrder)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in ListOrder)
                    {
                        string sql = "select ID from ParkOrder where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkOrder set OrderNo=@OrderNo,TagID=@TagID,OrderType=@OrderType,PayWay=@PayWay,Amount=@Amount,UnPayAmount=@UnPayAmount,PayAmount=@PayAmount,DiscountAmount=@DiscountAmount,CarderateID=@CarderateID,
                                                Status=@Status ,OrderSource=@OrderSource,OrderTime=@OrderTime,PayTime=@PayTime,OldUserBegin=@OldUserBegin,OldUserulDate=@OldUserulDate,NewUsefulDate=@NewUsefulDate,NewUserBegin=@NewUserBegin,OldMoney=@OldMoney,
                                                 NewMoney=@NewMoney,PKID=@PKID,UserID=@UserID,OnlineUserID=@OnlineUserID,OnlineOrderNo=@OnlineOrderNo,Remark=@Remark,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus,CashTime=@CashTime,CashMoney=@CashMoney,FeeRuleID=@FeeRuleID  
                                                where RecordID=@RecordID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("OrderNo", obj.OrderNo);
                                dbOperator.AddParameter("TagID", obj.TagID);
                                dbOperator.AddParameter("OrderType", obj.OrderType);
                                dbOperator.AddParameter("PayWay", obj.PayWay);
                                dbOperator.AddParameter("Amount", obj.Amount);
                                dbOperator.AddParameter("UnPayAmount", obj.UnPayAmount);
                                dbOperator.AddParameter("PayAmount", obj.PayAmount);
                                dbOperator.AddParameter("DiscountAmount", obj.DiscountAmount);
                                dbOperator.AddParameter("CarderateID", obj.CarderateID);
                                dbOperator.AddParameter("Status", obj.Status);
                                dbOperator.AddParameter("OrderSource", obj.OrderSource);
                                if (obj.OrderTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("OrderTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("OrderTime", obj.OrderTime);
                                }
                                if (obj.PayTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("PayTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("PayTime", obj.PayTime);
                                }
                                if (obj.OldUserulDate == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("OldUserulDate", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("OldUserulDate", obj.OldUserulDate);
                                }
                                if (obj.NewUsefulDate == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("NewUsefulDate", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("NewUsefulDate", obj.NewUsefulDate);
                                }
                               
                              
                                dbOperator.AddParameter("OldMoney", obj.OldMoney);
                                dbOperator.AddParameter("NewMoney", obj.NewMoney);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("UserID", obj.UserID);
                                dbOperator.AddParameter("OnlineUserID", obj.OnlineUserID);
                                dbOperator.AddParameter("OnlineOrderNo", obj.OnlineOrderNo);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("RecordID", obj.RecordID);

                                if (obj.OldUserBegin == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("OldUserBegin", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("OldUserBegin", obj.OldUserBegin);
                                }

                                if (obj.NewUserBegin == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("NewUserBegin", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("NewUserBegin", obj.NewUserBegin);
                                }
                                if (obj.CashTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("CashTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("CashTime", obj.CashTime);
                                }
                                
                                dbOperator.AddParameter("CashMoney", obj.CashMoney);
                                dbOperator.AddParameter("FeeRuleID", obj.FeeRuleID);
                                
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkOrder(RecordID,OrderNo,TagID,OrderType,PayWay,Amount,UnPayAmount,PayAmount,DiscountAmount,
                                                                CarderateID,Status,OrderSource,OrderTime,PayTime,OldUserulDate,NewUsefulDate,OldMoney,NewMoney,PKID,
                                                                UserID,OnlineUserID,OnlineOrderNo,Remark,LastUpdateTime,HaveUpdate,DataStatus,OldUserBegin,NewUserBegin,CashTime,CashMoney,FeeRuleID)
                                                                values(@RecordID,@OrderNo,@TagID,@OrderType,@PayWay,@Amount,@UnPayAmount,@PayAmount,@DiscountAmount,
                                                                @CarderateID,@Status,@OrderSource,@OrderTime,@PayTime,@OldUserulDate,@NewUsefulDate,@OldMoney,@NewMoney,@PKID,
                                                                @UserID,@OnlineUserID,@OnlineOrderNo,@Remark,@LastUpdateTime,@HaveUpdate,@DataStatus,@OldUserBegin,@NewUserBegin,@CashTime,@CashMoney,@FeeRuleID)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("OrderNo", obj.OrderNo);
                                dbOperator.AddParameter("TagID", obj.TagID);
                                dbOperator.AddParameter("OrderType", obj.OrderType);
                                dbOperator.AddParameter("PayWay", obj.PayWay);
                                dbOperator.AddParameter("Amount", obj.Amount);
                                dbOperator.AddParameter("UnPayAmount", obj.UnPayAmount);
                                dbOperator.AddParameter("PayAmount", obj.PayAmount);
                                dbOperator.AddParameter("DiscountAmount", obj.DiscountAmount);
                                dbOperator.AddParameter("CarderateID", obj.CarderateID);
                                dbOperator.AddParameter("Status", obj.Status);
                                dbOperator.AddParameter("OrderSource", obj.OrderSource);
                                if (obj.OrderTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("OrderTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("OrderTime", obj.OrderTime);
                                }
                                if (obj.PayTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("PayTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("PayTime", obj.PayTime);
                                }
                                if (obj.OldUserulDate == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("OldUserulDate", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("OldUserulDate", obj.OldUserulDate);
                                }
                                if (obj.NewUsefulDate == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("NewUsefulDate", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("NewUsefulDate", obj.NewUsefulDate);
                                }


                                dbOperator.AddParameter("OldMoney", obj.OldMoney);
                                dbOperator.AddParameter("NewMoney", obj.NewMoney);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("UserID", obj.UserID);
                                dbOperator.AddParameter("OnlineUserID", obj.OnlineUserID);
                                dbOperator.AddParameter("OnlineOrderNo", obj.OnlineOrderNo);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("RecordID", obj.RecordID);

                                if (obj.OldUserBegin == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("OldUserBegin", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("OldUserBegin", obj.OldUserBegin);
                                }

                                if (obj.NewUserBegin == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("NewUserBegin", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("NewUserBegin", obj.NewUserBegin);
                                }
                                if (obj.CashTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("CashTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("CashTime", obj.CashTime);
                                }

                                dbOperator.AddParameter("CashMoney", obj.CashMoney);
                                dbOperator.AddParameter("FeeRuleID", obj.FeeRuleID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载订单信息异常：UploadParkOrder()" + ex.Message + JsonHelper.GetJsonString(ListOrder) + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载进出纪录
        /// </summary>
        /// <param name="ListIORecord"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkIORecord(List<ParkIORecord> ListIORecord)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in ListIORecord)
                    {
                        string sql = "select RecordID from ParkIORecord where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkIORecord set CardID=@CardID,PlateNumber=@PlateNumber,CardNo=@CardNo,CardNumb=@CardNumb,EntranceTime=@EntranceTime,EntranceImage=@EntranceImage,EntranceGateID=@EntranceGateID,EntranceOperatorID=@EntranceOperatorID,ExitTime=@ExitTime,
                                                ExitImage=@ExitImage ,ExitGateID=@ExitGateID,ExitOperatorID=@ExitOperatorID,CarTypeID=@CarTypeID,CarModelID=@CarModelID,IsExit=@IsExit,AreaID=@AreaID,IsOffline=@IsOffline,OfflineID=@OfflineID, 
                                                 ParkingID=@ParkingID,ReleaseType=@ReleaseType,EnterType=@EnterType,DataStatus=@DataStatus,MHOut=@MHOut,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,Remark=@Remark,IsScanCodeIn=@IsScanCodeIn,IsScanCodeOut=@IsScanCodeOut
                                                ,EntranceCertificateNo=@EntranceCertificateNo,ExitCertificateNo=@ExitCertificateNo,EntranceCertificateImage=@EntranceCertificateImage,ExitcertificateImage=@ExitcertificateImage,PlateColor=@PlateColor,EntranceIDCardPhoto=@ExitIDCardPhoto,ExitIDCardPhoto=@ExitIDCardPhoto,ReserveBitID=@ReserveBitID, 
                                                EntranceCertName=@EntranceCertName,EntranceSex=@EntranceSex,EntranceNation=@EntranceNation,EntranceBirthDate=@EntranceBirthDate,EntranceAddress=@EntranceAddress,ExitCertName=@ExitCertName,ExitSex=@ExitSex,ExitNation=@ExitNation,ExitBirthDate=@ExitBirthDate,ExitAddress=@ExitAddress
                                                ,NetWeight=@NetWeight,Tare=@Tare,Goods=@Goods,Shipper=@Shipper,Shippingspace=@Shippingspace,DocumentsNo=@DocumentsNo 
                                                where RecordID=@RecordID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("CardID", obj.CardID);
                                dbOperator.AddParameter("PlateNumber", obj.PlateNumber);
                                dbOperator.AddParameter("CardNo", obj.CardNo);
                                dbOperator.AddParameter("CardNumb", obj.CardNumb);
                                if (obj.EntranceTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("EntranceTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("EntranceTime", obj.EntranceTime);
                                }
                                dbOperator.AddParameter("EntranceImage", obj.EntranceImage);
                                dbOperator.AddParameter("EntranceGateID", obj.EntranceGateID);
                                dbOperator.AddParameter("EntranceOperatorID", obj.EntranceOperatorID);
                                if (obj.ExitTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("ExitTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("ExitTime", obj.ExitTime);
                                }
                             
                                dbOperator.AddParameter("ExitImage", obj.ExitImage);
                                dbOperator.AddParameter("ExitGateID", obj.ExitGateID);
                                dbOperator.AddParameter("ExitOperatorID", obj.ExitOperatorID);
                                dbOperator.AddParameter("CarTypeID", obj.CarTypeID);
                                dbOperator.AddParameter("CarModelID", obj.CarModelID);
                                dbOperator.AddParameter("IsExit", obj.IsExit);
                                dbOperator.AddParameter("AreaID", obj.AreaID);
                                dbOperator.AddParameter("ParkingID", obj.ParkingID);
                                dbOperator.AddParameter("ReleaseType", obj.ReleaseType);
                                dbOperator.AddParameter("EnterType", obj.EnterType);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("MHOut", obj.MHOut);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                               
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                dbOperator.AddParameter("RecordID", obj.RecordID);

                                dbOperator.AddParameter("EntranceCertificateNo", obj.EntranceCertificateNo);
                                dbOperator.AddParameter("ExitCertificateNo", obj.ExitCertificateNo);
                                dbOperator.AddParameter("EntranceCertificateImage", obj.EntranceCertificateImage);
                                dbOperator.AddParameter("ExitcertificateImage", obj.ExitcertificateImage);
                                dbOperator.AddParameter("PlateColor", obj.PlateColor);
                                dbOperator.AddParameter("IsOffline", obj.IsOffline);
                                dbOperator.AddParameter("OfflineID", obj.OfflineID);
                                dbOperator.AddParameter("EntranceIDCardPhoto", obj.EntranceIDCardPhoto);
                                dbOperator.AddParameter("ExitIDCardPhoto", obj.ExitIDCardPhoto);

                                dbOperator.AddParameter("IsScanCodeIn", obj.IsScanCodeIn);
                                dbOperator.AddParameter("IsScanCodeOut", obj.IsScanCodeOut);
                                dbOperator.AddParameter("ReserveBitID", obj.ReserveBitID);

                                dbOperator.AddParameter("EntranceCertName", obj.EntranceCertName);
                                dbOperator.AddParameter("EntranceSex", obj.EntranceSex);
                                dbOperator.AddParameter("EntranceNation", obj.EntranceNation);
                                dbOperator.AddParameter("EntranceBirthDate", obj.EntranceBirthDate);
                                dbOperator.AddParameter("EntranceAddress", obj.EntranceAddress);
                                dbOperator.AddParameter("ExitCertName", obj.ExitCertName);
                                dbOperator.AddParameter("ExitSex", obj.ExitSex);
                                dbOperator.AddParameter("ExitNation", obj.ExitNation);
                                dbOperator.AddParameter("ExitBirthDate", obj.ExitBirthDate);
                                dbOperator.AddParameter("ExitAddress", obj.ExitAddress);
                             
                                dbOperator.AddParameter("NetWeight", obj.NetWeight);
                                dbOperator.AddParameter("Tare", obj.Tare);
                                dbOperator.AddParameter("Goods", obj.Goods);
                                dbOperator.AddParameter("Shipper", obj.Shipper);
                                dbOperator.AddParameter("Shippingspace", obj.Shippingspace);
                                dbOperator.AddParameter("DocumentsNo", obj.DocumentsNo);
                             
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID,ID=obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkIORecord( RecordID,CardID,PlateNumber,CardNo,CardNumb,EntranceTime,EntranceImage,EntranceGateID,EntranceOperatorID,ExitTime,
                                                ExitImage ,ExitGateID,ExitOperatorID,CarTypeID,CarModelID,IsExit,AreaID,
                                                 ParkingID,ReleaseType,EnterType,DataStatus,MHOut,LastUpdateTime,HaveUpdate,Remark,EntranceCertificateNo,ExitCertificateNo,EntranceCertificateImage,ExitcertificateImage,PlateColor,IsOffline,OfflineID,EntranceIDCardPhoto,ExitIDCardPhoto,IsScanCodeIn,IsScanCodeOut,ReserveBitID,
                                                    EntranceCertName,EntranceSex,EntranceNation,EntranceBirthDate,EntranceAddress,ExitCertName,ExitSex,ExitNation,ExitBirthDate,ExitAddress,
                                                   NetWeight, Tare,Goods,Shipper,Shippingspace,DocumentsNo)
                                                                values(@RecordID,@CardID,@PlateNumber,@CardNo,@CardNumb,@EntranceTime,@EntranceImage,@EntranceGateID,@EntranceOperatorID,@ExitTime,
                                                @ExitImage ,@ExitGateID,@ExitOperatorID,@CarTypeID,@CarModelID,@IsExit,@AreaID,
                                                 @ParkingID,@ReleaseType,@EnterType,@DataStatus,@MHOut,@LastUpdateTime,@HaveUpdate,@Remark,@EntranceCertificateNo,@ExitCertificateNo,@EntranceCertificateImage,@ExitcertificateImage,@PlateColor,@IsOffline,@OfflineID,@EntranceIDCardPhoto,@ExitIDCardPhoto,@IsScanCodeIn,@IsScanCodeOut,@ReserveBitID,
                                                @EntranceCertName,@EntranceSex,@EntranceNation,@EntranceBirthDate,@EntranceAddress,@ExitCertName,@ExitSex,@ExitNation,@ExitBirthDate,@ExitAddress, @NetWeight, @Tare,@Goods,@Shipper,@Shippingspace,@DocumentsNo)");
                                dbOperator.ClearParameters();

                                dbOperator.AddParameter("CardID", obj.CardID);
                                dbOperator.AddParameter("PlateNumber", obj.PlateNumber);
                                dbOperator.AddParameter("CardNo", obj.CardNo);
                                dbOperator.AddParameter("CardNumb", obj.CardNumb);
                                if (obj.EntranceTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("EntranceTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("EntranceTime", obj.EntranceTime);
                                }
                                dbOperator.AddParameter("EntranceImage", obj.EntranceImage);
                                dbOperator.AddParameter("EntranceGateID", obj.EntranceGateID);
                                dbOperator.AddParameter("EntranceOperatorID", obj.EntranceOperatorID);
                                if (obj.ExitTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("ExitTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("ExitTime", obj.ExitTime);
                                }

                                dbOperator.AddParameter("ExitImage", obj.ExitImage);
                                dbOperator.AddParameter("ExitGateID", obj.ExitGateID);
                                dbOperator.AddParameter("ExitOperatorID", obj.ExitOperatorID);
                                dbOperator.AddParameter("CarTypeID", obj.CarTypeID);
                                dbOperator.AddParameter("CarModelID", obj.CarModelID);
                                dbOperator.AddParameter("IsExit", obj.IsExit);
                                dbOperator.AddParameter("AreaID", obj.AreaID);
                                dbOperator.AddParameter("ParkingID", obj.ParkingID);
                                dbOperator.AddParameter("ReleaseType", obj.ReleaseType);
                                dbOperator.AddParameter("EnterType", obj.EnterType);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("MHOut", obj.MHOut);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }

                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                dbOperator.AddParameter("RecordID", obj.RecordID);

                                dbOperator.AddParameter("EntranceCertificateNo", obj.EntranceCertificateNo);
                                dbOperator.AddParameter("ExitCertificateNo", obj.ExitCertificateNo);
                                dbOperator.AddParameter("EntranceCertificateImage", obj.EntranceCertificateImage);
                                dbOperator.AddParameter("ExitcertificateImage", obj.ExitcertificateImage);
                                dbOperator.AddParameter("PlateColor", obj.PlateColor);

                                dbOperator.AddParameter("IsOffline", obj.IsOffline);
                                dbOperator.AddParameter("OfflineID", obj.OfflineID);
                                dbOperator.AddParameter("EntranceIDCardPhoto", obj.EntranceIDCardPhoto);
                                dbOperator.AddParameter("ExitIDCardPhoto", obj.ExitIDCardPhoto);
                                dbOperator.AddParameter("IsScanCodeIn", obj.IsScanCodeIn);
                                dbOperator.AddParameter("IsScanCodeOut", obj.IsScanCodeOut);
                                dbOperator.AddParameter("ReserveBitID", obj.ReserveBitID);
                                dbOperator.AddParameter("EntranceCertName", obj.EntranceCertName);
                                dbOperator.AddParameter("EntranceSex", obj.EntranceSex);
                                dbOperator.AddParameter("EntranceNation", obj.EntranceNation);
                                dbOperator.AddParameter("EntranceBirthDate", obj.EntranceBirthDate);
                                dbOperator.AddParameter("EntranceAddress", obj.EntranceAddress);
                                dbOperator.AddParameter("ExitCertName", obj.ExitCertName);
                                dbOperator.AddParameter("ExitSex", obj.ExitSex);
                                dbOperator.AddParameter("ExitNation", obj.ExitNation);
                                dbOperator.AddParameter("ExitBirthDate", obj.ExitBirthDate);
                                dbOperator.AddParameter("ExitAddress", obj.ExitAddress);

                                dbOperator.AddParameter("NetWeight", obj.NetWeight);
                                dbOperator.AddParameter("Tare", obj.Tare);
                                dbOperator.AddParameter("Goods", obj.Goods);
                                dbOperator.AddParameter("Shipper", obj.Shipper);
                                dbOperator.AddParameter("Shippingspace", obj.Shippingspace);
                                dbOperator.AddParameter("DocumentsNo", obj.DocumentsNo);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载进出纪录异常：UploadParkIORecord()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载进出时序表
        /// </summary>
        /// <param name="listTimeseries"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkTimeseries(List<ParkTimeseries> listTimeseries)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listTimeseries)
                    {
                        string sql = "select RecordID from ParkTimeseries where TimeseriesID=@TimeseriesID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("TimeseriesID", obj.TimeseriesID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkTimeseries set IORecordID=@IORecordID,EnterImage=@EnterImage,ExitImage=@ExitImage,ParkingID=@ParkingID,EnterTime=@EnterTime,
                                                ExitTime=@ExitTime,ExitGateID=@ExitGateID,EnterGateID=@EnterGateID,IsExit=@IsExit,
                                                LastUpdateTime=@LastUpdateTime ,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus,ReleaseType=@ReleaseType where TimeseriesID=@TimeseriesID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("IORecordID", obj.IORecordID);
                                dbOperator.AddParameter("EnterImage", obj.EnterImage);
                                dbOperator.AddParameter("ExitImage", obj.ExitImage);
                                dbOperator.AddParameter("ParkingID", obj.ParkingID);
                                if (obj.EnterTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("EnterTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("EnterTime", obj.EnterTime);
                                }
                                if (obj.ExitTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("ExitTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("ExitTime", obj.ExitTime);
                                }
                               
                                dbOperator.AddParameter("ExitGateID", obj.ExitGateID);
                                dbOperator.AddParameter("EnterGateID", obj.EnterGateID);
                                dbOperator.AddParameter("IsExit", obj.IsExit);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                             
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("ReleaseType", obj.ReleaseType);
                                dbOperator.AddParameter("TimeseriesID", obj.TimeseriesID);
                             
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.TimeseriesID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkTimeseries( TimeseriesID,IORecordID,EnterImage,ExitImage,ParkingID,EnterTime,
                                                ExitTime,ExitGateID,EnterGateID,IsExit,LastUpdateTime ,HaveUpdate,DataStatus,ReleaseType)
                                                                values(@TimeseriesID,@IORecordID,@EnterImage,@ExitImage,@ParkingID,@EnterTime,
                                                @ExitTime,@ExitGateID,@EnterGateID,@IsExit,@LastUpdateTime ,@HaveUpdate,@DataStatus,@ReleaseType)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("IORecordID", obj.IORecordID);
                                dbOperator.AddParameter("EnterImage", obj.EnterImage);
                                dbOperator.AddParameter("ExitImage", obj.ExitImage);
                                dbOperator.AddParameter("ParkingID", obj.ParkingID);
                                if (obj.EnterTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("EnterTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("EnterTime", obj.EnterTime);
                                }
                                if (obj.ExitTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("ExitTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("ExitTime", obj.ExitTime);
                                }

                                dbOperator.AddParameter("ExitGateID", obj.ExitGateID);
                                dbOperator.AddParameter("EnterGateID", obj.EnterGateID);
                                dbOperator.AddParameter("IsExit", obj.IsExit);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }

                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("ReleaseType", obj.ReleaseType);
                                dbOperator.AddParameter("TimeseriesID", obj.TimeseriesID);
                             
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.TimeseriesID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载进出时序表异常：UploadParkTimeseries()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载通道事件
        /// </summary>
        /// <param name="ListEvent"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkEvent(List<ParkEvent> ListEvent)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in ListEvent)
                    {
                        string sql = "select RecordID from ParkEvent where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkEvent set CardNo=@CardNo,CardNum=@CardNum,EmployeeName=@EmployeeName,PlateNumber=@PlateNumber,PlateColor=@PlateColor,CarTypeID=@CarTypeID,CarModelID=@CarModelID,RecTime=@RecTime,GateID=@GateID,
                                                OperatorID=@OperatorID ,PictureName=@PictureName,EventID=@EventID,IOState=@IOState,IORecordID=@IORecordID,ParkingID=@ParkingID,LastUpdateTime=@LastUpdateTime,
                                                 HaveUpdate=@HaveUpdate,DataStatus=@DataStatus,Remark=@Remark,IsOffline=@IsOffline,IsScanCode=@IsScanCode,CertName=@CertName,CertNo=@CertNo,Sex=@Sex,Nation=@Nation,BirthDate=@BirthDate,Address=@Address,CertificateImage=@CertificateImage 
                                                 where RecordID=@RecordID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("CardNo", obj.CardNo);
                                dbOperator.AddParameter("CardNum", obj.CardNum);
                                dbOperator.AddParameter("EmployeeName", obj.EmployeeName);
                                dbOperator.AddParameter("PlateNumber", obj.PlateNumber);
                                dbOperator.AddParameter("PlateColor", obj.PlateColor);
                                dbOperator.AddParameter("CarTypeID", obj.CarTypeID);
                                dbOperator.AddParameter("CarModelID", obj.CarModelID);
                                if (obj.RecTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("RecTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("RecTime", obj.RecTime);
                                }
                                
                                dbOperator.AddParameter("GateID", obj.GateID);
                                dbOperator.AddParameter("OperatorID", obj.OperatorID);
                                dbOperator.AddParameter("PictureName", obj.PictureName);
                                dbOperator.AddParameter("EventID", obj.EventID);
                                dbOperator.AddParameter("IOState", obj.IOState);
                                dbOperator.AddParameter("IORecordID", obj.IORecordID);
                                dbOperator.AddParameter("ParkingID", obj.ParkingID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                              
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                dbOperator.AddParameter("IsOffline", obj.IsOffline);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("IsScanCode", obj.IsScanCode);

                                dbOperator.AddParameter("CertName", obj.CertName);
                                dbOperator.AddParameter("CertNo", obj.CertNo);
                                dbOperator.AddParameter("Sex", obj.Sex);
                                dbOperator.AddParameter("Nation", obj.Nation);
                                dbOperator.AddParameter("BirthDate", obj.BirthDate);
                                dbOperator.AddParameter("Address", obj.Address);
                                dbOperator.AddParameter("CertificateImage", obj.CertificateImage);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID,ID=obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkEvent(RecordID,CardNo,CardNum,EmployeeName,PlateNumber,PlateColor,CarTypeID,CarModelID,RecTime,GateID,
                                                OperatorID ,PictureName,EventID,IOState,IORecordID,ParkingID,LastUpdateTime,
                                                 HaveUpdate,DataStatus,Remark,IsOffline,IsScanCode,CertName,CertNo,Sex,Nation,BirthDate,Address,CertificateImage)
                                                                values(@RecordID,@CardNo,@CardNum,@EmployeeName,@PlateNumber,@PlateColor,@CarTypeID,@CarModelID,@RecTime,@GateID,
                                                @OperatorID ,@PictureName,@EventID,@IOState,@IORecordID,@ParkingID,@LastUpdateTime,
                                                 @HaveUpdate,@DataStatus,@Remark,@IsOffline,@IsScanCode,@CertName,@CertNo,@Sex,@Nation,@BirthDate,@Address,@CertificateImage)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("CardNo", obj.CardNo);
                                dbOperator.AddParameter("CardNum", obj.CardNum);
                                dbOperator.AddParameter("EmployeeName", obj.EmployeeName);
                                dbOperator.AddParameter("PlateNumber", obj.PlateNumber);
                                dbOperator.AddParameter("PlateColor", obj.PlateColor);
                                dbOperator.AddParameter("CarTypeID", obj.CarTypeID);
                                dbOperator.AddParameter("CarModelID", obj.CarModelID);
                                if (obj.RecTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("RecTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("RecTime", obj.RecTime);
                                }

                                dbOperator.AddParameter("GateID", obj.GateID);
                                dbOperator.AddParameter("OperatorID", obj.OperatorID);
                                dbOperator.AddParameter("PictureName", obj.PictureName);
                                dbOperator.AddParameter("EventID", obj.EventID);
                                dbOperator.AddParameter("IOState", obj.IOState);
                                dbOperator.AddParameter("IORecordID", obj.IORecordID);
                                dbOperator.AddParameter("ParkingID", obj.ParkingID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }

                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("Remark", obj.Remark);
                                dbOperator.AddParameter("IsOffline", obj.IsOffline);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("IsScanCode", obj.IsScanCode);
                                dbOperator.AddParameter("CertName", obj.CertName);
                                dbOperator.AddParameter("CertNo", obj.CertNo);
                                dbOperator.AddParameter("Sex", obj.Sex);
                                dbOperator.AddParameter("Nation", obj.Nation);
                                dbOperator.AddParameter("BirthDate", obj.BirthDate);
                                dbOperator.AddParameter("Address", obj.Address);
                                dbOperator.AddParameter("CertificateImage", obj.CertificateImage);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID,ID=obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载通道事件异常：UploadParkEvent()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        
        /// <summary>
        /// 上传下载交接班信息
        /// </summary>
        /// <param name="listChangeshiftrecord"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkChangeshiftrecord(List<ParkChangeshiftrecord> listChangeshiftrecord)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();

            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listChangeshiftrecord)
                    {
                        string sql = "select RecordID from ParkChangeshiftrecord where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update ParkChangeshiftrecord set UserID=@UserID,StartWorkTime=@StartWorkTime,EndWorkTime=@EndWorkTime,
                                                    BoxID=@BoxID,PKID=@PKID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus
                                                where RecordID=@RecordID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("UserID", obj.UserID);
                               
                                if (obj.StartWorkTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("StartWorkTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("StartWorkTime", obj.StartWorkTime);
                                }
                                if (obj.EndWorkTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("EndWorkTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("EndWorkTime", obj.EndWorkTime);
                                }
                              
                                dbOperator.AddParameter("BoxID", obj.BoxID);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkChangeshiftrecord(RecordID,UserID,StartWorkTime,EndWorkTime,
                                                    BoxID,PKID,LastUpdateTime,HaveUpdate,DataStatus)
                                                                values(@RecordID,@UserID,@StartWorkTime,@EndWorkTime,
                                                    @BoxID,@PKID,@LastUpdateTime,@HaveUpdate,@DataStatus)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("UserID", obj.UserID);

                                if (obj.StartWorkTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("StartWorkTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("StartWorkTime", obj.StartWorkTime);
                                }
                                if (obj.EndWorkTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("EndWorkTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("EndWorkTime", obj.EndWorkTime);
                                }

                                dbOperator.AddParameter("BoxID", obj.BoxID);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载交接班信息异常：UploadParkChangeshiftrecord()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载角色
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadSysRoles(List<SysRoles> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from SysRoles where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update SysRoles set RoleName=@RoleName,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,CPID=@CPID,DataStatus=@DataStatus,
                                IsDefaultRole=@IsDefaultRole where RecordID=@RecordID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RoleName", obj.RoleName);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("CPID", obj.CPID);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("IsDefaultRole", obj.IsDefaultRole);
                                dbOperator.AddParameter("RecordID", obj.RecordID);

                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into SysRoles(RecordID,RoleName,LastUpdateTime,HaveUpdate,CPID,DataStatus,IsDefaultRole )
                                                                values(@RecordID,@RoleName,@LastUpdateTime,@HaveUpdate,@CPID,@DataStatus,@IsDefaultRole )");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("RoleName", obj.RoleName);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("CPID", obj.CPID);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("IsDefaultRole", obj.IsDefaultRole);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载角色异常：UploadSysRoles()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载用户信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadSysUser(List<SysUser> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from SysUser where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update SysUser set UserAccount=@UserAccount,UserName=@UserName,Password=@Password,PwdErrorTime=@PwdErrorTime,PwdErrorCount=@PwdErrorCount,
                                LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,CPID=@CPID,DataStatus=@DataStatus,IsDefaultUser=@IsDefaultUser where RecordID=@RecordID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("UserAccount", obj.UserAccount);
                                dbOperator.AddParameter("UserName", obj.UserName);
                                dbOperator.AddParameter("Password", obj.Password);
                                if (obj.PwdErrorTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("PwdErrorTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("PwdErrorTime", obj.PwdErrorTime);
                                }
                                
                               
                                dbOperator.AddParameter("PwdErrorCount", obj.PwdErrorCount);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("CPID", obj.CPID);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("IsDefaultUser", obj.IsDefaultUser);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into SysUser(RecordID,UserAccount,UserName,Password,PwdErrorTime,PwdErrorCount,
                                                             LastUpdateTime,HaveUpdate,CPID,DataStatus,IsDefaultUser )
                                                                values(@RecordID,@UserAccount,@UserName,@Password,@PwdErrorTime,@PwdErrorCount,
                                                                @LastUpdateTime,@HaveUpdate,@CPID,@DataStatus,@IsDefaultUser )");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("UserAccount", obj.UserAccount);
                                dbOperator.AddParameter("UserName", obj.UserName);
                                dbOperator.AddParameter("Password", obj.Password);
                                if (obj.PwdErrorTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("PwdErrorTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("PwdErrorTime", obj.PwdErrorTime);
                                }


                                dbOperator.AddParameter("PwdErrorCount", obj.PwdErrorCount);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("CPID", obj.CPID);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("IsDefaultUser", obj.IsDefaultUser);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                               
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载用户信息异常：UploadSysUser()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载用户对应角色
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadSysUserRolesMapping(List<SysUserRolesMapping> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from SysUserRolesMapping where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update SysUserRolesMapping set UserRecordID=@UserRecordID,RoleID=@RoleID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus where RecordID=@RecordID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("UserRecordID", obj.UserRecordID);
                                dbOperator.AddParameter("RoleID", obj.RoleID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into SysUserRolesMapping(RecordID,UserRecordID,RoleID,LastUpdateTime,HaveUpdate,DataStatus )
                                                                values(@RecordID,@UserRecordID,@RoleID,@LastUpdateTime,@HaveUpdate,@DataStatus )");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("UserRecordID", obj.UserRecordID);
                                dbOperator.AddParameter("RoleID", obj.RoleID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                             

                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载用户对应角色异常：UploadSysUserRolesMapping()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载作用域
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadSysScope(List<SysScope> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from SysScope where ASID=@ASID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ASID", obj.ASID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update SysScope set CPID=@CPID,ASName=@ASName,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus,IsDefaultScope=@IsDefaultScope where ASID=@ASID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("CPID", obj.CPID);
                                dbOperator.AddParameter("ASName", obj.ASName);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("IsDefaultScope", obj.IsDefaultScope);
                                dbOperator.AddParameter("ASID", obj.ASID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.ASID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into SysScope(ASID,CPID,ASName,LastUpdateTime,HaveUpdate,DataStatus,IsDefaultScope )
                                                                values(@ASID,@CPID,@ASName,@LastUpdateTime,@HaveUpdate,@DataStatus,@IsDefaultScope )");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("ASID", obj.ASID);
                                dbOperator.AddParameter("CPID", obj.CPID);
                                dbOperator.AddParameter("ASName", obj.ASName);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("IsDefaultScope", obj.IsDefaultScope);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.ASID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载作用域异常：UploadSysScope()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载作用域对应的区域
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadSysScopeAuthorize(List<SysScopeAuthorize> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from SysScopeAuthorize where ASDID=@ASDID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ASDID", obj.ASDID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update SysScopeAuthorize set ASID=@ASID,TagID=@TagID,ASType=@ASType,CPID=@CPID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus where ASDID=@ASDID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("ASID", obj.ASID);
                                dbOperator.AddParameter("TagID", obj.TagID);
                                dbOperator.AddParameter("ASType", obj.ASType);
                                dbOperator.AddParameter("CPID", obj.CPID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("ASDID", obj.ASDID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.ASDID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into SysScopeAuthorize(ASDID,ASID,TagID,ASType,CPID,LastUpdateTime,HaveUpdate,DataStatus)
                                                                values(@ASDID,@ASID,@TagID,@ASType,@CPID,@LastUpdateTime,@HaveUpdate,@DataStatus)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("ASDID", obj.ASDID);
                                dbOperator.AddParameter("ASID", obj.ASID);
                                dbOperator.AddParameter("TagID", obj.TagID);
                                dbOperator.AddParameter("ASType", obj.ASType);
                                dbOperator.AddParameter("CPID", obj.CPID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.ASDID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载作用域对应的区域异常：UploadSysScopeAuthorize()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载用户对应的作用域
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadSysUserScopeMapping(List<SysUserScopeMapping> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from SysUserScopeMapping where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update SysUserScopeMapping set UserRecordID=@UserRecordID,ASID=@ASID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus where RecordID=@RecordID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("UserRecordID", obj.UserRecordID);
                                dbOperator.AddParameter("ASID", obj.ASID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into SysUserScopeMapping(RecordID,UserRecordID,ASID,LastUpdateTime,HaveUpdate,DataStatus)
                                                                values(@RecordID,@UserRecordID,@ASID,@LastUpdateTime,@HaveUpdate,@DataStatus)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("UserRecordID", obj.UserRecordID);
                                dbOperator.AddParameter("ASID", obj.ASID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                               
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载用户对应的作用域异常：UploadSysUserScopeMapping()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载角色对应的权限
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadSysRoleAuthorize(List<SysRoleAuthorize> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from SysRoleAuthorize where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update SysRoleAuthorize set ModuleID=@ModuleID,RoleID=@RoleID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus,ParentID=@ParentID where RecordID=@RecordID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("ModuleID", obj.ModuleID);
                                dbOperator.AddParameter("RoleID", obj.RoleID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("ParentID", obj.ParentID);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into SysRoleAuthorize(RecordID,ModuleID,RoleID,LastUpdateTime,HaveUpdate,DataStatus,ParentID)
                                                                values(@RecordID,@ModuleID,@RoleID,@LastUpdateTime,@HaveUpdate,@DataStatus,@ParentID)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("ModuleID", obj.ModuleID);
                                dbOperator.AddParameter("RoleID", obj.RoleID);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("ParentID", obj.ParentID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载角色对应的权限异常：UploadSysRoleAuthorize()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载当班统计信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadChangeShift(List<Statistics_ChangeShift> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from Statistics_ChangeShift where ChangeShiftID=@ChangeShiftID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ChangeShiftID", obj.ChangeShiftID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                sql = string.Format(@"update Statistics_ChangeShift set ParkingID=@ParkingID,BoxID=@BoxID,AdminID=@AdminID,StartWorkTime=@StartWorkTime,EndWorkTime=@EndWorkTime
                                ,Receivable_Amount=@Receivable_Amount,Real_Amount=@Real_Amount,Diff_Amount=@Diff_Amount,Cash_Amount=@Cash_Amount,Cash_Count=@Cash_Count,Temp_Amount=@Temp_Amount,
                                Temp_Count=@Temp_Count,OnLineTemp_Amount=@OnLineTemp_Amount,OnLineTemp_Count=@OnLineTemp_Count,StordCard_Amount=@StordCard_Amount,StordCard_Count=@StordCard_Count,
                                OnLine_Amount=@OnLine_Amount,OnLine_Count=@OnLine_Count,Discount_Amount=@Discount_Amount,Discount_Count=@Discount_Count,ReleaseType_Normal=@ReleaseType_Normal,
                                ReleaseType_Charge=@ReleaseType_Charge,ReleaseType_Free=@ReleaseType_Free,ReleaseType_Catch=@ReleaseType_Catch,VIPExtend_Count=@VIPExtend_Count,Entrance_Count=@Entrance_Count,
                                Exit_Count=@Exit_Count,VIPCard=@VIPCard,StordCard=@StordCard,MonthCard=@MonthCard,JobCard=@JobCard,TempCard=@TempCard,OnLineMonthCardExtend_Count=@OnLineMonthCardExtend_Count,
                                OnLineMonthCardExtend_Amount=@OnLineMonthCardExtend_Amount,MonthCardExtend_Count=@MonthCardExtend_Count,MonthCardExtend_Amount=@MonthCardExtend_Amount,OnLineStordCard_Count=@OnLineStordCard_Count,
                                OnLineStordCard_Amount=@OnLineStordCard_Amount,StordCardRecharge_Count=@StordCardRecharge_Count,StordCardRecharge_Amount=@StordCardRecharge_Amount,CashDiscount_Amount=@CashDiscount_Amount,
                                CashDiscount_Count=@CashDiscount_Count,OnLineDiscount_Amount=@OnLineDiscount_Amount,OnLineDiscount_Count=@OnLineDiscount_Count,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime where ChangeShiftID=@ChangeShiftID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("ParkingID", obj.ParkingID);
                                dbOperator.AddParameter("BoxID", obj.BoxID);
                              
                                dbOperator.AddParameter("AdminID", obj.AdminID);
                                if (obj.StartWorkTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("StartWorkTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("StartWorkTime", obj.StartWorkTime);
                                }
                                if (obj.EndWorkTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("EndWorkTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("EndWorkTime", obj.EndWorkTime);
                                }
                               
                                dbOperator.AddParameter("Receivable_Amount", obj.Receivable_Amount);
                                dbOperator.AddParameter("Real_Amount", obj.Real_Amount);
                                dbOperator.AddParameter("Diff_Amount", obj.Diff_Amount);

                                dbOperator.AddParameter("Cash_Amount", obj.Cash_Amount);
                                dbOperator.AddParameter("Cash_Count", obj.Cash_Count);
                                dbOperator.AddParameter("Temp_Amount", obj.Temp_Amount);
                                dbOperator.AddParameter("Temp_Count", obj.Temp_Count);
                                dbOperator.AddParameter("OnLineTemp_Amount", obj.OnLineTemp_Amount);
                                dbOperator.AddParameter("OnLineTemp_Count", obj.OnLineTemp_Count);
                                dbOperator.AddParameter("StordCard_Amount", obj.StordCard_Amount);

                                dbOperator.AddParameter("StordCard_Count", obj.StordCard_Count);
                                dbOperator.AddParameter("OnLine_Amount", obj.OnLine_Amount);
                                dbOperator.AddParameter("OnLine_Count", obj.OnLine_Count);
                                dbOperator.AddParameter("Discount_Amount", obj.Discount_Amount);
                                dbOperator.AddParameter("Discount_Count", obj.Discount_Count);

                                dbOperator.AddParameter("ReleaseType_Normal", obj.ReleaseType_Normal);
                                dbOperator.AddParameter("ReleaseType_Charge", obj.ReleaseType_Charge);
                                dbOperator.AddParameter("ReleaseType_Free", obj.ReleaseType_Free);
                                dbOperator.AddParameter("ReleaseType_Catch", obj.ReleaseType_Catch);
                                dbOperator.AddParameter("VIPExtend_Count", obj.VIPExtend_Count);

                                dbOperator.AddParameter("Entrance_Count", obj.Entrance_Count);
                                dbOperator.AddParameter("Exit_Count", obj.Exit_Count);
                                dbOperator.AddParameter("VIPCard", obj.VIPCard);
                                dbOperator.AddParameter("StordCard", obj.StordCard);
                                dbOperator.AddParameter("MonthCard", obj.MonthCard);

                                dbOperator.AddParameter("JobCard", obj.JobCard);
                                dbOperator.AddParameter("TempCard", obj.TempCard);
                                dbOperator.AddParameter("OnLineMonthCardExtend_Count", obj.OnLineMonthCardExtend_Count);
                                dbOperator.AddParameter("OnLineMonthCardExtend_Amount", obj.OnLineMonthCardExtend_Amount);
                                dbOperator.AddParameter("MonthCardExtend_Count", obj.MonthCardExtend_Count);

                                dbOperator.AddParameter("MonthCardExtend_Amount", obj.MonthCardExtend_Amount);
                                dbOperator.AddParameter("OnLineStordCard_Count", obj.OnLineStordCard_Count);
                                dbOperator.AddParameter("OnLineStordCard_Amount", obj.OnLineStordCard_Amount);
                                dbOperator.AddParameter("StordCardRecharge_Count", obj.StordCardRecharge_Count);

                                dbOperator.AddParameter("StordCardRecharge_Amount", obj.StordCardRecharge_Amount);
                                dbOperator.AddParameter("CashDiscount_Amount", obj.CashDiscount_Amount);
                                dbOperator.AddParameter("CashDiscount_Count", obj.CashDiscount_Count);
                                dbOperator.AddParameter("OnLineDiscount_Amount", obj.OnLineDiscount_Amount);
                                dbOperator.AddParameter("OnLineDiscount_Count", obj.OnLineDiscount_Count);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                              
                                dbOperator.AddParameter("ChangeShiftID", obj.ChangeShiftID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.ChangeShiftID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into Statistics_ChangeShift(ChangeShiftID, ParkingID,BoxID,AdminID,StartWorkTime,EndWorkTime
                                ,Receivable_Amount,Real_Amount,Diff_Amount,Cash_Amount,Cash_Count,Temp_Amount,
                                Temp_Count,OnLineTemp_Amount,OnLineTemp_Count,StordCard_Amount,StordCard_Count,
                                OnLine_Amount,OnLine_Count,Discount_Amount,Discount_Count,ReleaseType_Normal,
                                ReleaseType_Charge,ReleaseType_Free,ReleaseType_Catch,VIPExtend_Count,Entrance_Count,
                                Exit_Count,VIPCard,StordCard,MonthCard,JobCard,TempCard,OnLineMonthCardExtend_Count,
                                OnLineMonthCardExtend_Amount,MonthCardExtend_Count,MonthCardExtend_Amount,OnLineStordCard_Count,
                                OnLineStordCard_Amount,StordCardRecharge_Count,StordCardRecharge_Amount,CashDiscount_Amount,
                                CashDiscount_Count,OnLineDiscount_Amount,OnLineDiscount_Count,HaveUpdate,LastUpdateTime)
                                                                values(@ChangeShiftID, @ParkingID,@BoxID,@AdminID,@StartWorkTime,@EndWorkTime
                                ,@Receivable_Amount,@Real_Amount,@Diff_Amount,@Cash_Amount,@Cash_Count,@Temp_Amount,
                                @Temp_Count,@OnLineTemp_Amount,@OnLineTemp_Count,@StordCard_Amount,@StordCard_Count,
                                @OnLine_Amount,@OnLine_Count,@Discount_Amount,@Discount_Count,@ReleaseType_Normal,
                                @ReleaseType_Charge,@ReleaseType_Free,@ReleaseType_Catch,@VIPExtend_Count,@Entrance_Count,
                                @Exit_Count,@VIPCard,@StordCard,@MonthCard,@JobCard,@TempCard,@OnLineMonthCardExtend_Count,
                                @OnLineMonthCardExtend_Amount,@MonthCardExtend_Count,@MonthCardExtend_Amount,@OnLineStordCard_Count,
                                @OnLineStordCard_Amount,@StordCardRecharge_Count,@StordCardRecharge_Amount,@CashDiscount_Amount,
                                @CashDiscount_Count,@OnLineDiscount_Amount,@OnLineDiscount_Count,@HaveUpdate,@LastUpdateTime)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("ParkingID", obj.ParkingID);
                                dbOperator.AddParameter("BoxID", obj.BoxID);
                                dbOperator.AddParameter("ChangeShiftID", obj.ChangeShiftID);
                                dbOperator.AddParameter("AdminID", obj.AdminID);
                                if (obj.StartWorkTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("StartWorkTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("StartWorkTime", obj.StartWorkTime);
                                }
                                if (obj.EndWorkTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("EndWorkTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("EndWorkTime", obj.EndWorkTime);
                                }

                                dbOperator.AddParameter("Receivable_Amount", obj.Receivable_Amount);
                                dbOperator.AddParameter("Real_Amount", obj.Real_Amount);
                                dbOperator.AddParameter("Diff_Amount", obj.Diff_Amount);

                                dbOperator.AddParameter("Cash_Amount", obj.Cash_Amount);
                                dbOperator.AddParameter("Cash_Count", obj.Cash_Count);
                                dbOperator.AddParameter("Temp_Amount", obj.Temp_Amount);
                                dbOperator.AddParameter("Temp_Count", obj.Temp_Count);
                                dbOperator.AddParameter("OnLineTemp_Amount", obj.OnLineTemp_Amount);
                                dbOperator.AddParameter("OnLineTemp_Count", obj.OnLineTemp_Count);
                                dbOperator.AddParameter("StordCard_Amount", obj.StordCard_Amount);

                                dbOperator.AddParameter("StordCard_Count", obj.StordCard_Count);
                                dbOperator.AddParameter("OnLine_Amount", obj.OnLine_Amount);
                                dbOperator.AddParameter("OnLine_Count", obj.OnLine_Count);
                                dbOperator.AddParameter("Discount_Amount", obj.Discount_Amount);
                                dbOperator.AddParameter("Discount_Count", obj.Discount_Count);

                                dbOperator.AddParameter("ReleaseType_Normal", obj.ReleaseType_Normal);
                                dbOperator.AddParameter("ReleaseType_Charge", obj.ReleaseType_Charge);
                                dbOperator.AddParameter("ReleaseType_Free", obj.ReleaseType_Free);
                                dbOperator.AddParameter("ReleaseType_Catch", obj.ReleaseType_Catch);
                                dbOperator.AddParameter("VIPExtend_Count", obj.VIPExtend_Count);

                                dbOperator.AddParameter("Entrance_Count", obj.Entrance_Count);
                                dbOperator.AddParameter("Exit_Count", obj.Exit_Count);
                                dbOperator.AddParameter("VIPCard", obj.VIPCard);
                                dbOperator.AddParameter("StordCard", obj.StordCard);
                                dbOperator.AddParameter("MonthCard", obj.MonthCard);

                                dbOperator.AddParameter("JobCard", obj.JobCard);
                                dbOperator.AddParameter("TempCard", obj.TempCard);
                                dbOperator.AddParameter("OnLineMonthCardExtend_Count", obj.OnLineMonthCardExtend_Count);
                                dbOperator.AddParameter("OnLineMonthCardExtend_Amount", obj.OnLineMonthCardExtend_Amount);
                                dbOperator.AddParameter("MonthCardExtend_Count", obj.MonthCardExtend_Count);

                                dbOperator.AddParameter("MonthCardExtend_Amount", obj.MonthCardExtend_Amount);
                                dbOperator.AddParameter("OnLineStordCard_Count", obj.OnLineStordCard_Count);
                                dbOperator.AddParameter("OnLineStordCard_Amount", obj.OnLineStordCard_Amount);
                                dbOperator.AddParameter("StordCardRecharge_Count", obj.StordCardRecharge_Count);

                                dbOperator.AddParameter("StordCardRecharge_Amount", obj.StordCardRecharge_Amount);
                                dbOperator.AddParameter("CashDiscount_Amount", obj.CashDiscount_Amount);
                                dbOperator.AddParameter("CashDiscount_Count", obj.CashDiscount_Count);
                                dbOperator.AddParameter("OnLineDiscount_Amount", obj.OnLineDiscount_Amount);
                                dbOperator.AddParameter("OnLineDiscount_Count", obj.OnLineDiscount_Count);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }

                               
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.ChangeShiftID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载当班统计信息异常：UploadChangeShift()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载实时统计
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadGather(List<Statistics_Gather> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID,StatisticsGatherID from Statistics_Gather where ParkingID=@ParkingID and GatherTime=@GatherTime ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ParkingID", obj.ParkingID);
                        dbOperator.AddParameter("GatherTime", obj.GatherTime);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                string StatisticsGatherID= reader["StatisticsGatherID"].ToString();
                                reader.Close();
                                sql = string.Format(@"update Statistics_Gather set ParkingID=@ParkingID,ParkingName=@ParkingName,GatherTime=@GatherTime,Receivable_Amount=@Receivable_Amount,
                                  Real_Amount=@Real_Amount,Diff_Amount=@Diff_Amount,Cash_Amount=@Cash_Amount,Cash_Count=@Cash_Count,Temp_Amount=@Temp_Amount,Temp_Count=@Temp_Count,OnLineTemp_Amount=@OnLineTemp_Amount,
                                 OnLineTemp_Count=@OnLineTemp_Count,StordCard_Amount=@StordCard_Amount,StordCard_Count=@StordCard_Count,OnLine_Amount=@OnLine_Amount,OnLine_Count=@OnLine_Count,Discount_Amount=@Discount_Amount,
                                  Discount_Count=@Discount_Count,ReleaseType_Normal=@ReleaseType_Normal,ReleaseType_Charge=@ReleaseType_Charge,ReleaseType_Free=@ReleaseType_Free,ReleaseType_Catch=@ReleaseType_Catch,
                                  VIPExtend_Count=@VIPExtend_Count,Entrance_Count=@Entrance_Count,Exit_Count=@Exit_Count,VIPCard=@VIPCard,StordCard=@StordCard,MonthCard=@MonthCard,JobCard=@JobCard,TempCard=@TempCard,
                                 OnLineMonthCardExtend_Count=@OnLineMonthCardExtend_Count,OnLineMonthCardExtend_Amount=@OnLineMonthCardExtend_Amount,MonthCardExtend_Count=@MonthCardExtend_Count,MonthCardExtend_Amount=@MonthCardExtend_Amount,
                               OnLineStordCard_Count=@OnLineStordCard_Count,OnLineStordCard_Amount=@OnLineStordCard_Amount,StordCardRecharge_Count=@StordCardRecharge_Count,StordCardRecharge_Amount=@StordCardRecharge_Amount,CashDiscount_Amount=@CashDiscount_Amount,
                               CashDiscount_Count=@CashDiscount_Count,OnLineDiscount_Amount=@OnLineDiscount_Amount,OnLineDiscount_Count=@OnLineDiscount_Count,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime where StatisticsGatherID=@StatisticsGatherID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("ParkingID", obj.ParkingID);
                                dbOperator.AddParameter("ParkingName", obj.ParkingName);

                                if (obj.GatherTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("GatherTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("GatherTime", obj.GatherTime);
                                }
                                dbOperator.AddParameter("Receivable_Amount", obj.Receivable_Amount);
                                dbOperator.AddParameter("Real_Amount", obj.Real_Amount);
                                dbOperator.AddParameter("Diff_Amount", obj.Diff_Amount);
                                dbOperator.AddParameter("Cash_Amount", obj.Cash_Amount);
                                dbOperator.AddParameter("Cash_Count", obj.Cash_Count);
                                dbOperator.AddParameter("Temp_Amount", obj.Temp_Amount);

                                dbOperator.AddParameter("Temp_Count", obj.Temp_Count);
                                dbOperator.AddParameter("OnLineTemp_Amount", obj.OnLineTemp_Amount);
                                dbOperator.AddParameter("OnLineTemp_Count", obj.OnLineTemp_Count);
                                dbOperator.AddParameter("StordCard_Amount", obj.StordCard_Amount);
                                dbOperator.AddParameter("StordCard_Count", obj.StordCard_Count);
                                dbOperator.AddParameter("OnLine_Amount", obj.OnLine_Amount);
                                dbOperator.AddParameter("OnLine_Count", obj.OnLine_Count);

                                dbOperator.AddParameter("Discount_Amount", obj.Discount_Amount);
                                dbOperator.AddParameter("Discount_Count", obj.Discount_Count);
                                dbOperator.AddParameter("ReleaseType_Normal", obj.ReleaseType_Normal);
                                dbOperator.AddParameter("ReleaseType_Charge", obj.ReleaseType_Charge);
                                dbOperator.AddParameter("ReleaseType_Free", obj.ReleaseType_Free);

                                dbOperator.AddParameter("ReleaseType_Catch", obj.ReleaseType_Catch);
                                dbOperator.AddParameter("VIPExtend_Count", obj.VIPExtend_Count);
                                dbOperator.AddParameter("Entrance_Count", obj.Entrance_Count);
                                dbOperator.AddParameter("Exit_Count", obj.Exit_Count);
                                dbOperator.AddParameter("VIPCard", obj.VIPCard);

                                dbOperator.AddParameter("StordCard", obj.StordCard);
                                dbOperator.AddParameter("MonthCard", obj.MonthCard);
                                dbOperator.AddParameter("JobCard", obj.JobCard);
                                dbOperator.AddParameter("TempCard", obj.TempCard);
                                dbOperator.AddParameter("OnLineMonthCardExtend_Count", obj.OnLineMonthCardExtend_Count);

                                dbOperator.AddParameter("OnLineMonthCardExtend_Amount", obj.OnLineMonthCardExtend_Amount);
                                dbOperator.AddParameter("MonthCardExtend_Count", obj.MonthCardExtend_Count);
                                dbOperator.AddParameter("MonthCardExtend_Amount", obj.MonthCardExtend_Amount);
                                dbOperator.AddParameter("OnLineStordCard_Count", obj.OnLineStordCard_Count);
                                dbOperator.AddParameter("OnLineStordCard_Amount", obj.OnLineStordCard_Amount);

                                dbOperator.AddParameter("StordCardRecharge_Count", obj.StordCardRecharge_Count);
                                dbOperator.AddParameter("StordCardRecharge_Amount", obj.StordCardRecharge_Amount);
                                dbOperator.AddParameter("CashDiscount_Amount", obj.CashDiscount_Amount);
                                dbOperator.AddParameter("CashDiscount_Count", obj.CashDiscount_Count);

                                dbOperator.AddParameter("OnLineDiscount_Amount", obj.OnLineDiscount_Amount);
                                dbOperator.AddParameter("OnLineDiscount_Count", obj.OnLineDiscount_Count);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("StatisticsGatherID", StatisticsGatherID);
                                
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                              
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.StatisticsGatherID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into Statistics_Gather(StatisticsGatherID,ParkingID,ParkingName,GatherTime,Receivable_Amount,
                                  Real_Amount,Diff_Amount,Cash_Amount,Cash_Count,Temp_Amount,Temp_Count,OnLineTemp_Amount,
                                 OnLineTemp_Count,StordCard_Amount,StordCard_Count,OnLine_Amount,OnLine_Count,Discount_Amount,
                                  Discount_Count,ReleaseType_Normal,ReleaseType_Charge,ReleaseType_Free,ReleaseType_Catch,
                                  VIPExtend_Count,Entrance_Count,Exit_Count,VIPCard,StordCard,MonthCard,JobCard,TempCard,
                                 OnLineMonthCardExtend_Count,OnLineMonthCardExtend_Amount,MonthCardExtend_Count,MonthCardExtend_Amount,
                               OnLineStordCard_Count,OnLineStordCard_Amount,StordCardRecharge_Count,StordCardRecharge_Amount,CashDiscount_Amount,
                               CashDiscount_Count,OnLineDiscount_Amount,OnLineDiscount_Count,HaveUpdate,LastUpdateTime)
                                                                values(@StatisticsGatherID,@ParkingID,@ParkingName,@GatherTime,@Receivable_Amount,
                                  @Real_Amount,@Diff_Amount,@Cash_Amount,@Cash_Count,@Temp_Amount,@Temp_Count,@OnLineTemp_Amount,
                                 @OnLineTemp_Count,@StordCard_Amount,@StordCard_Count,@OnLine_Amount,@OnLine_Count,@Discount_Amount,
                                  @Discount_Count,@ReleaseType_Normal,@ReleaseType_Charge,@ReleaseType_Free,@ReleaseType_Catch,
                                  @VIPExtend_Count,@Entrance_Count,@Exit_Count,@VIPCard,@StordCard,@MonthCard,@JobCard,@TempCard,
                                 @OnLineMonthCardExtend_Count,@OnLineMonthCardExtend_Amount,@MonthCardExtend_Count,@MonthCardExtend_Amount,
                               @OnLineStordCard_Count,@OnLineStordCard_Amount,@StordCardRecharge_Count,@StordCardRecharge_Amount,@CashDiscount_Amount,
                               @CashDiscount_Count,@OnLineDiscount_Amount,@OnLineDiscount_Count,@HaveUpdate,@LastUpdateTime)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("ParkingID", obj.ParkingID);
                                dbOperator.AddParameter("ParkingName", obj.ParkingName);
                                if (obj.GatherTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("GatherTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("GatherTime", obj.GatherTime);
                                }
                                dbOperator.AddParameter("Receivable_Amount", obj.Receivable_Amount);
                                dbOperator.AddParameter("Real_Amount", obj.Real_Amount);
                                dbOperator.AddParameter("Diff_Amount", obj.Diff_Amount);
                                dbOperator.AddParameter("Cash_Amount", obj.Cash_Amount);
                                dbOperator.AddParameter("Cash_Count", obj.Cash_Count);
                                dbOperator.AddParameter("Temp_Amount", obj.Temp_Amount);

                                dbOperator.AddParameter("Temp_Count", obj.Temp_Count);
                                dbOperator.AddParameter("OnLineTemp_Amount", obj.OnLineTemp_Amount);
                                dbOperator.AddParameter("OnLineTemp_Count", obj.OnLineTemp_Count);
                                dbOperator.AddParameter("StordCard_Amount", obj.StordCard_Amount);
                                dbOperator.AddParameter("StordCard_Count", obj.StordCard_Count);
                                dbOperator.AddParameter("OnLine_Amount", obj.OnLine_Amount);
                                dbOperator.AddParameter("OnLine_Count", obj.OnLine_Count);

                                dbOperator.AddParameter("Discount_Amount", obj.Discount_Amount);
                                dbOperator.AddParameter("Discount_Count", obj.Discount_Count);
                                dbOperator.AddParameter("ReleaseType_Normal", obj.ReleaseType_Normal);
                                dbOperator.AddParameter("ReleaseType_Charge", obj.ReleaseType_Charge);
                                dbOperator.AddParameter("ReleaseType_Free", obj.ReleaseType_Free);

                                dbOperator.AddParameter("ReleaseType_Catch", obj.ReleaseType_Catch);
                                dbOperator.AddParameter("VIPExtend_Count", obj.VIPExtend_Count);
                                dbOperator.AddParameter("Entrance_Count", obj.Entrance_Count);
                                dbOperator.AddParameter("Exit_Count", obj.Exit_Count);
                                dbOperator.AddParameter("VIPCard", obj.VIPCard);

                                dbOperator.AddParameter("StordCard", obj.StordCard);
                                dbOperator.AddParameter("MonthCard", obj.MonthCard);
                                dbOperator.AddParameter("JobCard", obj.JobCard);
                                dbOperator.AddParameter("TempCard", obj.TempCard);
                                dbOperator.AddParameter("OnLineMonthCardExtend_Count", obj.OnLineMonthCardExtend_Count);

                                dbOperator.AddParameter("OnLineMonthCardExtend_Amount", obj.OnLineMonthCardExtend_Amount);
                                dbOperator.AddParameter("MonthCardExtend_Count", obj.MonthCardExtend_Count);
                                dbOperator.AddParameter("MonthCardExtend_Amount", obj.MonthCardExtend_Amount);
                                dbOperator.AddParameter("OnLineStordCard_Count", obj.OnLineStordCard_Count);
                                dbOperator.AddParameter("OnLineStordCard_Amount", obj.OnLineStordCard_Amount);

                                dbOperator.AddParameter("StordCardRecharge_Count", obj.StordCardRecharge_Count);
                                dbOperator.AddParameter("StordCardRecharge_Amount", obj.StordCardRecharge_Amount);
                                dbOperator.AddParameter("CashDiscount_Amount", obj.CashDiscount_Amount);
                                dbOperator.AddParameter("CashDiscount_Count", obj.CashDiscount_Count);

                                dbOperator.AddParameter("OnLineDiscount_Amount", obj.OnLineDiscount_Amount);
                                dbOperator.AddParameter("OnLineDiscount_Count", obj.OnLineDiscount_Count);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("StatisticsGatherID", obj.StatisticsGatherID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.StatisticsGatherID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载实时统计异常：UploadGather()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载统计时长
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadGatherLongTime(List<Statistics_GatherLongTime> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID,RecordID from Statistics_GatherLongTime where ParkingID=@ParkingID and GatherTime=@GatherTime ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("ParkingID", obj.ParkingID);
                        dbOperator.AddParameter("GatherTime", obj.GatherTime);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                string RecordID = reader["RecordID"].ToString();
                                reader.Close();
                                sql = string.Format(@"update Statistics_GatherLongTime set ParkingID=@ParkingID,GatherTime=@GatherTime,LTime=@LTime,Times=@Times,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime  where RecordID=@RecordID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("ParkingID", obj.ParkingID);
                                if (obj.GatherTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("GatherTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("GatherTime", obj.GatherTime);
                                }
                               
                                dbOperator.AddParameter("LTime", obj.LTime);
                                dbOperator.AddParameter("Times", obj.Times);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.GatherTime);
                                }

                                dbOperator.AddParameter("RecordID", RecordID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into Statistics_GatherLongTime(RecordID,ParkingID,GatherTime,LTime,Times,HaveUpdate,LastUpdateTime)
                                                                values(@RecordID,@ParkingID,@GatherTime,@LTime,@Times,@HaveUpdate,@LastUpdateTime)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("ParkingID", obj.ParkingID);
                                if (obj.GatherTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("GatherTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("GatherTime", obj.GatherTime);
                                }
                                dbOperator.AddParameter("LTime", obj.LTime);
                                dbOperator.AddParameter("Times", obj.Times);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.GatherTime);
                                }
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载统计时长异常：UploadGatherLongTime()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载通道每小时统计数据
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadGatherGate(List<Statistics_GatherGate> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID,StatisticsGatherID from Statistics_GatherGate where GateID=@GateID and GatherTime=@GatherTime ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("GateID", obj.GateID);
                        dbOperator.AddParameter("GatherTime", obj.GatherTime);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                string StatisticsGatherID = reader["StatisticsGatherID"].ToString();
                                reader.Close();
                                sql = string.Format(@"update Statistics_GatherGate set ParkingID=@ParkingID,ParkingName=@ParkingName,BoxID=@BoxID,BoxName=@BoxName,
                               GateID=@GateID,GateName=@GateName,GatherTime=@GatherTime,Receivable_Amount=@Receivable_Amount,Real_Amount=@Real_Amount,Diff_Amount=@Diff_Amount,
                               Cash_Amount=@Cash_Amount,Cash_Count=@Cash_Count,Temp_Amount=@Temp_Amount,Temp_Count=@Temp_Count,OnLineTemp_Amount=@OnLineTemp_Amount, 
                                OnLineTemp_Count=@OnLineTemp_Count,StordCard_Amount=@StordCard_Amount,StordCard_Count=@StordCard_Count,OnLine_Amount=@OnLine_Amount,
                               OnLine_Count=@OnLine_Count,Discount_Amount=@Discount_Amount,Discount_Count=@Discount_Count,ReleaseType_Normal=@ReleaseType_Normal,
                               ReleaseType_Charge=@ReleaseType_Charge,ReleaseType_Free=@ReleaseType_Free,ReleaseType_Catch=@ReleaseType_Catch,VIPExtend_Count=@VIPExtend_Count,
                               Entrance_Count=@Entrance_Count,Exit_Count=@Exit_Count,VIPCard=@VIPCard,StordCard=@StordCard,MonthCard=@MonthCard,JobCard=@JobCard,TempCard=@TempCard,
                               OnLineMonthCardExtend_Count=@OnLineMonthCardExtend_Count,OnLineMonthCardExtend_Amount=@OnLineMonthCardExtend_Amount,MonthCardExtend_Count=@MonthCardExtend_Count,
                               MonthCardExtend_Amount=@MonthCardExtend_Amount,OnLineStordCard_Count=@OnLineStordCard_Count,OnLineStordCard_Amount=@OnLineStordCard_Amount,StordCardRecharge_Count=@StordCardRecharge_Count,
                               StordCardRecharge_Amount=@StordCardRecharge_Amount,CashDiscount_Amount=@CashDiscount_Amount,CashDiscount_Count=@CashDiscount_Count,OnLineDiscount_Amount=@OnLineDiscount_Amount,
                              OnLineDiscount_Count=@OnLineDiscount_Count, HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime where StatisticsGatherID=@StatisticsGatherID ");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("ParkingID", obj.ParkingID);
                                dbOperator.AddParameter("ParkingName", obj.ParkingName);
                                dbOperator.AddParameter("BoxID", obj.BoxID);
                                dbOperator.AddParameter("BoxName", obj.BoxName);
                                dbOperator.AddParameter("GateID", obj.GateID);
                                dbOperator.AddParameter("GateName", obj.GateName);
                                if (obj.GatherTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("GatherTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("GatherTime", obj.GatherTime);
                                }
                                dbOperator.AddParameter("Receivable_Amount", obj.Receivable_Amount);
                                dbOperator.AddParameter("Real_Amount", obj.Real_Amount);
                                dbOperator.AddParameter("Diff_Amount", obj.Diff_Amount);
                                dbOperator.AddParameter("Cash_Amount", obj.Cash_Amount);
                                dbOperator.AddParameter("Cash_Count", obj.Cash_Count);
                                dbOperator.AddParameter("Temp_Amount", obj.Temp_Amount);
                                dbOperator.AddParameter("Temp_Count", obj.Temp_Count);
                                dbOperator.AddParameter("OnLineTemp_Amount", obj.OnLineTemp_Amount);
                                dbOperator.AddParameter("OnLineTemp_Count", obj.OnLineTemp_Count);
                                dbOperator.AddParameter("StordCard_Amount", obj.StordCard_Amount);
                                dbOperator.AddParameter("StordCard_Count", obj.StordCard_Count);
                                dbOperator.AddParameter("OnLine_Amount", obj.OnLine_Amount);
                                dbOperator.AddParameter("OnLine_Count", obj.OnLine_Count);
                                dbOperator.AddParameter("Discount_Amount", obj.Discount_Amount);
                                dbOperator.AddParameter("Discount_Count", obj.Discount_Count);
                                dbOperator.AddParameter("ReleaseType_Normal", obj.ReleaseType_Normal);
                                dbOperator.AddParameter("ReleaseType_Charge", obj.ReleaseType_Charge);
                                dbOperator.AddParameter("ReleaseType_Free", obj.ReleaseType_Free);
                                dbOperator.AddParameter("ReleaseType_Catch", obj.ReleaseType_Catch);
                                dbOperator.AddParameter("VIPExtend_Count", obj.VIPExtend_Count);
                                dbOperator.AddParameter("Entrance_Count", obj.Entrance_Count);
                                dbOperator.AddParameter("Exit_Count", obj.Exit_Count);
                                dbOperator.AddParameter("VIPCard", obj.VIPCard);
                                dbOperator.AddParameter("StordCard", obj.StordCard);
                                dbOperator.AddParameter("MonthCard", obj.MonthCard);
                                dbOperator.AddParameter("JobCard", obj.JobCard);
                                dbOperator.AddParameter("TempCard", obj.TempCard);
                                dbOperator.AddParameter("OnLineMonthCardExtend_Count", obj.OnLineMonthCardExtend_Count);
                                dbOperator.AddParameter("OnLineMonthCardExtend_Amount", obj.OnLineMonthCardExtend_Amount);
                                dbOperator.AddParameter("MonthCardExtend_Count", obj.MonthCardExtend_Count);
                                dbOperator.AddParameter("MonthCardExtend_Amount", obj.MonthCardExtend_Amount);
                                dbOperator.AddParameter("OnLineStordCard_Count", obj.OnLineStordCard_Count);
                                dbOperator.AddParameter("OnLineStordCard_Amount", obj.OnLineStordCard_Amount);
                                dbOperator.AddParameter("StordCardRecharge_Count", obj.StordCardRecharge_Count);
                                dbOperator.AddParameter("StordCardRecharge_Amount", obj.StordCardRecharge_Amount);
                                dbOperator.AddParameter("CashDiscount_Amount", obj.CashDiscount_Amount);
                                dbOperator.AddParameter("CashDiscount_Count", obj.CashDiscount_Count);

                                dbOperator.AddParameter("OnLineDiscount_Amount", obj.OnLineDiscount_Amount);
                                dbOperator.AddParameter("OnLineDiscount_Count", obj.OnLineDiscount_Count);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }

                                dbOperator.AddParameter("StatisticsGatherID", StatisticsGatherID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.StatisticsGatherID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into Statistics_GatherGate( StatisticsGatherID,ParkingID,ParkingName,BoxID,BoxName,
                               GateID,GateName,GatherTime,Receivable_Amount,Real_Amount,Diff_Amount,
                               Cash_Amount,Cash_Count,Temp_Amount,Temp_Count,OnLineTemp_Amount, 
                                OnLineTemp_Count,StordCard_Amount,StordCard_Count,OnLine_Amount,
                               OnLine_Count,Discount_Amount,Discount_Count,ReleaseType_Normal,
                               ReleaseType_Charge,ReleaseType_Free,ReleaseType_Catch,VIPExtend_Count,
                               Entrance_Count,Exit_Count,VIPCard,StordCard,MonthCard,JobCard,TempCard,
                               OnLineMonthCardExtend_Count,OnLineMonthCardExtend_Amount,MonthCardExtend_Count,
                               MonthCardExtend_Amount,OnLineStordCard_Count,OnLineStordCard_Amount,StordCardRecharge_Count,
                               StordCardRecharge_Amount,CashDiscount_Amount,CashDiscount_Count,OnLineDiscount_Amount,
                              OnLineDiscount_Count, HaveUpdate,LastUpdateTime)
                                                                values(@StatisticsGatherID,@ParkingID,@ParkingName,@BoxID,@BoxName,
                               @GateID,@GateName,@GatherTime,@Receivable_Amount,@Real_Amount,@Diff_Amount,
                               @Cash_Amount,@Cash_Count,@Temp_Amount,@Temp_Count,@OnLineTemp_Amount, 
                                @OnLineTemp_Count,@StordCard_Amount,@StordCard_Count,@OnLine_Amount,
                               @OnLine_Count,@Discount_Amount,@Discount_Count,@ReleaseType_Normal,
                               @ReleaseType_Charge,@ReleaseType_Free,@ReleaseType_Catch,@VIPExtend_Count,
                               @Entrance_Count,@Exit_Count,@VIPCard,@StordCard,@MonthCard,@JobCard,@TempCard,
                               @OnLineMonthCardExtend_Count,@OnLineMonthCardExtend_Amount,@MonthCardExtend_Count,
                               @MonthCardExtend_Amount,@OnLineStordCard_Count,@OnLineStordCard_Amount,@StordCardRecharge_Count,
                               @StordCardRecharge_Amount,@CashDiscount_Amount,@CashDiscount_Count,@OnLineDiscount_Amount,
                              @OnLineDiscount_Count, @HaveUpdate,@LastUpdateTime)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("ParkingID", obj.ParkingID);
                                dbOperator.AddParameter("ParkingName", obj.ParkingName);
                                dbOperator.AddParameter("BoxID", obj.BoxID);
                                dbOperator.AddParameter("BoxName", obj.BoxName);
                                dbOperator.AddParameter("GateID", obj.GateID);
                                dbOperator.AddParameter("GateName", obj.GateName);
                                if (obj.GatherTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("GatherTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("GatherTime", obj.GatherTime);
                                }

                                dbOperator.AddParameter("Receivable_Amount", obj.Receivable_Amount);
                                dbOperator.AddParameter("Real_Amount", obj.Real_Amount);
                                dbOperator.AddParameter("Diff_Amount", obj.Diff_Amount);
                                dbOperator.AddParameter("Cash_Amount", obj.Cash_Amount);
                                dbOperator.AddParameter("Cash_Count", obj.Cash_Count);
                                dbOperator.AddParameter("Temp_Amount", obj.Temp_Amount);
                                dbOperator.AddParameter("Temp_Count", obj.Temp_Count);
                                dbOperator.AddParameter("OnLineTemp_Amount", obj.OnLineTemp_Amount);
                                dbOperator.AddParameter("OnLineTemp_Count", obj.OnLineTemp_Count);
                                dbOperator.AddParameter("StordCard_Amount", obj.StordCard_Amount);
                                dbOperator.AddParameter("StordCard_Count", obj.StordCard_Count);
                                dbOperator.AddParameter("OnLine_Amount", obj.OnLine_Amount);
                                dbOperator.AddParameter("OnLine_Count", obj.OnLine_Count);
                                dbOperator.AddParameter("Discount_Amount", obj.Discount_Amount);
                                dbOperator.AddParameter("Discount_Count", obj.Discount_Count);
                                dbOperator.AddParameter("ReleaseType_Normal", obj.ReleaseType_Normal);
                                dbOperator.AddParameter("ReleaseType_Charge", obj.ReleaseType_Charge);
                                dbOperator.AddParameter("ReleaseType_Free", obj.ReleaseType_Free);
                                dbOperator.AddParameter("ReleaseType_Catch", obj.ReleaseType_Catch);
                                dbOperator.AddParameter("VIPExtend_Count", obj.VIPExtend_Count);
                                dbOperator.AddParameter("Entrance_Count", obj.Entrance_Count);
                                dbOperator.AddParameter("Exit_Count", obj.Exit_Count);
                                dbOperator.AddParameter("VIPCard", obj.VIPCard);
                                dbOperator.AddParameter("StordCard", obj.StordCard);
                                dbOperator.AddParameter("MonthCard", obj.MonthCard);
                                dbOperator.AddParameter("JobCard", obj.JobCard);
                                dbOperator.AddParameter("TempCard", obj.TempCard);
                                dbOperator.AddParameter("OnLineMonthCardExtend_Count", obj.OnLineMonthCardExtend_Count);
                                dbOperator.AddParameter("OnLineMonthCardExtend_Amount", obj.OnLineMonthCardExtend_Amount);
                                dbOperator.AddParameter("MonthCardExtend_Count", obj.MonthCardExtend_Count);
                                dbOperator.AddParameter("MonthCardExtend_Amount", obj.MonthCardExtend_Amount);
                                dbOperator.AddParameter("OnLineStordCard_Count", obj.OnLineStordCard_Count);
                                dbOperator.AddParameter("OnLineStordCard_Amount", obj.OnLineStordCard_Amount);
                                dbOperator.AddParameter("StordCardRecharge_Count", obj.StordCardRecharge_Count);
                                dbOperator.AddParameter("StordCardRecharge_Amount", obj.StordCardRecharge_Amount);
                                dbOperator.AddParameter("CashDiscount_Amount", obj.CashDiscount_Amount);
                                dbOperator.AddParameter("CashDiscount_Count", obj.CashDiscount_Count);

                                dbOperator.AddParameter("OnLineDiscount_Amount", obj.OnLineDiscount_Amount);
                                dbOperator.AddParameter("OnLineDiscount_Count", obj.OnLineDiscount_Count);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                if (obj.LastUpdateTime == DateTime.MinValue)
                                {
                                    dbOperator.AddParameter("LastUpdateTime", DBNull.Value);
                                }
                                else
                                {
                                    dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                }
                                dbOperator.AddParameter("StatisticsGatherID", obj.StatisticsGatherID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.StatisticsGatherID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载通道每小时统计数据异常：UploadGatherGate()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载来访信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadVisitorInfo(List<VisitorInfo> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from VisitorInfo where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                            
                                reader.Close();
                                sql = string.Format(@"update VisitorInfo set PlateNumber=@PlateNumber,VisitorMobilePhone=@VisitorMobilePhone,VisitorCount=@VisitorCount,BeginDate=@BeginDate
                                                    ,EndDate=@EndDate,AccountID=@AccountID,CreateTime=@CreateTime,VisitorState=@VisitorState,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,
                                                    DataStatus=@DataStatus,VID=@VID,LookSate=@LookSate,IsExamine=@IsExamine,VisitorName=@VisitorName,VisitorSex=@VisitorSex,Birthday=@Birthday,
                                                    CertifType=@CertifType,CertifNo=@CertifNo,CertifAddr=@CertifAddr,EmployeeID=@EmployeeID,CardNo=@CardNo,OperatorID=@OperatorID,VisitorCompany=@VisitorCompany,
                                                    VistorPhoto=@VistorPhoto,CertifPhoto=@CertifPhoto where RecordID=@RecordID");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("PlateNumber", obj.PlateNumber);
                                dbOperator.AddParameter("VisitorMobilePhone", obj.VisitorMobilePhone);
                                dbOperator.AddParameter("VisitorCount", obj.VisitorCount);
                                dbOperator.AddParameter("BeginDate", obj.BeginDate);
                                dbOperator.AddParameter("EndDate", obj.EndDate);
                                dbOperator.AddParameter("AccountID", obj.AccountID);
                                dbOperator.AddParameter("CreateTime", obj.CreateTime);
                                dbOperator.AddParameter("VisitorState", obj.VisitorState);
                                dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("VID", obj.VID);
                                dbOperator.AddParameter("LookSate", obj.LookSate);
                                dbOperator.AddParameter("IsExamine", obj.IsExamine);
                                dbOperator.AddParameter("VisitorName", obj.VisitorName);
                                dbOperator.AddParameter("VisitorSex", obj.VisitorSex);
                                dbOperator.AddParameter("Birthday", obj.Birthday);
                                dbOperator.AddParameter("CertifType", obj.CertifType);
                                dbOperator.AddParameter("CertifNo", obj.CertifNo);
                                dbOperator.AddParameter("CertifAddr", obj.CertifAddr);
                                dbOperator.AddParameter("EmployeeID", obj.EmployeeID);
                                dbOperator.AddParameter("CardNo", obj.CardNo);
                                dbOperator.AddParameter("OperatorID", obj.OperatorID);
                                dbOperator.AddParameter("VisitorCompany", obj.VisitorCompany);
                                dbOperator.AddParameter("VistorPhoto", obj.VistorPhoto);
                                dbOperator.AddParameter("CertifPhoto", obj.CertifPhoto);
                               
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into VisitorInfo(RecordID, PlateNumber,VisitorMobilePhone,VisitorCount,BeginDate
                                                    ,EndDate,AccountID,CreateTime,VisitorState,LastUpdateTime,HaveUpdate,
                                                    DataStatus,VID,LookSate,IsExamine,VisitorName,VisitorSex,Birthday,
                                                    CertifType,CertifNo,CertifAddr,EmployeeID,CardNo,OperatorID,VisitorCompany,
                                                    VistorPhoto,CertifPhoto)
                                                                values(@RecordID, @PlateNumber,@VisitorMobilePhone,@VisitorCount,@BeginDate
                                                    ,@EndDate,@AccountID,@CreateTime,@VisitorState,@LastUpdateTime,@HaveUpdate,
                                                    @DataStatus,@VID,@LookSate,@IsExamine,@VisitorName,@VisitorSex,@Birthday,
                                                    @CertifType,@CertifNo,@CertifAddr,@EmployeeID,@CardNo,@OperatorID,@VisitorCompany,
                                                    @VistorPhoto,@CertifPhoto)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("PlateNumber", obj.PlateNumber);
                                dbOperator.AddParameter("VisitorMobilePhone", obj.VisitorMobilePhone);
                                dbOperator.AddParameter("VisitorCount", obj.VisitorCount);
                                dbOperator.AddParameter("BeginDate", obj.BeginDate);
                                dbOperator.AddParameter("EndDate", obj.EndDate);
                                dbOperator.AddParameter("AccountID", obj.AccountID);
                                dbOperator.AddParameter("CreateTime", obj.CreateTime);
                                dbOperator.AddParameter("VisitorState", obj.VisitorState);
                                dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("VID", obj.VID);
                                dbOperator.AddParameter("LookSate", obj.LookSate);
                                dbOperator.AddParameter("IsExamine", obj.IsExamine);
                                dbOperator.AddParameter("VisitorName", obj.VisitorName);
                                dbOperator.AddParameter("VisitorSex", obj.VisitorSex);
                                dbOperator.AddParameter("Birthday", obj.Birthday);
                                dbOperator.AddParameter("CertifType", obj.CertifType);
                                dbOperator.AddParameter("CertifNo", obj.CertifNo);
                                dbOperator.AddParameter("CertifAddr", obj.CertifAddr);
                                dbOperator.AddParameter("EmployeeID", obj.EmployeeID);
                                dbOperator.AddParameter("CardNo", obj.CardNo);
                                dbOperator.AddParameter("OperatorID", obj.OperatorID);
                                dbOperator.AddParameter("VisitorCompany", obj.VisitorCompany);
                                dbOperator.AddParameter("VistorPhoto", obj.VistorPhoto);
                                dbOperator.AddParameter("CertifPhoto", obj.CertifPhoto);
                               
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载来访信息异常：UploadVisitorInfo()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载访客车辆信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkVisitor(List<ParkVisitor> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkVisitor where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                sql = string.Format(@"update ParkVisitor set VisitorID=@VisitorID,PKID=@PKID,VID=@VID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus,AlreadyVisitorCount=@AlreadyVisitorCount where RecordID=@RecordID");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("VisitorID", obj.VisitorID);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("VID", obj.VID);
                                dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("AlreadyVisitorCount", obj.AlreadyVisitorCount);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkVisitor(VisitorID,PKID,VID,LastUpdateTime,HaveUpdate,DataStatus,AlreadyVisitorCount,RecordID)
                                                                values(@VisitorID,@PKID,@VID,@LastUpdateTime,@HaveUpdate,@DataStatus,@AlreadyVisitorCount,@RecordID)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("VisitorID", obj.VisitorID);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("VID", obj.VID);
                                dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("AlreadyVisitorCount", obj.AlreadyVisitorCount);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载访客车辆信息异常：UploadParkVisitor()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }

        /// <summary>
        /// 上传下载车位预定信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkReserveBit(List<ParkReserveBit> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkReserveBit where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {

                                sql = string.Format(@"update ParkReserveBit set AccountID=@AccountID,PKID=@PKID,BitNo=@BitNo,PlateNumber=@PlateNumber,StartTime=@StartTime,EndTime=@EndTime,CreateTime=@CreateTime,Remark=@Remark,Status=@Status
                                LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus where RecordID=@RecordID");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("AccountID", obj.AccountID);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("BitNo", obj.BitNo);
                                dbOperator.AddParameter("PlateNumber", obj.PlateNumber);
                                dbOperator.AddParameter("StartTime", obj.StartTime);
                                dbOperator.AddParameter("EndTime", obj.EndTime);
                                dbOperator.AddParameter("CreateTime", obj.CreateTime);
                                dbOperator.AddParameter("Remark", obj.Remark);

                                dbOperator.AddParameter("Status", obj.Status);
                                dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkReserveBit(AccountID,PKID,BitNo,PlateNumber,StartTime,EndTime,CreateTime,Remark,Status,LastUpdateTime,HaveUpdate,DataStatus,RecordID)
                                                                values(@AccountID,@PKID,@BitNo,@PlateNumber,@StartTime,@EndTime,@CreateTime,@Remark,@Status,@LastUpdateTime,@HaveUpdate,@DataStatus,@RecordID)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("AccountID", obj.AccountID);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("BitNo", obj.BitNo);
                                dbOperator.AddParameter("PlateNumber", obj.PlateNumber);
                                dbOperator.AddParameter("StartTime", obj.StartTime);
                                dbOperator.AddParameter("EndTime", obj.EndTime);
                                dbOperator.AddParameter("CreateTime", obj.CreateTime);
                                dbOperator.AddParameter("Remark", obj.Remark);

                                dbOperator.AddParameter("Status", obj.Status);
                                dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载车位预定信息异常：UploadParkReserveBit()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }


        /// <summary>
        /// 上传下载车位组信息
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public List<UploadResult> UploadParkCarBitGroup(List<ParkCarBitGroup> listmodel)
        {
            //记录自增ID
            List<UploadResult> listresult = new List<UploadResult>();
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    foreach (var obj in listmodel)
                    {
                        string sql = "select ID from ParkCarBitGroup where RecordID=@RecordID ";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("RecordID", obj.RecordID);
                        using (DbDataReader reader = dbOperator.ExecuteReader(sql.ToString()))
                        {
                            if (reader.Read())
                            {
                                sql = string.Format(@"update ParkCarBitGroup set CarBitName=@CarBitName,CarBitNum=@CarBitNum,PKID=@PKID,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate,DataStatus=@DataStatus where RecordID=@RecordID");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("CarBitName", obj.CarBitName);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("CarBitNum", obj.CarBitNum);
                                dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                               
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }
                            else
                            {
                                reader.Close();
                                sql = string.Format(@"insert into ParkCarBitGroup(RecordID,CarBitName,CarBitNum,PKID,LastUpdateTime,HaveUpdate,DataStatus)
                                                                values(@RecordID,@CarBitName,@CarBitNum,@PKID,@LastUpdateTime,@HaveUpdate,@DataStatus)");
                                dbOperator.ClearParameters();
                                dbOperator.AddParameter("RecordID", obj.RecordID);
                                dbOperator.AddParameter("CarBitName", obj.CarBitName);
                                dbOperator.AddParameter("PKID", obj.PKID);
                                dbOperator.AddParameter("CarBitNum", obj.CarBitNum);
                                dbOperator.AddParameter("LastUpdateTime", obj.LastUpdateTime);
                                dbOperator.AddParameter("HaveUpdate", obj.HaveUpdate);
                                dbOperator.AddParameter("DataStatus", obj.DataStatus);
                                if (dbOperator.ExecuteNonQuery(sql) > 0)
                                {
                                    listresult.Add(new UploadResult() { RecordID = obj.RecordID, ID = obj.ID });
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("上传下载车位组信息异常：UploadParkCarBitGroup()" + ex.Message + "\r\n");
            }
            finally
            {

            }

            return listresult;
        }
    }
}

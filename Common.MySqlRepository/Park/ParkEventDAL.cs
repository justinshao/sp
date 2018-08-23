using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.DataAccess;
using Common.Entities;
using Common.Utilities;
using System.Data.Common;

namespace Common.MySqlRepository
{
    public class ParkEventDAL : BaseDAL, IParkEvent
    {
        public ParkEvent AddEventRec(ParkEvent model, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    model.DataStatus = (int)DataStatus.Normal;
                    model.LastUpdateTime = DateTime.Now;
                    model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;
                    model.RecordID = GuidGenerator.GetGuidString();
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into ParkEvent(RecordID,CardNo,CardNum,CarModelID,CarTypeID,DataStatus,EmployeeName,EventID,GateID,HaveUpdate,IORecordID,IOState,LastUpdateTime,OperatorID,ParkingID,PictureName,PlateColor,PlateNumber,RecTime,Remark,IsScanCode,IsOffline,CertNo,CertName,CertificateImage,Address,BirthDate,Nation,Sex)");
                    strSql.Append(" values(@RecordID,@CardNo,@CardNum,@CarModelID,@CarTypeID,@DataStatus,@EmployeeName,@EventID,@GateID,@HaveUpdate,@IORecordID,@IOState,@LastUpdateTime,@OperatorID,@ParkingID,@PictureName,@PlateColor,@PlateNumber,@RecTime,@Remark,@IsScanCode,@IsOffline,@CertNo,@CertName,@CertificateImage,@Address,@BirthDate,@Nation,@Sex);");
                    strSql.Append(" select * from ParkEvent where RecordID=@RecordID ");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", model.RecordID);
                    dbOperator.AddParameter("CardNo", model.CardNo);
                    dbOperator.AddParameter("CardNum", model.CardNum);
                    dbOperator.AddParameter("CarModelID", model.CarModelID);
                    dbOperator.AddParameter("CarTypeID", model.CarTypeID);
                    dbOperator.AddParameter("DataStatus", model.DataStatus);
                    dbOperator.AddParameter("EmployeeName", model.EmployeeName);
                    dbOperator.AddParameter("EventID", model.EventID);
                    dbOperator.AddParameter("GateID", model.GateID);
                    dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                    dbOperator.AddParameter("IORecordID", model.IORecordID);
                    dbOperator.AddParameter("IOState", model.IOState);
                    dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                    dbOperator.AddParameter("OperatorID", model.OperatorID);
                    dbOperator.AddParameter("ParkingID", model.ParkingID);
                    dbOperator.AddParameter("PictureName", model.PictureName);
                    dbOperator.AddParameter("PlateColor", model.PlateColor);
                    dbOperator.AddParameter("PlateNumber", model.PlateNumber);
                    dbOperator.AddParameter("RecTime", model.RecTime);
                    dbOperator.AddParameter("Remark", model.Remark);
                    dbOperator.AddParameter("IsScanCode", model.IsScanCode);
                    dbOperator.AddParameter("IsOffline", model.IsOffline);

                    dbOperator.AddParameter("CertNo", model.CertNo);
                    dbOperator.AddParameter("CertName", model.CertName);
                    dbOperator.AddParameter("CertificateImage", model.CertificateImage);
                    dbOperator.AddParameter("Address", model.Address);
                    dbOperator.AddParameter("BirthDate", model.BirthDate);
                    dbOperator.AddParameter("Nation", model.Nation);
                    dbOperator.AddParameter("Sex", model.Sex);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkEvent>.ToModel(reader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }


        public List<ParkEvent> GetEventRecYC(DateTime startTime, DateTime endtime)
        {
            List<ParkEvent> ParkEvents = new List<ParkEvent>();
            
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select * from ParkEvent where  (EventID=13 or EventID=19) and DataStatus!=@DataStatus  and RecTime>@StartTime and RecTime<@EndTime");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();

                    dbOperator.AddParameter("StartTime", startTime);
                    dbOperator.AddParameter("EndTime", endtime);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkEvents.Add(DataReaderToModel<ParkEvent>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
               
            }
            return ParkEvents;
        }

        public List<ParkEvent> GetEventRecWhitPageTab(string parkingID, int pageSize, int pageIndex, string carTypeID, string carModelid, string operatorID, string gateID, int eventID, DateTime startTime, DateTime endTime, int iostate, string likePlateNumber, out int pageCount)
        {
            throw new NotImplementedException();
        }
    }
}

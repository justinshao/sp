using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Parking;
using Common.Entities.WX;
using Common.DataAccess;
using Common.Entities;
using System.Data.Common;
using Common.Utilities;
using Common.Entities.Condition;

namespace Common.MySqlRepository
{
    public class ParkVisitorDAL : BaseDAL, IParkVisitor
    {
        public bool Add(VisitorInfo model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                try
                {
                    dbOperator.BeginTransaction();
                    dbOperator.ClearParameters();
                    bool result = AddVisitorInfo(model, dbOperator);
                    if (!result) throw new MyException("保存访客信息失败");
                    if (model.ParkVisitors != null && model.ParkVisitors.Count > 0)
                    {
                        foreach (var item in model.ParkVisitors)
                        {
                            dbOperator.ClearParameters();
                            result = AddVisitorCar(item, dbOperator);
                            if (!result) throw new MyException("保存访客车辆信息失败");
                        }
                    }
                    dbOperator.CommitTransaction();
                    return result;
                }
                catch
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }
        private bool AddVisitorInfo(VisitorInfo model, DbOperator dbOperator)
        {
            model.DataStatus = (int)DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into VisitorInfo(RecordID,PlateNumber,VisitorMobilePhone,VisitorCount,BeginDate,EndDate,AccountID,CreateTime,VisitorState,LastUpdateTime,HaveUpdate");
            strSql.Append(" ,DataStatus,VID,LookSate,IsExamine,VisitorName,VisitorSex,Birthday,CertifType,CertifNo,CertifAddr,EmployeeID,CardNo,OperatorID,VisitorCompany,VistorPhoto,CertifPhoto,VisitorSource)");
            strSql.Append(" values(@RecordID,@PlateNumber,@VisitorMobilePhone,@VisitorCount,@BeginDate,@EndDate,@AccountID,@CreateTime,@VisitorState,@LastUpdateTime,@HaveUpdate");
            strSql.Append(" ,@DataStatus,@VID,@LookSate,@IsExamine,@VisitorName,@VisitorSex,@Birthday,@CertifType,@CertifNo,@CertifAddr,@EmployeeID,@CardNo,@OperatorID,@VisitorCompany,@VistorPhoto,@CertifPhoto,@VisitorSource)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("RecordID", model.RecordID);
            dbOperator.AddParameter("PlateNumber", model.PlateNumber);
            dbOperator.AddParameter("VisitorMobilePhone", model.VisitorMobilePhone);
            dbOperator.AddParameter("VisitorCount", model.VisitorCount);
            dbOperator.AddParameter("BeginDate", model.BeginDate);
            dbOperator.AddParameter("EndDate", model.EndDate);
            dbOperator.AddParameter("AccountID", model.AccountID);
            dbOperator.AddParameter("CreateTime", model.CreateTime);
            dbOperator.AddParameter("VisitorState", model.VisitorState);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", model.DataStatus);
            dbOperator.AddParameter("VID", model.VID);
            dbOperator.AddParameter("LookSate", model.LookSate);
            dbOperator.AddParameter("IsExamine", model.IsExamine);
            dbOperator.AddParameter("VisitorName", model.VisitorName);
            dbOperator.AddParameter("VisitorSex", model.VisitorSex);
            dbOperator.AddParameter("Birthday", model.Birthday);
            dbOperator.AddParameter("CertifType", model.CertifType);
            dbOperator.AddParameter("CertifNo", model.CertifNo);
            dbOperator.AddParameter("CertifAddr", model.CertifAddr);
            dbOperator.AddParameter("EmployeeID", model.EmployeeID);
            dbOperator.AddParameter("CardNo", model.CardNo);
            dbOperator.AddParameter("OperatorID", model.OperatorID);
            dbOperator.AddParameter("VisitorCompany", model.VisitorCompany);
            dbOperator.AddParameter("VistorPhoto", model.VistorPhoto);
            dbOperator.AddParameter("CertifPhoto", model.CertifPhoto);
            dbOperator.AddParameter("VisitorSource", model.VisitorSource);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;

        }
        private bool AddVisitorCar(ParkVisitor model, DbOperator dbOperator)
        {
            model.DataStatus = (int)DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ParkVisitor(RecordID,VisitorID,PKID,VID,LastUpdateTime,HaveUpdate,DataStatus,AlreadyVisitorCount)");
            strSql.Append(" values(@RecordID,@VisitorID,@PKID,@VID,@LastUpdateTime,@HaveUpdate,@DataStatus,@AlreadyVisitorCount)");

            dbOperator.ClearParameters();
            dbOperator.AddParameter("RecordID", model.RecordID);
            dbOperator.AddParameter("VisitorID", model.VisitorID);
            dbOperator.AddParameter("PKID", model.PKID);
            dbOperator.AddParameter("VID", model.VID);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", model.DataStatus);
            dbOperator.AddParameter("AlreadyVisitorCount", model.AlreadyVisitorCount);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public bool CancelVisitor(string visitorId)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update VisitorInfo set VisitorState=@VisitorState,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where RecordID=@RecordID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", visitorId);
                dbOperator.AddParameter("VisitorState",2);
                dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }
        public List<VisitorInfo> GetVisitorInfoPage(string operatorId,int pageIndex, int pageSize, out int total)
        {
            List<VisitorInfo> models = new List<VisitorInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select t.*,v.VName from VisitorInfo t left join BaseVillage v on t.VID=v.VID where t.OperatorID=@OperatorID and t.DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("OperatorID", operatorId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.Paging(strSql.ToString(), " ID DESC", pageIndex, pageSize, out total))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<VisitorInfo>.ToModel(reader));
                    }
                }
            }
            List<ParkVisitor> carModels = new List<ParkVisitor>();
            if (models.Count > 0) {
                StringBuilder strCarSql = new StringBuilder();
                strCarSql.AppendFormat("select v.*,p.PKName from ParkVisitor v left join BaseParkinfo p on v.PKID=P.PKID where v.VisitorID in('{0}') and v.DataStatus!=@DataStatus", string.Join("','", models.Select(p => p.RecordID)));

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strCarSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            carModels.Add(DataReaderToModel<ParkVisitor>.ToModel(reader));
                        }
                    }
                }
            }
            foreach (var item in models)
            {
                item.ParkVisitors = carModels.Where(p=>p.VisitorID==item.RecordID).ToList();
            }
            return models;
        }
        public VisitorInfo GetVisitor(string villageID, string plateNumber, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from VisitorInfo where PlateNumber=@PlateNumber and VID=@VID and DataStatus!=@DataStatus");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PlateNumber", plateNumber);
                    dbOperator.AddParameter("VID", villageID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<VisitorInfo>.ToModel(reader);
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

        public ParkVisitor GetVisitorCar(string parkingID, string visitorID, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkVisitor where VisitorID=@VisitorID and PKID=@PKID and DataStatus!=@DataStatus");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("VisitorID", visitorID);
                    dbOperator.AddParameter("PKID", parkingID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        return DataReaderToModel<ParkVisitor>.ToModel(reader);
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return null;
        }

        public bool ModifyVisitorCar(ParkVisitor model, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateConnection())
                {
                    model.DataStatus = DataStatus.Normal;
                    model.LastUpdateTime = DateTime.Now;
                    model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(@"update ParkVisitor set AlreadyVisitorCount=@AlreadyVisitorCount,DataStatus=@DataStatus,HaveUpdate=@HaveUpdate,LastUpdateTime=@LastUpdateTime,PKID=@PKID,
                        VID=@VID,VisitorID=@VisitorID");
                    strSql.Append(" where RecordID=@RecordID");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("AlreadyVisitorCount", model.AlreadyVisitorCount);
                    dbOperator.AddParameter("DataStatus", model.DataStatus);
                    dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                    dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                    dbOperator.AddParameter("PKID", model.PKID);
                    dbOperator.AddParameter("RecordID", model.RecordID);
                    dbOperator.AddParameter("VID", model.VID);
                    dbOperator.AddParameter("VisitorID", model.VisitorID);
                    return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return false;
        }

        public bool EditVisitorInfo(VisitorInfo model)
        {
            throw new NotImplementedException();
        }

        public List<VisitorInfo> QueryPage(VisitorInfoCondition condition, int pagesize, int pageindex, out int total)
        {
            throw new NotImplementedException();
        }

        public List<VisitorInfo> QueryPageIn(VisitorInfoCondition condition, int pagesize, int pageindex, out int total)
        {
            throw new NotImplementedException();
        }

        public List<VisitorInfo> QueryPageInOut(VisitorInfoCondition condition, int pagesize, int pageindex, out int total)
        {
            throw new NotImplementedException();
        }
    }
}

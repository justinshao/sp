using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.BWY;
using Common.DataAccess;
using Common.Entities;
using System.Data.Common;
using Common.Utilities;
using Common.IRepository;

namespace Common.MySqlRepository
{
    public class BWYGateMappingDAL : BaseDAL, IBWYGateMapping
    {
        public bool Add(BWYGateMapping model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into BWYGateMapping(RecordID,ParkingID,ParkingName,GateID,GateName,CreateTime,DataStatus,LastUpdateTime,HaveUpdate)");
                strSql.Append(" values(@RecordID,@ParkingID,@ParkingName,@GateID,@GateName,@CreateTime,@DataStatus,@LastUpdateTime,@HaveUpdate)");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("ParkingID", model.ParkingID);
                dbOperator.AddParameter("ParkingName", model.ParkingName);
                dbOperator.AddParameter("GateID", model.GateID);
                dbOperator.AddParameter("GateName", model.GateName);
                dbOperator.AddParameter("CreateTime", DateTime.Now);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Update(BWYGateMapping model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update BWYGateMapping set ParkingID=@ParkingID,ParkingName=@ParkingName,GateID=@GateID,GateName=@GateName");
                strSql.Append(",LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where RecordID=@RecordID");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", model.RecordID);
                dbOperator.AddParameter("ParkingID", model.ParkingID);
                dbOperator.AddParameter("ParkingName", model.ParkingName);
                dbOperator.AddParameter("GateID", model.GateID);
                dbOperator.AddParameter("GateName", model.GateName);
                dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }
        }

        public bool Delete(string recordId)
        {
            return CommonDelete("BWYGateMapping", "RecordID", recordId);
        }

        public BWYGateMapping QueryByRecordId(string recordId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BWYGateMapping where RecordID=@RecordID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("RecordID", recordId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if(reader.Read())
                    {
                        return DataReaderToModel<BWYGateMapping>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
        public List<BWYGateMapping> QueryByParkingID(string parkingId)
        {
            List<BWYGateMapping> models = new List<BWYGateMapping>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BWYGateMapping where ParkingID=@ParkingID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("ParkingID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while(reader.Read())
                    {
                        models.Add(DataReaderToModel<BWYGateMapping>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
        public BWYGateMapping QueryByGateID(string gateId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BWYGateMapping where GateID=@GateID and DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("GateID", gateId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<BWYGateMapping>.ToModel(reader);
                    }
                    return null;
                }
            }
        }
        public List<BWYGateMapping> QueryPage(string parkName,string gateName, int pageIndex, int pageSize, out int recordTotalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM BWYGateMapping WHERE  DataStatus!=@DataStatus");
           
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                if (!string.IsNullOrWhiteSpace(parkName))
                {
                    sql.Append(" and ParkingName like @ParkingName");
                    dbOperator.AddParameter("ParkingName", "%" + parkName + "%");
                }
                if (!string.IsNullOrWhiteSpace(gateName))
                {
                    sql.Append(" and GateName like @GateName");
                    dbOperator.AddParameter("GateName", "%" + gateName + "%");
                }


                List<BWYGateMapping> models = new List<BWYGateMapping>();
                using (DbDataReader reader = dbOperator.Paging(sql.ToString(), "id DESC", pageIndex, pageSize, out recordTotalCount))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<BWYGateMapping>.ToModel(reader));
                    }

                }
                return models;

            }
        }

        public List<BWYGateMapping> QueryAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(BWYGateMapping model, DbOperator dbOperator)
        {
            throw new NotImplementedException();
        }

        public List<BWYGateMapping> QueryByDataSource(int dataSurce)
        {
            throw new NotImplementedException();
        }

        public List<BWYGateMapping> QueryPage(string parkName, string gateName, int? dataSource, int pageIndex, int pageSize, out int recordTotalCount)
        {
            throw new NotImplementedException();
        }
    }
}

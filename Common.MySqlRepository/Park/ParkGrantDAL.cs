using Common.IRepository.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.DataAccess;
using System.Data.Common;
using Common.Utilities;
using Common.Entities.Parking;

namespace Common.MySqlRepository
{
    public class ParkGrantDAL : BaseDAL, IParkGrant
    {
        public ParkGrant GetCardgrant(string plateID, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkGrant where PlateID=@PlateID and DataStatus!=@DataStatus");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PlateID", plateID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        if (reader.Read())
                        {
                            return DataReaderToModel<ParkGrant>.ToModel(reader);
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

        public List<ParkGrant> GetCardgrantsByLot(string pkid, string likelot, out string ErrorMessage)
        {
            List<ParkGrant> ParkGrants = new List<ParkGrant>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkGrant where PKID=@PKID and  PKLot like @PKLot and DataStatus!=@DataStatus");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", pkid);
                    dbOperator.AddParameter("PKLot", "%"+likelot+ "%");
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkGrants.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return ParkGrants;
        }

        public List<ParkGrant> GetParkGrantByPlateNumberID(string pkid, string plateID, out string ErrorMessage)
        {
            List<ParkGrant> ParkGrants = new List<ParkGrant>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkGrant where PKID=@PKID and PlateID=@PlateID and DataStatus!=@DataStatus");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", pkid);
                    dbOperator.AddParameter("PlateID", plateID);
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkGrants.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return ParkGrants;
        }


        public bool Add(ParkGrant model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Add(model, dbOperator);
            }
        }

        public bool Add(ParkGrant model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ParkGrant(GID,CardID,PKID,BeginDate,EndDate,CarTypeID,CarModelID,PKLot,PlateID,ComdState,AreaIDS,GateID,State,LastUpdateTime,HaveUpdate,DataStatus)");
            strSql.Append(" values(@GID,@CardID,@PKID,@BeginDate,@EndDate,@CarTypeID,@CarModelID,@PKLot,@PlateID,@ComdState,@AreaIDS,@GateID,@State,@LastUpdateTime,@HaveUpdate,@DataStatus)");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("GID", model.GID);
            dbOperator.AddParameter("CardID", model.CardID);
            dbOperator.AddParameter("PKID", model.PKID);
            dbOperator.AddParameter("BeginDate", model.BeginDate);
            dbOperator.AddParameter("EndDate", model.EndDate);
            dbOperator.AddParameter("CarTypeID", model.CarTypeID);
            dbOperator.AddParameter("CarModelID", model.CarModelID);
            dbOperator.AddParameter("PKLot", model.PKLot);
            dbOperator.AddParameter("PlateID", model.PlateID);
            dbOperator.AddParameter("ComdState", (int)model.ComdState);
            dbOperator.AddParameter("AreaIDS", model.AreaIDS);
            dbOperator.AddParameter("GateID", model.GateID);
            dbOperator.AddParameter("State", (int)model.State);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Add(List<ParkGrant> models)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Add(models, dbOperator);
            }
        }

        public bool Add(List<ParkGrant> models, DbOperator dbOperator)
        {
            if (models.Count == 0) return false;

            StringBuilder strSql = new StringBuilder();

            strSql.Append("insert into ParkGrant(GID,CardID,PKID,BeginDate,EndDate,CarTypeID,CarModelID,PKLot,PlateID,ComdState,AreaIDS,GateID,State,LastUpdateTime,HaveUpdate,DataStatus) values");
            int index = 0;
            foreach (var item in models)
            {
                strSql.AppendFormat("(@GID{0},@CardID{0},@PKID{0},@BeginDate{0},@EndDate{0},@CarTypeID{0},@CarModelID{0},@PKLot{0},@PlateID{0},@ComdState{0},@AreaIDS{0},@GateID{0},@State{0},@LastUpdateTime{0},@HaveUpdate{0},@DataStatus{0}),", index);
                index++;
            }

            bool hasData = false;
            index = 0;
            foreach (var model in models)
            {
                model.DataStatus = DataStatus.Normal;
                model.LastUpdateTime = DateTime.Now;
                model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

                dbOperator.ClearParameters();
                dbOperator.AddParameter("GID" + index, model.GID);
                dbOperator.AddParameter("CardID" + index, model.CardID);
                dbOperator.AddParameter("PKID" + index, model.PKID);
                dbOperator.AddParameter("BeginDate" + index, model.BeginDate);
                dbOperator.AddParameter("EndDate" + index, model.EndDate);
                dbOperator.AddParameter("CarTypeID" + index, model.CarTypeID);
                dbOperator.AddParameter("CarModelID" + index, model.CarModelID);
                dbOperator.AddParameter("PKLot" + index, model.PKLot);
                dbOperator.AddParameter("PlateID" + index, model.PlateID);
                dbOperator.AddParameter("ComdState" + index, (int)model.ComdState);
                dbOperator.AddParameter("AreaIDS" + index, model.AreaIDS);
                dbOperator.AddParameter("GateID" + index, model.GateID);
                dbOperator.AddParameter("State", (int)model.State);
                dbOperator.AddParameter("LastUpdateTime" + index, model.LastUpdateTime);
                dbOperator.AddParameter("HaveUpdate" + index, model.HaveUpdate);
                dbOperator.AddParameter("DataStatus" + index, (int)model.DataStatus);
                hasData = true;
                index++;
            }
            if (hasData)
            {
                return dbOperator.ExecuteNonQuery(strSql.Remove(strSql.Length - 10, 10).ToString()) > 0;
            }
            return false;
        }

        public bool Update(ParkGrant model)
        {
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                return Update(model, dbOperator);
            }
        }

        public bool Update(ParkGrant model, DbOperator dbOperator)
        {
            model.DataStatus = DataStatus.Normal;
            model.LastUpdateTime = DateTime.Now;
            model.HaveUpdate = SystemDefaultConfig.DataUpdateFlag;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkGrant set CardID=@CardID,PKID=@PKID,BeginDate=@BeginDate,EndDate=@EndDate,CarTypeID=@CarTypeID,CarModelID=@CarModelID,PKLot=@PKLot,");
            strSql.Append(" PlateID=@PlateID,ComdState=@ComdState,AreaIDS=@AreaIDS,GateID=@GateID,State=@State,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
            strSql.Append(" where GID=@GID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("GID", model.GID);
            dbOperator.AddParameter("CardID", model.CardID);
            dbOperator.AddParameter("PKID", model.PKID);
            dbOperator.AddParameter("BeginDate", model.BeginDate);
            dbOperator.AddParameter("EndDate", model.EndDate);
            dbOperator.AddParameter("CarTypeID", model.CarTypeID);
            dbOperator.AddParameter("CarModelID", model.CarModelID);
            dbOperator.AddParameter("PKLot", model.PKLot);
            dbOperator.AddParameter("PlateID", model.PlateID);
            dbOperator.AddParameter("ComdState", (int)model.ComdState);
            dbOperator.AddParameter("AreaIDS", model.AreaIDS);
            dbOperator.AddParameter("GateID", model.GateID);
            dbOperator.AddParameter("State", (int)model.State);
            dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
            dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public ParkGrant QueryByPlateNumber(string plateNumber)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select g.* from ParkGrant g inner join  EmployeePlate p where p.PlateNo=@PlateNo and g.DataStatus!=@DataStatus and p.DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PlateNo", plateNumber);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkGrant>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public ParkGrant QueryByGrantId(string grantId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkGrant  where GID=@GID and DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("GID", grantId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkGrant>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public bool Update(string grantId, ParkGrantState state, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkGrant set State=@State,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
            strSql.Append(" where GID=@GID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("GID", grantId);
            dbOperator.AddParameter("State", (int)state);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Delete(string grantId)
        {
            return CommonDelete("ParkGrant", "GID", grantId);
        }

        public bool Delete(string cardId, string parkingId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkGrant DataStatus!=@DataStatus,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
            strSql.Append(" where CardID=@CardID and PKID=@PKID");
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CardID", cardId);
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
            }

        }

        public bool DeleteByCardId(string cardId)
        {
            return CommonDelete("ParkGrant", "CardID", cardId);
        }

        public bool DeleteByCardId(string cardId, DbOperator dbOperator)
        {
            return CommonDelete("ParkGrant", "CardID", cardId, dbOperator);
        }

        public bool Delete(List<string> cardIds, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkGrant DataStatus!=@DataStatus,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
            strSql.AppendFormat(" where CardID in('{0}')", string.Join("','", cardIds));

            dbOperator.ClearParameters();
            dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public bool Delete(string grantId, DbOperator dbOperator)
        {
            return CommonDelete("ParkGrant", "GID", grantId, dbOperator);
        }

        public List<ParkGrant> QueryNormalParkGrant(string parkingId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkGrant  where PKID=@PKID and State=@State and DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("State", (int)ParkGrantState.Normal);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                List<ParkGrant> models = new List<ParkGrant>();
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<ParkGrant> QueryByParkingId(string parkingId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkGrant  where PKID=@PKID and DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                List<ParkGrant> models = new List<ParkGrant>();
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<ParkGrant> QueryByParkingIds(List<string> parkingIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from ParkGrant  where PKID in('{0}') and DataStatus!=@DataStatus", string.Join("','", parkingIds));

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                List<ParkGrant> models = new List<ParkGrant>();
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<ParkGrant> QueryByCardIds(List<string> cardIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from ParkGrant  where CardID in('{0}') and DataStatus!=@DataStatus", string.Join("','", cardIds));

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                List<ParkGrant> models = new List<ParkGrant>();
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<ParkGrant> QueryByCardIdAndParkingIds(string cardId, List<string> parkingIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from ParkGrant  where CardID=@CardID AND PKID in('{0}') and DataStatus!=@DataStatus", string.Join("','", parkingIds));

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CardID", cardId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                List<ParkGrant> models = new List<ParkGrant>();
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public ParkGrant QueryByCardIdAndParkingId(string cardId, string parkingId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkGrant  where CardID=@CardID AND PKID =@PKID and DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CardID", cardId);
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkGrant>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public List<ParkGrant> QueryByCardId(string cardId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkGrant  where CardID=@CardID and DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("CardID", cardId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                List<ParkGrant> models = new List<ParkGrant>();
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<ParkGrant> QueryByParkingAndPlateId(string parkingId, string plateId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkGrant  where PKID=@PKID AND PlateID=@PlateID and DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("PlateID", plateId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                List<ParkGrant> models = new List<ParkGrant>();
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<ParkGrant> QueryByPlateId(string plateId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ParkGrant  where PlateID=@PlateID and DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PlateID", plateId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                List<ParkGrant> models = new List<ParkGrant>();
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public ParkGrant QueryByParkingAndPlateNumber(string parkingId, string plateNumber)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT P.* FROM PARKGRANT P INNER JOIN EmployeePlate E ON P.PlateID=E.PlateID");
            strSql.Append(" WHERE P.PKID=@PKID AND E.PlateNo=@PlateNo AND E.DataStatus!=@DataStatus AND P.DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("PlateNo", plateNumber);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    if (reader.Read())
                    {
                        return DataReaderToModel<ParkGrant>.ToModel(reader);
                    }
                    return null;
                }
            }
        }

        public List<ParkGrant> QueryByParkingAndLotAndCarType(string parkingId, string lots, BaseCarType carType, string excludeGrantId)
        {


            List<ParkGrant> models = new List<ParkGrant>();
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string[] strLots = lots.TrimEnd(',').Split(',');
                for (int i = 0; i < strLots.Length; i++)
                {
                    dbOperator.ClearParameters();
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("SELECT G.* FROM ParkGrant G INNER JOIN ParkCarType P");
                    strSql.AppendFormat(" ON G.CarTypeID=P.CarTypeID WHERE g.GID!='{0}' AND G.PKID='{1}'", excludeGrantId, parkingId);
                    strSql.AppendFormat("  AND ','+G.PKLot+',' like '%,{0},%' AND P.BaseTypeID={1}", strLots[i], (int)carType);
                    strSql.AppendFormat("  AND G.DataStatus!={0} AND P.DataStatus!={0}", (int)DataStatus.Delete);

                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkGrant model = DataReaderToModel<ParkGrant>.ToModel(reader);
                            if (models.FirstOrDefault(p => p.GID == model.GID) == null)
                            {
                                models.Add(model);
                            }
                        }
                    }
                }
                return models;
            }
        }

        public List<ParkGrantView> QueryPage(Entities.Condition.ParkGrantCondition condition, int pagesize, int pageindex, out int total)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select p.*,o.EmployeeID,o.EmployeeName,o.CertifNo,o.MobilePhone,o.HomePhone,u.CardNo,");
            strSql.Append(" u.CardNumb,u.Balance,op.PlateNo,op.Color,c.OverdueToTemp,");
            strSql.Append(" c.MonthCardExpiredEnterDay,o.FamilyAddr,o.Remark from ParkGrant p");
            strSql.Append(" inner join BaseCard u on p.CardID=u.CardID");
            strSql.Append(" inner join ParkCarType c on c.CarTypeID=p.CarTypeID");
            strSql.Append(" left join BaseEmployee o on o.EmployeeID =u.EmployeeID");
            strSql.Append(" left join EmployeePlate op on p.PlateID=op.PlateID");
            strSql.Append(" where p.DataStatus!=@DataStatus and  u.DataStatus!=@DataStatus and (o.DataStatus!=@DataStatus or o.DataStatus is null)");
            strSql.Append(" and (op.DataStatus!=@DataStatus or op.DataStatus is null) and p.PKID=@PKID");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PKID", condition.ParkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);

                if (!string.IsNullOrWhiteSpace(condition.EmployeeNameOrMoblie))
                {
                    strSql.Append(" and (o.EmployeeName like @EmployeeName or o.MobilePhone like @EmployeeName or o.HomePhone like @EmployeeName)");
                    dbOperator.AddParameter("EmployeeName", condition.EmployeeNameOrMoblie);
                }
                if (!string.IsNullOrWhiteSpace(condition.CarTypeId))
                {
                    strSql.Append(" and p.CarTypeID=@CarTypeID");
                    dbOperator.AddParameter("CarTypeID", condition.CarTypeId);
                }
                if (!string.IsNullOrWhiteSpace(condition.CarModelId))
                {
                    strSql.Append(" and p.CarModelId=@CarModelId");
                    dbOperator.AddParameter("CarModelId", condition.CarModelId);
                }
                if (!string.IsNullOrWhiteSpace(condition.PlateNumber))
                {
                    strSql.Append(" and op.PlateNo like @PlateNo");
                    dbOperator.AddParameter("PlateNo", "%" + condition.PlateNumber + "%");
                }
                if (!string.IsNullOrWhiteSpace(condition.CardNo))
                {
                    strSql.Append(" and (u.CardNo like @CardNo or u.CardNum like @CardNo)");
                    dbOperator.AddParameter("CardNo", condition.CardNo);
                }
                if (!string.IsNullOrWhiteSpace(condition.HomeAddress))
                {
                    strSql.Append(" and o.FamilyAddr like @FamilyAddr");
                    dbOperator.AddParameter("FamilyAddr", "%" + condition.HomeAddress + "%");
                }
                if (!string.IsNullOrWhiteSpace(condition.ParkingLot))
                {
                    strSql.Append(" and p.PKLot like @PKLot");
                    dbOperator.AddParameter("PKLot", "%" + condition.ParkingLot + "%");
                }
                if (condition.State.HasValue && condition.State.Value >= 0)
                {
                    strSql.Append(" and p.State=@State");
                    dbOperator.AddParameter("State", condition.State.Value);
                }
                if (condition.State.HasValue && condition.State.Value < 0)
                {
                    strSql.Append(" and (p.EndDate<@EndDate or p.EndDate is null)");
                    dbOperator.AddParameter("EndDate", DateTime.Now.Date.ToString("yyyy-MM-dd"));
                }
                if (condition.QueryHasParkingLot)
                {
                    strSql.Append(" and p.PKLot !='' and p.PKLot is not null ");
                }

                using (DbDataReader reader = dbOperator.Paging(strSql.ToString(), "ID DESC", pageindex, pagesize, out total))
                {
                    List<ParkGrantView> models = new List<ParkGrantView>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkGrantView>.ToModel(reader));
                    }
                    return models;
                }
            }
        }
        public bool Renewals(string grantId, DateTime beginDate, DateTime endDate, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkGrant set BeginDate=@BeginDate,EndDate=@EndDate,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
            strSql.Append(" where GID=@GID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("GID", grantId);
            dbOperator.AddParameter("BeginDate", beginDate);
            dbOperator.AddParameter("EndDate", endDate);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }
        public bool RestoreUse(string grantId, DateTime beginDate, DateTime endDate, DbOperator dbOperator)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ParkGrant set State=@State,BeginDate=@BeginDate,EndDate=@EndDate,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate");
            strSql.Append(" where GID=@GID");
            dbOperator.ClearParameters();
            dbOperator.AddParameter("GID", grantId);
            dbOperator.AddParameter("State", (int)ParkGrantState.Normal);
            dbOperator.AddParameter("BeginDate", beginDate);
            dbOperator.AddParameter("EndDate", endDate);
            dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
            dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
            return dbOperator.ExecuteNonQuery(strSql.ToString()) > 0;
        }

        public List<ParkGrant> Query(List<string> parkingIds, string plateNumber, BaseCarType carType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT P.* FROM PARKGRANT P INNER JOIN EmployeePlate E ON P.PlateID=E.PlateID");
            strSql.Append(" inner join ParkCarType t on t.CarTypeID=P.CarTypeID");
            strSql.AppendFormat(" WHERE P.PKID in('{0}') AND t.BaseTypeID=@BaseTypeID", string.Join("','", parkingIds));
            strSql.Append(" AND E.PlateNo=@PlateNo AND E.DataStatus!=@DataStatus AND P.DataStatus!=@DataStatus AND t.DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("BaseTypeID", (int)carType);
                dbOperator.AddParameter("PlateNo", plateNumber);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<ParkGrant> models = new List<ParkGrant>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<ParkGrant> QueryHasLotByParkingId(string parkingId, BaseCarType carType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT P.* FROM PARKGRANT P inner join ParkCarType t on t.CarTypeID=P.CarTypeID");
            strSql.Append(" WHERE  t.BaseTypeID=@BaseTypeID and P.PKID=@PKID  AND t.DataStatus!=@DataStatus AND P.DataStatus!=@DataStatus");
            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("BaseTypeID", (int)carType);
                dbOperator.AddParameter("PKID", parkingId);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    List<ParkGrant> models = new List<ParkGrant>();
                    while (reader.Read())
                    {
                        models.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                    }
                    return models;
                }
            }
        }

        public List<ParkGrant> GetCardgrantByParkingID(string parkingID, out string ErrorMessage)
        {
            List<ParkGrant> ParkGrants = new List<ParkGrant>();
            ErrorMessage = "";
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ParkGrant where PKID=@PKID  and DataStatus!=@DataStatus");

                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("PKID", parkingID); 
                    dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                    using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                    {
                        while (reader.Read())
                        {
                            ParkGrants.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return ParkGrants;
        }
        public List<ParkGrant> GetParkGrantByPlateNo(string plateNo) {
            List<ParkGrant> ParkGrants = new List<ParkGrant>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select g.* from ParkGrant g inner join EmployeePlate e on g.PlateID=e.PlateID inner join BaseParkinfo p on g.PKID=p.PKID where e.PlateNo=@PlateNo  and e.DataStatus!=@DataStatus and g.DataStatus!=@DataStatus and p.DataStatus!=@DataStatus");

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                dbOperator.ClearParameters();
                dbOperator.AddParameter("PlateNo", plateNo);
                dbOperator.AddParameter("DataStatus", (int)DataStatus.Delete);
                using (DbDataReader reader = dbOperator.ExecuteReader(strSql.ToString()))
                {
                    while (reader.Read())
                    {
                        ParkGrants.Add(DataReaderToModel<ParkGrant>.ToModel(reader));
                    }
                }
            }
            return ParkGrants;
        }

        /// <summary>
        /// 退款操作
        /// </summary>
        /// <param name="GID"></param>
        /// <param name="Amout"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public bool RefundCardAmout(List<ParkGrant> grantlist,DateTime EndTime,ParkOrder model)
        {
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    dbOperator.BeginTransaction();
                    foreach (var obj in grantlist)
                    {
                        string strsql = "update ParkGrant set EndDate=@EndDate,LastUpdateTime=@LastUpdateTime,HaveUpdate=@HaveUpdate where GID=@GID";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("EndDate", EndTime);
                        dbOperator.AddParameter("LastUpdateTime", DateTime.Now);
                        dbOperator.AddParameter("HaveUpdate", SystemDefaultConfig.DataUpdateFlag);
                        dbOperator.AddParameter("GID", obj.GID);
                        if (dbOperator.ExecuteNonQuery(strsql) < 0)
                        {
                            dbOperator.RollbackTransaction();
                            return false;
                        }
                    }

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(@"insert into ParkOrder(RecordID,Amount,CarderateID,CashMoney,CashTime,DataStatus,DiscountAmount,HaveUpdate,LastUpdateTime,NewMoney,NewUsefulDate,
                                    NewUserBegin,OldMoney,OldUserBegin,OldUserulDate,OnlineOrderNo,OnlineUserID,OrderNo,OrderSource,OrderTime,OrderType,PayAmount,
                                    PayTime,PayWay,PKID,Remark,Status,TagID,UnPayAmount,UserID,FeeRuleID) ");
                    strSql.Append(@" values(@RecordID,@Amount,@CarderateID,@CashMoney,@CashTime,@DataStatus,@DiscountAmount,@HaveUpdate,@LastUpdateTime,@NewMoney,@NewUsefulDate,
                                    @NewUserBegin,@OldMoney,@OldUserBegin,@OldUserulDate,@OnlineOrderNo,@OnlineUserID,@OrderNo,@OrderSource,@OrderTime,@OrderType,@PayAmount,
                                    @PayTime,@PayWay,@PKID,@Remark,@Status,@TagID,@UnPayAmount,@UserID,@FeeRuleID);");
                    dbOperator.ClearParameters();
                    dbOperator.AddParameter("RecordID", model.RecordID);
                    dbOperator.AddParameter("Amount", model.Amount);
                    dbOperator.AddParameter("CarderateID", model.CarderateID);
                    dbOperator.AddParameter("CashMoney", model.CashMoney);
                    dbOperator.AddParameter("DataStatus", (int)model.DataStatus);
                    dbOperator.AddParameter("DiscountAmount", model.DiscountAmount);
                    dbOperator.AddParameter("HaveUpdate", model.HaveUpdate);
                    dbOperator.AddParameter("LastUpdateTime", model.LastUpdateTime);
                    dbOperator.AddParameter("NewMoney", model.NewMoney);
                    dbOperator.AddParameter("CashTime", DBNull.Value);
                    dbOperator.AddParameter("NewUsefulDate", model.NewUsefulDate);
                    dbOperator.AddParameter("OldUserulDate", model.OldUserulDate);
                    dbOperator.AddParameter("OldUserBegin", model.OldUserBegin);
                    dbOperator.AddParameter("NewUserBegin", model.NewUserBegin);
                    dbOperator.AddParameter("OldMoney", model.OldMoney);
                    dbOperator.AddParameter("OnlineOrderNo", model.OnlineOrderNo);
                    dbOperator.AddParameter("OnlineUserID", model.OnlineUserID);
                    dbOperator.AddParameter("OrderNo", model.OrderNo);
                    dbOperator.AddParameter("OrderSource", model.OrderSource);
                    dbOperator.AddParameter("OrderTime", model.OrderTime);
                    dbOperator.AddParameter("OrderType", model.OrderType);
                    dbOperator.AddParameter("PayAmount", model.PayAmount);
                    dbOperator.AddParameter("PayTime", model.PayTime);
                    dbOperator.AddParameter("PayWay", model.PayWay);
                    dbOperator.AddParameter("PKID", model.PKID);
                    dbOperator.AddParameter("FeeRuleID", model.FeeRuleID);
                    dbOperator.AddParameter("Remark", model.Remark);
                    dbOperator.AddParameter("Status", model.Status);
                    dbOperator.AddParameter("TagID", model.TagID);
                    dbOperator.AddParameter("UnPayAmount", model.UnPayAmount);
                    dbOperator.AddParameter("UserID", model.UserID);
                    if (dbOperator.ExecuteNonQuery(strSql.ToString()) < 0)
                    {
                        dbOperator.RollbackTransaction();
                        return false;
                    }
                    dbOperator.CommitTransaction();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<ParkCarBitGroup> QueryCarBitGroupByParkingId(string parkingid)
        {
            throw new NotImplementedException();
        }

        public bool AddParkCarBitGroup(ParkCarBitGroup model, DbOperator dbOperator)
        {
            throw new NotImplementedException();
        }

        public bool UpdateParkGrantPKLot(string newPkLot, string oldPkLot, string parkingId, DbOperator dbOperator)
        {
            throw new NotImplementedException();
        }

        public ParkCarBitGroup GetParkCarBitGroup(string parkingId, string carBitName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateParkCarBitGroup(string recordId, string newPkLot, int carBitNum, DbOperator dbOperator)
        {
            throw new NotImplementedException();
        }

        public ParkCarBitGroup GetParkCarBitGroupByRecordID(string recordId)
        {
            throw new NotImplementedException();
        }
    }
}

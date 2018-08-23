using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;
using Common.Utilities;
using Common.Factory.Park;
using Common.IRepository.Park;
using Common.DataAccess;
using System.Data.Common;
using Common.Entities.Parking;
using System.IO;

namespace Common.Services.Park
{
    public class ParkDerateServices
    {
        /// <summary>
        /// 获取车牌进场信息
        /// </summary>
        /// <param name="PlateNumber"></param>
        /// <param name="VID"></param>
        /// <param name="SellerID"></param>
        /// <returns></returns>
        public static List<ParkIORecord> GetPKIORecordByPlateNumber(string PlateNumber, string VID, string SellerID, string imagePath)
        {
            List<ParkIORecord> list = new List<ParkIORecord>();

            using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
            {
                string strsql = string.Format(@"select a.*,b.PKName  from ParkIORecord a left join BaseParkinfo b on a.parkingid=b.PKID 
                            left join ParkCarType d on a.CarTypeID=d.CarTypeID where IsExit=0 and b.VID=@VID and a.DataStatus<2 and b.DataStatus<2 
                            and a.PlateNumber like '%" + PlateNumber + "%' and (d.BaseTypeID=1 or d.BaseTypeID=3) ");
                dbOperator.ClearParameters();
                dbOperator.AddParameter("VID", VID);

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
                        if (Directory.Exists(imagePath))
                        {
                            if (File.Exists(imagePath + "\\" + model.EntranceImage))
                            {
                                try
                                {
                                    MemoryStream mstream = new MemoryStream(File.ReadAllBytes(imagePath + "\\" + model.EntranceImage));
                                    model.InimgData = System.Convert.ToBase64String(mstream.ToArray());
                                }
                                catch
                                {
                                }
                            }
                        }
                        list.Add(model);
                    }
                   
                }
            }
            return list;
        }
        public static bool Add(ParkDerate model) {
            if (model == null) throw new ArgumentNullException("model");

            model.DerateID = GuidGenerator.GetGuidString();
            if (model.DerateIntervar != null && model.DerateIntervar.Count > 0) {
                model.DerateIntervar.ForEach(p => { p.DerateIntervarID = GuidGenerator.GetGuidString(); p.DerateID = model.DerateID; });
            }
            IParkDerate factory = ParkDerateFactory.GetFactory();
            bool result = factory.Add(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkDerate>(model, OperateType.Add);
            }
            return result;
        }

       public static bool Update(ParkDerate model)
        {
            if (model == null) throw new ArgumentNullException("model");

            if (model.DerateIntervar != null && model.DerateIntervar.Count > 0)
            {
                model.DerateIntervar.ForEach(p => { p.DerateIntervarID = GuidGenerator.GetGuidString(); p.DerateID = model.DerateID; });
            }
            IParkDerate factory = ParkDerateFactory.GetFactory();
            bool result = factory.Update(model);
            if (result)
            {
                OperateLogServices.AddOperateLog<ParkDerate>(model, OperateType.Update);
            }
            return result;
        }

       public static bool Delete(string derateId)
       {
           if (derateId.IsEmpty()) throw new ArgumentNullException("derateId");

           IParkDerate factory = ParkDerateFactory.GetFactory();
           bool result = factory.Delete(derateId);
           if (result)
           {
               OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("derateId:{0}", derateId));
           }
           return result;
       }

       public static bool DeleteBySellerID(string sellerId)
       {
           if (sellerId.IsEmpty()) throw new ArgumentNullException("sellerId");

           IParkDerate factory = ParkDerateFactory.GetFactory();
           bool result = factory.DeleteBySellerID(sellerId);
           if (result)
           {
               OperateLogServices.AddOperateLog(OperateType.Delete, string.Format("sellerId:{0}", sellerId));
           }
           return result;
       }

       public static List<ParkDerate> QueryBySellerID(string sellerId)
       {
           if (sellerId.IsEmpty()) throw new ArgumentNullException("sellerId");

           IParkDerate factory = ParkDerateFactory.GetFactory();
           return factory.QueryBySellerID(sellerId);
       }

       public static ParkDerate Query(string derateId)
       {
           if (derateId.IsEmpty()) throw new ArgumentNullException("derateId");

           IParkDerate factory = ParkDerateFactory.GetFactory();
           return factory.Query(derateId);
       }

       public static List<ParkDerateIntervar> QueryParkDerateIntervar(string derateId)
       {
           if (derateId.IsEmpty()) throw new ArgumentNullException("derateId");

           IParkDerate factory = ParkDerateFactory.GetFactory();
           return factory.QueryParkDerateIntervar(derateId);
       }
       public static decimal GetParkCarDerateSumFreeMoney(string sellerId)
       {
           using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
           {
               string strsql = "select SUM(FreeMoney) as SumFreeMoney from ParkCarDerate a left join ParkDerate b on a.DerateID=b.DerateID  where a.DataStatus<2 and a.Status=1 and b.SellerID=@SellerID ";
               dbOperator.ClearParameters();
               dbOperator.AddParameter("SellerID", sellerId);
               using (DbDataReader reader = dbOperator.ExecuteReader(strsql.ToString()))
               {
                   if (reader.Read())
                   {
                       return reader["SumFreeMoney"].ToDecimal();
                   }
               }
               return 0;
           }
       }
    }
}

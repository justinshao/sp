using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.BWY;
using Common.Factory.BWY;
using Common.IRepository;
using Common.Utilities;
using Common.DataAccess;

namespace Common.Services
{
    public class BWYGateMappingServices
    {
        public static bool Add(BWYGateMapping model) {
            model.RecordID = GuidGenerator.GetGuidString();
            IBWYGateMapping factory = BWYGateMappingFactory.GetFactory();
            return factory.Add(model);
        }

        public static bool Update(BWYGateMapping model)
        {
            IBWYGateMapping factory = BWYGateMappingFactory.GetFactory();
            return factory.Update(model);
        }
        /// <summary>
        /// 修改赛菲姆车场信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpdateSFMParking(List<BWYGateMapping> models)
        {
             IBWYGateMapping factory = BWYGateMappingFactory.GetFactory();
             List<BWYGateMapping> oldModels = factory.QueryByDataSource(1);
            using (DbOperator dbOperator = ConnectionManager.CreateConnection())
            {

                try {
                    dbOperator.BeginTransaction();
                   foreach(var item in models){
                       if (item.DataSource != 1) throw new MyException("数据来源不正确");
                       BWYGateMapping oldModel = oldModels.FirstOrDefault(p => p.ParkingID == item.ParkingID);
                       if (oldModel == null)
                       {
                           item.RecordID = GuidGenerator.GetGuidString();
                           bool result = factory.Add(item);
                           if (!result) throw new MyException("添加车场信息失败");
                       }
                       else
                       {
                           oldModel.ParkingName = item.ParkingName;
                           bool result = factory.Update(oldModel);
                           if (!result) throw new MyException("修改车场信息失败");
                       }
                   }
                   dbOperator.CommitTransaction();
                   return true;
                }catch{
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
          
        }
        public static bool Delete(string recordId)
        {
            IBWYGateMapping factory = BWYGateMappingFactory.GetFactory();
            return factory.Delete(recordId);
        }

        public static BWYGateMapping QueryByRecordId(string recordId)
        {
            IBWYGateMapping factory = BWYGateMappingFactory.GetFactory();
            return factory.QueryByRecordId(recordId);
        }

        public static List<BWYGateMapping> QueryByParkingID(string parkingId)
        {
            IBWYGateMapping factory = BWYGateMappingFactory.GetFactory();
            return factory.QueryByParkingID(parkingId);
        }

        public static BWYGateMapping QueryByGateID(string gateId)
        {
            IBWYGateMapping factory = BWYGateMappingFactory.GetFactory();
            return factory.QueryByGateID(gateId);
        }
        public static List<BWYGateMapping> QueryAll() {
            IBWYGateMapping factory = BWYGateMappingFactory.GetFactory();
            return factory.QueryAll();
        }
        public static List<BWYGateMapping> QueryPage(string parkName, string gateName, int? dataSource, int pageIndex, int pageSize, out int recordTotalCount)
        {
            IBWYGateMapping factory = BWYGateMappingFactory.GetFactory();
            return factory.QueryPage(parkName, gateName, dataSource, pageIndex, pageSize, out recordTotalCount);
        }
    }
}

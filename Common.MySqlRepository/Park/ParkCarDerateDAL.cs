using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.IRepository.Park;
using Common.Entities;
using Common.DataAccess;

namespace Common.MySqlRepository
{
    public class ParkCarDerateDAL : BaseDAL, IParkCarDerate
    {

        public ParkCarDerate Add(ParkCarDerate model)
        {
            throw new NotImplementedException();
        }

        public bool Update(ParkCarDerate model)
        {
            throw new NotImplementedException();
        }

        public List<ParkCarDerate> QueryByDerateId(string derateId)
        {
            throw new NotImplementedException();
        }

        public List<ParkCarDerate> QueryByPlateNumber(string plateNumber)
        {
            throw new NotImplementedException();
        }

        public List<ParkCarDerate> QueryByCardNo(string cardNo)
        {
            throw new NotImplementedException();
        }

        public List<ParkCarDerate> QueryByIORecordID(string ioRecordId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteByExpiryTime(string derateId, DateTime expiredTime)
        {
            throw new NotImplementedException();
        }

        bool IParkCarDerate.Add(ParkCarDerate model)
        {
            throw new NotImplementedException();
        }

        public bool UpdateStatus(string carDerateID, CarDerateStatus status)
        {
            throw new NotImplementedException();
        }

        public ParkCarDerate GetNotUseParkCarDerate(string derateId, DateTime lessThanTime)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCarderateCreateTime(string carDerateID)
        {
            throw new NotImplementedException();
        }

        public ParkCarDerate QueryByCarDerateID(string carDerateId)
        {
            throw new NotImplementedException();
        }

        public List<ParkCarDerate> ParkCarDeratePage(string sellerId, string plateNumber, int? state, int? derateType, DateTime? start, DateTime? end, int pageSize, int pageIndex, ref int totalCount)
        {
            throw new NotImplementedException();
        }

        public bool Add(ParkCarDerate model, DbOperator dbOperator)
        {
            throw new NotImplementedException();
        }

        public bool QRCodeDiscount(string carDerateid, string parkingId, string ioRecordId, string plateNumber)
        {
            throw new NotImplementedException();
        }

        public bool DeleteNotUseByDerateQRCodeID(string derateQRCodeId, DbOperator dbOperator)
        {
            throw new NotImplementedException();
        }

        public ParkCarDerate QueryBySellerIdAndIORecordId(string sellerId, string ioReocdId)
        {
            throw new NotImplementedException();
        }

        public decimal GetTotalFreeMoney(string sellerId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, int> QuerySettlementdCarDerate(List<string> derateQRCodeIds)
        {
            throw new NotImplementedException();
        }
    }
}

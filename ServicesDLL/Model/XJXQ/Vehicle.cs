using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.XJXQ
{
    public class Vehicle
    {
        public string ver { set; get; }
        public string id { set; get; }
        public string passTime { set; get; }
        public string plateNum { set; get; }
        public string plateColor { set; get; }
        public string vehicleType { set; get; }
        public string vehicleImgUrl { set; get; }
        public string plateImgUrl { set; get; }
        public string areaCode { set; get; }
        public string x { set; get; }
        public string y { set; get; }
        public string equipmentId { set; get; }
        public string equipmentName { set; get; }
        public string equipmentType { set; get; }
        public string stationId { set; get; }
        public string stationName { set; get; }
        public string backUrl { set; get; }
        public string location { set; get; }
        public string vehiclecolor { set; get; }
        public string dareaname { set; get; }
        public string dareacode { set; get; }
        public string cartype { set; get; }
        public string placetype { set; get; }
        public string status { set; get; }
        public List<Visitor> visitor { set; get; }
        public List<DriverData> driverData { set; get; }
        public List<PassengerData> passengerData { set; get; }

    }
}

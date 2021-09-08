using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class HouseKeepingDTO
    {

        private string _AccountID;
        private string _TenantName;
        private string _TenantID;
        private string _Warehouse;
        private string _WarehouseId;
        private string _CartonNo;
        private string _Result;
        private string _UserId;


        public string AccountID { get => _AccountID; set => _AccountID = value; }
        public string TenantName { get => _TenantName; set => _TenantName = value; }
        public string TenantID { get => _TenantID; set => _TenantID = value; }
        public string Warehouse { get => _Warehouse; set => _Warehouse = value; }
        public string WarehouseId { get => _WarehouseId; set => _WarehouseId = value; }
        public string CartonNo { get => _CartonNo; set => _CartonNo = value; }
        public string Result { get => _Result; set => _Result = value; }
        public string UserId { get => _UserId; set => _UserId = value; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class Warehouse
    {
        private int _WarehouseID;

        private string _WarehouseCode;
        private string _WarehouseName;

        public int WarehouseID { get => _WarehouseID; set => _WarehouseID = value; }
        public string WarehouseCode { get => _WarehouseCode; set => _WarehouseCode = value; }
        public string WarehouseName { get => _WarehouseName; set => _WarehouseName = value; }
    }
}

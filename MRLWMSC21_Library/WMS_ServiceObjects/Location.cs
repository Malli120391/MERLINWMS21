using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_ServiceObjects
{
    public class Location
    {
        public string LocationCode { get; set; }
        public string Zone { get; set; }
        public string Type { get; set; }
        public int TypeId { get; set; }
        public decimal Volume { get; set; }
        public decimal MaxWeight { get; set; }
        public decimal IsMixedMaterial { get; set; }
        public int LocationId { get; set; }


    }

    public class LocationOccupancy 
    {
        public int LocationID { get; set; }
        public string LocationCode { get; set; }
        public decimal Volume { get; set; }
        public decimal MaxWeight { get; set; }
        public decimal VolumeOccupancy { get; set; }
        public decimal WeightOccupancy { get; set; }
        public decimal VolOccupiedPercentage { get; set; }
        public decimal WeightHoldPercentage { get; set; }

    }


    public class WarehouseZone 
    {
        public int WarehouseId { get; set; }
        public string WarehouseCode { get; set; }
        public string ZoneDescription { get; set; }
        public int ZoneId { get; set; }
        public string ZoneCode { get; set; }
        public int WarehouseGroupId { get; set; }
    }
}

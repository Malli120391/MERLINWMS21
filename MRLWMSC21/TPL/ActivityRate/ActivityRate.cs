using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.TPL.ActivityRate
{


    public class RateGroup
    {
        public int rateGroupID { get; set; }
        public string rateGroupName { get; set; }
        public IDictionary<int, RateGroupType> groupActivityTypes { get; set; }
    }

    public class RateGroupType
    {

        public int rateTypeID { get; set; }
        public string rateTypeName { get; set; }
        public IList<RateInfo> rates { get; set; }

    }

    public class RateInfo
    {

        public int rateID { get; set; }
        public string rateName { get; set; }
        public int UomID { get; set; }
        public decimal price { get; set; }
        public int currencyID { get; set; }
        public decimal discount { get; set; }
        public string currencyCode { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public byte isDiscountRate { get; set; }
        public string UOM { get; set; }
        public int GroupID { get; set; }
        public int RateTypeID { get; set; }
        public int rateAllocationID { get; set; }

    }

}
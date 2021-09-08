using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_DBCommon
{
    public class DBCriteria
    {
        
        public IDictionary<string, object> INVOICE_LIST_BY_INBOUND(CommonCriteria CRITERIA)
        {
            return new Dictionary<string, object>
            {
                {"INBOUNDID",CRITERIA.INBOUNDID}
            };
        }


        public IDictionary<string, object> MATERIAL_BIN_INFO_BY_MATERIALID(CommonCriteria CRITERIA)
        {

            return new Dictionary<string, object>
            {
                {"MATERIALID",CRITERIA.MATERIALID},
                {"MATERIAL_BIN_RESULT",(CRITERIA.MATERIAL_BIN)?1:0},
                {"TENANTID",CRITERIA.TENANTID},
                {"NEAR_BY_LOC",0}
            };
        }

        public IDictionary<string, object> LOCATION_OCCUPANCY_BY_ID(CommonCriteria CRITERIA)
        {

            return new Dictionary<string, object>
            {
                {"MATERIALID",0},
                {"MATERIAL_BIN_RESULT",0},
                {"TENANTID",CRITERIA.TENANTID},
                {"LOCATIONID",CRITERIA.LOCATIONID},
                {"LOCATION_ZONE",CRITERIA.LOCATION_LIKE_CODE+"%"},
                {"NEAR_BY_LOC",1}
            };
        }

        public IDictionary<string, object> LOCATION_LIST_BY_ZONECODE(CommonCriteria CRITERIA)
        {
            return new Dictionary<string, object>
            {
                {"MATERIALID",0},
                {"MATERIAL_BIN_RESULT",0},
                {"TENANTID",CRITERIA.TENANTID},
                {"LOCATIONID",CRITERIA.LOCATIONID},
                {"LOCATION_ZONE",CRITERIA.LOCATION_LIKE_CODE+"%"},
                {"SUPPLIERID",CRITERIA.SUPPLIERID},
                {"NEAR_BY_LOC",1}
            };
        }

        public IDictionary<string, object> MATERIAL_INWARD_OUTWARD_BY_ID(CommonCriteria CRITERIA)
        {
            return new Dictionary<string, object>
            {
                {"MATERIALID",CRITERIA.MATERIALID},
                {"MATERIAL_STAT",1},
                {"ORDER_STAT",0}
            };
        }

        public IDictionary<string, object> MATERIAL_OUTWARD_BY_ID(CommonCriteria CRITERIA)
        {
            return new Dictionary<string, object>
            {
                {"MATERIALID",CRITERIA.MATERIALID},
                {"MATERIAL_STAT",0},
                {"ORDER_STAT",1}
            };
        }

        public IDictionary<string, object> DELIVERY_PICK_NOTE(CommonCriteria CRITERIA)
        {
            return new Dictionary<string, object>
            {
                {"OUTBOUNDID",CRITERIA.OUTBOUNDID},
                {"MATERIALCODE",CRITERIA.MATERIALCODE},
            };
        }


        public IDictionary<string, object> SUGGESTED_LOCATION_OCCUPANCY(CommonCriteria CRITERIA)
        {
            return new Dictionary<string, object>
            {
                {"LOGICAL_OCCUPANCY",CRITERIA.LOGICAL_OCCUPANCY},
                {"INBOUNDID",CRITERIA.INBOUNDID},
                {"TENANTID",CRITERIA.TENANTID},
                {"SUPPLIERID",CRITERIA.SUPPLIERID}

            };
        }
          
     


    }

    public class CommonCriteria
    {

        public CommonCriteria()
        {
            INBOUNDID = 0;
            LOCATIONID = 0;
            TENANTID = 0;
            MATERIALID = 0;
            SUPPLIERID = 0;
            OUTBOUNDID = 0;
            LOGICAL_OCCUPANCY = 1;
        }

        public int LOGICAL_OCCUPANCY { get; set; }
        public int OUTBOUNDID { get; set; }
        public string MCODE { get; set; }
        public int MATERIALID { get; set; }
        public int INBOUNDID { get; set; }
        public int TENANTID { get; set; }
        public int LOCATIONID { get; set; }
        public bool NEAR_BY_LOC { get; set; }
        public bool MATERIAL_BIN { get; set; }
        public string LOCATION_LIKE_CODE { get; set; }
        public int SUPPLIERID { get; set; }
        public string MATERIALCODE { get; set; }


    }
}

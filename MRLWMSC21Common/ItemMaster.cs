using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21Common
{
    [Serializable]
    public class ItemMaster
    {
        public String MCode{   set; get; }
        public String CustomerPartNumber { set; get; }
        public String MCodeAlternative1 { set; get; }
        public String MCodeAlternative2 { set; get; }
        public String MDescription { set; get; }
        public String MDescriptionLong { set; get; }
        public String SearchText { set; get; }
        public String Remarks { set; get; }
        public String OEMPartNo { set; get; }
        public String UoM { set; get; }
        public String Supplier { set; get; }

        public String MType { get; set; }
        public String MPlant { get; set; }
        public String MGroup { get; set; }
        public String StorageCondition { get; set; }
        public String ProductCategory { get; set; }
     
        public int MTypeID { get; set; }
        public int MPlantID { get; set; }
        public int MGroupID { get; set; }
        public int StorageConditionID { get; set; }
        public int ProductCategoryID { get; set; }
        public int RequestedBy { get; set; }

        public int MLength { get; set; }
        public int MHeight { get; set; }
        public int MWidth { get; set; }
        public int TenantID { get; set; }

        public decimal MWeight { get; set; }

        
                

    }
}

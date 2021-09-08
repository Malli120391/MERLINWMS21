using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.mInventory
{
    public class ReturnMaterialData
    {
        private int  r_KitPlannerID=0, r_LineNumber;
        private Decimal  r_Quantity=0, r_ReturnQty=0;
        private String r_MaterialID;
        private bool r_Checked=false;
        public bool Checked
        {
            get { return r_Checked;}
            set { r_Checked = value; }
        }
        public String MaterialID
        {
            get { return r_MaterialID; }
            set { r_MaterialID = value; }
        }
        public int KitPlannerID
        {
            get { return r_KitPlannerID; }
            set { r_KitPlannerID = value; }
        }
        public Decimal Quantity
        {
            get { return r_Quantity; }
            set { r_Quantity = value; }
        }
        public Decimal ReturnQty
        {
            get { return r_ReturnQty; }
            set { r_ReturnQty = value; }
        }
        public int LineNumber
        {
            get { return r_LineNumber; }
            set { r_LineNumber = value; }
        }
        public void AddQuantity(Decimal quantity)
        {
            r_Quantity += quantity;
        }
        public void AddReturnQty(Decimal quantity)
        {
            r_ReturnQty += quantity;
        }
    }
}
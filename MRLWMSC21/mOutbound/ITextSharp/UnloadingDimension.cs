using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21.mOutbounds
{
    public static class Dimension
    {
        public static int[] SUMMARY_COLUMNS = { 60, 120, 200, 400, 470, 520 };
        public static int[] HEADER_COLUMNS = { 60, 150, 300, 500 };
        public static int[] FOOTER_COLUMNS = { 60, 150, 400, 500 };

        public static int XSTART = 60;
        public static int YSTART = 60;
        public static int YEND = 750;
        public static int YEND1 = 1000;
        public static int XEND = 520;
        public static int LINE_GAP = 12;
        public static int DOUBLE_GAP = 20;

        public static string LINE_SEPARATOR = "----------------------------------------------------------------------------------------------------------------------------------------------------------------";
        public static string TINY_LINE_SEPARATOR = "------------------------------------------------------------------------------------------------------------------------------------------";
        /// <summary>
        /// Author      : Naveen Kumar
        /// Date        : 29/06/2018
        /// Description : Unloading Part 
        /// </summary>
        public static List<String> Kiwi_Headers = new List<string>() { "SNo", "Item Description", "Colour", "MOP", "Pcs", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

        public static Dictionary<String, String> Kiwi_Unload_Headers = new Dictionary<string, string>() { {"LR_NO","Lr No"},
            {"HEADDING","UNLOADING SHEET"}, {"VEHICLE_NO", "Vehicle No"}, {"DATE","Date"},{"ORIGIN","Steller Value Chain Solutions Uran DC"},{"START_TIME","Start Time"},{"END_TIME","End Time"},
            {"TYPE","Type"},{"DOCK_NO","Dock No"} };

        //public static String kiwi_footer_text1 = "*Any complaints regarding quantity or discrepencies against this invoice should be made by return post immediately on receipt of goods. No claim thereafter will be entertained.";
        //public static String kiwi_footer_text2 = "*Goods once sold will not be taken back";
        //public static String kiwi_footer_text3 = "*Received Goods mentioned in this memo are in good condition";
        //public static String kiwi_footer_text4 = "VAT 5% on Slice products,(Vide G.O Ms No 1718 dated 13/09/2011 w.e.f dated 14/09/2011) remaining products VAT @VAT14.5% Subject to Visakhapatnam Jurisdiction";

        /// <summary>
        /// Author      : Naveen Kumar
        /// Date        : 29/06/2018
        /// Description : Unloading sheet Secutiry Part 
        /// </summary>
        //public static List<String> Kiwi_Headers_GST = new List<string>() { "SNo", "Product", "HSN Code", "MRP", "FC", "FB", "Basic Rate", "Basic Amt.", "Disc. Amt.", "Taxable Value", "CGST %", "CGST Amt.", "SGST %", "SGST Amt.", "UGST %", "UGST Amt.", "IGST %", "IGST Amt.", "CESS %", "CESS Amt.", "Total Amt." };

        public static Dictionary<String, String> Kiwi_Unload_Headers_UnloadSecurity = new Dictionary<string, string>() { {"TRANSPORTER","Transporter:"},
            {"HEADDING","STELLER VALUE CHAIN SOLUTIONS LTD"}, {"DATE", "Date:"}, {"DOCK_NO","Dock No:"},{"ORIGIN","UNLOADING SHEET(Secutiry)"},{"DESTINATION","Destination: "},{"TRUCK_NO","Truck No:"},{"UNLOADING_START_TIME","Unloading Start Time:"},
            {"UNLOADING_END_TIME","Unloading End Time:"},{"SEAL_NO","Seal No:"},{"SHIPMENT_NO","Shipment No:"},{"DOCK_IN_TIME","Dock In Time:"},{"DOCK_OUT_TIME","Doc Out Time:"} };

        //public static string kiwi_footer_text_GST = "E & O E";
        //public static string kiwi_footer_text1_GST = "*\"I/We hereby certify that food/foods mentioned in this invoice is/are warranted to be of the nature and quality which it/these purports/ purported to be.\"";
        //public static string kiwi_footer_text2_GST = "*Any complaints regarding quantity or discrepancies against this invoice should be made by return post immediately on receipt of goods. No claim thereafter will be entertained.";
        //public static string kiwi_footer_text3_GST = "*Goods once sold will not be taken back";
        //public static string kiwi_footer_text4_GST = "*Received Goods mentioned in this memo are in good condition";
        //public static string kiwi_footer_text5_GST = "*GST 12% on Juice Based Drinks, Soda & Water 18% & Carbonated Soft Drinks 28% + Cess 12% w.e.f dated 01/07/2017 Subject to Visakhapatnam Jurisdiction";
        //public static string kiwi_footer_text6_GST = "Whether Reverse Charge Applicable (Y/N                   ):No";
        //public static string kiwi_footer_text7_GST = "TERMS AND CONDITIONS :";


        /// <summary>
        /// Author      : Naveen Kumar
        /// Date        : 11/07/2018
        /// Description : Pick List PDF Creation
        /// </summary>

        public static List<String> Kiwi_Headers_Picklist = new List<string>()
        { "Customer Name", "Header Name", "Customer PO", "Shipping date", "Address1" ,"Address2"};

        public static Dictionary<String, String> Kiwi_PickList_Headers_PickLIst = new Dictionary<string, string>()
        { 
            {"HEAD1",""},
            {"HEAD2",""},
            {"HEADDING","PACKING LIST" },
            {"CustPONumber","Customer PO:"},
            {"PSNumber","ORDER NO:" },
            { "Shippingdate", "SHIPPING DATE:"},
            { "Address1","SHIP TO:"},{"Address2","BILL TO:"},
            //    { "WAREHOUSE","Warehouse"},{"DISPATCH_DOCK","Dispatch Dock" }
            //,{"AUTO_GEN","Auto Generate" },{"CUSTOMER","Customer" },{"ORDER_TYPE","Order Type" } };
        };

        //========================== Added New Method for Generate Warehouse Occupancy Report By M.D.Prasad ON 13-Nov-2019 ======================//
        public static Dictionary<String, String> Kiwi_WHOCCData = new Dictionary<string, string>()
        {
            {"HEAD1",""},
            {"HEAD2",""},
            {"HEADDING","Warehouse Occupancy Report" },
            {"WarehouseVolume","WarehouseVolume" },
            {"OccupiedVolume","OccupiedVolume" },
            {"AvailableVolume","AvailableVolume" },
            //    { "WAREHOUSE","Warehouse"},{"DISPATCH_DOCK","Dispatch Dock" }
            //,{"AUTO_GEN","Auto Generate" },{"CUSTOMER","Customer" },{"ORDER_TYPE","Order Type" } };
        };

        //========================== Added New Method for Generate Warehouse Occupancy Report By M.D.Prasad ON 13-Nov-2019 ======================//


        //==================== RCR ===============//
        public static Dictionary<String, String> RCRHeaderFormatData = new Dictionary<string, string>()
        {
            {"HEAD1",""},
            {"HEAD2",""},
            {"HEADDING","Receipt Confirmation Report" },
            {"TenantName","Company" },
            {"Address1","Address" },
            {"ShipmentType","Shipment Type" },

            {"VehicleRegistrationNo","Truck No./TrallerID" },
            {"StoreRefNo","Receipt No." },
            {"ShipmentReceivedOn","Document Date" },
        };
        //============== END =================//

        //==================== WHStock ===============//
        public static Dictionary<String, String> WHStockHeaderFormatData = new Dictionary<string, string>()
        {
            {"HEAD1",""},
            {"HEAD2",""},
            {"HEADDING","Warehouse Stock Information Report" },
            {"TenantName","Company" },
            {"Address1","Address" },
            {"ContactPerson","Contact Person" },

            {"EmailID","Email ID" },
            {"DocDate","Doc. Date" },
            {"ShipmentReceivedOn","Document Date" },
        };
        //============== END =================//

        //========================== Added New Method for Generate Warehouse Occupancy Report By M.D.Prasad ON 13-Nov-2019 ======================//
        public static Dictionary<String, String> Kiwi_BillingData = new Dictionary<string, string>()
        {
            {"HEAD1",""},
            {"HEAD2",""},
            {"HEADDING","Billing Report" },
        };
        //========================== Added New Method for Generate Warehouse Occupancy Report By M.D.Prasad ON 13-Nov-2019 ======================//

        public static class NUMBER_WORD
        {
            public static string[] TENS_NUM = { "", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY", "HUNDRED" };
            public static string[] DIGITS = { "", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN" };
            public static string[] TEN_TWENTY_BETWEEN = { "", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINTEEN" };
        }


    }
}

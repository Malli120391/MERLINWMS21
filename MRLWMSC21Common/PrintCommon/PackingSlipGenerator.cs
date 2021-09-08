using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21Common.PrintCommon
{
    

    public class PackingSlipGenerator
    {        
        public PackingData GeneratePackingSlip(string OBDId)
        {
            DataSet ds = new DataSet();
            ds = DB.GetDS("EXEC [dbo].[Get_OBD_DeliveryPackSlip] " + OBDId, false);

            PackingData PD = new PackingData();
            List<PackingSlip> lstBoxSlips = new List<PackingSlip>();
            List<BoxItemDetails> lstBoxDetails = null;
            PackingSlip PS = null;
            //int CurBoxNo = 0;
            //int PrevBoxNo=-1;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //CurBoxNo = Convert.ToInt32(ds.Tables[0].Rows[i]["BoxNo"].ToString());


                //if (PrevBoxNo != CurBoxNo)
                //{
                   // PrevBoxNo = CurBoxNo;
                    PS = new PackingSlip();
                    PS.CustomerName = ds.Tables[0].Rows[i]["CustomerName"].ToString();
                   // PS.CityName = ds.Tables[0].Rows[i]["City"].ToString();
                    PS.SiteCode = ds.Tables[0].Rows[i]["CustomerCode"].ToString();
                    PS.OBDNumber = ds.Tables[0].Rows[i]["OBDNumber"].ToString();
                    PS.VehicleNo= ds.Tables[0].Rows[i]["VehicleNo"].ToString();
                    PS.VehicleType= ds.Tables[0].Rows[i]["VehicleType"].ToString();
                    PS.DriverName= ds.Tables[0].Rows[i]["DriverName"].ToString();
                    PS.DriverMobileNo= ds.Tables[0].Rows[i]["DriverMobileNo"].ToString();

                    lstBoxDetails = new List<BoxItemDetails>();
                    lstBoxDetails.Add(new BoxItemDetails()
                    {
                            SNo = i,
                            SKUCode = ds.Tables[0].Rows[i]["MCode"].ToString(),
                            Quantity = Convert.ToInt32(ds.Tables[0].Rows[i]["PickedQty"])
                    });
                    PS.BoxItemDetails = lstBoxDetails;
                    lstBoxSlips.Add(PS);
                //}
                //else
                //{
                //    lstBoxDetails.Add(new BoxItemDetails()
                //    {
                //        SNo = i,
                //        SKUCode = ds.Tables[0].Rows[i]["MCode"].ToString(),
                //        Quantity = Convert.ToInt32(ds.Tables[0].Rows[i]["PickedQty"])
                //    });
                                      
                //}
               
                
                PS.BoxItemDetails = lstBoxDetails;
                
            }

            PD.OBDPackingData = lstBoxSlips;
                              
            return PD;
        }

        
    }

    public class PackingData
    {
        public List<PackingSlip> OBDPackingData { set; get; }
    }

    public class PackingSlip
    {
        public string CustomerName { get; set; }
        public string CityName { get; set; }
        public string SiteCode { get; set; }
        public string OBDNumber { get; set; }
        public string BoxNumber { get; set; }
        public int TotalNoOfBoxes { get; set; }
        public string userName { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleType { get; set; }
        public string DriverName { get; set; }
        public string DriverMobileNo { get; set; }
        public List<BoxItemDetails> BoxItemDetails {get;set;}
    }

    public class BoxItemDetails
    {
        public int SNo { get; set; }
        public string SKUCode { get; set; }
        public int Quantity { get; set; }        
    }
}

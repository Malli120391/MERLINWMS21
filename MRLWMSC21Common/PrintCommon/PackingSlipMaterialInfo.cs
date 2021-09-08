using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21Common.PrintCommon
{
    public class PackingSlipMaterialInfo
    {
        public PackingData1 PackingSlipInfo(string PSNHeaderID)
        {
            DataSet ds = new DataSet();
            ds = DB.GetDS("EXEC [dbo].[Get_OBD_DeliveryPackSlip_Material_Info] " + PSNHeaderID, false);
            PackingData1 PD = new PackingData1();
            List<PackingSlip1> lstBoxSlips = new List<PackingSlip1>();
            List<BoxItemDetails1> lstBoxDetails = null;
            PackingSlip1 PS = null;            
            PS.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
            PS.CustPONumber = ds.Tables[0].Rows[0]["CustPONumber"].ToString();
            PS.ShipmentDate = ds.Tables[0].Rows[0]["ShipmentDate"].ToString();
            PS.ItemDesc = ds.Tables[0].Rows[0]["ItemDesc"].ToString();
            PS.HandlingRemarks = ds.Tables[0].Rows[0]["Remarks"].ToString();
            PS.AddressLine1 = ds.Tables[0].Rows[0]["AddressLine1"].ToString();
            PS.Address2 = ds.Tables[0].Rows[0]["Address2"].ToString();
            PS.AccountName= ds.Tables[0].Rows[0]["Account"].ToString();
            //int CurBoxNo = 0;
            //int PrevBoxNo=-1;
            //for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            //
            //    PS = new PackingSlip1();
            //    lstBoxDetails = new List<BoxItemDetails1>();
            //    lstBoxDetails.Add(new BoxItemDetails1()
            //    {
            //        CustomerName = ds.Tables[0].Rows[j]["CustomerName"].ToString(),
            //        CustPONumber = ds.Tables[0].Rows[j]["CustPONumber"].ToString(),
            //        ItemDesc = ds.Tables[0].Rows[j]["ItemDesc"].ToString(),
            //        HandlingRemarks = ds.Tables[0].Rows[j]["Remarks"].ToString(),
            //        AddressLine1 = ds.Tables[0].Rows[j]["AddressLine1"].ToString(),
            //        Address2 = ds.Tables[0].Rows[j]["Address2"].ToString()
            //    });
            //    PS.BoxItemDetails = lstBoxDetails;
            //    lstBoxSlips.Add(PS);
            //    PS.BoxItemDetails = lstBoxDetails;
            //}
            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                PS = new PackingSlip1();
                lstBoxDetails = new List<BoxItemDetails1>();
                lstBoxDetails.Add(new BoxItemDetails1()
                {
                    SNo = i,
                    PackingSlipNo = ds.Tables[1].Rows[i]["PackingSlipNo"].ToString(),
                    MCode = ds.Tables[1].Rows[i]["MCode"].ToString(),
                    ItemDesc= ds.Tables[1].Rows[i]["ItemDesc"].ToString(),
                    HandlingRemarks =ds.Tables[1].Rows[i]["Remarks"].ToString(),
                    PickedQty = ds.Tables[1].Rows[i]["PickedQuantity"].ToString(),
                    UOM = ds.Tables[1].Rows[i]["UOM"].ToString(),
                    PackedQty = ds.Tables[1].Rows[i]["PackedQty"].ToString(),
                    Weight = ds.Tables[1].Rows[i]["ItemWeight"].ToString(),
                    Volume = ds.Tables[1].Rows[i]["ItemVolume"].ToString()
                });
                PS.BoxItemDetails = lstBoxDetails;
                lstBoxSlips.Add(PS);
                PS.BoxItemDetails = lstBoxDetails;

            }

            PD.OBDPackingData = lstBoxSlips;

            return PD;
        }


    }

    public class PackingData1
    {
        public List<PackingSlip1> OBDPackingData { set; get; }
    }

    public class PackingSlip1
    {
        public string CustomerName { get; set; }
        public string AddressLine1 { get; set; }
        public string Address2 { get; set; }
        //Acc Name Added//
        public string AccountName { get; set; }
        public string PSNumber { get; set; }
        public string CustPONumber { get; set; }
        public string ShipmentDate { get; set; }
        public string ProcessNo { get; set; }
        public string Carrier { get; set; }
        public string ItemDesc { get; set; }
        public string HandlingRemarks { get; set; }
        public string TotPickedQty { get; set; }
        public List<BoxItemDetails1> BoxItemDetails { get; set; }
    }

    public class BoxItemDetails1
    {
                public int SNo { get; set; }
                public string PackingSlipNo { get; set; }
                public string MCode { get; set; }
                public string ItemDesc { get; set; }
                public string HandlingRemarks { get; set; }
                public string PickedQty { get; set; }
                public string UOM { get; set; }
                public string PackedQty { get; set; }
                public string Weight { get; set; }
                public string Volume { get; set; }
        //public string CustomerName { get; set; }

        //public string AddressLine1 { get; set; }
        //public string Address2 { get; set; }
        //public string CustPONumber { get; set; }
        //public string ProcessNo { get; set; }
        //public string Carrier { get; set; }

        public string Qty { get; set; }

        public string ReqQty { get; set; }
    }
}

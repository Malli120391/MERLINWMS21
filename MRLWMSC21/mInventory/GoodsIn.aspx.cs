using MRLWMSC21Common;
using MRLWMSC21.mOutbound.BL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;

namespace MRLWMSC21.mInventory
{
    public partial class GoodsIn : System.Web.UI.Page
    {
        public static CustomPrincipal cp;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Outbound";
        }
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Goods Receipt (IN)");

            }
        }
        //--------------------- added by durga to get QC params on 05/03/2018 -------------------------//
        [WebMethod]
        public static List<QualityParams> GetQCParameters(string MMID)
        {
            return new GoodsIn().getQCParameters(MMID);
        }

        [WebMethod]
        public static List<DropDownList> GetStoreRefInfo(int TenantID, string Prefix)
        {
            return new GoodsIn().GetStoreRefNumber(TenantID, Prefix);
        }

        [WebMethod]
        public static List<DropDownList> GetSKUS(int InboundID)
        {
            return new GoodsIn().GetGoodsItems(InboundID);
        }
        [WebMethod]
        public static List<QCValues> GetQCvalue(int GMID)
        {
            return new GoodsIn().GetQCValues(GMID);
        }

        [WebMethod]
        public static List<DropDownList> Printer()
        {
            return new GoodsIn().GetPrinters();
        }
        [WebMethod]
        public static List<DropDownList> locations(string Prefix = "", string InboundID = "")
        {
            return new GoodsIn().Getlocation(Prefix, InboundID);
        }
        
        [WebMethod]
        public static List<DropDownList> Label()
        {
            return new GoodsIn().GetLabelsizes();
        }
        [WebMethod]
        public static List<DropDownList> SLOCs()
        {
            return new GoodsIn().GetSLocs();
        }

        [WebMethod]
        public static List<DropDownList> Containers(int INBID, string Prefix, string Location)
        {
            return new GoodsIn().GetContainers(INBID, Prefix, Location);
        }

        [WebMethod]
        public static List<DropDownList> location(string Prefix = "", string InboundID = "")
        {
            return new GoodsIn().Getlocation( Prefix, InboundID);
        }
        [WebMethod]
        public static List<DropDownList> GetNonConformityLocationFor3PL(string Prefix, int IsNonConformity, string InboundID)
        {
            return new GoodsIn().GetNonConformityLocationsFor3PL(Prefix,IsNonConformity,InboundID);
        }
        
        [WebMethod]
        public static List<SuggestedPutwayList> GetSuggestedList(int InboundID, int MaterialMasterID, int POHeaderID, string LineNumber,int SIID,int SIDetID)
        {
            return new GoodsIn().GetSuggestedPutawaylist(InboundID,MaterialMasterID, POHeaderID, LineNumber, SIID, SIDetID);
        }
        
     [WebMethod]
        public static List<QCList> GetQCList(int GMID)
        {
            return new GoodsIn().GetQCdetailsList(GMID);
        }

        [WebMethod]
        public static List<QCPending> GetQCPendinglist(int InboundID, int MaterialMasterID, int POHeaderID, string LineNumber, int SIID,int SIDetID)
        {
            return new GoodsIn().GetQCPendingList(InboundID, MaterialMasterID, POHeaderID, LineNumber, SIID, SIDetID);
        }
        
        [WebMethod]
        public static List<ErrorCode> GetEoorList (int InboundID)
        {
            return new GoodsIn().GetSuggestedErrorlist(InboundID);
        }

        [WebMethod]
        public static List<DropDownList> GetShipReason()
        {
            return new GoodsIn().GetShipReasonList();
        }
        //--------------------- added by durga to get QC params on 05/03/2018 -------------------------//
        public List<QualityParams> getQCParameters(string MMID)
        {
            List<QualityParams> lst = new List<QualityParams>();
            try
            {
                string Query = "[dbo].[sp_MFG_GetQcMaterialStorageParameterForMaterial] @MaterialMasterID=" + MMID;
                DataSet DS = DB.GetDS(Query, false);
                if (DS.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        lst.Add(new QualityParams()
                        {
                            ParameterName = row["ParameterName"].ToString(),
                            ParameterDataType = row["ParameterDataType"].ToString(),
                            ControlType = row["ControlType"].ToString(),
                            QualityParameterID = row["QualityParameterID"].ToString(),
                            IsRequired = Convert.ToBoolean(row["IsRequired"]),
                            MinTolerance = Convert.ToDecimal(row["MinTolerance"]),
                            MaxTolerance = Convert.ToDecimal(row["MaxTolerance"])
                        });
                    }
                }
                return lst;
            }
            catch (Exception e)
            {
                return null;
            }
           


        }

        public List<DropDownList> GetStoreRefNumber(int TenantID, string Prefix)
        {
            List<DropDownList> lst = new List<DropDownList>();
            string Query = "SELECT StoreRefNo, InboundID FROM INB_Inbound WHERE TenantID =  "+ TenantID + " AND StoreRefNo LIKE '%"+ Prefix + "%' AND InboundStatusID > 2 ";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["InboundID"]);
                    DDl.Name = row["StoreRefNo"].ToString();
                    lst.Add(DDl);
                }
            }
            return lst;
        }

      


        public List<DropDownList> GetSLocs()
        {
            List<DropDownList> lst = new List<DropDownList>();
            string Query = "  SELECT Id, Code FROM StorageLocation WHERE Id IN(3,4,5) ";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["Id"]);
                    DDl.Name = row["Code"].ToString();
                    lst.Add(DDl);
                }
            }
            return lst;
        }

        public List<DropDownList> Getlocation(string Prefix = "", string InboundID = "")
        {
            if (InboundID.Length == 0)
                InboundID = "0";
            // string _sInboundID = Request.QueryString["ibdno"].ToString();
            List<DropDownList> lst = new List<DropDownList>();
            string Query = "  SELECT TOP 15 LocationID, Location FROM INV_Location AS LM INNER JOIN INV_LocationZone AS LZ ON LM.ZoneId = LZ.LocationZoneID WHERE LZ.WarehouseID IN (SELECT DISTINCT WarehouseID FROM INB_RefWarehouse_Details AS ITW WHERE ITW.InboundID = (CASE WHEN " + InboundID + " = 0 THEN ITW.InboundID ELSE " + InboundID + " END)) AND Location LIKE '%" + Prefix + "%' ";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["LocationID"]);
                    DDl.Name = row["Location"].ToString();
                    lst.Add(DDl);
                }
            }
            return lst;
        }



        public List<DropDownList> GetGoodsItems(int InboundID)
        {
            List<DropDownList> lst = new List<DropDownList>();

            string Query = "EXEC [dbo].[GET_ITEMS_GOONDIN] @InboundID = "+ InboundID + " ";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["MaterialMasterID"]);
                    DDl.Name = row["Code"].ToString();
                    DDl.LineNumber = row["LineNumber"].ToString();
                    DDl.PODetailsID = Convert.ToInt32(row["PODetailsID"]);
                    DDl.POHeaderID = Convert.ToInt32(row["POHeaderID"]);
                    lst.Add(DDl);
                }

            }
            return lst;

        }
        public List<DropDownList> GetPrinters()
        {
            List<DropDownList> lst = new List<DropDownList>();

            string Query = "select CR.ClientResourceName,CR.DeviceIP from GEN_ClientResource CR JOIN  GEN_DeviceModel DM ON DM.DeviceModelID=CR.DeviceModelID AND DM.IsActive=1 AND DM.IsDeleted=0 JOIN GEN_DeviceType DT ON DT.DeviceTypeID=DM.DeviceTypeID AND DT.IsActive=1 AND DT.IsDeleted=0 where CR.IsActive=1 AND CR.IsDeleted=0 AND DM.DeviceTypeID=3";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.PrinterIP = (row["DeviceIP"]).ToString();
                    DDl.Name = row["ClientResourceName"].ToString();
                    lst.Add(DDl);
                }

            }
            return lst;

        }

        public List<DropDownList> GetLabelsizes()
        {
            List<DropDownList> lst = new List<DropDownList>();

            string Query = "select TenantBarcodeTypeID,convert(varchar(50),[Length])+' * '+convert(varchar(50),[Width])+' ('+convert(varchar(100),DPI)+')' LB,Length,Width,DPI from TPL_Tenant_BarcodeType where IsActive=1";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["TenantBarcodeTypeID"]);
                    DDl.Name = row["LB"].ToString();
                    lst.Add(DDl);
                }

            }
            return lst;

        }

        public List<DropDownList> GetContainers(int INBID,string prefix, string Location)
        {
            List<DropDownList> lst = new List<DropDownList>();

            string Query = "EXEC [dbo].[sp_GET_Loction_CartonList] @Location = "+DB.SQuote(Location)+ ", @InboundID = "+INBID+ " , @Prefix = "+DB.SQuote(prefix)+" ";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["CartonID"]);
                    DDl.Name = row["CartonCode"].ToString();
                    lst.Add(DDl);
                }
            }
            return lst;
        }

        [WebMethod]   
        public List<DropDownList> GetNonConformityLocationsFor3PL(string Prefix, int IsNonConformity, string InboundID)
        {
            List<DropDownList> lst = new List<DropDownList>();          

            string Query = "";

            if (IsNonConformity == 1)
            {
                Query = "select top 20 Location,LocationID from INV_Location where left(location,2)='Q1' and IsDeleted=0 and location like '" + Prefix + "%'";
            }
            else
            {
                Query = "SELECT top 20 Location,LocationID from INV_Location LOC JOIN INB_Inbound IBT ON IBT.TenantID=LOC.TenantID AND IBT.SupplierID=LOC.SupplierID AND IBT.IsDeleted=0 AND IBT.IsActive=1 WHERE (left(LOC.Location,2) !='Q1' AND LOC.IsDeleted=0 AND IBT.InboundID=" + InboundID + ")  and location like '" + Prefix + "%'";
            }

            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["LocationID"]);
                    DDl.Name = row["Location"].ToString();
                    lst.Add(DDl);
                }

            }
            return lst;
        }

        public List<DropDownList> GetShipReasonList()
        {
            List<DropDownList> lst = new List<DropDownList>();

            string Query = "SELECT Reason, ReasonID FROM GEN_SkipReason WHERE ReasonID <> 2 AND IsActive = 1 AND IsDeleted = 0";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["ReasonID"]);
                    DDl.Name = row["Reason"].ToString();
                    lst.Add(DDl);
                }
            }
            return lst;
        }

        public List<IsRquired> Getmandatory(int MMID)
        {
          
            List<IsRquired> lst = new List<IsRquired>();
            string Query = "select mm.MaterialMaster_MaterialStorageParameterID,mm.MaterialMasterID,mm.IsRequired,mm.MaterialStorageParameterID,msp.ParameterName from MMT_MaterialMaster_MSP mm join MMT_MSP msp on mm.MaterialStorageParameterID = msp.MaterialStorageParameterID where mm.MaterialMasterID=" + MMID;
            DataSet DS = new DataSet();
            DS = DB.GetDS(Query, false);
            if (DS.Tables.Count != 0 && DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    IsRquired SPL = new IsRquired();
                    SPL.MaterialStorageParameterID = Convert.ToInt32(row["MaterialStorageParameterID"]);
                    SPL.ParameterName = row["ParameterName"].ToString();
                    SPL.MaterialMasterID = Convert.ToInt32(row["MaterialMasterID"]);
                    SPL.IsRequired =Convert.ToBoolean(row["IsRequired"].ToString());

                    lst.Add(SPL);
                }
            }
            return lst;
        }
        [WebMethod]
        public static string GetParameters(int GMID)
        {
            string Json = "";
            try
            {
                DataSet ds = DB.GetDS("EXEC [dbo].[sp_ITEM_QC_PARAMATERS] @GoodsMovementDetailsID="+GMID, false);
                Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;
        }
        

        public List<QCValues> GetQCValues(int GMID)
        {
            List<QCValues> lst = new List<QCValues>();
            string Query = "EXEC [dbo].[sp_INV_GetGoodsMovementDetailsForQcValues_NEW] @GoodsMovementDetailsID = "+ GMID + "";
            DataSet DS = new DataSet();
            DS = DB.GetDS(Query, false);
            if (DS.Tables.Count != 0 && DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    QCValues SPL = new QCValues();
                    SPL.Location = row["Location"].ToString();
                    SPL.IsDamaged = row["Is Damaged"].ToString();
                    SPL.HasDiscrepancy = row["Has Discrepancy"].ToString();
                    SPL.AsIs = row["As Is"].ToString();
                    SPL.IsNonConfirmity = row["Is Non Conformity"].ToString();
                    SPL.MfgDate = row["MfgDate"].ToString();
                    SPL.ExpDate = row["ExpDate"].ToString(); ;
                    SPL.BatchNo = row["BatchNo"].ToString();
                    SPL.SerialNo = row["SerialNo"].ToString();
                    SPL.ProjectRefNo = row["ProjectRefNo"].ToString();
                    lst.Add(SPL);
                }
            }
            return lst;
        }

        public List<QCList> GetQCdetailsList(int GMID)
        {

            List<QCList> lst = new List<QCList>();
            string Query = "EXEC [dbo].[sp_INV_GetQCDetailsList] @GoodsmovementDeatailsID = " + GMID + "";
            DataSet DS = new DataSet();
            DS = DB.GetDS(Query, false);
            if (DS.Tables.Count != 0 && DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    QCList SPL = new QCList();
                    SPL.GoodsMovementDetailsID = string.IsNullOrEmpty(row["GoodsMovementDetailsID"].ToString()) ? 0 : Convert.ToInt32(row["GoodsMovementDetailsID"]);
                    SPL.QCSerialNo = row["QCSerialNo"].ToString();
                    SPL.Quantity = row["Quantity"].ToString();  
                    SPL.QCParamCaptureID= row["QualityParametersCaptureID"].ToString() == ""? 0: Convert.ToInt32(row["QualityParametersCaptureID"]);
                    SPL.SerailName= row["Serialname"].ToString();
                    SPL.totalQty = Convert.ToInt32(row["totalQty"]);
                    SPL.pendingQty = Convert.ToInt32(row["pendingQty"]);
                    SPL.completedQty = Convert.ToInt32(row["completedQty"]);
                    SPL.Isselect = false;
                    SPL.paramvalues = row["QCSerialNo"].ToString() == "" ? null : getQualityparamvalues(DS.Tables[1], row["QCSerialNo"].ToString());


                    lst.Add(SPL);
                }
            }
            return lst;
        }
        public List<QualityParamValues> getQualityparamvalues(DataTable dt,string serailno)
        {
            List<QualityParamValues> lst = new List<QualityParamValues>();
            foreach (DataRow row in dt.Rows)
            {
                if(Convert.ToInt32(row["QCSerialNo"])==Convert.ToInt32(serailno))
                {
                    lst.Add(new QualityParamValues()
                    {
                        QCSerialNo= Convert.ToInt32(row["QCSerialNo"]),
                        QualityParameterID= Convert.ToInt32(row["QualityParameterID"]),
                        value= row["value"].ToString()

                    });

                }
              
            }
                return lst;
        }

        public List<SuggestedPutwayList> GetSuggestedPutawaylist(int InboundID, int MaterialMasterID, int POHeaderID, string LineNumber,int SIID,int SupplierInvoicedetailsID)
        {
            List<SuggestedPutwayList> lst = new List<SuggestedPutwayList>();
            string Query = "EXEC [dbo].[GET_SUGGESTEDPUTAWAYLIST]  @InboundID= "+InboundID+ ", @POHeaderID= "+POHeaderID+ ", @LineNumber = "+LineNumber+ ", @MaterialMasterID="+MaterialMasterID+ ", @SupplierInvoiceID="+SIID+ ",@SupplierInvoiceDetailsID="+SupplierInvoicedetailsID;
            DataSet DS = new DataSet();
            DS = DB.GetDS(Query, false);
            if(DS.Tables.Count != 0 && DS.Tables[0].Rows.Count != 0)
            {
                foreach(DataRow row in DS.Tables[0].Rows)
                {
                    SuggestedPutwayList SPL = new SuggestedPutwayList();
                    SPL.SuggestedPutawayID = Convert.ToInt32(row["SuggestedPutawayID"]);
                    SPL.SuggestedQty = Convert.ToDecimal(row["SuggestedQty"]);
                    SPL.ReceivedQty = Convert.ToDecimal(row["ReceivedQuantity"]);
                    SPL.MaterialMasterID = Convert.ToInt32(row["MaterialMasterID"]);
                    SPL.SuggestedLocationID = string.IsNullOrEmpty(row["SuggestedLocationID"].ToString()) ? 0 : Convert.ToInt32(row["SuggestedLocationID"]);
                    SPL.SupplierInvoiceID = Convert.ToInt32(row["SupplierInvoiceID"]);
                    SPL.Location = row["Location"].ToString();
                    SPL.SupplierInvoice = row["InvoiceNumber"].ToString();
                    SPL.Ponumber = row["PONumber"].ToString();
                    SPL.BatchNo = row["BatchNo"].ToString();
                    SPL.MfgDate = row["MfgDate"].ToString();
                    SPL.ExpDate = row["ExpDate"].ToString();
                    SPL.TenatName = row["TenantName"].ToString();
                    SPL.StorefNumber = row["StoreRefNo"].ToString();
                    SPL.Mcode =  row["Mcode"].ToString();
                    SPL.SerialNumber = row["SerialNo"].ToString();
                    SPL.ProjectRef = row["ProjectRefNo"].ToString();
                    SPL.BUoMQty = Convert.ToDecimal(row["BuoMQty"]);
                    SPL.MRP = row["MRP"].ToString();
                    SPL.HUNo = Convert.ToInt32(row["HUNo"].ToString());
                    SPL.HUSize = Convert.ToInt32(row["HUSize"].ToString());
                    SPL.Isselected = false;
                    lst.Add(SPL);  
                }
            }
            return lst;
        }

       

        public List<QCPending> GetQCPendingList(int InboundID, int MaterialMasterID, int POHeaderID, string LineNumber, int SIID,int SupplierInvoiceDetailsID)
        {
            List<QCPending> lst = new List<QCPending>();
            //string Query = "EXEC sp_INV_GetGoodsMovementDetailsList_NEW  @InboundID= " + InboundID + ", @POHeaderID= " + POHeaderID + ", @LineNumber = " + LineNumber + ", @MaterialMasterID=" + MaterialMasterID + ", @SupplierInvoiceID=" + SIID + "";
            string Query = "EXEC sp_INV_GET_GoodsIn_List  @InboundID= " + InboundID + ", @POHeaderID= " + POHeaderID + ", @LineNumber = " + LineNumber + ", @MaterialMasterID=" + MaterialMasterID + ", @SupplierInvoiceID=" + SIID + ",@SupplierInvocieDetailsID="+ SupplierInvoiceDetailsID;
            DataSet DS = new DataSet();
            DS = DB.GetDS(Query, false);
            if (DS.Tables.Count != 0 && DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
        
                    QCPending SPL = new QCPending();
                    SPL.Location = row["Location"].ToString();
                    SPL.CartonCode =row["CartonCode"].ToString();
                    SPL.Quantity = row["Quantity"].ToString();
                    SPL.MfgDate= row["MfgDate"].ToString();
                    SPL.ExpDate = row["ExpDate"].ToString();
                    SPL.BatchNo = row["BatchNo"].ToString();
                    SPL.SerialNo = row["SerialNo"].ToString();
                    SPL.StorageLocations = row["StorageLocation"].ToString();
                    SPL.MaterialTransactionID= row["MaterialTransactionID"].ToString();
                    SPL.LocationID= row["LocationID"].ToString();
                    SPL.MaterialMasterID= row["MaterialMasterID"].ToString();
                    SPL.ProjectRefNo = row["ProjectRefNo"].ToString(); 
                    //SPL.SupplierInvoiceDetailsID = Convert.ToInt32(row["SupplierInvoiceDetailsID"]);
                    // SPL.POSODetailsID = Convert.ToInt32(row["POSODetailsID"]);
                    //SPL.IsNonConfirmity = row["IsNonConfirmity"].ToString();
                    //SPL.IsActive = Convert.ToInt32(row["IsActive"]);
                    //SPL.QCStatus = Convert.ToInt32(row["QCstatus"]);
                    //SPL.QCReq = Convert.ToInt32(row["QCReq"]);
                    //SPL.Isdisplay = Convert.ToInt32(row["Isdisplay"]);
                    //SPL.QCText = row["QCText"].ToString();
                    //SPL.IsDamaged = row["IsDamaged"].ToString();
                    //SPL.HasDiscrepancy = row["HasDiscrepancy"].ToString();
                    //SPL.CapturingQc = Convert.ToInt32(row["CapturingQc"]);
                    //SPL.AsIs = row["AsIs"].ToString();
                    //SPL.IsPositiveRecall = row["IsPositiveRecall"].ToString();
                    //SPL.GoodsMovementDetailsID = Convert.ToInt32(row["GoodsMovementDetailsID"]);

                    lst.Add(SPL);
                }
            }
            return lst;
        }

        [WebMethod]
        public static ResponceMessage SetGoodsIn(ReceiveGoodIN Rec, string VehicleNumber )
        {
            ResponceMessage responce = new ResponceMessage();
            cp = HttpContext.Current.User as CustomPrincipal;
       
            string[] MCode = Rec.MCode.Split('_');
            decimal Quantiy = Convert.ToDecimal(Rec.DocQty);
            if(Quantiy <= 0)
            {
                responce.Status = true;
                responce.Message = "Please check the quantity";
                return responce;


            }
            Rec.MCode = MCode[0];
            
            try
            {

            

                string SerialNo = Rec.SerialNo;
                if (SerialNo != "")
                {
                    string Query = "[dbo].[sp_GET_SERIAL_COUNT] @SerialNo = " + DB.SQuote(SerialNo) + ", @MCode= " + DB.SQuote(Rec.MCode) + "";
                    int Count = DB.GetSqlN(Query);
                    if (Count != 0)
                    {
                        responce.Status = true;
                        responce.Message = "Serial no is already received";
                        return responce;

                    }

                }
              
                string UserID = cp.UserID.ToString();
                MRLWMSC21Common.GoodsInDTO goodsInDTO = new GoodsInDTO();
                goodsInDTO.Location = Rec.Location;
                goodsInDTO.DocumentQuantity = Rec.DocQty;
                goodsInDTO.ConvertedQuantity = Rec.Quantity;
                goodsInDTO.InboundID = Rec.InboundID;
                goodsInDTO.LineNumber = Rec.LineNumber;
                goodsInDTO.MCode = Rec.MCode;
                goodsInDTO.LoggedinUserID = cp.UserID;
                goodsInDTO.CartonCode = Rec.Carton;
                goodsInDTO.StorageLocation = Rec.StorageLocation;
                goodsInDTO.MfgDate = Rec.MfgDate;
                goodsInDTO.ExpDate = Rec.ExpDate;
                goodsInDTO.MRP = Rec.MRP;
                goodsInDTO.SerialNumber = Rec.SerialNo;
                goodsInDTO.BatchNumber = Rec.BatchNo;
                goodsInDTO.ProjectRefNo = Rec.ProjectRefNo;
                goodsInDTO.SupplierInvoiceId = Rec.SupplierInvoiceID;
                goodsInDTO.PoHeaderID = Rec.POheaderID;
                goodsInDTO.IsRequestFromPC = 1;
                goodsInDTO.HUNo = Rec.HUNo;
                goodsInDTO.Storerefno = Rec.StorefNumber;
                goodsInDTO.HUsize = Rec.HUSize;

                string Result = MRLWMSC21Common.InventoryCommonClass.ReceiveItem(goodsInDTO);
                if(Result=="1")
                {
                    responce.Status = true;
                    responce.Message = "Successfully Received";
                }
                else
                {
                    responce.Status = false;
                    responce.Message = Result;
                }

            }
            catch (Exception ex)
            {
                responce.Status = false;
                responce.Message = ex.Message;
            }

            return responce;


        }

        [WebMethod]
        public static string UpsertParameters(int GMID,string QCSrNo, string Qty,string Remarks,string QcParams)
        {
            string Status = "";
            if(QCSrNo=="")
            {
                QCSrNo = "0";
            }
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("DECLARE @QCParamID int EXEC ");
              //  sb.Append("[dbo].[sp_ORD_UpsertPOHeader] ");
                sb.Append(" [dbo].[Usp_INV_UpsertGoodsMovementDetails_QualityParameterNew] ");
                sb.Append(" @GoodsMovementDetailsID=" + GMID );
                sb.Append(" ,@QCSerialNo=" + Convert.ToInt32(QCSrNo));
                sb.Append(" ,@QCQuantity=" + Convert.ToDecimal(Qty));
                //sb.Append(" ,@LocationID=" + LocationID);
                //sb.Append(" ,@AsIs=" + AsIs );
                sb.Append(" ,@QCRemarks='" + Remarks +"'");
                //sb.Append(" ,@IsNonConfirmity=" + IsNonConfirmity );
                sb.Append(" ,@CreatedBy=" + cp.UserID);
                sb.Append(" ,@QcParams='" + QcParams + "'");
                sb.Append(",@Result = @QCParamID OUTPUT Select @QCParamID as N");

                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }
            return Status;
        }

        [WebMethod]
        public static string AddParameters(QCList qcdata, List<QualityParams> qcparams,string Quantity,int NonConfirmity)
        {
            string Status = "";
            //if (QCSrNo == "")
            //{
            //    QCSrNo = "0";
            //}
            string s = string.Join("", qcparams);
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            emptyNamepsaces.Add("", "");
            var settings = new XmlWriterSettings();
            string xml = null;
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(List<QualityParams>));
                serialiser.Serialize(sw, qcparams, emptyNamepsaces);
                try
                {
                    xml = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            if (qcdata.QCSerialNo == "")
            {
                qcdata.QCSerialNo = "0";
            }
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("DECLARE @QCParamID int EXEC ");
                //  sb.Append("[dbo].[sp_ORD_UpsertPOHeader] ");
                sb.Append(" [dbo].[Usp_INV_UpsertGoodsMovementDetails_QualityParameterNew] ");
                sb.Append(" @GoodsMovementDetailsID=" + qcdata.GoodsMovementDetailsID);
                sb.Append(", @IsNonConfirmity=" + NonConfirmity);
                sb.Append(" ,@QCSerialNo=" + Convert.ToInt32(qcdata.QCSerialNo));
                sb.Append(" ,@QCQuantity=" + Convert.ToDecimal(Quantity));
                //sb.Append(" ,@LocationID=" + LocationID);
                //sb.Append(" ,@AsIs=" + AsIs );
                sb.Append(" ,@QCRemarks=''");
                //sb.Append(" ,@IsNonConfirmity=" + IsNonConfirmity );
                sb.Append(" ,@CreatedBy=" + cp.UserID);
                sb.Append(" ,@QcParams='" + xml + "'");
                sb.Append(",@Result = @QCParamID OUTPUT Select @QCParamID as N");

                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }
            return Status;
        }
        [WebMethod]
        public static ExecutionResponse DeletedItem(List<QCPending> Items)
        {
            ExecutionResponse er = new ExecutionResponse(true);
            try
            {
               GoodsIn UL = new GoodsIn();
                int result =   UL.Deleteditem(Items);

                if(result==0)
                {
                    er.Status = true;
                    er.Message = "The item deleted successfully";
                }
                else if(result==-111)
                {
                    er.Status = false;
                    er.Message = "GRN Done, cannot delete";
                }
                else if (result == -222)
                {
                    er.Status = false;
                    er.Message = "Item moved, cannot delete";

                }
            }
            catch (Exception ex)
            {
                er.Status = false;
                er.Message = ex.StackTrace;
            }
            return er;
        }


        //[WebMethod]
        //public static ExecutionResponse RevertReceivingLineItem(int supplierInvoiceDetailsID)
        //{
        //    ExecutionResponse responce = new ExecutionResponse();
        //    try
        //    {
               
        //        string strQuery = "EXEC @SupplierInvoiceDetailsID=" + supplierInvoiceDetailsID;
        //        int result = DB.GetSqlN(strQuery);
        //        if(result==1)
        //        {
        //            responce.Status = true;
        //            responce.Message = "Revert transaction completed successfully";
        //        }
        //        else
        //        {
        //            responce.Status = false;
        //            responce.Message = "GRN Completed cannot revert";
        //        }

        //    }
        //    catch(Exception ex)
        //    {
        //        responce.Status = false;
        //        responce.Message = ex.Message;
        //    }
        //    return responce;
        //}
        public int Deleteditem(List<QCPending> Items)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string xml = null;
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            string s = string.Join("", Items);
            var settings = new XmlWriterSettings();
            using (StringWriter sw = new StringWriter())
            {
                try
                {
                    XmlSerializer serialiser = new XmlSerializer(typeof(List<QCPending>));
                    serialiser.Serialize(sw, Items, emptyNamepsaces);
                    xml = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");
                   int result=  DB.GetSqlN("EXEC [dbo].[sp_GMD_Revert_Receiving]  @GoodsData	 = " + DB.SQuote(xml) + ",@CreatedBy="+ cp.UserID);
                    return result;

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
          
        }
        [WebMethod]
        public static ExecutionResponse DeletedQCCaptureItem(QCList qcdata)
        {
            ExecutionResponse er = new ExecutionResponse(true);
            try
            {
                GoodsIn UL = new GoodsIn();
                UL.DeleteCaptureItem(qcdata);
            }
            catch (Exception ex)
            {
                er.Status = false;
                er.Message = ex.StackTrace;
            }
            return er;
        }
        public void DeleteCaptureItem(QCList qcdata)
        {
            DB.ExecuteSQL("UPDATE INV_QualityParametersCapture SET IsDeleted = 1 where GoodsMovementDetailsID="+ qcdata.GoodsMovementDetailsID + " and QCSerialNo="+qcdata.QCSerialNo);
        }

        public List<ErrorCode> GetSuggestedErrorlist (int InboundID)
        {
            List<ErrorCode> lst = new List<ErrorCode>();
            string Query = "EXEC [dbo].[GET_ErrorDescrtptionForInbound] @InboundId ="+InboundID+"";
            DataSet DS = DB.GetDS(Query, false);
            if(DS.Tables[0].Rows.Count != 0)
            {
                foreach(DataRow row in DS.Tables[0].Rows)
                {
                    ErrorCode EC = new ErrorCode();
                    EC.errorCode = row["ErrorCode"].ToString();
                    EC.errordescription = row["ErrorMessage"].ToString();
                    lst.Add(EC);
                }
            }
            return lst;
        }

        public class DropDownList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int PODetailsID { get; set; }
            public string MaterialMasterID { get; set; }
            public string LineNumber { get; set; }
            public int POHeaderID { get; set; }
            public string PrinterIP { get; set; }
        }

        public class IsRquired
        {
            //public string batchno { get; set; }
            //public string SerialNo { get; set; }
            //public string ProjectrefNo { get; set; }
            //public string MfgDate { get; set; }
            //public string ExpDate { get; set; }
            //public int  Materialid { get; set; }
            public int MaterialStorageParameterID { get; set; }
            public int MaterialMasterID { get; set; }
            public string ParameterName { get; set; }
            public Boolean IsRequired { get; set; }
        }

        

        public class SuggestedPutwayList
        {
            public int SuggestedPutawayID { get; set; }
            public int SuggestedLocationID { get; set; }
            public string Location { get; set; }
            public int MaterialMasterID { get; set; }
            public decimal SuggestedQty { get; set; }
            public decimal SuggestedPendingQty { get; set; }
            public int SupplierInvoiceID { get; set; }
            public string Ponumber { get; set; }
            public string BatchNo { get; set; }
            public string SerialNumber { get; set; }
            public string MfgDate { get; set; }
            public string ExpDate { get; set;}
            public string ProjectRef { get; set; }
            public string SupplierInvoice { get; set; }
            public string TenatName { get; set; }
            public string Mcode { get; set; }
            public string StorefNumber { get; set; }
            public Boolean Isselected { get; set; }
            public decimal BUoMQty { get; set; }
            public string MRP { get; set; }

            public decimal ReceivedQty { get; set; }

            public int HUNo { get; set; }
            public int HUSize { get; set; }

        }


        public class QCValues
        {
            public string Location { get; set; }             
            public string IsDamaged { get; set; }
            public string HasDiscrepancy { get; set; }
            public string AsIs { get; set; }      
            public string IsNonConfirmity { get; set; }
            public string MfgDate { get; set; }
            public string ExpDate { get; set; }
            public string BatchNo { get; set; }
            public string SerialNo { get; set; }
            public string ProjectRefNo { get; set; }

        }
        public class QCList
        {
            public int GoodsMovementDetailsID { get; set; }
            public string QCSerialNo { get; set; }
            public string Quantity { get; set; }
            public int QCParamCaptureID { get; set; }
            public Boolean Isselect { get; set; }
            public string SerailName { get; set; }
            public decimal totalQty { get; set; }
            public decimal pendingQty { get; set; }
            public decimal completedQty { get; set; }
            public List<QualityParamValues> paramvalues { get; set; }
        }
        public class QualityParamValues
        {
            public int QCSerialNo { get; set; }
            public int QualityParameterID { get; set; }
            public string value { get; set; }
        }

        public class QCPending
        {
            public string LocationID { get; set; }
            public string Location { get; set; }
            public string CartonCode { get; set; }
            public string DocQty { get; set; }
            public string IsDamaged { get; set; }
            public string HasDiscrepancy { get; set; }

            public string AsIs { get; set; }
            public string IsPositiveRecall { get; set; }
            public string IsNonConfirmity { get; set; }

            public int POSODetailsID { get; set; }
            public string Value { get; set; }
            public int CapturingQc { get; set; }
            public int GoodsMovementDetailsID { get; set; }

            public int QCReq { get; set; }
            public int Isdisplay { get; set; }
            public string QCText { get; set; }
            public string MfgDate { get; set; }
            public string ExpDate { get; set; }
            public string BatchNo { get; set; }
            public string SerialNo { get; set; }
            public int IsActive { get; set; }

            public string StorageLocations { get; set; }

            public string IsSelected { get; set; }

            public int QCStatus { get; set; }


            public string SupplierInvoiceDetailsID { get; set; }

            public string MaterialTransactionID { get; set; }

            public string Quantity { get; set; }

            public string MaterialMasterID { get; set; }

            public string ProjectRefNo { get; set; }
        } 

        public class ReceiveGoodIN
        {
        public string Location { get; set; }
        public string DocQty { get; set; }
        public string Quantity { get; set; }
        public string LineNumber { get; set; }
        public string InboundID { get; set; }
        public string MMID { get; set; }
        public string IsDamaged { get; set; }
        public string MCode { get; set; }
        public int LocationID { get; set; }
        public string hasDiscrepancy { get; set; }
        public int CreatedBy { get; set; }
        public string remarks { get; set; } 
        public string Carton { get; set; }
        public int StorageLocationID { get; set; }
        public string StorageLocation { get; set; }
        public int UserID { get; set; }
        public string MfgDate { get; set; }
        public string ExpDate { get; set; }
        public string BatchNo { get; set; }

        public string SerialNo { get; set; }
        public string ProjectRefNo { get; set; }
        
        public string StorefNumber { get; set; }
        public string SPID { get; set; }

        public string POheaderID { get; set; }
        public string SupplierInvoiceID { get; set; }
        public string SkipReasonID { get; set; }
        public string MRP { get; set; }
        public string HUNo { get; set; }

        public string HUSize { get; set; }

        }

        //------------------ added by durga for Material Quality parameters on 05/03/2018 ---------------------//
        public class QualityParams
        {
            public string ParameterName { get; set; }
            public string ParameterDataType { get; set; }
            public string ControlType { get; set; }
            public string QualityParameterID { get; set; }
            public bool IsRequired { get; set; }
            public decimal MinTolerance { get; set; }
            public decimal MaxTolerance { get; set; }
            public string value { get; set; }

        }

        public class ErrorCode
        {
            public string errorCode { get; set; }
            public string errordescription { get; set; }
        }

        public class ResponceMessage

        {
            public bool Status { set; get; }
            public string  Message { set; get; }
        }

}
}
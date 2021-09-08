using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MRLWMSC21Common;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Web.Script.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Converters;
using MRLWMSC21Common;
using System.Threading;
using System.Globalization;
using System.Collections;


namespace MRLWMSC21.mWebServices
{
    /// <summary>
    /// Summary description for AndroidWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AndroidWebService : System.Web.Services.WebService
    {

       // public static double latitude=0, longitude=0;

        public static IDictionary<String, GpsPosition> MobilePositionList = new Dictionary<String, GpsPosition>();
        
        public class GpsPosition
        {

            public GpsPosition(string latitude, string longitude)
            {
                this.latitude = latitude;
                this.longitude = longitude;
            }
            public string latitude = "", longitude = "";
        }

        
        [WebMethod(EnableSession=true)]
        public String PushLatLongDetails(String DiviceID, string latitude, string longitude)
        {
            //if (Session["MobilePositionList"] != null)
            //{
            //    MobilePositionList = Session["MobilePositionList"] as Dictionary<String, GpsPosition>;
            //}
            //else
            //{
            //    MobilePositionList = new Dictionary<String, GpsPosition>();
            //}
            GpsPosition hhhh;
            if (MobilePositionList.TryGetValue(DiviceID, out hhhh))
            {
                hhhh.latitude = latitude;
                hhhh.longitude = longitude;
            }
            else
            {
                MobilePositionList.Add(DiviceID, new GpsPosition(latitude, longitude));
            }
            //Session["MobilePositionList"] = MobilePositionList;
            return "Successfully posted";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String GetDeviceLatLong(String DeviceID) {

            //if (Session["MobilePositionList"] != null)
            //{
            //    MobilePositionList = Session["MobilePositionList"] as Dictionary<String, GpsPosition>;
            //}
            //else
            //{
            //    MobilePositionList = new Dictionary<String, GpsPosition>();
            //}

            GpsPosition gpsp;
            if (MobilePositionList.TryGetValue(DeviceID, out gpsp))
                return gpsp.latitude + "," + gpsp.longitude;
            else
                return "No data found";
        }

        [WebMethod]
        public string UploadFile(byte[] f, string fileName)
        {
            // the byte array argument contains the content of the file
            // the string argument contains the name and extension
            // of the file passed in the byte array
            try
            {
                // instance a memory stream and pass the
                // byte array to its constructor
                MemoryStream ms = new MemoryStream(f);
 
                // instance a filestream pointing to the
                // storage folder, use the original file name
                // to name the resulting file
                FileStream fs = new FileStream
                    (System.Web.Hosting.HostingEnvironment.MapPath
                    ("~/uploadedfile/") +
                    fileName, FileMode.Create);
 
                // write the memory stream containing the original
                // file as a byte array to the filestream
                ms.WriteTo(fs);
 
                // clean up
                ms.Close();
                fs.Close();
                fs.Dispose();
 
                // return OK if we made it this far
                return "OK";
            }
            catch (Exception ex)
            {
                // return the error message if the operation fails
                return ex.Message.ToString();
            }
        }
   
        public string ConvertDatatableToXML(DataTable dt)
        {
            MemoryStream str = new MemoryStream();
            dt.WriteXml(str, true);
            str.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(str);
            string xmlstr;
            xmlstr = sr.ReadToEnd();
            return (xmlstr);
        }

        [WebMethod]
        public string GetUserDetails(String UserName, String Password)
        {

            try
            {

                DataSet dsUserDetails = DB.GetDS("EXEC dbo.[sp_Android_CheckUserLogin] @Email='" + UserName + "', @Password='" + Password + "'", false);

                DataTable dtUserDetails = dsUserDetails.Tables[0];

                dtUserDetails.Namespace = "UserDetails";
                dtUserDetails.TableName = "UserDetails";

                string jsUserDetails = JsonConvert.SerializeObject(dsUserDetails, new DataSetConverter());



                return jsUserDetails;



            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [WebMethod]
        public String GetMenuLinks()
        {

            try
            {

                DataSet dsUser = DB.GetDS("EXEC [dbo].[sp_Android_GetMenuLinks]  ", false);

                DataTable dt = dsUser.Tables[0];


                dt.Namespace = "MenuLinks";
                dt.TableName = "MenuLinks";


                return ConvertDatatableToXML(dt);

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [WebMethod]
        public string GetInboundStatistics()
        {

            try
            {

                DataSet dsIBStatstics = DB.GetDS("EXEC [dbo].[sp_Android_GetInboundStatistics]  ", false);

                DataTable dtIBStatstics = dsIBStatstics.Tables[0];


                dtIBStatstics.Namespace = "INBStatstics";
                dtIBStatstics.TableName = "INBStatstics";


                string jsIBStatstics = JsonConvert.SerializeObject(dsIBStatstics, new DataSetConverter());


                return jsIBStatstics;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public string GetOutboundStatistics()
        {

            try
            {

                DataSet dsOBStatstics = DB.GetDS("EXEC [dbo].[sp_Android_GetOutboundStatistics]  ", false);

                DataTable dt = dsOBStatstics.Tables[0];


                dt.Namespace = "OBDStatstics";
                dt.TableName = "OBDStatstics";


                string jsOBStatstics = JsonConvert.SerializeObject(dsOBStatstics, new DataSetConverter());

                return jsOBStatstics;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public string GetInOutActivities()
        {

            try
            {

                DataSet dsInOutActivities = DB.GetDS("EXEC [dbo].[sp_Android_GetInOutActivities]  ", false);

                DataTable dt = dsInOutActivities.Tables[0];


                dt.Namespace = "InOutActivities";
                dt.TableName = "InOutActivities";

                string jsInOutActivities = JsonConvert.SerializeObject(dsInOutActivities, new DataSetConverter());


                return jsInOutActivities;

            }
            catch (Exception ex)
            {
                return null;
            }
        }



        [WebMethod]
        public string GetInboundList(String WarehouseIDs, int ShipmentTypeID, int InboundStatusID, int SupplierID,int Limit)
        {

            try
            {
                WarehouseIDs = null;
                // DataSet dsInboundList = DB.GetDS("EXEC [dbo].[sp_Android_INB_GetInboundList]  @WarehouseIDs='" + 0 + "',@InboundStatusID=" + InboundStatusID + ",@ShipmentTypeID=" + ShipmentTypeID + ",@SupplierID=" + SupplierID + ",@Limit=" + Limit, false);
                DataSet dsInboundList = DB.GetDS("EXEC [dbo].[sp_Android_INB_GetInboundList] @Limit=" + Limit, false);
                DataTable dt = dsInboundList.Tables[0];


                dt.Namespace = "InboundList";
                dt.TableName = "InboundList";

                string jsdsInboundList = JsonConvert.SerializeObject(dsInboundList, new DataSetConverter());


                return jsdsInboundList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public string GetOutboundList(String WarehouseIDs, int DocumentTypeID, int OBDStatusID, int CustomerID, int Limit)
        {

            try
            {

                DataSet dsOutboundList = DB.GetDS("EXEC  [dbo].[sp_Android_OBD_GetOutboundList]  @WarehouseIDs='" + WarehouseIDs + "',@DocumentTypeID=" + DocumentTypeID + ",@OBDStatusID=" + OBDStatusID + ",@CustomerID=" + CustomerID+",@Limit="+Limit, false);

                DataTable dt = dsOutboundList.Tables[0];


                dt.Namespace = "OutboundList";
                dt.TableName = "OutboundList";

                string jsOutboundList = JsonConvert.SerializeObject(dsOutboundList, new DataSetConverter());

                return jsOutboundList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [WebMethod]
        public string GetJobOrderOutboundList(int Limit)
        {

            try
            {

                DataSet OutboundList = DB.GetDS("EXEC  [dbo].[sp_Android_OBD_GetJobOrderOutboundList] @Limit="+Limit, false);

                DataTable dt = OutboundList.Tables[0];


                dt.Namespace = "JOutboundList";
                dt.TableName = "JOutboundList";

                string jsJobOrderList = JsonConvert.SerializeObject(OutboundList, new DataSetConverter());

                return jsJobOrderList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [WebMethod]
        public DataSet GetInOutOperations()
        {

            try
            {

                DataSet dsInOutActivities = DB.GetDS("EXEC [dbo].[sp_Android_INOUT_GetInOutOperations]  ", false);

                DataTable dt = dsInOutActivities.Tables[0];


                dt.Namespace = "InOutOperations";
                dt.TableName = "InOutOperations";


                return dsInOutActivities;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public string GetLiveStock(String MCode, String Location, String BatchNo,String PageSize)
        {

            try
            {
                DataSet dsInOutActivities = DB.GetDS("EXEC [dbo].[sp_Android_INV_GetLiveStock] @MaterialMaster='" + MCode + "',@Location='" + Location + "',@BatchNo='" + BatchNo + "'" + ",@PageSize="+PageSize, false);

                DataTable dt = dsInOutActivities.Tables[0];


                dt.Namespace = "LiveStock";
                dt.TableName = "LiveStock";

                string jsJobOrderList = JsonConvert.SerializeObject(dsInOutActivities, new DataSetConverter());


                return jsJobOrderList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [WebMethod]
        public string GetJobOrderList(int Limit)
        {

            try
            {

                DataSet dsJobOrderList = DB.GetDS("EXEC [dbo].[sp_Android_GetJobOrderList] @Limit=" + Limit, false);

                DataTable dt = dsJobOrderList.Tables[0];


                dt.Namespace = "JobOrderList";
                dt.TableName = "JobOrderList";


                //return dsInOutActivities;

                string jsJobOrderList = JsonConvert.SerializeObject(dsJobOrderList, new DataSetConverter());

                return jsJobOrderList;


            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [WebMethod]
        public string GetWarehouses()
        {

            try
            {

                DataSet dsWarehouses = DB.GetDS("EXEC [dbo].[sp_Android_GetWarehouses] ", false);

                DataTable dt = dsWarehouses.Tables[0];


                dt.Namespace = "Warehouses";
                dt.TableName = "Warehouses";

                string jsWarehouses = JsonConvert.SerializeObject(dsWarehouses, new DataSetConverter());

                return jsWarehouses;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public string GetSuppliers()
        {

            try
            {

                DataSet dsSuppliers = DB.GetDS("EXEC [dbo].[sp_Android_GetSuppliers] ", false);

                DataTable dt = dsSuppliers.Tables[0];


                dt.Namespace = "Suppliers";
                dt.TableName = "Suppliers";

                string jsSuppliers = JsonConvert.SerializeObject(dsSuppliers, new DataSetConverter());

                return jsSuppliers;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [WebMethod]
        public string GetShipmentType()
        {

            try
            {

                DataSet dsShipmentType = DB.GetDS("EXEC [dbo].[sp_Android_GetShipmentType] ", false);

                DataTable dt = dsShipmentType.Tables[0];


                dt.Namespace = "ShipmentType";
                dt.TableName = "ShipmentType";

                string jsShipmentType = JsonConvert.SerializeObject(dsShipmentType, new DataSetConverter());


                return jsShipmentType;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public string GetShipmentStatus()
        {

            try
            {

                DataSet dsShipmentStatus = DB.GetDS("EXEC [dbo].[sp_Android_GetShipmentStatus] ", false);

                DataTable dt = dsShipmentStatus.Tables[0];


                dt.Namespace = "ShipmentStatus";
                dt.TableName = "ShipmentStatus";


                string jsShipmentStatus = JsonConvert.SerializeObject(dsShipmentStatus, new DataSetConverter());

                return jsShipmentStatus;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [WebMethod]
        public string GetDeliveryStatus()
        {

            try
            {

                DataSet dsDeliveryStatus = DB.GetDS("EXEC [dbo].[sp_Android_GetDeliveryStatus] ", false);

                DataTable dt = dsDeliveryStatus.Tables[0];


                dt.Namespace = "DeliveryStatus";
                dt.TableName = "DeliveryStatus";

                string jsDeliveryStatus = JsonConvert.SerializeObject(dsDeliveryStatus, new DataSetConverter());


                return jsDeliveryStatus;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [WebMethod]
        public string GetCustomers()
        {

            try
            {

                DataSet dsCustomers = DB.GetDS("EXEC [dbo].[sp_Android_GetCustomer] ", false);

                DataTable dt = dsCustomers.Tables[0];


                dt.Namespace = "Customers";
                dt.TableName = "Customers";

                string jsCustomers = JsonConvert.SerializeObject(dsCustomers, new DataSetConverter());

                return jsCustomers;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public string GetDocumentType()
        {

            try
            {

                DataSet dsDocumentType = DB.GetDS("EXEC [dbo].[sp_Android_GetDocumentType] ", false);

                DataTable dtDocumentType = dsDocumentType.Tables[0];

                dtDocumentType.TableName = "DocumentType";

                string jsonDocumentType = JsonConvert.SerializeObject(dsDocumentType, new DataSetConverter());

                return jsonDocumentType;

            }
            catch (Exception ex)
            {
                return null;
            }
        
        }

        [WebMethod]
        public string GetPartNumberList() {

            try
            {

                DataSet dsPartNumberList = DB.GetDS("EXEC [dbo].[sp_Android_GetPartNumberList] ", false);

                DataTable dtPartNumberList = dsPartNumberList.Tables[0];

                dtPartNumberList.TableName = "PartNumberList";

                string jsonPartNumberList = JsonConvert.SerializeObject(dsPartNumberList, new DataSetConverter());

                return jsonPartNumberList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public string GetVersionDetails()
        {

            try
            {

                DataSet dsVersionDetails = DB.GetDS("EXEC [dbo].[sp_Android_GetVersionDetails] ", false);

                DataTable dtVersionDetails = dsVersionDetails.Tables[0];

                dtVersionDetails.TableName = "VersionDetails";

                string jsonVersionDetails = JsonConvert.SerializeObject(dsVersionDetails, new DataSetConverter());

                return jsonVersionDetails;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public string UpdatePODSignature(string signature,string deliverydocno)
        {

            try
            {

            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [WebMethod]
        public String GetMaterialStorageParameters(String MaterialCode, int TenantID)
        {
            DataSet dsmaterialparams = DB.GetDS("EXEC [sp_INV_GetMaterialStorageParameters] @MCode='" + MaterialCode + "',@TenantID=1", false);

            DataTable dtmaterialparams = dsmaterialparams.Tables[0];
            dtmaterialparams.Namespace = "MaterialParams";
            dtmaterialparams.TableName = "MaterialParams";



            string jsonVersionDetails = JsonConvert.SerializeObject(dsmaterialparams, new DataSetConverter());

            return jsonVersionDetails;


         
        }
        
          [WebMethod]
         public DataTable GetMaterialReceived(string LineNumber, String MaterialCode, String StoreRefNo, String PONumber, string SupplierInvoieID)
         {
            
             int PoHeaderID;
             if (PONumber != "0")
             {
                 PoHeaderID = DB.GetSqlN("select POHeaderID as N from ORD_POHeader where PONumber=" + DB.SQuote(PONumber));
             }
             else
             {
                 PoHeaderID = 0;
             }
             int vMMID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(MaterialCode));
             int vInboundID = DB.GetSqlN("select InboundID as N from INB_Inbound where StoreRefNo=" + DB.SQuote(StoreRefNo));
             DataSet ds = DB.GetDS("EXEC  [sp_INV_GetGoodsMovementDetailsList]@MaterialMasterID=" + vMMID + ",@InboundID=" + vInboundID + ",@LineNumber=" + LineNumber + ",@POHeaderID=" + PoHeaderID + ",@SupplierInvoiceID=" + SupplierInvoieID, false);
             DataTable dt = ds.Tables[0];
             return dt;
         }
        [WebMethod]
          public string UpdateReceiveItem(string GoodsMovementDetailsID, string GoodsMovementTypeID, string Location, string MaterialMaster_IUoMID, string DocQty, string Quantity, string Storerefnum, string LineNumber, string mcode, string IsDamaged, string hasDiscrepancy, string POSOHeaderID, string CreatedBy, string MaterialStorageParameterIDs, string MaterialStorageParameterValues, string Conversionfactor, string remarks, string LastModifiedBy, string IsActive, string KitplannerID, string InvoiceQuantity, string SupplierInvoiceID, string IsPositiveRecall, int IsPrintRequired, string printerIP, string serialNumber)
          {


              int vInboundID = DB.GetSqlN("select InboundID as N from INB_Inbound where StoreRefNo=" + DB.SQuote(Storerefnum));
              if (serialNumber.Length != 0)
              {
                  string QueryforSerialNumberCheck = "select GMDMSP.GoodsMovementDetails_MaterialStorageParameterID as N from INV_GoodsMovementDetails GMD " +
                           "JOIN INV_GoodsMovementDetails_MMT_MaterialStorageParameter GMDMSP ON GMD.GoodsMovementDetailsID=GMDMSP.GoodsMovementDetailsID " +
                           "AND GMDMSP.MaterialStorageParameterID=3 AND GMDMSP.IsDeleted=0 WHERE GMD.GoodsMovementTypeID=1 AND GMD.IsDeleted=0 AND " +
                           "GMD.TransactionDocID=" + vInboundID + " AND CONVERT(NVARCHAR,GMDMSP.Value)='" + serialNumber + "'";
                  if (DB.GetSqlN(QueryforSerialNumberCheck) != 0)
                  {
                      return "Serial no. already exists, cannot receive";

                  }
              }

              IDataReader reader = DB.GetRS("select InboundTracking_WarehouseID from INB_InboundTracking_Warehouse where InboundID=" + vInboundID);
              if (reader.Read())
              {
              }
              else
              {
                  return "Shipment not received";
              }
              int vDeliveryStatus = DB.GetSqlN("SELECT InboundStatusID AS N FROM INB_Inbound WHERE InboundID=" + vInboundID);
              if (vDeliveryStatus >= 4)
              {
                  return "Shipment verified, Cannot receive";
              }

              int CARTONid = DB.GetSqlN("select CartonID as N from INV_Carton where IsActive=1 and IsDeleted=0 AND CartonCode='" + Location + "'");

              if (CARTONid == 0)
              {
                  return " Not a valid container";
              }
              int vMMID = 0;// DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(mcode));
              string OEMPartNumber = String.Empty;
              string Description = String.Empty;

              //string sqlcommandforMMDetails = "select MaterialMasterID,isnull(OEMPartNo,'') as OEMPartNo,isnull(MDescription,'') as MDescription  from MMT_MaterialMaster where MCode=" + DB.SQuote(mcode);

              string sqlcommandforMMDetails = "select MM.MaterialMasterID,isnull(MM.OEMPartNo,'') as OEMPartNo,isnull(MM.MDescription,'') as MDescription  from MMT_MaterialMaster MM JOIN TPL_Tenant_MaterialMaster TPL_MM ON TPL_MM.MaterialMasterID=MM.MaterialMasterID AND TPL_MM.IsDeleted=0 AND TPL_MM.IsActive=1  JOIN INB_Inbound INB ON INB.TenantID=TPL_MM.TenantID AND INB.IsDeleted=0 AND INB.IsActive=1  where INB.StoreRefNo='" + Storerefnum + "' AND MM.MCode=" + DB.SQuote(mcode);

              IDataReader dataReader = DB.GetRS(sqlcommandforMMDetails);
              while (dataReader.Read())
              {
                  vMMID = DB.RSFieldInt(dataReader, "MaterialMasterID");
                  OEMPartNumber = DB.RSField(dataReader, "OEMPartNo");
                  Description = DB.RSField(dataReader, "MDescription");

              }
              //select MaterialMasterID,OEMPartNo,MDescription as N from MMT_MaterialMaster where MCode='RT205001000027'


              decimal vGoodsINQuantity = DB.GetSqlNDecimal("EXEC  [sp_INV_GetGoodsInQuantitySum]    @MaterialMasterID=" + vMMID + ",@InboundID=" + vInboundID + ",@LineNumber=" + LineNumber + ",@POHeaderID=" + POSOHeaderID + ",@GoodsMovementDetailsID=0" + ",@SupplierInvoiceID=" + SupplierInvoiceID);
              decimal vEnteredQuantity = decimal.Parse(DocQty);
              decimal vInvoiceQuantity = decimal.Parse(InvoiceQuantity);
              //if (vEnteredQuantity + vGoodsINQuantity > vInvoiceQuantity)
              //{
              //    return "Goodsin quantity is greater than invoice quantity cannot receive";
              //}
              int CycleCountOn = DB.GetSqlN("select convert(int,qcc.IsOn) as N from QCC_CycleCount qcc left join QCC_CycleCountDetails qccd on qccd.CycleCountID=qcc.CycleCountID  where qccd.MaterialMasterID=" + vMMID);
              if (CycleCountOn == 1)
              {
                  return "This material is in cycle count,cannot receive";
              }

              try
              {
                  int result = InventoryCommonClass.UpdateGoodsMovementDetails(GoodsMovementDetailsID, GoodsMovementTypeID, "", MaterialMaster_IUoMID, DocQty, Quantity, vInboundID.ToString(), SupplierInvoiceID, LineNumber, vMMID.ToString(), IsDamaged, hasDiscrepancy, POSOHeaderID, CreatedBy, MaterialStorageParameterIDs, MaterialStorageParameterValues, Conversionfactor, remarks, LastModifiedBy, IsActive, KitplannerID, IsPositiveRecall, "0", "0", CARTONid.ToString());
                  if (result == 1)
                  {
                      try
                      {
                          if (IsPrintRequired == 1)
                          {
                              //For Printing Lables
                              MRLWMSC21Common.TracklineMLabel thisMLabel = new MRLWMSC21Common.TracklineMLabel();

                              thisMLabel.MCode = mcode;
                              thisMLabel.OEMPartNo = OEMPartNumber;
                              thisMLabel.Description = Description;
                              thisMLabel.AltMCode = String.Empty;

                              string batchNo = String.Empty;
                              string serialNo = String.Empty;
                              DateTime mfgDate = DateTime.MinValue;
                              DateTime ExpDate = DateTime.MinValue;
                              string[] msps = MaterialStorageParameterIDs.Split(',');
                              string[] mspvalues = MaterialStorageParameterValues.Split(',');
                              for (int index = 0; index < msps.Length; index++)
                              {
                                  if (msps[index] == "1")
                                  {
                                      mfgDate = Convert.ToDateTime(mspvalues[index]);
                                  }
                                  else if (msps[index] == "2")
                                  {
                                      ExpDate = Convert.ToDateTime(mspvalues[index]);
                                  }
                                  else if (msps[index] == "3")
                                  {
                                      serialNo = mspvalues[index];
                                  }
                                  else if (msps[index] == "4")
                                  {
                                      batchNo = mspvalues[index];
                                  }
                              }
                              thisMLabel.MfgDate = mfgDate.ToString();
                              thisMLabel.ExpDate = ExpDate.ToString();

                              thisMLabel.BatchNo = batchNo;
                              thisMLabel.SerialNo = serialNo;
                              thisMLabel.PrintQty = DocQty;


                              thisMLabel.PrinterType = "IP";
                              thisMLabel.PrinterIP = printerIP;
                              thisMLabel.IsBoxLabelReq = true;
                              thisMLabel.ReqNo = String.Empty;

                              thisMLabel.StrRefNo = Storerefnum;
                              thisMLabel.KitCode = String.Empty;
                              thisMLabel.OBDNumber = String.Empty;
                              thisMLabel.IsBoxLabelReq = false;
                              CommonLogic.PrintLabel(thisMLabel);
                              //Thread worker = new Thread(new ParameterizedThreadStart(this.ThermalLabelWorkerForGoodsIN));
                              //worker.SetApartmentState(ApartmentState.STA);
                              //worker.Name = "ThermalLabelWorker";
                              //worker.Start(thisMLabel);
                              //worker.Join();
                              //if (thisMLabel.Result != "Success")
                              //{
                              //    return "successwithnoprint";//, unable to print the label due to printer unavailability. Please check the printer.";

                              //}
                              //else
                              //{
                              //    return "Success";
                              //}

                          }
                      }
                      catch
                      {
                          return "successwithnoprint";//, unable to print the label due to printer unavailability. Please check the printer.";
                      }
                  }
                  else
                  {
                      return "Process failed, Please contact support team";
                  }
                  return "Success";
              }
              catch
              {
                  return "Process failed,Please contact support team";
              }

    }
           [WebMethod]
        public int CartonValidatorAtPutaway(string StoreRefNo, string ContainerCode)
        {

            if (StoreRefNo != null && !StoreRefNo.Trim().Equals("") && ContainerCode != null && !ContainerCode.Trim().Equals(""))
            {
                StringBuilder drlStatement = new StringBuilder(" EXEC [dbo].[sp_Inv_CartonValidationAtPutaway] @StoreRefNo=" + DB.SQuote(StoreRefNo) + " , @CartonCode=" + DB.SQuote(ContainerCode));
                return DB.GetSqlN(drlStatement.ToString());
            }

            return 0;
        }
           [WebMethod]
           public string DeleteGoodsMomentDetails(String GoodsMomentID, int goodsMomentTypeID)
           {
               int isactive = DB.GetSqlTinyInt("SELECT ISNULL(IsActive,0) AS TI FROM INV_GoodsMovementDetails WHERE GoodsMovementDetailsID=" + GoodsMomentID + " AND IsDeleted=0");
               if (isactive == 1)
               {
                   if (goodsMomentTypeID == 1)
                   {
                       return "Shipment verified,Cannot Delete";
                   }
                   else
                   {
                       return "Cannot delete, as PGI is updated ";
                   }

               }
               try
               {
                   DB.ExecuteSQL("EXEC [sp_INV_DeleteGoodsMovementDetails] @GoodsMovementDetailsIDs=" + DB.SQuote(GoodsMomentID) + ", @GoodsMovementTypeID=" + goodsMomentTypeID);
                   return "Successfully Deleted";
               }
               catch
               {
                   return "Failed to delete selected details";
               }
           }
         [WebMethod]
    
           public String GetSOItemProperties(String OBDNumber, String MaterialCode, String Location, int LineNumber, String SONumber)
           {
               Outbound obduom = new Outbound();
               BO.Stockoutitems stockoutitems = new BO.Stockoutitems();
               List<BO.Stockoutitems> lststock = new List<BO.Stockoutitems>();
               string jsonVersionDetails = null;
               try
               {

                   int vMMID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(MaterialCode));
                   int vDocumneTypeID = DB.GetSqlN("select DocumentTypeID as N from OBD_Outbound where OBDNumber=" + DB.SQuote(OBDNumber));

                   int CycleCountOn = DB.GetSqlN("select convert(int,qcc.IsOn) as N from QCC_CycleCount qcc left join QCC_CycleCountDetails qccd on qccd.CycleCountID=qcc.CycleCountID  where qccd.MaterialMasterID=" + vMMID);


                   int vObdID = DB.GetSqlN("select OutboundID as N from OBD_Outbound where OBDNumber=" + DB.SQuote(OBDNumber));
                   if (SONumber != "0")
                   {
                       int SoHeaderID = DB.GetSqlN("select SOHeaderID as N from ORD_SOHeader where SONumber=" + DB.SQuote(SONumber));
                       SONumber = SoHeaderID.ToString();

                   }
                   string vSqlForCheckingDuplicates = "exec [dbo].[sp_INV_GetSOQuantityListForOBDNumber_ForHHT] @MatrialMastrID=" + vMMID + ",@LineNumber=" + LineNumber + ",@ObdID=" + vObdID + ",@SOHeaderID=" + SONumber;
                   DataSet ds = DB.GetDS(vSqlForCheckingDuplicates, false);
                   int rowCount = ds.Tables[0].Rows.Count;
                   if (rowCount > 1)
                   {
                       return jsonVersionDetails;
                   }
                   else if (rowCount == 1)
                   {
                       int vSoid = (int)ds.Tables[0].Rows[0][0];
                       int vKitid = (int)ds.Tables[0].Rows[0][1];
                       int vLocationId = DB.GetSqlN("select LocationID as N from  INV_Location where Location=" + DB.SQuote(Location));
                       DataSet dsUoMs = null;
                       if (vKitid != 0)
                       {
                           dsUoMs = DB.GetDS("EXEC [sp_INV_GetSOQuantityListForOBDNumber]@MaterialMasterID=" + vMMID + ",@OuboundID=" + vObdID + ",@LocationID=" + vLocationId + ",@LineNumber=" + LineNumber + ",@kitPlannerID=" + vKitid + ",@SOHeaderID=" + vSoid, false);
                       }
                       else
                       {
                           dsUoMs = DB.GetDS("EXEC [sp_INV_GetSOQuantityListForOBDNumber]@MaterialMasterID=" + vMMID + ",@OuboundID=" + vObdID + ",@LocationID=" + vLocationId + ",@LineNumber=" + LineNumber + ",@kitPlannerID=NULL,@SOHeaderID=" + vSoid, false);
                       }
                       DataTable dtUoMs = dsUoMs.Tables[0];
                       dtUoMs.Namespace = "SUoM";
                       dtUoMs.TableName = "SUoM";
                       stockoutitems.OBDID = vObdID;
                       stockoutitems.MaterialMasterID = vMMID;
                       stockoutitems.LocationID = vLocationId;
                       stockoutitems.IsCycleCountON = CycleCountOn;
                       stockoutitems.KitPlannerID = vKitid;
                       stockoutitems.MaterialCode = dsUoMs.Tables[0].Rows[0]["MCode"].ToString();
                       stockoutitems.MDescription = dsUoMs.Tables[0].Rows[0]["MDescription"].ToString();
                       stockoutitems.SOHeaderID = dsUoMs.Tables[0].Rows[0]["SOHeaderID"].ToString();
                       stockoutitems.BUoM = dsUoMs.Tables[0].Rows[0]["BUoM"].ToString();
                       stockoutitems.BUoMQty = dsUoMs.Tables[0].Rows[0]["BUoMQty"].ToString();
                       stockoutitems.MaterialMaster_SUoMID = dsUoMs.Tables[0].Rows[0]["MaterialMaster_SUoMID"].ToString();
                       stockoutitems.MUoM = dsUoMs.Tables[0].Rows[0]["MUoM"].ToString();
                       stockoutitems.MUoMQty = dsUoMs.Tables[0].Rows[0]["MUoMQty"].ToString();
                       stockoutitems.SUoM = dsUoMs.Tables[0].Rows[0]["SUoM"].ToString();
                       stockoutitems.SUoMQty = dsUoMs.Tables[0].Rows[0]["SUoMQty"].ToString();
                       stockoutitems.CustPOUoM = dsUoMs.Tables[0].Rows[0]["CustPOUoM"].ToString();
                       stockoutitems.CustPOUoMQty = dsUoMs.Tables[0].Rows[0]["CustPOUoMQty"].ToString();
                       
                       if (dsUoMs.Tables[0].Rows[0]["MaterialGroup"].ToString() != "NULL")
                       {
                           stockoutitems.MaterialGroup = dsUoMs.Tables[0].Rows[0]["MaterialGroup"].ToString();
                       }
                       stockoutitems.SOQuantity = dsUoMs.Tables[0].Rows[0]["SOQuantity"].ToString();
                       if (dsUoMs.Tables[0].Rows[0]["MeasurementTypeID"].ToString() != "NULL")
                       {
                           stockoutitems.MeasurementTypeID = dsUoMs.Tables[0].Rows[0]["MeasurementTypeID"].ToString();
                       }
                       stockoutitems.ConvertionFactionValue = Convert.ToDecimal(dsUoMs.Tables[0].Rows[0]["CF"].ToString());
                       stockoutitems.ConvertionInMUom = Convert.ToDecimal(dsUoMs.Tables[0].Rows[0]["CFInMUoM"].ToString());
                       stockoutitems.ContainerId = vLocationId;
                       
                       lststock.Add(stockoutitems);
                       obduom.stockout = lststock;
                        }
                   jsonVersionDetails = JsonConvert.SerializeObject(obduom, new DataTableConverter());
                   return jsonVersionDetails;
               }
               catch (Exception ex)
               {
                   return "Error while getting data.";
               }


           }
         [WebMethod]
         public String GetAvailbleQtyList(int OBDID, int MaterialMasterID, int LocationID, int LineNumber, int KitPlannerID, int SoHeaderID, int containerId)
         {
             try
             {
                 string jsonVersionDetails = null;

                 DataSet dataset;
                 dataset = DB.GetDS("EXEC [sp_INV_GetStockInList]  @MaterialMasterID=" + MaterialMasterID + ", @LocationID=" + LocationID + ",@KitPlannerID=" + (KitPlannerID != 0 ? KitPlannerID.ToString() : "NULL") + ",@OutBoundID=" + OBDID + ",@SOHeaderID=" + SoHeaderID + ",@LineNumber=" + LineNumber + ",@CartonId=" + containerId, false);

                 DataTable dtavail = dataset.Tables[0];
                 dtavail.Namespace = "AvailableQty";
                 dtavail.TableName = "AvailableQty";



                 jsonVersionDetails = JsonConvert.SerializeObject(dataset, new DataSetConverter());


                 return jsonVersionDetails;
             }
             catch (Exception ex)
             {
                 return "Error while getting data.";
             }

         }
        [WebMethod]
         public string UpdateGoosOUT(string GoodsMovementDetailsID, string GoodsMovementTypeID, int LocationID, string Location, string MaterialMaster_IUoMID, string DocQty, string Quantity, string SOQty, int OBDID, string LineNumber, int MaterialMasterID, string mcode, string IsDamaged, string hasDiscrepancy, string POSOHeaderID, string CreatedBy, string MaterialStorageParameterNames, string MaterialStorageParameterValues, string Conversionfactor, string remarks, string LastModifiedBy, string IsActive, int KitplannerID, int TenantID, string SODetailsID, string ISNC, string asIS, int IsPrintRequired, string printerIP, string IsPositiveRecall, string cartonid)
         {


             int cartonidvalue = DB.GetSqlN("select CartonID AS N from INV_Carton where CartonCode=" + DB.SQuote(cartonid));
             int vDeliveryStatus = DB.GetSqlN("SELECT DeliveryStatusID AS N FROM OBD_Outbound WHERE OutboundID=" + OBDID);
             if (vDeliveryStatus > 3)
             {
                 return "Cannot pick the item,as PGI is done";
             }
             Decimal StockoutSum = DB.GetSqlNDecimal("EXEC dbo.sp_INV_GetGoodsOutQuantitySum @MaterialMasterID=" + MaterialMasterID + ",@LocationID=" + LocationID + ",@OutboundID=" + OBDID + ",@linenumber=" + LineNumber + ",@SOHeaderID=" + POSOHeaderID);
             Decimal totalQuantity = Convert.ToDecimal(SOQty);
             if (StockoutSum + Convert.ToDecimal(DocQty) > totalQuantity)
             {

                 return "Pick quantity should not exceed delv. doc. quantity";
             }
             string MaterialStorageParameterIDs = String.Empty;
             if (MaterialStorageParameterNames.Length != 0 && MaterialStorageParameterNames!="null")
             {
                 try
                 {
                     /*IDataReader rsGetData = DB.GetRS("EXEC [sp_INV_GetMaterialStorageParameters] @MCode='" + mcode + "',@TenantID="+TenantID);
                     while (rsGetData.Read())
                     {
                         MaterialStorageParameterIDs += DB.RSFieldInt(rsGetData, "MaterialStorageParameterID").ToString()+",";
                     
                     }*/
                     IDataReader rsGetData = DB.GetRS("select MaterialStorageParameterID from MMT_MaterialStorageParameter where ParameterName in (" + MaterialStorageParameterNames + ") order by MaterialStorageParameterID");
                     while (rsGetData.Read())
                     {
                         MaterialStorageParameterIDs += DB.RSFieldInt(rsGetData, "MaterialStorageParameterID").ToString() + ",";

                     }
                 }
                 catch
                 {
                     return "Error in picking item, please contact support team";
                 }
                 MaterialStorageParameterIDs = MaterialStorageParameterIDs.Remove(MaterialStorageParameterIDs.Length - 1, 1);
             }
             try
             {
                 if (GoodsMovementDetailsID == "0")
                 {
                     if (IsDamaged == "" || IsDamaged == null)
                     {
                         IsDamaged = "0";
                     }
                     int result = InventoryCommonClass.UpdateGoodsMovementDetails(GoodsMovementDetailsID, GoodsMovementTypeID, Location, MaterialMaster_IUoMID, DocQty, Quantity, OBDID.ToString(), "0", LineNumber, MaterialMasterID.ToString(), IsDamaged, hasDiscrepancy, POSOHeaderID, CreatedBy, MaterialStorageParameterIDs, MaterialStorageParameterValues, Conversionfactor, remarks, LastModifiedBy, IsActive, KitplannerID.ToString(), IsPositiveRecall, ISNC, asIS, cartonidvalue.ToString());
                     if (result != 1)
                     {
                         return "Error in picking item,Please contact support team";
                     }
                     try
                     {
                         if (IsPrintRequired == 1)
                         {
                             MRLWMSC21Common.TracklineMLabel thisMLabel = new MRLWMSC21Common.TracklineMLabel();
                             int vMMID = 0;// DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(mcode));
                             string OEMPartNumber = String.Empty;
                             string Description = String.Empty;
                             string DelDocNumber = String.Empty;
                             string sqlcommandforMMDetails = "select MaterialMasterID,isnull(OEMPartNo,'') as OEMPartNo,isnull(MDescription,'') as MDescription ,(select OBDNumber from OBD_Outbound where OutboundID=" + OBDID + ") as DeliveryDocNumber from MMT_MaterialMaster where MCode=" + DB.SQuote(mcode);
                             IDataReader dataReader = DB.GetRS(sqlcommandforMMDetails);
                             while (dataReader.Read())
                             {
                                 vMMID = DB.RSFieldInt(dataReader, "MaterialMasterID");
                                 OEMPartNumber = DB.RSField(dataReader, "OEMPartNo");
                                 Description = DB.RSField(dataReader, "MDescription");
                                 DelDocNumber = DB.RSField(dataReader, "DeliveryDocNumber");

                             }
                             if (IsPositiveRecall == "1")
                             {
                                 thisMLabel.MCode = mcode + "-P";
                             }
                             else
                             {
                                 thisMLabel.MCode = mcode;
                             }

                             thisMLabel.OEMPartNo = OEMPartNumber;
                             thisMLabel.Description = Description;
                             thisMLabel.AltMCode = String.Empty;


                             string kitcode = String.Empty;
                             IDataReader dr = DB.GetRS("[sp_MFG_ProductionDeliveryPickNoteDetails]  @OutboundID=" + OBDID);
                             while (dr.Read())
                             {
                                 kitcode = DB.RSField(dr, "KitCode");
                             }

                             string batchNo = String.Empty;
                             string serialNo = String.Empty;
                             DateTime mfgDate = DateTime.MinValue;
                             DateTime ExpDate = DateTime.MinValue;
                             string[] msps = MaterialStorageParameterIDs.Split(',');
                             string[] mspvalues = MaterialStorageParameterValues.Split(',');
                             for (int index = 0; index < msps.Length; index++)
                             {
                                 if (msps[index] == "1")
                                 {
                                     mfgDate = DateTime.ParseExact(mspvalues[index], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                     //mfgDate = Convert.ToDateTime(mspvalues[index]);
                                 }
                                 else if (msps[index] == "2")
                                 {
                                     ExpDate = DateTime.ParseExact(mspvalues[index], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                 }
                                 else if (msps[index] == "3")
                                 {
                                     serialNo = mspvalues[index];
                                 }
                                 else if (msps[index] == "4")
                                 {
                                     batchNo = mspvalues[index];
                                 }
                             }

                             thisMLabel.MfgDate = mfgDate.ToString();
                             thisMLabel.ExpDate = ExpDate.ToString();

                             thisMLabel.BatchNo = batchNo;
                             thisMLabel.SerialNo = serialNo;

                             thisMLabel.InvQty = Convert.ToDecimal(DocQty);
                             thisMLabel.PrintQty = DocQty;


                             thisMLabel.PrinterType = "IP";
                             thisMLabel.PrinterIP = printerIP;
                             thisMLabel.IsBoxLabelReq = true;
                             thisMLabel.ReqNo = String.Empty;

                             thisMLabel.StrRefNo = String.Empty;
                             thisMLabel.OBDNumber = DelDocNumber;
                             thisMLabel.IsBoxLabelReq = false;

                             thisMLabel.KitCode = kitcode;
                             CommonLogic.PrintLabel(thisMLabel);
                             //Thread worker = new Thread(new ParameterizedThreadStart(this.ThermalLabelWorkerForGoodsOut));
                             //worker.SetApartmentState(ApartmentState.STA);
                             //worker.Name = "ThermalLabelWorker";
                             //worker.Start(thisMLabel);
                             //worker.Join();
                             //if (thisMLabel.Result != "Success")
                             //{
                             //    return "successwithnoprint"; //"Item picked successfully, unable to print the label due to printer unavailability. Please check the printer.";

                             //}
                             //else
                             //{
                             //    return "success";

                             //}

                         }

                     }
                     catch
                     {
                         return "successwithnoprint";//Item picked successfully, unable to print the label due to printer unavailability. Please check the printer.";
                     }


                     return "success";
                 }
                 else
                 {
                     InventoryCommonClass.UpdateReturnGoodsMovementDetails(GoodsMovementDetailsID, Quantity, DocQty, OBDID.ToString(), SODetailsID, Conversionfactor, MaterialMaster_IUoMID, CreatedBy,cartonid);


                     return "success";
                 }


             }
             catch
             {
                 return "Error in picking item,Please contact support team";
             }
         }

         [WebMethod]
        public String GetPOItemProperties(int LineNumber, string MaterialCode, string Storerefno, string PONumber, string InvoiceNumber)
        {
            
             
             List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();
            int MaterialMasterID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where MCode=" + DB.SQuote(MaterialCode));
            int InboundID = DB.GetSqlN("select InboundID as N from INB_Inbound where StoreRefNo=" + DB.SQuote(Storerefno));
            int PoHeaderID;
            if (PONumber != "0")
            {
                PoHeaderID = DB.GetSqlN("select POHeaderID as N from ORD_POHeader where PONumber=" + DB.SQuote(PONumber));
                if (PoHeaderID == 0)
                {

                    return PoHeaderID.ToString();
                }
            }
            else
            {
                PoHeaderID = 0;
            }
            int SupplierInvoiceID;
            if (InvoiceNumber != "0")
            {
                SupplierInvoiceID = DB.GetSqlN("SELECT SupplierInvoiceID AS N  FROM ORD_SupplierInvoice where InvoiceNumber like '%" + InvoiceNumber + "%' AND POHeaderID=" + PoHeaderID + " AND IsActive=1 AND IsDeleted=0");
                if (SupplierInvoiceID == 0)
                {
                    //  sources.Add(new THHTWSData.KeyValueStruct("SupplierInvoiceID", "0"));
                    return SupplierInvoiceID.ToString();
                }
            }
            else
            {
                SupplierInvoiceID = 0;
            }
            
          
            DataSet dsUoMs = DB.GetDS("dbo.sp_INV_GetPOQuantityListForStoreRef @InboundID=" + InboundID + ",@MaterialMasterID=" + MaterialMasterID + ",@POLineNumber=" + LineNumber + ",@POHeaderID=" + PoHeaderID + ",@SupplierInvoiceID=" + SupplierInvoiceID, false);
           
            DataTable dtUoMs = dsUoMs.Tables[0];
            dtUoMs.Namespace = "UoM";
            dtUoMs.TableName = "UoM";



            string jsonVersionDetails = JsonConvert.SerializeObject(dsUoMs, new DataSetConverter());

            return jsonVersionDetails;
        }

         [WebMethod]
         public String GetMaterialPicked(int MaterialMasterID, int LocationID, int OBDID, int SOHeaderID, int LineNumber)
         {
             
             string jsonVersionDetails = null;
             try
             {
                 
                 DataSet ds = DB.GetDS("EXEC [sp_INV_GetStockOutList]@MaterialMasterID=" + MaterialMasterID + ", @LocationID=" + LocationID + ", @OutboundID=" + OBDID + ", @linenumber=" + LineNumber + ",@SOHeaderID=" + SOHeaderID, false);
                 DataTable dt = ds.Tables[0];
                 dt.Namespace = "Pickedlist";
                 dt.TableName = "Pickedlist";



                 jsonVersionDetails = JsonConvert.SerializeObject(ds, new DataSetConverter());

             }
             catch (Exception ex)
             {

             }
             return jsonVersionDetails;

             
         }


        [WebMethod]
        public string getData(string value) {
            return "Hello World" + value;
        }

     

    }
    public class Outbound
    {
        public List<BO.Stockoutitems> stockout
        {
            get;set;
        }
    }

}

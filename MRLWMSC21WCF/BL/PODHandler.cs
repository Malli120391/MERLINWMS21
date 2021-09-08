using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using MRLWMSC21WCF.BO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MRLWMSC21WCF.BL
{
    public class PODHandler
    {
        OBDTrack oBDTrack = new OBDTrack();
        string TenantRootDir = "";
        string OutboundPath = "";
        string OBD_PODPath = "";
        string OBD_DeliveryNotePath = "";
        string OBD_PickandCheckSheetPath = "";
        internal List<PODBO> GetJobOrderList(int Limit)
        {
            try
            {
                DataSet dsJobOrderList = DB.GetDS("EXEC [dbo].[sp_Android_GetJobOrderList] @Limit=" + Limit, false);

                DataTable dt = dsJobOrderList.Tables[0];


                dt.Namespace = "JobOrderList";
                dt.TableName = "JobOrderList";


                //return dsInOutActivities;

                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal PODBO GetUserDetails(string UserName, string Password)
        {

            try
            {

                DataSet dsUserDetails = DB.GetDS("EXEC dbo.[sp_Android_CheckUserLogin] @Email='" + UserName + "', @Password='" + Password + "'", false);

                DataTable dtUserDetails = dsUserDetails.Tables[0];

                dtUserDetails.Namespace = "UserDetails";
                dtUserDetails.TableName = "UserDetails";

                if (dtUserDetails.Rows.Count > 0)
                    return RowToEntity<PODBO>(dtUserDetails.Rows[0]);
                else
                    return new PODBO();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetMenuLinks()
        {
            try
            {

                DataSet dsUser = DB.GetDS("EXEC [dbo].[sp_Android_GetMenuLinks]  ", false);

                DataTable dt = dsUser.Tables[0];


                dt.Namespace = "MenuLinks";
                dt.TableName = "MenuLinks";


                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetInboundStatistics()
        {
            try
            {

                DataSet dsIBStatstics = DB.GetDS("EXEC [dbo].[sp_Android_GetInboundStatistics]  ", false);

                DataTable dtIBStatstics = dsIBStatstics.Tables[0];

                dtIBStatstics.Namespace = "INBStatstics";
                dtIBStatstics.TableName = "INBStatstics";

                if (dtIBStatstics.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dtIBStatstics);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetOutboundStatistics()
        {
            try
            {

                DataSet dsOBStatstics = DB.GetDS("EXEC [dbo].[sp_Android_GetOutboundStatistics]  ", false);

                DataTable dt = dsOBStatstics.Tables[0];


                dt.Namespace = "OBDStatstics";
                dt.TableName = "OBDStatstics";
                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetInOutActivities()
        {

            try
            {

                DataSet dsInOutActivities = DB.GetDS("EXEC [dbo].[sp_Android_GetInOutActivities]  ", false);

                DataTable dt = dsInOutActivities.Tables[0];


                dt.Namespace = "InOutActivities";
                dt.TableName = "InOutActivities";

                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetInboundList(string WarehouseIDs, int ShipmentTypeID, int InboundStatusID, int SupplierID, int Limit, string InboundStatus)
        {
            try
            {
                WarehouseIDs = null;
                // DataSet dsInboundList = DB.GetDS("EXEC [dbo].[sp_Android_INB_GetInboundList]  @WarehouseIDs='" + 0 + "',@InboundStatusID=" + InboundStatusID + ",@ShipmentTypeID=" + ShipmentTypeID + ",@SupplierID=" + SupplierID + ",@Limit=" + Limit, false);
                DataSet dsInboundList = DB.GetDS("EXEC [dbo].[sp_Android_INB_GetInboundList] @Limit=" + Limit + ",@InboundStatus='" + InboundStatus + "'", false);
                DataTable dt = dsInboundList.Tables[0];


                dt.Namespace = "InboundList";
                dt.TableName = "InboundList";

                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetOutboundList(string WarehouseIDs, int DocumentTypeID, int OBDStatusID, int CustomerID, int Limit, string DeliveryStatus)
        {

            try
            {

                DataSet dsOutboundList = DB.GetDS("EXEC  [dbo].[sp_Android_OBD_GetOutboundList]  @WarehouseIDs='" + WarehouseIDs + "',@DocumentTypeID=" + DocumentTypeID + ",@OBDStatusID=" + OBDStatusID + ",@CustomerID=" + CustomerID + ",@Limit=" + Limit + ",@DeliveryStatus='" + DeliveryStatus + "'", false);

                DataTable dt = dsOutboundList.Tables[0];


                dt.Namespace = "OutboundList";
                dt.TableName = "OutboundList";

                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetJobOrderOutboundList(int Limit)
        {
            try
            {

                DataSet OutboundList = DB.GetDS("EXEC  [dbo].[sp_Android_OBD_GetJobOrderOutboundList] @Limit=" + Limit, false);

                DataTable dt = OutboundList.Tables[0];


                dt.Namespace = "JOutboundList";
                dt.TableName = "JOutboundList";

                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetInOutOperations()
        {
            try
            {

                DataSet dsInOutActivities = DB.GetDS("EXEC [dbo].[sp_Android_INOUT_GetInOutOperations]  ", false);

                DataTable dt = dsInOutActivities.Tables[0];


                dt.Namespace = "InOutOperations";
                dt.TableName = "InOutOperations";


                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetLiveStock(string MCode, string Location, string BatchNo, string PageSize)
        {

            try
            {
                DataSet dsInOutActivities = DB.GetDS("EXEC [dbo].[sp_Android_INV_GetLiveStock] @MaterialMaster='" + MCode + "',@Location='" + Location + "',@BatchNo='" + BatchNo + "'" + ",@PageSize=" + PageSize, false);

                DataTable dt = dsInOutActivities.Tables[0];


                dt.Namespace = "LiveStock";
                dt.TableName = "LiveStock";
                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetWarehouses()
        {
            try
            {

                DataSet dsWarehouses = DB.GetDS("EXEC [dbo].[sp_Android_GetWarehouses] ", false);

                DataTable dt = dsWarehouses.Tables[0];


                dt.Namespace = "Warehouses";
                dt.TableName = "Warehouses";

                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetSuppliers()
        {
            try
            {

                DataSet dsSuppliers = DB.GetDS("EXEC [dbo].[sp_Android_GetSuppliers] ", false);

                DataTable dt = dsSuppliers.Tables[0];


                dt.Namespace = "Suppliers";
                dt.TableName = "Suppliers";

                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetShipmentType()
        {
            try
            {
                DataSet dsShipmentType = DB.GetDS("EXEC [dbo].[sp_Android_GetShipmentType] ", false);

                DataTable dt = dsShipmentType.Tables[0];


                dt.Namespace = "ShipmentType";
                dt.TableName = "ShipmentType";

                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetShipmentStatus(int Limit)
        {
            try
            {

                DataSet dsShipmentStatus = DB.GetDS("EXEC [dbo].[sp_Android_GetShipmentStatus]  @Type=" + Limit, false);

                DataTable dt = dsShipmentStatus.Tables[0];


                dt.Namespace = "ShipmentStatus";
                dt.TableName = "ShipmentStatus";


                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetDeliveryStatus()
        {

            try
            {

                DataSet dsDeliveryStatus = DB.GetDS("EXEC [dbo].[sp_Android_GetDeliveryStatus] ", false);

                DataTable dt = dsDeliveryStatus.Tables[0];


                dt.Namespace = "DeliveryStatus";
                dt.TableName = "DeliveryStatus";

                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetCustomers()
        {
            try
            {

                DataSet dsCustomers = DB.GetDS("EXEC [dbo].[sp_Android_GetCustomer] ", false);

                DataTable dt = dsCustomers.Tables[0];


                dt.Namespace = "Customers";
                dt.TableName = "Customers";

                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetDocumentType()
        {
            try
            {

                DataSet dsDocumentType = DB.GetDS("EXEC [dbo].[sp_Android_GetDocumentType] ", false);

                DataTable dtDocumentType = dsDocumentType.Tables[0];

                dtDocumentType.TableName = "DocumentType";

                if (dtDocumentType.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dtDocumentType);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetPartNumberList(int TenantID)
        {
            try
            {

                string mmSql = "SELECT TOP 10 MM.MaterialMasterID, MM.MCode FROM TPL_Tenant_MaterialMaster AS TM JOIN MMT_MaterialMaster AS MM ON TM.MaterialMasterID = MM.MaterialMasterID AND MM.IsActive = 1 AND MM.IsDeleted = 0 AND TM.IsActive = 1 AND TM.IsDeleted = 0 WHERE TM.TenantID =" + TenantID + "";
                List<string> mmList = new List<string>();

                //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

                List<PODBO> _lstPOBO = new List<PODBO>();
                IDataReader rsMCodeList = DB.GetRS(mmSql);

                while (rsMCodeList.Read())
                {
                    _lstPOBO.Add(new PODBO() { MCode = rsMCodeList["MCode"].ToString(), MaterialMasterID = rsMCodeList["MaterialMasterID"].ToString() });
                }

                rsMCodeList.Close();
                return _lstPOBO;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetVersionDetails()
        {

            try
            {

                DataSet dsVersionDetails = DB.GetDS("EXEC [dbo].[sp_Android_GetVersionDetails] ", false);

                DataTable dtVersionDetails = dsVersionDetails.Tables[0];

                dtVersionDetails.TableName = "VersionDetails";

                if (dtVersionDetails.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dtVersionDetails);
                else
                    return new List<PODBO>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal List<PODBO> GetMaterialStorageParameters(string MaterialCode, int TenantID)
        {
            try
            {
                DataSet dsmaterialparams = DB.GetDS("EXEC [sp_INV_GetMaterialStorageParameters] @MCode='" + MaterialCode + "',@TenantID=1", false);

                DataTable dtmaterialparams = dsmaterialparams.Tables[0];
                dtmaterialparams.Namespace = "MaterialParams";
                dtmaterialparams.TableName = "MaterialParams";

                if (dtmaterialparams.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dtmaterialparams);
                else
                    return new List<PODBO>();
            }
            catch (Exception)
            {

                throw;
            }

        }

        internal List<PODBO> GetMaterialReceived(string LineNumber, string MaterialCode, string StoreRefNo, string PONumber, string SupplierInvoieID)
        {
            try
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
                if (dt.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dt);
                else
                    return new List<PODBO>();
            }
            catch (Exception)
            {

                throw;
            }

        }

        internal string UpdateReceiveItem(string GoodsMovementDetailsID, string GoodsMovementTypeID, string Location, string MaterialMaster_IUoMID, string DocQty, string Quantity, string Storerefnum, string LineNumber, string Mcode, string IsDamaged, string HasDiscrepancy, string POSOHeaderID, string CreatedBy, string MaterialStorageParameterIDs, string MaterialStorageParameterValues, string Conversionfactor, string Remarks, string LastModifiedBy, string IsActive, string KitplannerID, string InvoiceQuantity, string SupplierInvoiceID, string IsPositiveRecall, int IsPrintRequired, string PrinterIP, string SerialNumber)
        {
            try
            {
                int vInboundID = DB.GetSqlN("select InboundID as N from INB_Inbound where StoreRefNo=" + DB.SQuote(Storerefnum));
                if (SerialNumber.Length != 0)
                {
                    string QueryforSerialNumberCheck = "select GMDMSP.GoodsMovementDetails_MaterialStorageParameterID as N from INV_GoodsMovementDetails GMD " +
                             "JOIN INV_GoodsMovementDetails_MMT_MaterialStorageParameter GMDMSP ON GMD.GoodsMovementDetailsID=GMDMSP.GoodsMovementDetailsID " +
                             "AND GMDMSP.MaterialStorageParameterID=3 AND GMDMSP.IsDeleted=0 WHERE GMD.GoodsMovementTypeID=1 AND GMD.IsDeleted=0 AND " +
                             "GMD.TransactionDocID=" + vInboundID + " AND CONVERT(NVARCHAR,GMDMSP.Value)='" + SerialNumber + "'";
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

                string sqlcommandforMMDetails = "select MM.MaterialMasterID,isnull(MM.OEMPartNo,'') as OEMPartNo,isnull(MM.MDescription,'') as MDescription  from MMT_MaterialMaster MM JOIN TPL_Tenant_MaterialMaster TPL_MM ON TPL_MM.MaterialMasterID=MM.MaterialMasterID AND TPL_MM.IsDeleted=0 AND TPL_MM.IsActive=1  JOIN INB_Inbound INB ON INB.TenantID=TPL_MM.TenantID AND INB.IsDeleted=0 AND INB.IsActive=1  where INB.StoreRefNo='" + Storerefnum + "' AND MM.MCode=" + DB.SQuote(Mcode);

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
                    int result = InventoryCommonClass.UpdateGoodsMovementDetails(GoodsMovementDetailsID, GoodsMovementTypeID, "", MaterialMaster_IUoMID, DocQty, Quantity, vInboundID.ToString(), SupplierInvoiceID, LineNumber, vMMID.ToString(), IsDamaged, HasDiscrepancy, POSOHeaderID, CreatedBy, MaterialStorageParameterIDs, MaterialStorageParameterValues, Conversionfactor, Remarks, LastModifiedBy, IsActive, KitplannerID, IsPositiveRecall, "0", "0", CARTONid.ToString());
                    if (result == 1)
                    {
                        try
                        {
                            if (IsPrintRequired == 1)
                            {
                                //For Printing Lables
                                MRLWMSC21Common.TracklineMLabel thisMLabel = new MRLWMSC21Common.TracklineMLabel();

                                thisMLabel.MCode = Mcode;
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
                                thisMLabel.PrinterIP = PrinterIP;
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
            catch (Exception)
            {

                throw;
            }
        }

        internal List<PODBO> GetPOList(string StatusName)
        {
            try
            {
                DataSet dsmaterialparams = DB.GetDS("EXEC [sp_Android_POHeaderList] @StatusName='" + StatusName + "'", false);

                DataTable dtmaterialparams = dsmaterialparams.Tables[0];

                if (dtmaterialparams.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dtmaterialparams);
                else
                    return new List<PODBO>();
            }
            catch (Exception)
            {

                throw;
            }

        }

        internal List<PODBO> GetSOList(string StatusName)
        {
            try
            {
                DataSet dsmaterialparams = DB.GetDS("EXEC [sp_Android_SOHeaderList] @StatusName='" + StatusName + "'", false);

                DataTable dtmaterialparams = dsmaterialparams.Tables[0];

                if (dtmaterialparams.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dtmaterialparams);
                else
                    return new List<PODBO>();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<PODBO> GetMaterialType(int TenantID)
        {
            string mmSql = "SELECT TOP 10 MType,MTypeID FROM MMT_MType WHERE IsActive=1 AND IsDeleted=0 AND TenantID =" + TenantID;
            List<PODBO> mmList = new List<PODBO>();
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(new PODBO()
                {
                    MType = rsMCodeList["MType"].ToString(),
                    MTypeID = rsMCodeList["MTypeID"].ToString()
                });
            }

            rsMCodeList.Close();
            return mmList;
        }

        public List<PODBO> GetWHForWHList(int TenantID)
        {
            List<PODBO> mmList = new List<PODBO>();

            string mmSql = "SELECT DISTINCT TC.WarehouseID,WH.WHCode,TC.TenantID FROM TPL_Tenant_Contract AS TC JOIN  GEN_Warehouse AS WH ON TC.WarehouseID = WH.WarehouseID AND TC.IsActive=1 AND TC.IsDeleted=0 AND WH.IsActive=1 AND WH.IsDeleted=0 AND FORMAT(GETDATE(),'yyyy-MM-dd') BETWEEN FORMAT(TC.EffectiveFrom,'yyyy-MM-dd') AND  FORMAT(TC.EffectiveTo,'yyyy-MM-dd')";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(new PODBO()
                {
                    WHCode = rsMCodeList["WHCode"].ToString(),
                    WarehouseID = rsMCodeList["WarehouseID"].ToString()
                });
            }

            rsMCodeList.Close();
            return mmList;
        }

        public PODBO PODDelivery(PODBO OBDTrack)
        {
            PODBO mmList = new PODBO();

            //{
            //    resetError("Sorry, only Delivery-Incharge can update this delivery", true);
            //    return;
            //}

            //Page.Validate("Receivedelivery");

            //if (Page.IsValid == false)
            //{
            //    resetError("Please check for mandatory fields", true);
            //    return;

            //}


            //if (ddlReceivingStore.SelectedValue == "0")
            //{
            //    resetError("Please select a store", true);
            //    ddlReceivingStore.Focus();
            //    return;
            //}

            //if (IsDCRReceived.Checked && !fucODeliveryConfirmReciept.HasFile)
            //{
            //    resetError("Attach a valid file if 'Is Proof of Delivery (POD) received from the customer?' is checked", true);
            //    return;
            //}

            //if (OBDTrack.DeliveryStatusID == 4 && !IsDCRReceived.Checked)
            //{
            //    resetError("Delivery is already made. Please attach the Proof of Delivery (POD) to process this request", true);
            //    return;
            //}

            //<!-------------------Procedure Converting--------------->
            // int REF_WHID = DB.GetSqlN("select OB_RefWarehouse_DetailsID AS N from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and WarehouseID=" + ddlReceivingStore .SelectedValue + " and OutboundID=" + OBDTrack.OBDTrackingID.ToString());
            DataSet ds = DB.GetDS("Exec [dbo].[USP_Android_GetREF_WHID] @OutboundID = " + OBDTrack.OBDTrackingID.ToString(), false);
            int REF_WHID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            int WH_ID = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
            OBDTrack.TenantID = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
            //if (OBDTrack.IsDCRReceived != 0)
            //{
            //    if (DB.GetSqlN("Select DeliveredBy as N from OBD_OutboundTracking_Warehouse Where OutboundID =" + OBDTrack.OBDTrackingID.ToString() + " AND  OB_RefWarehouse_DetailsID=" + REF_WHID) != 0)
            //    {
            //        resetError("The selected store (" + ddlReceivingStore.SelectedItem.Text + ") already delivered  . Please check for another store from the 'Receiving Store' dropdown", true);
            //        return;
            //    }
            //}

            //if (IsDCRReceived.Checked && DB.GetSqlTinyInt("select IsPODReceived AS N from OBD_OutboundTracking_Warehouse where OB_RefWarehouse_DetailsID=" + REF_WHID + " AND OutboundID=" + OBDTrack.OBDTrackingID) == 1)
            //{
            //    resetError("POD already attached", true);
            //    return;
            //}

            OBDTrack.InstructionModeID = "1";
            OBDTrack.Requester = "";
            OBDTrack.DocumentNumber = "";


            string ShipDate = OBDTrack.DeliveryDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");

            // OBDTrack.DocumentReceivedDate = ((txtDocRcvdDate.Text.Trim() != "") ? DateTime.ParseExact(txtDocRcvdDate.Text.Trim() , "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : null);
            OBDTrack.DocumentReceivedDate = DateTime.Now.ToString("dd/MM/yyyy");


            // OBDTrack.DocumentReceivedDate = (txtDocRcvdDate.Text != ""  ? DateTime.ParseExact(txtDocRcvdDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy") : null); 
            OBDTrack.DeliveredBy = Convert.ToInt32(OBDTrack.UserID);
            OBDTrack.DriverName = OBDTrack.UserID;
            //OBDTrack.DeliveryDate = DateTime.ParseExact(DateTime.Now.ToString() + " " + DateTime.Now.ToString("HH:mm:ss tt"), "dd-MMM-yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy hh:mm tt");
            //OBDTrack.DeliveryDate = DateTime.ParseExact(ShipDate, "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None).ToString();
            //OBDTrack.DeliveryDate = ((txtDeliveryDate.Text.Trim() != "" && txtReceivedDelTimeEntry.Text.Trim() != "") ? DateTime.ParseExact(txtDeliveryDate.Text.Trim() + " " + txtReceivedDelTimeEntry.Text.Trim(), "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm tt") : null);
            OBDTrack.ReceivedBy = OBDTrack.UserID;
            OBDTrack.RemBy_DeliveryIncharge = "";

            OBDTrack.RefWHID = REF_WHID;
            //Need to pass warehouse ID
            OBDTrack.TransferedtoStoreID = 4;
            Boolean DCRUploaded = true;
            OBDTrack.Outbound = Convert.ToInt32(OBDTrack.OBDTrackingID);

            //if (fucODeliveryConfirmReciept.HasFile)
            //{
            //    String FileExtenion = Path.GetExtension(fucODeliveryConfirmReciept.FileName);

            //    if (FileExtenion == ".pdf")
            //    {
            //        DCRUploaded = true;
            //    }
            //    else
            //    {
            //        resetError("Please attach a valid PDF file", true);
            //    }
            //}

            if (DCRUploaded)
            {
                OBDTrack.IsDCRReceived = 1;

            }
            else
                OBDTrack.IsDCRReceived = 0;


            OBDTrack.DeliveryStatusID = "4";
            OBDTrack.DeliveredBy = Convert.ToInt32(OBDTrack.UserID);
            LoadFromDB(Convert.ToInt32(OBDTrack.OutboundID), 1);
            int Result = UpdateOBDTracking_Delivery(OBDTrack);
            if (DCRUploaded)
            {
                //string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=2";

                //DataSet dsPath = DB.GetDS(query.ToString(), false);
                string querydire = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=2";

                DataSet dsPath1 = DB.GetDS(querydire.ToString(), false);

                TenantRootDir = dsPath1.Tables[0].Rows[4][0].ToString();
                OutboundPath = dsPath1.Tables[0].Rows[0][0].ToString();
                OBD_DeliveryNotePath = dsPath1.Tables[0].Rows[1][0].ToString();
                OBD_PickandCheckSheetPath = dsPath1.Tables[0].Rows[2][0].ToString();
                OBD_PODPath = dsPath1.Tables[0].Rows[3][0].ToString();
                FileUpload fucODeliveryConfirmReciept = null; // Need to change
                //3  - WarehouseID need to change
                PODImageUpload.PODUploadSoapClient client = new PODImageUpload.PODUploadSoapClient();
                //if (CommonLogic.UploadFile(TenantRootDir + 1 + OutboundPath + OBD_PODPath, OBDTrack.FileConent, OBDTrack.WebURL) == false)
                if (client.UploadFile(TenantRootDir + Convert.ToInt32(OBDTrack.TenantID) + OutboundPath + OBD_PODPath, OBDTrack.FileConent, OBDTrack.OBDNumber + "_" + 1 + ".pdf") == false)
                {
                    OBDTrack.Result = "Error while attaching POD";
                    //resetError("Error while attaching POD", true);
                    //return;
                }
                if (Result == -1)
                {
                    OBDTrack.Result = "Error while updating delivery details";
                    //resetError("Error while updating delivery details", true);
                    //return;

                }
            }


            if (Result == -1)
            {
                OBDTrack.Result = "Error while updating delivery details";
                //resetError("Error while updating delivery details", true);
                //return;
            }

            else if (Result == -2)
            {
                OBDTrack.Result = "POD already attached";
                //String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + hifTenant.Value + OutboundPath + OBD_PODPath, OBDTrack.OBDNumber.ToString() + "_" + Convert.ToInt32(ddlReceivingStore.SelectedValue));
                //if (sFileName.Contains(".pdf"))
                //{
                //    LoadReceivedDeliveryDetails(REF_WHID, Convert.ToInt32(ddlReceivingStore.SelectedValue));
                //    resetError("POD already attached", true);
                //}
                //return;
            }
            //else if (Result == -3)
            //{
            //    OBDTrack.Result = "POD already attached";
            //    //resetError("The selected store (" + ddlReceivingStore.SelectedItem.Text + ") already delivered  . Please check for another store from the 'Receiving Store' dropdown", true);
            //    //return;
            //}
            //LoadReceivedDeliveryDetails(REF_WHID, Convert.ToInt32(ddlReceivingStore.SelectedValue));
            //resetError("Delivery details successfully updated", false);

            //lnkAddNewUsedVehicle.Visible = true;

            //ValidateOutboundStatus();
            OBDTrack.Result = "Signature captured successfully";

            return OBDTrack;
        }

        public static bool UploadFile(String PathName, FileUpload FileControl, String FileName)
        {

            bool status = false;

            if (FileControl.HasFile)
            {
                try
                {
                    var _Path = System.Web.HttpContext.Current.Server.MapPath("~/" + PathName);

                    var _Directory = new DirectoryInfo(_Path);

                    if (_Directory.Exists == false)
                    {
                        _Directory.Create();
                    }

                    var file = Path.Combine(_Path, FileName);

                    if (File.Exists(file))
                        File.Delete(file);

                    FileControl.SaveAs(file);

                    status = true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return status;
        }

        public void LoadFromDB(int OutBoundID, int StoreID)
        {

            String GetOBDTrackSQL = "EXEC [dbo].[sp_OBD_GetOutboundTrackingWHDetails] @OutboundID=" + OutBoundID.ToString() + " , @RefStoreID=" + StoreID.ToString();
            IDataReader rs = DB.GetRS(GetOBDTrackSQL);

            if (!rs.Read())
            {

                oBDTrack.OBDReceivedDT = null;
                oBDTrack.NoofLines = 0;
                oBDTrack.TotalQuantity = 0;
                oBDTrack.SentForPGI_DT = null;
                oBDTrack.RemBy_StoreIncharge = "";
                oBDTrack.StoreInchargeID = 0;
                oBDTrack.PickedBy = 0;
                oBDTrack.OBDTrack_WHID = 0;
                oBDTrack.CheckedBy = "";

                oBDTrack.InstructionModeID = 0;
                oBDTrack.Requester = "";
                oBDTrack.DocumentReceivedDate = null;
                oBDTrack.TransferedtoStoreID = 0;
                oBDTrack.DocumentNumber = "";
                oBDTrack.DocumentReceivedDate = null;
                oBDTrack.DeliveredBy = 0;
                oBDTrack.DriverName = "";
                oBDTrack.DeliveryDate = null;
                oBDTrack.DelivertTime = null;
                oBDTrack.ReceivedBy = "";
                oBDTrack.IsDCRReceived = 0;
                oBDTrack.RemBy_DeliveryIncharge = "";

            }
            else
            {


                oBDTrack.OBDReceivedDT = DB.RSFieldDateTime(rs, "OBDReceivedOn").ToString();
                oBDTrack.NoofLines = DB.RSFieldInt(rs, "NoofLines");
                oBDTrack.TotalQuantity = Convert.ToInt32(DB.RSFieldDecimal(rs, "TotalQuantity"));
                oBDTrack.SentForPGI_DT = DB.RSFieldDateTime(rs, "SentForPGIOn").ToString();
                oBDTrack.RemBy_StoreIncharge = DB.RSField(rs, "RemByStoreIncharge");
                oBDTrack.StoreInchargeID = DB.RSFieldInt(rs, "StoreInchargeID");
                oBDTrack.CheckedBy = DB.RSFieldInt(rs, "CheckedBy").ToString();
                oBDTrack.DocumentReceivedDate = DB.RSFieldDateTime(rs, "DocumentReceivedDate").ToLongDateString();
                //oBDTrack.DocumentReceivedDate = ((DB.RSFieldDateTime(rs, "DocumentReceivedDate") == DateTime.MinValue) ? DateTime.Now.ToString("dd-MMM-yyyy") : DB.RSFieldDateTime(rs, "DeliveryDate").ToString().Split(' ')[0]);
                oBDTrack.PickedBy = DB.RSFieldInt(rs, "PickedBy");
                oBDTrack.TransferedtoStoreID = DB.RSFieldInt(rs, "TransferedToWarehouseID");
                oBDTrack.OBDTrack_WHID = DB.RSFieldInt(rs, "OutboundTracking_WarehouseID");

                oBDTrack.InstructionModeID = DB.RSFieldInt(rs, "InstructionModeID");
                oBDTrack.Requester = DB.RSField(rs, "Requester").ToString();
                oBDTrack.DocumentNumber = DB.RSField(rs, "DocumentNumber");
                oBDTrack.DeliveredBy = DB.RSFieldInt(rs, "DeliveredBy");
                oBDTrack.DriverName = DB.RSField(rs, "DriverName");

                oBDTrack.DeliveryDate = DB.RSFieldDateTime(rs, "DeliveryDate").ToString();
                oBDTrack.DelivertTime = DB.RSField(rs, "DeliveryTime").ToString();
                //oBDTrack.DeliveryDate = ((DB.RSFieldDateTime(rs, "DeliveryDate") == DateTime.MinValue) ? DateTime.Now.ToString("dd-MMM-yyyy") : DB.RSFieldDateTime(rs, "DeliveryDate").ToString().ToString().Split(' ')[0]);
                oBDTrack.ReceivedBy = DB.RSField(rs, "ReceivedBy");
                oBDTrack.IsDCRReceived = DB.RSFieldTinyInt(rs, "IsPODReceived");
                oBDTrack.RemBy_DeliveryIncharge = DB.RSField(rs, "RemByDeliveryIncharge");


            }



            rs.Close();


        }

        public int UpdateOBDTracking_Delivery(PODBO OBDTrack)
        {
            try
            {
                //UPDATE  OBD Delivered Transaction
                StringBuilder strUpdateOBDTracking = new StringBuilder(2500);

                strUpdateOBDTracking.Append("DECLARE @NewUpdateOutboundID int;  ");
                strUpdateOBDTracking.Append("EXEC [dbo].[sp_OBD_UpsertPODStatus] ");
                strUpdateOBDTracking.Append("  @OBDNumber='" + OBDTrack.Outbound.ToString() + "',");
                strUpdateOBDTracking.Append("@DeliveryStatusID=4 ,");
                strUpdateOBDTracking.Append("@NewOutboundID=@NewUpdateOutboundID OUTPUT,");
                strUpdateOBDTracking.Append("@UpdatedBy=" + OBDTrack.UserID.ToString());
                strUpdateOBDTracking.Append("  select @NewUpdateOutboundID AS N ;");

                //strUpdateOBDTracking.Append("DECLARE @NewUpdateOutboundID int;  ");
                //strUpdateOBDTracking.Append("EXEC [sp_OBD_UpsertDelivery] ");
                //strUpdateOBDTracking.Append("  @OutboundID=" + OBDTrack.Outbound.ToString() + ",");
                //strUpdateOBDTracking.Append("@TransferedToWarehouseID=" + (OBDTrack.TransferedtoStoreID == 0 ? "NULL" : OBDTrack.TransferedtoStoreID.ToString()) + ",");
                //strUpdateOBDTracking.Append("@InstructionModeID=" + (OBDTrack.InstructionModeID == "0" ? "NULL" : OBDTrack.InstructionModeID.ToString()) + ",");
                //strUpdateOBDTracking.Append("@Requester=" + (OBDTrack.Requester == "" ? "NULL" : DB.SQuote(OBDTrack.Requester)) + ",");
                //strUpdateOBDTracking.Append("@DocumentNumber=" + (OBDTrack.DocumentNumber == "" ? "NULL" : DB.SQuote(OBDTrack.DocumentNumber)) + ",");
                ////strUpdateOBDTracking.Append("@DocumentReceivedDate=" + (OBDTrack.DocumentReceivedDate == null ? "NULL" : DB.SQuote(OBDTrack.DocumentReceivedDate)) + ",");
                ////strUpdateOBDTracking.Append("@DeliveryDate=" + (OBDTrack.DeliveryDate == null ? "NULL" : DB.SQuote(OBDTrack.DeliveryDate)) + ",");
                //strUpdateOBDTracking.Append("@DeliveredBy=" + OBDTrack.DeliveredBy.ToString() + ",");
                //strUpdateOBDTracking.Append("@DriverName=" + (OBDTrack.DriverName == "" ? "NULL" : DB.SQuote(OBDTrack.DriverName)) + ",");
                //strUpdateOBDTracking.Append("@ReceivedBy=" + (OBDTrack.ReceivedBy == "" ? "NULL" : DB.SQuote(OBDTrack.ReceivedBy)) + ",");
                //strUpdateOBDTracking.Append("@RemByDeliveryIncharge=" + (OBDTrack.RemBy_DeliveryIncharge == "" ? "NULL" : DB.SQuote(OBDTrack.RemBy_DeliveryIncharge)) + ",");
                //strUpdateOBDTracking.Append("@IsPODReceived=" + OBDTrack.IsDCRReceived.ToString() + ",");
                //strUpdateOBDTracking.Append("@OB_RefWarehouse_DetailsID=" + OBDTrack.RefWHID.ToString() + ",");
                //strUpdateOBDTracking.Append("@DeliveryStatusID=4 ,");
                ////strUpdateOBDTracking.Append("@WareHouseID=" + oBDTrack.TransferedtoStoreID + ",");
                //strUpdateOBDTracking.Append("@NewOutboundID=@NewUpdateOutboundID OUTPUT,");
                //strUpdateOBDTracking.Append("@UpdatedBy=" + OBDTrack.UserID.ToString());
                //strUpdateOBDTracking.Append("  select @NewUpdateOutboundID AS N ;");

                int Result = DB.GetSqlN(strUpdateOBDTracking.ToString());

                //DB.ExecuteSQL("[dbo].[sp_INB_CloseSOStatus]  @OutboundID=" + oBDTrack.OBDTrackingID + ",@SOStatusID=4,@Status=1");
                return Result;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }



        //public string GetPartNumberList()
        //{

        //    try
        //    {

        //        DataSet dsPartNumberList = DB.GetDS("EXEC [dbo].[sp_Android_GetPartNumberList] ", false);

        //        DataTable dtPartNumberList = dsPartNumberList.Tables[0];

        //        dtPartNumberList.TableName = "PartNumberList";

        //        string jsonPartNumberList = JsonConvert.SerializeObject(dsPartNumberList, new DataSetConverter());

        //        return jsonPartNumberList;

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public static T RowToEntity<T>(DataRow dr)
        {
            T item = GetItem<T>(dr);
            return item;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            try
            {

                List<T> data = new List<T>();
                foreach (DataRow row in dt.Rows)
                {
                    T item = GetItem<T>(row);
                    data.Add(item);
                }
                return data;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static T GetItem<T>(DataRow dr)
        {
            try
            {

                Type temp = typeof(T);
                T obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name == column.ColumnName)
                        {
                            object value = dr[column.ColumnName];
                            if (value == DBNull.Value)
                                value = null;
                            pro.SetValue(obj, value, null);
                        }
                        else
                            continue;
                    }
                }
                return obj;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        internal List<PODBO> GetPendingOutboundList(int UserID)
        {
            try
            {

                DataSet dsPendingOutbound = DB.GetDS("SELECT OutboundID,OBDNumber FROM OBD_Outbound WHERE DeliveryStatusID = 7 AND CreatedBy =" + UserID, false);

                DataTable dtmaterialparams = dsPendingOutbound.Tables[0];

                if (dtmaterialparams.Rows.Count > 0)
                    return ConvertDataTable<PODBO>(dtmaterialparams);
                else
                    return new List<PODBO>();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<PODBO> GetTenants(int WarehouseId)
        {
            List<PODBO> mmList = new List<PODBO>();

            string mmSql = "select tnt.TenantId,tnt.TenantName from GEN_Warehouse WH join gen_account acc on WH.AccountID=acc.AccountID and wh.isactive=1 and wh.IsDeleted=0 join TPL_Tenant tnt on acc.AccountID=tnt.AccountID and acc.isactive=1 and acc.isdeleted=0 where tnt.isactive=1 and tnt.isdeleted=0 and WH.WarehouseID=" + WarehouseId + "";

            IDataReader rsTenantList = DB.GetRS(mmSql);

            while (rsTenantList.Read())
            {
                mmList.Add(new PODBO()
                {
                    TenantName = rsTenantList["TenantName"].ToString(),
                    TenantID = Convert.ToInt16(rsTenantList["TenantId"].ToString())
                });
            }

            rsTenantList.Close();
            return mmList;
        }

        public List<PODBO> GetCurrentStock(PODBO _requestPOBO)
        {
            List<PODBO> mmList = new List<PODBO>();

            DataSet ds = DB.GetDS("Exec  [dbo].[sp_Android_GetCurrentStockReportDynamic] @TenantID=" + _requestPOBO.TenantID + ", @MTypeID =" + _requestPOBO.MTypeID + ",@MaterialMasterID=" + _requestPOBO.MaterialMasterID + ", @WarehouseID=" + _requestPOBO.WarehouseID + " ", false);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PODBO pODBO = new PODBO();
                pODBO.WHCode = dr["WH"].ToString();
                pODBO.PartNumber = dr["Part#"].ToString();
                pODBO.Bin = dr["Bin"].ToString();
                pODBO.SLoc = dr["SLoc"].ToString();
                pODBO.QuantOfHandHeld = dr["QoH"].ToString();
                mmList.Add(pODBO);
            }

            return mmList;
        }
    }


}

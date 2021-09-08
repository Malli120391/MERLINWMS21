using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MRLWMSC21Common;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Data.OleDb;
using System.IO; 
using MRLWMSC21Common;
//using Newtonsoft.Json.Converters;
//using Newtonsoft.Json;


namespace MRLWMSC21.mWebServices
{
    /// <summary>
    /// Summary description for FinchWebService
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class FalconWebService : System.Web.Services.WebService
    {
        public string displayStat = "";
        public static IDictionary<String, GpsPosition> MobilePositionList = new Dictionary<String, GpsPosition>();
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public class GpsPosition
        {

            public GpsPosition(string latitude, string longitude)
            {
                this.latitude = latitude;
                this.longitude = longitude;
            }
            public string latitude = "", longitude = "";
        }


        [WebMethod(EnableSession = true)]
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
        public String GetDeviceLatLong(String DeviceID)
        {

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
                return "No DataBind";
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UpdateContainerGPSLocation(string EPCCode, string GPSData)
        {
            string[] latAndLong = GPSData.Split(',');
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("declare @NewResult int ");
            sbSqlString.Append(" EXEC  [dbo].[sp_CNT_UpsertGoodsMovementMasterDetails]");
            sbSqlString.Append("@EPCCode=" + DB.SQuote(EPCCode));
            sbSqlString.Append(" ,@Latitude=" + latAndLong[0]);
            sbSqlString.Append(" ,@Langitude=" + latAndLong[1]);
            sbSqlString.Append(",@Result=@NewResult OUTPUT ");
            sbSqlString.Append("SELECT @NewResult AS N");

            try
            {

                int result = DB.GetSqlN(sbSqlString.ToString());
                if (result == -1)
                {
                    return "Invalid EPC Code";
                }
                else
                {
                    return "Success";
                }

            }
            catch
            {
                return "Excepton";
            }

        }







        #region     -----------------   Developed By Naresh   -------------------------

        [WebMethod]
        //[ScriptMethod(UseHttpGet=true,ResponseFormat = ResponseFormat.Xml)]
        public void HelloWorld()
        {

            //DataSet dsVersionDetails = DB.GetDS("select MCode,MDescription from MMT_MaterialMaster", false);




            //DataTable dtVersionDetails = dsVersionDetails.Tables[0];

            //dtVersionDetails.TableName = "VersionDetails";

            //string jsonVersionDetails = JsonConvert.SerializeObject(dsVersionDetails, new DataSetConverter());





            //this.Context.Response.ContentType = "application/json; charset=utf-8";
            //this.Context.Response.Write(jsonVersionDetails);


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Print_Location_2x1p5(DataTable dtLocations, out String result)
        {


            try
            {

                MRLWMSC21Common.CommonLogic.SendPrintJob_Location_2x1p5("", dtLocations, "", out result);

                return "Successfully Printed";
            }
            catch (Exception ex)
            {
                result = "";
                return "Error while printing";
            }
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetMCode(string prefix, string TenantID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "SELECT Top 10 MM.MCode, MM.MaterialMasterID FROM MMT_MaterialMaster MM LEFT OUTER JOIN TPL_Tenant_MaterialMaster TMM ON TMM.MaterialMasterID = MM.MaterialMasterID AND TMM.IsActive = 1 AND TMM.IsDeleted = 0 LEFT OUTER JOIN TPL_Tenant TNT ON TNT.TenantID = TMM.TenantID AND TNT.IsActive = 1 AND TNT.IsDeleted = 0 where MM.IsDeleted = 0 and MM.IsActive = 1  AND MCode like '" + prefix + "%' AND TNT.AccountID = case when 0 = " + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetMCodeForWHStock(string prefix, string TenantID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "SELECT Top 10 MCode, MaterialMasterID FROM MMT_MaterialMaster WHERE TenantID = " + TenantID + " and IsDeleted = 0 and IsActive = 1  AND MCode like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetMCode1(string prefix, int SupplierID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "SELECT Top 10 MM.MCode, MM.MaterialMasterID FROM MMT_MaterialMaster MM LEFT OUTER JOIN MMT_MaterialMaster_Supplier TMM ON TMM.MaterialMasterID = MM.MaterialMasterID AND TMM.IsActive = 1 AND TMM.IsDeleted = 0 AND MM.IsActive = 1 AND MM.IsDeleted = 0 LEFT OUTER JOIN MMT_Supplier sup on sup.SupplierID=TMM.SupplierID  AND sup.IsActive = 1 AND sup.IsDeleted = 0 left outer join  TPL_Tenant TNT ON TNT.TenantID = TMM.TenantID AND TNT.IsActive = 1 AND TNT.IsDeleted = 0 where MM.IsDeleted = 0 and MM.IsActive = 1 and TMM.SupplierID=" + SupplierID + "  AND MCode like '" + prefix + "%' AND TNT.AccountID = case when 0 = " + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetCusPOInvoiceNoList(string prefix, string CustomerPOID, string InvoiceNo, string ActualCustomerPOID)
        {
            List<string> mmList = new List<string>();

            CustomerPOID = (CustomerPOID == "" ? "0" : CustomerPOID);
            ActualCustomerPOID = (ActualCustomerPOID == "" ? "0" : ActualCustomerPOID);

            //string mmSql = "select SOD.InvoiceNo from ORD_SODetails SOD left join OBD_Outbound_ORD_CustomerPO OBD_CUS ON OBD_CUS.CustomerPOID=SOD.CustomerPOID AND LTRIM(RTRIM(SOD.InvoiceNo))=LTRIM(RTRIM(OBD_CUS.InvoiceNo))  AND OBD_CUS.IsActive=1 AND OBD_CUS.IsDeleted=0 where SOD.IsActive=1 AND SOD.IsDeleted=0 AND OBD_CUS.Outbound_CustomerPOID IS NULL AND SOD.CustomerPOID=" + CusPOID + " AND    SOD.InvoiceNo like '" + prefix + "%'";

            string mmSql = "EXEC [dbo].[sp_OBD_GetDataForCustomerPo] @Type=2, @CustomerPOID=" + CustomerPOID + ",@InvoiceNo=" + DB.SQuote(InvoiceNo) + ",@ActualCustomerPOID=" + ActualCustomerPOID + ",@prefix='%" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["InvoiceNo"], rsMCodeList["InvoiceNo"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadOpenKitCodes(string Prefix)
        {
            List<string> KitCodeList = new List<string>();
            string cMdKitCodeList = "select distinct top 10  KitCode from MFG_ProductionOrderHeader where IsDeleted=0 and ProductionOrderStatusID=2 AND ProductionOrderTypeID!=7 and KitCode like '" + Prefix + "%'";

            IDataReader rsKitCodeList = DB.GetRS(cMdKitCodeList);

            while (rsKitCodeList.Read())
            {
                KitCodeList.Add(string.Format("{0}", rsKitCodeList["KitCode"]));
            }
            rsKitCodeList.Close();
            return KitCodeList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadProhRefNoBasedonKitCode(string Prefix)
        {
            List<String> PRONumberList = new List<String>();
            String cMdPRONumber = "select distinct top 10 MCode+'-'+MMV.Revision+' '+LEFT(RDT.RoutingDocumentType,1) as PRORefNo,PROH.ProductionOrderHeaderID from MFG_ProductionOrderHeader PROH JOIN MFG_SOPO_ProductionOrder SOPO ON SOPO.ProductionOrderHeaderID=PROH.ProductionOrderHeaderID AND SOPO.SOPOTypeID=2 AND SOPO.IsDeleted=0 JOIN MMT_MaterialMaster_Revision MMV ON MMV.MaterialMasterRevisionID=PROH.MaterialMasterRevisionID AND MMV.IsActive=1 AND MMV.IsDeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMV.MaterialMasterID JOIN MFG_RoutingHeader_Revision ROUV ON ROUV.RoutingHeaderRevisionID=PROH.RoutingHeaderRevisionID AND ROUV.IsDeleted=0 JOIN MFG_RoutingDocumentType RDT ON RDT.RoutingDocumentTypeID=ROUV.RoutingDocumentTypeID where PROH.IsDeleted=0 and PROH.IsActive=1 AND  PROH.ProductionOrderStatusID=2 AND KitCode='" + Prefix + "'";
            IDataReader rsPRONumberList = DB.GetRS(cMdPRONumber);
            while (rsPRONumberList.Read())
            {
                PRONumberList.Add(String.Format("{0},{1}", rsPRONumberList["PRORefNo"], rsPRONumberList["ProductionOrderHeaderID"]));

            }
            rsPRONumberList.Close();

            return PRONumberList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadActivityNos(string prefix, string ProHID)
        {
            List<string> mmList = new List<string>();

            // string mmSql = "select  distinct RDA.RoutingDetailsActivityID, RD.OperationNumber+' - '+ RDA.ActivityCode AS ActivityCode  from MFG_ProductionOrderHeader MFG_PH JOIN MFG_RoutingHeader_Revision RHRv ON RHRv.RoutingHeaderRevisionID=MFG_PH.RoutingHeaderRevisionID JOIN MFG_RoutingDetails RD  ON  RD.RoutingHeaderID=RHRv.RoutingHeaderID JOIN MFG_RoutingDetailsActivity RDA ON RDA.RoutingDetailsID=RD.RoutingDetailsID JOIN MFG_RoutingDetailsActivityCapture RDAC ON RDAC.RoutingDetailsActivityID=RDA.RoutingDetailsActivityID AND RDAC.ProductionOrderHeaderID=MFG_PH.ProductionOrderHeaderID where  MFG_PH.IsActive=1 AND MFG_PH.IsDeleted=0 AND MFG_PH.ProductionOrderStatusID=2 AND MFG_PH.ProductionOrderHeaderID=" + ProHID + " AND RDA.ActivityCode  like '" + prefix + "%'";

            string mmSql = "SELECT MAX(RDAC.RoutingDetailsActivityCaptureID) RoutingDetailsActivityCaptureID,RD.OperationNumber+' - '+ RDA.ActivityCode AS ActivityCode FROM    MFG_RoutingDetailsActivityCapture RDAC  JOIN MFG_RoutingDetailsActivity RDA ON RDA.RoutingDetailsActivityID=RDAC.RoutingDetailsActivityID JOIN MFG_RoutingDetails RD ON RD.RoutingDetailsID=RDA.RoutingDetailsID JOIN MFG_RoutingHeader RH ON RH.RoutingHeaderID=RD.RoutingHeaderID JOIN MFG_RoutingHeader_Revision ROHR ON ROHR.RoutingHeaderID=RH.RoutingHeaderID JOIN MFG_ProductionOrderHeader POH ON POH.RoutingHeaderRevisionID=ROHR.RoutingHeaderRevisionID WHERE POH.ProductionOrderHeaderID=" + ProHID + " AND (RDAC.UserRoleID=-2 OR RDAC.UserRoleID=-3 OR UserRoleID=-4) and rdac.IsActive=1 and rdac.IsDeleted=0 AND RD.OperationNumber+' - '+ RDA.ActivityCode like   '" + prefix + "%'  GROUP BY RDA.RoutingDetailsActivityID,RD.OperationNumber,RDA.ActivityCode   ";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["ActivityCode"], rsMCodeList["RoutingDetailsActivityCaptureID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetActiveStockMCode(string prefix, string TenantID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select Top 20 MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode , MaterialMasterID from MMT_MaterialMaster where IsDeleted=0 and  IsActive=1   AND ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetTenantActiveStockMCode(string prefix, string TenantID)
        {
            List<string> mmList = new List<string>();

            //string mmSql = " select Top 20 MCode +   isnull( ' ` '+ MM.OEMPartNo,'')  AS MCode , MM.MaterialMasterID from MMT_MaterialMaster MM "
            //                 +" left join TPL_Tenant_MaterialMaster TTM on TTM.MaterialMasterID=MM.MaterialMasterID and TTM.IsActive=1 and TTM.IsDeleted=0 "
            //                    + " LEFT OUTER JOIN TPL_Tenant TNT ON TNT.TenantID = TMM.TenantID AND TNT.IsActive = 1 AND TNT.IsDeleted = 0 where TNT.TenantID =" + cp.TenantID + " and MM.IsDeleted = 0 and MM.IsActive = 1 AND TNT.AccountID =" + cp.AccountID + " AND(MCode like '%' OR  OEMPartNo like '%') order by MCode";

            //          + "  AND ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";



            string mmSql = "select Top 20 MCode +   isnull( ' ` '+ MM.OEMPartNo,'')  AS MCode , MM.MaterialMasterID from MMT_MaterialMaster MM  left join TPL_Tenant_MaterialMaster TTM on TTM.MaterialMasterID=MM.MaterialMasterID and TTM.IsActive=1 and TTM.IsDeleted=0  LEFT OUTER JOIN TPL_Tenant TNT ON TNT.TenantID = TTM.TenantID AND TNT.IsActive = 1 AND TNT.IsDeleted = 0 where TNT.TenantID =" + TenantID + " and MM.IsDeleted = 0 and MM.IsActive = 1  AND (MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%') order by MCode";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMCodeOEMData(string prefix, string TenantID)
        {
            List<string> MMList = new List<string>();
            string MMSql = "select top 20 MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode  from MMT_MaterialMaster where IsDeleted=0 and IsActive=1 AND ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";

            IDataReader rsMM = DB.GetRS(MMSql);

            while (rsMM.Read())
            {
                MMList.Add(string.Format("{0}", rsMM["MCode"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadKitMCodeOEMData(string prefix, string TenantID)
        {
            List<string> MMList = new List<string>();
            string MMSql = "select top 20 MM.MCode +   isnull( ' ` '+ MM.OEMPartNo,'')  AS MCode from  MMT_KitPlanner Kit JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=Kit.ParentMaterialMasterID AND MM.IsActive=1 AND MM.IsDeleted=0 where Kit.TenantID=" + TenantID + "  and Kit.IsDeleted=0 and Kit.IsActive=1 AND ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";

            IDataReader rsMM = DB.GetRS(MMSql);

            while (rsMM.Read())
            {
                MMList.Add(string.Format("{0}", rsMM["MCode"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }








        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Print_Location(string location, string printerIP)
        {
            //string result;

            //if (location.EndsWith(",")) {
            //    location = location.Substring(0, location.Length - 1);
            //}

            string[] locationsplit = location.Split(',');

            //locationsplit = CommonLogic.FilterSpacesInArrElements(locationsplit);

            //DataTable dtBCode = new DataTable();
            //dtBCode.Columns.Add("EmployeeCode");


            //    DataRow dtRow = dtBCode.NewRow();

            //    dtRow["EmployeeCode"] = locationsplit[i];

            //    dtBCode.Rows.Add(dtRow);

            //}

            try
            {

                for (int i = 0; i < locationsplit.Length; i++)
                {
                    if (locationsplit[i] != "")
                    {
                        MRLWMSC21Common.PrintLocationLabel printlabel = new MRLWMSC21Common.PrintLocationLabel();
                        printlabel.PrintLable(locationsplit[i], printerIP);
                    }
                }

                //MRLWMSC21Common.CommonLogic.SendPrintJob_Location_2x1p5("", dtBCode,printerIP, out result);

                return "Successfully Printed";
            }
            catch (Exception ex)
            {

                return "Error while printing";
            }
        }



        //============================ Added By MD Prasad For Client Side Print ZPL Printing =================================//

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetClientResource(string prefix)
        {
            List<string> mmList = new List<string>();
            string mmSql = "";

            mmSql = "select DeviceIP ClientResourceID,ClientResourceName from gen_clientresource where isdeleted=0 and isactive=1 and  (ClientResourceName like '" + prefix + "%' or ClientResourceName like '%" + prefix + "' or ClientResourceName like '%" + prefix + "%') ";



            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["ClientResourceName"], rsMCodeList["ClientResourceID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Print_Location_ZPL(string location, string printerIP)
        {

            string[] locationsplit = location.Split(',');
            try
            {
                string dt = "";
                for (int i = 0; i < locationsplit.Length; i++)
                {
                    if (locationsplit[i] != "")
                    {
                        MRLWMSC21Common.PrintLocationLabel printlabel = new MRLWMSC21Common.PrintLocationLabel();
                        //dt += printlabel.PrintLable_ZPL_DisplayLocCode(locationsplit[i], printerIP);
                        dt += printlabel.PrintLable_ZPL_FromDB(locationsplit[i], printerIP);
                    }
                }

                //MRLWMSC21Common.CommonLogic.SendPrintJob_Location_2x1p5("", dtBCode,printerIP, out result);

                return dt;
            }
            catch (Exception ex)
            {

                return "";
            }
        }


        //============================ Added By MD Prasad For Client Side Print ZPL Printing =================================//


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadBOMSearchList(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select BOH.BOMHeaderID,MMT_MMM.MCode+'-'+MMT_MMMR.Revision+'/'+BOHR.Revision AS BOMREV from MFG_BOMHeader BOH JOIN MFG_BOMHeader_Revision BOHR ON BOHR.BOMHeaderID=BOH.BOMHeaderID ANd BOHR.IsActive=1 AND BOHR.IsDeleted=0 JOIN MMT_MaterialMaster_Revision MMT_MMMR ON MMT_MMMR.MaterialMasterRevisionID=BOHR.MaterialMasterRevisionID AND MMT_MMMR.IsActive=1 AND MMT_MMMR.IsDeleted=0 JOIN MMT_MaterialMaster MMT_MMM ON  MMT_MMM.MaterialMasterID=MMT_MMMR.MaterialMasterID AND MMT_MMM.IsActive=1 AND MMT_MMM.IsDeleted=0 where BOH.IsActive=1 AND BOH.IsDeleted=0 AND  MMT_MMM.MCode+'-'+MMT_MMMR.Revision+'/'+BOHR.Revision LIKE '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["BOMREV"], rsMCodeList["BOMHeaderID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadRoutingSearchList(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "SELECT RH.RoutingHeaderID, (MM.MCode + '-'+MMT_Rv.Revision + '/'+ MFG_RV.Revision )   AS MCode FROM MFG_RoutingHeader RH JOIN MFG_RoutingHeader_Revision MFG_RV ON MFG_RV.RoutingHeaderID=RH.RoutingHeaderID AND MFG_RV.IsActive=1 AND MFG_RV.IsDeleted=0 JOIN MMT_MaterialMaster_Revision MMT_Rv ON MMT_Rv.MaterialMasterRevisionID=MFG_RV.MaterialMasterRevisionID AND MMT_Rv.IsActive=1 AND MMT_Rv.IsDeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMT_Rv.MaterialMasterID AND MM.IsActive=1 AND MM.IsDeleted=0 WHERE  RH.IsActive=1 AND RH.IsDeleted=0 AND MM.MCode + '-'+MMT_Rv.Revision + '/'+ MFG_RV.Revision LIKE  '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["RoutingHeaderID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadSourceActivityList(string prefix, string RoutingHeaderID, string RoutingDetailsActivityID, string DisplayNumber, string DisplayOrder, string ActivityID)
        {
            List<string> mmList = new List<string>();

            //string mmSql = "select MFG_RD.OperationNumber + ' - ' + MFG_RDA.ActivityCode AS SourceActivity,MFG_RDA.RoutingDetailsActivityID   from MFG_RoutingDetails MFG_RD JOIN MFG_RoutingDetailsActivity MFG_RDA ON MFG_RDA.RoutingDetailsID=MFG_RD.RoutingDetailsID AND MFG_RDA.IsActive=1 AND MFG_RDA.IsDeleted=0 where MFG_RD.IsActive=1 AND MFG_RD.IsDeleted=0 AND MFG_RD.RoutingHeaderID=" + RoutingHeaderID + " AND   ( 0=" + RoutingDetailsActivityID + " or MFG_RDA.RoutingDetailsActivityID<" + RoutingDetailsActivityID + " )  AND   DisplayNumber<= "+DisplayNumber+" AND  MFG_RDA.ActivityCode LIKE  '" + prefix + "%'   order by MFG_RDA.DisplayOrder";
            //string mmSql = "select MFG_RD.OperationNumber + ' - ' + MFG_RDA.ActivityCode AS SourceActivity,MFG_RDA.RoutingDetailsActivityID   from MFG_RoutingDetails MFG_RD JOIN MFG_RoutingDetailsActivity MFG_RDA ON MFG_RDA.RoutingDetailsID=MFG_RD.RoutingDetailsID AND MFG_RDA.IsActive=1 AND MFG_RDA.IsDeleted=0 where MFG_RD.IsActive=1 AND MFG_RD.IsDeleted=0 AND MFG_RD.RoutingHeaderID=" + RoutingHeaderID + " AND   DisplayNumber<=" + DisplayNumber + " AND MFG_RDA.RoutingDetailsActivityID<>" + RoutingDetailsActivityID +  "  AND  MFG_RDA.ActivityCode LIKE  '" + prefix + "%'   order by MFG_RDA.DisplayOrder";

            string mmSql = " EXEC [dbo].[sp_MFG_GetSourceActivityList]  @RoutingHeaderID =" + RoutingHeaderID + ",@RoutingDetailsID =" + RoutingDetailsActivityID + ",@DisplayNumber=" + DisplayNumber + ",@DisplayOrder=" + DisplayOrder + " ,@Prefix='" + prefix + "',@ActivityID=" + ActivityID;

            //" AND DisplayOrder<=" + DisplayOrder +


            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["SourceActivity"], rsMCodeList["RoutingDetailsActivityID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadQCCheckListNames(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select CheckListName,QCCheckListConfigurationID from MFG_QCCheckListConfiguration where IsActive=1 AND IsDeleted=0 AND CheckListName like  '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["CheckListName"], rsMCodeList["QCCheckListConfigurationID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();




        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadTERCaptureType(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select  TERCaptureType,TERCaptureTypeID from MFG_TERCaptureType where IsActive=1 AND IsDeleted=0 AND TERCaptureType like  '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["TERCaptureType"], rsMCodeList["TERCaptureTypeID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();




        }






        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadStoreRefNumbers(string prefix, string TenantId, string WarehouseId)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select inb.StoreRefNo,inb.InboundID from INB_Inbound inb join TPL_Tenant tnt on tnt.TenantID = inb.TenantID join GEN_Account acc on acc.AccountID = tnt.AccountID " +
                           " join INB_RefWarehouse_Details RefWH ON RefWH.InboundID = inb.InboundID where inb.IsActive = 1 AND inb.IsDeleted = 0 and RefWH.IsActive = 1 AND RefWH.IsDeleted = 0 and" +
                           " (acc.AccountID =" + cp.AccountID + " OR " + cp.AccountID + "=0) AND inb.TenantID=" + TenantId + "  AND (RefWH.WarehouseID=" + WarehouseId + " OR " + WarehouseId + "=0) AND StoreRefNo  like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["StoreRefNo"], rsMCodeList["InboundID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadStoreRefNos(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select StoreRefNo,InboundID from INB_Inbound where ISACTIVE = 1 AND ISDELETED = 0 and StoreRefNo like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["StoreRefNo"], rsMCodeList["InboundID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] CustomerBasedonAccount(string prefix, int AccountID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select CustomerName,CustomerID from GEN_Customer CUS join TPL_Tenant tnt on tnt.TenantID = CUS.TenantID join GEN_Account acc on acc.AccountID = tnt.AccountID where CUS.IsActive = 1 AND CUS.IsDeleted = 0 and acc.AccountID =" + AccountID + "AND CustomerName  like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["CustomerName"], rsMCodeList["CustomerID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] OBDNumberBasedonAccount(string prefix, int AccountID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select OBDNumber,OutboundID  from OBD_Outbound OBD join TPL_Tenant tnt on tnt.TenantID = OBD.TenantID join GEN_Account acc on acc.AccountID = tnt.AccountID where OBD.IsActive = 1 AND OBD.IsDeleted = 0 and acc.AccountID =" + AccountID + "AND OBDNumber  like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["OBDNumber"], rsMCodeList["OutboundID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMCodeByMType(string prefix, string TenantID, string MTypeID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select Top 20 MCode, MaterialMasterID from MMT_MaterialMaster where MTypeID=" + MTypeID + " AND IsDeleted=0 and  IsActive=1  AND  MCode like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMCodeByMTypeWithOEM(string prefix, string TenantID, string MTypeID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select Top 20 MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode, MaterialMasterID from MMT_MaterialMaster where MTypeID=" + MTypeID + " AND IsDeleted=0 and  IsActive=1  AND  ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }






        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadRoutingDetailsActivityCodes(string prefix, string RoutingDetailsID)
        {
            List<string> activityList = new List<string>();

            string activitySql = "select ActivityCode,RoutingDetailsActivityID from MFG_RoutingDetailsActivity where ActivityTypeID=1 AND IsActive=1 AND IsDeleted=0 AND RoutingDetailsID=" + RoutingDetailsID + " AND  ActivityCode like '" + prefix + "%'";

            IDataReader rsactivityCodeList = DB.GetRS(activitySql);

            while (rsactivityCodeList.Read())
            {
                activityList.Add(string.Format("{0},{1}", rsactivityCodeList["ActivityCode"], rsactivityCodeList["RoutingDetailsActivityID"]));
            }

            rsactivityCodeList.Close();
            return activityList.ToArray();

        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadBoMMCodes(string prefix, string BoMHeaderID, string MaterialMasterRevisionID, string RoutingDocTypeID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "";

            if (RoutingDocTypeID != "2")
            {

                mmSql = "select MM.MCode,MM.OEMPartNo,BD.BOMMaterialMasterID from MFG_BOMDetails BD JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=BD.BOMMaterialMasterID where BD.BOMHeaderID=" + BoMHeaderID + " AND BD.ParentMaterialMasterID  IN (select BOMMaterialMasterID from MFG_BOMDetails where BOMHeaderID=" + BoMHeaderID + " AND MaterialMasterRevisionID=" + MaterialMasterRevisionID + ")  AND MM.MCode like '" + prefix + "%'";

            }
            else
            {

                mmSql = "select MM.MCode,BD.BOMMaterialMasterID,MM.OEMPartNo from MFG_BOMDetails BD JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=BD.BOMMaterialMasterID  AND  MM.IsActive=1 AND MM.IsDeleted=0 where BD.BOMHeaderID=" + BoMHeaderID + " AND BD.IsActive=1 AND BD.IsDeleted=0 AND MaterialMasterRevisionID is null  AND MM.MCode like '" + prefix + "%'";

            }

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"] + " [ " + rsMCodeList["OEMPartNo"] + " ] ", rsMCodeList["BOMMaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadBoMMCodesWithOEM(string prefix, string BoMHeaderID, string MaterialMasterRevisionID, string RoutingDocTypeID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "";

            if (RoutingDocTypeID != "2")
            {

                mmSql = "select MM.MCode  +   isnull( ' ` '+ MM.OEMPartNo,'')  AS MCode, BD.BOMMaterialMasterID from MFG_BOMDetails BD JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=BD.BOMMaterialMasterID where   MM.MTypeID=7 AND BD.BOMHeaderID=" + BoMHeaderID + " AND BD.ParentMaterialMasterID  IN (select BOMMaterialMasterID from MFG_BOMDetails where BOMHeaderID=" + BoMHeaderID + " AND MaterialMasterRevisionID=" + MaterialMasterRevisionID + ")  AND ( MM.MCode like '" + prefix + "%' OR  MM.OEMPartNo like '" + prefix + "%' ) order by MCode";

            }
            else
            {

                mmSql = "select MM.MCode  +   isnull( ' ` '+ MM.OEMPartNo,'')  AS MCode ,BD.BOMMaterialMasterID  from MFG_BOMDetails BD JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=BD.BOMMaterialMasterID  AND  MM.IsActive=1 AND MM.IsDeleted=0 where BD.BOMHeaderID=" + BoMHeaderID + " AND BD.IsActive=1 AND BD.IsDeleted=0 AND MaterialMasterRevisionID is null  AND MM.MTypeID=7  AND ( MM.MCode like '" + prefix + "%' OR  MM.OEMPartNo like '" + prefix + "%' ) order by MCode";

            }

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["MCode"], rsMCodeList["BOMMaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadParameterDataTypes(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select ParameterDataType,ParameterDataTypeID from GEN_ParameterDataType where IsActive=1 AND IsDeleted=0 AND ParameterDataTypeID IN(3,2) AND ParameterDataType like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["ParameterDataType"], rsMCodeList["ParameterDataTypeID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetParameterType(string QCParameterID)
        {
            int status = 0;

            string mmSql = "select * from QCC_QualityParameters where ( ParameterDataTypeID=2 OR ParameterDataTypeID=1 ) AND  QualityParameterID=" + QCParameterID;

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                status = 1;
            }

            rsMCodeList.Close();

            return status.ToString();


        }








        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMMRUoMWithQty(String MMRvID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select G_UM.UoM,MM_UM.UoMQty,MM_UM.MaterialMaster_UoMID from  MMT_MaterialMaster_Revision MMT_MMR JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMT_MMR.MaterialMasterID AND MM.isactive=1 AND MM.IsDeleted=0 JOIN MMT_MaterialMaster_GEN_UoM MM_UM On MM_UM.MaterialMasterID=MM.MaterialMasterID AND MM_UM.IsActive=1 AND MM_UM.IsDeleted=0 JOIN GEN_UoM G_UM ON G_UM.UoMID=MM_UM.UoMID AND G_UM.IsActive=1 AND G_UM.IsDeleted=0 where MMT_MMR.IsActive=1 AND MMT_MMR.IsDeleted=0 AND MM_UM.UoMTypeID<>1 AND MMT_MMR.MaterialMasterRevisionID=" + MMRvID;

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["UoM"] + "/" + rsMCodeList["UoMQty"], rsMCodeList["MaterialMaster_UoMID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadRotingRefWithRevsions(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select  MM.MCode + '-' + MMT_Rv.Revision  AS MCode ,MFG_Rv.Revision,MFG_Rv.RoutingHeaderRevisionID  from MFG_RoutingHeader_Revision MFG_Rv JOIN MMT_MaterialMaster_Revision MMT_Rv ON MMT_Rv.MaterialMasterRevisionID=MFG_Rv.MaterialMasterRevisionID AND MMT_Rv.IsActive=1 AND MMT_Rv.IsDeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMT_Rv.MaterialMasterID AND MM.IsActive=1 AND MM.IsDeleted=0 WHERE MFG_Rv.IsActive=1 AND MFG_Rv.IsDeleted=0 AND MM.MTypeID IN (8,9) AND  MM.MCode + '-' + MMT_Rv.Revision like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"] + "/" + rsMCodeList["Revision"], rsMCodeList["RoutingHeaderRevisionID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadBoMRefWithRevsions(String prefix, String MMRevisionID)
        {
            List<String> BoMVersionRefNoList = new List<String>();
            String cMdBoMVersionRefNo = "select MM.MCode + '-' + MMT_MMR.Revision + '/' + MFG_BHR.Revision AS BOMRefNo ,MFG_BHR.BOMHeaderRevisionID from MFG_BOMHeader_Revision MFG_BHR JOIN MMT_MaterialMaster_Revision  MMT_MMR ON MMT_MMR.MaterialMasterRevisionID=MFG_BHR.MaterialMasterRevisionID JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMT_MMR.MaterialMasterID where MFG_BHR.IsActive=1 AND MFG_BHR.IsDeleted=0 AND   MFG_BHR.MaterialMasterRevisionID=" + MMRevisionID + " AND MM.MTypeID IN (8,9) AND  MM.MCode + '-' + MMT_MMR.Revision + '/' + MFG_BHR.Revision like  '" + prefix + "%'";
            IDataReader rsBoMVersionRefNoList = DB.GetRS(cMdBoMVersionRefNo);
            while (rsBoMVersionRefNoList.Read())
            {
                BoMVersionRefNoList.Add(String.Format("{0},{1}", rsBoMVersionRefNoList["BOMRefNo"], rsBoMVersionRefNoList["BOMHeaderRevisionID"]));

            }
            rsBoMVersionRefNoList.Close();

            return BoMVersionRefNoList.ToArray();
        }





        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadRevMcodes(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select MM_R.MaterialMasterRevisionID , MM.MCode ,MM_R.Revision from MMT_MaterialMaster MM JOIN MMT_MaterialMaster_Revision MM_R ON MM_R.MaterialMasterID=MM.MaterialMasterID AND MM_R.IsActive=1 AND MM_R.IsDeleted=0 where MM.IsActive=1 AND MM.isdeleted=0   AND MM.MTypeID IN (8,9) AND   MM.MCode + '-' + MM_R.Revision like '" + prefix + "%' order by MM.MCode";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"] + "-" + rsMCodeList["Revision"], rsMCodeList["MaterialMasterRevisionID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadRoutingRevMcodes(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select MM_R.MaterialMasterRevisionID , MM.MCode ,MM_R.Revision ,MT.MType from MMT_MaterialMaster MM JOIN MMT_MaterialMaster_Revision MM_R ON MM_R.MaterialMasterID=MM.MaterialMasterID AND MM_R.IsActive=1 AND MM_R.IsDeleted=0 JOIN MMT_MType MT ON MT.MTypeID=MM.MTypeID where MM.IsActive=1 AND MM.isdeleted=0   AND MM.MTypeID IN (9,8) AND   MM.MCode +' - '+ MM_R.Revision   like '" + prefix + "%' order by MM.MCode";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"] + " - " + rsMCodeList["Revision"], rsMCodeList["MaterialMasterRevisionID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }






        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadParentRevMcodes(string prefix, string BOMHeaderID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select MM.MCode + ' - ' + MMRev.Revision AS MCode , MMRev.MaterialMasterRevisionID from MFG_BOMDetails BD JOIN MMT_MaterialMaster_Revision MMRev ON MMRev.MaterialMasterRevisionID=BD.MaterialMasterRevisionID JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMRev.MaterialMasterID where BD.IsActive=1 AND BD.IsDeleted=0 AND BD.BOMHeaderID=" + BOMHeaderID + "   AND MM.MTypeID IN (8,9) AND   MM.MCode + '-' + MMRev.Revision like '" + prefix + "%' order by MM.MCode";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterRevisionID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }





        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadRevMcodesByMType(string prefix, string MTypeID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select MM_R.MaterialMasterRevisionID , MM.MCode ,MM_R.Revision from MMT_MaterialMaster MM JOIN MMT_MaterialMaster_Revision MM_R ON MM_R.MaterialMasterID=MM.MaterialMasterID AND MM_R.IsActive=1 AND MM_R.IsDeleted=0 where MM.IsActive=1 AND MM.isdeleted=0 AND  MM.MTypeID=" + MTypeID + "  AND  MM.MCode + '-' + MM_R.Revision like '" + prefix + "%' order by MM.MCode";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"] + " - " + rsMCodeList["Revision"], rsMCodeList["MaterialMasterRevisionID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadRevMcodesByMTypeWithOEM(string prefix, string MTypeID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select top 20  case when MM.MTypeID IN (8,9) then MM_R.MaterialMasterRevisionID else MM.MaterialMasterID end AS MaterialMasterRevisionID , MM.MCode+ isnull(' - '+MM_R.Revision,'') + case when MTypeID=7 then isnull('`'+MM.OEMPartNo,'') else '' end AS MCode from MMT_MaterialMaster MM LEFT JOIN MMT_MaterialMaster_Revision MM_R ON MM_R.MaterialMasterID=MM.MaterialMasterID AND MM_R.IsActive=1 AND MM_R.IsDeleted=0 where MM.IsActive=1 AND MM.isdeleted=0 AND  MM.MTypeID=" + MTypeID + " AND (  MM.MCode + case when MM.MTypeID IN (8,9) then  '-' + MM_R.Revision else '' end like '" + prefix + "%'  OR  case when MTypeID=7 then OEMPartNo else '' end like '" + prefix + "%') order by MM.MCode";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterRevisionID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadUoMWithQty1(String MaterialID, String MTypeID)
        {
            List<String> list = new List<string>();
            string sql = "[dbo].[USP_LoadUoMWithQty] @MtypeID =" + MTypeID + ",@MaterialID INT=" + MaterialID + "";
            //String sql = "declare @mtypeid int=" + MTypeID + "; SELECT MM_IUoM.MaterialMaster_UoMID,UOM,MM_IUoM.UoMQty FROM MMT_MaterialMaster_GEN_UoM MM_IUoM JOIN GEN_UoM UOM ON UOM.UoMID=MM_IUoM.UoMID JOIN MMT_MaterialMaster_GEN_UoM MM_MUoM on MM_IUoM.MaterialMasterID=MM_MUoM.MaterialMasterID and MM_MUoM.IsDeleted=0 and MM_MUoM.UoMTypeID=1 LEFT JOIN MMT_MaterialMaster_Revision MMRv ON MMRv.MaterialMasterID=MM_IUoM.MaterialMasterID WHERE MM_IUoM.IsDeleted=0 AND (MM_IUoM.UoMTypeID!=2 OR MM_IUoM.UoMID!=MM_MUoM.UoMID OR MM_IUoM.UoMQty!=MM_MUoM.UoMQty) AND  ( (@mtypeid=7 AND MM_IUoM.MaterialMasterID=" + MaterialID + ") OR (@mtypeid IN (8,9) AND MMRv.MaterialMasterRevisionID=" + MaterialID + "))";

            //mmgu.UoMTypeID!=1 AND -- This is removed as per prasad requirement 17.03.2015(UoM)
            IDataReader SuomReader = DB.GetRS(sql);
            while (SuomReader.Read())
            {
                list.Add(String.Format("{0}/{1},{2}", SuomReader["UoM"], SuomReader["UoMQty"], SuomReader["MaterialMaster_UoMID"]));
            }

            SuomReader.Close();
            return list.ToArray();

        }










        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadFinishedMcodes(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select MM_R.MaterialMasterRevisionID , MM.MCode ,MM_R.Revision from MMT_MaterialMaster MM JOIN MMT_MaterialMaster_Revision MM_R ON MM_R.MaterialMasterID=MM.MaterialMasterID AND MM_R.IsActive=1 AND MM_R.IsDeleted=0 where MM.IsActive=1 AND MM.isdeleted=0 AND MM.MTypeID IN (8,9)  AND  MM.MCode + '-' + MM_R.Revision like '" + prefix + "%' order by MM.MCode";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"] + "-" + rsMCodeList["Revision"], rsMCodeList["MaterialMasterRevisionID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadBOMFinishedMcodes(string prefix, String BOMHeaderRevID, string RoutingDocTypeID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "";
            if (RoutingDocTypeID == "2")
            {

                mmSql = "select MM.MCode+'-'+MMRev.Revision AS MCode , BD.MaterialMasterRevisionID from MFG_BOMDetails BD JOIN MMT_MaterialMaster_Revision MMRev ON MMRev.MaterialMasterRevisionID=BD.MaterialMasterRevisionID JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMRev.MaterialMasterID JOIN MFG_BOMHeader_Revision BHRev ON BHRev.BOMHeaderID= BD.BOMHeaderID where BD.IsActive=1 AND BD.IsDeleted=0 AND BHRev.BOMHeaderRevisionID=" + BOMHeaderRevID + "  AND MM.MTypeID IN (9) AND   MM.MCode + '-' + MMRev.Revision like '" + prefix + "%' order by MM.MCode";
            }
            else
            {
                mmSql = "select MM.MCode+'-'+MMRev.Revision AS MCode , BD.MaterialMasterRevisionID from MFG_BOMDetails BD JOIN MMT_MaterialMaster_Revision MMRev ON MMRev.MaterialMasterRevisionID=BD.MaterialMasterRevisionID JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMRev.MaterialMasterID JOIN MFG_BOMHeader_Revision BHRev ON BHRev.BOMHeaderID= BD.BOMHeaderID where BD.IsActive=1 AND BD.IsDeleted=0 AND BHRev.BOMHeaderRevisionID=" + BOMHeaderRevID + "  AND MM.MTypeID IN (8) AND   MM.MCode + '-' + MMRev.Revision like '" + prefix + "%' order by MM.MCode";
            }

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterRevisionID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadActivityMcodes(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select TOP 20 MaterialMasterID,MCode from MMT_MaterialMaster WHERE MCode LIKE '%" + prefix + "%' and mtypeid=7 ";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadCustomerPONumbers(string prefix, string SOHeaderID, string InvoiceNo)
        {
            List<string> mmList = new List<string>();

            SOHeaderID = (SOHeaderID == "" ? "0" : SOHeaderID);

            string mmSql = "EXEC [dbo].[sp_OBD_GetDataForCustomerPo] @SOHeaderID=" + SOHeaderID + ",@Type=1,@InvoiceNo=" + DB.SQuote(InvoiceNo) + ",@prefix='%" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["CustPONumber"], rsMCodeList["CustomerPOID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadActivityTypes(string prefix)
        {
            List<string> ListActivityTypes = new List<string>();

            string ActivityTypesSql = "select ActivityType,ActivityTypeID from MFG_ActivityType where IsActive=1 AND IsDeleted=0 AND ActivityType Like '" + prefix + "%'";

            IDataReader rsActivityTypes = DB.GetRS(ActivityTypesSql);

            while (rsActivityTypes.Read())
            {
                ListActivityTypes.Add(string.Format("{0},{1}", rsActivityTypes["ActivityType"], rsActivityTypes["ActivityTypeID"]));
            }

            rsActivityTypes.Close();


            return ListActivityTypes.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadOEMPartNo(string prefix)
        {
            List<string> ListActivityTypes = new List<string>();

            string ActivityTypesSql = "select OEMPartNo ,MaterialMasterID from MMT_MaterialMaster where IsActive=1 AND IsDeleted=0 AND IsAproved=1 AND OEMPartNo LIKE '" + prefix + "%'";

            IDataReader rsActivityTypes = DB.GetRS(ActivityTypesSql);

            while (rsActivityTypes.Read())
            {
                ListActivityTypes.Add(string.Format("{0},{1}", rsActivityTypes["OEMPartNo"], rsActivityTypes["MaterialMasterID"]));
            }

            rsActivityTypes.Close();


            return ListActivityTypes.ToArray();

        }





        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWorkCenterGroups(string prefix)
        {
            List<string> ListActivityTypes = new List<string>();

            string ActivityTypesSql = "select WorkCenterGroupID,WorkCenterGroup from MFG_WorkCenterGroup where IsActive=1 AND IsDeleted=0 AND WorkCenterGroup LIKE '" + prefix + "%'";

            IDataReader rsActivityTypes = DB.GetRS(ActivityTypesSql);

            while (rsActivityTypes.Read())
            {
                ListActivityTypes.Add(string.Format("{0},{1}", rsActivityTypes["WorkCenterGroup"], rsActivityTypes["WorkCenterGroupID"]));
            }

            rsActivityTypes.Close();


            return ListActivityTypes.ToArray();

        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadRoutingDocumentType(string prefix)
        {
            List<string> ListActivityTypes = new List<string>();

            string ActivityTypesSql = "select RoutingDocumentTypeID ,RoutingDocumentType from MFG_RoutingDocumentType where IsActive=1 AND IsDeleted=0 AND RoutingDocumentType  LIKE '" + prefix + "%'";

            IDataReader rsActivityTypes = DB.GetRS(ActivityTypesSql);

            while (rsActivityTypes.Read())
            {
                ListActivityTypes.Add(string.Format("{0},{1}", rsActivityTypes["RoutingDocumentType"], rsActivityTypes["RoutingDocumentTypeID"]));
            }

            rsActivityTypes.Close();


            return ListActivityTypes.ToArray();

        }






        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] MaterialUoMList(string prefix, string MMID)
        {
            List<string> UoMList = new List<string>();


            if (DB.GetSqlN("select Count( MMT_U.MaterialMaster_UoMID )  AS N  from MMT_MaterialMaster MMT left join MMT_MaterialMaster_GEN_UoM MMT_U on MMT_U.MaterialMasterID=MMT.MaterialMasterID and MMT_U.IsDeleted=0 left join GEN_UoM GEN_U on GEN_U.UoMID=MMT_U.UoMID and MMT_U.IsDeleted=0   where MMT_U.IsActive=1 and MMT_U.IsDeleted=0 and   MMT.MaterialMasterID=" + MMID) >= 2)
            {


                string UoMSql = "select distinct MMT_U.MaterialMaster_UoMID,MMT_U.UoMID,GEN_U.UoM,MMT_U.UoMQty from MMT_MaterialMaster MMT left join MMT_MaterialMaster_GEN_UoM MMT_U on MMT_U.MaterialMasterID=MMT.MaterialMasterID and MMT_U.IsDeleted=0 left join GEN_UoM GEN_U on GEN_U.UoMID=MMT_U.UoMID and MMT_U.IsDeleted=0   where MMT_U.IsActive=1 and MMT_U.IsDeleted=0 and MMT_U.UoMTypeID!=2 and  MMT.MaterialMasterID=" + MMID;


                IDataReader rsUoM = DB.GetRS(UoMSql);

                while (rsUoM.Read())
                {
                    UoMList.Add(string.Format("{0},{1}", rsUoM["UoM"] + "/" + rsUoM["UoMQty"], rsUoM["MaterialMaster_UoMID"]));
                }

                rsUoM.Close();
            }
            return UoMList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] MaterialBasedUoMList(string prefix, string MMID)
        {
            List<string> UoMList = new List<string>();


            if (DB.GetSqlN("select Count( MMT_U.MaterialMaster_UoMID )  AS N  from MMT_MaterialMaster MMT left join MMT_MaterialMaster_GEN_UoM MMT_U on MMT_U.MaterialMasterID=MMT.MaterialMasterID and MMT_U.IsDeleted=0 left join GEN_UoM GEN_U on GEN_U.UoMID=MMT_U.UoMID and MMT_U.IsDeleted=0   where MMT_U.IsActive=1 and MMT_U.IsDeleted=0 and   MMT.MaterialMasterID=" + MMID) >= 2)
            {


                string UoMSql = "select MMT_U.MaterialMaster_UoMID,MMT_U.UoMID,GEN_U.UoM,MMT_U.UoMQty from MMT_MaterialMaster MMT left join MMT_MaterialMaster_GEN_UoM MMT_U on MMT_U.MaterialMasterID=MMT.MaterialMasterID and MMT_U.IsDeleted=0 left join GEN_UoM GEN_U on GEN_U.UoMID=MMT_U.UoMID and MMT_U.IsDeleted=0   where MMT_U.IsActive=1 and MMT_U.IsDeleted=0 and   MMT.MaterialMasterID=" + MMID;


                IDataReader rsUoM = DB.GetRS(UoMSql);

                while (rsUoM.Read())
                {
                    UoMList.Add(string.Format("{0},{1}", rsUoM["UoM"] + "/" + rsUoM["UoMQty"], rsUoM["UoMID"]));
                }

                rsUoM.Close();
            }
            return UoMList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadUsersData(string prefix, string TenantID)
        {

            List<string> userList = new List<string>();
            // <!------------Procedure Convertion------>
            //  string userSql = "select UserID,FirstName from GEN_User where TenantID=" + TenantID + " and IsActive=1  AND AccountID = case when 0 =" + cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end AND  FirstName like '" + prefix + "%'";

            string userSql = "Exec [dbo].[USP_LoadUserDropDown] @Prefix='" + prefix + "',@TenantID=" + TenantID + ",@LogAccountID=" + cp.AccountID.ToString() + "";
            IDataReader rsUser = DB.GetRS(userSql);

            while (rsUser.Read())
            {
                userList.Add(string.Format("{0},{1}", rsUser["FirstName"], rsUser["UserID"]));
            }
            rsUser.Close();
            return userList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadUsersData1(string prefix, string TenantID, string WarehouseID)
        {
            int flag = 1;
            List<string> userList = new List<string>();
            // <!------------Procedure Convertion------>
            //  string userSql = "select UserID,FirstName from GEN_User where TenantID=" + TenantID + " and IsActive=1  AND AccountID = case when 0 =" + cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end AND  FirstName like '" + prefix + "%'";

            string userSql = "Exec [dbo].[USP_LoadUserDropDown1] @Prefix='" + prefix + "',@TenantID=" + TenantID + ",@WarehouseID=" + WarehouseID + ",@flag=" + flag + ",@LogAccountID=" + cp.AccountID.ToString() + "";
            IDataReader rsUser = DB.GetRS(userSql);

            while (rsUser.Read())
            {
                userList.Add(string.Format("{0},{1}", rsUser["FirstName"], rsUser["UserID"]));
            }
            rsUser.Close();
            return userList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadUsersData2(string prefix, string TenantID, string WarehouseID)
        {
            int flag = 0;
            List<string> userList = new List<string>();
            // <!------------Procedure Convertion------>
            //  string userSql = "select UserID,FirstName from GEN_User where TenantID=" + TenantID + " and IsActive=1  AND AccountID = case when 0 =" + cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end AND  FirstName like '" + prefix + "%'";

            string userSql = "Exec [dbo].[USP_LoadUserDropDown1] @Prefix='" + prefix + "',@TenantID=" + TenantID + ",@WarehouseID=" + WarehouseID + ",@flag=" + flag + ",@LogAccountID=" + cp.AccountID.ToString() + "";
            IDataReader rsUser = DB.GetRS(userSql);

            while (rsUser.Read())
            {
                userList.Add(string.Format("{0},{1}", rsUser["FirstName"], rsUser["UserID"]));
            }
            rsUser.Close();
            return userList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadPlantData(string prefix, string TenantID)
        {

            List<string> MMPlant = new List<string>();

            string plantSql = "select Top 30 Plant, MPlantID from MMT_MPlant where IsActive=1  AND  Plant like '" + prefix + "%'";


            IDataReader rsPlant = DB.GetRS(plantSql);

            while (rsPlant.Read())
            {
                MMPlant.Add(string.Format("{0},{1}", rsPlant["Plant"], rsPlant["MPlantID"]));
            }

            rsPlant.Close();

            return MMPlant.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadSupplierData(string prefix, string TenantID)
        {

            List<string> supplierList = new List<string>();
            //string supplierSql = "select distinct  Top 10 SupplierName+isnull('-'+SupplierCode,'') as SupplierName,SupplierID from MMT_Supplier  where  ishidden=0 and SupplierID>0 AND IsActive=1 AND IsDeleted=0  AND  SupplierName+isnull('-'+SupplierCode,'') like '" + prefix + "%'  order by SupplierName";
            string supplierSql = "select distinct  Top 10 SupplierName+isnull('-'+SupplierCode,'') as SupplierName,SupplierID from MMT_Supplier MMS JOIN TPL_Tenant TNT ON TNT.TenantID = MMS.TenantID where MMS.ishidden = 0 and (" + TenantID + "=0 or TNT.TenantID=" + TenantID + ") and SupplierID> 0 AND MMS.IsActive = 1 AND MMS.IsDeleted = 0 AND SupplierName+isnull('-' + SupplierCode, '') like '%" + prefix + "%' AND TNT.AccountID = case when 0 = " + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end order by SupplierName";

            IDataReader rsSupplier = DB.GetRS(supplierSql);

            while (rsSupplier.Read())
            {
                supplierList.Add(string.Format("{0},{1}", rsSupplier["SupplierName"], rsSupplier["SupplierID"]));
            }
            rsSupplier.Close();
            return supplierList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadSupplierDataBasedTenant(string prefix, string TenantID)
        {

            List<string> supplierList = new List<string>();
            //string supplierSql = "select distinct  Top 10 SupplierName+isnull('-'+SupplierCode,'') as SupplierName,SupplierID from MMT_Supplier  where  ishidden=0 and SupplierID>0 AND IsActive=1 AND IsDeleted=0  AND  SupplierName+isnull('-'+SupplierCode,'') like '" + prefix + "%'  order by SupplierName";
            string supplierSql = "select distinct  Top 10 SupplierName ,SupplierID from MMT_Supplier MMS JOIN TPL_Tenant TNT ON TNT.TenantID = MMS.TenantID where MMS.ishidden = 0 and SupplierID> 0 AND MMS.IsActive = 1 AND MMS.IsDeleted = 0 AND SupplierName+isnull('-' + SupplierCode, '') like '%" + prefix + "%' AND TNT.TenantID =" + TenantID + " order by SupplierName";

            IDataReader rsSupplier = DB.GetRS(supplierSql);

            while (rsSupplier.Read())
            {
                supplierList.Add(string.Format("{0},{1}", rsSupplier["SupplierName"], rsSupplier["SupplierID"]));
            }
            rsSupplier.Close();
            return supplierList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadServiceSupplierData(string prefix, string TenantID)
        {

            List<string> supplierList = new List<string>();
            string supplierSql = "select distinct  Top 30 SupplierName,SupplierCode,SupplierID from MMT_Supplier  where  ishidden=0 and SupplierID>0  AND IsActive=1 AND IsDeleted=0  AND  SupplierTypeID=2 AND SupplierName like '" + prefix + "%'  order by SupplierName";


            IDataReader rsSupplier = DB.GetRS(supplierSql);

            while (rsSupplier.Read())
            {
                supplierList.Add(string.Format("{0},{1}", rsSupplier["SupplierName"], rsSupplier["SupplierID"]));
            }
            rsSupplier.Close();
            return supplierList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMMSupplierData(string prefix, string TenantID)
        {

            List<string> supplierList = new List<string>();
            string supplierSql = "select distinct  Top 30 SupplierName,SupplierCode,SupplierID from MMT_Supplier  where IsActive=1 AND IsDeleted=0  AND SupplierID>0 AND  SupplierName like '" + prefix + "%'  order by SupplierName";


            IDataReader rsSupplier = DB.GetRS(supplierSql);

            while (rsSupplier.Read())
            {
                supplierList.Add(string.Format("{0},{1}", rsSupplier["SupplierName"], rsSupplier["SupplierID"]));
            }
            rsSupplier.Close();
            return supplierList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadConfiguredMMSupplierData(string prefix, string MMID)
        {

            List<string> supplierList = new List<string>();
            string supplierSql = "select Sup.SupplierName,Sup.SupplierID from MMT_MaterialMaster_Supplier MMT_Sup JOIN MMT_Supplier Sup ON Sup.SupplierID=MMT_Sup.SupplierID AND Sup.isActive=1 AND Sup.isDeleted=0 where MMT_Sup.isActive=1 AND MMT_Sup.isDeleted=0 AND MMT_Sup.MaterialMasterID= " + MMID + "  AND  Sup.SupplierName like '" + prefix + "%'  order by Sup.SupplierName";


            IDataReader rsSupplier = DB.GetRS(supplierSql);

            while (rsSupplier.Read())
            {
                supplierList.Add(string.Format("{0},{1}", rsSupplier["SupplierName"], rsSupplier["SupplierID"]));
            }
            rsSupplier.Close();
            return supplierList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadSupplierDataWithoutPrefix(string prefix, string TenantID)
        {

            List<string> supplierList = new List<string>();
            string supplierSql = "select Top 30 SupplierName,SupplierID from MMT_Supplier  where IsActive=1 AND IsDeleted=0  AND SupplierID>0 AND SupplierName like '" + prefix + "%'";
            IDataReader rsSupplier = DB.GetRS(supplierSql);
            while (rsSupplier.Read())
            {
                supplierList.Add(string.Format("{0}", rsSupplier["SupplierName"]));
            }
            rsSupplier.Close();
            return supplierList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMTypeData(string prefix, string TenantID)
        {

            List<string> MTypeList = new List<string>();
            string mTypeSql = "select top 20 MTypeID,MType,Description from MMT_MType where IsActive=1 AND MTypeID IN(8,7)  AND  MType like '" + prefix + "%'";


            IDataReader rsMType = DB.GetRS(mTypeSql);

            while (rsMType.Read())
            {
                MTypeList.Add(string.Format("{0},{1}", rsMType["MType"] + "-" + rsMType["Description"], rsMType["MTypeID"]));
            }
            rsMType.Close();

            return MTypeList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMTypeDataItem(string prefix, string TenantID)
        {

            List<string> MTypeList = new List<string>();
            string mTypeSql = "Select top 10 MTypeID, MType + ' - ' + Description as MTypeDesc from MMT_MType Where IsActive =1 AND TenantID=" + TenantID + " AND  MType like '" + prefix + "%'";


            IDataReader rsMType = DB.GetRS(mTypeSql);

            while (rsMType.Read())
            {
                MTypeList.Add(string.Format("{0},{1}", rsMType["MTypeDesc"], rsMType["MTypeID"]));
            }
            rsMType.Close();

            return MTypeList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadStorageConditionData(string prefix, string TenantID)
        {

            List<string> storageCondList = new List<string>();

            string storageCondSql = "select StorageCondition,StorageConditionID from MMT_StorageCondition  where IsActive=1  AND  StorageCondition  like '" + prefix + "%' order by StorageCondition";

            IDataReader rsstorageCond = DB.GetRS(storageCondSql);

            while (rsstorageCond.Read())
            {
                storageCondList.Add(string.Format("{0},{1}", rsstorageCond["StorageCondition"], rsstorageCond["StorageConditionID"]));
            }
            rsstorageCond.Close();

            return storageCondList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWarehouseData(string prefix, string TenantID)
        {

            List<string> WHList = new List<string>();

            //string WHSql = "select top 20 WHCode,Location,WarehouseID from GEN_Warehouse  where IsActive=1  AND  WHCode  like '" + prefix + "%' order by WHCode";

            //<!---Procedure Converting--->
            // string WHSql = "select top 20 WHCode,Location,WarehouseID from GEN_Warehouse  where IsActive=1  AND  WHCode  like '" + prefix + "%'";

            //Account and Tenant filteration

            //  WHSql = WHSql + " AND AccountID = "+ cp.AccountID.ToString() +"  order by WHCode";

            string WHSql = "Exec [dbo].[USP_MST_DropWH] @AccountID=" + cp.AccountID.ToString() + ",@TenantID=" + TenantID + ",@Prefix='" + prefix + "'";

            IDataReader rsWH = DB.GetRS(WHSql);

            while (rsWH.Read())
            {
                WHList.Add(string.Format("{0},{1}", rsWH["WHCode"] + "-" + rsWH["Location"], rsWH["WarehouseID"]));
            }
            rsWH.Close();

            return WHList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWarehouseData1(string prefix, string TenantID)
        {

            List<string> WHList = new List<string>();

            //string WHSql = "select top 20 WHCode,Location,WarehouseID from GEN_Warehouse  where IsActive=1  AND  WHCode  like '" + prefix + "%' order by WHCode";

            //<!---Procedure Converting--->
            // string WHSql = "select top 20 WHCode,Location,WarehouseID from GEN_Warehouse  where IsActive=1  AND  WHCode  like '" + prefix + "%'";

            //Account and Tenant filteration

            //  WHSql = WHSql + " AND AccountID = "+ cp.AccountID.ToString() +"  order by WHCode";

            string WHSql = "Exec [dbo].[USP_MST_DropWH1] @AccountID=" + cp.AccountID.ToString() + ",@TenantID=" + TenantID + ",@Prefix='" + prefix + "'";

            IDataReader rsWH = DB.GetRS(WHSql);

            while (rsWH.Read())
            {
                WHList.Add(string.Format("{0},{1}", rsWH["WHCode"] + "-" + rsWH["Location"], rsWH["WarehouseID"]));
            }
            rsWH.Close();

            return WHList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWarehouses(string prefix)
        {

            List<string> WHList = new List<string>();

            //string WHSql = "select top 20 WHCode,Location,WarehouseID from GEN_Warehouse  where IsActive=1  AND  WHCode  like '" + prefix + "%' order by WHCode";

            //<!---Procedure Converting--->
            // string WHSql = "select top 20 WHCode,Location,WarehouseID from GEN_Warehouse  where IsActive=1  AND  WHCode  like '" + prefix + "%'";

            //Account and Tenant filteration

            //  WHSql = WHSql + " AND AccountID = "+ cp.AccountID.ToString() +"  order by WHCode";

            string WHSql = "Exec [dbo].[USP_MST_DropWH] @AccountID=" + cp.AccountID.ToString() + ",@Prefix='" + prefix + "'";

            IDataReader rsWH = DB.GetRS(WHSql);

            while (rsWH.Read())
            {
                WHList.Add(string.Format("{0},{1}", rsWH["WHCode"], rsWH["WarehouseID"]));
            }
            rsWH.Close();

            return WHList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWarehousesBasedonTenant(string prefix, string tenantID)
        {

            List<string> WHList = new List<string>();

            string WHSql = "SELECT WHCode,WarehouseID FROM GEN_Warehouse WHERE WarehouseID IN(select WarehouseID from TPL_Tenant_Contract where (TenantID = " + tenantID + " OR 0 =" + tenantID + ") AND IsActive=1 and IsDeleted=0) AND WHCode like '%" + prefix + "%' AND IsActive=1 and IsDeleted=0";

            IDataReader rsWH = DB.GetRS(WHSql);

            while (rsWH.Read())
            {
                WHList.Add(string.Format("{0},{1}", rsWH["WHCode"], rsWH["WarehouseID"]));
            }
            rsWH.Close();

            return WHList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMaterialGroupData(string prefix, string TenantID)
        {

            List<string> MGroupList = new List<string>();
            string MGroupSql = "select top 10 MGroupID,MaterialGroup from MMT_MGroup  where IsActive=1  AND  MaterialGroup  like '" + prefix + "%' order by MaterialGroup";


            IDataReader rsMGroup = DB.GetRS(MGroupSql);

            while (rsMGroup.Read())
            {
                MGroupList.Add(string.Format("{0},{1}", rsMGroup["MaterialGroup"], rsMGroup["MGroupID"]));
            }

            rsMGroup.Close();

            return MGroupList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMaterialGroupDataItem(string prefix, string TenantID)
        {

            List<string> MGroupList = new List<string>();
            string MGroupSql = "select top 10 MGroupID,MaterialGroup from MMT_MGroup  where IsActive=1  AND IsDeleted=0 AND TenantID = " + TenantID + " AND MaterialGroup  like '" + prefix + "%' order by MaterialGroup";


            IDataReader rsMGroup = DB.GetRS(MGroupSql);

            while (rsMGroup.Read())
            {
                MGroupList.Add(string.Format("{0},{1}", rsMGroup["MaterialGroup"], rsMGroup["MGroupID"]));
            }

            rsMGroup.Close();

            return MGroupList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadProductCategoriesData(string prefix, string MTypeID, string TenantID)
        {

            List<string> PCatList = new List<string>();
            string PCatSql = "";
            if (MTypeID != "11")
                PCatSql = "select top 20 ProductCategory,ProductCategoryID from MMT_ProductCategory where IsActive=1 and isdeleted=0  AND  ProductCategory  like '" + prefix + "%' order by ProductCategory";
            else
                PCatSql = "select top 20 ProductCategory,ProductCategoryID from MMT_ProductCategory where IsActive=1 and isdeleted=0  AND ProductCategoryID=3 AND ProductCategory  like '" + prefix + "%' order by ProductCategory";

            IDataReader rsPCat = DB.GetRS(PCatSql);

            while (rsPCat.Read())
            {
                PCatList.Add(string.Format("{0},{1}", rsPCat["ProductCategory"], rsPCat["ProductCategoryID"]));
            }

            rsPCat.Close();

            return PCatList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadUoMData(string prefix)
        {

            List<string> UoMList = new List<string>();

            string UoMSql = "select top 20 UoMID,UoM,Description from GEN_UoM where  IsActive=1  AND  UoM  like '" + prefix + "%' order by UoM";


            IDataReader rsUoM = DB.GetRS(UoMSql);

            while (rsUoM.Read())
            {
                UoMList.Add(string.Format("{0},{1}", rsUoM["UoM"] + "-" + rsUoM["Description"], rsUoM["UoMID"]));
            }
            rsUoM.Close();
            return UoMList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadProfitCenterData(string prefix, string TenantID)
        {

            List<string> PCenterList = new List<string>();
            string PCenterSql = "Select top 20 MGroupID,ProfitCenter from MMT_MGroup where IsActive=1   AND  ProfitCenter  like '" + prefix + "%' order by ProfitCenter";


            IDataReader rsPCenter = DB.GetRS(PCenterSql);

            while (rsPCenter.Read())
            {
                PCenterList.Add(string.Format("{0},{1}", rsPCenter["ProfitCenter"], rsPCenter["MGroupID"]));
            }

            rsPCenter.Close();

            return PCenterList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadCurrencyData(string prefix)
        {

            List<string> CurrencyList = new List<string>();
            //string CurrencySql = " select Currency,CurrencyID,Code from GEN_Currency where  IsDeleted=0 AND   IsActive=1     AND  Code  like '" + prefix + "%' order by Code";

            //<!--------------------Procedure Converting -------------->  string CurrencySql = " select CurrencyID,Code+'-'+Currency as Currency from GEN_Currency where  IsDeleted=0 AND IsActive=1 AND Code+'-'+Currency like '%" + prefix + "%'  order by Code";

            //string supplierSql = "select distinct  Top 10 SupplierName+isnull('-'+SupplierCode,'') as SupplierName,SupplierID from MMT_Supplier MMS JOIN TPL_Tenant TNT ON TNT.TenantID = MMS.TenantID where MMS.ishidden = 0 and SupplierID> 0 AND MMS.IsActive = 1 AND MMS.IsDeleted = 0 AND SupplierName+isnull('-' + SupplierCode, '') like '" + prefix + "%' AND TNT.AccountID = case when 0 = " + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end order by SupplierName";

            string CurrencySql = "Exec [dbo].[USP_MST_CurrencyDropData] @prefix='" + prefix + "'";
            IDataReader rsCurrency = DB.GetRS(CurrencySql);

            while (rsCurrency.Read())
            {
                //CurrencyList.Add(string.Format("{0},{1}", rsCurrency["Code"] + "-" + rsCurrency["Currency"], rsCurrency["CurrencyID"]));
                CurrencyList.Add(string.Format("{0},{1}", rsCurrency["Currency"], rsCurrency["CurrencyID"]));

            }

            rsCurrency.Close();

            return CurrencyList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadPurchageGroupData(string prefix, string TenantID)
        {

            List<string> PGroupList = new List<string>();
            string PGroupSql = " Select PurchaseGroupID, PurchaseGroupCode, PurchaseGroup as PurchaseGroupDesc from MMT_PurchaseGroup where IsDeleted=0 AND IsActive = 1 AND  PurchaseGroupCode  like '" + prefix + "%' order by PurchaseGroupCode";


            IDataReader rsPGroup = DB.GetRS(PGroupSql);

            while (rsPGroup.Read())
            {
                PGroupList.Add(string.Format("{0},{1}", rsPGroup["PurchaseGroupCode"] + "-" + rsPGroup["PurchaseGroupDesc"], rsPGroup["PurchaseGroupID"]));
            }
            rsPGroup.Close();
            return PGroupList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadShipmentTypeData(string prefix, String TenantID)
        {

            List<string> ShpTypeList = new List<string>();
            string ShpTypeSql = " select ShipmentType,ShipmentTypeID from GEN_ShipmentType where ShipmentTypeID NOT IN (6,7,4)  AND IsActive=1 and IsDeleted=0 and ShipmentType like'" + prefix + "%' order by ShipmentType";


            IDataReader rsShpType = DB.GetRS(ShpTypeSql);

            while (rsShpType.Read())
            {
                ShpTypeList.Add(string.Format("{0},{1}", rsShpType["ShipmentType"], rsShpType["ShipmentTypeID"]));
            }

            rsShpType.Close();

            return ShpTypeList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadClearenceCompanyData(string prefix, string TenantID)
        {

            List<string> CCList = new List<string>();
            string CCSql = " select ClearanceCompanyID,ClearanceCompany from GEN_ClearanceCompany where IsActive=1 and IsDeleted=0 and ClearanceCompany like'" + prefix + "%' AND TenantID = case when  " + cp.TenantID.ToString() + "=0  then TenantID else " + cp.TenantID.ToString() + " end order by ClearanceCompany";


            IDataReader rsCC = DB.GetRS(CCSql);

            while (rsCC.Read())
            {
                CCList.Add(string.Format("{0},{1}", rsCC["ClearanceCompany"], rsCC["ClearanceCompanyID"]));
            }

            rsCC.Close();

            return CCList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMCodeData(string prefix, string TenantID)
        {
            List<string> MMList = new List<string>();
            string MMSql = "select  Top 20 MCode from MMT_MaterialMaster where IsDeleted=0 and IsActive=1 AND ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";

            IDataReader rsMM = DB.GetRS(MMSql);

            while (rsMM.Read())
            {
                MMList.Add(string.Format("{0}", rsMM["MCode"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadSupplierPartNoData(string prefix)
        {
            List<string> SPList = new List<string>();
            string SPListSql = "select Top 30 SupplierPartNo from MMT_MaterialMaster where SupplierPartNo like '" + prefix + "%' order by SupplierPartNo";


            IDataReader rsSPList = DB.GetRS(SPListSql);

            while (rsSPList.Read())
            {
                SPList.Add(string.Format("{0}", rsSPList["SupplierPartNo"]));
            }
            rsSPList.Close();
            return SPList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CheckSupplierPartNo(string _CatlogNumber, string TenantID)
        {
            string IsSPartNo = "false";

            string supplerCode = "";// DB.GetSqlS("  select SupplierCode AS S from MMT_Supplier where TenantID=" + TenantID + " and  SupplierID=" + _SupplierID);

            supplerCode = _CatlogNumber;

            string SPSql = "select MaterialMasterID from MMT_MaterialMaster where MCode='" + supplerCode + "'";

            IDataReader rsSP = DB.GetRS(SPSql);

            while (rsSP.Read())
            {
                IsSPartNo = "true";
            }

            rsSP.Close();

            return IsSPartNo;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string IsRecordTypeChecked(string RoutingHeaderID, string RoutingDetailsActivityID)
        {
            string IsSPartNo = "";


            string SPSql = "select Convert(nvarchar,RecordedType) +','+ Convert(nvarchar,MFG_RQC.ParameterDataTypeID) AS S from MFG_RoutingDetailsActivity_QualityParameter MFG_RQC LEFT JOIN GEN_ParameterDataType GEN_PD ON GEN_PD.ParameterDataTypeID=MFG_RQC.ParameterDataTypeID AND GEN_PD.IsActive=1 AND GEN_PD.IsDeleted=0 LEFT JOIN MFG_RoutingDetailsActivity MFG_RDA ON MFG_RDA.RoutingDetailsActivityID=MFG_RQC.RoutingDetailsActivityID LEFT JOIN MFG_RoutingDetails MFG_RD ON MFG_RD.RoutingDetailsID=MFG_RDA.RoutingDetailsID where MFG_RQC.IsActive=1 AND MFG_RQC.IsDeleted=0 AND MFG_RD.RoutingHeaderID=" + RoutingHeaderID + " AND MFG_RQC.RoutingDetailsActivity_QualityParameterID=" + RoutingDetailsActivityID;

            IsSPartNo = DB.GetSqlS(SPSql);


            return IsSPartNo;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadFreightCompanies(String prefix)
        {
            List<String> list = new List<String>();
            //Procedure Converting
            // String sql = "select FreightCompany,FreightCompanyID from GEN_FreightCompany WHERE IsDeleted=0 and IsActive=1 and FreightCompany like '" + prefix + "%'";
            String sql = "Exec [dbo].[USP_LoadFreightCompanies] @Prefix='" + prefix + "'";
            IDataReader rs = DB.GetRS(sql);
            while (rs.Read())
            {
                list.Add(String.Format("{0},{1}", rs["FreightCompany"], rs["FreightCompanyID"]));

            }
            rs.Close();

            return list.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadSOCustomerNames(string prefix, string TenantID)
        {
            List<String> list = new List<string>();
            //<!-----------Procedure Converting------------->
            //   String CustomerSql = "SELECT distinct  GEN_Cus.CustomerID,CustomerName FROM GEN_Customer GEN_Cus LEFT JOIN ORD_SOHeader ORD_SO on ORD_SO.CustomerID=GEN_Cus.CustomerID  and ORD_SO.IsActive=1 and ORD_SO.IsDeleted=0  where ORD_SO.SOStatusID=1 and  GEN_Cus.TenantID=" + TenantID + "  and  GEN_Cus.IsActive=1 and GEN_Cus.IsDeleted=0 and  GEN_Cus.CustomerID NOT IN (-1,-2)    AND CustomerName like '" + prefix + "%'";
            String CustomerSql = "Exec [dbo].[USP_GetCustomersForOBD] @Prefix='" + prefix + "',@TenantID=" + TenantID + "";
            IDataReader rsCustomer = DB.GetRS(CustomerSql);
            while (rsCustomer.Read())
            {
                list.Add(String.Format("{0},{1}", rsCustomer["CustomerName"], rsCustomer["CustomerID"]));
            }

            rsCustomer.Close();

            return list.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadCustomerNames(string prefix, string TenantID)
        {
            List<String> list = new List<string>();

            //String CustomerSql = "SELECT Top 10 GEN_Cus.CustomerID,CustomerName FROM GEN_Customer GEN_Cus where ishidden=0 and  GEN_Cus.TenantID=" + TenantID + "  and  GEN_Cus.IsActive=1 and GEN_Cus.IsDeleted=0 and GEN_Cus.CustomerName like '" + prefix + "%'";

            //CustomerSql = CustomerSql + " AND TenantID = case when 0 = " + cp.TenantID.ToString() + " then TenantID else " + cp.TenantID.ToString() + " end ";


            String CustomerSql = "Exec [dbo].[USP_LoadCustomers] @Prefix='" + prefix + "',@TenantID=" + TenantID + ",@LoggedTenantID=" + cp.TenantID.ToString() + "";
            IDataReader rsCustomer = DB.GetRS(CustomerSql);
            while (rsCustomer.Read())
            {
                list.Add(String.Format("{0},{1}", rsCustomer["CustomerName"], rsCustomer["CustomerID"]));
            }

            rsCustomer.Close();

            return list.ToArray();
        }

        //load addresstypes
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadAddressTypes(string prefix, string CustomerID)
        {
            List<String> list = new List<string>();

            //String CustomerSql = "SELECT distinct GC.CustomerID, DeliveryPoint,[dbo].[UDF_ParseAndReturnLocaleString](AT.AddressType,'en') as AddressType, AT.GEN_MST_AddressType_ID,MA.GEN_MST_Address_ID FROM GEN_MST_EntityAddresses EA INNER JOIN GEN_MST_OrgEntities ORG ON EA.GEN_MST_OrgEntity_ID = ORG.GEN_MST_OrgEntity_ID AND ORG.GEN_MST_OrgEntity_ID = 2 AND ORG.IsActive = 1 AND ORG.IsDeleted = 0 AND EA.IsActive = 1 AND EA.IsDeleted = 0 INNER JOIN GEN_MST_Addresses MA ON MA.GEN_MST_Address_ID = EA.GEN_MST_Address_ID AND MA.IsActive = 1 AND MA.IsDeleted = 0 INNER JOIN GEN_MST_AddressTypes AT ON AT.GEN_MST_AddressType_ID = MA.GEN_MST_AddressType_ID AND AT.IsActive = 1 AND AT.IsDeleted = 0 INNER JOIN GEN_Customer GC ON GC.CustomerID = EA.EntityID AND GC.ISActive = 1 AND MA.GEN_MST_AddressType_ID!=2 and GC.IsDeleted = 0 AND (GC.CustomerID = ISNULL(" + CustomerID+", GC.CustomerID) OR "+CustomerID+" = 0) ";
            //  <!------------------Converting into Procedure  -------------------->
            //  String CustomerSql = "SELECT distinct GC.CustomerID, DeliveryPoint,[dbo].[UDF_ParseAndReturnLocaleString](AT.AddressType,'en') as AddressType, AT.GEN_MST_AddressType_ID,MA.GEN_MST_Address_ID FROM GEN_MST_EntityAddresses EA INNER JOIN GEN_MST_OrgEntities ORG ON EA.GEN_MST_OrgEntity_ID = ORG.GEN_MST_OrgEntity_ID AND ORG.GEN_MST_OrgEntity_ID = 2 AND ORG.IsActive = 1 AND ORG.IsDeleted = 0 AND EA.IsActive = 1 AND EA.IsDeleted = 0 INNER JOIN GEN_MST_Addresses MA ON MA.GEN_MST_Address_ID = EA.GEN_MST_Address_ID AND MA.IsActive = 1 AND MA.IsDeleted = 0 INNER JOIN GEN_MST_AddressTypes AT ON AT.GEN_MST_AddressType_ID = MA.GEN_MST_AddressType_ID AND AT.IsActive = 1 AND AT.IsDeleted = 0 INNER JOIN GEN_Customer GC ON GC.CustomerID = EA.EntityID AND GC.ISActive = 1 AND MA.GEN_MST_AddressType_ID!=2 and GC.IsDeleted = 0 AND GC.CustomerID = " + CustomerID ;
            String CustomerSql = "Exec [dbo].[USP_SOLoadAddressTypes] @CustomerID=" + CustomerID;
            IDataReader rsCustomer = DB.GetRS(CustomerSql);
            while (rsCustomer.Read())
            {
                list.Add(String.Format("{0},{1}", rsCustomer["AddressType"] + " - " + rsCustomer["DeliveryPoint"], rsCustomer["GEN_MST_Address_ID"]));
            }

            rsCustomer.Close();

            return list.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadDivision(String prefix, String TenantID, String DeptID)
        {
            List<String> DivList = new List<String>();
            String DivSql = "select Division,DivisionID from GEN_Division where IsDeleted=0 and  DepartmentID=" + DeptID + " AND Division like '" + prefix + "%'";
            IDataReader rsDiv = DB.GetRS(DivSql);
            while (rsDiv.Read())
            {
                DivList.Add(String.Format("{0},{1}", rsDiv["Division"], rsDiv["DivisionID"]));

            }
            rsDiv.Close();

            return DivList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadVLPDS(String prefix, int TenantID)
        {
            //    if(TenantID=="")
            //    {
            //        TenantID = null;
            //    }

            List<String> DivList = new List<String>();
            String DivSql = "Exec Get_VLPDList @TenantID=" + TenantID + " ";
            IDataReader rsDiv = DB.GetRS(DivSql);
            while (rsDiv.Read())
            {
                DivList.Add(String.Format("{0},{1}", rsDiv["VLPDNumber"], rsDiv["VLPDID"]));

            }
            rsDiv.Close();

            return DivList.ToArray();
        }
        //load vlpds
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadVLPDS1(String prefix, int TenantID, int WarehouseID)
        {
            //    if(TenantID=="")
            //    {
            //        TenantID = null;
            //    }

            List<String> DivList = new List<String>();
            String DivSql = "Exec Get_VLPDList @TenantID=" + TenantID + ",@WarehouseID=" + WarehouseID;
            IDataReader rsDiv = DB.GetRS(DivSql);
            while (rsDiv.Read())
            {
                DivList.Add(String.Format("{0},{1}", rsDiv["VLPDNumber"], rsDiv["VLPDID"]));

            }
            rsDiv.Close();

            return DivList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadDepartment(String prefix, String TenentID)
        {
            List<String> DeptList = new List<String>();
            String DeptSql = "select Department,DepartmentID from GEN_Department where IsDeleted=0 AND Department like '" + prefix + "%'";
            IDataReader rsDept = DB.GetRS(DeptSql);
            while (rsDept.Read())
            {
                DeptList.Add(String.Format("{0},{1}", rsDept["Department"], rsDept["DepartmentID"]));

            }
            rsDept.Close();

            return DeptList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPONumbers(String prefix, String TenentID, String SupplierID)
        {
            List<String> POList = new List<String>();
            String POSql = "select top 10 PONumber,POHeaderID from ORD_POHeader where POStatusID<=2 AND IsActive=1 and IsDeleted=0 and ( 0=" + SupplierID + " or SupplierID=" + SupplierID + ")  and  TenantID=" + TenentID + " and PONumber like '" + prefix + "%' order by  CreatedOn desc ";
            IDataReader rsPO = DB.GetRS(POSql);
            while (rsPO.Read())
            {
                POList.Add(String.Format("{0},{1}", rsPO["PONumber"], rsPO["POHeaderID"]));
            }
            rsPO.Close();

            return POList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadKitMcodes(String prefix, string tenantid, string kittypeid, string headerid)
        {
            List<String> POList = new List<String>();
            String POSql = "select top 10 kit.KitPlannerID,KitCode  from MMT_KitPlanner kit " +
                            "join(select distinct KitPlannerID from MMT_KitPlannerDetail where IsDeleted = 0 and IsActive = 1) detail on kit.KitPlannerID = detail.KitPlannerID and kit.IsDeleted = 0 " +
                            " and kit.IsActive = 1 where  KitTypeID = " + kittypeid + " and kit.KitPlannerID not in (select KitPlannerID from MMT_KitJobOrderDetails where KitJobOrderHeaderID = " + headerid + ") " +
                            "and(KitCode like '" + prefix + "%' or KitCode like '" + prefix + "%' or KitCode like '%" + prefix + "%') ";
            IDataReader rsPO = DB.GetRS(POSql);
            while (rsPO.Read())
            {
                POList.Add(String.Format("{0},{1}", rsPO["KitCode"], rsPO["KitPlannerID"]));
            }
            rsPO.Close();

            return POList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadSONumbers(String prefix, String TenentID, String CustomerID)
        {
            List<String> SOList = new List<String>();
            //<!---------------Procedure Converting----->
            // String SOSql = "select SONumber,SOHeaderID from ORD_SOHeader where IsActive=1 and IsDeleted=0 and SOStatusID=1 and (0=" + (CustomerID!=""?CustomerID:"0") + " or  CustomerID=" + CustomerID + ") and TenantID=" + TenentID + " and SONumber like '" + prefix + "%' order by SONumber";
            String SOSql = "Exec [dbo].[USP_GetSONumbersForOBD]  @Prefix='" + prefix + "',@CustomerID=" + CustomerID + ", @TenantID=" + TenentID + "";
            IDataReader rsSO = DB.GetRS(SOSql);
            while (rsSO.Read())
            {
                SOList.Add(String.Format("{0},{1}", DB.RSField(rsSO, "SONumber"), DB.RSFieldInt(rsSO, "SOHeaderID")));

            }
            rsSO.Close();

            return SOList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadSONumbersList(String prefix, String TenentID, String IsToolItem)
        {
            List<String> SOList = new List<String>();

            if (TenentID == "")
                TenentID = "0";

            //String SOSql = "select SONumber,SOHeaderID from ORD_SOHeader ORD inner join TPL_Tenant TNT on ORD.TenantID=TNT.TenantID and TNT.isactive=1 and TNT.IsDeleted=0 where ORD.IsActive = 1 and ORD.IsDeleted = 0 and (0=" + IsToolItem + " or SOTypeID=9) and (1=" + IsToolItem + " or SOTypeID!=9) and (0=" + TenentID + " OR TNT.TenantID=" + TenentID + ") and SONumber like '" + prefix + "%'";

            //SOSql = SOSql + " AND TNT.TenantID = case when 0 = " + cp.TenantID.ToString() + " then TNT.TenantID else " + cp.TenantID.ToString() + " end AND TNT.AccountID = " + cp.AccountID.ToString()+"   order by SONumber";

            string SOSql = "Exec [dbo].[USP_GetSONumbersList] @LogTenantID=" + cp.TenantID.ToString() + ",@LogAccountID=" + cp.AccountID.ToString() + " ,@Prefix='" + prefix + "',@IsToolItem=" + IsToolItem + "";
            IDataReader rsSO = DB.GetRS(SOSql);
            while (rsSO.Read())
            {
                SOList.Add(String.Format("{0},{1}", DB.RSField(rsSO, "SONumber"), DB.RSFieldInt(rsSO, "SOHeaderID")));

            }
            rsSO.Close();

            return SOList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadProOrderNumbers(String prefix, String TenantID)
        {
            List<String> ProOList = new List<String>();

            if (TenantID == "")
                TenantID = "0";

            String ProOSql = "select PRORefNo,ProductionOrderHeaderID from MFG_ProductionOrderHeader where IsActive=1 and IsDeleted=0 and ProductionOrderStatusID=1  and TenantID=" + TenantID + " and PRORefNo like '" + prefix + "%' order by PRORefNo";
            IDataReader rsProO = DB.GetRS(ProOSql);
            while (rsProO.Read())
            {
                ProOList.Add(String.Format("{0},{1}", DB.RSField(rsProO, "PRORefNo"), DB.RSFieldInt(rsProO, "ProductionOrderHeaderID")));

            }
            rsProO.Close();

            return ProOList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadInvoiceNumbers(String prefix, String SupplierID, String POHeaderID, String SupplierInvoiceID)
        {
            List<String> InvList = new List<String>();
            // String InvSql = "select InvoiceNumber,SupplierInvoiceID from ORD_SupplierInvoice ORD_SUPI LEFT JOIN ORD_POHeader ORD_POH on ORD_POH.POStatusID=1 and ORD_POH.POHeaderID=ORD_SUPI.POHeaderID and ORD_POH.IsActive=1 and ORD_POH.IsDeleted=0 where ORD_SUPI.IsActive=1 and ORD_SUPI.IsDeleted=0 and ORD_POH.SupplierID=" + SupplierID + " and SupplierInvoiceID NOT IN ( SELECT SupplierInvoiceID from INB_Inbound_ORD_SupplierInvoice where IsActive=1 and IsDeleted=0 AND SupplierInvoiceID!="+SupplierInvoiceID+")  and InvoiceNumber like '" + prefix + "%' order by InvoiceNumber";

            //            String InvSql = "select InvoiceNumber,ORD_SUPI.SupplierInvoiceID from ORD_SupplierInvoice ORD_SUPI JOIN ORD_POHeader ORD_POH on ORD_POH.POStatusID=1 and ORD_POH.POHeaderID=ORD_SUPI.POHeaderID and ORD_POH.IsActive=1 and ORD_POH.IsDeleted=0 JOIN ORD_PODetails POD ON POD.IsDeleted=0 and pod.POHeaderID=ORD_SUPI.POHeaderID and pod.IsActive=1 JOIN ORD_SupplierInvoiceDetails ORD_SUPID ON ORD_SUPID.PODetailsID=POD.PODetailsID AND ORD_SUPID.IsActive=1 AND ORD_SUPID.IsDeleted=0 where ORD_SUPI.IsActive=1 and ORD_SUPI.IsDeleted=0 and ORD_POH.SupplierID="+SupplierID+" and  ORD_SUPI.SupplierInvoiceID NOT IN ( SELECT SupplierInvoiceID from INB_Inbound_ORD_SupplierInvoice where IsActive=1 and IsDeleted=0)  and InvoiceNumber like '"+prefix+"%' order by InvoiceNumber";

            //String InvSql = "select ORD_SUPI.InvoiceNumber,ORD_SUPI.SupplierInvoiceID from ORD_SupplierInvoice ORD_SUPI where ORD_SUPI.IsActive=1 AND ORD_SUPI.IsDeleted=0 AND ORD_SUPI.SupplierID=" + SupplierID + " AND ORD_SUPI.POHeaderID=" + POHeaderID + "and ORD_SUPI.InvoiceNumber like '" + prefix + "%' order by ORD_SUPI.InvoiceNumber";

            SupplierInvoiceID = (SupplierInvoiceID == "" ? "0" : SupplierInvoiceID);
            //Procedure conversion
            // String InvSql = "select ORD_SUPI.InvoiceNumber,ORD_SUPI.SupplierInvoiceID from ORD_SupplierInvoice ORD_SUPI where ORD_SUPI.IsActive=1 AND ORD_SUPI.IsDeleted=0 AND ORD_SUPI.SupplierID=" + SupplierID + " AND ORD_SUPI.POHeaderID=" + POHeaderID + " AND ( ORD_SUPI.SupplierInvoiceID=" + SupplierInvoiceID + " OR ORD_SUPI.SupplierInvoiceID NOT IN ( select ORD_SUPI1.SupplierInvoiceID from INB_Inbound_ORD_SupplierInvoice ORD_SUPI1 where ORD_SUPI1.IsActive=1 AND ORD_SUPI1.IsDeleted=0 AND ORD_SUPI1.POHeaderID=" + POHeaderID + " )  )  and ORD_SUPI.InvoiceNumber like '" + prefix + "%' order by ORD_SUPI.InvoiceNumber";
            String InvSql = "[dbo].[USP_InboundLoadInvoiceNo] @SupplierID= " + SupplierID + ",@POHeaderID=" + POHeaderID + ",@prefix='" + prefix + "',@SupplierInvoiceID=" + SupplierInvoiceID + "";

            IDataReader rsInv = DB.GetRS(InvSql);
            while (rsInv.Read())
            {
                InvList.Add(String.Format("{0},{1}", rsInv["InvoiceNumber"], rsInv["SupplierInvoiceID"]));

            }
            rsInv.Close();

            return InvList.ToArray();
        }

        // for multiple grns in Inbound -- gopi

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadMaterialCodesforInbound(String prefix, String SupplierID, String POHeaderID, String SupplierInvoiceID)
        {
            List<String> InvList = new List<String>();

            SupplierInvoiceID = (SupplierInvoiceID == "" ? "0" : SupplierInvoiceID);

            POHeaderID = (POHeaderID == "" ? "0" : POHeaderID);

            StringBuilder sb = new StringBuilder();

            sb.Append("select MM.MCode, MM.MaterialMasterID from[ORD_SupplierInvoice] SIH ");
            sb.Append(" JOIN ORD_SupplierInvoiceDetails SIND  on SIH.SupplierInvoiceID = SIND.SupplierInvoiceID AND SIND.IsActive = 1 AND SIND.IsDeleted = 0");
            sb.Append(" JOIN ORD_PODetails ORD_POD ON SIND.PODetailsID=ORD_POD.PODetailsID AND ORD_POD.IsActive=1 AND ORD_POD.IsDeleted=0");
            sb.Append(" JOIN MMT_MaterialMaster MM ON ORD_POD.MaterialMasterID = MM.MaterialMasterID AND MM.IsActive = 1 AND MM.IsDeleted = 0");
            sb.Append(" where SIH.IsActive = 1 AND SIH.IsDeleted = 0 and SIH.SupplierInvoiceID =" + SupplierInvoiceID + " AND ORD_POD.POHeaderID = " + POHeaderID);

            String InvSql = sb.ToString();
            IDataReader rsInv = DB.GetRS(InvSql);
            while (rsInv.Read())
            {
                InvList.Add(String.Format("{0},{1}", rsInv["MCode"], rsInv["MaterialMasterID"]));

            }
            rsInv.Close();

            return InvList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPOLineNumbersforInbound(String prefix, String SupplierID, String POHeaderID, String MaterialMasterID)
        {
            List<String> InvList = new List<String>();

            MaterialMasterID = (MaterialMasterID == "" ? "0" : MaterialMasterID);

            POHeaderID = (POHeaderID == "" ? "0" : POHeaderID);

            StringBuilder sb = new StringBuilder();

            sb.Append(" select  ORD_POD.LineNumber, ORD_POD.PODetailsID from MMT_MaterialMaster MM  ");
            sb.Append("JOIN ORD_PODetails ORD_POD ON MM.MaterialMasterID = ORD_POD.MaterialMasterID AND ORD_POD.IsActive=1 AND ORD_POD.IsDeleted=0");
            sb.Append("where MM.IsActive=1 AND MM.IsDeleted=0 and MM.MaterialMasterID=" + MaterialMasterID + "AND ORD_POD.POHeaderID=" + POHeaderID);

            String InvSql = sb.ToString();
            IDataReader rsInv = DB.GetRS(InvSql);
            while (rsInv.Read())
            {
                InvList.Add(String.Format("{0},{1}", rsInv["LineNumber"], rsInv["LineNumber"]));

            }
            rsInv.Close();
            return InvList.ToArray();
        }


        // Material code Ends

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPOInvoiceNumbers(String prefix, String POHeaderID)
        {
            List<String> InvList = new List<String>();
            // String InvSql = "select InvoiceNumber,SupplierInvoiceID from ORD_SupplierInvoice ORD_SUPI LEFT JOIN ORD_POHeader ORD_POH on ORD_POH.POStatusID=1 and ORD_POH.POHeaderID=ORD_SUPI.POHeaderID and ORD_POH.IsActive=1 and ORD_POH.IsDeleted=0 where ORD_SUPI.IsActive=1 and ORD_SUPI.IsDeleted=0 and ORD_POH.SupplierID=" + SupplierID + " and SupplierInvoiceID NOT IN ( SELECT SupplierInvoiceID from INB_Inbound_ORD_SupplierInvoice where IsActive=1 and IsDeleted=0 AND SupplierInvoiceID!="+SupplierInvoiceID+")  and InvoiceNumber like '" + prefix + "%' order by InvoiceNumber";

            String InvSql = "select ORD_SUPI.InvoiceNumber,ORD_SUPI.SupplierInvoiceID from ORD_SupplierInvoice ORD_SUPI  where ORD_SUPI.POHeaderID=" + POHeaderID + " AND ORD_SUPI.IsActive=1 AND ORD_SUPI.IsDeleted=0 AND InvoiceNumber LIKE '" + prefix + "%' order by InvoiceNumber";
            IDataReader rsInv = DB.GetRS(InvSql);
            while (rsInv.Read())
            {
                InvList.Add(String.Format("{0},{1}", rsInv["InvoiceNumber"], rsInv["SupplierInvoiceID"]));

            }
            rsInv.Close();

            return InvList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadGRNPOInvoiceNumbers(String prefix, String POHeaderID, String SupplierInvoiceID, String InboundID)
        {
            List<String> InvList = new List<String>();
            // String InvSql = "select InvoiceNumber,SupplierInvoiceID from ORD_SupplierInvoice ORD_SUPI LEFT JOIN ORD_POHeader ORD_POH on ORD_POH.POStatusID=1 and ORD_POH.POHeaderID=ORD_SUPI.POHeaderID and ORD_POH.IsActive=1 and ORD_POH.IsDeleted=0 where ORD_SUPI.IsActive=1 and ORD_SUPI.IsDeleted=0 and ORD_POH.SupplierID=" + SupplierID + " and SupplierInvoiceID NOT IN ( SELECT SupplierInvoiceID from INB_Inbound_ORD_SupplierInvoice where IsActive=1 and IsDeleted=0 AND SupplierInvoiceID!="+SupplierInvoiceID+")  and InvoiceNumber like '" + prefix + "%' order by InvoiceNumber";

            SupplierInvoiceID = (SupplierInvoiceID == "" ? "0" : SupplierInvoiceID);


            //String InvSql = "select ORD_SUPI.InvoiceNumber,ORD_SUPI.SupplierInvoiceID from ORD_SupplierInvoice ORD_SUPI  JOIN INB_Inbound_ORD_SupplierInvoice INB_SUPI ON INB_SUPI.SupplierInvoiceID=ORD_SUPI.SupplierInvoiceID AND INB_SUPI.IsActive=1 AND INB_SUPI.IsDeleted=0  where ORD_SUPI.POHeaderID=" + POHeaderID + " AND ORD_SUPI.IsActive=1 AND ORD_SUPI.IsDeleted=0 AND InvoiceNumber LIKE '" + prefix + "%' order by InvoiceNumber";

            //String InvSql = "select ORD_SUPI.InvoiceNumber,ORD_SUPI.SupplierInvoiceID from ORD_SupplierInvoice ORD_SUPI  JOIN INB_Inbound_ORD_SupplierInvoice INB_SUPI ON INB_SUPI.SupplierInvoiceID=ORD_SUPI.SupplierInvoiceID AND INB_SUPI.IsActive=1 AND INB_SUPI.IsDeleted=0  where ORD_SUPI.POHeaderID=" + POHeaderID + " AND ( ORD_SUPI.SupplierInvoiceID=" + SupplierInvoiceID + " OR ORD_SUPI.SupplierInvoiceID NOT IN( select IBG.SupplierInvoiceID from INB_GRNUpdate IBG where IBG.POHeaderID=" + POHeaderID + " AND IBG.IsActive=1 AND IBG.IsDeleted=0 AND  IBG.InboundID=" + InboundID + " ) )  AND ORD_SUPI.IsActive=1 AND ORD_SUPI.IsDeleted=0 AND INB_SUPI.InboundID="+InboundID+"  AND InvoiceNumber LIKE '" + prefix + "%' order by InvoiceNumber";

            String InvSql = "select ORD_SUPI.InvoiceNumber,ORD_SUPI.SupplierInvoiceID from ORD_SupplierInvoice ORD_SUPI JOIN INB_Inbound_ORD_SupplierInvoice INB_SUPI ON INB_SUPI.SupplierInvoiceID = ORD_SUPI.SupplierInvoiceID AND INB_SUPI.IsActive = 1 AND INB_SUPI.IsDeleted = 0 where ORD_SUPI.POHeaderID = " + POHeaderID + " AND ORD_SUPI.IsActive = 1 AND ORD_SUPI.IsDeleted = 0 AND INB_SUPI.InboundID = " + InboundID + "  AND InvoiceNumber LIKE '" + prefix + "%' order by InvoiceNumber";

            IDataReader rsInv = DB.GetRS(InvSql);
            while (rsInv.Read())
            {
                InvList.Add(String.Format("{0}~{1}", rsInv["InvoiceNumber"], rsInv["SupplierInvoiceID"]));

            }
            rsInv.Close();

            return InvList.ToArray();
        }








        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadDisInvoiceNumbers(String prefix, String InboundID)
        {
            List<String> InvList = new List<String>();
            String InvSql = "select ORD_SUPI.InvoiceNumber,ORD_SUPI.SupplierInvoiceID from ORD_SupplierInvoice ORD_SUPI LEFT JOIN INB_Inbound_ORD_SupplierInvoice INB_SupI on INB_SupI.SupplierInvoiceID=ORD_SUPI.SupplierInvoiceID and INB_SupI.IsActive=1 and INB_SupI.IsDeleted=0 where ORD_SUPI.IsActive=1 and ORD_SUPI.IsDeleted=0 and  INB_SupI.InboundID=" + InboundID + " and ORD_SUPI.InvoiceNumber  like '" + prefix + "%' order by ORD_SUPI.InvoiceNumber";
            IDataReader rsInv = DB.GetRS(InvSql);
            while (rsInv.Read())
            {
                InvList.Add(String.Format("{0},{1}", rsInv["InvoiceNumber"], rsInv["SupplierInvoiceID"]));

            }
            rsInv.Close();

            return InvList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadConfiguredIBPONumbers(String prefix, String TenentID, String InboundID, String SupplierInvoiceID)
        {
            List<String> POList = new List<String>();
            String POSql = "  select ORD_POH.POHeaderID,ORD_POH.PONumber from INB_Inbound_ORD_SupplierInvoice INB_ORD_S left join ORD_POHeader ORD_POH on ORD_POH.POHeaderID=INB_ORD_S.POHeaderID left join ORD_SupplierInvoice ORD_Sup on ORD_Sup.SupplierInvoiceID=INB_ORD_S.SupplierInvoiceID and ORD_Sup.POHeaderID=INB_ORD_S.POHeaderID where INB_ORD_S.IsActive=1 and INB_ORD_S.IsDeleted=0 and TenantID=" + TenentID + " and  INB_ORD_S.InboundID=" + InboundID + " and  ORD_Sup.SupplierInvoiceID=" + SupplierInvoiceID + " AND ORD_POH.PONumber like  '" + prefix + "%' order by ORD_POH.PONumber";
            IDataReader rsPO = DB.GetRS(POSql);
            while (rsPO.Read())
            {
                POList.Add(String.Format("{0},{1}", rsPO["PONumber"], rsPO["POHeaderID"]));

            }
            rsPO.Close();

            return POList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadConfiguredGRNIBPONumbers(String prefix, String TenentID, String InboundID)
        {
            List<String> POList = new List<String>();
            //String POSql = "  select distinct ORD_POH.POHeaderID,ORD_POH.PONumber from INB_Inbound_ORD_SupplierInvoice INB_ORD_S left join ORD_POHeader ORD_POH on ORD_POH.POHeaderID=INB_ORD_S.POHeaderID left join ORD_SupplierInvoice ORD_Sup on ORD_Sup.SupplierInvoiceID=INB_ORD_S.SupplierInvoiceID and ORD_Sup.POHeaderID=INB_ORD_S.POHeaderID where INB_ORD_S.IsActive=1 and INB_ORD_S.IsDeleted=0 and TenantID=" + TenentID + " and  INB_ORD_S.InboundID=" + InboundID +  " AND ORD_POH.PONumber like  '" + prefix + "%' order by ORD_POH.PONumber";
            String POSql = "  SELECT DISTINCT PONumber, POH.POHeaderID FROM(SELECT SupplierInvoiceID, POHeaderID, InboundID FROM INB_Inbound_ORD_SupplierInvoice WHERE InboundID = " + InboundID + "AND  IsActive = 1 AND IsDeleted = 0)INB_ORD_S INNER JOIN ORD_SupplierInvoice SI ON SI.SupplierInvoiceID = INB_ORD_S.SupplierInvoiceID AND SI.IsActive = 1 AND SI.IsDeleted = 0 AND SI.POHeaderID = INB_ORD_S.POHeaderID LEFT JOIN ORD_SupplierInvoiceDetails OSID ON OSID.SupplierInvoiceID = SI.SupplierInvoiceID AND OSID.IsActive = 1 AND OSID.IsDeleted = 0 LEFT JOIN ORD_PODetails POD ON POD.PODetailsID = OSID.PODetailsID AND POD.IsActive = 1 AND POD.IsDeleted = 0 LEFT JOIN INB_GRNUpdate GRN ON GRN.InboundID = INB_ORD_S.InboundID AND GRN.POHeaderID = INB_ORD_S.POHeaderID AND GRN.SupplierInvoiceID = SI.SupplierInvoiceID AND GRN.IsActive = 1 AND GRN.IsDeleted = 0 LEFT JOIN INB_GRNUpdateDetails GRND ON GRND.GRNUpdateID = GRN.GRNUpdateID AND GRND.MaterialMasterID = POD.MaterialMasterID AND GRND.LineNumber = POD.LineNumber LEFT JOIN ORD_POHeader POH ON POH.POHeaderID = SI.POHeaderID AND POH.IsActive = 1 AND POH.IsDeleted = 0 WHERE POH.TenantID =" + TenentID + "AND POH.PONumber like  '" + prefix + "%' GROUP BY PONumber, POH.POHeaderID ";
            IDataReader rsPO = DB.GetRS(POSql);
            while (rsPO.Read())
            {
                POList.Add(String.Format("{0},{1}", rsPO["PONumber"], rsPO["POHeaderID"]));

            }
            rsPO.Close();

            return POList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPOMCodes(String prefix, String InboundID, String SupplierInvoiceID)
        {
            List<String> POList = new List<String>();
            //String POSql = "select distinct ORD_POD.LineNumber, ORD_POD.MaterialMasterID,MCode  from ORD_PODetails ORD_POD  LEFT JOIN MMT_MaterialMaster MMT_M on MMT_M.MaterialMasterID=ORD_POD.MaterialMasterID and MMT_M.IsDeleted=0 and MMT_M.IsActive=1 and MMT_M.IsAproved=1  LEFT JOIN ORD_SupplierInvoiceDetails ORD_SUPID ON ORD_SUPID.PODetailsID=ORD_POD.PODetailsID AND ORD_SUPID.IsActive=1 AND ORD_SUPID.IsDeleted=0 LEFT JOIN INB_Inbound_ORD_SupplierInvoice INB_Sup on  INB_Sup.SupplierInvoiceID=ORD_SUPID.SupplierInvoiceID AND INB_Sup.POHeaderID=ORD_POD.POHeaderID and INB_Sup.IsActive=1 and INB_Sup.IsDeleted=0  where ORD_POD.IsDeleted=0 and ORD_POD.IsActive=1  AND InboundID="+InboundID+" and  INB_Sup.SupplierInvoiceID="+SupplierInvoiceID+"   and MCode like '" + prefix + "%'";
            // String POSql = "select distinct INV_GMD.POSODetailsID,MMT_MM.MCode,MMT_MM.MaterialMasterID from INV_GoodsMovementDetails INV_GMD JOIN MMT_MaterialMaster MMT_MM ON MMT_MM.MaterialMasterID=INV_GMD.MaterialMasterID AND MMT_MM.IsActive=1 AND MMT_MM.IsDeleted=0 JOIN ORD_SupplierInvoiceDetails ORD_SD  ON ORD_SD.SupplierInvoiceDetailsID=INV_GMD.POSODetailsID AND  ORD_SD.IsActive=1 AND ORD_SD.IsDeleted=0  where ( INV_GMD.IsDamaged=1 OR INV_GMD.HasDiscrepancy=1 OR INV_GMD.IsNonConfirmity=1  ) AND INV_GMD.IsDeleted=0 AND INV_GMD.TransactionDocID=" + InboundID + " AND INV_GMD.GoodsMovementTypeID=1 AND ORD_SD.SupplierInvoiceID=" + SupplierInvoiceID + "   and MMT_MM.MCode like '" + prefix + "%'"; 
            String POSql = "SELECT DISTINCT MM.MaterialMasterID, MM.MCode FROM INB_Inbound_ORD_SupplierInvoice ISI JOIN ORD_SupplierInvoiceDetails OSID ON OSID.SupplierInvoiceID = ISI.SupplierInvoiceID JOIN ORD_PODetails POD ON POD.PODetailsID = OSID.PODetailsID JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID = POD.MaterialMasterID WHERE ISI.SupplierInvoiceID = " + SupplierInvoiceID + " AND ISI.InboundID = " + InboundID + " AND MM.MCode LIKE '%" + prefix + "%'";
            IDataReader rsPO = DB.GetRS(POSql);
            while (rsPO.Read())
            {
                POList.Add(String.Format("{0},{1}", rsPO["MCode"], rsPO["MaterialMasterID"]));

            }
            rsPO.Close();

            return POList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPOLineNumbers(String prefix, String InboundID, String SupplierInvoiceID, String MMID, String POHeaderID)
        {
            List<String> POList = new List<String>();
            //String POSql = "select distinct ORD_POD.LineNumber, ORD_POD.MaterialMasterID,MCode  from ORD_PODetails ORD_POD  LEFT JOIN MMT_MaterialMaster MMT_M on MMT_M.MaterialMasterID=ORD_POD.MaterialMasterID and MMT_M.IsDeleted=0 and MMT_M.IsActive=1 and MMT_M.IsAproved=1 LEFT JOIN ORD_SupplierInvoiceDetails ORD_SUPID ON ORD_SUPID.PODetailsID=ORD_POD.PODetailsID AND ORD_SUPID.IsActive=1 AND ORD_SUPID.IsDeleted=0 LEFT JOIN INB_Inbound_ORD_SupplierInvoice INB_Sup on  INB_Sup.SupplierInvoiceID=ORD_SUPID.SupplierInvoiceID AND INB_Sup.POHeaderID=ORD_POD.POHeaderID and INB_Sup.IsActive=1 and INB_Sup.IsDeleted=0 where ORD_POD.IsDeleted=0 and ORD_POD.IsActive=1  and  INB_Sup.SupplierInvoiceID=" + SupplierInvoiceID + " AND  ORD_POD.POHeaderID="+POHeaderID+"  AND ORD_POD.MaterialMasterID=" + MMID + "  AND InboundID=" + InboundID + " and MCode like '" + prefix + "%'";
            String POSql = "select distinct ORD_POD.LineNumber, ORD_POD.MaterialMasterID,MCode  from ORD_PODetails ORD_POD  LEFT JOIN MMT_MaterialMaster MMT_M on MMT_M.MaterialMasterID=ORD_POD.MaterialMasterID and MMT_M.IsDeleted=0 and MMT_M.IsActive=1 and MMT_M.IsAproved=1 LEFT JOIN ORD_SupplierInvoiceDetails ORD_SUPID ON ORD_SUPID.PODetailsID=ORD_POD.PODetailsID AND ORD_SUPID.IsActive=1 AND ORD_SUPID.IsDeleted=0 LEFT JOIN INB_Inbound_ORD_SupplierInvoice INB_Sup on  INB_Sup.SupplierInvoiceID=ORD_SUPID.SupplierInvoiceID AND INB_Sup.POHeaderID=ORD_POD.POHeaderID and INB_Sup.IsActive=1 and INB_Sup.IsDeleted=0 where ORD_POD.IsDeleted=0 and ORD_POD.IsActive=1  and  INB_Sup.SupplierInvoiceID=" + SupplierInvoiceID + " AND  ORD_POD.POHeaderID=" + POHeaderID + "  AND ORD_POD.MaterialMasterID=" + MMID + "  AND InboundID=" + InboundID + "";
            IDataReader rsPO = DB.GetRS(POSql);
            while (rsPO.Read())
            {
                POList.Add(String.Format("{0},{1}", rsPO["LineNumber"], rsPO["MaterialMasterID"]));

            }
            rsPO.Close();

            return POList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadNonConfirmityCode(String prefix, String NonConfirmityTypeID)
        {
            List<String> RoutList = new List<String>();
            String RoutSql = " select NonConfirmityCodeID ,NonConfirmityCode,NonConfirmityName  from  MFG_NonConfirmityCode where IsActive=1 AND IsDeleted=0 AND NonConfirmityTypeID=" + NonConfirmityTypeID + " AND  NonConfirmityCode LIKE '" + prefix + "%'";
            IDataReader rsRout = DB.GetRS(RoutSql);
            while (rsRout.Read())
            {
                RoutList.Add(String.Format("{0},{1}", DB.RSField(rsRout, "NonConfirmityCode") + "-" + DB.RSField(rsRout, "NonConfirmityName"), DB.RSFieldInt(rsRout, "NonConfirmityCodeID").ToString()));

            }
            rsRout.Close();

            return RoutList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadNonConfirmityTypes(String prefix)
        {
            List<String> RoutList = new List<String>();
            String RoutSql = " select NonConfirmityTypeID ,NonConfirmityType  from  MFG_NonConfirmityType where IsActive=1 AND IsDeleted=0  AND  NonConfirmityType LIKE '" + prefix + "%'";
            IDataReader rsRout = DB.GetRS(RoutSql);
            while (rsRout.Read())
            {
                RoutList.Add(String.Format("{0},{1}", rsRout["NonConfirmityType"], rsRout["NonConfirmityTypeID"]));

            }
            rsRout.Close();

            return RoutList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadQualityParameters(String prefix)
        {
            List<String> QCList = new List<String>();
            String QCSql = " select QualityParameterID,ParameterName from  QCC_QualityParameters where IsActive=1 AND IsDeleted=0 AND ParameterName LIKE '" + prefix + "%'";
            IDataReader rsQC = DB.GetRS(QCSql);
            while (rsQC.Read())
            {
                QCList.Add(String.Format("{0},{1}", rsQC["ParameterName"], rsQC["QualityParameterID"]));

            }
            rsQC.Close();

            return QCList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadFileTypes(String prefix)
        {
            List<String> QCList = new List<String>();
            String QCSql = " select FileTypeID,FileType from MMT_FileType where IsActive=1 AND IsDeleted=0 AND FileType LIKE '" + prefix + "%'";
            IDataReader rsQC = DB.GetRS(QCSql);
            while (rsQC.Read())
            {
                QCList.Add(String.Format("{0},{1}", rsQC["FileType"], rsQC["FileTypeID"]));

            }
            rsQC.Close();

            return QCList.ToArray();
        }







        #endregion     -----------------   Developed By Naresh   -------------------------


        #region     -----------------   Developed By Prasad   -------------------------


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadBoMRefNoList(String prefix)
        {
            List<String> BoMRefNoList = new List<String>();
            String cMdBoMRefNo = "select top 10 BOMRefNumber,BOMHeaderID from MFG_BOMHeader where IsDeleted=0 and IsActive=1 and BOMRefNumber like '" + prefix + "%'";
            IDataReader rsBoMRefNoList = DB.GetRS(cMdBoMRefNo);
            while (rsBoMRefNoList.Read())
            {
                BoMRefNoList.Add(String.Format("{0},{1}", rsBoMRefNoList["BOMRefNumber"], rsBoMRefNoList["BOMHeaderID"]));

            }
            rsBoMRefNoList.Close();

            return BoMRefNoList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPONumbersforPOList(String prefix, String TenentID, String StatusID)
        {
            List<String> POList = new List<String>();
            String POSql = "select top 10 PONumber,POHeaderID from ORD_POHeader where  IsActive=1 and IsDeleted=0 and ( 0=" + StatusID + " or POStatusID=" + StatusID + ")  and  TenantID=" + TenentID + " and PONumber like '" + prefix + "%' order by PONumber";
            IDataReader rsPO = DB.GetRS(POSql);
            while (rsPO.Read())
            {
                POList.Add(String.Format("{0},{1}", rsPO["PONumber"], rsPO["POHeaderID"]));
            }
            rsPO.Close();

            return POList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadSOStatus(String prefix)
        {
            List<String> list = new List<string>();
            String sql = "select SOStatusID,StatusName from ORD_SOStatus where IsActive=1 and IsDeleted=0 and StatusName like'" + prefix + "%' order by StatusName";
            IDataReader reader = DB.GetRS(sql);
            while (reader.Read())
            {
                list.Add(String.Format("{0},{1}", reader["StatusName"], reader["SOStatusID"]));
            }
            reader.Close();
            return list.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMCodeForSaleOrder(string prefix, string TenantID)
        {
            List<string> MMList = new List<string>();
            //string MMSql = "select  Top 20 MCode+isnull( '` '+ OEMPartNo,'')  AS MCode,MaterialMasterID from MMT_MaterialMaster where TenantID=" + TenantID + " AND MTypeID IN (8,9,7) and IsDeleted=0 and IsActive=1 AND  MCode like '" + prefix + "%' order by MCode";
            string MMSql = "select  Top 20 MCode+isnull( '` '+ OEMPartNo,'')  AS MCode,MaterialMasterID from MMT_MaterialMaster where IsDeleted=0 and IsActive=1 AND  MCode like '" + prefix + "%' order by MCode";

            IDataReader rsMM = DB.GetRS(MMSql);

            while (rsMM.Read())
            {
                MMList.Add(string.Format("{0}~{1}", rsMM["MCode"], rsMM["MaterialMasterID"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadTenantMCodeForSaleOrder(string prefix, string TenantID)
        {
            List<string> MMList = new List<string>();
            string MMSql = "select  Top 20 MM.MCode+isnull( '` '+ MM.OEMPartNo,'')  AS MCode,MM.MaterialMasterID from MMT_MaterialMaster MM "
                                + " left join TPL_Tenant_MaterialMaster  TTM on MM.MaterialMasterID=TTM.MaterialMasterID and TTM.IsActive=1 and TTM.IsDeleted=0 "
                                + " where TTM.TenantID=" + TenantID + " and MM.IsDeleted=0 and MM.IsActive=1  AND  MCode like '" + prefix + "%' order by MCode";

            IDataReader rsMM = DB.GetRS(MMSql);

            while (rsMM.Read())
            {
                MMList.Add(string.Format("{0}~{1}", rsMM["MCode"], rsMM["MaterialMasterID"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadTenantMDescForSaleOrder(string prefix, string TenantID)
        {
            List<string> MMList = new List<string>();

            string MMSql = "SELECT DISTINCT TOP 100 MM.MDescription,MM.MCode,MM.MaterialMasterID FROM MMT_MaterialMaster MM" +
                " LEFT JOIN TPL_Tenant_MaterialMaster TTM ON TTM.MaterialMasterID = MM.MaterialMasterID AND TTM.IsActive = 1 AND TTM.IsDeleted = 0 AND MM.IsActive = 1 AND MM.IsDeleted = 0" +
                " WHERE TTM.TenantID = "+ TenantID + " AND (MM.MCode LIKE '%" + prefix + "%' OR MM.MDescription LIKE '%" + prefix + "%')";
            IDataReader rsMM = DB.GetRS(MMSql);

            while (rsMM.Read())
            {
                MMList.Add(string.Format("{0}~{1}~{2}", rsMM["MDescription"], rsMM["MaterialMasterID"], rsMM["MCode"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMCodeForSaleOrderWithOEM(string prefix, string TenantID, String SOheaderID)
        {
            List<string> MMList = new List<string>();
            string MMSql;
            if (SOheaderID == "0")
                //<!-------------Procedure Converting------------------------->
                // MMSql = "select Distinct Top 20 MCode  +   isnull( ' ` '+ OEMPartNo,'')  AS MCode ,MaterialMasterID from MMT_MaterialMaster where MTypeID IN (8,9) and IsDeleted=0 and IsActive=1 AND  ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";
                MMSql = "Exec [dbo].[USP_LoadMCodeWithOEM] @Prefix='" + prefix + "'";
            else
                //  MMSql = "select Distinct  Top 20 MCode  +   isnull( ' ` '+ OEMPartNo,'')  AS MCode ,MM.MaterialMasterID from MMT_MaterialMaster MM join ORD_SODetails SOD ON SOD.MaterialMasterID=MM.MaterialMasterID AND SOD.ISDELETED=0  where SOHeaderID=" + SOheaderID+" AND  ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";
                MMSql = "Exec [dbo].[USP_LoadMCodeForSOWithOEM] @Prefix='" + prefix + "',@SOHeaderID=" + SOheaderID + "";
            IDataReader rsMM = DB.GetRS(MMSql);

            while (rsMM.Read())
            {
                MMList.Add(string.Format("{0}~{1}", rsMM["MCode"], rsMM["MaterialMasterID"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMCodeForToolOrder(string prefix, string TenantID)
        {
            List<string> MMList = new List<string>();
            string MMSql = "select  Top 20 MCode,MaterialMasterID from MMT_MaterialMaster where IsDeleted=0 and IsActive=1 AND MTypeID=1011 AND MCode like '" + prefix + "%' order by MCode";

            IDataReader rsMM = DB.GetRS(MMSql);

            while (rsMM.Read())
            {
                MMList.Add(string.Format("{0},{1}", rsMM["MCode"], rsMM["MaterialMasterID"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadSOTypes(String prefix)
        {
            List<String> list = new List<string>();
            //<!----------------Procedure conversion------------->
            //  String sql = "select SOTypeID,SoType from ORD_SOType where IsDeleted=0 and ishidden=0 AND ISActive=1 and SoType like'" + prefix + "%' order by SoType";
            String sql = "Exec [dbo].[USP_MST_DropSoType] @prefix='" + prefix + "'";
            IDataReader reader = DB.GetRS(sql);
            while (reader.Read())
            {
                list.Add(String.Format("{0},{1}", reader["SOType"], reader["SOTypeID"]));
            }
            reader.Close();
            return list.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadCountries(String prefix)
        {
            List<String> list = new List<string>();
            //<!--------------Procedure Converting----------------->
            // String sql = "SELECT  CountryMasterID,CountryName FROM GEN_CountryMaster where IsActive=1 and IsDeleted=0 and CountryName like'" + prefix + "%'  order by CountryName";
            String sql = "Exec [dbo].[USP_LoadCountryDropDown] @Prefix='" + prefix + "'";
            IDataReader reader = DB.GetRS(sql);
            while (reader.Read())
            {
                list.Add(String.Format("{0},{1}", reader["CountryName"], reader["CountryMasterID"]));
            }
            reader.Close();
            return list.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPOStatus(String prefix)
        {
            List<String> list = new List<string>();
            String sql = "select POStatusID,StatusName from ORD_POStatus where IsActive=1 and IsDeleted=0 and StatusName like'" + prefix + "%' order by StatusName";
            IDataReader reader = DB.GetRS(sql);
            while (reader.Read())
            {
                list.Add(String.Format("{0},{1}", reader["StatusName"], reader["POStatusID"]));
            }
            reader.Close();
            return list.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPOTypes(String prefix)
        {
            List<String> list = new List<string>();
            String sql = "select POTypeID,POType from ORD_POType where IsDeleted=0 AND ISActive=1 and isnull(IsHidden,0)=0 and POType like'" + prefix + "%' order by POTypeID";
            IDataReader reader = DB.GetRS(sql);
            while (reader.Read())
            {
                list.Add(String.Format("{0},{1}", reader["POType"], reader["POTypeID"]));
            }
            reader.Close();
            return list.ToArray();

        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadUoMWithQty(String MaterialID)
        {
            List<String> list = new List<string>();

            //  String sql = "select gu.UoM,gu.UoMID,mmgu.UoMQty,mmgu.MaterialMaster_UoMID from GEN_UoM gu LEFT JOIN MMT_MaterialMaster_GEN_UoM mmgu on gu.UoMID=mmgu.UoMID where mmgu.UoMTypeID!=1 AND mmgu.IsDeleted=0 AND mmgu.MaterialMasterID=" + MaterialID;

            //   <!--- GetNCRefNoForReworkProcedureReport Conversion--------------->
            //String cmdGetUoMForMaterial = " SELECT MM_IUoM.MaterialMaster_UoMID,UOM,MM_IUoM.UoMQty FROM MMT_MaterialMaster_GEN_UoM MM_IUoM"
            //                  +" JOIN GEN_UoM UOM ON UOM.UoMID=MM_IUoM.UoMID "
            //                  +" JOIN MMT_MaterialMaster_GEN_UoM MM_MUoM on MM_IUoM.MaterialMasterID=MM_MUoM.MaterialMasterID and MM_MUoM.IsDeleted=0 and MM_MUoM.UoMTypeID=1 "
            //                 + " WHERE MM_IUoM.IsDeleted=0 AND (MM_IUoM.UoMTypeID!=2 OR MM_IUoM.UoMID!=MM_MUoM.UoMID OR MM_IUoM.UoMQty!=MM_MUoM.UoMQty) AND MM_IUoM.MaterialMasterID=" + MaterialID;

            string cmdGetUoMForMaterial = "[dbo].[USP_LoadUoMQtyPO] @MaterialID =" + MaterialID + "";
            IDataReader SuomReader = DB.GetRS(cmdGetUoMForMaterial);
            while (SuomReader.Read())
            {
                list.Add(String.Format("{0}/{1},{2}", SuomReader["UoM"], SuomReader["UoMQty"], SuomReader["MaterialMaster_UoMID"]));
            }

            SuomReader.Close();
            return list.ToArray();

        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadKitPlanner(String MaterialID, string TenantID)
        {
            List<String> list = new List<string>();
            //String sql = "select KitPlannerID from MMT_KitPlanner where IsDeleted=0 AND ParentMaterialMasterID=" + MaterialID + " AND TenantID=" + TenantID;

            string sql = "Exec USP_MST_KitPlannerDropData @MaterialMasterID=" + MaterialID + ",@TenantID=" + TenantID + "";
            IDataReader kitReader = DB.GetRS(sql);
            while (kitReader.Read())
            {
                list.Add(String.Format("{0},{1}", kitReader["KitCode"], kitReader["KitPlannerID"]));
            }
            kitReader.Close();
            return list.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String MaterialConfigurationService(String MaterialId, string TenantID)
        {
            List<String> list = new List<string>();
            StringBuilder ParameterIds = new StringBuilder();
            int position = 1;
            String values = "";
            String sql = "sp_GEN_GetAllMaterialStorageParameters @ParameterUsageTypeID=1,@TenantID=" + TenantID;
            IDataReader reader = DB.GetRS(sql);
            while (reader.Read())
            {
                if (position == 1)
                {
                    if (reader["ControlType"].ToString() == "DropDownList")
                        ParameterIds.Append("ddl" + reader["ParameterName"].ToString());
                    else
                        ParameterIds.Append("txt" + reader["ParameterName"].ToString());
                }
                else
                {
                    if (reader["ControlType"].ToString() == "DropDownList")
                        ParameterIds.Append(",ddl" + reader["ParameterName"].ToString());
                    else
                        ParameterIds.Append(",txt" + reader["ParameterName"].ToString());
                }
                position++;
            }
            values = ParameterIds.ToString() + "|";
            //list.Add(ParameterIds.ToString());
            ParameterIds = new StringBuilder();
            reader.Close();
            sql = "EXEC sp_ORD_GetMaterialStorageParameters @MaterialMasterID=" + MaterialId + ",@TenantID=" + TenantID;
            position = 1;
            IDataReader reader2 = DB.GetRS(sql);
            while (reader2.Read())
            {
                if (position == 1)
                {
                    if (reader2["ControlType"].ToString() == "DropDownList")
                        ParameterIds.Append("ddl" + reader2["ParameterName"].ToString());
                    else
                        ParameterIds.Append("txt" + reader2["ParameterName"].ToString());
                }
                else
                {
                    if (reader2["ControlType"].ToString() == "DropDownList")
                        ParameterIds.Append(",ddl" + reader2["ParameterName"].ToString());
                    else
                        ParameterIds.Append(",txt" + reader2["ParameterName"].ToString());
                }
                position++;
            }
            values += ParameterIds.ToString();
            //list.Add(ParameterIds.ToString());
            reader2.Close();
            return values;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String CustomerDetails(String CustomerID, String TenantID)
        {
            StringBuilder CustomerDetails = new StringBuilder();
            CustomerDetails.Append("<table>");
            if (CustomerID != "")
            {
                String sCmdCustmer = "Select CustomerName,Address1,Address2,City,State,GENC.CountryName,Zip,Phone1 from GEN_Customer gen LEFT JOIN GEN_CountryMaster GENC ON gen.CountryMasterID=GENC.CountryMasterID where CustomerID=" + CustomerID + "  and TenantID=" + TenantID;
                IDataReader rsCustomer = DB.GetRS(sCmdCustmer);

                if (rsCustomer.Read())
                {
                    CustomerDetails.Append("<tr> <td>CustomerName</td> <td>:</td> <td>" + DB.RSField(rsCustomer, "CustomerName") + "</td> </tr>");
                    CustomerDetails.Append("<tr> <td>Address1</td> <td>:</td> <td> " + DB.RSField(rsCustomer, "Address1") + "</td> </tr>");
                    CustomerDetails.Append("<tr><td>Address2</td><td>:</td><td>" + DB.RSField(rsCustomer, "Address1") + "</td>     </tr>");
                    CustomerDetails.Append("<tr><td>City</td><td>:</td><td>" + DB.RSField(rsCustomer, "Address2") + "</td>     </tr>");
                    CustomerDetails.Append("<tr><td>State</td><td>:</td><td>" + DB.RSField(rsCustomer, "City") + "</td>     </tr>");
                    CustomerDetails.Append("<tr><td>Country</td><td>:</td><td>" + DB.RSField(rsCustomer, "State") + "</td>     </tr>");
                    CustomerDetails.Append("<tr><td>Zip</td><td>:</td><td>" + DB.RSField(rsCustomer, "Zip") + "</td>     </tr>");
                    CustomerDetails.Append("<tr><td>Phone No</td><td>:</td><td>" + DB.RSField(rsCustomer, "Phone1") + "</td>     </tr>");
                }
                rsCustomer.Close();
                CustomerDetails.Append("</table>");
            }
            return CustomerDetails.ToString();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadSupplierCode(String SupplierCode, String TenantID)
        {

            String sCmdSupplierCode = "select top 20 SupplierID,SupplierCode from MMT_Supplier where SupplierCode like '" + SupplierCode + "%' order by SupplierCode  ";
            IDataReader rsSupplierCode = DB.GetRS(sCmdSupplierCode);

            List<String> SuppCodeList = new List<string>();
            while (rsSupplierCode.Read())
            {
                SuppCodeList.Add(String.Format("{0}", rsSupplierCode["SupplierCode"]));

            }
            rsSupplierCode.Close();
            return SuppCodeList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPONumber(String StoreRefNo)
        {

            String sCmdPOQuantity = "Select POHeaderID,PONumber from ORD_POHeader where POHeaderID in( select POHeaderID from INB_Inbound_ORD_SupplierInvoice where InboundID=( select InboundID from INB_Inbound where StoreRefNo=" + DB.SQuote(StoreRefNo) + "))";
            IDataReader rsPOQuantity = DB.GetRS(sCmdPOQuantity);

            List<String> PONumberList = new List<string>();
            while (rsPOQuantity.Read())
            {
                PONumberList.Add(String.Format("{0},{1}", rsPOQuantity["PONumber"], rsPOQuantity["POHeaderID"]));

            }
            rsPOQuantity.Close();
            return PONumberList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] getMCodeforstorerefno(string prefix, String StorerefNo)
        {
            List<string> mmList = new List<string>();

            //string mmSql = "select distinct mm.MCode + isnull( '` '+ OEMPartNo,'')  AS MCode ,mm.MaterialMasterID from  INB_Inbound inb join INB_Inbound_ORD_SupplierInvoice inb_inv on inb.InboundID=inb_inv.InboundID and inb_inv.IsDeleted=0 and inb_inv.IsActive=1 join ORD_SupplierInvoiceDetails inv on inv.SupplierInvoiceID=inb_inv.SupplierInvoiceID and inv.IsDeleted=0 and inv.IsActive=1 join ORD_PODetails pod on pod.PODetailsID =inv.PODetailsID and pod.IsDeleted=0 and pod.IsActive=1 join MMT_MaterialMaster mm on mm.MaterialMasterID=pod.MaterialMasterID where inb.StoreRefNo=" + DB.SQuote(StorerefNo) + "  and mm.MCode like '" + prefix + "%'";
            string mmSql = "Exec  [dbo].[USP_GetMCodeforstorerefno] @StoreRefNo=" + DB.SQuote(StorerefNo) + ",@prefix= '" + prefix + "'";


            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}", rsMCodeList["MCode"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] getMCodeforstorerefnoWithOEM(string prefix, String StorerefNo)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select distinct mm.MCode +   isnull( ' ` '+ mm.OEMPartNo,'')  AS MCode ,mm.MaterialMasterID from  INB_Inbound inb join INB_Inbound_ORD_SupplierInvoice inb_inv on inb.InboundID=inb_inv.InboundID and inb_inv.IsDeleted=0 and inb_inv.IsActive=1 join ORD_SupplierInvoiceDetails inv on inv.SupplierInvoiceID=inb_inv.SupplierInvoiceID and inv.IsDeleted=0 and inv.IsActive=1 join ORD_PODetails pod on pod.PODetailsID =inv.PODetailsID and pod.IsDeleted=0 and pod.IsActive=1 join MMT_MaterialMaster mm on mm.MaterialMasterID=pod.MaterialMasterID where inb.StoreRefNo=" + DB.SQuote(StorerefNo) + "    AND ( MM.MCode like '" + prefix + "%' OR  MM.OEMPartNo like '" + prefix + "%' ) order by MM.MCode";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}", rsMCodeList["MCode"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetMCodeBasedonSupplier(string prefix, String SuppleirID, string TenantID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select Top 20 MCode, MaterialMasterID from MMT_MaterialMaster where MTypeID in(7,1011) AND IsDeleted=0 and  IsActive=1  AND  MCode like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetMCodeBasedonSupplierWithOEM(string prefix, String SuppleirID, string TenantID, String POHeaderID)
        {
            List<string> mmList = new List<string>();
            string mmSql;
            if (POHeaderID == "0")
                //mmSql = "select Top 20 MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode , MaterialMasterID from MMT_MaterialMaster where TenantID=" + TenantID + " and MTypeID in(7,1011) AND IsDeleted=0 and  IsActive=1  AND  ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";
                mmSql = "select Top 20 MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode , MaterialMasterID from MMT_MaterialMaster where IsDeleted=0 and  IsActive=1  AND  ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";
            //mmSql = "select Top 20 MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode , mm.MaterialMasterID from MMT_MaterialMaster MM join MMT_MaterialMaster_Supplier MMS on mms.MaterialMasterID=mm.MaterialMasterID and mms.IsDeleted=0 where mms.SupplierID=" + SuppleirID + " and  mm.TenantID=" + TenantID + " AND mm.IsDeleted=0 and  mm.IsActive=1  AND  ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";
            //Requested by rt on 24/03/2015
            else
                mmSql = "select Top 20 * from (select distinct MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode , MM.MaterialMasterID from MMT_MaterialMaster MM join ORD_PODetails POD ON POD.MaterialMasterID=MM.MaterialMasterID AND POD.IsDeleted=0 where POHeaderID=" + POHeaderID + " AND  ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' )) as POMList order by MCode";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String CheckInvoice(String POHeaderID)
        {
            //<!-----------------Procedure Converting------------------->
            // String sCMdCheckInvoice = "SELECT TOP 1 SupplierInvoiceID FROM ORD_SupplierInvoice where IsDeleted=0 AND POHeaderID=" + POHeaderID;
            String sCMdCheckInvoice = "Exec [dbo].[USP_GetSupplierInvoiceID] @POHeaderID=" + POHeaderID + "";


            String check = "false";
            IDataReader drCheckInvoice = DB.GetRS(sCMdCheckInvoice);
            if (drCheckInvoice.Read())
            {
                check = "true";
            }

            return check;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadCustomPONumbers(String SOHeaderID)
        {
            List<String> InvList = new List<String>();
            //String CustomPOSql = "select CustomerPOID,CustPONumber from ORD_CustomerPO where IsDeleted=0 and SOHeaderID=" + SOHeaderID;
            String CustomPOSql = "Exec [dbo].[USP_GetCustomerPOdata] @SOHeaderID=" + SOHeaderID;
            IDataReader rsCustomPO = DB.GetRS(CustomPOSql);
            while (rsCustomPO.Read())
            {
                InvList.Add(String.Format("{0},{1}", rsCustomPO["CustPONumber"], rsCustomPO["CustomerPOID"]));

            }
            rsCustomPO.Close();

            return InvList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String CheckCustomPO(String SOHeaderID)
        {
            String sCMdCheckcustomPO = "select top 1 CustomerPOID from ORD_CustomerPO where IsDeleted=0 and SOHeaderID=" + SOHeaderID;
            String check = "false";
            IDataReader drCheckCustomPO = DB.GetRS(sCMdCheckcustomPO);
            if (drCheckCustomPO.Read())
            {
                check = "true";
            }

            return check;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadInvNumbers(String POHeaderID, String PODetailsID, String SupplierInvoiceDetailsID)
        {
            List<String> InvList = new List<String>();
            // String CustomPOSql = "select sup_inv.SupplierInvoiceID,sup_inv.InvoiceNumber  from ORD_SupplierInvoice sup_inv left join INB_Inbound_ORD_SupplierInvoice inb_inv on inb_inv.SupplierInvoiceID=sup_inv.SupplierInvoiceID and inb_inv.IsDeleted=0 and inb_inv.IsActive=1 left join INB_InboundTracking_Warehouse inbwar on inbwar.InboundID=inb_inv.InboundID and inbwar.IsDeleted=0 and inbwar.IsActive=1 where sup_inv.IsDeleted=0  and inbwar.InboundTracking_WarehouseID is null AND (sup_inv.SupplierInvoiceID=" + SupplierInvoiceDetailsID + " or sup_inv.SupplierInvoiceID NOT IN(SELECT SupplierInvoiceID FROM ORD_SupplierInvoiceDetails WHERE PODetailsID=" + PODetailsID + " and IsDeleted=0 and IsActive=1) ) and sup_inv.POHeaderID=" + POHeaderID;

            //String CustomPOSql = " select sup_inv.SupplierInvoiceID,sup_inv.InvoiceNumber  from ORD_SupplierInvoice sup_inv left  join INB_Inbound_ORD_SupplierInvoice inb_inv on inb_inv.SupplierInvoiceID = sup_inv.SupplierInvoiceID and inb_inv.IsDeleted = 0 and inb_inv.IsActive = 1 left join INB_InboundTracking_Warehouse inbwar on inbwar.InboundID = inb_inv.InboundID and inbwar.IsDeleted = 0 and inbwar.IsActive = 1  where sup_inv.IsDeleted = 0 and inbwar.InboundTracking_WarehouseID is null AND (sup_inv.SupplierInvoiceID = " + SupplierInvoiceDetailsID + " or 1 = (SELECT 1 FROM ORD_SupplierInvoiceDetails ID INNER JOIN ORD_PODetails POD ON POD.PODetailsID = ID.PODetailsID WHERE POD.PODetailsID = " + PODetailsID + " and ID.IsDeleted = 0 and ID.IsActive = 1 GROUP BY POD.POQuantity  HAVING POD.POQuantity > SUM(ID.InvoiceQuantity)) OR sup_inv.SupplierInvoiceID IN (SELECT DISTINCT SI.SupplierInvoiceID FROM ORD_SupplierInvoice SI LEFT join ORD_SupplierInvoiceDetails ID   ON SI.SupplierInvoiceID = ID.SupplierInvoiceID WHERE SI.POHeaderID= " + POHeaderID+ " and ID.SupplierInvoiceID IS NULL)) and sup_inv.POHeaderID = " + POHeaderID;
            //  String CustomPOSql = "  select sup_inv.SupplierInvoiceID,sup_inv.InvoiceNumber from ORD_SupplierInvoice sup_inv left  join INB_Inbound_ORD_SupplierInvoice inb_inv on inb_inv.SupplierInvoiceID = sup_inv.SupplierInvoiceID and inb_inv.IsDeleted = 0 and inb_inv.IsActive = 1 left join INB_InboundTracking_Warehouse inbwar on inbwar.InboundID = inb_inv.InboundID and inbwar.IsDeleted = 0 and inbwar.IsActive = 1  where sup_inv.IsDeleted = 0 and inbwar.InboundTracking_WarehouseID is null AND (sup_inv.SupplierInvoiceID = " + SupplierInvoiceDetailsID + " or 1 = (SELECT 1 FROM ORD_SupplierInvoiceDetails ID INNER JOIN ORD_PODetails POD ON POD.PODetailsID = ID.PODetailsID WHERE POD.PODetailsID = " + PODetailsID + " and ID.IsDeleted = 0 and ID.IsActive = 1 GROUP BY POD.POQuantity  HAVING POD.POQuantity > SUM(ID.InvoiceQuantity)) OR sup_inv.SupplierInvoiceID IN(SELECT DISTINCT SI.SupplierInvoiceID FROM ORD_SupplierInvoice SI LEFT join ORD_SupplierInvoiceDetails ID   ON SI.SupplierInvoiceID = ID.SupplierInvoiceID AND ID.IsActive = 1 AND ID.IsDeleted = 0 AND SI.IsActive = 1 AND SI.IsDeleted = 0 WHERE SI.POHeaderID= " + POHeaderID+" and ID.SupplierInvoiceID IS NULL )) and sup_inv.POHeaderID = " + POHeaderID;

            //Procedure conversion
            //  String CustomPOSql = "select sup_inv.SupplierInvoiceID,sup_inv.InvoiceNumber from ORD_SupplierInvoice sup_inv left join INB_Inbound_ORD_SupplierInvoice inb_inv on inb_inv.SupplierInvoiceID  = sup_inv.SupplierInvoiceID and inb_inv.IsDeleted = 0 and inb_inv.IsActive = 1 left join INB_InboundTracking_Warehouse inbwar on inbwar.InboundID = inb_inv.InboundID and inbwar.IsDeleted = 0 and inbwar.IsActive = 1 where sup_inv.IsDeleted = 0 and inbwar.InboundTracking_WarehouseID is null AND (sup_inv.SupplierInvoiceID = " + SupplierInvoiceDetailsID + " or 1 = (SELECT 1 FROM ORD_SupplierInvoiceDetails ID INNER JOIN ORD_PODetails POD ON POD.PODetailsID = ID.PODetailsID WHERE POD.PODetailsID = " + PODetailsID+" and ID.IsDeleted = 0 and ID.IsActive = 1 GROUP BY POD.POQuantity  HAVING POD.POQuantity > SUM(ID.InvoiceQuantity)) OR sup_inv.SupplierInvoiceID IN(SELECT DISTINCT SI.SupplierInvoiceID FROM ORD_SupplierInvoice SI LEFT join ORD_SupplierInvoiceDetails ID   ON SI.SupplierInvoiceID = ID.SupplierInvoiceID AND ID.IsActive = 1 AND ID.IsDeleted = 0 AND SI.IsActive = 1 AND SI.IsDeleted = 0 WHERE SI.POHeaderID= "+ POHeaderID+ " and ID.SupplierInvoiceID IS NULL ) OR 1 = (SELECT 1 WHERE(SELECT SUM(POQuantity) FROM ORD_PODetails WHERE isactive=1 and isdeleted=0 and POHeaderID = " + POHeaderID+ ") > (SELECT SUM(InvoiceQuantity) FROM ORD_SupplierInvoiceDetails ID INNER JOIN ORD_SupplierInvoice SI ON SI.SupplierInvoiceID = ID.SupplierInvoiceID and ID.isactive=1 and ID.isdeleted=0 and SI.isactive=1 and SI.isdeleted=0 WHERE  POHeaderID = " + POHeaderID+ "))) and sup_inv.isactive=1 and sup_inv.isdeleted=0 and sup_inv.POHeaderID = " + POHeaderID ;
            String CustomPOSql = "Exec [dbo].[USP_LoadInvNumbersSI]  @SupplierInvoiceID=" + SupplierInvoiceDetailsID + ", @POHeaderID= " + POHeaderID + ", @PODetailsID= " + PODetailsID + "";
            IDataReader rsInvNum = DB.GetRS(CustomPOSql);
            while (rsInvNum.Read())
            {
                InvList.Add(String.Format("{0},{1}", rsInvNum["InvoiceNumber"], rsInvNum["SupplierInvoiceID"]));
            }
            rsInvNum.Close();
            return InvList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String MaterialConfigurationServiceForActiveStock(String MaterialId, string TenantID)
        {
            List<String> list = new List<string>();
            StringBuilder ParameterIds = new StringBuilder();
            int position = 1;
            String values = "";
            String sql = "sp_GEN_GetAllMaterialStorageParameters @ParameterUsageTypeID=1,@TenantID=" + TenantID;
            IDataReader reader = DB.GetRS(sql);
            while (reader.Read())
            {
                if (position == 1)
                {

                    ParameterIds.Append("" + reader["ParameterName"].ToString());
                }
                else
                {

                    ParameterIds.Append("," + reader["ParameterName"].ToString());
                }
                position++;
            }
            values = ParameterIds.ToString() + "|";
            //list.Add(ParameterIds.ToString());
            ParameterIds = new StringBuilder();
            reader.Close();
            sql = "EXEC [sp_ORD_GetMaterialStorageParameters] @ParameterUsageTypeID=1, @MaterialMasterID=" + MaterialId + ", @TenantID=" + TenantID;
            position = 1;
            IDataReader reader2 = DB.GetRS(sql);
            while (reader2.Read())
            {
                if (position == 1)
                {

                    ParameterIds.Append("" + reader2["ParameterName"].ToString());
                }
                else
                {

                    ParameterIds.Append("," + reader2["ParameterName"].ToString());
                }
                position++;
            }
            values += ParameterIds.ToString();
            //list.Add(ParameterIds.ToString());
            reader2.Close();
            return values;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPROoperation(String prefix)
        {
            List<String> operationsList = new List<String>();
            String cmdoperations = "select OperationID,OperationCode from MFG_Operation where IsActive=1 and IsDeleted=0 and OperationCode like '" + prefix + "%'";
            IDataReader rsOperations = DB.GetRS(cmdoperations);
            while (rsOperations.Read())
            {
                operationsList.Add(String.Format("{0},{1}", rsOperations["OperationCode"], rsOperations["OperationID"]));

            }
            rsOperations.Close();

            return operationsList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPROStatus(String prefix)
        {
            List<String> PROStatusList = new List<String>();
            String cMdPROStatus = "select ProductionOrderStatus,ProductionOrderStatusID from MFG_ProductionOrderStatus where IsActive=1 and IsDeleted=0 and ProductionOrderStatus like '" + prefix + "%'";
            IDataReader rsOperations = DB.GetRS(cMdPROStatus);
            while (rsOperations.Read())
            {
                PROStatusList.Add(String.Format("{0},{1}", rsOperations["ProductionOrderStatus"], rsOperations["ProductionOrderStatusID"]));

            }
            rsOperations.Close();

            return PROStatusList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPRONumber(String prefix)
        {
            List<String> PRONumberList = new List<String>();

            String cMdPRONumber = "select distinct top 10 MCode+'-'+MMV.Revision JobOrderRefNo,PROH.ProductionOrderHeaderID from MFG_ProductionOrderHeader PROH JOIN MMT_MaterialMaster_Revision MMV ON MMV.MaterialMasterRevisionID=PROH.MaterialMasterRevisionID AND MMV.IsActive=1 AND MMV.IsDeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMV.MaterialMasterID where PROH.IsDeleted=0 and PROH.IsActive=1 and MCode+'-'+MMV.Revision like  '" + prefix + "%'";
            IDataReader rsPRONumberList = DB.GetRS(cMdPRONumber);
            while (rsPRONumberList.Read())
            {
                PRONumberList.Add(String.Format("{0},{1}", rsPRONumberList["JobOrderRefNo"], rsPRONumberList["ProductionOrderHeaderID"]));

            }
            rsPRONumberList.Close();

            return PRONumberList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPRONumberforReturns(String KitCode)
        {
            List<String> PRONumberList = new List<String>();
            String cMdPRONumber = "select distinct top 10 MCode+'-'+MMV.Revision+' '+LEFT(RDT.RoutingDocumentType,1) as PRORefNo,PROH.ProductionOrderHeaderID from MFG_ProductionOrderHeader PROH JOIN MFG_SOPO_ProductionOrder SOPO ON SOPO.ProductionOrderHeaderID=PROH.ProductionOrderHeaderID AND SOPO.SOPOTypeID=2 AND SOPO.IsDeleted=0 JOIN MMT_MaterialMaster_Revision MMV ON MMV.MaterialMasterRevisionID=PROH.MaterialMasterRevisionID AND MMV.IsActive=1 AND MMV.IsDeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMV.MaterialMasterID JOIN MFG_RoutingHeader_Revision ROUV ON ROUV.RoutingHeaderRevisionID=PROH.RoutingHeaderRevisionID AND ROUV.IsDeleted=0 JOIN MFG_RoutingDocumentType RDT ON RDT.RoutingDocumentTypeID=ROUV.RoutingDocumentTypeID where PROH.IsDeleted=0 and PROH.IsActive=1 AND KitCode='" + KitCode + "'";
            IDataReader rsPRONumberList = DB.GetRS(cMdPRONumber);
            while (rsPRONumberList.Read())
            {
                PRONumberList.Add(String.Format("{0},{1}", rsPRONumberList["PRORefNo"], rsPRONumberList["ProductionOrderHeaderID"]));

            }
            rsPRONumberList.Close();

            return PRONumberList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadmfgLocation(String prefix)
        {
            List<String> mfgLocationList = new List<String>();
            String cMdmfgLocation = "select top 10 ProductionLocation,ProductionLocationID from MFG_ProductionLocation  where IsDeleted=0 and ProductionLocation like '" + prefix + "%'";
            IDataReader rsmfgLocationList = DB.GetRS(cMdmfgLocation);
            while (rsmfgLocationList.Read())
            {
                mfgLocationList.Add(String.Format("{0},{1}", rsmfgLocationList["ProductionLocation"], rsmfgLocationList["ProductionLocationID"]));

            }
            rsmfgLocationList.Close();

            return mfgLocationList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadMaterialproviderSupplerList(String prefix, String TenantID)
        {
            List<String> SupplerList = new List<String>();
            String cMdSuppler = "select top 10 SupplierName,SupplierID from MMT_Supplier where SupplierTypeID=2 and IsDeleted=0 and IsActive=1 and SupplierName like '" + prefix + "%'";
            IDataReader rsSupplerList = DB.GetRS(cMdSuppler);
            while (rsSupplerList.Read())
            {
                SupplerList.Add(String.Format("{0},{1}", rsSupplerList["SupplierName"], rsSupplerList["SupplierID"]));

            }
            rsSupplerList.Close();

            return SupplerList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadWorkCenterGroupList(String prefix, String RoutingHeaderID)
        {
            List<String> WorkCenterList = new List<String>();
            String cMdWorkCenter = "select top 10 WorkCenterGroup,wcg.WorkCenterGroupID from MFG_WorkCenterGroup wcg join MFG_RoutingDetails roud on roud.WorkCenterGroupID=wcg.WorkCenterGroupID and wcg.IsActive=1 and wcg.IsDeleted=0  where wcg.IsActive=1 and wcg.IsDeleted=0 and (0=" + RoutingHeaderID + " or roud.RoutingHeaderID=" + RoutingHeaderID + ") and WorkCenterGroup like '" + prefix + "%'";
            IDataReader rsWorkCenterList = DB.GetRS(cMdWorkCenter);
            while (rsWorkCenterList.Read())
            {
                WorkCenterList.Add(String.Format("{0},{1}", rsWorkCenterList["WorkCenterGroup"], rsWorkCenterList["WorkCenterGroupID"]));

            }
            rsWorkCenterList.Close();

            return WorkCenterList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadBoMTypeList(String prefix)
        {
            List<String> BoMTypeList = new List<String>();
            String cMdBoMType = "select top 10 BOMType,BOMTypeID from MFG_BOMType where IsDeleted=0 and IsActive=1 and BOMType like '" + prefix + "%'";
            IDataReader rsBoMTypeList = DB.GetRS(cMdBoMType);
            while (rsBoMTypeList.Read())
            {
                BoMTypeList.Add(String.Format("{0},{1}", rsBoMTypeList["BOMType"], rsBoMTypeList["BOMTypeID"]));

            }
            rsBoMTypeList.Close();

            return BoMTypeList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadRoutingHeaderList(String prefix)
        {
            List<String> RoutingHeaderList = new List<String>();
            String cMdRoutingHeader = "select top 10 RoutingRefNo,RoutingHeaderID from MFG_RoutingHeader where IsDeleted=0 and IsActive=1 and RoutingRefNo like '" + prefix + "%'";
            IDataReader rsRoutingHeaderList = DB.GetRS(cMdRoutingHeader);
            while (rsRoutingHeaderList.Read())
            {
                RoutingHeaderList.Add(String.Format("{0},{1}", rsRoutingHeaderList["RoutingRefNo"], rsRoutingHeaderList["RoutingHeaderID"]));

            }
            rsRoutingHeaderList.Close();

            return RoutingHeaderList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadRoutingVersionRefNoList(String prefix, String MaterialMasterRevisionID)
        {
            List<String> RoutingVersionRefNoList = new List<String>();
            String cMdRoutingVersionRefNo = "select ROUR.RoutingHeaderRevisionID,MM.MCode+'-'+MMV.Revision+'/'+ROUR.Revision+' '+LEFT(RoutingDocumentType,1)  as RoutingHeaderRevision FROM MFG_RoutingHeader ROUH join  MFG_RoutingHeader_Revision ROUR ON ROUH.RoutingHeaderID=ROUR.RoutingHeaderID AND ROUR.IsDeleted=0 AND ROUR.IsActive=1 AND CONVERT(NVARCHAR,GETDATE(),112)>=CONVERT(NVARCHAR,ROUR.EffectiveDate,112) join MMT_MaterialMaster_Revision MMV on MMV.MaterialMasterRevisionID=ROUR.MaterialMasterRevisionID and MMV.IsActive=1 and MMV.IsDeleted=0 join MMT_MaterialMaster MM ON MM.MaterialMasterID=MMV.MaterialMasterID join MFG_RoutingDocumentType RDT ON RDT.RoutingDocumentTypeID=ROUR.RoutingDocumentTypeID where ROUH.IsDeleted=0 and dbo.CheckRoutingQuantitywithBoM(ROUR.RoutingHeaderID)=1 AND MMV.MaterialMasterRevisionID=" + MaterialMasterRevisionID + " and mm.MCode like '" + prefix + "%'";
            IDataReader rsRoutingVersionRefNoList = DB.GetRS(cMdRoutingVersionRefNo);
            while (rsRoutingVersionRefNoList.Read())
            {
                RoutingVersionRefNoList.Add(String.Format("{0},{1}", rsRoutingVersionRefNoList["RoutingHeaderRevision"], rsRoutingVersionRefNoList["RoutingHeaderRevisionID"]));

            }
            rsRoutingVersionRefNoList.Close();

            return RoutingVersionRefNoList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadIORefNoList(String prefix)
        {
            List<String> IORefNoList = new List<String>();
            //String cMdIORefNo = "SELECT TOP 10 IORefNo FROM (select DISTINCT IORefNo from MFG_InternalOrderHeader where IsDeleted=0 and IsActive=1 and IORefNo like'" + prefix + "%') AS INTER";
            String cMdIORefNo = "SELECT TOP 10 IORefNo FROM (select DISTINCT IORefNo from MFG_InternalOrderHeader where IsDeleted=0 and IsActive=1 and IORefNo like'" + prefix + "%') AS INTER";
            IDataReader rsIORefNoList = DB.GetRS(cMdIORefNo);
            while (rsIORefNoList.Read())
            {
                IORefNoList.Add(String.Format("{0},{1}", rsIORefNoList["IORefNo"], rsIORefNoList["IORefNo"]));

            }
            rsIORefNoList.Close();

            return IORefNoList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetNonBoMMCode(string prefix, string TenantID, String BoMID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select Top 20 mm.MCode, mm.MaterialMasterID from MMT_MaterialMaster mm left join  MFG_BOMHeader bom on bom.MaterialMasterID=mm.MaterialMasterID and bom.IsDeleted=0 and bom.IsActive=1 where mm.IsDeleted=0 and mm.IsActive=1 and (bom.BOMHeaderID=" + BoMID + " or bom.BOMHeaderID is null) and mm.MCode like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadRoutingRefNoList(String prefix)
        {
            List<String> RoutingRefNoList = new List<String>();
            String cMdRoutingRefNo = "select top 10 RoutingRefNo,RoutingHeaderID from MFG_RoutingHeader where IsDeleted=0 and IsActive=1 and RoutingRefNo like '" + prefix + "%'";
            IDataReader rsRoutingRefNoList = DB.GetRS(cMdRoutingRefNo);
            while (rsRoutingRefNoList.Read())
            {
                RoutingRefNoList.Add(String.Format("{0},{1}", rsRoutingRefNoList["RoutingRefNo"], rsRoutingRefNoList["RoutingHeaderID"]));

            }
            rsRoutingRefNoList.Close();

            return RoutingRefNoList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadSONumbersForPROHeader(String ProductionOrderHeaderID, String MaterialMasterRevision, String KitCode, String TenantID)
        {
            List<String> SOList = new List<String>();
            String SOSql = "dbo.SP_MFG_CheckSOforJobOrders @KitCode='" + KitCode + "',@MaterialMasterRevisionID=" + MaterialMasterRevision + ",@TenantID=" + TenantID + ",@ProductionOrderHeaderID=" + ProductionOrderHeaderID;
            // Modified by Naresh...

            //String SOSql = "select SOH.SONumber,SOD.SOHeaderID from ORD_SOHeader SOH JOIN ORD_SODetails SOD ON SOD.SOHeaderID=SOH.SOHeaderID AND SOD.IsDeleted=0 AND SOD.IsActive=1 JOIN MMT_MaterialMaster_Revision MMV ON MMV.MaterialMasterID=SOD.MaterialMasterID AND MMV.IsDeleted=0 AND MMV.IsActive=1 LEFT JOIN MFG_ProductionOrderHeader POH ON POH.MaterialMasterRevisionID=MMV.MaterialMasterRevisionID  AND KitCode!='" + KitCode + "' AND POH.IsDeleted=0 AND POH.IsActive=1 where MMV.MaterialMasterRevisionID=" + MaterialMasterRevision + " and SOH.TenantID=" + TenantID + " and SOH.IsDeleted=0 and SOH.SONumber like '" + prefix + "%' GROUP BY SOH.SONumber,SOD.SOHeaderID,sod.SOQuantity order by SONumber";

            IDataReader rsSO = DB.GetRS(SOSql);
            while (rsSO.Read())
            {
                SOList.Add(String.Format("{0},{1}", rsSO["SONumber"], rsSO["SOHeaderID"]));

            }
            rsSO.Close();

            return SOList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadUoMForPRO(String RoutingHeaderVersionID)
        {
            List<String> UoMList = new List<String>();
            String cmdUoMList = "select UOM.UoM,UOM.UoMID from GEN_UoM  UOM JOIN MMT_MaterialMaster_GEN_UoM MMUOM ON MMUOM.UoMID=UOM.UoMID AND MMUOM.IsDeleted=0 AND MMUOM.IsActive=1 AND MMUOM.UoMTypeID!=1 JOIN MMT_MaterialMaster_Revision MMV ON MMV.MaterialMasterID=MMUOM.MaterialMasterID AND MMV.IsActive=1 AND MMV.IsDeleted=0 join MFG_RoutingHeader_Revision ROUV ON ROUV.MaterialMasterRevisionID=MMV.MaterialMasterRevisionID AND ROUV.IsDeleted=0 AND ROUV.IsActive=1 WHERE UOM.IsDeleted=0 AND UOM.IsActive=1 AND UOM.IsDeleted=0 AND ROUV.RoutingHeaderRevisionID=" + RoutingHeaderVersionID;
            IDataReader rsUoMList = DB.GetRS(cmdUoMList);
            while (rsUoMList.Read())
            {
                UoMList.Add(String.Format("{0},{1}", rsUoMList["UoM"], rsUoMList["UoMID"]));

            }
            rsUoMList.Close();

            return UoMList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadUoMForPRODetails(String BoMHeaderID)
        {
            List<String> UoMList = new List<String>();
            String cmdUoMList = "select UOM.UoM,MaterialMaster_UoMID,UoMQty from GEN_UoM  UOM JOIN MMT_MaterialMaster_GEN_UoM MMUOM ON MMUOM.UoMID=UOM.UoMID AND MMUOM.IsDeleted=0 AND MMUOM.IsActive=1 join MFG_BOMHeader BOM ON BOM.MaterialMasterID=MMUOM.MaterialMasterID AND MMUOM.IsActive=1 AND MMUOM.IsDeleted=0 WHERE UOM.IsDeleted=0 AND UOM.IsActive=1 AND BOMHeaderID=" + BoMHeaderID;
            IDataReader rsUoMList = DB.GetRS(cmdUoMList);
            while (rsUoMList.Read())
            {
                UoMList.Add(String.Format("{0}/{1},{2}", rsUoMList["UoM"], rsUoMList["UoMQty"], rsUoMList["MaterialMaster_UoMID"]));

            }
            rsUoMList.Close();

            return UoMList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPRORefNo(String KitCode)
        {
            List<String> PRORefNoList = new List<String>();
            String cmdPRORefNoList = "select top 10 MM.MCode+'-'+MMR.Revision+' '+LEFT(RoutingDocumentType,1) AS  PRORefNo,ProductionOrderHeaderID from MFG_ProductionOrderHeader PROH JOIN MMT_MaterialMaster_Revision MMR ON MMR.MaterialMasterRevisionID=PROH.MaterialMasterRevisionID AND MMR.IsActive=1 AND MMR.IsDeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMR.MaterialMasterID JOIN MFG_RoutingHeader_Revision ROUV ON ROUV.RoutingHeaderRevisionID=PROH.RoutingHeaderRevisionID AND ROUV.IsDeleted=0 JOIN MFG_RoutingDocumentType RDT ON RDT.RoutingDocumentTypeID=ROUV.RoutingDocumentTypeID where PROH.IsDeleted=0 and PROH.IsActive=1 and proh.KitCode='" + KitCode + "'";
            IDataReader rsPRORefNoList = DB.GetRS(cmdPRORefNoList);
            while (rsPRORefNoList.Read())
            {
                PRORefNoList.Add(String.Format("{0},{1}", rsPRORefNoList["PRORefNo"], rsPRORefNoList["ProductionOrderHeaderID"]));

            }
            rsPRORefNoList.Close();

            return PRORefNoList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadWorkCenter(String prefix)
        {
            List<String> WorkCenterList = new List<String>();
            String cmdWorkCenterList = "select WorkCenter,WorkCenterID from MFG_WorkCenter where IsActive=1 and IsDeleted=0 and WorkCenter like '" + prefix + "%'";
            IDataReader rsWorkCenterList = DB.GetRS(cmdWorkCenterList);
            while (rsWorkCenterList.Read())
            {
                WorkCenterList.Add(String.Format("{0},{1}", rsWorkCenterList["WorkCenter"], rsWorkCenterList["WorkCenterID"]));

            }
            rsWorkCenterList.Close();

            return WorkCenterList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadReasonForInternalOrderRequest(String prefix)
        {
            List<String> ReasonForInternalOrderRequestList = new List<String>();
            String cmdReasonForInternalOrderRequestList = "select ReasonForInternalOrderRequest,ReasonForInternalOrderRequestID from MFG_ReasonForInternalOrderRequest where IsActive=1 and IsDeleted=0 and ReasonForInternalOrderRequest like '" + prefix + "%'";
            IDataReader rsReasonForInternalOrderRequestList = DB.GetRS(cmdReasonForInternalOrderRequestList);
            while (rsReasonForInternalOrderRequestList.Read())
            {
                ReasonForInternalOrderRequestList.Add(String.Format("{0},{1}", rsReasonForInternalOrderRequestList["ReasonForInternalOrderRequest"], rsReasonForInternalOrderRequestList["ReasonForInternalOrderRequestID"]));

            }
            rsReasonForInternalOrderRequestList.Close();

            return ReasonForInternalOrderRequestList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadWorkCenterForProductionOrder(String prefix, String ProductionOrderHeaderID)
        {
            List<String> WorkCenterList = new List<String>();
            String cmdWorkCenterList = "SELECT DISTINCT TOP 10 WorkCenter,WC.WorkCenterID FROM MFG_WorkCenter WC JOIN MFG_ProductionOrder_WorkCenter PROHW ON PROHW.WorkCenterID=WC.WorkCenterID WHERE WC.IsDeleted=0 AND WC.IsActive=1 AND ProductionOrderHeaderID =" + ProductionOrderHeaderID + " AND  WC.WorkCenter LIKE '" + prefix + "%'";
            IDataReader rsWorkCenterList = DB.GetRS(cmdWorkCenterList);
            while (rsWorkCenterList.Read())
            {
                WorkCenterList.Add(String.Format("{0},{1}", rsWorkCenterList["WorkCenter"], rsWorkCenterList["WorkCenterID"]));

            }
            rsWorkCenterList.Close();

            return WorkCenterList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetMCodeForworkCenter(string prefix, String ProductionOrderHeaderID, String WorkCenterID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select top 10 mm.MCode,mm.MaterialMasterID from MFG_WorkCenter wc join MFG_WorkCenterGroup wcg on wcg.WorkCenterGroupID=wc.WorkCenterGroupID and wcg.IsDeleted=0 and wcg.IsActive=1 join MFG_BOMDetails BOMD ON BOMD.WorkCenterGroupID=WCG.WorkCenterGroupID AND BOMD.IsActive=1 AND BOMD.IsDeleted=0 join MFG_ProductionOrderDetails prod on prod.BOMHeaderID=BOMD.BOMHeaderID AND prod.IsDeleted=0 AND PROD.IsActive=1 join MMT_MaterialMaster mm on mm.MaterialMasterID=BOMD.BOMMaterialMasterID and mm.IsDeleted=0 and mm.IsActive=1 where wc.IsActive=1 and wc.IsDeleted=0 and prod.ProductionOrderHeaderID=" + ProductionOrderHeaderID + " AND WorkCenterID=" + WorkCenterID + " and mm.MCode like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadUoMForMaterial(String MaterialMasterID)
        {
            List<String> UoMList = new List<String>();
            String cmdUoMList = "select UoM,uom.UoMID from GEN_UoM uom join MMT_MaterialMaster_GEN_UoM mmuom on mmuom.UoMID=uom.UoMID and mmuom.IsDeleted=0 and mmuom.IsActive=1 where uom.IsDeleted=0 and uom.IsActive=1 and MaterialMasterID=" + MaterialMasterID;
            IDataReader rsUoMList = DB.GetRS(cmdUoMList);
            while (rsUoMList.Read())
            {
                UoMList.Add(String.Format("{0},{1}", rsUoMList["UoM"], rsUoMList["UoMID"]));

            }
            rsUoMList.Close();

            return UoMList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadReturnOBDNumbers(String prefix)
        {
            List<String> OBDNumberList = new List<String>();
            String cmdOBDNumberList = "select OutboundID,OBDNumber from OBD_Outbound where DeliveryStatusID>=4 and DocumentTypeID<6 and IsDeleted=0 and IsActive=1 and OBDNumber like '" + prefix + "%'";
            IDataReader rsOBDNumberList = DB.GetRS(cmdOBDNumberList);
            while (rsOBDNumberList.Read())
            {
                OBDNumberList.Add(String.Format("{0},{1}", rsOBDNumberList["OBDNumber"], rsOBDNumberList["OutboundID"]));

            }
            rsOBDNumberList.Close();

            return OBDNumberList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadReturnOBDNumbersTenant(String prefix, String TenantID)
        {
            List<String> OBDNumberList = new List<String>();
            String cmdOBDNumberList = "select OutboundID,OBDNumber from OBD_Outbound where DeliveryStatusID IN (4,6,7) and DocumentTypeID not in (11, 12) and IsDeleted=0 and IsActive=1 and TenantID=" + TenantID + " and CustomerID not in (-2,-1) and OBDNumber like '" + prefix + "%'";
            IDataReader rsOBDNumberList = DB.GetRS(cmdOBDNumberList);
            while (rsOBDNumberList.Read())
            {
                OBDNumberList.Add(String.Format("{0},{1}", rsOBDNumberList["OBDNumber"], rsOBDNumberList["OutboundID"]));

            }
            rsOBDNumberList.Close();

            return OBDNumberList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadReturnInvoices(String prefix, String OutboundID)
        {
            List<String> invoiceList = new List<String>();
           // String cmdInvoiceList = "select DISTINCT CustomerPOID,InvoiceNo from  OBD_Outbound_ORD_CustomerPO OBDCUS join INV_PO_GoodsOutLink GOUT on GOUT.SODetailsID = OBDCUS.SODetailsID WHERE OBDCUS.IsActive=1 and OBDCUS.IsDeleted=0 AND OutboundID=" + OutboundID + " and InvoiceNo like '" + prefix + "%'";
            String cmdInvoiceList = "select DISTINCT CustomerPOID,InvoiceNo from  OBD_Outbound_ORD_CustomerPO OBDCUS WHERE OBDCUS.IsActive=1 and OBDCUS.IsDeleted=0 AND OutboundID=" + OutboundID + " and InvoiceNo like '" + prefix + "%'";

            IDataReader rsInvoiceList = DB.GetRS(cmdInvoiceList);
            while (rsInvoiceList.Read())
            {
                invoiceList.Add(String.Format("{0},{1}", rsInvoiceList["CustomerPOID"], rsInvoiceList["InvoiceNo"]));

            }
            rsInvoiceList.Close();

            return invoiceList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadReturnStoreNumber(String OutboundID)
        {
            List<String> StoreNumberList = new List<String>();
            String cmdStoreNumberList = "select wh.WarehouseID,wh.WHCode from OBD_RefWarehouse_Details rwh join GEN_Warehouse wh on wh.WarehouseID=rwh.WarehouseID and wh.IsDeleted=0 and wh.IsActive=1  where OutboundID=" + OutboundID + " and rwh.IsActive=1 and rwh.IsDeleted=0";
            //String cmdStoreNumberList = "select WHCode,WH.WarehouseID from GEN_Warehouse WH WHERE wh.IsDeleted=0 and IsActive=1";
            IDataReader rsOBDNumberList = DB.GetRS(cmdStoreNumberList);
            while (rsOBDNumberList.Read())
            {
                StoreNumberList.Add(String.Format("{0},{1}", rsOBDNumberList["WHCode"], rsOBDNumberList["WarehouseID"]));

            }
            rsOBDNumberList.Close();

            return StoreNumberList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetTolarenceforQc(String MaterialMasterID)
        {
            List<String> TolarencesList = new List<String>();
            String cmdQcTolarance = "SELECT MaxTolerance,MinTolerance,ParameterName FROM MMT_MaterialMaster_QualityParameters MM_QCP JOIN QCC_QualityParameters QCP ON MM_QCP.QualityParameterID=QCP.QualityParameterID AND QCP.IsDeleted=0 AND QCP.IsActive=1 and (ControlTypeID=1 and ParameterDataTypeID in(1,2)) WHERE MM_QCP.MaterialMasterID=" + MaterialMasterID + "  AND MM_QCP.IsActive=1 AND MM_QCP.IsDeleted=0";
            IDataReader rsQCTolarenceList = DB.GetRS(cmdQcTolarance);
            while (rsQCTolarenceList.Read())
                TolarencesList.Add(String.Format("{0}|{1}|{2}", rsQCTolarenceList["MinTolerance"], rsQCTolarenceList["MaxTolerance"], rsQCTolarenceList["ParameterName"]));
            rsQCTolarenceList.Close();
            return TolarencesList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetGRNNumberList(String GRNNumber)
        {
            List<String> GRNNumberList = new List<String>();
            String cmdGRNNumber = "select GRNNumber,GRNUpdateID from INB_GRNUpdate where GRNNumber like '" + GRNNumber + "%'";
            IDataReader rsGRNNumberList = DB.GetRS(cmdGRNNumber);
            while (rsGRNNumberList.Read())
                GRNNumberList.Add(String.Format("{0},{1}", rsGRNNumberList["GRNNumber"], rsGRNNumberList["GRNUpdateID"]));
            rsGRNNumberList.Close();
            return GRNNumberList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadproductionType(String Prefix)
        {
            List<String> productionTypeList = new List<String>();
            String cmdProductionType = "select ProductionOrderType,ProductionOrderTypeID from MFG_ProductionOrderType where IsDeleted=0 and IsActive=1  and ProductionOrderType like '" + Prefix + "%'";
            IDataReader rsProductionTypeList = DB.GetRS(cmdProductionType);
            while (rsProductionTypeList.Read())
                productionTypeList.Add(String.Format("{0},{1}", rsProductionTypeList["ProductionOrderType"], rsProductionTypeList["ProductionOrderTypeID"]));
            rsProductionTypeList.Close();
            return productionTypeList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadUsersBasedRole(String Prefix, String TenantID)
        {
            List<String> UsersList = new List<String>();
            String cmdUsers = "select isnull(FirstName,'')+' '+isnull(MiddleName,'')+' '+isnull(LastName,'') as UserName,UserID from GEN_User where TenantID=" + TenantID + " and IsDeleted=0 and IsActive=1 and FirstName like '" + Prefix + "%' order by FirstName";
            IDataReader rsUsersList = DB.GetRS(cmdUsers);
            while (rsUsersList.Read())
                UsersList.Add(String.Format("{0},{1}", rsUsersList["UserName"], rsUsersList["UserID"]));
            rsUsersList.Close();
            return UsersList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String CheckAutoKitNoorNot(String SOHeaderID)
        {
            String IsRequire = "0";
            String cMdIsRequire = "select 1 AS N from  ORD_SOHeader where ProjectCode='RT175' AND SOHeaderID=" + SOHeaderID;
            IsRequire = Convert.ToString(DB.GetSqlN(cMdIsRequire));
            return IsRequire;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadMaterialListForProduction(String prefix)
        {
            List<String> MaterialList = new List<String>();
            String cmdMaterial = "SELECT MM.MCode+'-'+MMV.Revision AS ProductionMaterial,mmv.MaterialMasterRevisionID FROM MMT_MaterialMaster_Revision MMV JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMV.MaterialMasterID AND MM.MTypeID IN (9,8) WHERE MMV.IsDeleted=0 AND MMV.IsActive=1 and mm.MCode+'-'+MMV.Revision like '" + prefix + "%'";
            IDataReader rsMaterialList = DB.GetRS(cmdMaterial);
            while (rsMaterialList.Read())
                MaterialList.Add(String.Format("{0},{1}", rsMaterialList["ProductionMaterial"], rsMaterialList["MaterialMasterRevisionID"]));
            rsMaterialList.Close();
            return MaterialList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadMaterialListForPhantomProduction(String prefix)
        {
            List<String> MaterialList = new List<String>();
            String cmdMaterial = "SELECT MM.MCode+'-'+MMV.Revision AS ProductionMaterial,mmv.MaterialMasterRevisionID FROM MMT_MaterialMaster_Revision MMV JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMV.MaterialMasterID AND MM.MTypeID=8 WHERE MMV.IsDeleted=0 AND MMV.IsActive=1 and mm.MCode+'-'+MMV.Revision like '" + prefix + "%'";
            IDataReader rsMaterialList = DB.GetRS(cmdMaterial);
            while (rsMaterialList.Read())
                MaterialList.Add(String.Format("{0},{1}", rsMaterialList["ProductionMaterial"], rsMaterialList["MaterialMasterRevisionID"]));
            rsMaterialList.Close();
            return MaterialList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadRoutingDetetailsForProduction(String prefix, String ProductionOrderHeaderID)
        {
            List<String> RoutingDetailList = new List<String>();
            String cmdRoutingDetailsList = "select DISTINCT ROUD.OperationNumber,ROUD.RoutingDetailsID from MFG_ProductionOrderHeader PROH  JOIN MFG_RoutingHeader_Revision ROUV ON ROUV.RoutingHeaderRevisionID=PROH.RoutingHeaderRevisionID AND ROUV.IsActive=1 AND ROUV.IsDeleted=0 JOIN MFG_RoutingDetails ROUD ON ROUD.RoutingHeaderID=ROUV.RoutingHeaderID AND ROUD.IsActive=1 AND ROUD.IsDeleted=0 JOIN MFG_RoutingDetailsActivity ROUA ON ROUA.RoutingDetailsID=ROUD.RoutingDetailsID AND ROUA.IsDeleted=0 JOIN MFG_RoutingDetailsActivity_MaterilaMaster ROUMM ON ROUMM.RoutingDetailsActivityID=ROUA.RoutingDetailsActivityID AND ROUMM.IsDeleted=0 WHERE PROH.ProductionOrderHeaderID=" + ProductionOrderHeaderID + " and ROUD.OperationNumber like '" + prefix + "%'";
            IDataReader rsRouotingDetailslList = DB.GetRS(cmdRoutingDetailsList);
            while (rsRouotingDetailslList.Read())
                RoutingDetailList.Add(String.Format("{0},{1}", rsRouotingDetailslList["OperationNumber"], rsRouotingDetailslList["RoutingDetailsID"]));
            rsRouotingDetailslList.Close();
            return RoutingDetailList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadRoutingDetetailsActivitiesForRoutingDetails(String prefix, String RoutingDetailsID)
        {
            List<String> RoutingDetailsActivityList = new List<String>();
            String cmdRoutingDetailsActivityList = "select DISTINCT ROUA.ActivityCode,ROUA.RoutingDetailsActivityID from MFG_RoutingDetailsActivity ROUA JOIN MFG_RoutingDetailsActivity_MaterilaMaster ROUMM ON ROUMM.RoutingDetailsActivityID=ROUA.RoutingDetailsActivityID AND ROUMM.IsDeleted=0 where ROUA.IsDeleted=0 and ROUA.IsActive=1 and ROUA.RoutingDetailsID=" + RoutingDetailsID + " and ActivityCode like '" + prefix + "%'";
            IDataReader rsRouotingDetailsActivitylList = DB.GetRS(cmdRoutingDetailsActivityList);
            while (rsRouotingDetailsActivitylList.Read())
                RoutingDetailsActivityList.Add(String.Format("{0},{1}", rsRouotingDetailsActivitylList["ActivityCode"], rsRouotingDetailsActivitylList["RoutingDetailsActivityID"]));
            rsRouotingDetailsActivitylList.Close();
            return RoutingDetailsActivityList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetPOHeaderList(String prefix)
        {
            List<String> POHeaderList = new List<String>();
            String cmdPOHeader = "select DISTINCT PONumber,POH.POHeaderID from INB_Inbound_ORD_SupplierInvoice INB_SUP JOIN ORD_SupplierInvoice SUP ON SUP.SupplierInvoiceID=INB_SUP.SupplierInvoiceID AND INB_SUP.IsActive=1 AND INB_SUP.IsDeleted=0 JOIN ORD_POHeader POH ON POH.POHeaderID=SUP.POHeaderID AND POH.IsDeleted=0 AND POH.IsActive=1 JOIN INB_GRNUpdate GRN ON GRN.InboundID=INB_SUP.InboundID AND GRN.IsActive=1 AND GRN.IsDeleted=0 where POTypeID<5 and PONumber like '" + prefix + "%'";
            IDataReader rsPOHeaderList = DB.GetRS(cmdPOHeader);
            while (rsPOHeaderList.Read())
                POHeaderList.Add(String.Format("{0},{1}", rsPOHeaderList["PONumber"], rsPOHeaderList["POHeaderID"]));
            rsPOHeaderList.Close();
            return POHeaderList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetPOHeaderListTenant(String prefix, String TenantID)
        {
            List<String> POHeaderList = new List<String>();
            //String cmdPOHeader = "select DISTINCT PONumber,POH.POHeaderID from INB_Inbound_ORD_SupplierInvoice INB_SUP JOIN ORD_SupplierInvoice SUP ON SUP.SupplierInvoiceID=INB_SUP.SupplierInvoiceID AND INB_SUP.IsActive=1 AND INB_SUP.IsDeleted=0 JOIN ORD_POHeader POH ON POH.POHeaderID=SUP.POHeaderID AND POH.IsDeleted=0 AND POH.IsActive=1 JOIN INB_GRNUpdate GRN ON GRN.InboundID=INB_SUP.InboundID AND GRN.IsActive=1 AND GRN.IsDeleted=0 where POTypeID<5 and POH.TenantID=" + TenantID + " and POH.SupplierID not in (-1,-3,-4) and PONumber like '" + prefix + "%'";
            String cmdPOHeader = "select DISTINCT PONumber,POH.POHeaderID from INB_Inbound_ORD_SupplierInvoice INB_SUP JOIN ORD_SupplierInvoice SUP ON SUP.SupplierInvoiceID=INB_SUP.SupplierInvoiceID AND INB_SUP.IsActive=1 AND INB_SUP.IsDeleted=0 JOIN ORD_POHeader POH ON POH.POHeaderID=SUP.POHeaderID AND POH.IsDeleted=0 AND POH.IsActive=1 JOIN ORD_POType POT ON POT.POTypeID=POH.POTypeID AND isnull(POT.IsHidden,0)=0 AND POT.IsDeleted=0 AND POT.IsActive=1 JOIN INB_GRNUpdate GRN ON GRN.InboundID=INB_SUP.InboundID AND GRN.IsActive=1 AND GRN.IsDeleted=0 where POH.TenantID=" + TenantID + " and POH.SupplierID not in (-1,-3,-4) and PONumber like '" + prefix + "%'";
            IDataReader rsPOHeaderList = DB.GetRS(cmdPOHeader);
            while (rsPOHeaderList.Read())
                POHeaderList.Add(String.Format("{0},{1}", rsPOHeaderList["PONumber"], rsPOHeaderList["POHeaderID"]));
            rsPOHeaderList.Close();
            return POHeaderList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetInvoiceListForPONumber(String POHeaderID)
        {
            List<String> InvoiceList = new List<String>();
            String cmdInvoice = "select distinct sup.SupplierInvoiceID,sup.InvoiceNumber from ORD_SupplierInvoice sup join INB_GRNUpdate GRN ON GRN.SupplierInvoiceID=SUP.SupplierInvoiceID AND GRN.IsDeleted=0 AND GRN.IsActive=1 where SUP.IsDeleted=0 and SUP.IsActive=1 and SUP.POHeaderID=" + POHeaderID;
            IDataReader rsInvoiceList = DB.GetRS(cmdInvoice);
            while (rsInvoiceList.Read())
                InvoiceList.Add(String.Format("{0},{1}", rsInvoiceList["InvoiceNumber"], rsInvoiceList["SupplierInvoiceID"]));
            rsInvoiceList.Close();
            return InvoiceList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetStoreForPONumber(String POHeaderID, String InvoiceNumber)
        {
            List<String> StoreList = new List<String>();
            String cmdStore = "select distinct WHCode,WH.WarehouseID from INB_RefWarehouse_Details REFW JOIN INB_Inbound_ORD_SupplierInvoice INB_INV ON INB_INV.InboundID=REFW.InboundID AND INB_INV.IsDeleted=0 AND INB_INV.IsActive=1 JOIN ORD_POHeader POH ON POH.POHeaderID=INB_INV.POHeaderID AND POH.IsActive=1 AND POH.IsDeleted=0 JOIN GEN_Warehouse WH ON WH.WarehouseID=REFW.WarehouseID WHERE WH.isdeleted=0 and wh.isactive=1 and POH.POHeaderID=" + POHeaderID + " AND INB_INV.SupplierInvoiceID=" + InvoiceNumber;
            //String cmdStore = "select WHCode,WH.WarehouseID from GEN_Warehouse WH WHERE wh.IsDeleted=0 and IsActive=1";
            IDataReader rsStoreList = DB.GetRS(cmdStore);
            while (rsStoreList.Read())
                StoreList.Add(String.Format("{0},{1}", rsStoreList["WHCode"], rsStoreList["WarehouseID"]));
            rsStoreList.Close();
            return StoreList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetMCodeForInternalOrder(string prefix, String ProductionOrderHeaderID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "SELECT top 10 MM.MaterialMasterID,MCode FROM MFG_BOMDetails BOMD JOIN MFG_BOMHeader_Revision BOMDV ON BOMDV.BOMHeaderID=BOMD.BOMHeaderID AND BOMDV.IsDeleted=0 AND BOMDV.IsActive=1 JOIN MFG_RoutingHeader_Revision ROUV ON ROUV.BOMHeaderRevisionID=BOMDV.BOMHeaderRevisionID AND ROUV.IsDeleted=0 AND ROUV.IsActive=1 JOIN MFG_ProductionOrderHeader PROH ON PROH.RoutingHeaderRevisionID=ROUV.RoutingHeaderRevisionID AND PROH.IsDeleted=0 AND PROH.IsActive=1 JOIN MMT_MaterialMaster_Revision MMV ON MMV.MaterialMasterRevisionID=ROUV.FinishedMaterialMasterID JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=BOMD.BOMMaterialMasterID AND BOMD.ParentMaterialMasterID=MMV.MaterialMasterID AND MM.MTypeID=7 WHERE BOMD.IsDeleted=0 AND BOMD.IsActive=1 AND PROH.ProductionOrderHeaderID=" + ProductionOrderHeaderID + " and MCode LIKE '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetMCodeForInternalOrderWithOEM(string prefix, String ProductionOrderHeaderID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "SELECT top 10 MM.MaterialMasterID,MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode  FROM MFG_BOMDetails BOMD JOIN MFG_BOMHeader_Revision BOMDV ON BOMDV.BOMHeaderID=BOMD.BOMHeaderID AND BOMDV.IsDeleted=0 AND BOMDV.IsActive=1 JOIN MFG_RoutingHeader_Revision ROUV ON ROUV.BOMHeaderRevisionID=BOMDV.BOMHeaderRevisionID AND ROUV.IsDeleted=0 AND ROUV.IsActive=1 JOIN MFG_ProductionOrderHeader PROH ON PROH.RoutingHeaderRevisionID=ROUV.RoutingHeaderRevisionID AND PROH.IsDeleted=0 AND PROH.IsActive=1 JOIN MMT_MaterialMaster_Revision MMV ON MMV.MaterialMasterRevisionID=ROUV.FinishedMaterialMasterID JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=BOMD.BOMMaterialMasterID AND BOMD.ParentMaterialMasterID=MMV.MaterialMasterID AND MM.MTypeID=7 WHERE BOMD.IsDeleted=0 AND BOMD.IsActive=1 AND PROH.ProductionOrderHeaderID=" + ProductionOrderHeaderID + " AND ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetEmployeeListInTMO(string prefix)
        {
            List<string> EmployeeList = new List<string>();
            string EmployeeSql = "select distinct us.UserID,us.FirstName+' '+isnull(us.MiddleName,'')+' '+isnull(us.LastName,'') as UserName from ORD_SOHeader SOH join OBD_Outbound_ORD_CustomerPO OBDCUS on OBDCUS.SOHeaderID=soh.SOHeaderID and OBDCUS.IsDeleted=0 join OBD_Outbound OBD ON OBD.OutboundID=OBDCUS.OutboundID AND OBD.DeliveryStatusID!=1 AND OBD.IsDeleted=0 JOIN GEN_User us on us.UserID=SOH.UserID where SOH.IsDeleted=0 and FirstName like '" + prefix + "%'";

            IDataReader rsEmployeeList = DB.GetRS(EmployeeSql);

            while (rsEmployeeList.Read())
            {
                EmployeeList.Add(string.Format("{0},{1}", rsEmployeeList["UserName"], rsEmployeeList["UserID"]));
            }
            rsEmployeeList.Close();
            return EmployeeList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetTMOListEmployee(string UserID)
        {
            List<string> TMOList = new List<string>();
            string TMOSql = "select SOH.SOHeaderID,SONumber from ORD_SOHeader SOH JOIN OBD_Outbound_ORD_CustomerPO OBDCUS ON OBDCUS.SOHeaderID=SOH.SOHeaderID AND OBDCUS.IsDeleted=0 JOIN OBD_Outbound OBD ON OBD.OutboundID=OBDCUS.OutboundID AND OBD.IsDeleted=0 AND OBD.DeliveryStatusID!=1 WHERE SOH.IsDeleted=0 and UserID=" + UserID;

            IDataReader rsTMOList = DB.GetRS(TMOSql);

            while (rsTMOList.Read())
            {
                TMOList.Add(string.Format("{0},{1}", rsTMOList["SONumber"], rsTMOList["SOHeaderID"]));
            }
            rsTMOList.Close();
            return TMOList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetStoreForEmployee(string SOHeaderID)
        {
            List<string> StoreList = new List<string>();
            string StoreSql = "select WH.WarehouseID,WH.WHCode from ORD_SOHeader SOH JOIN OBD_Outbound_ORD_CustomerPO obd_cus on obd_cus.SOHeaderID=soh.SOHeaderID and obd_cus.IsDeleted=0 join OBD_RefWarehouse_Details ref on ref.OutboundID=obd_cus.OutboundID and ref.IsDeleted=0 join GEN_Warehouse WH ON WH.WarehouseID=REF.WarehouseID WHERE SOH.SOHeaderID=" + SOHeaderID;

            IDataReader rsStoreList = DB.GetRS(StoreSql);

            while (rsStoreList.Read())
            {
                StoreList.Add(string.Format("{0},{1}", rsStoreList["WHCode"], rsStoreList["WarehouseID"]));
            }
            rsStoreList.Close();
            return StoreList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetNonConformityLocations(string Prefix, String IsNonConformity, String AsIs)
        {
            List<string> LocationList = new List<string>();
            string cMdLocation = "select top 20 Location,LocationID from INV_Location where left(location,2) " + (IsNonConformity == "1" && AsIs == "0" ? "='Q1'" : "!='Q1'") + " and IsDeleted=0 and location like '" + Prefix + "%'";

            IDataReader rsLocation = DB.GetRS(cMdLocation);

            while (rsLocation.Read())
            {
                LocationList.Add(string.Format("{0},{1}", rsLocation["Location"], rsLocation["LocationID"]));
            }
            rsLocation.Close();
            return LocationList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetPONumberForNCReport(string Prefix)
        {
            List<string> PONumberList = new List<string>();
            string cMdPONnumberList = "select POH.POHeaderID,PONumber from ORD_POHeader POH JOIN INB_Inbound_ORD_SupplierInvoice INBSUP ON INBSUP.POHeaderID=POH.POHeaderID AND INBSUP.IsDeleted=0 AND INBSUP.IsActive=1 WHERE POH.IsDeleted=0 AND POH.IsActive=1 AND POH.PONumber LIKE '" + Prefix + "%'";

            IDataReader rsPONumberList = DB.GetRS(cMdPONnumberList);

            while (rsPONumberList.Read())
            {
                PONumberList.Add(string.Format("{0},{1}", rsPONumberList["PONumber"], rsPONumberList["POHeaderID"]));
            }
            rsPONumberList.Close();
            return PONumberList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDashboardData()
        {
            List<string> DashboardData = new List<string>();
            string cMdDashboardData = "exec [dbo].[sp_MFG_GetJoborderDashboardData]";

            IDataReader rsDashboardData = DB.GetRS(cMdDashboardData);
            StringBuilder DashboardDetails = new StringBuilder();
            while (rsDashboardData.Read())
            {
                DashboardDetails.Append("þ" + string.Format("{0}*{1}*{2}*{3}*{4}", rsDashboardData["JobDetails"], rsDashboardData["OperationsData"], rsDashboardData["ActivitiesCount"], rsDashboardData["OperationCount"], DB.RSField(rsDashboardData, "MaterialDeficiancyDetails")));
                //DashboardData.Add(string.Format("{0}*{1}*{2}*{3}", rsDashboardData["JobDetails"], rsDashboardData["OperationsData"], rsDashboardData["ActivitiesCount"], rsDashboardData["OperationCount"]));
            }
            if (DashboardDetails.ToString().Length > 0)
                DashboardDetails.Remove(0, 1);
            rsDashboardData.Close();
            return DashboardDetails.ToString();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetStorerefNoForGoodsIn(string Prefix)
        {
            List<string> StoreNoList = new List<string>();
            string cMdStoreRefNo = "select top 10 StoreRefNo,InboundID from INB_Inbound where IsDeleted=0 and IsActive=1 and StoreRefNo like '" + Prefix + "%'";

            IDataReader rsStorerefNo = DB.GetRS(cMdStoreRefNo);

            while (rsStorerefNo.Read())
            {
                StoreNoList.Add(string.Format("{0}", rsStorerefNo["StoreRefNo"]));
            }
            rsStorerefNo.Close();
            return StoreNoList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetStorerefNoForGoodsInTenant(string Prefix, string TenantID)
        {
            List<string> StoreNoList = new List<string>();
            string cMdStoreRefNo = "select top 10 StoreRefNo,InboundID from INB_Inbound where IsDeleted=0 and IsActive=1 and TenantID=" + TenantID + "and StoreRefNo like '" + Prefix + "%'";

            IDataReader rsStorerefNo = DB.GetRS(cMdStoreRefNo);

            while (rsStorerefNo.Read())
            {
                StoreNoList.Add(string.Format("{0}", rsStorerefNo["StoreRefNo"]));
            }
            rsStorerefNo.Close();
            return StoreNoList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetJobOrderrefData(string Prefix)
        {
            List<string> JobOrderData = new List<string>();
            string cMdJobOrderData = "SELECT  top 10  MCode,Revision,KitCode,LineNumber,MCode+'-'+Revision AS JobOrderRefNo,SOD.SODetailsID  FROM OBD_Outbound_ORD_CustomerPO C_PO JOIN MFG_SOPO_ProductionOrder SOPO_P ON SOPO_P.SOPOHeaderID=C_PO.SOHeaderID AND SOPO_P.IsActive=1 AND SOPO_P.IsDeleted=0 JOIN ORD_SODetails SOD ON SOD.SOHeaderID=C_PO.SOHeaderID AND SOD.IsActive=1 AND SOD.IsDeleted=0 JOIN MFG_ProductionOrderHeader PH ON PH.ProductionOrderHeaderID=SOPO_P.ProductionOrderHeaderID AND PH.IsActive=1 AND PH.IsDeleted=0 JOIN MMT_MaterialMaster_Revision MM_R ON MM_R.MaterialMasterRevisionID=PH.MaterialMasterRevisionID AND MM_R.IsActive=1 AND MM_R.IsDeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MM_R.MaterialMasterID AND MM.IsActive=1 AND MM.IsDeleted=0 WHERE  C_PO.IsActive=1 AND C_PO.IsDeleted=0  and MCode like '" + Prefix + "%' ";

            IDataReader rsJobOrderData = DB.GetRS(cMdJobOrderData);

            while (rsJobOrderData.Read())
            {
                JobOrderData.Add(string.Format("{0}", rsJobOrderData["JobOrderRefNo"]));
            }
            rsJobOrderData.Close();
            return JobOrderData.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetPOLineNumbers(string StoreRefNo, String MCode)
        {
            List<string> POLineNumbersList = new List<string>();
            string cMdPOLineNumber = "select distinct LineNumber from INB_Inbound INB JOIN INB_Inbound_ORD_SupplierInvoice INBSUP ON INBSUP.InboundID=INB.InboundID AND INBSUP.IsDeleted=0 JOIN ORD_SupplierInvoiceDetails SD ON SD.SupplierInvoiceID=INBSUP.SupplierInvoiceID AND SD.IsActive=1 AND SD.IsDeleted=0 JOIN ORD_PODetails POD ON POD.PODetailsID=SD.PODetailsID AND POD.IsDeleted=0 AND POD.IsActive=1 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=POD.MaterialMasterID  WHERE INB.StoreRefNo='" + StoreRefNo + "' AND MM.MCode='" + MCode + "'";

            IDataReader rsPOLineNumber = DB.GetRS(cMdPOLineNumber);

            while (rsPOLineNumber.Read())
            {
                POLineNumbersList.Add(string.Format("{0}", rsPOLineNumber["LineNumber"]));
            }
            rsPOLineNumber.Close();
            return POLineNumbersList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetLocationList(string Prefix, String ProductCategory, String InboundID)
        {
            List<string> LocationList = new List<string>();
            string cMdLocationList = "select top 10 Location,LocationID from INV_Location loc join INV_LocationZone locz on locz.LocationZoneCode=left(Location,2) and locz.IsDeleted=0 left join INB_RefWarehouse_Details refw on refw.WarehouseID=locz.WarehouseID and refw.IsDeleted=0 and refw.InboundID=" + InboundID + " where (" + InboundID + "=0 or refw.InboundID is not null) and loc.IsDeleted=0 and (" + ProductCategory + " not in (0,3) or  (0=" + ProductCategory + " and left(Location,2)!='Q1') or (3=" + ProductCategory + " and left(Location,2)='P1')) and Location  like '" + Prefix + "%'";

            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0}", rslocationList["Location"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetKitCodeList(string Prefix, String Type)
        {
            List<string> KitCodeList = new List<string>();
            string cMdKitCodeList = "";
            if (Type == "0")
            {
                cMdKitCodeList = "select distinct top 10  KitCode from MFG_ProductionOrderHeader where IsDeleted=0 and ProductionOrderStatusID=2 AND ProductionOrderTypeID!=7 and KitCode like '" + Prefix + "%'";

            }
            else
            {
                cMdKitCodeList = "select distinct top 10  KitCode from MFG_ProductionOrderHeader where IsDeleted=0 and KitCode like '" + Prefix + "%'";
            }
            IDataReader rsKitCodeList = DB.GetRS(cMdKitCodeList);

            while (rsKitCodeList.Read())
            {
                KitCodeList.Add(string.Format("{0}", rsKitCodeList["KitCode"]));
            }
            rsKitCodeList.Close();
            return KitCodeList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetinvoiceListForGoodsIN(String StoreNo, String MCode, String LineNo, String POHeaderID)
        {
            List<string> InvoiceList = new List<string>();
            StringBuilder cMdInvoiceListForGoodsIn = new StringBuilder();
            cMdInvoiceListForGoodsIn.Append("select distinct SUP.SupplierInvoiceID,sup.InvoiceNumber from ORD_POHeader POH JOIN ORD_SupplierInvoice SUP ON SUP.POHeaderID=POH.POHeaderID ");
            cMdInvoiceListForGoodsIn.Append("JOIN INB_Inbound_ORD_SupplierInvoice INBSUP ON INBSUP.SupplierInvoiceID=SUP.SupplierInvoiceID JOIN INB_Inbound INB ON INB.InboundID=INBSUP.InboundID ");
            cMdInvoiceListForGoodsIn.Append("JOIN ORD_PODetails POD ON POD.POHeaderID=POH.POHeaderID AND POD.IsDeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=POD.MaterialMasterID ");
            cMdInvoiceListForGoodsIn.Append("JOIN ORD_SupplierInvoiceDetails SUPID ON SUPID.SupplierInvoiceID=SUP.SupplierInvoiceID WHERE StoreRefNo='" + StoreNo + "' AND MCode='" + MCode + "' AND POD.LineNumber=" + LineNo + " AND (0=" + POHeaderID + " or POH.POHeaderID=" + POHeaderID + ")");
            IDataReader rsInvoiceList = DB.GetRS(cMdInvoiceListForGoodsIn.ToString());

            while (rsInvoiceList.Read())
            {
                InvoiceList.Add(string.Format("{0},{1}", rsInvoiceList["InvoiceNumber"], rsInvoiceList["SupplierInvoiceID"]));
            }
            rsInvoiceList.Close();
            return InvoiceList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMCodeForECO(string prefix, string TenantID)
        {
            List<string> MMList = new List<string>();
            StringBuilder MMSql = new StringBuilder();
            MMSql.Append("SELECT DISTINCT MM.MCode,MM.MaterialMasterID from MFG_RoutingHeader_Revision ROUR ");
            MMSql.Append("JOIN MMT_MaterialMaster_Revision MMR ON MMR.MaterialMasterRevisionID=ROUR.MaterialMasterRevisionID ");
            MMSql.Append("JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMR.MaterialMasterID ");
            MMSql.Append("JOIN MFG_RoutingDetails ROUD ON ROUD.RoutingHeaderID=ROUR.RoutingHeaderID AND ROUD.IsDeleted=0 ");
            MMSql.Append("JOIN MFG_RoutingDetailsActivity ROUDA ON ROUDA.RoutingDetailsID=ROUD.RoutingDetailsID AND ROUDA.IsDeleted=0 ");
            MMSql.Append("JOIN MFG_RoutingClone ROUC ON ROUC.OldRoutingDetailsActivityID=ROUDA.RoutingDetailsActivityID ");
            MMSql.Append("JOIN MFG_RoutingDetailsActivity ROUDA1 ON ROUDA1.RoutingDetailsActivityID=ROUC.NewRoutingDetailsActivityID ");
            MMSql.Append("JOIN MFG_RoutingDetails ROUD1 ON ROUD1.RoutingDetailsID=ROUDA1.RoutingDetailsID ");
            MMSql.Append("JOIN MFG_RoutingHeader_Revision ROUR1 ON ROUR1.RoutingHeaderID=ROUD1.RoutingHeaderID ");
            MMSql.Append("where MM.MCode like '" + prefix + "%'");

            IDataReader rsMM = DB.GetRS(MMSql.ToString());

            while (rsMM.Read())
            {
                MMList.Add(string.Format("{0}~{1}", rsMM["MCode"], rsMM["MaterialMasterID"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }

        #endregion     -----------------   Developed By Prasad   -------------------------


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetWorkStationType(string Prefix)
        {
            List<string> WorkStationList = new List<string>();
            string cMdWorkStationsql = "select WorkCenterGroup,WorkCenterGroupID,* from MFG_WorkCenterGroup  where WorkCenterGroup  like '" + Prefix + "%' ";

            IDataReader drWorkStation = DB.GetRS(cMdWorkStationsql);

            while (drWorkStation.Read())
            {
                WorkStationList.Add(string.Format("{0}", drWorkStation["WorkCenterGroup"]));
            }
            drWorkStation.Close();
            return WorkStationList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetWorkCenterType(string Prefix)
        {
            List<string> WorkCenterList = new List<string>();
            string cMdWorkCentersql = "select WorkCenter,WorkCenterID from MFG_WorkCenter  where WorkCenter like '" + Prefix + "%' ";

            IDataReader drWorkCenter = DB.GetRS(cMdWorkCentersql);

            while (drWorkCenter.Read())
            {
                WorkCenterList.Add(string.Format("{0}", drWorkCenter["WorkCenter"]));
            }
            drWorkCenter.Close();
            return WorkCenterList.ToArray();
        }

        #region     -----------------   Learned By Krishna   -------------------------

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetFuncExecCode(string Prefix)
        {
            List<string> LocationList = new List<string>();
            string cMdLocationList = "select ConfigurableFunctionID,FuncExecCode from [GEN_Configurable_Functionality] where IsActive=1 and IsDeleted=0 and FuncExecCode like '" + Prefix + "%'";

            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0},{1}", rslocationList["FuncExecCode"], rslocationList["ConfigurableFunctionID"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetLocationForMaterial_WareHouse(string Prefix, String materialID, String TenantID)
        {
            List<string> LocationList = new List<string>();
            string cMdLocationList = "SELECT TOP 15 DisplayLocationCode Location, LocationID FROM INV_Location LOC JOIN INV_LocationZone LOCZ ON LOCZ.LocationZoneID = LOC.ZoneId WHERE Location  like '" + Prefix + "%' AND WarehouseID IN(SELECT WarehouseID  FROM TPL_Tenant_Contract WHERE TenantID = " + TenantID + "  ) and  LOC.DockID is null";
            //" JOIN TPL_Tenant_MaterialMaster TNT_M ON TNT_M.TenantID=LOC.TenantID AND TNT_M.IsDeleted=0 "+
            // " JOIN MMT_MaterialMaster_Supplier MM_S ON MM_S.MaterialMasterID=TNT_M.MaterialMasterID AND MM_S.SupplierID=LOC.SupplierID AND MM_S.IsDeleted=0  "+
            //" WHERE TNT_M.MaterialMasterID=" + materialID + " and IsQuarantine=" + IsQuarantine + " and Location  like '" + Prefix + "%'";
            // " WHERE  IsQuarantine=" + IsQuarantine + " and Location  like '" + Prefix + "%' and createdby="+cp.UserID+"";
            //"select top 10 Location,LocationID from INV_Location loc join INV_LocationZone locz on locz.LocationZoneCode=left(Location,2) and locz.IsDeleted=0 left join INB_RefWarehouse_Details refw on refw.WarehouseID=locz.WarehouseID and refw.IsDeleted=0 and refw.InboundID=" + InboundID + " where (" + InboundID + "=0 or refw.InboundID is not null) and loc.IsDeleted=0 and (" + ProductCategory + " not in (0,3) or  (0=" + ProductCategory + " and left(Location,2)!='Q1') or (3=" + ProductCategory + " and left(Location,2)='P1')) and Location  like '" + Prefix + "%'";

            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0}", rslocationList["Location"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetLocationForMaterial(string Prefix, String materialID, String IsQuarantine)
        {
            List<string> LocationList = new List<string>();
            string cMdLocationList = "SELECT TOP 10 LocationID,Location FROM INV_Location LOC " +
                                        //" JOIN TPL_Tenant_MaterialMaster TNT_M ON TNT_M.TenantID=LOC.TenantID AND TNT_M.IsDeleted=0 "+
                                        // " JOIN MMT_MaterialMaster_Supplier MM_S ON MM_S.MaterialMasterID=TNT_M.MaterialMasterID AND MM_S.SupplierID=LOC.SupplierID AND MM_S.IsDeleted=0  "+
                                        //" WHERE TNT_M.MaterialMasterID=" + materialID + " and IsQuarantine=" + IsQuarantine + " and Location  like '" + Prefix + "%'";
                                        " WHERE  IsQuarantine=" + IsQuarantine + " and Location  like '" + Prefix + "%' and createdby=" + cp.UserID + "";
            //"select top 10 Location,LocationID from INV_Location loc join INV_LocationZone locz on locz.LocationZoneCode=left(Location,2) and locz.IsDeleted=0 left join INB_RefWarehouse_Details refw on refw.WarehouseID=locz.WarehouseID and refw.IsDeleted=0 and refw.InboundID=" + InboundID + " where (" + InboundID + "=0 or refw.InboundID is not null) and loc.IsDeleted=0 and (" + ProductCategory + " not in (0,3) or  (0=" + ProductCategory + " and left(Location,2)!='Q1') or (3=" + ProductCategory + " and left(Location,2)='P1')) and Location  like '" + Prefix + "%'";

            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0}", rslocationList["Location"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetCurrencyForTPL(string prefix)
        {

            List<string> CurrencyList = new List<string>();
            string CurrencySql = " select top 10 Currency,CurrencyID,Code from GEN_Currency where    IsActive=1     AND  Code  like '" + prefix + "%' order by Code";


            IDataReader rsCurrency = DB.GetRS(CurrencySql);

            while (rsCurrency.Read())
            {
                CurrencyList.Add(string.Format("{0},{1}", rsCurrency["Code"] + "-" + rsCurrency["Currency"], rsCurrency["CurrencyID"]));
            }

            rsCurrency.Close();

            return CurrencyList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetRawMaterial(string prefix, string TenantID)
        {
            List<string> mmList = new List<string>();

            //string mmSql = "select Top 20 MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode , MaterialMasterID from MMT_MaterialMaster where TenantID=" + TenantID + " and IsDeleted=0 and  IsActive=1  AND ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";//and MTypeID=7
            //string mmSql = "select distinct Top 20 MCode  AS MCode , MM.MaterialMasterID from MMT_MaterialMaster MM  " +
            //    "join MMT_MaterialMaster_Supplier MMTSUP on mm.MaterialMasterID=MMTSUP.MaterialMasterID and MMTSUP.IsDeleted=0 and MMTSUP.isactive=1 " +
            //    "join MMT_MaterialMaster_GEN_UoM uom on mm.MaterialMasterID =uom.MaterialMasterID and uom.IsDeleted=0 and uom.IsActive=1 " +
            //    "LEFT OUTER JOIN TPL_Tenant_MaterialMaster TMM ON TMM.MaterialMasterID = MM.MaterialMasterID AND TMM.IsActive = 1 AND TMM.IsDeleted = 0 LEFT OUTER JOIN TPL_Tenant TNT ON TNT.TenantID = TMM.TenantID AND TNT.IsActive = 1 AND TNT.IsDeleted = 0 join vAvailableStock AVl on AVl.MaterialMasterID = MM.MaterialMasterID  where TNT.TenantID =" + TenantID + " and MM.IsDeleted = 0 and MM.IsActive = 1 AND TNT.AccountID =" + cp.AccountID + " AND (MCode like '" + prefix + "%' OR  OEMPartNo like '%') order by MCode";//and MTypeID=7

            string mmSql = "select distinct Top 20 MCode  AS MCode , MM.MaterialMasterID from MMT_MaterialMaster MM  " +
               "join MMT_MaterialMaster_GEN_UoM uom on mm.MaterialMasterID =uom.MaterialMasterID and uom.IsDeleted=0 and uom.IsActive=1 " +
               "join vAvailableStock AVl on AVl.MaterialMasterID = MM.MaterialMasterID  where MM.TenantID =" + TenantID + " and MM.IsDeleted = 0 and MM.IsActive = 1 AND AVl.AccountID =" + cp.AccountID + " AND (MCode like '" + prefix + "%' OR  OEMPartNo like '"+ prefix + "%') order by MCode";//and MTypeID=7




            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetMaterialForMiscReceipt(string prefix, string TenantID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select DISTINCT Top 20 MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode , MM.MaterialMasterID from MMT_MaterialMaster MM LEFT OUTER JOIN TPL_Tenant_MaterialMaster TMM ON TMM.MaterialMasterID = MM.MaterialMasterID AND TMM.IsActive = 1 AND TMM.IsDeleted = 0 LEFT OUTER JOIN TPL_Tenant TNT ON TNT.TenantID = TMM.TenantID AND TNT.IsActive = 1 AND TNT.IsDeleted = 0 JOIN MMT_MaterialMaster_GEN_UoM MM_UoM on MM.MaterialMasterID=MM_UoM.MaterialMasterID AND MM_UoM.IsActive=1 AND MM_UoM.IsDeleted=0 where TNT.TenantID =" + TenantID + " and MM.IsDeleted = 0 and MM.IsActive = 1 AND (TNT.AccountID =" + cp.AccountID + " OR 0=" + cp.AccountID + ") AND (MCode like '" + prefix + "%' OR  OEMPartNo like '"+ prefix + "%') order by MCode";//and MTypeID=7

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetSupplierID(String prefix, String mid)
        {
            List<String> sid = new List<string>();
            String query = "select top 20 ms.SupplierName,ms.SupplierID from MMT_MaterialMaster mm join MMT_Supplier ms on mm.SupplierID=ms.SupplierID where  MaterialMasterID=" + mid + " and ms.IsActive=1 and ms.IsDeleted=0 and ms.SupplierID like '%'";
            IDataReader rsfe = DB.GetRS(query);
            while (rsfe.Read())
            {
                sid.Add(String.Format("{0},{1}", rsfe["SupplierName"], rsfe["SupplierID"]));
            }
            rsfe.Close();
            return sid.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadDelvDocNo(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select top 30 obdnumber from OBD_Outbound where IsActive=1 AND IsDeleted=0 AND OBDNumber like'" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(rsMCodeList["obdnumber"].ToString());
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadRevertStoreRefNumbers(string prefix, string TenantID)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC SP_INB_RevertInbound @StoreRefNo=NULL,@AccountID_New=" + cp.AccountID + ",@UserID_New="+cp.UserID+", @TenantID=" + (TenantID == "" ? "0" : TenantID);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("StoreRefNo like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["StoreRefNo"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();




        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadSITStoreRefNumbers(string prefix, string TenantID)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [dbo].[sp_INB_GetSITList] @StoreRefNo=NULL,@WarehouseIDs='' ,@AccountID_New = " + cp.AccountID + ",@TenantID=" + (TenantID == "" ? "0" : TenantID);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("StoreRefNo like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["StoreRefNo"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadSIEStoreRefNumbers(string prefix, string TenantID)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [dbo].[sp_INB_GetShipmentExpectedList] @StoreRefNo=NULL,@WarehouseIDs='' ,@AccountID_New =  " + cp.AccountID + " ,@TenantID=" + (TenantID == "" ? "0" : TenantID);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("StoreRefNo like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["StoreRefNo"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadSIEStoreRefNumbers_TC(string prefix, string TenantID, string WHID)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [dbo].[sp_INB_GetShipmentExpectedList] @StoreRefNo=NULL,@WarehouseIDs='" + WHID + "' ,@AccountID_New =  " + cp.AccountID + " ,@TenantID=" + (TenantID == "" ? "0" : TenantID);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("StoreRefNo like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["StoreRefNo"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadSIPStoreRefNumbers(string prefix, string TenantID)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [dbo].[sp_INB_GetSIPList] @StoreRefNo=NULL,@WarehouseIDs='',@AccountID_New = " + cp.AccountID + ",@TenantID=" + (TenantID == "" ? "0" : TenantID);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("StoreRefNo like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["StoreRefNo"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadGRNStoreRefNumbers(string prefix, string TenantID)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [dbo].[sp_INB_GetGRNPendingList] @StoreRefNo=NULL,@WarehouseIDs='',@AccountID_New=" + cp.AccountID + ",@TenantID=" + (TenantID == "" ? "0" : TenantID);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("StoreRefNo like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["StoreRefNo"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadDISStoreRefNumbers(string prefix, string TenantID)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [dbo].[sp_INB_GetDiscepancyShipmentsList] @StoreRefNo=NULL,@WarehouseIDs='',@AccountID_New= " + cp.AccountID + " ,@TenantID=" + (TenantID == "" ? "0" : TenantID);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("StoreRefNo like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["StoreRefNo"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadPNCDelvDocNo(string prefix)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [dbo].[sp_OBD_GetPlantDeliveryNoteList] @OBDNumber=NULL,@WarehouseIDs=NULL";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("OBDNumber like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["OBDNumber"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadPNCPendingDelvDocNo(string prefix, string TenantID)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [dbo].[sp_OBD_GetDIPList] @OBDNumber=NULL,@WarehouseIDs=NULL,@TenantID=" + (TenantID == "" ? "0" : TenantID) + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("OBDNumber like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["OBDNumber"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadPNCPendingDelvDocNo_TC(string prefix, string TenantID, string WHID)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [dbo].[sp_OBD_GetDIPList] @OBDNumber=NULL,@WarehouseIDs='" + WHID + "',@TenantID=" + (TenantID == "" ? "0" : TenantID) + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("OBDNumber like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["OBDNumber"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadVLPDDelvDocNo(string prefix, string TenantID)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [dbo].[sp_OBD_GetVLPDPendingList] @accountid=" + cp.AccountID + ",@tenantid=" + (TenantID == "" ? "0" : TenantID) + ",@OBDNumber=" + DB.SQuote(prefix);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            //DataRow[] drArray = dt.Select("OBDNumber like'" + prefix + "%'");
            foreach (DataRow dr in dt.Rows)
            {
                StoreRefList.Add(dr["obdnumber"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadPGIPendingDelvDocNo(string prefix, string TenantID)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [dbo].[sp_OBD_GetPGIPendingList] @OBDNumber=NULL,@WarehouseIDs=NULL,@TenantID=" + (TenantID == "" ? "0" : TenantID) + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("OBDNumber like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["OBDNumber"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadDelvPendDelvDocNo(string prefix, string TenantID)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [dbo].[sp_OBD_GetDeliveriesPendingList] @OBDNumber=NULL,@WarehouseIDs=NULL,@TenantID=" + (TenantID == "" ? "0" : TenantID) + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("OBDNumber like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["OBDNumber"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadRTRMCodes(string prefix, string InboundID)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [sp_INB_ReceivingTallyReport]  @InboundID=" + InboundID + ",@MCode=null";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
            {
                StoreRefList.Add("");
                ds.Dispose();
                dt.Dispose();
            }
            else
            {
                DataTable distinctTable = dt.AsEnumerable()
                                .GroupBy(r => r.Field<string>("MCode"))
                                .Select(g => g.First())
                                .CopyToDataTable();

                DataRow[] drArray = distinctTable.Select("MCode like'" + prefix + "%'");
                foreach (DataRow dr in drArray)
                {
                    StoreRefList.Add(dr["MCode"].ToString());
                }
                ds.Dispose();
                dt.Dispose();
                distinctTable.Dispose();
            }
            return StoreRefList.ToArray();
        }






        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadPODDelvDocNo(string prefix, string TenantID)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [dbo].[sp_OBD_GetPODPendingList] @OBDNumber=NULL,@WarehouseIDs=NULL,@TenantID=" + (TenantID == "" ? "0" : TenantID) + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("OBDNumber like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["OBDNumber"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadRevertDelvDocNo(string prefix, string Tenant)
        {
            if (Tenant == "Tenant...")
                Tenant = "";

            List<string> StoreRefList = new List<string>();
            string sql = "EXEC [sp_OBD_RevertOutbound] @OutboundID =NULL,@Tenant=" + DB.SQuote(Tenant);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("OBDNumber like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["OBDNumber"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadInwdQCStoreRefNumbers(string prefix)
        {
            List<string> StoreRefList = new List<string>();
            string sql = "EXEC sp_INB_GetShipmentVerificationProcessList @StoRerefNo='',@WarehouseIDs=''";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Clear();
            ds.Clear();
            ds = DB.GetDS(sql, false);
            dt = ds.Tables[0];
            DataRow[] drArray = dt.Select("StoRerefNo like'" + prefix + "%'");
            foreach (DataRow dr in drArray)
            {
                StoreRefList.Add(dr["StoRerefNo"].ToString());
            }
            ds.Dispose();
            dt.Dispose();
            return StoreRefList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadDPNoteMCodeOEMData(string prefix, string OutboundID)
        {


            List<string> OEMPartList = new List<string>();
            //string sql = "EXEC [sp_MFG_GetDeliverypickNoteForManufacturing]  @OutboundID=" + OutboundID.ToString() + ",@MCode=''";
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            //dt.Clear();
            //ds.Clear();
            //ds = DB.GetDS(sql, false);
            //dt = ds.Tables[0];
            //DataRow[] drArray = dt.Select("MCode like'" + prefix + "%' OR OEMPartNO like'" + prefix + "%'");
            //foreach (DataRow dr in drArray)
            //{
            //    StoreRefList.Add(dr["MCode"].ToString() + "`" + dr["OEMPartNo"].ToString());
            //}
            //ds.Dispose();
            //dt.Dispose();
            //return StoreRefList.ToArray();
            string sql = "select distinct top 10 mm.MCode+'`'+ISNULL(mm.OEMPartNo,'')[MCode] from ORD_SODetails sod 	join OBD_Outbound_ORD_CustomerPO  cpo on sod.SOHeaderID=cpo.SOHeaderID and cpo.CustomerPOID=sod.CustomerPOID and (cpo.InvoiceNo is null or ltrim(rtrim(cpo.InvoiceNo))=ltrim(rtrim(sod.InvoiceNo))) join MMT_MaterialMaster MM on mm.MaterialMasterID=sod.MaterialMasterID where cpo. OutboundID=" + OutboundID + " and sod.IsActive=1 and sod.IsDeleted=0 and mm.MCode like '" + prefix + "%'";
            IDataReader drOEMPart = DB.GetRS(sql);
            while (drOEMPart.Read())
            {
                OEMPartList.Add(string.Format("{0}", drOEMPart["MCode"]));
            }
            drOEMPart.Close();
            return OEMPartList.ToArray();
        }

        #endregion     -----------------   Developed By Krishna   -------------------------

        #region     -----------------   Developed By Subrahmanyam   -------------------------

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetJobOrderRefNo(String Prefix)
        {
            List<String> sid = new List<string>();
            String query = "EXEC [sp_RPT_GetJobOrderRefNos]";
            IDataReader rsfe = DB.GetRS(query);
            while (rsfe.Read())
            {
                sid.Add(String.Format("{0},{1}", rsfe["JobOrderRefNo"], rsfe["ProductionOrderHeaderID"]));
            }
            rsfe.Close();
            return sid.ToArray();
        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPRONumberforCoC(String KitCode)
        {
            List<String> PRONumberList = new List<String>();
            String cMdPRONumber = "select distinct top 10 MCode+'-'+MMV.Revision+' '+LEFT(RDT.RoutingDocumentType,1) as PRORefNo,PROH.ProductionOrderHeaderID from MFG_ProductionOrderHeader PROH JOIN MFG_SOPO_ProductionOrder SOPO ON SOPO.ProductionOrderHeaderID=PROH.ProductionOrderHeaderID AND SOPO.SOPOTypeID=2 AND SOPO.IsDeleted=0 JOIN MMT_MaterialMaster_Revision MMV ON MMV.MaterialMasterRevisionID=PROH.MaterialMasterRevisionID AND MMV.IsActive=1 AND MMV.IsDeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMV.MaterialMasterID JOIN MFG_RoutingHeader_Revision ROUV ON ROUV.RoutingHeaderRevisionID=PROH.RoutingHeaderRevisionID AND ROUV.IsDeleted=0 JOIN MFG_RoutingDocumentType RDT ON RDT.RoutingDocumentTypeID=ROUV.RoutingDocumentTypeID and RDT.RoutingDocumentTypeID=2  where PROH.IsDeleted=0 and PROH.IsActive=1 AND KitCode='" + KitCode + "'";
            IDataReader rsPRONumberList = DB.GetRS(cMdPRONumber);
            while (rsPRONumberList.Read())
            {
                PRONumberList.Add(String.Format("{0},{1}", rsPRONumberList["PRORefNo"], rsPRONumberList["ProductionOrderHeaderID"]));

            }
            rsPRONumberList.Close();

            return PRONumberList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetKitCodeListForInspectionCheckList(string Prefix)
        {
            List<string> KitCodeList = new List<string>();
            string cMdKitCodeList = "select distinct top 10  KitCode from MFG_ProductionOrderHeader where IsDeleted=0 and ProductionOrderStatusID!=1 AND ProductionOrderTypeID!=7 and KitCode like '" + Prefix + "%'";

            IDataReader rsKitCodeList = DB.GetRS(cMdKitCodeList);

            while (rsKitCodeList.Read())
            {
                KitCodeList.Add(string.Format("{0}", rsKitCodeList["KitCode"]));
            }
            rsKitCodeList.Close();
            return KitCodeList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetNCRefNoForReworkProcedureReport(String ProductionOrderHeaderID)
        {
            List<String> sid = new List<string>();
            String query = "SELECT DISTINCT RPC.NCReferenceNO AS NCRefNo FROM  MFG_ReworkProcedureCapture RPC JOIN MFG_RoutingDetailsActivity ROUDA ON ROUDA.RoutingDetailsActivityID=RPC.RoutingDetailsActivityID JOIN MFG_RoutingDetails ROUD ON ROUD.RoutingDetailsID=ROUDA.RoutingDetailsID JOIN MFG_RoutingHeader_Revision ROUR ON ROUR.RoutingHeaderID=ROUD.RoutingHeaderID JOIN MFG_ProductionOrderHeader PROH ON PROH.RoutingHeaderRevisionID=ROUR.RoutingHeaderRevisionID WHERE RPC.IsDeleted=0 and PROH.ProductionOrderHeaderID=" + ProductionOrderHeaderID + " AND RPC.ProductionOrderHeaderID=" + ProductionOrderHeaderID + " ORDER BY NCRefNo DESC";
            IDataReader rsfe = DB.GetRS(query);
            while (rsfe.Read())
            {

                sid.Add(String.Format("{0}", rsfe["NCRefNo"]));
            }
            rsfe.Close();
            return sid.ToArray();
        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetNCRefNoForNonConformanceReport(String ProductionOrderHeaderID)
        {
            List<String> sid = new List<string>();
            String query = "SELECT NCReferenceNumber AS NCRefNo FROM MFG_RoutingDetailsActivityCapture RDAC JOIN MFG_RoutingDetailsActivity RDA ON RDA.RoutingDetailsActivityID=RDAC.RoutingDetailsActivityID AND RDA.IsDeleted=0 JOIN MFG_RoutingDetails RD ON RD.RoutingDetailsID=RDA.RoutingDetailsID AND RD.IsDeleted=0 JOIN MFG_RoutingHeader_Revision RHR ON RHR.RoutingHeaderID=RD.RoutingHeaderID AND RHR.IsDeleted=0 JOIN MFG_ProductionOrderHeader POH ON POH.RoutingHeaderRevisionID=RHR.RoutingHeaderRevisionID AND RDAC.ProductionOrderHeaderID=POH.ProductionOrderHeaderID AND POH.IsDeleted=0 WHERE IsFailed=1 AND UserRoleID <>-1 AND RDAC.IsDeleted=0 AND POH.ProductionOrderHeaderID=" + ProductionOrderHeaderID + " ORDER BY NCReferenceNumber DESC";
            IDataReader rsfe = DB.GetRS(query);
            while (rsfe.Read())
            {
                sid.Add(String.Format("{0}", rsfe["NCRefNo"]));
            }
            rsfe.Close();
            return sid.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMCodesForMaterialAgeingReport(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "SELECT DISTINCT GMD.MaterialMasterID, MCODE FROM INV_GoodsMovementDetails GMD JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=GMD.MaterialMasterID WHERE GMD.GoodsMovementTypeID=1 AND GMD.IsDeleted=0 AND MTypeID=7 AND MCODE LIKE '%" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();


        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetReplenishedMaterialCode(String prefix, string TenantID)
        {
            List<String> mmList = new List<string>();

            String mmSql = "SELECT DISTINCT TOP 50 MM.MaterialMasterID, MCODE FROM MMT_MaterialMaster MM  LEFT OUTER JOIN TPL_Tenant_MaterialMaster TMM ON TMM.MaterialMasterID = MM.MaterialMasterID AND TMM.IsActive = 1 AND TMM.IsDeleted = 0 LEFT OUTER JOIN TPL_Tenant TNT ON TNT.TenantID = TMM.TenantID AND TNT.IsActive = 1 AND TNT.IsDeleted = 0 WHERE MM.IsActive = 1 AND MM.IsDeleted = 0  AND MCODE LIKE '%" + prefix + "%' AND TNT.AccountID = case when 0 = " + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end AND TNT.TenantID=" + TenantID;

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(String.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetTablesbasedOnObjects(String prefix)
        {
            List<String> mmList = new List<string>();
            //  String mmSql = "select * from sys.objects where object_id in( 73311571,1454628225,669349549)";
            //String mmSql = "SELECT DISTINCT top 50 GMD.MaterialMasterID, MCODE FROM INV_GoodsMovementDetails GMD JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=GMD.MaterialMasterID WHERE GMD.GoodsMovementTypeID=1 AND GMD.IsDeleted=0 AND MCODE LIKE '%" + prefix + "%'";
            String mmSql = "select TOP 25 * from sys.objects WHERE type = 'U' AND name LIKE '%" + prefix + "%' ORDER BY NAME";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(String.Format("{0},{1}", rsMCodeList["name"], rsMCodeList["object_id"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetReferenceNumbers(String prefix, int CategoryID)
        {
            List<String> mmList = new List<string>();
            String mmSql = "";
            if (CategoryID == 1)      //Inbound      
                mmSql = "SELECT TOP 10 InboundID ID, StoreRefNo Number FROM INB_Inbound WHERE IsActive=1 AND IsDeleted=0 AND StoreRefNo LIKE '%" + prefix + "%'";
            else if (CategoryID == 2)      //Outbound      
                mmSql = "SELECT TOP 10 OutboundID ID, OBDNumber Number FROM OBD_Outbound WHERE IsActive=1 AND IsDeleted=0 AND OBDNumber LIKE '%" + prefix + "%'";
            else if (CategoryID == 3)  // Internal Trransfer
                mmSql = "SELECT TOP 10 TransferRequestID ID, TransferRequestNumber Number  FROM INV_TransferRequest WHERE IsActive=1 AND IsDeleted=0 AND TransferRequestNumber LIKE '%" + prefix + "%'";
            else if (CategoryID == 4)   //Cycle counts
                mmSql = "SELECT TOP 10 CCM_CNF_AccountCycleCount_ID ID, dbo.UDF_ParseAndReturnLocaleString(AccountCycleCountName,'en')  Number FROM CCM_CNF_AccountCycleCounts WHERE IsActive=1 AND IsDeleted=0 AND dbo.UDF_ParseAndReturnLocaleString(AccountCycleCountName,'en') LIKE '%" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(String.Format("{0},{1}", rsMCodeList["Number"], rsMCodeList["ID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetJobOrderRefNoForMaterialDeficiencyReport(String prefix)
        {
            List<String> sid = new List<string>();
            String query = "EXEC [sp_RPT_ProductionOrderDeficiency_JobOrderRefNos]";
            IDataReader rsfe = DB.GetRS(query);
            while (rsfe.Read())
            {
                sid.Add(String.Format("{0},{1}", rsfe["JobOrderRefNo"], rsfe["ProductionOrderHeaderID"]));
            }
            rsfe.Close();
            return sid.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadUsersDataForOperatorPerformanceReport(string prefix)
        {

            List<string> userList = new List<string>();
            //string userSql = "select DISTINCT USRR.UserID,FirstName+ISNULL(' '+LastName,'') as Name from GEN_User_UserRole USRR JOIN GEN_User USR ON USR.UserID=USRR.UserID WHERE UserRoleID<0  AND  FirstName like '" + prefix + "%'";
            string userSql = "select DISTINCT USRR.UserID,FirstName+ISNULL(' '+LastName,'') as Name from GEN_User_UserRole USRR JOIN GEN_User USR ON USR.UserID = USRR.UserID WHERE UserRoleID<0  AND FirstName like '" + prefix + "%' AND USR.AccountID =case when 0 =" + cp.AccountID.ToString() + " then USR.AccountID else " + cp.AccountID.ToString() + " end";
            IDataReader rsUser = DB.GetRS(userSql);

            while (rsUser.Read())
            {
                userList.Add(string.Format("{0},{1}", rsUser["UserID"], rsUser["Name"]));
            }
            rsUser.Close();
            return userList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] Load_UsersData(string prefix)
        {

            List<string> userList = new List<string>();
            //string userSql = "select DISTINCT USRR.UserID,FirstName+ISNULL(' '+LastName,'') as Name from GEN_User_UserRole USRR JOIN GEN_User USR ON USR.UserID=USRR.UserID WHERE UserRoleID<0  AND  FirstName like '" + prefix + "%'";
            string userSql = "select DISTINCT UserID,FirstName from GEN_User USR AND FirstName like '" + prefix + "%' AND USR.AccountID =case when 0 =" + cp.AccountID.ToString() + " then USR.AccountID else " + cp.AccountID.ToString() + " end";
            IDataReader rsUser = DB.GetRS(userSql);

            while (rsUser.Read())
            {
                userList.Add(string.Format("{0},{1}", rsUser["UserID"], rsUser["FirstName"]));
            }
            rsUser.Close();
            return userList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadUserRolesForOperatorPerformanceReport(string prefix)
        {

            List<string> userList = new List<string>();
            string userSql = "select UserRoleID, UserRole from GEN_UserRole where UserRoleID in (-1,-2,-3) AND IsDeleted=0  AND  UserRole like '" + prefix + "%'";

            IDataReader rsUser = DB.GetRS(userSql);

            while (rsUser.Read())
            {
                userList.Add(string.Format("{0},{1}", rsUser["UserRoleID"], rsUser["UserRole"]));
            }
            rsUser.Close();
            return userList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadSupplierForSupplierPerformanceReport(string prefix, string TenantID)
        {

            List<string> supplierList = new List<string>();
            string supplierSql = "select distinct  Top 10 SupplierName+isnull('-'+SupplierCode,'') as SupplierName,SupplierID from MMT_Supplier MMS JOIN TPL_Tenant TNT ON TNT.TenantID = MMS.TenantID where MMS.ishidden = 0 and SupplierID> 0 AND MMS.IsActive = 1 AND MMS.IsDeleted = 0 AND SupplierName+isnull('-' + SupplierCode, '') like '%" + prefix + "%' AND TNT.AccountID = case when 0 = " + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end AND TNT.TenantID=" + TenantID + "  order by SupplierName";


            IDataReader rsSupplier = DB.GetRS(supplierSql);

            while (rsSupplier.Read())
            {
                supplierList.Add(string.Format("{0},{1}", rsSupplier["SupplierName"], rsSupplier["SupplierID"]));
            }
            rsSupplier.Close();
            return supplierList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadSONumbersForSalesOrderReport(String prefix, String TenentID, String CustomerID)
        {


            List<String> SOList = new List<String>();
            //String SOSql = "select top 10 SONumber,SOHeaderID from ORD_SOHeader where IsActive=1 and IsDeleted=0 and (0=" + (CustomerID != "" ? CustomerID : "0") + " or  CustomerID=" + CustomerID + ") and TenantID=" + TenentID + " and SONumber like '" + prefix + "%' order by SONumber";
            //String SOSql = "select top 10 SONumber,SOHeaderID from ORD_SOHeader where IsActive=1 and IsDeleted=0 and SONumber like '" + prefix + "%' order by SONumber";
            String SOSql = "select top 10 SONumber,SOHeaderID from ORD_SOHeader SO JOIN TPL_Tenant TNT ON TNT.TenantID = SO.TenantID AND TNT.IsActive = 1 AND TNT.IsDeleted = 0 where SO.IsActive = 1 and SO.IsDeleted = 0 and SONumber like '" + prefix + "%' AND TNT.AccountID = case when 0 = " + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end order by SONumber";
            IDataReader rsSO = DB.GetRS(SOSql);
            while (rsSO.Read())
            {
                SOList.Add(String.Format("{0},{1}", DB.RSField(rsSO, "SONumber"), DB.RSFieldInt(rsSO, "SOHeaderID")));

            }
            rsSO.Close();

            return SOList.ToArray();
        }
        //Load OBD Numbers
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadOBDNumbers(String prefix, int TenantId)
        {
            List<String> OBDList = new List<String>();
            String SOSql = "select top 10 OBDNumber,OutBoundID from OBD_Outbound OBD join TPL_Tenant TNT on TNT.TenantID=OBD.TenantID where OBD.IsActive=1 and OBD.IsDeleted=0 and OBD.DocumentTypeID <> 12  and TNT.AccountID=" + cp.AccountID + " and OBD.TenantID=" + TenantId + " and OBDNumber like '" + prefix + "%' order by OBDNumber";
            IDataReader rsSO = DB.GetRS(SOSql);
            while (rsSO.Read())
            {
                OBDList.Add(String.Format("{0},{1}", DB.RSField(rsSO, "OBDNumber"), DB.RSFieldInt(rsSO, "OutBoundID")));

            }
            rsSO.Close();

            return OBDList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPONumbersForPurchaseOrderReport(String prefix, String TenentID, String SupplierID)
        {
            List<String> POList = new List<String>();
            //String POSql = "select top 10 PONumber,POHeaderID from ORD_POHeader where  IsActive=1 and IsDeleted=0 and ( 0=" + SupplierID + " or SupplierID=" + SupplierID + ")  and  TenantID=" + TenentID + " and PONumber like '" + prefix + "%' order by PONumber";
            //String POSql = "select top 10 PONumber,POHeaderID from ORD_POHeader where  IsActive=1 and IsDeleted=0 and PONumber like '" + prefix + "%' order by PONumber";
            String POSql = "select top 10 PONumber,POHeaderID from ORD_POHeader poh join TPL_Tenant tnt on tnt.TenantID = poh.TenantID join GEN_Account acc on acc.AccountID = tnt.AccountID where poh.IsActive = 1 and poh.IsDeleted = 0 and acc.AccountID =" + cp.AccountID + " and PONumber like '" + prefix + "%' order by PONumber";
            IDataReader rsPO = DB.GetRS(POSql);
            while (rsPO.Read())
            {
                POList.Add(String.Format("{0},{1}", rsPO["PONumber"], rsPO["POHeaderID"]));
            }
            rsPO.Close();
            return POList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadItemNumbersForBinReplenishmentReport(String prefix)
        {
            List<String> ItemList = new List<String>();
            //String POSql = "select top 10 PONumber,POHeaderID from ORD_POHeader where  IsActive=1 and IsDeleted=0 and ( 0=" + SupplierID + " or SupplierID=" + SupplierID + ")  and  TenantID=" + TenentID + " and PONumber like '" + prefix + "%' order by PONumber";
            //String ItemSql = "select DISTINCT  MM.MCode, MM.MaterialMasterID FROM MMT_MaterialMaster MM JOIN MMT_BinReplenishmentPlan MMTB ON MMTB.MaterialMasterID = MM.MaterialMasterID JOIN TPL_Tenant_MaterialMaster TMM ON TMM.MaterialMasterID = MMTB.MaterialMasterID AND TMM.IsActive = 1 AND TMM.IsDeleted = 0 ";
            String ItemSql = "select DISTINCT TOP 50 MM.MCode, MM.MaterialMasterID FROM MMT_MaterialMaster MM JOIN TPL_Tenant_MaterialMaster TMM ON TMM.MaterialMasterID = MM.MaterialMasterID AND MM.TenantID=TMM.TenantID AND TMM.IsActive = 1 AND TMM.IsDeleted = 0 AND MM.IsActive=1 AND MM.IsDeleted=0 LEFT JOIN MMT_BinReplenishmentPlan MMTB ON MMTB.MaterialMasterID = MM.MaterialMasterID JOIN TPL_Tenant TNT ON TNT.TenantID = TMM.TenantID AND TNT.IsActive = 1 AND TNT.IsDeleted = 0 where MM.IsDeleted = 0 and MM.IsActive = 1 AND MCode like '" + prefix + "%' AND TNT.AccountID = case when 0 = " + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end";
            IDataReader rsItem = DB.GetRS(ItemSql);
            while (rsItem.Read())
            {
                ItemList.Add(String.Format("{0},{1}", rsItem["MCode"], rsItem["MaterialMasterID"]));
            }
            rsItem.Close();
            return ItemList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadItemNumbersForBinReplenishmentReportNew(String prefix,String TenantId)
        {
            List<String> ItemList = new List<String>();
            //String POSql = "select top 10 PONumber,POHeaderID from ORD_POHeader where  IsActive=1 and IsDeleted=0 and ( 0=" + SupplierID + " or SupplierID=" + SupplierID + ")  and  TenantID=" + TenentID + " and PONumber like '" + prefix + "%' order by PONumber";
            //String ItemSql = "select DISTINCT  MM.MCode, MM.MaterialMasterID FROM MMT_MaterialMaster MM JOIN MMT_BinReplenishmentPlan MMTB ON MMTB.MaterialMasterID = MM.MaterialMasterID JOIN TPL_Tenant_MaterialMaster TMM ON TMM.MaterialMasterID = MMTB.MaterialMasterID AND TMM.IsActive = 1 AND TMM.IsDeleted = 0 ";
            String ItemSql = "select DISTINCT TOP 10 MM.MCode, MM.MaterialMasterID FROM MMT_MaterialMaster MM JOIN TPL_Tenant_MaterialMaster TMM ON TMM.MaterialMasterID = MM.MaterialMasterID AND MM.TenantID=TMM.TenantID AND TMM.IsActive = 1 AND TMM.IsDeleted = 0 AND MM.IsActive=1 AND MM.IsDeleted=0 LEFT JOIN MMT_BinReplenishmentPlan MMTB ON MMTB.MaterialMasterID = MM.MaterialMasterID JOIN TPL_Tenant TNT ON TNT.TenantID = TMM.TenantID AND TNT.IsActive = 1 AND TNT.IsDeleted = 0 where MM.IsDeleted = 0 and MM.IsActive = 1 AND MCode like '" + prefix + "%' AND TNT.TenantID="+TenantId+"  AND TNT.AccountID = case when 0 = " + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end";
            IDataReader rsItem = DB.GetRS(ItemSql);
            while (rsItem.Read())
            {
                ItemList.Add(String.Format("{0},{1}", rsItem["MCode"], rsItem["MaterialMasterID"]));
            }
            rsItem.Close();
            return ItemList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPONumbersForInwardQCReport(String prefix, String TenentID, String SupplierID)
        {
            List<String> POList = new List<String>();
            //String POSql = "select top 10 PONumber,POHeaderID from ORD_POHeader where  IsActive=1 and IsDeleted=0 and ( 0=" + SupplierID + " or SupplierID=" + SupplierID + ")  and  TenantID=" + TenentID + " and PONumber like '" + prefix + "%' order by PONumber";
            String POSql = "  select distinct top 10 PONumber,poh.POHeaderID from ORD_POHeader poh JOIN ORD_PODetails POD ON POD.POHeaderID = POH.POHeaderID JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID = POD.MaterialMasterID JOIN MMT_MaterialMaster_QualityParameters MMQ ON MMQ.MaterialMasterID = MM.MaterialMasterID join TPL_Tenant tnt on tnt.TenantID = poh.TenantID join GEN_Account acc on acc.AccountID = tnt.AccountID where poh.IsActive = 1 and poh.IsDeleted = 0 and acc.AccountID =" + cp.AccountID + " and PONumber like '" + prefix + "%' order by PONumber";
            IDataReader rsPO = DB.GetRS(POSql);
            while (rsPO.Read())
            {
                POList.Add(String.Format("{0},{1}", rsPO["PONumber"], rsPO["POHeaderID"]));
            }
            rsPO.Close();

            return POList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetKitCodeListForMaterialDeficiencyReport(string Prefix)
        {
            List<string> KitCodeList = new List<string>();
            string cMdKitCodeList = "select distinct top 10  KitCode from MFG_ProductionOrderHeader where IsDeleted=0  AND ProductionOrderTypeID!=7 and KitCode like '" + Prefix + "%'";

            IDataReader rsKitCodeList = DB.GetRS(cMdKitCodeList);

            while (rsKitCodeList.Read())
            {
                KitCodeList.Add(string.Format("{0}", rsKitCodeList["KitCode"]));
            }
            rsKitCodeList.Close();
            return KitCodeList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadJobOrderRefNoforMaterialDeficiencyReport(String KitCode)
        {
            List<String> PRONumberList = new List<String>();
            String cMdPRONumber = "select distinct top 10 MCode+'-'+MMV.Revision+' '+LEFT(RDT.RoutingDocumentType,1) as PRORefNo,PROH.ProductionOrderHeaderID from MFG_ProductionOrderHeader PROH JOIN MMT_MaterialMaster_Revision MMV ON MMV.MaterialMasterRevisionID=PROH.MaterialMasterRevisionID AND MMV.IsActive=1 AND MMV.IsDeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMV.MaterialMasterID JOIN MFG_RoutingHeader_Revision ROUV ON ROUV.RoutingHeaderRevisionID=PROH.RoutingHeaderRevisionID AND ROUV.IsDeleted=0 JOIN MFG_RoutingDocumentType RDT ON RDT.RoutingDocumentTypeID=ROUV.RoutingDocumentTypeID where PROH.IsDeleted=0 and PROH.IsActive=1 AND KitCode='" + KitCode + "'";
            IDataReader rsPRONumberList = DB.GetRS(cMdPRONumber);
            while (rsPRONumberList.Read())
            {
                PRONumberList.Add(String.Format("{0},{1}", rsPRONumberList["PRORefNo"], rsPRONumberList["ProductionOrderHeaderID"]));

            }
            rsPRONumberList.Close();

            return PRONumberList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadTenantsForReports(string prefix)
        {

            List<string> userList = new List<string>();
            string userSql = "SELECT TenantID, TenantName FROM TPL_Tenant WHERE IsActive=1 AND IsDeleted=0 AND TenantID > 0 AND TenantName like '" + prefix + "%' AND AccountID = case when 0 = " + cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end";
            userSql = userSql + " AND TenantID = case when  " + cp.TenantID.ToString() + "=0  then TenantID else " + cp.TenantID.ToString() + " end  order by TenantName";

            IDataReader rsUser = DB.GetRS(userSql);

            while (rsUser.Read())
            {
                userList.Add(string.Format("{0},{1}", rsUser["TenantID"], rsUser["TenantName"]));
            }
            rsUser.Close();
            return userList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMTypeDataForReports(string prefix, string TenantID)
        {

            List<string> MTypeList = new List<string>();
            string mTypeSql = "select top 20 MTypeID,MType,Description from MMT_MType where IsActive = 1 AND MType like '" + prefix + "%'" + "AND TenantID=" + TenantID;

            IDataReader rsMType = DB.GetRS(mTypeSql);

            while (rsMType.Read())
            {
                MTypeList.Add(string.Format("{0},{1}", rsMType["MType"] + "-" + rsMType["Description"], rsMType["MTypeID"]));
            }
            rsMType.Close();

            return MTypeList.ToArray();
        }


        #endregion     -----------------   Developed By Subrahmanyam   -------------------------

        #region     -----------------  Developed by Devi  -------------------------

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetKitcode(string Prefix)
        {
            List<string> KitcodeList = new List<string>();
            string cMdKitcodesql = "SELECT ProductionOrderHeaderID,KitCode FROM MFG_ProductionOrderHeader WHERE IsActive=1 AND IsDeleted=0 and ProductionOrderTypeID=7 and ProductionOrderStatusID=2  and KitCode like '" + Prefix + "%' ";

            IDataReader drkitcode = DB.GetRS(cMdKitcodesql);

            while (drkitcode.Read())
            {
                KitcodeList.Add(string.Format("{0},{1}", drkitcode["KitCode"], drkitcode["ProductionOrderHeaderID"]));
            }
            drkitcode.Close();
            return KitcodeList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetLinenoforjob(string ProductionOrderHeaderID)
        {
            List<string> joiblinenolist = new List<string>();
            string cMdjoiblineno = "select distinct LineNumber from MFG_SOPO_ProductionOrder SOPO JOIN ORD_SODetails SOD ON SOD.SOHeaderID=SOPO.SOPOHeaderID  where ProductionOrderHeaderID=" + ProductionOrderHeaderID;

            IDataReader drcMdjoiblineno = DB.GetRS(cMdjoiblineno);

            while (drcMdjoiblineno.Read())
            {
                joiblinenolist.Add(string.Format("{0}", drcMdjoiblineno["LineNumber"]));
            }
            drcMdjoiblineno.Close();
            return joiblinenolist.ToArray();
        }






        #endregion     -----------------   Developed by Devi    -------------------------

        #region     -----------------  Doveloped By Swamy For CycleCount -------------------------
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetMcodeForCycleCount(string Prefix)
        {
            List<string> mmList = new List<string>();
            string mmSql = "select MM.MCode+isnull( ' ` '+ OEMPartNo,'')  AS MCode,CCD.MaterialMasterID from QCC_CycleCountDetails CCD " +
                            "JOIN QCC_CycleCount CC ON CC.CycleCountID=CCD.CycleCountID AND CC.IsActive=1 AND CC.IsDeleted=0 " +
                            "JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=CCD.MaterialMasterID " +
                            "WHERE CC.IsOn=1 AND CCD.IsActive=1 AND CCD.IsDeleted=0 AND (MCode LIKE '" + Prefix + "%' OR  OEMPartNo like '" + Prefix + "%' ) order by MCode";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetCCID(string MMID)
        {
            List<string> LocationList = new List<string>();
            string cMdLocationList = "SELECT CC.CycleCountID from QCC_CycleCountDetails CCD " +
                                    "JOIN QCC_CycleCount CC ON CC.CycleCountID=CCD.CycleCountID AND CC.IsActive=1 AND CC.IsDeleted=0" +
                                    "WHERE CC.IsOn=1 AND CCD.IsActive=1 AND CCD.IsDeleted=0 AND CCD.MaterialMasterID=" + MMID;


            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0}", rslocationList["CycleCountID"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetLocationListINCC(string Prefix)
        {
            List<string> LocationList = new List<string>();
            //string cMdLocation = "SELECT Location,LocationID FROM INV_Location WHERE IsActive=1 AND IsDeleted=0 AND Location LIKE '"+Prefix+"%'";
            string cMdLocation = "SELECT TOP 10  Location,LocationID FROM INV_Location INVL LEFT OUTER JOIN TPL_Tenant TNT ON TNT.TenantID = INVL.TenantID  where INVL.IsActive = 1 AND Location LIKE '" + Prefix + "%' AND TNT.AccountID = case when 0 = " + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end";

            IDataReader rsLocation = DB.GetRS(cMdLocation);

            while (rsLocation.Read())
            {
                LocationList.Add(string.Format("{0},{1}", rsLocation["Location"], rsLocation["LocationID"]));
            }
            rsLocation.Close();
            return LocationList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetRPTLocationListINCC(string Prefix)
        {
            List<string> LocationList = new List<string>();
            //string cMdLocation = "SELECT Location,LocationID FROM INV_Location WHERE IsActive=1 AND IsDeleted=0 AND Location LIKE '"+Prefix+"%'";
            string cMdLocation = "SELECT TOP 10 Location,LocationID FROM INV_Location INVL LEFT OUTER JOIN TPL_Tenant TNT ON TNT.TenantID = INVL.TenantID  where INVL.IsActive = 1 AND Location LIKE '" + Prefix + "%' AND TNT.AccountID = case when 0 = " + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end";

            IDataReader rsLocation = DB.GetRS(cMdLocation);

            while (rsLocation.Read())
            {
                LocationList.Add(string.Format("{0},{1}", rsLocation["Location"], rsLocation["LocationID"]));
            }
            rsLocation.Close();
            return LocationList.ToArray();
        }
        #endregion  -----------------  Doveloped By Swamy For CycleCount -------------------------

        #region     -----------------  Doveloped By Swamy For Dojo -------------------------
        public class Employee
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Employee> TetsForAdaptor()
        {
            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee() { FirstName = "Swamy", LastName = "Pasem" });
            employees.Add(new Employee() { FirstName = "Ram", LastName = "K" });
            employees.Add(new Employee() { FirstName = "Prasad", LastName = "G" });
            return employees;
        }
        #endregion  -----------------  Doveloped By Swamy For Dojo -------------------------


        #region     -----------------  3PL Billing -------------------------

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadSupplierDataFor3PL(string prefix, string TenantID, string Type)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<string> supplierList = new List<string>();

            if (TenantID == "")
                TenantID = "0";

            string supplierSql = "";

            if (Type == "PO")
            {
                //supplierSql = "SELECT distinct TOP 10 SupplierName,SUP.SupplierID FROM MMT_Supplier SUP LEFT JOIN TPL_Tenant_Supplier TNT_S ON TNT_S.SupplierID=SUP.SupplierID AND TNT_S.IsActive=1 AND TNT_S.IsDeleted=0 WHERE SUP.IsActive=1 AND SUP.IsDeleted=0 AND SUP.SupplierID>0 AND TNT_S.TenantID=" + TenantID + " AND  SupplierName like '" + prefix + "%' ";
                //supplierSql = supplierSql + " AND TNT_S.TenantID = case when 0 = " + cp.TenantID.ToString() + " then TNT_S.TenantID else " + cp.TenantID.ToString() + " end  order by SupplierName";

                //supplierSql = "SELECT distinct TOP 10 SupplierName+isnull('-'+SupplierCode,'') AS SupplierName,SUP.SupplierID FROM MMT_Supplier SUP JOIN TPL_Tenant_Supplier TNT_S ON TNT_S.SupplierID = SUP.SupplierID AND TNT_S.IsActive = 1 AND TNT_S.IsDeleted = 0 JOIN TPL_Tenant TNT ON TNT.TenantID = TNT_S.TenantID WHERE SUP.IsActive = 1 AND SUP.IsDeleted = 0 AND SUP.SupplierID > 0 AND TNT_S.TenantID=" + TenantID + "  AND  SupplierName+isnull('-'+SupplierCode,'') like '%" + prefix + "%'";
                //supplierSql = supplierSql + "AND TNT.AccountID = case when 0 =" + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end  AND TNT.TenantID = case when  " + cp.TenantID.ToString() + "=0  then TNT.TenantID else " + cp.TenantID.ToString() + " end order by SupplierName";

                supplierSql = "[USP_LoadSupplierDataPOtype] @TenantID=" + TenantID + ",@AccountID=" + cp.AccountID.ToString() + ",@prefix='" + prefix + "',@LogTenantID=" + cp.TenantID.ToString() + "";

            }

            if (Type == "Supplier")
            {
                string AccTenantId = TenantID == "0" ? (cp.TenantID.ToString() == "0" ? "SUP.TenantID" : cp.TenantID.ToString()) : TenantID;

                //supplierSql = "SELECT distinct TOP 10 SupplierName,SUP.SupplierID FROM MMT_Supplier SUP LEFT JOIN TPL_Tenant_Supplier TNT_S ON TNT_S.SupplierID=SUP.SupplierID AND TNT_S.IsActive=1 AND TNT_S.IsDeleted=0 WHERE SUP.IsActive=1 AND SUP.IsDeleted=0 AND SUP.SupplierID>0 AND ( 0=" + TenantID + " OR TNT_S.TenantID=" + TenantID + " ) AND  SupplierName like '" + prefix + "%'";
                //supplierSql = supplierSql + " AND SUP.TenantID ="+ AccTenantId + " order by SupplierName";

                //supplierSql = "SELECT distinct  SupplierName,SUP.SupplierID FROM MMT_Supplier SUP JOIN TPL_Tenant_Supplier TNT_S ON TNT_S.SupplierID = SUP.SupplierID AND TNT_S.IsActive = 1 AND TNT_S.IsDeleted = 0 JOIN TPL_Tenant TNT ON TNT.TenantID = TNT_S.TenantID WHERE SUP.IsActive = 1 AND SUP.IsDeleted = 0 AND SUP.SupplierID > 0 AND (0 = 0 OR TNT_S.TenantID = 0) AND SupplierName like '" + prefix + "%'";
                //supplierSql = supplierSql + "AND TNT.TenantID = " + AccTenantId + " AND TNT.AccountID = case when 0 =" + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end order by SupplierName";

                //supplierSql = "SELECT SUP.SupplierID, SUP.SupplierName FROM TPL_Tenant_Supplier TSUP INNER JOIN MMT_Supplier SUP ON SUP.SupplierID = TSUP.SupplierID AND TSUP.IsActive = 1 AND TSUP.IsDeleted = 0 AND SUP.IsActive = 1 AND SUP.IsDeleted = 0 INNER JOIN TPL_Tenant TNT  ON TNT.TenantID = TSUP.TenantID AND TNT.IsActive = 1 AND TNT.IsDeleted = 0 WHERE TNT.TenantID = "+TenantID+" AND TNT.AccountID = "+cp.AccountID+ " and SupplierName like '" + prefix + "%' ORDER BY SUP.SupplierName ASC";


                //<!------------------Procedure converting--------------->
                // supplierSql = "SELECT SUP.SupplierID, SUP.SupplierName+isnull('-'+SUP.SupplierCode,'') AS SupplierName FROM TPL_Tenant_Supplier TSUP INNER JOIN MMT_Supplier SUP ON SUP.SupplierID = TSUP.SupplierID AND TSUP.IsActive = 1 AND TSUP.IsDeleted = 0 AND SUP.IsActive = 1 AND SUP.IsDeleted = 0 WHERE TSUP.TenantID = " + TenantID + " AND SUP.SupplierName+isnull('-'+SUP.SupplierCode,'') like '%" + prefix + "%' ORDER BY SUP.SupplierName ASC";
                supplierSql = "Exec [dbo].[USP_MST_DropSupplierTenantWise] @prefix='" + prefix + "',@TenantID=" + TenantID + ",@AccountID=" + cp.AccountID.ToString() + "";
                // supplierSql = "SELECT distinct TOP 10 SupplierName,SUP.SupplierID FROM MMT_Supplier SUP JOIN TPL_Tenant_Supplier TNT_S ON TNT_S.SupplierID = SUP.SupplierID and TNT_S.tenantID=SUP.tenantID AND TNT_S.IsActive = 1 AND TNT_S.IsDeleted = 0 JOIN TPL_Tenant TNT ON TNT.TenantID = TNT_S.TenantID WHERE SUP.IsActive = 1 AND SUP.IsDeleted = 0 AND SUP.SupplierID > 0  AND SupplierName like '" + prefix + "%' and SUP.TenantID="+TenantID+" and TNT.AccountID="+cp.AccountID+"";
                // supplierSql = "SELECT distinct TOP 10 SupplierName,SUP.SupplierID,sup.TenantID FROM MMT_Supplier SUP Left outer JOIN TPL_Tenant_Supplier TNT on TNT.TenantID = SUP.TenantID and TNT.SupplierID = SUP.SupplierID join TPL_Tenant TT on TT.TenantID = TNT.TenantID where (SUP.TenantID = 0 OR SUP.TenantID = "+TenantID+") AND (TNT.AccountID="+cp.AccountID+") and SUP.IsActive = 1 AND SUP.IsDeleted = 0 AND SUP.SupplierID > 0  AND SupplierName like '" + prefix + "%'";
            }

            IDataReader rsSupplier = DB.GetRS(supplierSql);

            while (rsSupplier.Read())
            {
                supplierList.Add(string.Format("{0},{1}", rsSupplier["SupplierName"], rsSupplier["SupplierID"]));

            }
            rsSupplier.Close();
            return supplierList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadTenantDataFor3PL(string prefix)
        {

            List<string> TenantList = new List<string>();
            //<!------------------Procedure Converting------------->
            //   string TenantListSql = "SELECT TOP 10 TenantID,TenantName+isnull('-'+TenantCode,'') AS TenantName FROM TPL_Tenant WHERE IsActive=1 AND IsDeleted=0 AND TenantID<>0 AND  TenantName+isnull('-'+TenantCode,'') like '%" + prefix + "%' AND AccountID = case when 0 =" + cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end";

            //Account and Tenant filteration

            //  TenantListSql = TenantListSql + " AND TenantID = case when  " + cp.TenantID.ToString() + "=0  then TenantID else " + cp.TenantID.ToString() + " end  order by TenantName";



            string TenantListSql = "Exec [dbo].[USP_MST_DropTenantAccountWise] @prefix='" + prefix + "', @AccountID=" + cp.AccountID.ToString() + ",@TenantID=" + cp.TenantID.ToString() + "";


            IDataReader rsTenantList = DB.GetRS(TenantListSql);

            while (rsTenantList.Read())
            {
                TenantList.Add(string.Format("{0},{1}", rsTenantList["TenantName"], rsTenantList["TenantID"]));
            }
            rsTenantList.Close();
            return TenantList.ToArray();
        }
       

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadTenantsDataFor3PL(string prefix, int Accountid)
        {

            List<string> TenantList = new List<string>();
            //<!------------------Procedure Converting------------->
            //   string TenantListSql = "SELECT TOP 10 TenantID,TenantName+isnull('-'+TenantCode,'') AS TenantName FROM TPL_Tenant WHERE IsActive=1 AND IsDeleted=0 AND TenantID<>0 AND  TenantName+isnull('-'+TenantCode,'') like '%" + prefix + "%' AND AccountID = case when 0 =" + cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end";

            //Account and Tenant filteration

            //  TenantListSql = TenantListSql + " AND TenantID = case when  " + cp.TenantID.ToString() + "=0  then TenantID else " + cp.TenantID.ToString() + " end  order by TenantName";



            string TenantListSql = "Exec [dbo].[USP_MST_DropTenantAccountWise] @prefix='" + prefix + "', @AccountID=" + cp.AccountID.ToString() + ",@TenantID=" + cp.TenantID.ToString() + "";


            IDataReader rsTenantList = DB.GetRS(TenantListSql);

            while (rsTenantList.Read())
            {
                TenantList.Add(string.Format("{0},{1}", rsTenantList["TenantName"], rsTenantList["TenantID"]));
            }
            rsTenantList.Close();
            return TenantList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadTenantsByWH(string prefix, int whid)
        {

            List<string> TenantList = new List<string>();
           
            string TenantListSql = "EXEC [dbo].[SP_RPT_TENANTDROPDOWN] @prefix='" + prefix + "',@whid=" + whid;


            IDataReader rsTenantList = DB.GetRS(TenantListSql);

            while (rsTenantList.Read())
            {
                TenantList.Add(string.Format("{0},{1}", rsTenantList["TenantCode"], rsTenantList["TenantID"]));
            }
            rsTenantList.Close();
            return TenantList.ToArray();
        }


        //=============== for getting inbound status====================== //
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetInboundStatus(string prefix)
        {

            List<string> TenantList = new List<string>();

            string TenantListSql = "select top 10 InboundStatusID,InboundStatus from INB_InboundStatus where IsActive = 1 and IsDeleted = 0 and InboundStatus like '%" + prefix + "%'";


            IDataReader rsTenantList = DB.GetRS(TenantListSql);

            while (rsTenantList.Read())
            {
                TenantList.Add(string.Format("{0},{1}", rsTenantList["InboundStatus"], rsTenantList["InboundStatusID"]));
            }
            rsTenantList.Close();
            return TenantList.ToArray();
        }



          [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetShipmentTypeForInbound(string prefix)
        {

            List<string> TenantList = new List<string>();

            string TenantListSql = "select top 10 ShipmentTypeID,ShipmentType from GEN_ShipmentType where IsActive = 1 and IsDeleted = 0 and ShipmentType like '%" + prefix + "%'";


            IDataReader rsTenantList = DB.GetRS(TenantListSql);

            while (rsTenantList.Read())
            {
                TenantList.Add(string.Format("{0},{1}", rsTenantList["ShipmentType"], rsTenantList["ShipmentTypeID"]));
            }
            rsTenantList.Close();
            return TenantList.ToArray();
        }





        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadTenantDataFor3PLSO(string prefix)
        {

            List<string> TenantList = new List<string>();

            string TenantListSql = "SELECT TOP 10 TenantID,TenantName+isnull('-'+TenantCode,'') AS TenantName FROM TPL_Tenant WHERE IsActive=1 AND IsDeleted=0 AND TenantID<>0 AND  TenantName+isnull('-'+TenantCode,'') like '%" + prefix + "%' AND AccountID = case when 0 =" + cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end";

            //Account and Tenant filteration

            TenantListSql = TenantListSql + " AND TenantID = case when  " + cp.TenantID.ToString() + "=0  then TenantID else " + cp.TenantID.ToString() + " end  order by TenantName";

            IDataReader rsTenantList = DB.GetRS(TenantListSql);

            while (rsTenantList.Read())
            {
                TenantList.Add(string.Format("{0},{1}", rsTenantList["TenantName"], rsTenantList["TenantID"]));
            }
            rsTenantList.Close();
            return TenantList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadTenantDataFor3PLByAccount(string prefix, int AccountID)
        {

            List<string> TenantList = new List<string>();

            string TenantListSql = "SELECT TOP 10 TenantID,TenantName,TenantCode FROM TPL_Tenant WHERE IsActive=1 AND IsDeleted=0 AND TenantID<>0 AND  TenantName like '%" + prefix + "%' AND AccountID = case when 0 =" + cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end";

            //Account and Tenant filteration

            TenantListSql = TenantListSql + " AND TenantID = case when  " + cp.TenantID.ToString() + "=0  then TenantID else " + cp.TenantID.ToString() + " end  order by TenantName";

            IDataReader rsTenantList = DB.GetRS(TenantListSql);

            while (rsTenantList.Read())
            {
                TenantList.Add(string.Format("{0},{1}", rsTenantList["TenantName"], rsTenantList["TenantID"]));
            }
            rsTenantList.Close();
            return TenantList.ToArray();
        }
        //For dock
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadDockFor3PL(string prefix)
        {

            List<string> DockList = new List<string>();
            //string DockListSql = "SELECT TOP 10 DockID,DockNo FROM GEN_Dock WHERE IsActive=1 AND IsDeleted=0 and DockNo like '" + prefix + "%'  order by DockNo";
            //string DockListSql = "SELECT DockID, DockNo, DockName, DockLocation FROM GEN_Dock DOCK WHERE(DOCK.DockTypeID = 1 OR DOCK.DockTypeID = 3) AND WarehouseID = " + prefix + "  AND DockID NOT IN(SELECT DockID FROM INB_Inbound_Dock INBDOCK JOIN INB_Inbound INB ON INB.InboundID= INBDOCK.InboundID WHERE ( INB.InboundStatusID >= 3 AND INB.InboundStatusID < 26) AND INB.IsActive = 1 AND INB.IsDeleted = 0 AND INBDOCK.IsActive = 1 AND INBDOCK.IsDeleted = 0)";
            string DockListSql = "Exec [SP_GetDocks] @IsOutbound=0,@Warehouseid=" + prefix;

            IDataReader rsDockList = DB.GetRS(DockListSql);

            while (rsDockList.Read())
            {
                DockList.Add(string.Format("{0},{1}", rsDockList["DockNo"], rsDockList["DockID"]));
            }
            rsDockList.Close();
            return DockList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadOutboundDocks(string prefix)
        {

            List<string> DockList = new List<string>();
            //string DockListSql = "SELECT TOP 10 DockID,DockNo FROM GEN_Dock WHERE IsActive=1 AND IsDeleted=0 and DockNo like '" + prefix + "%'  order by DockNo";
            //string DockListSql = "SELECT DockID, DockNo, DockName, DockLocation FROM GEN_Dock DOCK WHERE(DOCK.DockTypeID = 1 OR DOCK.DockTypeID = 3) AND WarehouseID = " + prefix + "  AND DockID NOT IN(SELECT DockID FROM INB_Inbound_Dock INBDOCK JOIN INB_Inbound INB ON INB.InboundID= INBDOCK.InboundID WHERE ( INB.InboundStatusID >= 3 AND INB.InboundStatusID < 26) AND INB.IsActive = 1 AND INB.IsDeleted = 0 AND INBDOCK.IsActive = 1 AND INBDOCK.IsDeleted = 0)";
            string DockListSql = "Exec [SP_GetDocks] @IsOutbound=1,@Warehouseid=" + prefix;

            IDataReader rsDockList = DB.GetRS(DockListSql);

            while (rsDockList.Read())
            {
                DockList.Add(string.Format("{0},{1}", rsDockList["DockNo"], rsDockList["DockID"]));
            }
            rsDockList.Close();
            return DockList.ToArray();
        }



        //For Account
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadAccountDataFor3PL(string prefix)
        {

            List<string> AccList = new List<string>();
            string AccListSql = "SELECT TOP 10 AccountID,Account+isnull('-'+AccountCode,'') AS Account  FROM GEN_Account WHERE IsActive=1 AND IsDeleted=0 AND AccountID<>0 AND AccountID = case when 0 =" + cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end AND  Account+isnull('-'+AccountCode,'') like '%" + prefix + "%'  order by Account";
            //  string TenantListSql = "SELECT TOP 10 TenantID,TenantName+isnull('-'+TenantCode,'') AS TenantName FROM TPL_Tenant WHERE IsActive=1 AND IsDeleted=0 AND TenantID<>0 AND  TenantName+isnull('-'+TenantCode,'') like '%" + prefix + "%' AND AccountID = case when 0 =" + cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end";


            IDataReader rsAccList = DB.GetRS(AccListSql);

            while (rsAccList.Read())
            {
                AccList.Add(string.Format("{0},{1}", rsAccList["Account"], rsAccList["AccountID"]));
            }
            rsAccList.Close();
            return AccList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadTenantDataFor3PLandCheckSpaceutilization(string prefix, string MID)
        {

            List<string> TenantList = new List<string>();
            string TenantListSql = "SELECT TOP 10 TT.TenantName,SpaceUtilizationID,	"
                                        + " case when TTS.SpaceUtilizationID=4  "
                                        + "      then convert(nvarchar,TT.TenantID)+'²0' "
                                        + "	else "
                                        + "	    case when MM.MLength is not null AND MM.MHeight is not null AND MM.MWidth is not null AND MM.MWeight is not null   "
                                        + "      then"
                                        + "          convert(nvarchar,TT.TenantID)+'²0'"
                                        + "      else"
                                        + "          convert(nvarchar,TT.TenantID)+'²1' "
                                        + "  	end"
                                        + "	end [TenantID] FROM TPL_Tenant TT "
                                        + "  join TPL_Tenant_SpaceUtilization TTS on TT.TenantID=TTS.TenantID and TTS.IsActive=1 and TTS.IsDeleted=0 "
                                        + "  ,MMT_MaterialMaster mm "
                                        + "  WHERE TT.IsActive=1 AND TT.IsDeleted=0 AND TT.TenantID<>0 and mm.MaterialMasterID=" + MID
                                        + " and TT.TenantName like '" + prefix + "%'  order by TenantName";


            IDataReader rsTenantList = DB.GetRS(TenantListSql);

            while (rsTenantList.Read())
            {
                TenantList.Add(string.Format("{0},{1}", rsTenantList["TenantName"], rsTenantList["TenantID"]));
            }
            rsTenantList.Close();
            return TenantList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPONumbersfor3PL(String prefix, String TenantID, String StatusID)
        {
            List<String> POList = new List<String>();

            if (TenantID == "")
                TenantID = "0";

            String POSql = "select top 10 PONumber,POHeaderID from ORD_POHeader ORD inner join TPL_Tenant TNT on ORD.TenantID = TNT.TenantID and TNT.isactive = 1 and TNT.IsDeleted = 0 where  ORD.IsActive=1 and ORD.IsDeleted=0 and ( 0=" + StatusID + " or POStatusID=" + StatusID + ")  and  (0=" + TenantID + " OR TNT.TenantID=" + TenantID + ") and PONumber like '" + prefix + "%'";

            POSql = POSql + " AND TNT.TenantID = case when 0 = " + cp.TenantID.ToString() + " then TNT.TenantID else  " + cp.TenantID.ToString() + " end AND TNT.AccountID = " + cp.AccountID.ToString() + "   order by PONumber";

            IDataReader rsPO = DB.GetRS(POSql);
            while (rsPO.Read())
            {
                POList.Add(String.Format("{0},{1}", rsPO["PONumber"], rsPO["POHeaderID"]));
            }
            rsPO.Close();

            return POList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMMSupplierDataFor3PL(string prefix, string TenantID)
        {

            List<string> supplierList = new List<string>();

            //string TenantIDs = DB.GetSqlS("select stuff((select ', '+convert(nvarchar, TenantID) from TPL_Tenant_MaterialMaster where MaterialMasterID=" + MaterialMasterID + " AND IsActive=1 AND IsDeleted=0 for xml path('')),1,1, '') S");
            string supplierSql = "SELECT DISTINCT TOP 10 SupplierName,TNT_S.SupplierID  FROM TPL_Tenant_Supplier TNT_S JOIN MMT_Supplier SUP ON SUP.SupplierID=TNT_S.SupplierID AND SUP.IsActive=1 AND SUP.IsDeleted=0 WHERE  TNT_S.IsDeleted=0 AND TNT_S.TenantID=" + DB.SQuote(TenantID) + " AND  SupplierName like '" + prefix + "%'  order by SupplierName";

            //string supplierSql = "select distinct  Top 30 SupplierName,SupplierCode,SupplierID from MMT_Supplier  where IsActive=1 AND IsDeleted=0  AND SupplierID>0 AND  SupplierName like '" + prefix + "%'  order by SupplierName";

            IDataReader rsSupplier = DB.GetRS(supplierSql);

            while (rsSupplier.Read())
            {
                supplierList.Add(string.Format("{0},{1}", rsSupplier["SupplierName"], rsSupplier["SupplierID"]));
            }
            rsSupplier.Close();
            return supplierList.ToArray();
        }
        //load parent menus
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] Getparentmenu(string prefix)
        {
            List<string> WorkCenterList = new List<string>();
            string cMdWorkCentersql = "  SELECT Menu.MenuID as ID, Menu.MenuText as Value FROM GEN_MenuLinks AS Menu LEFT JOIN GEN_MenuLinks AS MENU1 ON MENU1.MenuID = Menu.ParentMenuID where Menu.isactive=1 and Menu.isdeleted=0 and Menu.MenuText Like '%" + prefix + "%' and Menu.MenuText IS NOT NULL ";


            //List<DropDownData> olst = new List<DropDownData>();
            //// DataSet ds = DB.GetDS("select UserRoleID,UserRole from GEN_UserRole where IsDeleted=0 and IsActive=1 and (UserRoleTypeID="+ usertypeid + " OR UserRoleTypeID IS NULL)", false);
            //DataSet ds = DB.GetDS("   SELECT Menu.MenuID as ID, Menu.MenuText as Value FROM GEN_MenuLinks AS Menu LEFT JOIN GEN_MenuLinks AS MENU1 ON MENU1.MenuID = Menu.ParentMenuID where Menu.MenuText Like '%"+prefix+"%' and Menu.MenuText IS NOT NULL", false);
            IDataReader drWorkCenter = DB.GetRS(cMdWorkCentersql);

            while (drWorkCenter.Read())
            {
                WorkCenterList.Add(String.Format("{0},{1}", drWorkCenter["Value"], drWorkCenter["ID"]));

            }
            drWorkCenter.Close();
            return WorkCenterList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetMCodeBasedonSupplierWithOEMFor3PL(string prefix, String SuppleirID, string TenantID, String POHeaderID)
        {
            List<string> mmList = new List<string>();
            string mmSql;
            if (POHeaderID == "0")

                // mmSql = "SELECT DISTINCT Top 20 MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode , TNT_M.MaterialMasterID FROM TPL_Tenant_Supplier TNT_S JOIN TPL_Tenant_MaterialMaster TNT_M ON TNT_M.TenantID=TNT_S.TenantID AND TNT_M.IsActive=1 AND TNT_M.IsDeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=TNT_M.MaterialMasterID AND MM.IsActive=1 AND MM.IsDeleted=0  WHERE  TNT_S.IsActive=1 AND TNT_S.IsDeleted=0 AND TNT_S.SupplierID=" + SuppleirID + " AND TNT_M.TenantID=" + TenantID + " AND  ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";
                //<!------------procedure converting------------->
                //mmSql = "SELECT DISTINCT Top 20 MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode , TNT_M.MaterialMasterID FROM MMT_MaterialMaster_Supplier MM_S JOIN TPL_Tenant_MaterialMaster TNT_M ON TNT_M.MaterialMasterID=MM_S.MaterialMasterID AND TNT_M.isdeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=TNT_M.MaterialMasterID AND MM.IsActive=1 AND MM.IsDeleted=0 WHERE MM_S.SupplierID="+ SuppleirID+" and MM_S.isdeleted=0 AND TNT_M.TenantID="+ TenantID+"  AND  ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";
                mmSql = "Exec [dbo].[USP_MST_MCodeDropData] @prefix='" + prefix + "',@TenantID=" + TenantID + ",@SupplierID=" + SuppleirID + "";
            else
                //mmSql = "SELECT DISTINCT Top 20 MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode , TNT_M.MaterialMasterID FROM TPL_Tenant_Supplier TNT_S JOIN TPL_Tenant_MaterialMaster TNT_M ON TNT_M.TenantID=TNT_S.TenantID AND TNT_M.IsActive=1 AND TNT_M.IsDeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=TNT_M.MaterialMasterID AND MM.IsActive=1 AND MM.IsDeleted=0 JOIN ORD_PODetails POD ON POD.MaterialMasterID=TNT_M.MaterialMasterID AND  POD.IsDeleted=0 WHERE  TNT_S.IsActive=1 AND TNT_S.IsDeleted=0 AND TNT_S.SupplierID=" + SuppleirID + " AND TNT_M.TenantID=" + TenantID + " AND POD.POHeaderID=" + POHeaderID + " AND  ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";

                //<!------------procedure converting------------->
                // mmSql = "SELECT DISTINCT Top 20 MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode , POD.MaterialMasterID FROM ORD_PODetails POD JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=POD.MaterialMasterID AND MM.IsActive=1 AND MM.IsDeleted=0  WHERE POHeaderID=" + POHeaderID + " AND POD.IsDeleted=0 AND POD.IsActive=1 AND  ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";
                mmSql = "Exec [dbo].[USP_PODetails_MCodeDropData] @prefix='" + prefix + "',@POHeaderID=" + POHeaderID + "";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadKitMCodeOEMDataFor3PL(string prefix, string TenantID)
        {
            List<string> MMList = new List<string>();

            if (TenantID == "")
                TenantID = "0";

            string MMSql = "select top 20 MM.MCode +   isnull( ' ` '+ MM.OEMPartNo,'')  AS MCode from  MMT_KitPlanner Kit JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=Kit.ParentMaterialMasterID AND MM.IsActive=1 AND MM.IsDeleted=0 where Kit.IsDeleted=0 and Kit.IsActive=1 AND (0=" + TenantID + " OR Kit.TenantID=" + TenantID + ") AND ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";

            IDataReader rsMM = DB.GetRS(MMSql);

            while (rsMM.Read())
            {
                MMList.Add(string.Format("{0}", rsMM["MCode"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetParentMCodeKitFor3PL(string prefix, string TenantID, int KitTypeID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "";
            if (KitTypeID == 1)
            {
                //string mmSql = "SELECT Top 20 MCode, TNT_M.MaterialMasterID FROM TPL_Tenant_MaterialMaster TNT_M JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=TNT_M.MaterialMasterID AND TNT_M.IsActive=1 AND TNT_M.IsDeleted=0 WHERE TNT_M.TenantID=" + TenantID + " AND  MM.IsActive=1 AND MM.IsDeleted=0 AND ISNULL(IsAproved,0)=1 AND  MCode like '" + prefix + "%'";
                mmSql = "SELECT Top 20 MCode, TNT_M.MaterialMasterID,MHeight,MWidth,MLength,MWeight FROM TPL_Tenant_MaterialMaster TNT_M JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=TNT_M.MaterialMasterID AND TNT_M.IsActive=1 AND TNT_M.IsDeleted=0 WHERE TNT_M.TenantID=" + TenantID + " AND  MM.IsActive=1 AND MM.IsDeleted=0 AND KitPlannerID is null AND MCode like '" + prefix + "%'";
            }

            else
            {
                mmSql = "SELECT MCode,MaterialMasterID,MHeight,MWidth,MLength,MWeight FROM MMT_MaterialMaster WHERE IsActive=1 AND IsDeleted=0 AND MTypeID=-1 AND TenantID=" + TenantID + " AND MCode like '" + prefix + "%'";
            }

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1},{2},{3},{4},{5}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"], rsMCodeList["MHeight"], rsMCodeList["MWidth"], rsMCodeList["MLength"], rsMCodeList["MWeight"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] getParentMcodesForBOM(string prefix, string TenantID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "";

            mmSql = "select top 10 mmt.MaterialMasterID,MCode,uom.UoM+' / '+convert(nvarchar, MMTUOM.UoMQty) UOM,mmtuom.MaterialMaster_UoMID UOMID from MMT_MaterialMaster mmt " +
                    " join TPL_Tenant_MaterialMaster tnt on mmt.MaterialMasterID=tnt.MaterialMasterID and tnt.IsDeleted=0 and tnt.isactive=1 " +
                       "join MMT_MaterialMaster_GEN_UoM MMTUOM on mmt.MaterialMasterID = MMTUOM.MaterialMasterID and mmt.IsDeleted = 0 and mmt.IsActive = 1 and MMTUOM.uomtypeid = 1 " +
                        "join gen_uom uom on mmtuom.uomid = uom.UoMID and MMTUOM.IsDeleted = 0 and MMTUOM.IsActive = 1 and uom.IsActive = 1 and uom.IsDeleted = 0 " +
                        "where mmt.MaterialMasterID not in (select MaterialMasterId from BOMHeader) and tnt.TenantID = " + TenantID + " and(mcode like '" + prefix + "%' or mcode like '%" + prefix + "' or mcode like '%" + prefix + "%') ";



            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1},{2},{3}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"], rsMCodeList["UOM"], rsMCodeList["UOMID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] getBOMRefNo(string prefix, string TenantID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "";

            mmSql = "select distinct top 10  bom.BOMHeaderId,BOMRefNo from BOMHeader  bom join BOMDetails det on bom.BOMHeaderId=det.BOMHeaderId and det.isdeleted=0 and det.isactive=1 " +
                        "where TenantID = " + TenantID + " and (BOMRefNo like '" + prefix + "%' or BOMRefNo like '%" + prefix + "' or BOMRefNo like '%" + prefix + "%') ";



            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["BOMRefNo"], rsMCodeList["BOMHeaderId"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] getPartNOForJoblist(string prefix, string TenantID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "";

            //mmSql = "select top 10 materialmasterid,mcode from MMT_MaterialMaster " +
            //            "where IsDeleted=0 and IsActive=1 and TenantID = " + TenantID + " and (mcode like '" + prefix + "%' or mcode like '%" + prefix + "' or mcode like '%" + prefix + "%') ";

            mmSql = "select distinct top 10  MMT.MaterialMasterID,MMT.MCode from MMT_MaterialMaster MMT join INV_GoodsMovementDetails GMD on GMD.MaterialMasterID = MMT.MaterialMasterID and MMT.IsActive=1 and MMT.IsDeleted=0 where gmd.IsDeleted=0 and gmd.IsActive=1 and" +
                         " MMT.TenantID =  " + TenantID + " and CONVERT(date,gmd.ExpDate)< convert(date,getdate()) and (mcode like '%' or mcode like '%' or mcode like '%%') AND GMD.ExpDate <> '1900-01-01 00:00:00.000' ";



            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["mcode"], rsMCodeList["materialmasterid"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] getPartNOForKits(string prefix, string TenantID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "";

            mmSql = "select Distinct top 10 materialmasterid,mcode from MMT_KitPlanner KP left JOIN MMT_MaterialMaster MM ON  (KP.ParentMaterialMasterID=MM.MaterialMasterID  ) AND MM.IsActive=1 AND MM.IsDeleted=0 where  KP.TenantID = " + TenantID + " and KP.Isactive=1 and KP.IsDeleted=0  and (mcode like '" + prefix + "%' or mcode like '%" + prefix + "' or mcode like '%" + prefix + "%') ";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["mcode"], rsMCodeList["materialmasterid"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] getJOBRefNo(string prefix, string TenantID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "";

            mmSql = "select top 10 JOBOrderHeaderID,JobOrderRefNo from MMT_KitJobOrderHeader  " +
                        "where IsDeleted=0 and IsActive=1 and  TenantID = " + TenantID + " and (JobOrderRefNo like '" + prefix + "%' or JobOrderRefNo like '%" + prefix + "' or JobOrderRefNo like '%" + prefix + "%') ";



            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["JobOrderRefNo"], rsMCodeList["JOBOrderHeaderID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] getParentMcodesForBOMList(string prefix, string TenantID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "";

            mmSql = "select top 10 mmt.MaterialMasterID,MCode,uom.UoM+' / '+convert(nvarchar, MMTUOM.UoMQty) UOM,mmtuom.MaterialMaster_UoMID UOMID from MMT_MaterialMaster mmt " +
                       "join MMT_MaterialMaster_GEN_UoM MMTUOM on mmt.MaterialMasterID = MMTUOM.MaterialMasterID and mmt.IsDeleted = 0 and mmt.IsActive = 1 and MMTUOM.uomtypeid = 1 " +
                        "join BOMHeader boh on mmt.MaterialMasterID=boh.MaterialMasterId join gen_uom uom on mmtuom.uomid = uom.UoMID and MMTUOM.IsDeleted = 0 and MMTUOM.IsActive = 1 and uom.IsActive = 1 and uom.IsDeleted = 0 " +
                        "where  mmt.TenantID = " + TenantID + " and(mcode like '" + prefix + "%' or mcode like '%" + prefix + "' or mcode like '%" + prefix + "%') ";



            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1},{2},{3}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"], rsMCodeList["UOM"], rsMCodeList["UOMID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] getChildLineMcodesForBOM(string prefix, string TenantID, string BOMid)
        {
            List<string> mmList = new List<string>();
            string mmSql = "";

            mmSql = "select top 10 mmt.MaterialMasterID,MCode,uom.UoM+' / '+convert(nvarchar, MMTUOM.UoMQty) UOM,mmtuom.MaterialMaster_UoMID UOMID from " +
                "MMT_MaterialMaster mmt join TPL_Tenant_MaterialMaster tnt on mmt.MaterialMasterID=tnt.MaterialMasterID and tnt.IsDeleted=0 and tnt.IsActive=1 " +
                       "join MMT_MaterialMaster_GEN_UoM MMTUOM on mmt.MaterialMasterID = MMTUOM.MaterialMasterID and mmt.IsDeleted = 0 and mmt.IsActive = 1 and MMTUOM.uomtypeid = 1 " +
                        "join gen_uom uom on mmtuom.uomid = uom.UoMID and MMTUOM.IsDeleted = 0 and MMTUOM.IsActive = 1 and uom.IsActive = 1 and uom.IsDeleted = 0 " +
                        "where mmt.MaterialMasterID not in (select MaterialMasterId from BOMDetails where bomheaderid=" + BOMid + " and Isdeleted=0 and IsActive=1) and mmt.MaterialMasterID not in (select MaterialMasterId from bomheader  ) and tnt.TenantID = " + TenantID + " and(mcode like '" + prefix + "%' or mcode like '%" + prefix + "' or mcode like '%" + prefix + "%') ";



            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1},{2},{3}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"], rsMCodeList["UOM"], rsMCodeList["UOMID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] getChildLineMcodesUOMForBOM(string MMID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "";

            //mmSql = "select top 10 uom.UoM+' / '+convert(nvarchar, MMTUOM.UoMQty) UOM,mmtuom.MaterialMaster_UoMID UOMID "+
            //        " from MMT_MaterialMaster_GEN_UoM MMTUOM "+
            //        " join gen_uom uom on mmtuom.uomid = uom.UoMID and MMTUOM.IsDeleted = 0 and MMTUOM.IsActive = 1  "+
            //        "and uom.IsActive = 1 and uom.IsDeleted = 0 where  MMTUOM.MaterialMasterID = "+ MMID;

            mmSql = "Exec [dbo].[USP_LoadUoMWithQty]  @MtypeID=7 ,@MaterialID=" + MMID;

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1},", rsMCodeList["UOM"] + "/" + rsMCodeList["UoMQty"], rsMCodeList["MaterialMaster_UoMID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetParentMCodeKitFor3PL1(string prefix, string TenantID, string kitheaderid, string SupplierID)
        {
            List<string> mmList = new List<string>();
            int supid = DB.GetSqlN(" select isnull(SupplierId,0) as N from MMT_KitPlanner where KitPlannerID=" + kitheaderid);
            //string mmSql = "SELECT Top 20 MCode, TNT_M.MaterialMasterID FROM TPL_Tenant_MaterialMaster TNT_M JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=TNT_M.MaterialMasterID AND TNT_M.IsActive=1 AND TNT_M.IsDeleted=0 WHERE TNT_M.TenantID=" + TenantID + " AND  MM.IsActive=1 AND MM.IsDeleted=0 AND ISNULL(IsAproved,0)=1 AND  MCode like '" + prefix + "%'";
            string mmSql = "SELECT DISTINCT Top 20 MCode, TNT_M.MaterialMasterID,MHeight,MWidth,MLength,MWeight FROM TPL_Tenant_MaterialMaster TNT_M JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=TNT_M.MaterialMasterID AND TNT_M.IsActive=1 AND TNT_M.IsDeleted=0 join MMT_MaterialMaster_Supplier mmtsup on mm.MaterialMasterID=mmtsup.MaterialMasterID and mmtsup.isdeleted=0 and mmtsup.IsActive=1 WHERE TNT_M.TenantID=" + TenantID + " AND  MM.IsActive=1 AND MM.IsDeleted=0 AND KitPlannerID is null AND MCode like '" + prefix + "%'  AND MCode like '%' and mm.MaterialMasterID not in (select ChildMMID from MMT_KitPlannerDetail where KitPlannerID=" + kitheaderid + ")  and " +
                            " mm.MaterialMasterID not in (select isnull((select ParentMaterialMasterID from MMT_KitPlanner where KitPlannerID = " + kitheaderid + "),0))";
            // and mmtsup.SupplierID="+ supid

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1},{2},{3},{4},{5}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"], rsMCodeList["MHeight"], rsMCodeList["MWidth"], rsMCodeList["MLength"], rsMCodeList["MWeight"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetsuppliersForMcode(string prefix, string MMid)
        {
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT Top 20 MCode, TNT_M.MaterialMasterID FROM TPL_Tenant_MaterialMaster TNT_M JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=TNT_M.MaterialMasterID AND TNT_M.IsActive=1 AND TNT_M.IsDeleted=0 WHERE TNT_M.TenantID=" + TenantID + " AND  MM.IsActive=1 AND MM.IsDeleted=0 AND ISNULL(IsAproved,0)=1 AND  MCode like '" + prefix + "%'";
            string mmSql = "select top 10 sup.supplierid,SupplierName+' - '+SupplierCode supplier from MMT_MaterialMaster_Supplier mmts " +
                            " join MMT_Supplier sup on mmts.SupplierID = sup.SupplierID " +
                        " and mmts.IsDeleted = 0 and mmts.IsActive = 1 and sup.IsDeleted = 0 and sup.isactive = 1 where MaterialMasterID = " + MMid + " and(SupplierName like '" + prefix + "%' or SupplierName like '%" + prefix + "' or SupplierName like '%" + prefix + "%')";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["supplier"], rsMCodeList["supplierid"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetChildMCodeKitFor3PL(string prefix, string TenantID, string ParentMMID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "SELECT distinct Top 20 MCode, TNT_M.MaterialMasterID FROM TPL_Tenant_MaterialMaster TNT_M JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=TNT_M.MaterialMasterID AND MM.IsActive=1 AND MM.IsDeleted=0 WHERE TNT_M.TenantID=" + TenantID + " AND TNT_M.IsDeleted=0 AND TNT_M.MaterialMasterID<>" + ParentMMID + " AND  MCode like '" + prefix + "%'";
            // string mmSql = "SELECT TOP 10 MCode, MM.MaterialMasterID FROM MMT_MaterialMaster MM  LEFT JOIN MMT_KitPlannerDetail KPD ON KPD.ChildMMID=MM.MaterialMasterID AND KPD.IsDeleted=0 AND KPD.KitPlannerID="+ KitID +" LEFT JOIN MMT_KitPlanner KP ON KP.ParentMaterialMasterID=MM.MaterialMasterID AND KP.IsDeleted=0 AND KP.KitPlannerID=" + KitID + " WHERE MM.TenantID=" + TenantID + " AND MM.IsActive=1 AND MM.IsDeleted=0 AND KPD.KitPlannerID IS NULL  AND KP.KitPlannerID IS NULL AND  MCode like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadActivityRateType(String prefix, String InOut)
        {
            List<String> ActivityRateList = new List<String>();

            String ARSql = "";

            ARSql = "select TOP 10 ActivityRateTypeID,ActivityRateType from TPL_Activity_RateType where IsActive=1 and IsDeleted=0 and ActivityRateTypeID NOT IN (13) and InOutID in (" + InOut + ")" + " and ActivityRateType like '" + prefix + "%'";
            IDataReader rsPO = DB.GetRS(ARSql);
            while (rsPO.Read())
            {
                ActivityRateList.Add(String.Format("{0},{1}", rsPO["ActivityRateType"], rsPO["ActivityRateTypeID"]));
            }
            rsPO.Close();

            return ActivityRateList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetDataForActivityrateType(string prefix, string ActivityRateGroupID, string ActivityRateTypeID)
        {
            List<string> Chargelist = new List<string>();
            //string cMdChargelist = "SELECT TOP 10 ChargeDetailID,ChargeDetail FROM [3PL_ChargeDetail] WHERE InOutID in (" + InOutID + " )";
            string cMdChargelist = "select TOP 10 ActivityRateTypeID,ActivityRateType from TPL_Activity_RateType where ActivityRateTypeID=" + ActivityRateTypeID + " AND IsDeleted=0 union select TOP 10 ActivityRateTypeID,ActivityRateType from TPL_Activity_RateType where ActivityRateGroupID=" + ActivityRateGroupID + "AND IsDeleted=0 AND ActivityRateType like '" + prefix + "%' order by ActivityRateTypeID";
            // string cMdChargelist = "SELECT TOP 10 ActivityRateType, ActivityRateTypeID FROM TPL_Activity_RateType WHERE ActivityRateType like '" + prefix + "%' AND IsActive = 1 AND IsDeleted =0 order by ActivityRateTypeID";
            IDataReader drcMdcMdChargelist = DB.GetRS(cMdChargelist);

            while (drcMdcMdChargelist.Read())
            {
                Chargelist.Add(string.Format("{0},{1}", drcMdcMdChargelist["ActivityRateType"], drcMdcMdChargelist["ActivityRateTypeID"]));
            }
            drcMdcMdChargelist.Close();
            return Chargelist.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetDataForActivityName(string prefix, string ActivityRateTypeID)
        {
            List<string> Chargelist = new List<string>();
            //string cMdChargelist = "SELECT TOP 10 ChargeConfigurationID,ServiceRequired FROM [3PL_ChargeConfiguration] WHERE ChargeDetailID=" + ChargeDetailID;
            string cMdChargelist = "SELECT TOP 10 ActivityRateID,ActivityRateName FROM TPL_Activity_Rate WHERE ActivityRateTypeID=" + ActivityRateTypeID + "AND IsDeleted=0 AND ActivityRateName like '" + prefix + "%' order by ActivityRateName";


            IDataReader drcMdcMdChargelist = DB.GetRS(cMdChargelist);

            while (drcMdcMdChargelist.Read())
            {
                Chargelist.Add(string.Format("{0},{1}", drcMdcMdChargelist["ActivityRateName"], drcMdcMdChargelist["ActivityRateID"]));
            }
            drcMdcMdChargelist.Close();
            return Chargelist.ToArray();
        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetDataForActivityName_Inbound(string prefix, string ActivityRateTypeID, string InboundId)
        {
            List<string> Chargelist = new List<string>();
            //Commented BY Swamy to remove Mapping for TransCrate Data on OCT 24 2019
            int WareHouseId = DB.GetSqlN("SELECT WarehouseID AS N FROM INB_RefWarehouse_Details where InboundID  = " + InboundId + " AND IsActive = 1 AND IsDeleted = 0");


            //string cMdChargelist = "SELECT TOP 10 ChargeConfigurationID,ServiceRequired FROM [3PL_ChargeConfiguration] WHERE ChargeDetailID=" + ChargeDetailID;

            //Commented BY Swamy to remove Mapping for TransCrate Data on OCT 24 2019
            string cMdChargelist = "SELECT TOP 10 ActivityRateID,ActivityRateName FROM TPL_Activity_Rate WHERE ActivityRateTypeID=" + ActivityRateTypeID + "AND WareHouseId = " + WareHouseId + " AND IsDeleted=0 AND ActivityRateName like '" + prefix + "%' order by ActivityRateName";



            //  string cMdChargelist = "SELECT TOP 10 ActivityRateID,ActivityRateName FROM TPL_Activity_Rate WHERE ActivityRateTypeID=" + ActivityRateTypeID + " AND IsDeleted=0 AND ActivityRateName like '" + prefix + "%' order by ActivityRateName";
            IDataReader drcMdcMdChargelist = DB.GetRS(cMdChargelist);

            while (drcMdcMdChargelist.Read())
            {
                Chargelist.Add(string.Format("{0},{1}", drcMdcMdChargelist["ActivityRateName"], drcMdcMdChargelist["ActivityRateID"]));
            }
            drcMdcMdChargelist.Close();
            return Chargelist.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMCodeOEMDataFor3PL(string prefix)
        {
            List<string> MMList = new List<string>();
            string MMSql = "select top 20 MCode +   isnull( ' ` '+ OEMPartNo,'')  AS MCode  from MMT_MaterialMaster where IsDeleted=0 and IsActive=1 AND ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) order by MCode";

            IDataReader rsMM = DB.GetRS(MMSql);

            while (rsMM.Read())
            {
                MMList.Add(string.Format("{0}", rsMM["MCode"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetTaxation(string prefix)
        {
            List<string> MMList = new List<string>();

            string MMSql = "select TaxationID,TaxCode from TPL_Taxation WHERE IsActive=1 AND IsDeleted=0";

            IDataReader rsMM = DB.GetRS(MMSql);

            while (rsMM.Read())
            {
                //MMList.Add(string.Format("{0}", rsMM["TaxationID"]));
                MMList.Add(String.Format("{0},{1}", rsMM["TaxationID"], rsMM["TaxCode"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMCodeDataFor3PL(string prefix, string TenantID)
        {
            List<string> MMList = new List<string>();

            if (TenantID == "")
                TenantID = "0";

            //Account and Tenant filteration

            string AccTenantId = TenantID == "0" ? (cp.TenantID.ToString() == "0" ? "TNT_M.TenantID" : cp.TenantID.ToString()) : TenantID;

            string MMSql = "SELECT DISTINCT TOP 10 MCode FROM MMT_MaterialMaster MM LEFT JOIN TPL_Tenant_MaterialMaster TNT_M ON TNT_M.MaterialMasterID=MM.MaterialMasterID JOIN TPL_Tenant TNT ON TNT.TenantID = TNT_M.TenantID JOIN GEN_Account ACC ON ACC.AccountID = TNT.AccountID AND TNT_M.IsActive=1 AND TNT_M.IsDeleted=0 WHERE MM.IsActive=1 AND MM.IsDeleted=0 AND (0=" + TenantID + " OR TNT_M.TenantID=" + TenantID + ") AND ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) ";
            MMSql = MMSql + "and TNT.AccountID= case when 0 =" + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end AND TNT.TenantID = case when  " + cp.TenantID.ToString() + "=0  then TNT.TenantID else " + cp.TenantID.ToString() + " end order by MCode";

            IDataReader rsMM = DB.GetRS(MMSql);

            while (rsMM.Read())
            {
                MMList.Add(string.Format("{0}", rsMM["MCode"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }
        //Load Mcode based on Supplier
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMCodeDataFor3PLSupplier(string prefix, string TenantID, int SupplierID)
        {
            List<string> MMList = new List<string>();

            if (TenantID == "")
                TenantID = "0";

            //Account and Tenant filteration

            string AccTenantId = TenantID == "0" ? (cp.TenantID.ToString() == "0" ? "TNT_M.TenantID" : cp.TenantID.ToString()) : TenantID;

            string MMSql = "select TOP 10 MCode from MMT_MaterialMaster_Supplier MSUP left join MMT_MaterialMaster MM on MSUP.MaterialMasterID=MM.MaterialMasterID left join MMT_Supplier SUP on SUP.SupplierID=MSUP.SupplierID JOIN GEN_Account ACC ON ACC.AccountID = SUP.AccountID AND SUP.IsActive=1 AND SUP.IsDeleted=0 WHERE MM.IsActive=1 AND MM.IsDeleted=0 and MSUP.isactive=1 and MSUP.isdeleted=0 AND  MSUP.SupplierID=" + SupplierID + " and  SUP.AccountID= case when 0 =" + cp.AccountID.ToString() + " then SUP.AccountID else " + cp.AccountID.ToString() + " end and SupplierName like '" + prefix + "%'  order by SupplierName ";


            //string MMSql = "SELECT DISTINCT TOP 10 MCode FROM MMT_MaterialMaster MM LEFT JOIN TPL_Tenant_MaterialMaster TNT_M ON TNT_M.MaterialMasterID=MM.MaterialMasterID JOIN TPL_Tenant TNT ON TNT.TenantID = TNT_M.TenantID JOIN GEN_Account ACC ON ACC.AccountID = TNT.AccountID AND TNT_M.IsActive=1 AND TNT_M.IsDeleted=0 WHERE MM.IsActive=1 AND MM.IsDeleted=0 AND (0=" + TenantID + " OR TNT_M.TenantID=" + TenantID + ") AND ( MCode like '" + prefix + "%' OR  OEMPartNo like '" + prefix + "%' ) ";
            //MMSql = MMSql + "and TNT.AccountID= case when 0 =" + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end AND TNT.TenantID = case when  " + cp.TenantID.ToString() + "=0  then TNT.TenantID else " + cp.TenantID.ToString() + " end order by MCode";

            IDataReader rsMM = DB.GetRS(MMSql);

            while (rsMM.Read())
            {
                MMList.Add(string.Format("{0}", rsMM["MCode"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadActivityRate(String prefix, String RateTypeID, String OutboundId)
        {
            List<String> ActivityRateList = new List<String>();

            String ARSql = "";

            ARSql = "select ActivityRateID,ActivityRateName from TPL_Activity_Rate where ActivityRateTypeID=" + RateTypeID + " AND WareHouseId IN (SELECT WarehouseID FROM OBD_RefWarehouse_Details WHERE OutboundID = " + OutboundId + ") and IsActive=1 and IsDeleted=0 and ActivityRateName like '" + prefix + "%'";
            IDataReader rsPO = DB.GetRS(ARSql);
            while (rsPO.Read())
            {
                ActivityRateList.Add(String.Format("{0},{1}", rsPO["ActivityRateName"], rsPO["ActivityRateID"]));
            }
            rsPO.Close();

            return ActivityRateList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMaterialShape(string prefix)
        {

            List<string> CurrencyList = new List<string>();
            string CurrencySql = " select MaterialShapeID,MaterialShape from TPL_MaterialShape where IsActive=1 and IsDeleted=0     AND  MaterialShape  like '" + prefix + "%'";


            IDataReader rsCurrency = DB.GetRS(CurrencySql);

            while (rsCurrency.Read())
            {
                CurrencyList.Add(string.Format("{0},{1}", rsCurrency["MaterialShape"], rsCurrency["MaterialShapeID"]));
            }

            rsCurrency.Close();

            return CurrencyList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadSpaceutilization(string prefix)
        {

            List<string> SpaceutilizationList = new List<string>();
            string SpaceutilizationSql = "select SpaceUtilizationID,SpaceUtilization from TPL_SpaceUtilization where IsActive=1 and IsDeleted=0 AND  SpaceUtilization  like '" + prefix + "%'";


            IDataReader rsSpaceutilization = DB.GetRS(SpaceutilizationSql);

            while (rsSpaceutilization.Read())
            {
                SpaceutilizationList.Add(string.Format("{0},{1}", rsSpaceutilization["SpaceUtilization"], rsSpaceutilization["SpaceUtilizationID"]));
            }

            rsSpaceutilization.Close();

            return SpaceutilizationList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] MaterialUoMListForKit3PL(string prefix, string MMID)
        {
            List<string> UoMList = new List<string>();

            string UoMSql = "select MMT_U.MaterialMaster_UoMID,GEN_U.UoM+'/'+CONVERT(nvarchar,MMT_U.UoMQty) [UoM/UoMQty] from MMT_MaterialMaster MMT left join MMT_MaterialMaster_GEN_UoM MMT_U on MMT_U.MaterialMasterID=MMT.MaterialMasterID and MMT_U.IsDeleted=0 left join GEN_UoM GEN_U on GEN_U.UoMID=MMT_U.UoMID and MMT_U.IsDeleted=0   where MMT_U.IsActive=1 and MMT_U.IsDeleted=0 and   MMT.MaterialMasterID=" + MMID;

            DataSet ds = DB.GetDS(UoMSql, false);

            if (ds.Tables[0].Rows.Count >= 2)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    UoMList.Add(string.Format("{0},{1}", DB.RowField(dr, "UoM/UoMQty"), DB.RowFieldInt(dr, "MaterialMaster_UoMID")));
                }
            }
            return UoMList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] MaterialBasedUoMList3PL(string prefix, string MMID)
        {
            List<string> UoMList = new List<string>();

            string UoMSql = "select MMT_U.UoMID,GEN_U.UoM+'/'+CONVERT(nvarchar,MMT_U.UoMQty) [UoM/UoMQty] from MMT_MaterialMaster MMT left join MMT_MaterialMaster_GEN_UoM MMT_U on MMT_U.MaterialMasterID=MMT.MaterialMasterID and MMT_U.IsDeleted=0 left join GEN_UoM GEN_U on GEN_U.UoMID=MMT_U.UoMID and MMT_U.IsDeleted=0   where MMT_U.IsActive=1 and MMT_U.IsDeleted=0 and   MMT.MaterialMasterID=" + MMID;

            DataSet ds = DB.GetDS(UoMSql, false);

            if (ds.Tables[0].Rows.Count >= 2)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    UoMList.Add(string.Format("{0},{1}", DB.RowField(dr, "UoM/UoMQty"), DB.RowFieldInt(dr, "UoMID")));
                }
            }

            return UoMList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetNonConformityLocationsFor3PL(string Prefix, String IsNonConformity, String AsIs, String InboundID)
        {
            List<string> LocationList = new List<string>();

            string cMdLocation = "";

            if (IsNonConformity == "1" && AsIs == "0")
            {
                cMdLocation = "select top 20 Location,LocationID from INV_Location where left(location,2)='Q1' and IsDeleted=0 and location like '" + Prefix + "%'";
            }
            else
            {
                cMdLocation = "SELECT top 20 Location,LocationID from INV_Location LOC JOIN INB_Inbound IBT ON IBT.TenantID=LOC.TenantID AND IBT.SupplierID=LOC.SupplierID AND IBT.IsDeleted=0 AND IBT.IsActive=1 WHERE (left(LOC.Location,2) !='Q1' AND LOC.IsDeleted=0 AND IBT.InboundID=" + InboundID + ")  and location like '" + Prefix + "%'";
            }

            IDataReader rsLocation = DB.GetRS(cMdLocation);

            while (rsLocation.Read())
            {
                LocationList.Add(string.Format("{0},{1}", rsLocation["Location"], rsLocation["LocationID"]));
            }
            rsLocation.Close();
            return LocationList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetLocationListFor3PL(string Prefix, String ProductCategory, String InboundID, String IsQuarantine)
        {
            List<string> LocationList = new List<string>();
            //string cMdLocationList = "select top 10 Location,LocationID from INV_Location loc join INV_LocationZone locz on locz.LocationZoneCode=left(Location,2) and locz.IsDeleted=0 left join INB_RefWarehouse_Details refw on refw.WarehouseID=locz.WarehouseID and refw.IsDeleted=0 and refw.InboundID="+InboundID+" where ("+InboundID+"=0 or refw.InboundID is not null) and loc.IsDeleted=0 and (" + ProductCategory + " not in (0,3) or  (0=" + ProductCategory + " and left(Location,2)!='Q1') or (3=" + ProductCategory + " and left(Location,2)='P1')) and Location  like '" + Prefix + "%'";
            //string cMdLocationList = "select top 10 Location,LocationID from INV_Location loc join INV_LocationZone locz on locz.LocationZoneCode=left(Location,2) and locz.IsDeleted=0 left join INB_RefWarehouse_Details refw on refw.WarehouseID=locz.WarehouseID and refw.IsDeleted=0 and refw.InboundID=" + InboundID + " join INB_Inbound IBT ON IBT.InboundID=refw.InboundID and IBT.IsDeleted=0 and IBT.IsActive=1 AND IBT.TenantID=loc.TenantID and IBT.SupplierID=loc.SupplierID  where (" + InboundID + "=0 or refw.InboundID is not null) and loc.IsDeleted=0 and (" + ProductCategory + " not in (0,3) or  (0=" + ProductCategory + " and left(Location,2)!='Q1') or (3=" + ProductCategory + " and left(Location,2)='P1')) AND IsQuarantine=" + IsQuarantine + " and Location  like '" + Prefix + "%'";
            string cMdLocationList = "[sp_TPL_GetLocationsFor3PL] @InboundID=" + InboundID + ",@ProductCategoryID=" + ProductCategory + ",@IsQuarantine=" + IsQuarantine + ",@Prefix=" + DB.SQuote(Prefix);

            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0}", rslocationList["Location"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetContainers_Tenant(string Prefix, string TenantID, string Location,string WarehouseId)
        {
            List<string> StoreNoList = new List<string>();
            //string cMdStoreRefNo = "select top 10 CartonID,CartonCode from INV_Carton where IsActive=1 and IsDeleted=0 and CartonCode like '_______" + storeRefNumber + "%'and CartonCode like '" + Prefix + "%'";
            string cMdStoreRefNo = "EXEC [dbo].[sp_GET_Loction_CartonList_Tenant] @Location = " + DB.SQuote(Location) + ",@WarehouseId ="+WarehouseId+", @TenantID = " + TenantID + ",@UserId ="+cp.UserID+" , @Prefix = " + DB.SQuote(Prefix) + "";   // Userid added by Ganesh @ Sep 2020 28

            IDataReader rsStorerefNo = DB.GetRS(cMdStoreRefNo);

            while (rsStorerefNo.Read())
            {
                StoreNoList.Add(string.Format("{0},{1}", rsStorerefNo["CartonCode"], rsStorerefNo["CartonID"]));
            }
            rsStorerefNo.Close();
            return StoreNoList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetContainers(string Prefix)
        {
            List<string> StoreNoList = new List<string>();
            //string cMdStoreRefNo = "select top 10 CartonID,CartonCode from INV_Carton where IsActive=1 and IsDeleted=0 and CartonCode like '_______" + storeRefNumber + "%'and CartonCode like '" + Prefix + "%'";
            string cMdStoreRefNo = "select top 10 CartonID,CartonCode from INV_Carton where IsActive=1 and IsDeleted=0 AND ContainerTypeID=1 and CartonCode like '" + Prefix + "%' and createdby=" + cp.UserID + "";

            IDataReader rsStorerefNo = DB.GetRS(cMdStoreRefNo);

            while (rsStorerefNo.Read())
            {
                StoreNoList.Add(string.Format("{0},{1}", rsStorerefNo["CartonCode"], rsStorerefNo["CartonID"]));
            }
            rsStorerefNo.Close();
            return StoreNoList.ToArray();
        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetContainers_StockIn(string Prefix, string LocationID, string StoreRefNum)
        {
            List<string> StoreNoList = new List<string>();
            //string cMdStoreRefNo = "select top 10 CartonID,CartonCode from INV_Carton where IsActive=1 and IsDeleted=0 and CartonCode like '_______" + storeRefNumber + "%'and CartonCode like '" + Prefix + "%'";
            string cMdStoreRefNo = "EXEC [dbo].[sp_GET_CartonList] @LocationID = " + LocationID + " ,@CartonCode = '" + Prefix + "', @StoreNumber = '" + StoreRefNum + "'";

            IDataReader rsStorerefNo = DB.GetRS(cMdStoreRefNo);

            while (rsStorerefNo.Read())
            {
                StoreNoList.Add(string.Format("{0},{1}", rsStorerefNo["CartonCode"], rsStorerefNo["CartonID"]));
            }
            rsStorerefNo.Close();
            return StoreNoList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetLocationListForInternalTransfer(string Prefix, String ProductCategory, string MaterialMasterID, string IsQuarantine)
        {
            List<string> LocationList = new List<string>();

            //string cMdLocationList = "SELECT top 10 LocationID,Location FROM INV_Location LOC JOIN TPL_Tenant_MaterialMaster TNT_M ON TNT_M.TenantID=LOC.TenantID AND TNT_M.IsDeleted=0 JOIN MMT_MaterialMaster_Supplier MM_S ON MM_S.MaterialMasterID=TNT_M.MaterialMasterID AND MM_S.SupplierID=LOC.SupplierID AND MM_S.IsDeleted=0  WHERE MM_S.MaterialMasterID=" + MaterialMasterID + " and (" + ProductCategory + " not in (0,3) or  (0=" + ProductCategory + " and left(Location,2)!='Q1') or (3=" + ProductCategory + " and left(Location,2)='P1')) AND IsQuarantine=" + IsQuarantine + " and Location  like '" + Prefix + "%'";
            string cMdLocationList = "SELECT top 10 LocationID,Location FROM INV_Location LOC LEFT JOIN TPL_Tenant_MaterialMaster TNT_M ON TNT_M.TenantID=LOC.TenantID AND TNT_M.IsDeleted=0   WHERE TNT_M.MaterialMasterID=" + MaterialMasterID + " and  IsQuarantine=" + IsQuarantine + " and Location  like '" + Prefix + "%'";
            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0}", rslocationList["Location"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetLocationZones(string Prefix, String WarehouseID)
        {
            List<string> LocationList = new List<string>();

            string cMdLocationList = "select top 10 LocationZoneID,LocationZoneCode from INV_LocationZone where IsActive=1 and IsDeleted=0 and WarehouseID=" + WarehouseID + " and LocationZoneCode  like '" + Prefix + "%'";

            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0},{1}", rslocationList["LocationZoneCode"], rslocationList["LocationZoneID"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat=ResponseFormat.Json)]
        //public string[] GetZones(string Prefix,string WarehouseID)
        //{
        //    List<string> ZoneList = new List<string>();
        //    string cmdlist="select Distinct "
        //    return "";
        //}

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetLocationZonesByWHID(string Prefix, String WarehouseID)
        {
            List<string> LocationList = new List<string>();

            string cMdLocationList = "SELECT DISTINCT LocationZoneID, LocationZoneCode FROM INV_LocationZone ILZ JOIN GEN_Warehouse GW ON GW.WarehouseID = ILZ.WarehouseID JOIN TPL_Tenant TNT ON TNT.AccountID = GW.AccountID where ILZ.IsDeleted = 0 and ILZ.IsActive = 1 AND  ILZ.WarehouseID=" + WarehouseID + " AND GW.AccountID = case when 0 = " + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end";

            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0},{1}", rslocationList["LocationZoneCode"], rslocationList["LocationZoneID"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetLocations(string Prefix, String ProductCategory, String InboundID)
        {
            List<string> LocationList = new List<string>();
            //string cMdLocationList = "select top 10 Location,LocationID from INV_Location loc join INV_LocationZone locz on locz.LocationZoneCode=left(Location,2) and locz.IsDeleted=0 left join INB_RefWarehouse_Details refw on refw.WarehouseID=locz.WarehouseID and refw.IsDeleted=0 and refw.InboundID=" + InboundID + " where (" + InboundID + "=0 or refw.InboundID is not null) and loc.IsDeleted=0 and (" + ProductCategory + " not in (0,3) or  (0=" + ProductCategory + " and left(Location,2)!='Q1') or (3=" + ProductCategory + " and left(Location,2)='P1')) and Location  like '" + Prefix + "%'";

            string cMdLocationList = "select top 10 Location,LocationID from INV_Location loc join INV_LocationZone locz on locz.LocationZoneID=loc.ZoneId and locz.IsDeleted=0  join INB_RefWarehouse_Details refw on refw.WarehouseID=locz.WarehouseID and refw.IsDeleted=0 and refw.InboundID=" + InboundID + " where (" + InboundID + "=0 or refw.InboundID is not null) and loc.IsDeleted=0 and Location  like '" + Prefix + "%'";

            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0},{1}", rslocationList["Location"], rslocationList["LocationID"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetCartonsForInternalTransfer(string Prefix, string TenantID)
        {
            List<string> CartonList = new List<string>();

            string cMdCartonList = "EXEC [sp_INV_GetCartonCodes] @TenantID=" + TenantID + ",@Prefix=" + DB.SQuote(Prefix);

            IDataReader rscMdCartonList = DB.GetRS(cMdCartonList);

            while (rscMdCartonList.Read())
            {
                CartonList.Add(string.Format("{0},{1}", rscMdCartonList["CartonCode"], rscMdCartonList["CartonID"]));
            }
            rscMdCartonList.Close();
            return CartonList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetContainerList(string Prefix)
        {
            List<string> cartonlist = new List<string>();
            string Query = "SELECT TOP 10 CartonID, CartonCode FROM INV_Carton WHERE (CartonCode LIKE '" + Prefix + "%' OR CartonCode LIKE '%" + Prefix + "%' OR CartonCode LIKE '%" + Prefix + "')";
            IDataReader rscMdCartonList = DB.GetRS(Query);

            while (rscMdCartonList.Read())
            {
                cartonlist.Add(string.Format("{0},{1}", rscMdCartonList["CartonCode"], rscMdCartonList["CartonID"]));
            }
            rscMdCartonList.Close();
            return cartonlist.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetAllLocations(string Prefix, String ProductCategory)
        {
            List<string> LocationList = new List<string>();
            //string cMdLocationList = "select top 10 Location,LocationID from INV_Location loc join INV_LocationZone locz on locz.LocationZoneCode=left(Location,2) and locz.IsDeleted=0 left join INB_RefWarehouse_Details refw on refw.WarehouseID=locz.WarehouseID and refw.IsDeleted=0 and refw.InboundID=" + InboundID + " where (" + InboundID + "=0 or refw.InboundID is not null) and loc.IsDeleted=0 and (" + ProductCategory + " not in (0,3) or  (0=" + ProductCategory + " and left(Location,2)!='Q1') or (3=" + ProductCategory + " and left(Location,2)='P1')) and Location  like '" + Prefix + "%'";

            string cMdLocationList = "select top 10 Location,LocationID from INV_Location loc where loc.IsDeleted=0 and Location  like '" + Prefix + "%'";

            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0},{1}", rslocationList["Location"], rslocationList["LocationID"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CheckContainerMapping(string Prefix, string Carton)
        {

            string drlStatement = "EXEC [dbo].[sp_CheckContainerLocation] @CartonCode=" + DB.SQuote(Carton);
            int result = DB.GetSqlN(drlStatement);
            if (result == 0)
                displayStat = "block";
            else displayStat = "none";

            return displayStat;
        }
        //For Tariff Group List
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetTariffGroupList(String prefix)
        {
            List<String> TariffGroup = new List<string>();
            String query = "SELECT top 10 ActivityRateGroupID,ActivityRateGroup FROM TPL_Activity_RateGroup WHERE IsDeleted=0  and ActivityRateGroup like '" + prefix + "%'";
            IDataReader rsTariffGroup = DB.GetRS(query);
            while (rsTariffGroup.Read())
            {
                TariffGroup.Add(String.Format("{0},{1}", rsTariffGroup["ActivityRateGroup"], rsTariffGroup["ActivityRateGroupID"]));
            }
            rsTariffGroup.Close();
            return TariffGroup.ToArray();
        }

        //Get Tariff type
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetActivityTariffType(String prefix, string ActivityRateGroupID)
        {
            List<String> TariffGroup = new List<string>();
            String query = "SELECT top 10 ActivityRateTypeID,ActivityRateType FROM TPL_Activity_RateType WHERE IsDeleted=0 AND ActivityRateGroupID=" + ActivityRateGroupID + " AND ActivityRateType like '" + prefix + "%'";
            IDataReader rsTariffGroup = DB.GetRS(query);
            while (rsTariffGroup.Read())
            {
                TariffGroup.Add(String.Format("{0},{1}", rsTariffGroup["ActivityRateType"], rsTariffGroup["ActivityRateTypeID"]));
            }
            rsTariffGroup.Close();
            return TariffGroup.ToArray();
        }
        //Get Service TPL
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetServiceTypeForTPL(String prefix)
        {
            List<String> TariffGroup = new List<string>();
            String query = "SELECT TOP 10 ServiceTypeID,ServiceType FROM TPL_ServiceType WHERE IsActive=1 AND IsDeleted=0 AND ServiceType LIKE '" + prefix + "%'";
            IDataReader rsTariffGroup = DB.GetRS(query);
            while (rsTariffGroup.Read())
            {
                TariffGroup.Add(String.Format("{0},{1}", rsTariffGroup["ServiceType"], rsTariffGroup["ServiceTypeID"]));
            }
            rsTariffGroup.Close();
            return TariffGroup.ToArray();
        }

        //Get INOUT TPL    
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetInOutForTPL(String prefix, String ActivityRateGroupID)
        {
            List<String> TariffGroup = new List<string>();
            //String query = "SELECT DISTINCT TOP 10 A_RT.InOutID,InOutType FROM TPL_Activity_RateType A_RT JOIN GEN_InOut IOUT ON IOUT.InOutID=A_RT.InOutID AND IOUT.IsActive=1 AND IOUT.IsDeleted=0 WHERE A_RT.IsActive=1 AND A_RT.IsDeleted=0 AND A_RT.ActivityRateGroupID=" + ActivityRateGroupID + " AND InOutType LIKE '" + prefix + "%'";

            String query = "SELECT DISTINCT TOP 10 InOutID,InOutType FROM GEN_InOut WHERE InOutType LIKE '" + prefix + "%'";

            IDataReader rsTariffGroup = DB.GetRS(query);
            while (rsTariffGroup.Read())
            {
                TariffGroup.Add(String.Format("{0},{1}", rsTariffGroup["InOutType"], rsTariffGroup["InOutID"]));
            }
            rsTariffGroup.Close();
            return TariffGroup.ToArray();
        }
        //Get Warehouse Auto
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetWareHouse_Auto(String prefix)
        {
            List<String> WareHouse = new List<string>();
            // String query = "SELECT WarehouseID, WHCode FROM GEN_Warehouse WHERE AccountID=" + cp.AccountID + " AND (WHCode LIKE '%" + prefix + "' OR WHCode LIKE '%" + prefix + "%' OR WHCode LIKE '" + prefix + "%')";
            string mmSql = "EXEC [dbo].[SP_RPT_WHDROPDOWN] @USERID= " + cp.UserID + ",@PRIFIX =" + DB.SQuote(prefix);
            IDataReader rsWareHouse = DB.GetRS(mmSql);
            while (rsWareHouse.Read())
            {
                WareHouse.Add(String.Format("{0},{1}", rsWareHouse["WHCode"], rsWareHouse["WarehouseID"]));
            }
            rsWareHouse.Close();
            return WareHouse.ToArray();
        }
      

        //Get UoM For TPL
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetUoMForTPL(String prefix)
        {
            List<String> TariffGroup = new List<string>();
            String query = "SELECT TOP 10 UoMID,UoM FROM GEN_UoM WHERE IsActive=1 AND IsDeleted=0 AND UoM LIKE '" + prefix + "%'";
            IDataReader rsTariffGroup = DB.GetRS(query);
            while (rsTariffGroup.Read())
            {
                TariffGroup.Add(String.Format("{0},{1}", rsTariffGroup["UoM"], rsTariffGroup["UoMID"]));
            }
            rsTariffGroup.Close();
            return TariffGroup.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetTenantandBilltype(String prefix)
        {
            List<String> sid = new List<string>();
            String query = "select TOP 10 convert(nvarchar,TT.TenantID)+'`'+convert(nvarchar,TTis.BillTypeID) [TenantID],TenantName from TPL_Tenant TT join TPL_Tenant_InvoiceSettings TTIS on TTIS.TenantID=TT.TenantID where TT.TenantID!=0 and TT.IsActive=1 and TT.IsDeleted=0  and TenantName like '" + prefix + "%'";
            IDataReader rsfe = DB.GetRS(query);
            while (rsfe.Read())
            {
                sid.Add(String.Format("{0},{1}", rsfe["TenantName"], rsfe["TenantID"]));
            }
            rsfe.Close();
            return sid.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetTenantList(string prefix)
        {

            List<string> TenantList = new List<string>();
            //<!------------------Procedure Converting------------->//

            string TenantListSql = "Exec [dbo].[USP_MST_DropTenantAccountWise] @prefix='" + prefix + "', @AccountID=" + cp.AccountID.ToString() + ",@TenantID=" + cp.TenantID.ToString() + "";


            IDataReader rsTenantList = DB.GetRS(TenantListSql);

            while (rsTenantList.Read())
            {
                TenantList.Add(string.Format("{0},{1}", rsTenantList["TenantName"], rsTenantList["TenantID"]));
            }
            rsTenantList.Close();
            return TenantList.ToArray();

            //List<string> Tenantlist = new List<string>();
            ////string cMdTenantlist = "SELECT TOP 10 TenantID,TenantName FROM [TPL_Tenant] where  TenantID<>0 AND IsDeleted=0 AND IsActive=1 and AccountID=" + cp.AccountID + " and TenantName like '" + prefix + "%' order by TenantName ";
            //string cMdTenantlist = "SELECT TOP 10 TenantID,TenantCode FROM [TPL_Tenant] where  TenantID<>0 AND IsDeleted=0 AND IsActive=1 and AccountID=" + cp.AccountID + " and TenantCode like '" + prefix + "%' order by TenantCode ";

            //IDataReader drcMdcMdTenantlist = DB.GetRS(cMdTenantlist);

            //while (drcMdcMdTenantlist.Read())
            //{
            //    Tenantlist.Add(string.Format("{0},{1}", drcMdcMdTenantlist["TenantCode"], drcMdcMdTenantlist["TenantID"]));
            //}
            //drcMdcMdTenantlist.Close();
            //return Tenantlist.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetWarehouseTenant(string prefix, int WHID)
        {

            List<string> TenantList = new List<string>();
            //<!------------------Procedure Converting------------->//

            string TenantListSql = "select TC.TenantID,TenantName+'-'+TenantCode TenantName from TPL_Tenant_Contract TC "+
                                        "JOIN TPL_Tenant TNT ON TNT.TenantID = TC.TenantID AND TNT.Isactive = 1 AND TNT.IsDeleted = 0"+
                                          "  WHERE WarehouseID = "+ WHID + " AND (TenantName like '" + prefix + "%' or TenantCode like '%" + prefix + "%') AND TC.IsActive = 1 AND TC.IsDeleted = 0";

       
            IDataReader rsTenantList = DB.GetRS(TenantListSql);

            while (rsTenantList.Read())
            {
                TenantList.Add(string.Format("{0},{1}", rsTenantList["TenantName"], rsTenantList["TenantID"]));
            }
            rsTenantList.Close();
            return TenantList.ToArray();
        }


        // Method written by Ganesh --- Tenant Drop down data should be displayed by User base and Warehouse 
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadTenantDataByUserWH(string prefix)
        {

            List<string> TenantList = new List<string>();

            string TenantListSql = "Exec [dbo].[USP_LoadTenantDataByUserWH] @prefix='" + prefix + "', @AccountID=" + cp.AccountID.ToString() + ",@USERID=" + cp.UserID.ToString() + ",@TenantID=" + cp.TenantID.ToString() + "";
            IDataReader rsTenantList = DB.GetRS(TenantListSql);

            while (rsTenantList.Read())
            {
                TenantList.Add(string.Format("{0},{1}", rsTenantList["TenantName"], rsTenantList["TenantID"]));
            }
            rsTenantList.Close();
            return TenantList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetTenantList_CC(string prefix, string AccountID)
        {

            List<string> TenantList = new List<string>();
            //<!------------------Procedure Converting------------->//

            string TenantListSql = "Exec [dbo].[USP_MST_DropTenantAccountWise] @prefix='" + prefix + "', @AccountID=" + AccountID + "";


            IDataReader rsTenantList = DB.GetRS(TenantListSql);

            while (rsTenantList.Read())
            {
                TenantList.Add(string.Format("{0},{1}", rsTenantList["TenantName"], rsTenantList["TenantID"]));
            }
            rsTenantList.Close();
            return TenantList.ToArray();

            //List<string> Tenantlist = new List<string>();
            ////string cMdTenantlist = "SELECT TOP 10 TenantID,TenantName FROM [TPL_Tenant] where  TenantID<>0 AND IsDeleted=0 AND IsActive=1 and AccountID=" + cp.AccountID + " and TenantName like '" + prefix + "%' order by TenantName ";
            //string cMdTenantlist = "SELECT TOP 10 TenantID,TenantCode FROM [TPL_Tenant] where  TenantID<>0 AND IsDeleted=0 AND IsActive=1 and AccountID=" + cp.AccountID + " and TenantCode like '" + prefix + "%' order by TenantCode ";

            //IDataReader drcMdcMdTenantlist = DB.GetRS(cMdTenantlist);

            //while (drcMdcMdTenantlist.Read())
            //{
            //    Tenantlist.Add(string.Format("{0},{1}", drcMdcMdTenantlist["TenantCode"], drcMdcMdTenantlist["TenantID"]));
            //}
            //drcMdcMdTenantlist.Close();
            //return Tenantlist.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetFrieghtCompanyData(string prefix)
        {
            List<string> Tenantlist = new List<string>();
            //string cMdTenantlist = "SELECT TOP 10 TenantID,TenantName FROM [TPL_Tenant] where  TenantID<>0 AND IsDeleted=0 AND IsActive=1 and AccountID=" + cp.AccountID + " and TenantName like '" + prefix + "%' order by TenantName ";
            string cMdTenantlist = "select top 10 FreightCompanyID,FreightCompanyCode from GEN_FreightCompany where IsDeleted=0 and IsActive=1 and FreightCompanyCode  like '" + prefix + "%' order by FreightCompanyCode ";

            IDataReader drcMdcMdTenantlist = DB.GetRS(cMdTenantlist);

            while (drcMdcMdTenantlist.Read())
            {
                Tenantlist.Add(string.Format("{0},{1}", drcMdcMdTenantlist["FreightCompanyCode"], drcMdcMdTenantlist["FreightCompanyID"]));
            }
            drcMdcMdTenantlist.Close();
            return Tenantlist.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] getVehicleForGateEntry(string prefix, int TenantId)
        {
            List<string> Tenantlist = new List<string>();
            string cMdTenantlist = "select top 10 YM_MST_Vehicle_ID,RegistrationNumber,(StorageLength*StorageWidth*StorageHeight)/1000000 storagevol,(VehicleLength*VehicleWidth*VechicleHeight)/1000000 vehiclevol,MaxStorageWeight , " +
                                    "MaxTotalWeight from YM_MST_Vehicles vh join YM_MST_VehicleDimensions dimen on vh.YM_MST_Vehicle_ID=dimen.VehicleID and dimen.isdeleted=0 and dimen.IsActive=1 where accountid=" + cp.AccountID + " and vh.IsDeleted=0 and vh.IsActive=1 and RegistrationNumber like '" + prefix + "%' order by RegistrationNumber ";

            IDataReader drcMdcMdTenantlist = DB.GetRS(cMdTenantlist);

            while (drcMdcMdTenantlist.Read())
            {
                Tenantlist.Add(string.Format("{0},{1},{2},{3},{4},{5}", drcMdcMdTenantlist["RegistrationNumber"], drcMdcMdTenantlist["YM_MST_Vehicle_ID"], drcMdcMdTenantlist["storagevol"], drcMdcMdTenantlist["vehiclevol"], drcMdcMdTenantlist["MaxStorageWeight"], drcMdcMdTenantlist["MaxTotalWeight"]));
            }
            drcMdcMdTenantlist.Close();
            return Tenantlist.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] getDocksForGateEntry(string prefix, int warehouseid, string shipmentid)
        {
            List<string> Tenantlist = new List<string>();
            string cMdTenantlist = "";
            if (Convert.ToInt32(shipmentid) == 1)
            {
                cMdTenantlist = "select DockID,DockName from GEN_Dock where IsDeleted=0 and IsActive=1 and WarehouseID=" + warehouseid + " and DockTypeID in (1,3) and DockName like '" + prefix + "%' order by DockName ";
            }
            else
            {
                cMdTenantlist = "select DockID,DockName from GEN_Dock where IsDeleted=0 and IsActive=1 and WarehouseID=" + warehouseid + " and DockTypeID in (2,3) and DockName like '" + prefix + "%' order by DockName ";
            }


            IDataReader drcMdcMdTenantlist = DB.GetRS(cMdTenantlist);

            while (drcMdcMdTenantlist.Read())
            {
                Tenantlist.Add(string.Format("{0},{1}", drcMdcMdTenantlist["DockName"], drcMdcMdTenantlist["DockID"]));
            }
            drcMdcMdTenantlist.Close();
            return Tenantlist.ToArray();
        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetInboundDataForGateEntry(string prefix, int TenantId, string ShipmentType)
        {
            List<string> Tenantlist = new List<string>();
            string cMdTenantlist = "";
            if (Convert.ToInt32(ShipmentType) == 1)
            {
                cMdTenantlist = "select top 10 InboundID shipmentid,StoreRefNo shipmenttype from INB_Inbound where IsDeleted=0 and IsActive=1 and InboundStatusID in (3,5) and TenantID=" + TenantId + " and StoreRefNo like '" + prefix + "%' ";
            }
            else
            {
                cMdTenantlist = "select top 10 OutboundID shipmentid,OBDNumber shipmenttype from OBD_Outbound where IsDeleted=0 and IsActive=1 and TenantID=" + TenantId + " and OBDNumber like '" + prefix + "%' ";
            }



            IDataReader drcMdcMdTenantlist = DB.GetRS(cMdTenantlist);

            while (drcMdcMdTenantlist.Read())
            {
                Tenantlist.Add(string.Format("{0},{1}", drcMdcMdTenantlist["shipmenttype"], drcMdcMdTenantlist["shipmentid"]));
            }
            drcMdcMdTenantlist.Close();
            return Tenantlist.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetCountryForGateEntry(string prefix)
        {
            List<string> Tenantlist = new List<string>();
            string cMdTenantlist = "select top 10 CountryMasterID,CountryName from GEN_CountryMaster where IsDeleted=0 and IsActive=1 and CountryName like  '" + prefix + "%' ";

            IDataReader drcMdcMdTenantlist = DB.GetRS(cMdTenantlist);

            while (drcMdcMdTenantlist.Read())
            {
                Tenantlist.Add(string.Format("{0},{1}", drcMdcMdTenantlist["CountryName"], drcMdcMdTenantlist["CountryMasterID"]));
            }
            drcMdcMdTenantlist.Close();
            return Tenantlist.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetStateForGateEntry(string prefix, string countryid)
        {
            List<string> Tenantlist = new List<string>();
            string cMdTenantlist = "select top 10 StateMasterID,statename+' - '+StateCode state from GEN_StateMaster where IsDeleted=0 and IsActive=1 and CountryMasterID=" + countryid + " and (statename like  '" + prefix + "%' or StateCode like  '" + prefix + "%')  ";

            IDataReader drcMdcMdTenantlist = DB.GetRS(cMdTenantlist);

            while (drcMdcMdTenantlist.Read())
            {
                Tenantlist.Add(string.Format("{0},{1}", drcMdcMdTenantlist["state"], drcMdcMdTenantlist["StateMasterID"]));
            }
            drcMdcMdTenantlist.Close();
            return Tenantlist.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetArriveCityForGateEntry(string prefix, string stateid)
        {
            List<string> Tenantlist = new List<string>();
            string cMdTenantlist = "select top 10 CityMasterID,CityName from GEN_CityMaster where IsDeleted=0 and statemasterid=" + stateid + " and IsActive=1 and CityName like '" + prefix + "%' ";

            IDataReader drcMdcMdTenantlist = DB.GetRS(cMdTenantlist);

            while (drcMdcMdTenantlist.Read())
            {
                Tenantlist.Add(string.Format("{0},{1}", drcMdcMdTenantlist["CityName"], drcMdcMdTenantlist["CityMasterID"]));
            }
            drcMdcMdTenantlist.Close();
            return Tenantlist.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetCityForGateEntry(string prefix, string stateid, string gateid)
        {
            List<string> Tenantlist = new List<string>();
            string cMdTenantlist = "select top 10 CityMasterID,CityName from GEN_CityMaster where IsDeleted=0 and statemasterid=" + stateid + " and CityMasterID not in (select PreferredCity_ID from YM_TRN_VehicleDestPreferences where IsDeleted=0 and IsActive=1 and YM_TRN_VEhicleYardAvailability_ID=" + gateid + ") and IsActive=1 and CityName like '" + prefix + "%' ";

            IDataReader drcMdcMdTenantlist = DB.GetRS(cMdTenantlist);

            while (drcMdcMdTenantlist.Read())
            {
                Tenantlist.Add(string.Format("{0},{1}", drcMdcMdTenantlist["CityName"], drcMdcMdTenantlist["CityMasterID"]));
            }
            drcMdcMdTenantlist.Close();
            return Tenantlist.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetMaterialsForGateEntry(string prefix, string tenantid, int inboundID, int Outboundid, string gateid)
        {
            List<string> Tenantlist = new List<string>();
            string cMdTenantlist = "[SP_GetItemDetailsForGateEntry] @GateId=" + gateid + ",@inboundID=" + inboundID + ",@Outboundid=" + Outboundid + ",@Prefix=" + DB.SQuote(prefix) + ",@TenantId=" + tenantid;

            IDataReader drcMdcMdTenantlist = DB.GetRS(cMdTenantlist);

            while (drcMdcMdTenantlist.Read())
            {
                Tenantlist.Add(string.Format("{0},{1}", drcMdcMdTenantlist["MCode"], drcMdcMdTenantlist["MaterialMasterID"]));
            }
            drcMdcMdTenantlist.Close();
            return Tenantlist.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetOBD_ItemsForRevert(string prefix, int type, int TenantID)
        {

            List<string> Tenantlist = new List<string>();
            string cMdTenantlist = "";



            if (type == 2)
            {
                //    cMdTenantlist = "select top 10 outboundid ID,obdnumber Name from obd_outbound where IsDeleted=0 and IsActive=1 and  tenantid= " + TenantID +
                //    "and deliverystatusid=" + 5 + " and (OBDNumber like '" + prefix + "%' or OBDNumber like '%" + prefix + "%' or OBDNumber like '%" + prefix + "')";
                // above comment by ganesh  and bellow script by me.

                cMdTenantlist = "SELECT TOP  10 OBD.outboundid ID,obdnumber Name FROM obd_outbound  obd JOIN OBD_RefWarehouse_Details rd ON rd.OutboundID = obd.OutboundID JOIN GEN_User_Warehouse uw ON uw.WarehouseID = rd.WarehouseID and rd.IsActive = 1 and obd.IsActive = 1 and obd.IsDeleted = 0 and uw.IsActive = 1  WHERE  tenantid= " + TenantID +
                                 "and deliverystatusid=" + 5 + " AND UW.USERID="+cp.UserID+" and (OBDNumber like '" + prefix + "%' or OBDNumber like '%" + prefix + "%' or OBDNumber like '%" + prefix + "')";
                               
            }
            else if (type == 3)
            {
                //cMdTenantlist = "select top 10 outboundid ID,obdnumber Name from obd_outbound where IsDeleted=0 and IsActive=1 and  tenantid= " + TenantID +
                //Commented by M.D.Prasad//"and deliverystatusid=" + 6 + " and (OBDNumber like '" + prefix + "%' or OBDNumber like '%" + prefix + "%' or OBDNumber like '%" + prefix + "')";
                //"and deliverystatusid IN (6,7) and (OBDNumber like '" + prefix + "%' or OBDNumber like '%" + prefix + "%' or OBDNumber like '%" + prefix + "')";

                cMdTenantlist = "SELECT TOP  10 OBD.outboundid ID,obdnumber Name FROM obd_outbound  obd JOIN OBD_RefWarehouse_Details rd ON rd.OutboundID = obd.OutboundID JOIN GEN_User_Warehouse uw ON uw.WarehouseID = rd.WarehouseID and rd.IsActive = 1 and obd.IsActive = 1 and obd.IsDeleted = 0 and uw.IsActive = 1  WHERE  tenantid= " + TenantID +
                                 "and deliverystatusid IN (6,7) AND  UW.USERID=" + cp.UserID + " and (OBDNumber like '" + prefix + "%' or OBDNumber like '%" + prefix + "%' or OBDNumber like '%" + prefix + "')";

            }
            else
            {
                // cMdTenantlist = "SELECT TOP 10 MM.MaterialMasterID ID, MM.MCode Name FROM TPL_Tenant_MaterialMaster AS TM JOIN MMT_MaterialMaster AS MM ON TM.MaterialMasterID = MM.MaterialMasterID AND MM.IsActive = 1 AND MM.IsDeleted = 0 AND TM.IsActive = 1 AND TM.IsDeleted = 0 WHERE TM.TenantID = " + TenantID + " AND MM.MCode LIKE '%" + prefix + "%'";
                cMdTenantlist = "SELECT TOP 10 MM.MaterialMasterID ID, MM.MCode Name FROM TPL_Tenant_MaterialMaster AS TM JOIN MMT_MaterialMaster AS MM ON TM.MaterialMasterID = MM.MaterialMasterID AND MM.IsActive = 1 AND MM.IsDeleted = 0 AND TM.IsActive = 1 AND TM.IsDeleted = 0 WHERE TM.TenantID = " + TenantID + " AND MM.MCode LIKE '%" + prefix + "%'";


            }


            //if(type==2)
            //{
            //    cMdTenantlist= "select top 10 outboundid ID,obdnumber Name from obd_outbound where IsDeleted=0 and IsActive=1 and  tenantid in (select TenantID from TPL_Tenant where AccountID=" + cp.AccountID + ") " +
            //    "and deliverystatusid=" + 5 + " and (OBDNumber like '" + prefix + "%' or OBDNumber like '%" + prefix + "%' or OBDNumber like '%" + prefix + "')";
            //}
            //else if (type == 3)
            //{
            //    cMdTenantlist = "select top 10 outboundid ID,obdnumber Name from obd_outbound where IsDeleted=0 and IsActive=1 and  tenantid in (select TenantID from TPL_Tenant where AccountID=" + cp.AccountID + ") " +
            //   //Commented by M.D.Prasad//"and deliverystatusid=" + 6 + " and (OBDNumber like '" + prefix + "%' or OBDNumber like '%" + prefix + "%' or OBDNumber like '%" + prefix + "')";
            //   "and deliverystatusid IN (6,7) and (OBDNumber like '" + prefix + "%' or OBDNumber like '%" + prefix + "%' or OBDNumber like '%" + prefix + "')";
            //}
            //else
            //{
            //    cMdTenantlist = "select top 10 MaterialMasterID ID,mcode Name from mmt_materialmaster where IsDeleted=0 and isactive=1 and (MCode like '" + prefix + "%' or MCode like '%" + prefix + "%' or MCode like '%" + prefix + "') " +
            //                        "   and  tenantid in (select TenantID from TPL_Tenant where AccountID = " + cp.AccountID+")";
            //}

            IDataReader drcMdcMdTenantlist = DB.GetRS(cMdTenantlist);

            while (drcMdcMdTenantlist.Read())
            {
                Tenantlist.Add(string.Format("{0},{1}", drcMdcMdTenantlist["Name"], drcMdcMdTenantlist["ID"]));
            }
            drcMdcMdTenantlist.Close();
            return Tenantlist.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetActivityRateTypeAdmin(String prefix, String ActivityRateGroupID)
        {
            List<String> sid = new List<string>();
            String query = "select ActivityRateTypeID,ActivityRateType from TPL_Activity_RateType where ActivityRateGroupID=" + ActivityRateGroupID + " and IsActive=1 and IsDeleted=0 and ActivityRateType like '" + prefix + "%'";
            IDataReader rsfe = DB.GetRS(query);
            while (rsfe.Read())
            {
                sid.Add(String.Format("{0},{1}", rsfe["ActivityRateType"], rsfe["ActivityRateTypeID"]));
            }
            rsfe.Close();
            return sid.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] getContainers(String prefix, String WarehouseID)
        {
            List<String> sid = new List<string>();
            String query = "SELECT TOP 10 CartonID,CartonCode FROM INV_Carton  WHERE  ContainerTypeID=1 AND IsActive=1 AND IsDeleted=0 AND WareHouseID=" + WarehouseID + " AND CartonCode like '" + prefix + "%'";
            IDataReader rsfe = DB.GetRS(query);
            while (rsfe.Read())
            {
                sid.Add(String.Format("{0},{1}", rsfe["CartonCode"], rsfe["CartonID"]));
            }
            rsfe.Close();
            return sid.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetActivityRateGroupAdmin(String prefix)
        {
            List<String> sid = new List<string>();
            String query = "select ActivityRateGroupID,ActivityRateGroup from TPL_Activity_RateGroup where IsActive=1 and IsDeleted=0 and ActivityRateGroup like '" + prefix + "%'";
            IDataReader rsfe = DB.GetRS(query);
            while (rsfe.Read())
            {
                sid.Add(String.Format("{0},{1}", rsfe["ActivityRateGroup"], rsfe["ActivityRateGroupID"]));
            }
            rsfe.Close();
            return sid.ToArray();
        }


        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public string[] GetTaxation(string prefix)
        //{
        //    List<string> sid = new List<string>();
        //    string query = "select * from TPL_Taxation WHERE IsActive=1 AND IsDeleted=0" ;
        //    IDataReader rsfe = DB.GetRS(query);
        //    while (rsfe.Read())
        //    {
        //        sid.Add(string.Format("{0},{1}", rsfe["TaxCode"], rsfe["TaxationID"]));
        //    }
        //    rsfe.Close();
        //    return sid.ToArray();
        //}




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetActivityRateAdmin(String prefix, String ActivityRateTypeID)
        {
            List<String> sid = new List<string>();
            String query = "select ActivityRateID,ActivityRateName from TPL_Activity_Rate where ActivityRateTypeID=" + ActivityRateTypeID + " and IsActive=1 and IsDeleted=0 and ActivityRateName like '" + prefix + "%'";
            IDataReader rsfe = DB.GetRS(query);
            while (rsfe.Read())
            {
                sid.Add(String.Format("{0},{1}", rsfe["ActivityRateName"], rsfe["ActivityRateID"]));
            }
            rsfe.Close();
            return sid.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetActivityRateAdmin_Tarif(String prefix, String ActivityRateTypeID, String WareHouseId)
        {
            List<String> sid = new List<string>();
            String query = "select ActivityRateID,ActivityRateName from TPL_Activity_Rate where ActivityRateTypeID=" + ActivityRateTypeID + " AND WareHouseID = " + WareHouseId + " and IsActive=1 and IsDeleted=0 and ActivityRateName like '" + prefix + "%'";
            IDataReader rsfe = DB.GetRS(query);
            while (rsfe.Read())
            {
                sid.Add(String.Format("{0},{1}", rsfe["ActivityRateName"], rsfe["ActivityRateID"]));
            }
            rsfe.Close();
            return sid.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetActivityTariff(String prefix, string ActivityRateGroupID, string ActivityRateTypeID)
        {
            List<String> ActivityTariffType = new List<string>();

            if (ActivityRateTypeID == "")
                ActivityRateTypeID = "0";

            //String TariffTypeQuery = "SELECT TOP 10 ActivityRateTypeID,ActivityRateName FROM TPL_Activity_Rate WHERE IsDeleted=0 AND (0=" + ActivityRateTypeID + " OR ActivityRateTypeID=" + ActivityRateTypeID + ")  AND ActivityRateName like '" + prefix + "%'";

            // String TariffTypeQuery = "SELECT Distinct TOP 10 ACR.ActivityRateName,ACR.ActivityRateID FROM TPL_Activity_Rate ACR JOIN TPL_Activity_RateType ACT ON ACT.ActivityRateTypeID=ACR.ActivityRateTypeID AND ACT.IsActive=1 AND ACT.IsDeleted=0 WHERE ACR.IsActive=1 AND ACR.IsDeleted=0 AND ACT.ActivityRateGroupID=" + ActivityRateGroupID + " AND (0=" + ActivityRateTypeID + " OR ACT.ActivityRateTypeID=" + ActivityRateTypeID + ") AND ActivityRateName like '" + prefix + "%'";  commented by lalitha on 19/02/2019

            String TariffTypeQuery = "SELECT Distinct TOP 10 ACR.ActivityRateName,ACR.ActivityRateID FROM TPL_Activity_Rate ACR JOIN TPL_Activity_RateType ACT ON ACT.ActivityRateTypeID=ACR.ActivityRateTypeID AND ACT.IsActive=1 AND ACT.IsDeleted=0 WHERE ACR.IsActive=1 AND ACR.IsDeleted=0 AND ACT.ActivityRateGroupID=" + ActivityRateGroupID + " AND (0=" + ActivityRateTypeID + " OR ACT.ActivityRateTypeID=" + ActivityRateTypeID + ") AND ActivityRateName like '" + prefix + "%'";

            IDataReader rsTariffType = DB.GetRS(TariffTypeQuery);
            while (rsTariffType.Read())
            {
                ActivityTariffType.Add(String.Format("{0},{1}", rsTariffType["ActivityRateName"], rsTariffType["ActivityRateID"]));
            }
            rsTariffType.Close();
            return ActivityTariffType.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GetActivityRateTypeandTaxation(String prefix, string taxation)
        {
            List<String> TariffGroup = new List<string>();
            String query = "SELECT top 10 ActivityRateTypeID,ActivityRateType FROM TPL_Activity_RateType WHERE IsActive=1 and IsDeleted=0  AND ActivityRateType like '" + prefix + "%'";
            IDataReader rsTariffGroup = DB.GetRS(query);
            while (rsTariffGroup.Read())
            {
                TariffGroup.Add(String.Format("{0},{1}", rsTariffGroup["ActivityRateType"], rsTariffGroup["ActivityRateTypeID"]));
            }
            rsTariffGroup.Close();
            return TariffGroup.ToArray();
        }
        #endregion  -----------------  3PL Billing -------------------------

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadLoction_Auto(string Prefix)
        {
            List<String> list = new List<string>();
            String sql = "SELECT  LocationID, Location FROM INV_Location  WHERE IsActive = 1 AND IsDeleted = 0 AND (Location LIKE '%" + Prefix + "' OR Location LIKE '%" + Prefix + "%' OR Location LIKE '" + Prefix + "%')";

            IDataReader reader = DB.GetRS(sql);
            while (reader.Read())
            {
                list.Add(String.Format("{0},{1}", reader["Location"], reader["LocationID"]));
            }

            reader.Close();
            return list.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadIndustries_Auto(string Prefix)
        {
            List<String> list = new List<string>();
            String sql = "SELECT  IndustryID, IndustryName FROM GEN_Industry  WHERE IsActive = 1 AND IsDeleted = 0 AND (IndustryName LIKE '%" + Prefix + "' OR IndustryName LIKE '%" + Prefix + "%' OR IndustryName LIKE '" + Prefix + "%')";

            IDataReader reader = DB.GetRS(sql);
            while (reader.Read())
            {
                list.Add(String.Format("{0},{1}", reader["IndustryName"], reader["IndustryID"]));
            }

            reader.Close();
            return list.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] GettingTypesDataforCurrentStock(string Prefix)
        {
            List<String> list = new List<string>();
            list.Add(String.Format("{0},{1}", "PO No.", "1"));
            list.Add(String.Format("{0},{1}", "Store Ref# No.", "2"));
            list.Add(String.Format("{0},{1}", "GRN No.", "3"));

            return list.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadLoction_AutoForBin(string Prefix)
        {
            List<String> list = new List<string>();
            //String sql = "SELECT TOP 10  LocationID, Location FROM INV_Location  WHERE IsActive = 1 AND IsDeleted = 0 AND (Location LIKE '%" + Prefix + "' OR Location LIKE '%" + Prefix + "%' OR Location LIKE '" + Prefix + "%')";
            String sql = " SELECT DISTINCT TOP 10 LocationID, loc.Location FROM GEN_Warehouse wh JOIN INV_LocationZone lz ON lz.WarehouseID = wh.WarehouseID JOIN INV_Location loc ON loc.ZoneId = lz.LocationZoneID WHERE loc.IsActive = 1 AND loc.IsDeleted = 0 AND loc.IsFastMoving = 1 AND(loc.Location LIKE '%" + Prefix + "' OR loc.Location LIKE '%" + Prefix + "%' OR loc.Location LIKE '" + Prefix + "%') AND WH.AccountID = case when 0 = " + cp.AccountID.ToString() + " then WH.AccountID else " + cp.AccountID.ToString() + " end";

            IDataReader reader = DB.GetRS(sql);
            while (reader.Read())
            {
                list.Add(String.Format("{0},{1}", reader["Location"], reader["LocationID"]));
            }

            reader.Close();
            return list.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWarehouseByLoc(string prefix)
        {
            List<string> mmList = new List<string>();

            //string mmSql = "select StoreRefNo,InboundID from INB_Inbound inb join TPL_Tenant tnt on tnt.TenantID = inb.TenantID join GEN_Account acc on acc.AccountID = tnt.AccountID where inb.IsActive = 1 AND inb.IsDeleted = 0 and acc.AccountID =" + cp.AccountID + "AND StoreRefNo  like '" + prefix + "%'";
            string mmSql = "SELECT DISTINCT WarehouseID, WHCode + ' - ' + Location as WarehouseLoc FROM GEN_Warehouse WHERE IsActive = 1 AND IsDeleted = 0 AND AccountID = case when 0 = " + cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end AND Location like '" + prefix + "%'";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["WarehouseLoc"], rsMCodeList["WarehouseID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadSpplierDataBasedTenant(string prefix, int TenantID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "SELECT SUP.SupplierID, SUP.SupplierName FROM TPL_Tenant_Supplier TSUP INNER JOIN MMT_Supplier SUP ON SUP.SupplierID = TSUP.SupplierID AND SUP.IsActive = 1 AND SUP.IsDeleted = 0 INNER JOIN TPL_Tenant TNT ON TNT.TenantID = TSUP.TenantID AND TNT.IsDeleted = 0 AND TNT.IsActive = 1 WHERE TSUP.TenantID = " + TenantID + "AND SUP.SupplierName like '" + prefix + "%' ORDER BY SUP.SupplierName ASC";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["SupplierName"], rsMCodeList["SupplierID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMaterialDataBasedSupplier(string prefix, int SupplierID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select MM.MaterialMasterID, MM.MCode from MMT_MaterialMaster MM JOIN MMT_MaterialMaster_Supplier MMS ON MMS.MaterialMasterID = MM.MaterialMasterID AND MMS.IsActive=1 AND MMS.IsDeleted=0 JOIN TPL_Tenant_Supplier TS ON TS.SupplierID = MMS.SupplierID AND TS.IsActive=1 and TS.IsDeleted=0 WHERE MMS.SupplierID = " + SupplierID + " AND MM.MCode like '" + prefix + "%'";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadCountry(string prefix)
        {
            List<string> mmList = new List<string>();

            // string mmSql = "select Top 10 CountryMasterID,CountryName from GEN_CountryMaster where IsActive=1 and IsDeleted=0 AND CountryName like '" + prefix + "%'"+ "order by CountryName";
            string mmSql = "Exec [dbo].[USP_LoadCountryDropDown] @Prefix = '" + prefix + "'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["CountryName"], rsMCodeList["CountryMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadCurrency(string prefix, int CountryId)
        {
            List<string> mmList = new List<string>();

            //string mmSql = "select CurrencyID,Currency from GEN_Currency where IsActive=1 and IsDeleted=0 and CountryID =" + CountryId + "AND Currency like '" + prefix + "%'";
            string mmSql = "Exec [dbo].[USP_MST_GetCurrencyByCountry] @Prefix='" + prefix + "',@CountryID = " + CountryId + "";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["Currency"], rsMCodeList["CurrencyID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadState(string prefix, int CountryId)
        {
            List<string> mmList = new List<string>();

            // string mmSql = "select TOP 10 StateMasterID,StateName from GEN_StateMaster where IsDeleted=0 and IsActive=1 and CountryMasterID=" + CountryId + "AND StateName like '" + prefix + "%'";
            string mmSql = "Exec [dbo].[USP_MST_GetStateByCountry] @Prefix='" + prefix + "',@CountryID= " + CountryId + " ";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["StateName"], rsMCodeList["StateMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadCity(string prefix, int StateId)
        {
            List<string> mmList = new List<string>();

            //  string mmSql = "select CityMasterID,CityName from GEN_CityMaster where IsDeleted=0 and IsActive=1 and StateMasterID=" + StateId + "AND CityName like '" + prefix + "%'";
            string mmSql = "Exec [dbo].[USP_MST_GetCityDrop] @Prefix='" + prefix + "%',@StateID=" + StateId;
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["CityName"], rsMCodeList["CityMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadZipCodes(string prefix, int CityId)
        {
            List<string> mmList = new List<string>();

            //string mmSql = "select ZipCodeID,ZipCode from GEN_ZipCode where IsDeleted=0 and IsActive=1 and CityMasterID=" + CityId + "AND ZipCode like '" + prefix + "%'";
            string mmSql = "Exec [dbo].[USP_MST_GetZipCodes] @Prefix='" + prefix + "',@CityID=" + CityId;
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())

            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["ZipCode"], rsMCodeList["ZipCodeID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetSOListUnderOBD(string prefix, string OBDID)
        {
            List<string> SOList = new List<string>();//AND OBD.DeliveryStatusID!=1
            string SOSql = "SELECT SOH.SOHeaderID,SOH.SONumber FROM ORD_SOHeader SOH JOIN OBD_Outbound_ORD_CustomerPO OBDCUS ON OBDCUS.SOHeaderID=SOH.SOHeaderID AND OBDCUS.IsDeleted=0 JOIN OBD_Outbound OBD ON OBD.OutboundID=OBDCUS.OutboundID AND OBD.IsDeleted=0 WHERE SOH.IsDeleted=0 AND OBD.OutboundID=" + OBDID + " AND SOH.SONumber LIKE '" + prefix + "%'";

            IDataReader rsSOList = DB.GetRS(SOSql);

            while (rsSOList.Read())
            {
                SOList.Add(string.Format("{0},{1}", rsSOList["SONumber"], rsSOList["SOHeaderID"]));
            }
            rsSOList.Close();
            return SOList.ToArray();
        }
        //============================= Added By M.D.Prasad ===================================//
        //=== 05-May-2020=========//
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWarehouseByAccount(string prefix, int AccountID)
        {

            string mmSql = "SELECT WarehouseID,WHName+'-'+WHCode AS WHCode FROM GEN_Warehouse WHERE IsActive=1 AND IsDeleted=0 AND AccountID =" + AccountID + " AND WHCode LIKE '" + prefix + "%'";
            List<string> mmList = new List<string>();

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["WHCode"], rsMCodeList["WarehouseID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadAccountForWHList(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["Account"], rsMCodeList["AccountID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadTenantsForWHList(string prefix, int AccountID)
        {
            List<string> mmList = new List<string>();

            #region below block is commented by Kashyap Manchikanti on 25-September-2018 to pass the login accountid to tenants
            //string mmSql = "SELECT TOP 10 TenantID,TenantName+isnull('-'+TenantCode,'') AS TenantName  FROM TPL_Tenant WHERE IsActive=1 AND IsDeleted=0 AND AccountID=" + AccountID + " AND TenantName+isnull('-'+TenantCode,'') LIKE '%" + prefix + "%'";
            //IDataReader rsMCodeList = DB.GetRS(mmSql);

            //while (rsMCodeList.Read())
            //{
            //    mmList.Add(string.Format("{0},{1}", rsMCodeList["TenantName"], rsMCodeList["TenantID"]));
            //}

            //rsMCodeList.Close();
            //return mmList.ToArray();
            #endregion


            if (AccountID == cp.AccountID)
            {
                string mmSql = "SELECT TOP 10 TenantID,TenantName+isnull('-'+TenantCode,'') AS TenantName  FROM TPL_Tenant WHERE IsActive=1 AND IsDeleted=0 AND AccountID=" + AccountID + " AND TenantName+isnull('-'+TenantCode,'') LIKE '%" + prefix + "%'";
                IDataReader rsMCodeList = DB.GetRS(mmSql);

                while (rsMCodeList.Read())
                {
                    mmList.Add(string.Format("{0},{1}", rsMCodeList["TenantName"], rsMCodeList["TenantID"]));
                }

                rsMCodeList.Close();
                return mmList.ToArray();
            }
            else
                return null;

        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadTenantsForINBWHList(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "SELECT TOP 10 TenantID,TenantName+isnull('-'+TenantCode,'') AS TenantName  FROM TPL_Tenant WHERE IsActive=1 AND IsDeleted=0 AND AccountID=" + cp.AccountID + " AND TenantName+isnull('-'+TenantCode,'') LIKE '%" + prefix + "%'";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["TenantName"], rsMCodeList["TenantID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWHForWHList(string prefix, int TenantID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "SELECT DISTINCT TC.WarehouseID,WH.WHCode,TC.TenantID FROM TPL_Tenant_Contract AS TC JOIN  GEN_Warehouse AS WH ON TC.WarehouseID = WH.WarehouseID AND TC.IsActive=1 AND TC.IsDeleted=0 AND WH.IsActive=1 AND WH.IsDeleted=0 AND FORMAT(GETDATE(),'yyyy-MM-dd') BETWEEN FORMAT(TC.EffectiveFrom,'yyyy-MM-dd') AND  FORMAT(TC.EffectiveTo,'yyyy-MM-dd') AND TC.TenantID =" + TenantID + " AND WH.WHCode LIKE '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["WHCode"], rsMCodeList["WarehouseID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadUserWarehouse(string prefix, int UserID)
        {
            List<string> mmList = new List<string>();

            //  string mmSql = "SELECT DISTINCT TC.WarehouseID,WH.WHCode,TC.TenantID FROM TPL_Tenant_Contract AS TC JOIN  GEN_Warehouse AS WH ON TC.WarehouseID = WH.WarehouseID AND TC.IsActive=1 AND TC.IsDeleted=0 AND WH.IsActive=1 AND WH.IsDeleted=0 AND FORMAT(GETDATE(),'yyyy-MM-dd') BETWEEN FORMAT(TC.EffectiveFrom,'yyyy-MM-dd') AND  FORMAT(TC.EffectiveTo,'yyyy-MM-dd') AND TC.TenantID =" + TenantID + " AND WH.WHCode LIKE '" + prefix + "%'";
            string mmSql = "select DISTINCT UW.WarehouseID,WH.WHCode from GEN_User_WarehoGetCycleCountCodesuse UW  JOIN GEN_Warehouse WH ON WH.WarehouseID = UW.WarehouseID AND WH.IsActive = 1 AND WH.IsDeleted = 0"
                           + "JOIN GEN_User US ON US.UserID = UW.UserID AND US.isactive = 1 AND US.Isdeleted = 0  where  WH.WHCode LIKE '" + prefix + "%' AND UW.UserID =+" + UserID;
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["WHCode"], rsMCodeList["WarehouseID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadDocumentType(string prefix)
        {
            List<string> DoctypeList = new List<string>();

            string DoctypeListSql = "Exec [dbo].[sp_Android_GetDocumentType] ";
            IDataReader rsDoctypeList = DB.GetRS(DoctypeListSql);
            while (rsDoctypeList.Read())
            {
                DoctypeList.Add(string.Format("{0},{1}", rsDoctypeList["DocumentType"], rsDoctypeList["DocumentTypeID"]));
            }
            rsDoctypeList.Close();
            return DoctypeList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoaDWHListBasedonUser(string prefix)
        {
            List<string> mmList = new List<string>();

            string mmSql = "EXEC [dbo].[SP_RPT_WHDROPDOWN] @USERID= " + cp.UserID + ",@PRIFIX =" +DB.SQuote(prefix);

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["WHCode"], rsMCodeList["WarehouseID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWHForWHList_CurrentStock(string prefix, int TenantID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "SELECT DISTINCT TC.WarehouseID,WH.WHCode FROM TPL_Tenant_Contract AS TC JOIN  GEN_Warehouse AS WH ON TC.WarehouseID = WH.WarehouseID AND TC.IsActive=1 AND TC.IsDeleted=0 AND WH.IsActive=1 AND WH.IsDeleted=0 AND AccountID="+cp.AccountID +" AND FORMAT(GETDATE(),'yyyy-MM-dd') BETWEEN FORMAT(TC.EffectiveFrom,'yyyy-MM-dd') AND  FORMAT(TC.EffectiveTo,'yyyy-MM-dd') AND (TC.TenantID =" + TenantID + " OR 0=" + TenantID + ") AND WH.WHCode LIKE '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["WHCode"], rsMCodeList["WarehouseID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWHForWHListWithoutTenant(string prefix, int TenantID)
        {
            List<string> mmList = new List<string>();

            string mmSql = "SELECT DISTINCT TC.WarehouseID,WH.WHCode FROM TPL_Tenant_Contract AS TC JOIN  GEN_Warehouse AS WH ON TC.WarehouseID = WH.WarehouseID AND TC.IsActive=1 AND TC.IsDeleted=0 AND WH.IsActive=1 AND WH.IsDeleted=0 AND FORMAT(GETDATE(),'yyyy-MM-dd') BETWEEN FORMAT(TC.EffectiveFrom,'yyyy-MM-dd') AND  FORMAT(TC.EffectiveTo,'yyyy-MM-dd') AND (0 = " + TenantID + " OR TC.TenantID =" + TenantID + ") AND WH.WHCode LIKE '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["WHCode"], rsMCodeList["WarehouseID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWHForWHListWithUserID(string prefix, int TenantID)
        {
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT DISTINCT TC.WarehouseID,WH.WHCode,TC.TenantID FROM TPL_Tenant_Contract AS TC JOIN  GEN_Warehouse AS WH ON TC.WarehouseID = WH.WarehouseID AND TC.IsActive=1 AND TC.IsDeleted=0 AND WH.IsActive=1 AND WH.IsDeleted=0 AND FORMAT(GETDATE(),'yyyy-MM-dd') BETWEEN FORMAT(TC.EffectiveFrom,'yyyy-MM-dd') AND  FORMAT(TC.EffectiveTo,'yyyy-MM-dd') AND TC.TenantID =" + TenantID + " AND WH.WHCode LIKE '" + prefix + "%'";
            string mmSql = "SELECT DISTINCT TC.WarehouseID,WH.WHCode,TC.TenantID FROM TPL_Tenant_Contract AS TC JOIN  GEN_Warehouse AS WH ON TC.WarehouseID = WH.WarehouseID AND TC.IsActive=1 AND TC.IsDeleted=0 AND WH.IsActive=1 AND WH.IsDeleted=0  JOIN GEN_User_Warehouse UWH ON UWH.WarehouseID = WH.WarehouseID AND UWH.IsActive=1 AND UWH.IsDeleted=0 AND UWH.UserID=" + cp.UserID + " AND FORMAT(GETDATE(),'yyyy-MM-dd') BETWEEN FORMAT(TC.EffectiveFrom,'yyyy-MM-dd') AND  FORMAT(TC.EffectiveTo,'yyyy-MM-dd') AND TC.TenantID =" + TenantID + " AND WH.WHCode LIKE '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["WHCode"], rsMCodeList["WarehouseID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadAccountForCyccleCount(string prefix, int AccountID)
        {
            string mmSql = "";
            if (AccountID == 0)
            {
                // mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted=0 AND AccountID = " +cp.AccountID + " AND Account like '" + prefix + "%'";
                mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";
            }
            else
            {
                mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND AccountID = " + AccountID + " AND Account like '" + prefix + "%'";

            }
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["Account"], rsMCodeList["AccountID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }
        //load vehicles
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadYardVehicles(string prefix, int AccountID)
        {
            string mmSql = "";
            if (AccountID == 0)
            {
                // mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted=0 AND AccountID = " +cp.AccountID + " AND Account like '" + prefix + "%'";
                mmSql = "SELECT TOP 10 YM_MST_Vehicle_ID,RegistrationNumber FROM YM_MST_Vehicles WHERE IsActive=1 AND IsDeleted =0 AND RegistrationNumber like '" + prefix + "%'";
            }
            else
            {
                mmSql = "SELECT TOP 10 YM_MST_Vehicle_ID,RegistrationNumber FROM YM_MST_Vehicles WHERE IsActive=1 AND IsDeleted =0 AND AccountID = " + AccountID + " AND RegistrationNumber like '" + prefix + "%'";

            }
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["RegistrationNumber"], rsMCodeList["YM_MST_Vehicle_ID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWarehouseForCyccleCount(string prefix, int AccountID, string TenantId)
        {

            string mmSql = "SELECT DISTINCT TOP 10 WH.WarehouseID,WH.WHCode FROM GEN_Warehouse WH INNER JOIN TPL_Tenant_Contract TNTC ON TNTC.WarehouseID =  WH.WarehouseID AND TNTC.IsActive = 1 AND TNTC.IsDeleted = 0 WHERE WH.IsActive=1 AND WH.IsDeleted=0 AND TNTC.TenantID= " + TenantId + "AND (AccountID=" + AccountID + " OR 0 = " + AccountID + ") AND WHCode like '" + prefix + "%'";
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["WHCode"], rsMCodeList["WarehouseID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadUsersForAudit(string prefix, int AccountID)
        {

            string mmSql = "SELECT TOP 10 AccountID,UserID,FirstName FROM GEN_User WHERE IsActive=1 AND IsDeleted=0 AND AccountID=" + AccountID + " AND FirstName like '" + prefix + "%'";    // UserId added by Ganesh @Sep 28 2020-- data data should be displayed by User
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["FirstName"], rsMCodeList["UserID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        //======================== For Cycle Count Added By M.D.Prasad On 05-May-2020====================//

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetCCNames(string prefix, string CCMID, string AccountID,string WHID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "SELECT DISTINCT TOP 20 CCM_CNF_AccountCycleCount_ID,CCM_MST_CycleCount_ID,dbo.UDF_ParseAndReturnLocaleString(AccountCycleCountName, 'en') AS AccountCycleCountName" +
                " FROM CCM_CNF_AccountCycleCounts WHERE IsActive = 1 AND IsDeleted = 0 AND AM_MST_Account_ID =" + AccountID + " AND WarehouseID=" + WHID + " AND CCM_MST_CycleCount_ID=" + CCMID + " AND dbo.UDF_ParseAndReturnLocaleString(AccountCycleCountName, 'en') LIKE '%" + prefix + "%'  ORDER BY CCM_CNF_AccountCycleCount_ID DESC";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())

            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["AccountCycleCountName"], rsMCodeList["CCM_CNF_AccountCycleCount_ID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetCycleCountCodes(string prefix, string ACCID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "SELECT DISTINCT TOP 20 CycleCountCode,CCM_CNF_AccountCycleCount_ID,CCM_TRN_CycleCount_ID FROM CCM_TRN_CycleCounts" +
                " WHERE IsActive = 1 AND IsDeleted = 0 AND IsCompleted=1 AND CCM_CNF_AccountCycleCount_ID =" + ACCID + " AND CycleCountCode LIKE '%" + prefix + "%' ORDER BY CCM_TRN_CycleCount_ID";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())

            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["CycleCountCode"], rsMCodeList["CCM_TRN_CycleCount_ID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }
        //=================== END =======================//

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetCycleCountEntities(string prefix, int AccountID, string EntityName, int WarehouseID)
        {
            List<string> mmList = new List<string>();
            if (EntityName == "Material")
            {
                string mmSql = "SELECT DISTINCT TOP 10 MM.MaterialMasterID AS MM_MST_Material_ID, MM.MCode AS  Material_Name, MM.MCode AS MaterialCode,TM.AccountID"
            + " FROM MMT_MaterialMaster MM JOIN TPL_Tenant_MaterialMaster AS TM ON MM.MaterialMasterID = TM.MaterialMasterID AND MM.IsActive = 1 AND MM.IsDeleted = 0 AND TM.IsActive = 1 AND TM.IsDeleted = 0"
            + " JOIN MMT_MaterialMaster_GEN_UoM AS MUM ON MM.MaterialMasterID = MUM.MaterialMasterID AND MUM.IsActive = 1 AND MUM.IsDeleted = 0 AND MUM.UoMTypeID = 1 WHERE TM.AccountID IS NOT NULL AND (TM.AccountID=" + AccountID + " OR 0 = " + AccountID + ") AND MCode LIKE '%" + prefix + "%'";


                IDataReader rsMCodeList = DB.GetRS(mmSql);

                while (rsMCodeList.Read())
                {
                    mmList.Add(string.Format("{0},{1}", rsMCodeList["Material_Name"], rsMCodeList["MM_MST_Material_ID"]));
                }

                rsMCodeList.Close();
            }
            else if (EntityName == "User")
            {
                string mmSql = "select DISTINCT USR.UserID,FirstName from GEN_User USR JOIN GEN_User_Warehouse USR_WH ON USR_WH.UserID = USR.UserID WHERE FirstName like '" + prefix + "%' AND USR.AccountID =case when 0 =" + cp.AccountID.ToString() + " then USR.AccountID else " + cp.AccountID.ToString() + " end AND USR_WH.WarehouseID =" + WarehouseID;
                IDataReader rsMCodeList = DB.GetRS(mmSql);

                while (rsMCodeList.Read())
                {
                    mmList.Add(string.Format("{0},{1}", rsMCodeList["FirstName"], rsMCodeList["UserID"]));
                }
                rsMCodeList.Close();
            }
            else if (EntityName == "Classification")
            {
                string mmSql = "SELECT TOP 10 ProductCategory AS MaterialClassification, ProductCategoryID AS MM_MST_MaterialClassification_ID FROM MMT_ProductCategory WHERE IsActive = 1 AND IsDeleted = 0 AND ProductCategory LIKE '%" + prefix + "%'";
                IDataReader rsMCodeList = DB.GetRS(mmSql);

                while (rsMCodeList.Read())
                {
                    mmList.Add(string.Format("{0},{1}", rsMCodeList["MaterialClassification"], rsMCodeList["MM_MST_MaterialClassification_ID"]));
                }

                rsMCodeList.Close();
            }

            else if (EntityName == "Zone")
            {
                string mmSql = "SELECT TOP 10 CZ.LocationZoneID AS LOC_CNF_Zone_ID, CZ.LocationZoneCode AS ZoneName,CZ.WarehouseID FROM INV_LocationZone  AS CZ"
                + " JOIN GEN_Warehouse AS WH ON WH.WarehouseID = CZ.WarehouseID AND CZ.IsActive = 1 AND CZ.IsDeleted = 0 AND ISNULL(CZ.IsDockZone,0) = 0 AND WH.IsActive = 1 AND WH.IsDeleted = 0 AND CZ.WarehouseID =" + WarehouseID + " AND CZ.LocationZoneCode LIKE '%" + prefix + "%'";
                IDataReader rsMCodeList = DB.GetRS(mmSql);

                while (rsMCodeList.Read())
                {
                    mmList.Add(string.Format("{0},{1}", rsMCodeList["ZoneName"], rsMCodeList["LOC_CNF_Zone_ID"]));
                }

                rsMCodeList.Close();
            }

            else if (EntityName == "Tenant")
            {
                string mmSql = "SELECT TOP 10 TM.TenantName,TM.TenantID AS TM_MST_Tenant_ID, TM.AccountID FROM TPL_Tenant AS TM WHERE TM.IsActive = 1 AND TM.IsDeleted = 0 AND (TM.AccountID=" + AccountID + " OR 0 = " + AccountID + ") AND TM.TenantName LIKE '%" + prefix + "%'";
                IDataReader rsMCodeList = DB.GetRS(mmSql);

                while (rsMCodeList.Read())
                {
                    mmList.Add(string.Format("{0},{1}", rsMCodeList["TenantName"], rsMCodeList["TM_MST_Tenant_ID"]));
                }

                rsMCodeList.Close();
            }

            else if (EntityName == "Supplier")
            {
                string mmSql = "SELECT DISTINCT TOP 10 SUP.SupplierID, SUP.SupplierName,TNT.AccountID FROM TPL_Tenant_Supplier TSUP"
                + " INNER JOIN TPL_Tenant TNT ON TSUP.TenantID = TNT.TenantID AND TSUP.IsActive = 1 AND TSUP.IsDeleted = 0 AND TNT.IsActive = 1 AND TNT.IsDeleted = 0"
                + " INNER JOIN MMT_Supplier SUP ON SUP.SupplierID = TSUP.SupplierID AND SUP.IsActive = 1 AND SUP.IsDeleted = 0 WHERE SUP.SupplierID IS NOT NULL AND (TNT.AccountID=" + AccountID + " OR 0 = " + AccountID + ") AND SUP.SupplierName LIKE '%" + prefix + "%'";
                IDataReader rsMCodeList = DB.GetRS(mmSql);

                while (rsMCodeList.Read())
                {
                    mmList.Add(string.Format("{0},{1}", rsMCodeList["SupplierName"], rsMCodeList["SupplierID"]));
                }

                rsMCodeList.Close();
            }

            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMaterialsForCurrentStock(string prefix, int TenantID)
        {
            string mmSql = "SELECT TOP 10 MM.MaterialMasterID, MM.MCode FROM TPL_Tenant_MaterialMaster AS TM JOIN MMT_MaterialMaster AS MM ON TM.MaterialMasterID = MM.MaterialMasterID AND MM.IsActive = 1 AND MM.IsDeleted = 0 AND TM.IsActive = 1 AND TM.IsDeleted = 0 WHERE TM.TenantID =" + TenantID + " AND MM.MCode LIKE '%" + prefix + "%'";
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMaterialsData(string prefix, int TenantID)
        {
            string mmSql = "SELECT TOP 10 MM.MaterialMasterID, MM.MCode FROM TPL_Tenant_MaterialMaster AS TM JOIN MMT_MaterialMaster AS MM ON TM.MaterialMasterID = MM.MaterialMasterID AND MM.IsActive = 1 AND MM.IsDeleted = 0 AND TM.IsActive = 1 AND TM.IsDeleted = 0  WHERE  AccountID=" + cp.AccountID + " AND (0 = " + TenantID + " OR TM.TenantID =" + TenantID + ") AND MM.MCode LIKE '" + prefix + "%'";
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMaterials(string prefix, int TenantID)
        {
            string mmSql = "SELECT TOP 10 MM.MaterialMasterID, MM.MCode FROM TPL_Tenant_MaterialMaster AS TM JOIN MMT_MaterialMaster AS MM ON TM.MaterialMasterID = MM.MaterialMasterID AND TM.AccountID="+cp.AccountID+" AND MM.IsActive = 1 AND MM.IsDeleted = 0 AND TM.IsActive = 1 AND TM.IsDeleted = 0 WHERE (0 = " + TenantID + " OR TM.TenantID =" + TenantID + ") AND MM.MCode LIKE '" + prefix + "%'";
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }


        // This method written by Ganesh @ Oct 1 2020 -- PartNumber should be displyed Under Warehouse
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMaterialsForCycleCountReport(string prefix, int WarehouseId)
        {
            string mmSql = "SELECT DISTINCT TOP 10 MCode,MaterialMasterID FROM MMT_MaterialMaster MM JOIN TPL_Tenant_Contract TC ON TC.TenantID=MM.TenantID AND MM.IsActive=1 AND MM.IsDeleted=0 AND TC.IsActive=1 JOIN GEN_User_Warehouse UW ON UW.WarehouseID = TC.WarehouseID AND UW.IsActive = 1 AND UW.IsDeleted = 0 WHERE UW.UserID = "+cp.UserID+" AND UW.WarehouseId="+WarehouseId+" AND MM.MCode LIKE '" + prefix + "%' ORDER BY MaterialMasterID ";
            List<string> mmList = new List<string>();

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMaterialTypesForCurrentStock(string prefix, int TenantID)
        {
            string mmSql = "SELECT TOP 10 MType,MTypeID FROM MMT_MType WHERE IsActive=1 AND IsDeleted=0 AND TenantID =" + TenantID + " AND MType LIKE '" + prefix + "%'";
            List<string> mmList = new List<string>();
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MType"], rsMCodeList["MTypeID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadLocationsForCurrentStock(string prefix, int WarehouseID)
        {
            string mmSql = "SELECT TOP 10 LOC.LocationID,CASE WHEN LOC.DockID IS Null THEN LOC.DisplayLocationCode ELSE DOC.DockName END AS Location  FROM INV_Location AS LOC JOIN INV_LocationZone AS LOZ ON LOC.ZoneId = LOZ.LocationZoneID"
                            + " AND LOC.IsActive = 1 AND LOC.IsDeleted = 0 AND LOZ.IsActive = 1 AND LOZ.IsDeleted = 0   join GEN_Warehouse wh on LOZ.WarehouseID=wh.WarehouseID  Left JOIN GEN_Dock DOC ON  LOC.DockID = DOC.DockID AND DOC.IsActive=1 AND DOC.Isdeleted=0   WHERE (" + WarehouseID + "=0 or LOZ.WarehouseID =" + WarehouseID + ") AND (" + cp.AccountID + "=0 or wh.AccountID=" + cp.AccountID + ") and  LOC.DisplayLocationCode LIKE '" + prefix + "%'";
            List<string> mmList = new List<string>();
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["Location"], rsMCodeList["LocationID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetMMLocation(string Prefix,string WarehouseID)
        {
            string mmSql = "SELECT TOP 10 LOC.LocationID,CASE WHEN LOC.DockID IS Null THEN LOC.DisplayLocationCode ELSE DOC.DockName END AS Location  FROM INV_Location AS LOC JOIN INV_LocationZone AS LOZ ON LOC.ZoneId = LOZ.LocationZoneID"
                            + " AND LOC.IsActive = 1 AND LOC.IsDeleted = 0 AND LOZ.IsActive = 1 AND LOZ.IsDeleted = 0   join GEN_Warehouse wh on LOZ.WarehouseID=wh.WarehouseID  Left JOIN GEN_Dock DOC ON  LOC.DockID = DOC.DockID AND DOC.IsActive=1 AND DOC.Isdeleted=0   WHERE (" + WarehouseID + "=0 or LOZ.WarehouseID =" + WarehouseID + ") AND (" + cp.AccountID + "=0 or wh.AccountID=" + cp.AccountID + ") and  LOC.DisplayLocationCode LIKE '" + Prefix + "%'";
            List<string> mmList = new List<string>();
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["Location"], rsMCodeList["LocationID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetKitPlannerId(string prefix, int TenantID)
        {
            string mmSql = "SELECT KitCode,KitPlannerID FROM MMT_KitPlanner WHERE IsActive=1 AND IsDeleted=0 AND TenantID=" + TenantID + " AND KitCode IS NOT NULL AND KitCode Like '" + prefix + "%'";
            List<string> mmList = new List<string>();

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["KitCode"], rsMCodeList["KitPlannerID"]));
            }
            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMaterialsUnderMtype(string prefix, int MTypeID, int tenantid)
        {
            string mmSql = "SELECT TOP 10 MM.MaterialMasterID, MM.MCode FROM MMT_MaterialMaster AS MM WHERE MM.IsDeleted = 0 AND MM.IsActive = 1 AND tenantid=" + tenantid + " AND (MTypeID=" + MTypeID + " OR 0=" + MTypeID + ") AND  MM.MCode LIKE '%" + prefix + "%'";
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadFilters(string prefix, int TenantID, string Type)
        {
            if (Type == "1")
            {
                //string mmSql = "SELECT TOP 10 SUP.SupplierInvoiceID,SUP.InvoiceNumber FROM ORD_SupplierInvoice AS SUP JOIN ORD_POHeader AS POH "
                //    + " ON SUP.POHeaderID = POH.POHeaderID AND SUP.IsActive = 1 AND SUP.IsDeleted = 0 AND POH.IsActive = 1 AND POH.IsDeleted = 0 WHERE POH.TenantID ="+ TenantID + " AND SUP.InvoiceNumber LIKE '%" + prefix + "%'";

                string mmSql = "SELECT DISTINCT TOP 10 SUP.InvoiceNumber,1 as SupplierInvoiceID FROM ORD_SupplierInvoice AS SUP JOIN ORD_POHeader AS POH "
                  + " ON SUP.POHeaderID = POH.POHeaderID AND SUP.IsActive = 1 AND SUP.IsDeleted = 0 AND POH.IsActive = 1 AND POH.IsDeleted = 0 WHERE POH.TenantID =" + TenantID + " AND SUP.InvoiceNumber LIKE '%" + prefix + "%'";

                List<string> mmList = new List<string>();

                //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

                IDataReader rsMCodeList = DB.GetRS(mmSql);

                while (rsMCodeList.Read())
                {
                    mmList.Add(string.Format("{0},{1}", rsMCodeList["InvoiceNumber"], rsMCodeList["SupplierInvoiceID"]));
                }

                rsMCodeList.Close();
                return mmList.ToArray();
            }
            else
            {
                string mmSql = "SELECT TOP 10 MM.MaterialMasterID, MM.MCode FROM TPL_Tenant_MaterialMaster AS TM JOIN MMT_MaterialMaster AS MM ON TM.MaterialMasterID = MM.MaterialMasterID AND MM.IsActive = 1 AND MM.IsDeleted = 0 AND TM.IsActive = 1 AND TM.IsDeleted = 0 WHERE TM.TenantID =" + TenantID + " AND MM.MCode LIKE '%" + prefix + "%'";
                List<string> mmList = new List<string>();

                //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

                IDataReader rsMCodeList = DB.GetRS(mmSql);

                while (rsMCodeList.Read())
                {
                    mmList.Add(string.Format("{0},{1}", rsMCodeList["MCode"], rsMCodeList["MaterialMasterID"]));
                }

                rsMCodeList.Close();
                return mmList.ToArray();
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetContainerType(string prefix)
        {
            List<string> mmList = new List<string>();
            string mmSql = "";

            mmSql = "SELECT ContainerTypeID,ContainerType FROM INV_ContainerType where IsActive=1 and IsDeleted=0 and  (ContainerType like '" + prefix + "%' or ContainerType like '%" + prefix + "' or ContainerType like '%" + prefix + "%') ";



            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["ContainerType"], rsMCodeList["ContainerTypeID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        //============================= Added By M.D.Prasad ===================================//
        //============================= Added By Karanam Kishore ==============================//
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] Loadloaction_GoodsIn(string Prefix = "", string InboundID = "")
        {
            if (InboundID.Length == 0)
                InboundID = "0";

            string mmSql = "  SELECT TOP 15 LocationID, DisplayLocationCode Location FROM INV_Location AS LM INNER JOIN INV_LocationZone AS LZ ON LM.ZoneId = LZ.LocationZoneID AND LZ.IsDeleted=0 and LZ.IsActive=1 and LM.IsActive=1 and LM.IsDeleted=0 AND ISNULL(LM.IsBlockedForCycleCount, 0) = 0  WHERE LZ.WarehouseID IN (SELECT DISTINCT WarehouseID FROM INB_RefWarehouse_Details AS ITW WHERE ITW.InboundID = (CASE WHEN " + InboundID + " = 0 THEN ITW.InboundID ELSE " + InboundID + " END)) AND LM.isactive=1 and LM.Isdeleted=0 and LZ.IsActive=1 and LZ.Isdeleted=0 and  LM.Location Not in ('LNF') and Location LIKE '%" + Prefix + "%' ";
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["Location"], rsMCodeList["LocationID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetContainersForGoodsin(string Prefix, String VLPDID)
        {


            string mmSql = "[dbo].[sp_GET_Loction_CartonList] @Prefix=" + DB.SQuote(Prefix) + ",@VLPDID=" + VLPDID;
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["CartonCode"], rsMCodeList["CartonID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetContainersForGoodsOutNOBD(string Prefix, String Outbound, String Location = "")
        {


            string mmSql = "[dbo].[sp_GET_Loction_CartonList] @Prefix=" + DB.SQuote(Prefix) + ",@Location = " + DB.SQuote(Location) + ",@OutBoundID=" + Outbound;
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["CartonCode"], rsMCodeList["CartonID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadContainer_GoodsIn(int INBID, string prefix, string Location)
        {
            string mmSql = "EXEC [dbo].[sp_GET_Loction_CartonList] @Location = " + DB.SQuote(Location) + ", @InboundID = " + INBID + " , @Prefix = " + DB.SQuote(prefix) + " ";
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           
            DataSet ds = DB.GetDS(mmSql, false);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        mmList.Add(string.Format("{0},{1}", row["CartonCode"], row["CartonID"]));
                    }
                }
            }
            //IDataReader rsMCodeList = DB.GetRS(mmSql);
            //while (rsMCodeList.Read())
            //{
            //    mmList.Add(string.Format("{0},{1}", rsMCodeList["CartonCode"], rsMCodeList["CartonID"]));
            //}
            //rsMCodeList.Close();
            return mmList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadStorageLocation_GoodsIn(string prefix)
        {
            string mmSql = "  SELECT Id, Code FROM StorageLocation WHERE Id IN(3,4,5,9,10) ";
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);
            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["Code"], rsMCodeList["Id"]));
            }
            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWareHouse_TariffAllocation(string prefix, string Tenantid)
        {
            string mmSql = "EXEC SP_GET_TPL_WareHouseList @TenantId = " + Tenantid + ", @Prefix = " + DB.SQuote(prefix) + "";
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);
            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["WHCode"], rsMCodeList["WarehouseID"]));
            }
            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadVehicleList_GoodsIn(string prefix, string InboundId)
        {
            string mmSql = "SELECT InboundDockID, VehicleRegNo FROM INB_Inbound_Dock INBD WHERE  IsActive=1 AND IsDeleted=0 AND INBD.InboundID =  " + InboundId + "";
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);
            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["VehicleRegNo"], rsMCodeList["InboundDockID"]));
            }
            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadContainer_BinToBin(string prefix, string Location, string MaterialMasterID, string WarehouseId)
        {
            string mmSql = "EXEC [dbo].[sp_GET_Loction_CartonList_INTER] @Location = " + DB.SQuote(Location) + ", @MaterialMasterID = " + MaterialMasterID + " , @Prefix = " + DB.SQuote(prefix) + ", @WarehouseId=" + WarehouseId + " ";
            List<string> mmList = new List<string>();
            IDataReader rsMCodeList = DB.GetRS(mmSql);
            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["CartonCode"], rsMCodeList["CartonID"]));
            }
            rsMCodeList.Close();
            return mmList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetAllLocations_New(string Prefix, string MMID)
        {
            List<string> LocationList = new List<string>();
            //string cMdLocationList = "select top 10 Location,LocationID from INV_Location loc join INV_LocationZone locz on locz.LocationZoneCode=left(Location,2) and locz.IsDeleted=0 left join INB_RefWarehouse_Details refw on refw.WarehouseID=locz.WarehouseID and refw.IsDeleted=0 and refw.InboundID=" + InboundID + " where (" + InboundID + "=0 or refw.InboundID is not null) and loc.IsDeleted=0 and (" + ProductCategory + " not in (0,3) or  (0=" + ProductCategory + " and left(Location,2)!='Q1') or (3=" + ProductCategory + " and left(Location,2)='P1')) and Location  like '" + Prefix + "%'";

            string cMdLocationList = "EXEC [dbo].[sp_GET_Loction_INTER] @MaterialMasterID = " + MMID + ", @Prefix = " + DB.SQuote(Prefix) + "";

            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0},{1}", rslocationList["Location"], rslocationList["LocationID"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetStorageLoc()
        {
            List<string> LocationList = new List<string>();
            //string cMdLocationList = "select top 10 Location,LocationID from INV_Location loc join INV_LocationZone locz on locz.LocationZoneCode=left(Location,2) and locz.IsDeleted=0 left join INB_RefWarehouse_Details refw on refw.WarehouseID=locz.WarehouseID and refw.IsDeleted=0 and refw.InboundID=" + InboundID + " where (" + InboundID + "=0 or refw.InboundID is not null) and loc.IsDeleted=0 and (" + ProductCategory + " not in (0,3) or  (0=" + ProductCategory + " and left(Location,2)!='Q1') or (3=" + ProductCategory + " and left(Location,2)='P1')) and Location  like '" + Prefix + "%'";

            string cMdLocationList = "SELECT Code, Id FROM StorageLocation WHERE ID NOT IN (1,2,7)";

            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0},{1}", rslocationList["Code"], rslocationList["Id"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetCartonList_InternalTransfer(string Prefix, string Location, string TenantID = "0")
        {
            List<string> LocationList = new List<string>();
            //string cMdLocationList = "select top 10 Location,LocationID from INV_Location loc join INV_LocationZone locz on locz.LocationZoneCode=left(Location,2) and locz.IsDeleted=0 left join INB_RefWarehouse_Details refw on refw.WarehouseID=locz.WarehouseID and refw.IsDeleted=0 and refw.InboundID=" + InboundID + " where (" + InboundID + "=0 or refw.InboundID is not null) and loc.IsDeleted=0 and (" + ProductCategory + " not in (0,3) or  (0=" + ProductCategory + " and left(Location,2)!='Q1') or (3=" + ProductCategory + " and left(Location,2)='P1')) and Location  like '" + Prefix + "%'";

            //  string cMdLocationList = "SELECT Code, Id FROM StorageLocation WHERE ID NOT IN (1,2,7)";
            string cMdLocationList = "EXEC [dbo].[sp_GET_CartonList_Internal] @Location = '" + Location + "', @TenantID =" + TenantID + "";
            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0},{1}", rslocationList["CartonCode"], rslocationList["CartonID"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }



        //============================= Added By Karanam Kishore ==============================//

        //============================= Added By NShravya ==============================//

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadFreightCompanyBasedonAccount(string prefix, int AccountID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "SELECT TOP 10 FC.FreightCompanyID, FC.FreightCompany FROM GEN_FreightCompany FC INNER JOIN GEN_Account ACC ON FC.AccountID = ACC.AccountID AND FC.IsActive = 1 AND FC.IsDeleted = 0 AND ACC.IsActive = 1 AND ACC.IsDeleted = 0 WHERE ACC.AccountID = " + AccountID + "AND FreightCompany LIKE '%" + prefix + "%'";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["FreightCompany"], rsMCodeList["FreightCompanyID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadVehicleTypeBasedonAccount(string prefix, int AccountID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "SELECT TOP 10 VT.YM_MST_VehicleType_ID, VT.VehicleType FROM YM_MST_VehicleTypes VT WHERE VT.IsActive = 1 AND  VT.IsDeleted = 0 AND VehicleType LIKE '%" + prefix + "%'";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["VehicleType"], rsMCodeList["YM_MST_VehicleType_ID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadStorageUoMLength(string prefix)
        {
            List<string> mmList = new List<string>();
            string mmSql = "Select TOP 10 YM_MST_UoM_ID , UoMCode + ' - ' + UoM as UoMType from YM_MST_UoMs where IsActive = 1 AND IsDeleted = 0 AND IsLength = 1 AND UoMCode LIKE '%" + prefix + "%'";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["UoMType"], rsMCodeList["YM_MST_UoM_ID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadStorageUoMWeight(string prefix)
        {
            List<string> mmList = new List<string>();
            string mmSql = "Select TOP 10 YM_MST_UoM_ID , UoMCode + ' - ' + UoM as UoMType from YM_MST_UoMs where IsActive = 1 AND IsDeleted = 0 AND IsLength = 0 AND UoMCode LIKE '%" + prefix + "%'";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["UoMType"], rsMCodeList["YM_MST_UoM_ID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        //============================= Added By NShravya ==============================//

        // ============================== Added by Priya ======================//


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadPSNMaterialItems(string prefix, int OBDID)
        {
            List<string> mmList = new List<string>();
            string mmSql = "Exec USP_OutboundMaterialDropDown @OBDID =" + OBDID + ",@prefix ='%" + prefix + "%'";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["Mcode"], rsMCodeList["MaterialMasterID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        //======================  END ==================================//

        // ============================== Added by Priya ======================//
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadUOMs(string prefix)
        {
            List<string> mmList = new List<string>();
            string mmSql = "SELECT DISTINCT  UoM ,UoMID FROM GEN_UoM where UoM LIKE '%" + prefix + "%'";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["UOM"], rsMCodeList["UOMID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadSONOS(String prefix,string warehouseid)
        {


            List<String> SOList = new List<String>();
            //String SOSql = "select top 10 SONumber,SOHeaderID from ORD_SOHeader where IsActive=1 and IsDeleted=0 and (0=" + (CustomerID != "" ? CustomerID : "0") + " or  CustomerID=" + CustomerID + ") and TenantID=" + TenentID + " and SONumber like '" + prefix + "%' order by SONumber";
            //String SOSql = "select top 10 SONumber,SOHeaderID from ORD_SOHeader where IsActive=1 and IsDeleted=0 and SONumber like '" + prefix + "%' order by SONumber";
            String SOSql = "SELECT TOP 10 SOH.SOHeaderID,SOH.SONumber FROM ORD_SOHeader SOH JOIN OBD_Outbound_ORD_CustomerPO OBD_CUST ON OBD_CUST.SOHeaderID = SOH.SOHeaderID AND OBD_CUST.IsActive = 1 AND OBD_CUST.IsDeleted = 0 JOIN OBD_RefWarehouse_Details OBD_REF ON OBD_REF.OutboundID = OBD_CUST.OutboundID AND OBD_REF.IsActive = 1 AND OBD_REF.IsDeleted = 0 WHERE SOH.IsActive = 1 AND SOH.IsDeleted = 0 AND OBD_REF.WarehouseID = " + warehouseid + " AND SONumber like '" + prefix + "%' ";
            IDataReader rsSO = DB.GetRS(SOSql);
            while (rsSO.Read())
            {
                SOList.Add(String.Format("{0},{1}", DB.RSField(rsSO, "SONumber"), DB.RSFieldInt(rsSO, "SOHeaderID")));

            }
            rsSO.Close();

            return SOList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadOBDNOS(String prefix,int TenantId)
        {


            List<String> SOList = new List<String>();
            String SOSql = "select top 10  OutboundID,OBDNumber from OBD_Outbound WHERE DeliveryStatusID IN (1,2) AND IsActive = 1 AND IsDeleted = 0 AND TenantId ="+TenantId+" AND OBDNumber like '" + prefix + "%' ";
            IDataReader rsSO = DB.GetRS(SOSql);
            while (rsSO.Read())
            {
                SOList.Add(String.Format("{0},{1}", DB.RSField(rsSO, "OBDNumber"), DB.RSFieldInt(rsSO, "OutboundID")));

            }
            rsSO.Close();

            return SOList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadCustomers(String prefix)
        {


            List<String> SOList = new List<String>();
            String SOSql = "SELECT TOP 10 CustomerID,CustomerCode FROM GEN_Customer WHERE CustomerCode like'" + prefix + "%' ";
            IDataReader rsSO = DB.GetRS(SOSql);
            while (rsSO.Read())
            {
                SOList.Add(String.Format("{0},{1}", DB.RSField(rsSO, "CustomerCode"), DB.RSFieldInt(rsSO, "CustomerID")));

            }
            rsSO.Close();

            return SOList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadPriority(string prefix)
        {
            List<string> KitCodeList = new List<string>();
            string cMdKitCodeList = "SELECT DISTINCT TOP 10   [Priority] FROM OBD_Outbound WHERE [Priority] like '" + prefix + "%'";

            IDataReader rsKitCodeList = DB.GetRS(cMdKitCodeList);

            while (rsKitCodeList.Read())
            {
                KitCodeList.Add(string.Format("{0}", rsKitCodeList["Priority"]));
            }
            rsKitCodeList.Close();
            return KitCodeList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadOBDStatus(string prefix)
        {
            List<string> KitCodeList = new List<string>();
            string cMdKitCodeList = "SELECT DeliveryStatusID,DeliveryStatus FROM OBD_DeliveryStatus WHERE IsActive = 1 AND IsDeleted = 0 AND DeliveryStatusID IN (1,2) AND DeliveryStatus LIKE  '" + prefix + "%'";

            IDataReader rsKitCodeList = DB.GetRS(cMdKitCodeList);

            while (rsKitCodeList.Read())
            {

                KitCodeList.Add(String.Format("{0},{1}", DB.RSField(rsKitCodeList, "DeliveryStatus"), DB.RSFieldInt(rsKitCodeList, "DeliveryStatusID")));
            }
            rsKitCodeList.Close();
            return KitCodeList.ToArray();
        }

        //======================  END ==================================//
        // ============================== Added by Priya ======================//
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadPackageConditions(string prefix)
        {
            List<string> mmList = new List<string>();
            string mmSql = "select PackageCondition,PackageConditionID	from [OBD_PackageCondition] where IsActive=1 and Isdeleted=0 and PackageCondition LIKE '%" + prefix + "%'";
            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["PackageCondition"], rsMCodeList["PackageConditionID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        //======================  END ==================================//

        // ============================== Added by Lalitha ======================//
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetAllLocationsUnderWarehouse(string Prefix, string MMID, string WHID)
        {
            List<string> LocationList = new List<string>();
            //string cMdLocationList = "select top 10 Location,LocationID from INV_Location loc join INV_LocationZone locz on locz.LocationZoneCode=left(Location,2) and locz.IsDeleted=0 left join INB_RefWarehouse_Details refw on refw.WarehouseID=locz.WarehouseID and refw.IsDeleted=0 and refw.InboundID=" + InboundID + " where (" + InboundID + "=0 or refw.InboundID is not null) and loc.IsDeleted=0 and (" + ProductCategory + " not in (0,3) or  (0=" + ProductCategory + " and left(Location,2)!='Q1') or (3=" + ProductCategory + " and left(Location,2)='P1')) and Location  like '" + Prefix + "%'";

            string cMdLocationList = "EXEC [dbo].[sp_GET_Loction_INTER] @MaterialMasterID = " + MMID + ",@WarehouseID=" + WHID + ", @Prefix = " + DB.SQuote(Prefix) + "";

            IDataReader rslocationList = DB.GetRS(cMdLocationList);

            while (rslocationList.Read())
            {
                LocationList.Add(string.Format("{0},{1}", rslocationList["Location"], rslocationList["LocationID"]));
            }
            rslocationList.Close();
            return LocationList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetActiveStockLocations(string prefix, string TenantID, string WHID, string mmid)
        {
            List<string> mmList = new List<string>();

            int accountid = DB.GetSqlN("Exec[dbo].[USP_MST_GetAccount_Tenant] @TenantID = " + TenantID);
            string Query = "SELECT DISTINCT VAS.LocationID,LOC.DisplayLocationCode Location  FROM vAvailableStock VAS JOIN INV_Location LOC ON LOC.LocationID = VAS.LocationID  LEFT JOIN INV_Carton CN ON CN.CartonID = VAS.CartonID  LEFT JOIN INV_LocationZone LOCZ ON LOCZ.LocationZoneID =  LOC.ZoneId LEFT JOIN  TPL_Tenant_Contract TPL on TPL.WarehouseID=LOCZ.warehouseid LEFT JOIN GEN_Warehouse GEN on GEN.WarehouseID=TPL.WarehouseID"
                              + " where VAS.accountid = " + accountid + " and TPL.tenantID = " + TenantID + " and GEN.WarehouseID=" + WHID + " and TPl.IsActive = 1 and TPL.IsDeleted = 0 and LOC.IsActive = 1 AND LOC.IsDeleted = 0 and SYS_SlocID IN (1,2)  AND  (IsBlockedForCycleCount = 0 OR IsBlockedForCycleCount IS NULL)  AND MaterialMasterId = " + mmid + " AND VAS.Quantity > 0  AND LOC.Location like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(Query);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["Location"], rsMCodeList["LocationID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetCartonsUnderLocations(string prefix, string WHID, string mmid, string LocationId)
        {
            List<string> mmList = new List<string>();


           // string Query = "SELECT DISTINCT  VAS.CartonID, CN.CartonCode FROM vAvailableStock VAS JOIN INV_Location LOC ON LOC.LocationID = VAS.LocationID JOIN INV_Carton CN ON CN.CartonID = VAS.CartonID WHERE VAS.StorageLocationID = 3 AND SYS_SlocID=1  AND MaterialMasterId = " + mmid + " and CN.WareHouseID=" + WHID + " and VAS.LocationID=" + LocationId + " and CartonCode like '" + prefix + "%'";
            string Query= "EXEC  [dbo].[SP_GET_Containers] @MaterialMasterID = " + mmid + " , @LocationID = " + LocationId + " , @WareHouseID = " + WHID + " , @Prefix =" + DB.SQuote(prefix) + " , @UserID = " + cp.UserID + " , @AccountID = " + cp.AccountID;
            IDataReader rsMCodeList = DB.GetRS(Query);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["CartonCode"], rsMCodeList["CartonID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetCartonsUnderLocations_Inter(string prefix, string WHID, string mmid, string LocationId)
        {
            List<string> mmList = new List<string>();


            // string Query = "SELECT DISTINCT  VAS.CartonID, CN.CartonCode FROM vAvailableStock VAS JOIN INV_Location LOC ON LOC.LocationID = VAS.LocationID JOIN INV_Carton CN ON CN.CartonID = VAS.CartonID WHERE VAS.StorageLocationID = 3 AND SYS_SlocID=1  AND MaterialMasterId = " + mmid + " and CN.WareHouseID=" + WHID + " and VAS.LocationID=" + LocationId + " and CartonCode like '" + prefix + "%'";
            string Query = "EXEC  [dbo].[sp_GET_Container_INTER] @MaterialMasterID = " + mmid + " , @LocationID = " + LocationId + " , @WareHouseID = " + WHID + " , @Prefix =" + DB.SQuote(prefix) + "";
            IDataReader rsMCodeList = DB.GetRS(Query);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["CartonCode"], rsMCodeList["CartonID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetBatchnosForTransfer(string prefix, string LocationId, string mmid, string TenantId, string CartonId)
        {
            List<string> mmList = new List<string>();


            //string Query = "SELECT DISTINCT  VAS.CartonID, CN.CartonCode FROM vAvailableStock VAS JOIN INV_Location LOC ON LOC.LocationID = VAS.LocationID JOIN INV_Carton CN ON CN.CartonID = VAS.CartonID WHERE VAS.StorageLocationID = 3  AND MaterialMasterId = " + mmid + " and CN.WareHouseID=" + WHID + " and CartonCode like '" + prefix + "%'";

            string Query = " Exec [dbo].[SP_TRN_ABL_BATCHLIST] @MATERIALMASTERID =" + mmid + ", @LOCATIONID =" + LocationId + ", @CARTONID= " + CartonId + ", @TENANTID = " + TenantId + "";

            IDataReader rsMCodeList = DB.GetRS(Query);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}", rsMCodeList["BATCHNO"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetCartons(string prefix, string WHID)
        {
            List<string> mmList = new List<string>();


            string Query = "SELECT DISTINCT Top 10 VAS.CartonID, CN.CartonCode FROM vAvailableStock VAS JOIN INV_Location LOC ON LOC.LocationID = VAS.LocationID JOIN INV_Carton CN ON CN.CartonID = VAS.CartonID WHERE VAS.StorageLocationID = 3  and CN.WareHouseID=" + WHID + "  and CartonCode like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(Query);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["CartonCode"], rsMCodeList["CartonID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetFromStorageLocation(string prefix)
        {
            List<string> mmList = new List<string>();


            string Query = "SELECT Code, Id FROM StorageLocation WHERE ID NOT IN (1,7)";

            IDataReader rsMCodeList = DB.GetRS(Query);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["Code"], rsMCodeList["Id"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetToStorageLocation(string prefix)
        {
            List<string> mmList = new List<string>();


            string Query = "SELECT Code, Id FROM StorageLocation WHERE ID NOT IN (1,2,7)";

            IDataReader rsMCodeList = DB.GetRS(Query);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0}~{1}", rsMCodeList["Code"], rsMCodeList["Id"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] CustomerBasedonTenant(string prefix, int TenantId)
        {
            List<string> mmList = new List<string>();

            string mmSql = "select CustomerName,CustomerID from GEN_Customer CUS join TPL_Tenant tnt on tnt.TenantID = CUS.TenantID join GEN_Account acc on acc.AccountID = tnt.AccountID where CUS.IsActive = 1 AND CUS.IsDeleted = 0 and CUS.TenantID =" + TenantId + "AND CustomerName  like '" + prefix + "%'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["CustomerName"], rsMCodeList["CustomerID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();




        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] Get_OBDList(string prefix, int TenantId, int WarehouseId, int CustomerId)
        {
            List<string> mmList = new List<string>();

            string mmSql = "EXEC [dbo].[SP_GET_OBDLIST] @TenantID=" + TenantId + ",@WareHouseId = " + WarehouseId + ", @CustomerId= " + CustomerId + ", @Prefix ='" + prefix + "'";

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["OBDNumber"], rsMCodeList["OutboundID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWarehouse(string prefix, int AccountID)
        {

            string mmSql = "SELECT DISTINCT TOP 10 WH.WarehouseID,WH.WHCode FROM GEN_Warehouse WH join GEN_Account ACC on ACC.AccountID= WH.AccountID  WHERE WH.IsActive=1 AND WH.IsDeleted=0 AND (ACC.AccountID=" + AccountID + " OR 0 = " + AccountID + ") AND WHCode like '" + prefix + "%'";
            List<string> mmList = new List<string>();

            //string mmSql = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted =0 AND Account like '" + prefix + "%'";           

            IDataReader rsMCodeList = DB.GetRS(mmSql);

            while (rsMCodeList.Read())
            {
                mmList.Add(string.Format("{0},{1}", rsMCodeList["WHCode"], rsMCodeList["WarehouseID"]));
            }

            rsMCodeList.Close();
            return mmList.ToArray();
        }

        //======================  END ==================================//

        //Load OBD Numbers
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadLoadSheetNumbers(String prefix, int TenantId)
        {
            List<String> OBDList = new List<String>();
            String SOSql = "select top 10 LoadSheetNo,LoadSheetId from OBD_LoadSheet_Header OLH join TPL_Tenant TNT on TNT.TenantID=OLH.TenantID where OLH.IsActive=1 and OLH.IsDeleted=0 and TNT.AccountID=" + cp.AccountID + " and OLH.TenantID=" + TenantId + " and LoadSheetNo like '" + prefix + "%' order by LoadSheetNo";
            IDataReader rsSO = DB.GetRS(SOSql);
            while (rsSO.Read())
            {
                OBDList.Add(String.Format("{0},{1}", DB.RSField(rsSO, "LoadSheetNo"), DB.RSFieldInt(rsSO, "LoadSheetId")));

            }
            rsSO.Close();

            return OBDList.ToArray();
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String[] LoadPoNumberForGRN(String InboundID)
        {
            List<String> POList = new List<String>();

            String POSql = "select DISTINCT  POH.PONumber,POH.POHeaderID from INB_Inbound_ORD_SupplierInvoice SIV JOIN ORD_POHeader POH ON POH.POHeaderID=SIV.POHeaderID WHERE SIV.InboundID=" + InboundID;
            IDataReader rsPO = DB.GetRS(POSql);
            while (rsPO.Read())
            {
                POList.Add(String.Format("{0},{1}", rsPO["PONumber"], rsPO["POHeaderID"]));

            }
            rsPO.Close();

            return POList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadWarehouseBasedonUser(string prefix)
        {

            List<string> WHList = new List<string>();

            string WHSql = "Exec [dbo].[USP_MST_DropWH] @AccountID=" + cp.AccountID.ToString() + ",@UserID=" + cp.UserID + ",@Flag =2,@Prefix='" + prefix + "'";

            IDataReader rsWH = DB.GetRS(WHSql);

            while (rsWH.Read())
            {
                WHList.Add(string.Format("{0},{1}", rsWH["WHCode"] + "-" + rsWH["Location"], rsWH["WarehouseID"]));
            }
            rsWH.Close();

            return WHList.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] LoadMDesData(string prefix)
        {
            List<string> MMList = new List<string>();



            string MMSql = "select TOP 100 MaterialMasterID,MDescription from MMT_MaterialMaster WHERE IsActive=1 AND isdeleted=0 AND MDescription like '" + prefix + "%'";

            IDataReader rsMM = DB.GetRS(MMSql);

            while (rsMM.Read())
            {
                MMList.Add(String.Format("{0},{1}", rsMM["MDescription"], rsMM["MaterialMasterID"]));
            }
            rsMM.Close();
            return MMList.ToArray();
        }
    }
}

